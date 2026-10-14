Imports System.IO
Imports System.Text
Imports System.Windows.Forms
Public Class ClsFunction
  Private Shared ReadOnly LogSyncRoot As New Object()
  Private Shared UnhandledMessageVisible As Integer
  Private Shared ReadOnly StyledGrids As New Runtime.CompilerServices.ConditionalWeakTable(Of DataGridView, Object)()

  Public Shared DirName As String
  Public Shared LogDir As String
  Public Shared LogFileName As String

  Public Shared ErrorLogPath As String = ReadSettingIniFile("ERROR_LOG_PATH", "VALUE")
  Public Shared SqlLogPath As String = ReadSettingIniFile("SQL_LOG_PATH", "VALUE")
  Shared Sub New()
    AddHandler System.AppDomain.CurrentDomain.UnhandledException, AddressOf CurrentDomain_UnhandledException
    AddHandler System.Windows.Forms.Application.ThreadException, AddressOf Application_ThreadException
  End Sub

  Public Shared Sub InitializeExceptionLogging()
    ' Sharedコンストラクターをアプリケーション起動時に確実に実行するための入口。
  End Sub

  Public Shared Sub ShowErrorMessage(message As String, Optional owner As IWin32Window = Nothing)
    MessageBox.Show(owner, message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
  End Sub

  Public Shared Sub ShowWarningMessage(message As String, Optional owner As IWin32Window = Nothing)
    MessageBox.Show(owner, message, "確認", MessageBoxButtons.OK, MessageBoxIcon.Warning)
  End Sub

  Public Shared Sub ShowInformationMessage(message As String, Optional owner As IWin32Window = Nothing)
    MessageBox.Show(owner, message, "完了", MessageBoxButtons.OK, MessageBoxIcon.Information)
  End Sub

  Public Shared Sub MarkRequiredLabels(ParamArray labels As Label())
    If labels Is Nothing Then Return
    For Each label As Label In labels
      If label Is Nothing Then Continue For
      label.ForeColor = Drawing.Color.Firebrick
    Next
  End Sub

  Public Shared Sub AlignDetailActionButtons(form As Form,
                                             registerButton As Button,
                                             closeButton As Button)
    If form Is Nothing OrElse registerButton Is Nothing OrElse closeButton Is Nothing Then Return

    Const margin As Integer = 15
    Const gap As Integer = 8
    Dim buttonSize As New Drawing.Size(122, 52)
    registerButton.Size = buttonSize
    closeButton.Size = buttonSize
    closeButton.Location = New Drawing.Point(form.ClientSize.Width - margin - closeButton.Width,
                                              form.ClientSize.Height - margin - closeButton.Height)
    registerButton.Location = New Drawing.Point(closeButton.Left - gap - registerButton.Width,
                                                 closeButton.Top)
    registerButton.Anchor = AnchorStyles.Right Or AnchorStyles.Bottom
    closeButton.Anchor = AnchorStyles.Right Or AnchorStyles.Bottom
  End Sub

  ' 各画面のデータ読込後に、列幅・入力欄・色表示を揃える。
  Public Shared Sub ApplyScreenLayout(form As Form)
    If form Is Nothing Then Return
    form.SuspendLayout()
    Try
      ' フォント変更による再スケーリングで既存の項目配置が崩れないようにする。
      form.AutoScaleMode = AutoScaleMode.None
      form.ControlBox = False
      Dim controls As New List(Of Control)
      CollectScreenControls(form, controls)
      For Each control In controls
        If TypeOf control Is Button OrElse TypeOf control Is Label Then FitControlCaption(control)
      Next
      If form.Name.Contains("Detail") Then
        Dim registerButton = TryCast(controls.FirstOrDefault(Function(c) c.Name = "OkButton"), Button)
        Dim closeButton = TryCast(controls.FirstOrDefault(Function(c) c.Name = "CloseButton"), Button)
        AlignDetailActionButtons(form, registerButton, closeButton)
      End If
      If form.Name = "Form_Top" Then
        Dim title = controls.FirstOrDefault(Function(c) c.Name = "Label1")
        Dim version = controls.FirstOrDefault(Function(c) c.Name = "Label3")
        If title IsNot Nothing AndAlso version IsNot Nothing Then version.Left = Math.Max(405, title.Right + 40)
      End If
      Dim grids = controls.OfType(Of DataGridView)().ToList()
      If grids.Count > 0 Then
        For Each grid In grids
          StyleListColumns(grid)
        Next
      End If

      ' 名称欄はラベルとの余白を確保したうえで、グループ内の残り幅を使う。
      For Each input As TextBox In controls.OfType(Of TextBox)()
        Dim key As String = input.Name.ToLowerInvariant()
        If (key.Contains("name") OrElse key.Contains("memo")) AndAlso
           input.Parent Is form AndAlso form.Name.Contains("Detail") Then
          input.Width = form.ClientSize.Width - input.Left - 15
        End If
        If (key.Contains("name") OrElse key.Contains("memo")) AndAlso
           Not input.Multiline AndAlso TypeOf input.Parent Is GroupBox Then
          Dim labels = input.Parent.Controls.OfType(Of Label)().Where(Function(l) l.Visible).ToList()
          If labels.Count > 0 Then
            Dim left As Integer = labels.Max(Function(l) l.Right) + 20
            For Each field As Control In input.Parent.Controls
              If field.Visible AndAlso (TypeOf field Is TextBox OrElse TypeOf field Is ComboBox) Then
                field.Left = left
                field.Width = input.Parent.ClientSize.Width - left - 15
              End If
            Next
          End If
          input.Width = Math.Max(input.Width, input.Parent.ClientSize.Width - input.Left - 15)
        End If
      Next
    Finally
      form.ResumeLayout(True)
    End Try
  End Sub

  Private Shared Sub FitControlCaption(control As Control)
    If String.IsNullOrWhiteSpace(control.Text) Then Return
    Dim label = TryCast(control, Label)
    If label IsNot Nothing AndAlso label.AutoSize Then Return
    Dim horizontalInset As Integer = If(label Is Nothing, 10, label.Padding.Horizontal)
    Dim verticalInset As Integer = If(label Is Nothing, 6, label.Padding.Vertical)
    Dim available As New Drawing.Size(Math.Max(1, control.ClientSize.Width - horizontalInset),
                                     Math.Max(1, control.ClientSize.Height - verticalInset))
    Dim size As Single = control.Font.Size
    Dim flags = TextFormatFlags.WordBreak Or TextFormatFlags.NoPadding
    While size > 8.0F
      Dim measured = TextRenderer.MeasureText(control.Text, control.Font,
                                              New Drawing.Size(available.Width, Integer.MaxValue), flags)
      If measured.Width <= available.Width AndAlso measured.Height <= available.Height Then Exit While
      size -= 0.5F
      control.Font = New Drawing.Font("Meiryo UI", size, control.Font.Style)
    End While
  End Sub

  Private Shared Sub CollectScreenControls(parent As Control, result As List(Of Control))
    For Each child As Control In parent.Controls
      result.Add(child)
      CollectScreenControls(child, result)
    Next
  End Sub

  Private Shared Sub StyleListColumns(grid As DataGridView)
    ' 行の高さは固定し、列幅の調整は従来の設定を維持する。
    grid.AllowUserToResizeRows = False
    grid.EnableHeadersVisualStyles = False
    With grid.ColumnHeadersDefaultCellStyle
      .BackColor = Drawing.Color.LightBlue
      .ForeColor = Drawing.Color.Black
      .SelectionBackColor = Drawing.Color.LightBlue
      .SelectionForeColor = Drawing.Color.Black
      .Alignment = DataGridViewContentAlignment.MiddleCenter
      If .Font Is Nothing OrElse .Font.Name <> "Meiryo UI" OrElse
         .Font.Size <> 10.0F OrElse .Font.Style <> Drawing.FontStyle.Bold Then
        .Font = New Drawing.Font("Meiryo UI", 10.0F, Drawing.FontStyle.Bold)
      End If
    End With
    grid.TopLeftHeaderCell.Style.BackColor = Drawing.Color.LightBlue
    Dim selectionColor As Drawing.Color = Drawing.Color.FromArgb(66, 102, 132)
    grid.DefaultCellStyle.SelectionBackColor = selectionColor
    grid.DefaultCellStyle.SelectionForeColor = Drawing.Color.White
    grid.RowHeadersDefaultCellStyle.SelectionBackColor = selectionColor
    Dim fillColumn As DataGridViewColumn = Nothing
    For Each column As DataGridViewColumn In grid.Columns
      ' 列自体と値は残し、帳票・更新処理で使う列番号を変えない。
      If IsAuditDateColumn(column) Then column.Visible = False
      column.HeaderCell.Style.BackColor = Drawing.Color.LightBlue
      column.HeaderCell.Style.SelectionBackColor = Drawing.Color.LightBlue
      If Not column.Visible Then Continue For
      Dim header As String = column.HeaderText
      If header.Contains("日付") OrElse header.Contains("時刻") OrElse header.Contains("日時") Then
        column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
      ElseIf (header.Contains("重量") OrElse header.Contains("限値") OrElse header.Contains("基準値") OrElse
              header = "風袋" OrElse header = "回数" OrElse header = "件数" OrElse
              header.Contains("指示数") OrElse header.Contains("実績数") OrElse header.Contains("数量") OrElse
              header.Contains("個数") OrElse header.Contains("単重")) AndAlso Not header.Contains("単位") Then
        column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
      ElseIf header.Contains("品名") OrElse header.Contains("名称") OrElse header.Contains("IP_ADDRESS") OrElse header = "備考" Then
        column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
        fillColumn = column
      ElseIf header.EndsWith("名", StringComparison.Ordinal) Then
        column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
        If fillColumn Is Nothing Then fillColumn = column
      End If
    Next
    If fillColumn IsNot Nothing Then
      Dim owner = grid.FindForm()
      If owner IsNot Nothing AndAlso owner.Name = "Form_ItemList" AndAlso fillColumn.HeaderText = "品名" Then
        ' 品名を残り幅に収め、目標個数まで横スクロールなしで表示する。
        fillColumn.MinimumWidth = 120
      Else
        fillColumn.MinimumWidth = Math.Max(120, Math.Min(fillColumn.Width, 250))
      End If
      fillColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
      fillColumn.FillWeight = 100
    End If
    Dim marker As Object = Nothing
    If StyledGrids.TryGetValue(grid, marker) Then Return
    StyledGrids.Add(grid, New Object())
    AddHandler grid.DataBindingComplete, Sub(sender, e) StyleListColumns(grid)
    AddHandler grid.CellValidating, Sub(sender, e)
                                     If e.RowIndex < 0 OrElse e.ColumnIndex < 0 Then Return
                                     Dim cell = grid.Rows(e.RowIndex).Cells(e.ColumnIndex)
                                     If e.Cancel Then
                                       cell.Style.BackColor = Drawing.Color.MistyRose
                                       cell.Style.SelectionBackColor = Drawing.Color.Maroon
                                     Else
                                       cell.Style.BackColor = Drawing.Color.Empty
                                       cell.Style.SelectionBackColor = Drawing.Color.Empty
                                     End If
                                   End Sub
    AddHandler grid.CellEndEdit, Sub(sender, e)
                                  If e.RowIndex < 0 OrElse e.ColumnIndex < 0 Then Return
                                  Dim cell = grid.Rows(e.RowIndex).Cells(e.ColumnIndex)
                                  cell.Style.BackColor = Drawing.Color.Empty
                                  cell.Style.SelectionBackColor = Drawing.Color.Empty
                                End Sub
  End Sub

  Private Shared Function IsAuditDateColumn(column As DataGridViewColumn) As Boolean
    If column.HeaderText = "登録日" OrElse column.HeaderText = "更新日" OrElse column.HeaderText = "作成日" Then Return True
    For Each key In New String() {column.Name, column.DataPropertyName}
      Select Case key.Replace("_", "").Replace(" ", "").ToLowerInvariant()
        Case "createdate", "updatedate"
          Return True
      End Select
    Next
    Return False
  End Function

  Public Shared Sub FocusInputError(input As Control)
    If input Is Nothing Then Return
    Dim originalColor = input.BackColor
    input.BackColor = Drawing.Color.MistyRose
    Dim restore As EventHandler = Nothing
    restore = Sub(sender, e)
                input.BackColor = originalColor
                RemoveHandler input.TextChanged, restore
              End Sub
    AddHandler input.TextChanged, restore
    input.Focus()
  End Sub

  Public Shared Function DateTypeCheck(Day As String) As Boolean
    If String.IsNullOrWhiteSpace(Day) Then Return False

    Dim CheckResult As Boolean
    Dim yearPart As String = String.Empty
    Dim monthPart As String = String.Empty
    Dim dayPart As String = String.Empty

    Select Case Day.Length
      Case 6
        yearPart = Day.Substring(0, 2)
        monthPart = Day.Substring(2, 2)
        dayPart = Day.Substring(4, 2)
      Case 8
        yearPart = Day.Substring(0, 4)
        monthPart = Day.Substring(4, 2)
        dayPart = Day.Substring(6, 2)
    End Select

    If Not IsValidDate(yearPart, monthPart, dayPart) Then
      CheckResult = False
    Else
      CheckResult = True
    End If

    Return CheckResult
  End Function

  Public Shared Function IsValidDate(year As String, month As String, day As String) As Boolean
    If year Is Nothing OrElse month Is Nothing OrElse day Is Nothing Then Return False

    Dim dt As Date
    If year.Length = 2 Then
      year = "20" & year
    End If

    If Date.TryParseExact($"{year}-{month}-{day}", "yyyy-MM-dd", Nothing, Globalization.DateTimeStyles.None, dt) Then
      Return True
    Else
      Return False
    End If
  End Function

  Public Shared Function DateTxt2DateTxt(Day As String) As String
    If String.IsNullOrWhiteSpace(Day) Then Return String.Empty

    Dim formattedDate As String = String.Empty
    Dim yearPart As String
    Dim monthPart As String
    Dim dayPart As String

    Select Case Day.Length
      Case 6
        yearPart = Day.Substring(0, 2)
        monthPart = Day.Substring(2, 2)
        dayPart = Day.Substring(4, 2)
        formattedDate = $"20{yearPart}/{monthPart}/{dayPart}"
      Case 8
        yearPart = Day.Substring(0, 4)
        monthPart = Day.Substring(4, 2)
        dayPart = Day.Substring(6, 2)
        formattedDate = $"{yearPart}/{monthPart}/{dayPart}"
    End Select

    Return formattedDate
  End Function

  Public Shared Function ComGetProcDay() As String
    Return Date.Parse(ComGetProcTime()).ToString("dd")
  End Function

  Public Shared Function ComGetProcYear() As String
    Return Date.Parse(ComGetProcTime()).ToString("yyyy")
  End Function

  Public Shared Function ComGetProcYearMonth() As String
    Return Date.Parse(ComGetProcTime()).ToString("yyyy/MM")
  End Function

  'エラーログファイルを出力(文字列出力)
  Public Shared Sub ComWriteErrLog(ByVal strMsg As Exception,
                                   Optional ByVal filePath As String = "")
    If strMsg Is Nothing Then Return
    If String.IsNullOrWhiteSpace(filePath) Then filePath = GetErrorLogPath()
    Call ComWriteLog(FormatErrorLog("Unhandled", "Unknown", strMsg.ToString()), filePath)
  End Sub

  Public Shared Function ComGetProcTime() As String
    Dim dtNow As Date = Date.Now
    Return dtNow
  End Function

  Public Shared Sub ComWriteLog(ByVal desc As String, ByVal filePath As String)
    Dim targetPath As String = If(filePath, String.Empty)

    Try
      targetPath = ResolveLogFilePath(targetPath, "application.log")
      WriteLogCore(desc, targetPath)
    Catch
      ' 設定先へ書き込めない場合でも、実行フォルダー配下へログを残す。
      Dim fallbackFileName As String = "application.log"
      Try
        Dim configuredFileName As String = System.IO.Path.GetFileName(targetPath)
        If Not String.IsNullOrWhiteSpace(configuredFileName) Then
          fallbackFileName = configuredFileName
        End If
      Catch
      End Try
      Dim fallbackPath As String = System.IO.Path.Combine(
        System.AppDomain.CurrentDomain.BaseDirectory, "Log", fallbackFileName)
      Try
        WriteLogCore(desc, fallbackPath)
      Catch
        ' ログ障害が本来の業務処理を停止させないよう、ここでは例外を送出しない。
      End Try
    End Try
  End Sub

  Public Shared Sub WriteDetail(dt As DataTable, dgView As DataGridView,
  Optional ByVal columnCtl As Boolean = False)
    dgView.Rows.Clear()
    Try
      For i As Integer = 0 To dt.Rows.Count - 1
        dgView.Rows.Add()
        For j = 0 To dt.Columns.Count - 1
          'フィールドの取得

          'Dim field = dt.Rows(i).Item(j).ToString()

          Dim field As Object = dt.Rows(i)(j)

          If field Is DBNull.Value Then
            If columnCtl Then
              dgView.Rows(i).Cells(j + 1).Value = String.Empty
            Else
              dgView.Rows(i).Cells(j).Value = String.Empty
            End If
          Else
            If columnCtl Then
              dgView.Rows(i).Cells(j + 1).Value = field
            Else
              dgView.Rows(i).Cells(j).Value = field
            End If
          End If
        Next
      Next
      '書き込むファイルを開く
    Catch ex As Exception
      ComWriteErrLog(NameOf(ClsFunction), NameOf(WriteDetail), ex)
      MsgBox(ex.Message)
    End Try
  End Sub

  Public Shared Sub OutLog(msg As String, dataKind As String)
    Try
      Dim path As String = System.IO.Path.Combine(
        If(DirName, String.Empty), If(LogDir, String.Empty), If(LogFileName, "application.log"))
      ComWriteLog(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff") & " " & dataKind & "：" & msg, path)
    Catch ex As Exception
      ComWriteErrLog(NameOf(ClsFunction), NameOf(OutLog), ex)
    End Try

  End Sub

  Public Shared Function CountChar(ByVal s As String, ByVal c As Char) As String
    If s Is Nothing Then Return "0"
    Return s.Length - s.Replace(c.ToString(), "").Length
  End Function

  ''' <summary>SQL文字列リテラル内のシングルクォートをエスケープする。</summary>
  Public Shared Function EscapeSqlLiteral(ByVal value As String) As String
    If value Is Nothing Then Return String.Empty
    Return value.Replace("'", "''")
  End Function

  ''' <summary>空でなく、0～9だけで構成されているかを判定する。</summary>
  Public Shared Function IsDigitsOnly(ByVal value As String) As Boolean
    If String.IsNullOrWhiteSpace(value) Then Return False
    For Each character As Char In value
      If Not Char.IsDigit(character) Then Return False
    Next
    Return True
  End Function

  Public Shared Sub ClearTextBox(ByVal hParent As Control)
    For Each cControl As Control In hParent.Controls
      If cControl.HasChildren Then
        ClearTextBox(cControl)
      End If
      If TypeOf cControl Is TextBoxBase Then
        cControl.Text = String.Empty
      End If
    Next cControl
  End Sub

  ' ------------------------------------------------------
  ' ini ファイル読み込み
  ' ------------------------------------------------------
  '指定のIniファイルの指定キーの値を取得(文字列)
  ' AUTO版 GetPrivateProfileString
  Private Declare Auto Function GetPrivateProfileString Lib "kernel32" _
      (ByVal lpAppName As String,
          ByVal lpKeyName As String,
              ByVal lpDefault As String,
                  ByVal lpReturnedString As StringBuilder,
                      ByVal nSize As Integer,
                          ByVal lpFileName As String) As Integer

  'プロファイル文字列書込み  
  Private Declare Function WritePrivateProfileString Lib "kernel32" _
      Alias "WritePrivateProfileStringA" _
      (ByVal lpApplicationName As String, ByVal lpKeyName As String,
       ByVal lpString As String, ByVal lpFileName As String) As Long

  ''' <summary>
  ''' iniファイルから取得する
  ''' </summary>
  ''' <param name="lpAppName"></param>
  ''' <param name="lpKeyName"></param>
  ''' <param name="strPath"></param>
  ''' <returns></returns>
  Public Shared Function GetIniString(ByVal lpAppName As String, ByVal lpKeyName As String, strPath As String) As String
    Dim sb As New StringBuilder(256)
    Try
      Call GetPrivateProfileString(lpAppName, lpKeyName, "", sb, Convert.ToUInt32(sb.Capacity), strPath)
      Return sb.ToString
    Catch ex As Exception
      Return sb.ToString
    End Try
  End Function

  ''' <summary>
  ''' iniファイルに書き込む
  ''' </summary>
  ''' <param name="lpAppName"></param>
  ''' <param name="lpKeyName"></param>
  ''' <param name="lpValue"></param>
  ''' <param name="strPath"></param>
  ''' <returns></returns>
  Public Shared Function PutIniString(ByVal lpAppName As String, lpKeyName As String, ByVal lpValue As String, ByVal strPath As String) As Boolean
    Try
      Dim result As Long = WritePrivateProfileString(lpAppName, lpKeyName, lpValue, strPath)
      Return result <> 0
    Catch ex As Exception
      Return False
    End Try
  End Function

  Public Shared Sub OpenForm(StrKey As String)
    Dim exePath As String = ReadMenuIniFile(StrKey)
    If String.IsNullOrWhiteSpace(exePath) Then
      Throw New InvalidOperationException("メニュー設定に実行ファイルが指定されていません。キー: " & StrKey)
    End If
    Dim psi As New ProcessStartInfo(exePath)
    System.Diagnostics.Process.Start(psi)
  End Sub
  Public Shared Function ReadMenuIniFile(strKey As String)
    Dim strPath As String = "C:\COUNTING_DX\INI\menu.ini"
    Dim exeName As String = GetIniString(strKey, "EXE", strPath)
    Return exeName
  End Function

  Public Shared Function ReadSettingIniFile(strKey As String, keyName As String)
    Dim strPath As String = "C:\COUNTING_DX\INI\setting.ini"
    Dim stringValue As String = GetIniString(strKey, keyName, strPath)
    Return stringValue
  End Function

  ''' <summary>
  ''' 計量器マスタの号機番号を、計量器のファイル桁数設定に合わせて整形する。
  ''' FILENAME_DIGITS が空の場合はゼロ埋めなし、"00" の場合は2桁で返す。
  ''' </summary>
  Public Shared Function FormatUnitNumberForFileName(unitNumber As String, fileNameDigits As String) As String
    Dim number As Integer
    If Not Integer.TryParse(unitNumber, number) OrElse number < 1 OrElse number > 99 Then
      Throw New ArgumentException("ファイル名に使用する号機番号が不正です: " & unitNumber, NameOf(unitNumber))
    End If

    Dim numberFormat As String = If(String.IsNullOrWhiteSpace(fileNameDigits), "0", fileNameDigits.Trim())
    For Each character As Char In numberFormat
      If character <> "0"c Then
        Throw New ArgumentException("FILENAME_DIGITS には空白または0だけを設定してください: " & fileNameDigits,
                                    NameOf(fileNameDigits))
      End If
    Next

    Return number.ToString(numberFormat, System.Globalization.CultureInfo.InvariantCulture)
  End Function

  Public Shared Function ReadConnectIniFile(strKey As String, keyName As String)
    Dim strPath As String = "C:\COUNTING_DX\INI\connect.ini"
    Dim stringValue As String = GetIniString(strKey, keyName, strPath)
    Return stringValue
  End Function

  'エラーログファイルを出力
  Public Shared Sub ComWriteErrLog(ByVal prmSource As String,
                                 ByVal prmTargetSite As String,
                                 ByVal prmMessage As String)
    Dim callerStack As String = New System.Diagnostics.StackTrace(1, True).ToString()
    Call ComWriteLog(FormatErrorLog(prmSource, prmTargetSite,
                                   prmMessage & Environment.NewLine & callerStack),
                     GetErrorLogPath())
  End Sub

  ''' <summary>
  ''' 例外の型、内部例外、スタックトレースを含めてエラーログへ出力する。
  ''' </summary>
  Public Shared Sub ComWriteErrLog(ByVal prmSource As String,
                                   ByVal prmTargetSite As String,
                                   ByVal ex As Exception)
    If ex Is Nothing Then Return
    Call ComWriteLog(FormatErrorLog(prmSource, prmTargetSite, ex.ToString()), GetErrorLogPath())
  End Sub
  ''' <summary>
  ''' SQL実行ログ保存
  ''' </summary>
  ''' <param name="prmSql">SQL文</param>
  Public Shared Sub WriteExecuteLog(ByVal prmSource As String, ByVal prmTargetSite As String, prmSql As String)
    Try
      Dim tmpProcTime As String = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff")
      Dim tmpExeFileName As String = System.IO.Path.GetFileName(System.Windows.Forms.Application.ExecutablePath)
      Dim logText As String = "【Date】" & tmpProcTime _
                            & "【PC】" & System.Net.Dns.GetHostName() _
                            & "【Exe】" & tmpExeFileName _
                            & "【Source】" & prmSource _
                            & "【Method】" & prmTargetSite _
                            & "【Sql】" & prmSql
      ComWriteLog(logText, GetSqlLogPath())
    Catch
      ' SQLログ障害で業務SQLを失敗させない。
    End Try
  End Sub

  Private Shared Function FormatErrorLog(source As String, targetSite As String, description As String) As String
    Dim exeFileName As String = System.IO.Path.GetFileName(System.Windows.Forms.Application.ExecutablePath)
    Return "【Date】" & DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff") _
         & "【PC】" & System.Environment.MachineName _
         & "【Exe】" & exeFileName _
         & "【Source】" & If(source, String.Empty) _
         & "【Method】" & If(targetSite, String.Empty) _
         & "【Description】" & If(description, String.Empty)
  End Function

  Private Shared Function GetErrorLogPath() As String
    Return ResolveLogFilePath(ErrorLogPath,
                              System.IO.Path.Combine("Log", "error_" & DateTime.Now.ToString("yyyyMMdd") & ".log"))
  End Function

  Private Shared Function GetSqlLogPath() As String
    Return ResolveLogFilePath(SqlLogPath,
                              System.IO.Path.Combine("Log", "sql_" & DateTime.Now.ToString("yyyyMMdd") & ".log"))
  End Function

  Private Shared Function ResolveLogFilePath(configuredPath As String, defaultRelativePath As String) As String
    If String.IsNullOrWhiteSpace(configuredPath) Then
      Return System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, defaultRelativePath)
    End If

    Dim trimmedPath As String = configuredPath.Trim()
    If trimmedPath.EndsWith(System.IO.Path.DirectorySeparatorChar.ToString()) _
       OrElse trimmedPath.EndsWith(System.IO.Path.AltDirectorySeparatorChar.ToString()) _
       OrElse System.IO.Directory.Exists(trimmedPath) Then
      Return System.IO.Path.Combine(trimmedPath, System.IO.Path.GetFileName(defaultRelativePath))
    End If
    Return trimmedPath
  End Function

  Private Shared Sub WriteLogCore(desc As String, filePath As String)
    Dim parentDirectory As String = System.IO.Path.GetDirectoryName(filePath)
    If Not String.IsNullOrWhiteSpace(parentDirectory) Then
      System.IO.Directory.CreateDirectory(parentDirectory)
    End If

    SyncLock LogSyncRoot
      Using writer As New System.IO.StreamWriter(filePath, True, New UTF8Encoding(False))
        writer.WriteLine(If(desc, String.Empty))
      End Using
    End SyncLock
  End Sub

  Private Shared Sub Application_ThreadException(sender As Object,
                                                 e As System.Threading.ThreadExceptionEventArgs)
    ComWriteErrLog("Application", "ThreadException", e.Exception)
    If Threading.Interlocked.CompareExchange(UnhandledMessageVisible, 1, 0) <> 0 Then Return
    Try
      ShowErrorMessage("予期しないエラーが発生しました。" & vbCrLf &
                       "処理を中止し、エラーログをご確認ください。")
    Finally
      Threading.Interlocked.Exchange(UnhandledMessageVisible, 0)
    End Try
  End Sub

  Private Shared Sub CurrentDomain_UnhandledException(sender As Object,
                                                      e As UnhandledExceptionEventArgs)
    Dim ex As Exception = TryCast(e.ExceptionObject, Exception)
    If ex IsNot Nothing Then
      ComWriteErrLog("AppDomain", "UnhandledException", ex)
    Else
      ComWriteErrLog("AppDomain", "UnhandledException",
                     Convert.ToString(e.ExceptionObject, Globalization.CultureInfo.InvariantCulture))
    End If
  End Sub
End Class
