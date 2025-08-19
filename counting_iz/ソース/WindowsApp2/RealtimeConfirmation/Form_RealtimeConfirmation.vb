Imports System.IO
Imports System.IO.SearchOption
Imports System.Text
Imports Common
Imports Common.ClsFunction
Imports Microsoft.VisualBasic.FileIO

Public Class Form_RealtimeConfirmation
  Dim CheckboxExistFlg As New Boolean
  Dim Concat_ScaleNumber As String = String.Empty
  Dim PathName As String
  Dim TableName As String
  Dim DefText As String

  Private ReadOnly FileNameDigits As String = ReadSettingIniFile("FILENAME_DIGITS", "VALUE")
  Private ReadOnly FtpBackupPath As String = ReadSettingIniFile("FTP_BACKUP_PATH", "VALUE")

  Private ReadOnly tmpDb As New ClsSqlServer
  Dim tmpDt As New DataTable
  ' SQLサーバー操作オブジェクト
  Private _SqlServer As ClsSqlServer
  Private ReadOnly Property SqlServer As ClsSqlServer
    Get
      If _SqlServer Is Nothing Then
        _SqlServer = New ClsSqlServer
      End If
      Return _SqlServer
    End Get
  End Property
  Private Sub Form_RealtimeConfirmation_Load(sender As Object, e As EventArgs) Handles MyBase.Load
    'ユーザーからのデータ追加を不可にしておく
    MaximizeBox = False
    CheckboxExistFlg = True
    Dim updateTime As DateTime = System.IO.File.GetLastWriteTime(System.Reflection.Assembly.GetExecutingAssembly().Location)
    Text = "通信結果" & " ( " & updateTime & " ) "
    Me.KeyPreview = True
    ResultDetail.RowHeadersVisible = False
    ResultDetail.AllowUserToAddRows = False
    MaximizeBox = False
    FormBorderStyle = FormBorderStyle.FixedSingle
    Dim checkBox_Trans As New DataGridViewCheckBoxColumn
    ResultDetail.Columns.Add(checkBox_Trans)
    checkBox_Trans.Width = 65

    ResultDetail.ColumnCount = 6

    ResultDetail.Columns(0).HeaderText = "選択"
    ResultDetail.Columns(1).HeaderText = "号機No"
    ResultDetail.Columns(2).HeaderText = "実績受信"
    ResultDetail.Columns(3).HeaderText = "結果"
    ResultDetail.Columns(4).HeaderText = "マスタ送信"
    ResultDetail.Columns(5).HeaderText = "結果"
    'カラムの幅指定
    ResultDetail.Columns(1).Width = 100
    ResultDetail.Columns(2).Width = 160
    ResultDetail.Columns(3).Width = 90
    ResultDetail.Columns(4).Width = 160
    ResultDetail.Columns(5).Width = 90

    'ヘッダーの整列設定
    For i As Integer = 0 To 5
      ResultDetail.Columns(i).DefaultCellStyle.Alignment =
   DataGridViewContentAlignment.MiddleCenter
      ResultDetail.Columns(i).HeaderCell.Style.Alignment =
  DataGridViewContentAlignment.MiddleCenter
    Next

    '選択モード設定(全カラム)
    ResultDetail.SelectionMode = DataGridViewSelectionMode.FullRowSelect

    chkSendFreeMaster.Checked = False
    '照会メソッド呼出し
    SelectFtpResult()

    CustomizeDataGridViewHeader() ' ヘッダーのデザイン変更

  End Sub
  ' DataGridView のヘッダーのデザインを変更
  Private Sub CustomizeDataGridViewHeader()
    With ResultDetail
      ' ヘッダーの背景色を変更
      .EnableHeadersVisualStyles = False ' デフォルトの Windows スタイルを無効化
      .ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue ' ヘッダーの背景色
      .ColumnHeadersDefaultCellStyle.ForeColor = Color.Black ' ヘッダーの文字色
      .ColumnHeadersDefaultCellStyle.Font = New Font("Meiryo", 10, FontStyle.Bold) ' フォント変更
      .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter ' ヘッダー中央寄せ
    End With
  End Sub
  Private Sub ResultDetail_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles ResultDetail.CellClick
    If e.RowIndex >= 0 Then
      ' 左端（0セル目）がクリックされた時のみ
      If e.ColumnIndex = 0 Then
        ' 未チェック状態ならチェックに、逆なら未チェックに
        If CBool(ResultDetail.CurrentRow.Cells(0).Value) Then
          ResultDetail.CurrentRow.Cells(0).Value = False
        Else
          ResultDetail.CurrentRow.Cells(0).Value = True
        End If
      End If
    End If
  End Sub
  Private Sub SendButton_Click(sender As Object, e As EventArgs) Handles SendButton.Click
    If CheckSelectRow() = False Then
      MessageBox.Show("行を選択して下さい。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
    Else
      CallProcess("Upload", SetConcat_ScaleNumber)
    End If
  End Sub
  Private Sub CloseButton_Click(sender As Object, e As EventArgs) Handles CloseButton.Click
    Close()
  End Sub
  Private Function DuplicateCheck(PR_PROCESS_DATE As String, PR_PROCESS_TIME As String, PR_MACHINE_NO As String)
    Dim Result As Boolean
    Dim sql As String = String.Empty
    sql = GetDuplicateSelectSql(PR_PROCESS_DATE, PR_PROCESS_TIME, PR_MACHINE_NO)
    Try
      With tmpDb
        SqlServer.GetResult(tmpDt, sql)
        If tmpDt.Rows(0).Item("ROWC") = 0 Then
          Result = False
        Else
          Result = True
        End If
      End With
    Catch ex As Exception
      Call ComWriteErrLog([GetType]().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message)
      Throw New Exception(ex.Message)
    Finally
      tmpDt.Dispose()
    End Try
    Return Result
  End Function

  Private Function GetDuplicateSelectSql(PR_PROCESS_DATE As String, PR_PROCESS_TIME As String, PR_MACHINE_NO As String)
    Dim sql As String = String.Empty

    sql &= " Select"
    sql &= "     count(*) As ROWC"
    sql &= " From"
    sql &= "     TRN_Results"
    sql &= " Where"
    sql &= "     terminal_number = '" & PR_MACHINE_NO & "' "
    sql &= " and addition_date = '" & PR_PROCESS_DATE & "' "
    sql &= " and addition_time = '" & PR_PROCESS_TIME & "' "
    Call WriteExecuteLog([GetType]().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, sql)
    Return sql
  End Function

  Private Sub USB_SendButton_Click(sender As Object, e As EventArgs) Handles USB_SendButton.Click
    Dim ScaleNumber As String
    Dim UsbPath As String = GetUsbDriveRootPath() ' USBメモリのパスを取得
    Dim isFreeMaster As Boolean = chkSendFreeMaster.Checked

    If String.IsNullOrEmpty(UsbPath) Then
      MessageBox.Show("USBメモリが見つかりません。USBメモリを挿入してください。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
      Exit Sub
    End If

    ' 保存先フォルダを設定
    UsbPath = Path.Combine(UsbPath, "IM-7000\TEMP")

    ' フォルダが存在しない場合は作成
    If Not Directory.Exists(UsbPath) Then
      Directory.CreateDirectory(UsbPath)
    End If

    If ResultDetail.Rows.Count > 0 Then

      For i As Integer = 0 To ResultDetail.Rows.Count - 1
        ScaleNumber = ResultDetail.Rows(i).Cells(1).Value

        ' ファイルの保存処理
        If isFreeMaster Then
          CreateItemMasterCSV(ScaleNumber, UsbPath)
          CreateManufacturerMasterCSV(ScaleNumber, UsbPath)
          CreatePackingMasterCSV(ScaleNumber, UsbPath)
          CreateStaffMasterCSV(ScaleNumber, UsbPath)
          CreateFree1MasterCSV(ScaleNumber, UsbPath)
          CreateFree2MasterCSV(ScaleNumber, UsbPath)
          CreateFree3MasterCSV(ScaleNumber, UsbPath)
          CreateFree4MasterCSV(ScaleNumber, UsbPath)
          CreateFree5MasterCSV(ScaleNumber, UsbPath)
        Else
          CreateItemMasterCSV(ScaleNumber, UsbPath)
          CreateManufacturerMasterCSV(ScaleNumber, UsbPath)
          CreatePackingMasterCSV(ScaleNumber, UsbPath)
          CreateStaffMasterCSV(ScaleNumber, UsbPath)
        End If
      Next

      ' エクスプローラーでフォルダを開く
      Try
        Process.Start("explorer.exe", UsbPath)
        MessageBox.Show("データを保存しました。" & vbCrLf & "保存先: " & UsbPath, "完了", MessageBoxButtons.OK, MessageBoxIcon.Information)
      Catch ex As Exception
        MessageBox.Show("フォルダを開く際にエラーが発生しました。")
      End Try
    End If

  End Sub

  ''' <summary>
  ''' USBメモリのドライブを探し、そのパスを返す
  ''' </summary>
  ''' <returns>USBメモリのパス (例: "E:\")、見つからなければ空文字</returns>
  Private Function GetUsbDriveRootPath() As String
    For Each drive As DriveInfo In DriveInfo.GetDrives()
      If drive.DriveType = DriveType.Removable AndAlso drive.IsReady Then
        Return drive.RootDirectory.FullName ' USBが見つかったらルートパスを返す
      End If
    Next
    Return String.Empty ' USBが見つからない場合
  End Function

  Public Sub SelectFtpResult()
    Dim sql As String = String.Empty
    sql = GetResultSelectSql()
    Try
      With tmpDb
        SqlServer.GetResult(tmpDt, sql)
        If tmpDt.Rows.Count = 0 Then
          MessageBox.Show("計量器マスタにデータが登録されていません。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Else
          WriteDetail(tmpDt, ResultDetail, CheckboxExistFlg)
          SetAutomaticCheck()
        End If
      End With
    Catch ex As Exception
      Call ComWriteErrLog([GetType]().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message)
      Throw New Exception(ex.Message)
    Finally
      tmpDt.Dispose()
    End Try
  End Sub
  Public Sub SetAutomaticCheck()
    For i As Integer = 0 To ResultDetail.Rows.Count - 1
      If ResultDetail.Rows(i).Cells(3).Value = "NG" Then
        ResultDetail(3, i).Style.BackColor = Color.DarkRed
        ResultDetail(3, i).Style.SelectionBackColor = Color.DarkRed
      End If

      If ResultDetail.Rows(i).Cells(5).Value = "NG" Then
        ResultDetail(5, i).Style.BackColor = Color.DarkRed
        ResultDetail(5, i).Style.SelectionBackColor = Color.DarkRed
      End If

      ResultDetail.Rows(i).Cells(0).Value = True
    Next
  End Sub
  Private Function GetResultSelectSql() As String

    Dim sql As String = String.Empty

    sql &= " WITH AC_TRN_LOG AS("
    sql &= "     SELECT"
    sql &= "         MACHINE_NO,"
    sql &= "         PROCESS_DATE,"
    sql &= "         ACHIEVEMENT_RECEIVE_TIME,"
    sql &= "         ACHIEVEMENT_RESULT"
    sql &= "     FROM"
    sql &= "         TRN_LOG A"
    sql &= "     WHERE"
    sql &= "         ACHIEVEMENT_RECEIVE_TIME = ("
    sql &= "             SELECT"
    sql &= "                 MAX(ACHIEVEMENT_RECEIVE_TIME)"
    sql &= "             FROM"
    sql &= "                 TRN_LOG AS B"
    sql &= "             WHERE"
    sql &= "                 A.MACHINE_NO = B.MACHINE_NO"
    sql &= "             And B.ACHIEVEMENT_RESULT In ('OK', 'NG') "
    sql &= "         )"
    sql &= " 		AND ACHIEVEMENT_RESULT IN ('OK', 'NG') "
    sql &= " ),"
    sql &= " MST_TRN_LOG As("
    sql &= "     SELECT"
    sql &= "         MACHINE_NO,"
    sql &= "         PROCESS_DATE,"
    sql &= "         MASTER_SEND_TIME,"
    sql &= "         MASTER_RESULT"
    sql &= "     FROM"
    sql &= "         TRN_LOG A"
    sql &= "     WHERE"
    sql &= "         MASTER_SEND_TIME = ("
    sql &= "             SELECT"
    sql &= "                 MAX(MASTER_SEND_TIME)"
    sql &= "             FROM"
    sql &= "                 TRN_LOG AS B"
    sql &= "             WHERE"
    sql &= "                 A.MACHINE_NO = B.MACHINE_NO"
    sql &= "             AND B.MASTER_RESULT IN ('OK', 'NG') "
    sql &= "         )"
    sql &= " 		AND MASTER_RESULT IN ('OK', 'NG') "
    sql &= " )"
    sql &= " SELECT"
    sql &= "     UNIT_NUMBER As 号機番号,"
    sql &= "     SUBSTRING(ACHIEVEMENT_RECEIVE_TIME, 1, 19) As 実績受信日時,"
    sql &= "     ACHIEVEMENT_RESULT AS 実績受信結果,"
    sql &= "     SUBSTRING(MASTER_SEND_TIME, 1, 19) As マスタ送信日時,"
    sql &= "     MASTER_RESULT As マスタ送信結果"
    sql &= " FROM"
    sql &= "     MST_SCALE"
    sql &= "     LEFT JOIN"
    sql &= "         AC_TRN_LOG"
    sql &= "     ON  MST_SCALE.UNIT_NUMBER = AC_TRN_LOG.MACHINE_NO"
    sql &= "     LEFT JOIN"
    sql &= "         MST_TRN_LOG"
    sql &= "     ON  MST_SCALE.UNIT_NUMBER = MST_TRN_LOG.MACHINE_NO"
    sql &= " ORDER BY"
    sql &= "     MST_SCALE.UNIT_NUMBER"
    Call WriteExecuteLog([GetType]().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, sql)
    Return sql
  End Function
  Private Sub ReceiveButton_Click(sender As Object, e As EventArgs) Handles ReceiveButton.Click
    If CheckSelectRow() = False Then
      MessageBox.Show("行を選択して下さい。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
    Else
      CallProcess("DownLoad", SetConcat_ScaleNumber)
    End If
  End Sub
  Function CheckSelectRow() As Boolean
    Dim result As Boolean = False
    For i As Integer = 0 To ResultDetail.Rows.Count - 1
      If ResultDetail.Rows(i).Cells(0).Value = True Then
        result = True
        Exit For
      End If
    Next
    Return result
  End Function

  Function SetConcat_ScaleNumber() As String
    Dim InitialCheckFlg = True
    For i As Integer = 0 To ResultDetail.Rows.Count - 1
      If ResultDetail.Rows(i).Cells(0).Value = True Then
        If InitialCheckFlg Then
          Concat_ScaleNumber = ResultDetail.Rows(i).Cells(1).Value
          InitialCheckFlg = False
        Else
          Concat_ScaleNumber = Concat_ScaleNumber + " " + ResultDetail.Rows(i).Cells(1).Value
        End If
      End If
    Next
    Return Concat_ScaleNumber
  End Function
  Private Sub CallProcess(ProcessMode As String, Concat_ScaleNumber As String)

    Dim DownloadPath As String = ReadSettingIniFile("DOWNLOAD_PATH", "VALUE")
    Dim UploadPath As String = ReadSettingIniFile("UPLOAD_PATH", "VALUE")
    Dim isFreeMaster As Boolean = chkSendFreeMaster.Checked

    Select Case ProcessMode
      Case "DownLoad"
        Dim DownloadExe As New ProcessStartInfo With {
        .FileName = DownloadPath,
        .Arguments = Concat_ScaleNumber,
            .UseShellExecute = False
        }
        Dim p As System.Diagnostics.Process = System.Diagnostics.Process.Start(DownloadExe)
        p.WaitForExit()
        MessageBox.Show("実績受信終了しました。" & vbCrLf & "処理結果をご確認下さい。", "確認", MessageBoxButtons.OK, MessageBoxIcon.Information)
        SelectFtpResult()
      Case "Upload"
        ' isFreeMaster を引数として追加
        ' 🟠 スケール番号と isFreeMaster を 「|」で区切る
        Dim arguments As String = $"{Concat_ScaleNumber}|{isFreeMaster.ToString().ToLower()}"

        Dim UploadExe As New ProcessStartInfo With {
            .FileName = UploadPath,
           .Arguments = arguments,
           .UseShellExecute = False
        }
        'ファイルを開いて終了まで待機する
        Dim p As System.Diagnostics.Process = System.Diagnostics.Process.Start(UploadExe)
        p.WaitForExit()
        MessageBox.Show("マスタ送信終了しました。" & vbCrLf & "処理結果をご確認下さい。", "確認", MessageBoxButtons.OK, MessageBoxIcon.Information)
        SelectFtpResult()
    End Select
  End Sub
  Function GetInsertSql(columnNames() As String, dr As DataRow) As String
    Dim sql As String = "INSERT INTO TRN_Results ("
    Dim values As String = "VALUES ("

    ' カラム名と値をセット
    For i As Integer = 0 To columnNames.Length - 1

      Dim value As String = dr(i).ToString().Replace("'", "''")

      ' freeX_numberの場合は先頭に000を付与
      If columnNames(i) Like "free?_number" Then
        value = "000" & value
      End If

      sql &= columnNames(i)
      values &= "'" & value & "'"

      If i < columnNames.Length - 1 Then
        sql &= ", "
        values &= ", "
      End If
    Next

    sql &= ") " & values & ")"
    Call WriteExecuteLog("Module_Download", System.Reflection.MethodBase.GetCurrentMethod().Name, sql)
    Return sql
  End Function

  Private Sub CreateItemMasterCSV(ScaleNumber As String, UsbPath As String)
    PathName = "40MAS1"
    TableName = "MST_Item"
    DefText = "呼出コード:40001,品番:40002,単重:40003,単重単位:40004,安全計数:40005,目標個数:40006,風袋:40007,風袋単位:40008,上限値:40009,基準値:40011,下限値:40013,小計目標値:40028,小計目標回数:40030,品名:40031,単重上限値:40201,単重上限値単位:40202,単重下限値:40203,単重下限値単位:40204"
    CreateCsv(PathName, TableName, DefText, ScaleNumber, UsbPath)
  End Sub
  Private Sub CreateManufacturerMasterCSV(ScaleNumber As String, UsbPath As String)
    PathName = "40MAS2"
    TableName = "MST_Manufacturer"
    DefText = "製造者コード:40051,製造者名:40052"
    CreateCsv(PathName, TableName, DefText, ScaleNumber, UsbPath)
  End Sub

  Private Sub CreatePackingMasterCSV(ScaleNumber As String, UsbPath As String)
    PathName = "40TARE"
    TableName = "MST_Packing"
    DefText = "風袋№:40371,風袋重量:40372,風袋重量単位:40373,風袋名称:40374"
    CreateCsv(PathName, TableName, DefText, ScaleNumber, UsbPath)
  End Sub
  Private Sub CreateStaffMasterCSV(ScaleNumber As String, UsbPath As String)
    PathName = "40OPTR"
    TableName = "MST_Staff"
    DefText = "担当者№:40361,担当者名:40362"
    CreateCsv(PathName, TableName, DefText, ScaleNumber, UsbPath)
  End Sub

  Private Sub CreateFree1MasterCSV(ScaleNumber As String, UsbPath As String)
    PathName = "40FRE1"
    TableName = "MST_Free1"
    DefText = "フリー１№:40311,フリー１名称:40312"
    CreateCsv(PathName, TableName, DefText, ScaleNumber, UsbPath)
  End Sub

  Private Sub CreateFree2MasterCSV(ScaleNumber As String, UsbPath As String)
    PathName = "40FRE2"
    TableName = "MST_Free2"
    DefText = "フリー２№:40321,フリー２名称:40322"
    CreateCsv(PathName, TableName, DefText, ScaleNumber, UsbPath)
  End Sub

  Private Sub CreateFree3MasterCSV(ScaleNumber As String, UsbPath As String)
    PathName = "40FRE3"
    TableName = "MST_Free3"
    DefText = "フリー３№:40331,フリー３名称:40332
"
    CreateCsv(PathName, TableName, DefText, ScaleNumber, UsbPath)
  End Sub

  Private Sub CreateFree4MasterCSV(ScaleNumber As String, UsbPath As String)
    PathName = "40FRE4"
    TableName = "MST_Free4"
    DefText = "フリー４№:40341,フリー４名称:40342"
    CreateCsv(PathName, TableName, DefText, ScaleNumber, UsbPath)
  End Sub

  Private Sub CreateFree5MasterCSV(ScaleNumber As String, UsbPath As String)
    PathName = "40FRE5"
    TableName = "MST_Free5"
    DefText = "フリー５№:40351,フリー５名称:40352"
    CreateCsv(PathName, TableName, DefText, ScaleNumber, UsbPath)
  End Sub
  Private Sub CreateCsv(PathName As String, TableName As String, DefText As String, ScaleNumber As String, UsbPath As String)
    Dim CsvPath As String
    Dim DefPath As String
    Dim isWriteHeader As Boolean = True
    Dim sql As String = String.Empty
    Dim OutputDb As New DataTable
    Dim OutputDt As New DataTable

    CsvPath = UsbPath & "\" & PathName & FileNameDigits & ScaleNumber & ".CSV"
    DefPath = UsbPath & "\" & PathName & FileNameDigits & ScaleNumber & ".DEF"
    'CSVファイル出力時に使うEncoding
    '「Shift_JIS」を使用
    Dim encoding As System.Text.Encoding = System.Text.Encoding.GetEncoding("Shift_JIS")
    '書き込むファイルを開く
    Dim wrCsv As New System.IO.StreamWriter(CsvPath, False, encoding)
    Dim wrDef As New System.IO.StreamWriter(DefPath, False, encoding)
    wrDef.Write(DefText)
    wrDef.Close()

    Select Case TableName
      Case "MST_Item"
        sql = GetItemMasterSelectSql()
      Case "MST_Manufacturer"
        sql = GetManufacturerMasterSelectSql()
      Case "MST_Packing"
        sql = GetMSTPackingSelectSql()
      Case "MST_Staff"
        sql = GetStaffMasterSelectSql()
      Case "MST_Free1"
        sql = GetMST_Free1MasterSelectSql()
      Case "MST_Free2"
        sql = GetMST_Free2MasterSelectSql()
      Case "MST_Free3"
        sql = GetMST_Free3MasterSelectSql()
      Case "MST_Free4"
        sql = GetMST_Free4MasterSelectSql()
      Case "MST_Free5"
        sql = GetMST_Free5MasterSelectSql()
    End Select

    Try
      With OutputDb
        SqlServer.GetResult(OutputDt, sql)
        If OutputDt.Rows.Count = 0 Then
          Select Case TableName
            Case "MST_Item"
              MessageBox.Show("商品マスタのデータがありません。", "確認", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Case "MST_Manufacturer"
              MessageBox.Show("製造者マスタのデータがありません。", "確認", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Case "MST_Packing"
              MessageBox.Show("風袋マスタのデータがありません。", "確認", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Case "MST_Staff"
              MessageBox.Show("担当者マスタのデータがありません。", "確認", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Case "MST_Free1"
              MessageBox.Show("フリー1マスタのデータがありません。", "確認", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Case "MST_Free2"
              MessageBox.Show("フリー2マスタのデータがありません。", "確認", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Case "MST_Free3"
              MessageBox.Show("フリー3マスタのデータがありません。", "確認", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Case "MST_Free4"
              MessageBox.Show("フリー4マスタのデータがありません。", "確認", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Case "MST_Free5"
              MessageBox.Show("フリー5マスタのデータがありません。", "確認", MessageBoxButtons.OK, MessageBoxIcon.Error)
          End Select
        Else
          Dim colCount As Integer = OutputDt.Columns.Count
          Dim lastColIndex As Integer = colCount - 1
          Dim i As Integer
          'ヘッダを書き込む
          If isWriteHeader Then
            For i = 0 To colCount - 1
              'ヘッダの取得
              Dim field As String = OutputDt.Columns(i).Caption
              '"で囲み書き込む
              field = EncloseDoubleQuotes(field)
              wrCsv.Write(field)
              'カンマ付与
              If lastColIndex > i Then
                wrCsv.Write(","c)
              End If
            Next
            '改行
            wrCsv.Write(vbCrLf)
          End If
          'レコードを書き込む
          Dim row As DataRow
          For Each row In OutputDt.Rows
            For i = 0 To colCount - 1
              'フィールドの取得
              Dim field As String = row(i).ToString()
              '"で囲み書き込む
              field = EncloseDoubleQuotes(field)
              wrCsv.Write(field)
              'カンマ付与
              If lastColIndex > i Then
                wrCsv.Write(","c)
              End If
            Next
            '改行
            wrCsv.Write(vbCrLf)
          Next
          '閉じる
          wrCsv.Close()
        End If
      End With
    Catch ex As Exception
      Call ComWriteErrLog([GetType]().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message)
      Throw New Exception(ex.Message)
    Finally
      OutputDt.Dispose()
    End Try
  End Sub
  Private Function EncloseDoubleQuotes(field As String) As String
    Return "" & field & ""
  End Function
  Private Function GetItemMasterSelectSql() As String
    Dim sql As String = String.Empty
    sql &= " SELECT "
    sql &= "     call_code AS 呼出コード, "
    sql &= "     item_number AS 品番, "
    sql &= "     unit_weight AS 単重, "
    sql &= "     unit_weight_unit AS 単重単位, "
    sql &= "     safety_factor AS 安全計数, "
    sql &= "     target_qty AS 目標個数, "
    sql &= "     packing AS 風袋, "
    sql &= "     packing_unit AS 風袋単位, "
    sql &= "     upper_limit AS 上限値, "
    sql &= "     standard_value AS 基準値, "
    sql &= "     lower_limit AS 下限値, "
    sql &= "     subtotal_target_qty AS 小計目標値, "
    sql &= "     subtotal_target_cnt AS 小計目標回数, "
    sql &= "     item_name AS 品名, "
    sql &= "     upper_unit_weight AS 単重上限値, "
    sql &= "     upper_unit_weight_unit AS 単重上限値単位, "
    sql &= "     lower_unit_weight AS 単重下限値, "
    sql &= "     lower_unit_weight_unit AS 単重下限値単位 "
    sql &= " FROM "
    sql &= "     MST_Item "
    Call WriteExecuteLog("Module_Upload", System.Reflection.MethodBase.GetCurrentMethod().Name, sql)
    Return sql
  End Function
  Private Function GetManufacturerMasterSelectSql() As String
    Dim sql As String = String.Empty
    sql &= " SELECT"
    sql &= "     Manufacturer_Code As 製造者コード,"
    sql &= "     Manufacturer_Name As 製造者名"
    sql &= " FROM"
    sql &= "     MST_Manufacturer"
    Call WriteExecuteLog([GetType]().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, sql)
    Return sql
  End Function
  Private Function GetStaffMasterSelectSql() As String
    Dim sql As String = String.Empty
    sql &= " SELECT"
    sql &= "     Staff_Number As [担当者№],"
    sql &= "     Staff_Name As [担当者名]"
    sql &= " FROM"
    sql &= "     MST_Staff"
    Call WriteExecuteLog([GetType]().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, sql)
    Return sql
  End Function
  Private Function GetMSTPackingSelectSql() As String
    Dim sql As String = String.Empty
    sql &= " SELECT"
    sql &= "     PackingNo As [風袋№],"
    sql &= "     PackingWeight As [風袋重量],"
    sql &= "     PackingWeightUnit As [風袋重量単位],"
    sql &= "     PackingName As [風袋名称]"
    sql &= " FROM"
    sql &= "     MST_Packing"
    Call WriteExecuteLog([GetType]().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, sql)
    Return sql
  End Function

  Private Function GetMST_Free1MasterSelectSql() As String
    Dim sql As String = String.Empty
    sql &= " SELECT"
    sql &= "     free1_number AS [フリー１№],"
    sql &= "     free1_name AS [フリー１名称]"
    sql &= " FROM"
    sql &= "     MST_Free1"
    Call WriteExecuteLog([GetType]().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, sql)
    Return sql
  End Function

  ' フリー2マスター取得
  Private Function GetMST_Free2MasterSelectSql() As String
    Dim sql As String = String.Empty
    sql &= " SELECT"
    sql &= "     free2_number AS [フリー２№],"
    sql &= "     free2_name AS [フリー２名称]"
    sql &= " FROM"
    sql &= "     MST_Free2"
    Call WriteExecuteLog([GetType]().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, sql)
    Return sql
  End Function

  ' フリー3マスター取得
  Private Function GetMST_Free3MasterSelectSql() As String
    Dim sql As String = String.Empty
    sql &= " SELECT"
    sql &= "     free3_number AS [フリー３№],"
    sql &= "     free3_name AS [フリー３名称]"
    sql &= " FROM"
    sql &= "     MST_Free3"
    Call WriteExecuteLog([GetType]().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, sql)
    Return sql
  End Function

  ' フリー4マスター取得
  Private Function GetMST_Free4MasterSelectSql() As String
    Dim sql As String = String.Empty
    sql &= " SELECT"
    sql &= "     free4_number AS [フリー４№],"
    sql &= "     free4_name AS [フリー４名称]"
    sql &= " FROM"
    sql &= "     MST_Free4"
    Call WriteExecuteLog([GetType]().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, sql)
    Return sql
  End Function

  ' フリー5マスター取得
  Private Function GetMST_Free5MasterSelectSql() As String
    Dim sql As String = String.Empty
    sql &= " SELECT"
    sql &= "     free5_number AS [フリー５№],"
    sql &= "     free5_name AS [フリー５名称]"
    sql &= " FROM"
    sql &= "     MST_Free5"
    Call WriteExecuteLog([GetType]().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, sql)
    Return sql
  End Function

  Private Sub USB_ReceiveButton_Click(sender As Object, e As EventArgs) Handles USB_ReceiveButton.Click
    Dim DuplicateCount As Integer = 0
    Dim InsertCount As Integer = 0
    Dim tmpDb As New ClsSqlServer

    ' USBメモリのパスを取得
    Dim usbPath As String = GetUsbDriveRootPath()
    If String.IsNullOrEmpty(usbPath) Then
      MessageBox.Show("USBメモリが見つかりません。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
      Exit Sub
    End If

    ' FTP バックアップパスを取得（フォルダが存在しない場合、バックアップフォルダ作成）
    If String.IsNullOrEmpty(FtpBackupPath) Then
      Directory.CreateDirectory(FtpBackupPath)
    End If

    ' 確認ダイアログ
    Dim confirmation As DialogResult
    confirmation = MessageBox.Show("実績取り込みを開始しますか？", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
    If confirmation <> DialogResult.Yes Then Exit Sub

    Try
      ' USB内の該当ファイルを取得
      Dim usbDir As String = Path.Combine(usbPath, "IM-7000\TEMP")
      Dim files As String() = Directory.GetFiles(usbDir, "40TRAN*.CSV", System.IO.SearchOption.TopDirectoryOnly)

      If files.Length = 0 Then
        MessageBox.Show("該当するCSVファイルが見つかりません。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        Exit Sub
      End If

      For Each filePath In files

        InsertCount = 0
        DuplicateCount = 0
        ' ファイルパスから号機番号を取得
        Dim fileName As String = System.IO.Path.GetFileNameWithoutExtension(filePath) ' ファイル名（拡張子なし）
        Dim machineNumber As String = fileName.Substring(fileName.Length - 2, 2) & "号機" ' 号機番号 + "号機"

        ' CSVを処理
        Dim dt As New DataTable("TRN_Results")

        ' CSVカラム定義
        Dim columnNames() As String = {"addition_date", "addition_time", "terminal_number", "call_code", "item_number", "item_name",
                                   "unit_weight", "unit_weight_unit", "quantity", "packing", "packing_unit", "weight", "weight_unit",
                                   "manufacturer_code", "manufacturer_name", "lot1", "lot2", "classification", "upper_weight_limit",
                                   "upper_weight_unit", "lower_weight_limit", "lower_weight_unit", "packing1_weight", "packing1_weight_unit",
                                   "packing2_weight", "packing2_weight_unit", "packing2_multiplication", "packing1_number", "packing1_name",
                                   "packing2_number", "packing2_name", "staff_number", "staff_name", "free1_number", "free1_name",
                                   "free2_number", "free2_name", "free3_number", "free3_name", "free4_number", "free4_name",
                                   "free5_number", "free5_name", "processing_date", "processing_time", "effective_date",
                                   "effective_time", "work_order_number", "detail_number", "instruction_qty", "actual_qty",
                                   "work_order_name", "create_date", "update_date"}

        ' データテーブルにカラム追加
        For Each columnName As String In columnNames
          dt.Columns.Add(New DataColumn(columnName, GetType(String)))
        Next

        ' CSVを読み込む（Shift-JISを想定）
        Using textParser As New TextFieldParser(filePath, System.Text.Encoding.GetEncoding("Shift-JIS"))
          textParser.TextFieldType = FieldType.Delimited
          textParser.SetDelimiters(",")

          ' ヘッダーをスキップ
          If Not textParser.EndOfData Then
            textParser.ReadFields()
          End If

          ' 各行を処理
          While Not textParser.EndOfData
            Dim csvRow As String() = textParser.ReadFields()
            Dim dr As DataRow = dt.NewRow()

            ' 行データ追加
            Dim i As Integer
            For i = 0 To Math.Min(csvRow.Length - 1, columnNames.Length - 1)
              dr(i) = csvRow(i)
            Next

            ' 実行時の時刻を追加
            dr("create_date") = DateTime.Now.ToString("yyyy-MM-dd")
            dr("update_date") = DateTime.Now.ToString("yyyy-MM-dd")

            ' 重複チェック
            If DuplicateCheck(dr(0), dr(1), dr(2)) Then
              DuplicateCount += 1
              Continue While
            End If

            dt.Rows.Add(dr)
          End While
        End Using

        ' データをDBに登録
        For Each dr As DataRow In dt.Rows
          Dim sql As String = GetInsertSql(columnNames, dr)
          With tmpDb
            Try
              If .Execute(sql) = 1 Then
                .TrnCommit()
                InsertCount += 1
              Else
                MessageBox.Show("実績管理の登録処理に失敗しました。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
              End If
            Catch ex As Exception
              Call ComWriteErrLog([GetType]().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message)
              Throw New Exception(ex.Message)
            End Try
          End With
        Next

        ' 処理結果メッセージ
        If InsertCount > 0 Then
          If DuplicateCount > 0 Then
            MessageBox.Show(
            machineNumber & "：" & InsertCount & "件 取り込み完了しました。" & vbCrLf &
            "既に登録されている実績データが" & DuplicateCount & "件存在します。",
            "確認", MessageBoxButtons.OK, MessageBoxIcon.Information
        )
          Else
            MessageBox.Show(
            machineNumber & "：" & InsertCount & "件 取り込み完了しました。",
            "完了", MessageBoxButtons.OK, MessageBoxIcon.Information
        )
          End If
        Else
          If DuplicateCount > 0 Then
            MessageBox.Show(
            machineNumber & "：取り込み失敗しました。" & vbCrLf &
            "既に登録されている実績データが" & DuplicateCount & "件存在します。",
            "確認", MessageBoxButtons.OK, MessageBoxIcon.Error
        )
          Else
            MessageBox.Show(
            machineNumber & "：取り込みに失敗しました。データがありません。",
            "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error
        )
          End If
        End If

        ' ----------------------------------------------------
        ' 取り込みが完了した CSVファイルをFTP バックアップ用に
        ' タイムスタンプ付きファイル名に変更しコピー → USB 上の元ファイル削除
        ' ----------------------------------------------------
        If dt.Rows.Count > 0 Then
          Try
            ' 新しいファイル名作成（例: 40TRAN999_20250227142801.CSV）
            Dim timeStamp As String = DateTime.Now.ToString("yyyyMMddHHmmss")
            Dim newFileName As String = Path.GetFileNameWithoutExtension(filePath) & "_" & timeStamp & Path.GetExtension(filePath)
            Dim newFilePath As String = Path.Combine(FtpBackupPath, newFileName)

            ' コピー先のディレクトリが存在しない場合は作成
            If Not Directory.Exists(FtpBackupPath) Then
              Directory.CreateDirectory(FtpBackupPath)
            End If

            ' ファイルをコピーして、コピーできたら USB のファイルを削除
            File.Copy(filePath, newFilePath, overwrite:=False)

            ' 元ファイルとコピー先のファイルのサイズを比較
            If File.Exists(newFilePath) AndAlso
                 New FileInfo(filePath).Length = New FileInfo(newFilePath).Length Then
              File.Delete(filePath)
            Else
              MessageBox.Show("コピー検証に失敗しました。元ファイルは削除されません。", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If

            'File.Delete(filePath)

          Catch ex As Exception
            MessageBox.Show("バックアップファイルの処理に失敗しました：" & ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
          End Try
        End If
      Next
    Catch ex As Exception
      MessageBox.Show("エラーが発生しました：" & ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
    End Try
  End Sub

  Private Sub ResultDetail_CurrentCellDirtyStateChanged(sender As Object, e As EventArgs) Handles ResultDetail.CurrentCellDirtyStateChanged
    If TypeOf ResultDetail.CurrentCell Is DataGridViewCheckBoxCell Then
      ResultDetail.CommitEdit(DataGridViewDataErrorContexts.Commit)
    End If
  End Sub

  Private Sub ResultDetail_CellValueChanged(sender As Object, e As DataGridViewCellEventArgs) Handles ResultDetail.CellValueChanged
    If e.ColumnIndex = 0 AndAlso e.RowIndex >= 0 Then
      Dim isChecked As Boolean = Convert.ToBoolean(ResultDetail.Rows(e.RowIndex).Cells(e.ColumnIndex).Value)

      ' チェック状態に応じて背景色を変更
      If isChecked Then
        ResultDetail.Rows(e.RowIndex).DefaultCellStyle.BackColor = Color.LightYellow
      Else
        ResultDetail.Rows(e.RowIndex).DefaultCellStyle.BackColor = Color.White
      End If
    End If
  End Sub

  Private Sub Form_RealtimeConfirmation_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
    Select Case e.KeyCode
      Case Keys.F1
        ReceiveButton.PerformClick()
      Case Keys.F2
        SendButton.PerformClick()
      Case Keys.F3
        USB_ReceiveButton.PerformClick()
      Case Keys.F4
        USB_SendButton.PerformClick()
      Case Keys.Escape
        Me.Close()
    End Select
  End Sub
End Class
