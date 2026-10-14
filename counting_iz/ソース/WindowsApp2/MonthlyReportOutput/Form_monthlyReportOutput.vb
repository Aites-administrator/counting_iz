Imports Common
Imports Common.ClsFunction
Public Class Form_monthlyReportOutput
  Private ReadOnly ResultCsvPath As String = ReadSettingIniFile("RESULT_CSV_PATH", "VALUE")
  Private ReadOnly ReportMacro As String = ReadSettingIniFile("REPORTMACRO", "VALUE")
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
  Private Sub Form_monthlyReportOutput_Load(sender As Object, e As EventArgs) Handles MyBase.Load
    MaximizeBox = False
    Dim updateTime As DateTime = System.IO.File.GetLastWriteTime(System.Reflection.Assembly.GetExecutingAssembly().Location)
    Text = "月報出力" & " ( " & updateTime & " ) "
    Me.KeyPreview = True
    MaximizeBox = False
    StartPosition = FormStartPosition.CenterScreen
    FormBorderStyle = FormBorderStyle.FixedSingle

    ' システム日付取得
    Dim dtNow As Date = Date.Now

    ' 年・月のテキストボックスへ設定
    YearText.Text = dtNow.ToString("yyyy")
    MonthText.Text = dtNow.ToString("MM")

    ' 月初と月末を取得して設定
    Dim firstDayOfMonth As Date = New Date(dtNow.Year, dtNow.Month, 1)
    Dim lastDayOfMonth As Date = firstDayOfMonth.AddMonths(1).AddDays(-1)

    DateTime_From.Text = firstDayOfMonth.ToString("yyyy/MM/dd")
    DateTime_To.Text = lastDayOfMonth.ToString("yyyy/MM/dd")

    ' 閉め日をデフォルト31に設定
    ClosingDateText.Text = "31"
    SetScaleNumberComboBoxes()
    SetItemCodeComboBoxes()
  End Sub

  Private Sub StartButton_Click(sender As Object, e As EventArgs) Handles StartButton.Click
    If Not ValidatePeriodInputs() OrElse Not CheckRangeConditions() Then Return
    Using passwordForm As New Form_CsvExportPassword()
      If passwordForm.ShowDialog(Me) <> DialogResult.OK Then
        Exit Sub
      End If
    End Using

    OutputDetail()
  End Sub
  Private Sub MonthText_KeyPress(sender As Object, e As KeyPressEventArgs) Handles MonthText.KeyPress
    'キーが [0]～[9] または [BackSpace] 以外の場合イベントをキャンセル
    If Not (("0"c <= e.KeyChar And e.KeyChar <= "9"c) Or e.KeyChar = ControlChars.Back) Then
      e.Handled = True
    End If

    ' テキストボックスに現在の内容を取得
    Dim currentText As String = MonthText.Text

    ' テキストボックスに新しいキーを追加
    Dim newText As String = currentText.Substring(0, MonthText.SelectionStart) & e.KeyChar & currentText.Substring(MonthText.SelectionStart + MonthText.SelectionLength)

    ' 新しいテキストが13以上の場合は入力を無効化
    If Val(newText) >= 13 Then
      e.Handled = True
    End If
  End Sub
  Private Sub ClosingDateText_KeyPress(sender As Object, e As KeyPressEventArgs) Handles ClosingDateText.KeyPress
    'キーが [0]～[9] または [BackSpace] 以外の場合イベントをキャンセル
    If Not (("0"c <= e.KeyChar And e.KeyChar <= "9"c) Or e.KeyChar = ControlChars.Back) Then
      e.Handled = True
    End If
  End Sub
  Private Sub MonthText_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles MonthText.Validating
    If ActiveControl IsNot Nothing AndAlso ActiveControl.Name <> "MonthText" And ActiveControl.Name <> "CloseButton" Then
      Dim currentMonth As Integer = DateTime.Now.Month

      ' MonthText のテキストの長さに応じて処理を分岐
      Select Case MonthText.Text.Length
        Case 0
          ' テキストが空の場合、現在の月を2桁にして設定
          If currentMonth < 10 Then
            MonthText.Text = "0" & currentMonth.ToString()
          Else
            MonthText.Text = currentMonth.ToString()
          End If
        Case 1
          ' テキストが1桁の場合、前に0を追加
          MonthText.Text = "0" & MonthText.Text
      End Select
      ' 期間を設定する処理を呼び出す
      SetPeriod()
    End If
  End Sub
  Private Sub YearText_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles YearText.Validating
    ' アクティブなコントロールが "YearText" または "CloseButton" でない場合のみ処理を実行
    If ActiveControl IsNot Nothing AndAlso ActiveControl.Name <> "YearText" And ActiveControl.Name <> "CloseButton" Then
      Dim currentYear As Integer = DateTime.Now.Year
      Select Case YearText.Text.Length
        Case 0
          YearText.Text = currentYear
        Case 1, 3
          ' 正しい日付の形式でない場合、エラーメッセージを表示し、フォーカスを YearText に設定
          MessageBox.Show("正しい日付の形式で入力してください。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
          YearText.Focus()
        Case 2
          ' 2桁の場合、現在の年を取得し、入力された年に追加して SetPeriod メソッドを呼び出す
          YearText.Text = Strings.Left(currentYear.ToString(), 2) & YearText.Text
          SetPeriod()
        Case 4
          If YearText.Text = "0000" Then
            ' 正しい日付の形式でない場合、エラーメッセージを表示し、フォーカスを YearText に設定
            MessageBox.Show("正しい日付の形式で入力してください。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
            YearText.Focus()
          Else
            SetPeriod()
          End If
      End Select
    End If
  End Sub
  Private Sub ClosingDateText_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles ClosingDateText.Validating
    If ActiveControl IsNot Nothing AndAlso ActiveControl.Name <> "ClosingDateText" And ActiveControl.Name <> "CloseButton" Then
      SetPeriod()
    End If
  End Sub
  Private Sub ClosingDateText_KeyDown(sender As Object, e As KeyEventArgs) Handles ClosingDateText.KeyDown
    Select Case e.KeyCode
      Case Keys.Enter
        SetPeriod()
    End Select
  End Sub
  Private Sub YearText_KeyPress(sender As Object, e As KeyPressEventArgs) Handles YearText.KeyPress
    'キーが [0]～[9] または [BackSpace] 以外の場合イベントをキャンセル
    If Not (("0"c <= e.KeyChar And e.KeyChar <= "9"c) Or e.KeyChar = ControlChars.Back) Then
      e.Handled = True
    End If
  End Sub
  Private Sub ClosingDateText_TextChanged(sender As Object, e As EventArgs) Handles ClosingDateText.TextChanged
    If String.IsNullOrEmpty(ClosingDateText.Text) Then
      ClosingDateText.Text = "31"
    End If
  End Sub
  Private Sub CloseButton_Click(sender As Object, e As EventArgs) Handles CloseButton.Click
    Close()
  End Sub
  Private Sub DateTime_From_KeyPress(sender As Object, e As KeyPressEventArgs) Handles DateTime_From.KeyPress
    If Not (Char.IsDigit(e.KeyChar) Or e.KeyChar = ControlChars.Back Or e.KeyChar = "/"c) Then
      e.Handled = True
    End If
  End Sub
  Private Sub DateTime_To_KeyPress(sender As Object, e As KeyPressEventArgs) Handles DateTime_To.KeyPress
    If Not (Char.IsDigit(e.KeyChar) Or e.KeyChar = ControlChars.Back Or e.KeyChar = "/"c) Then
      e.Handled = True
    End If
  End Sub
  Private Sub DateTime_From_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles DateTime_From.Validating
    If ActiveControl IsNot Nothing AndAlso ActiveControl.Name <> "DateTime_From" And ActiveControl.Name <> "CloseButton" Then
      Dim inputText As String = DateTime_From.Text.Replace("/", "").Trim()

      If DateTypeCheck(inputText) Then
        DateTime_From.Text = DateTxt2DateTxt(inputText)
      Else
        MessageBox.Show("正しい日付形式を入力してください。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        DateTime_From.SelectAll()
        e.Cancel = True
      End If
    End If
  End Sub
  Private Sub DateTime_To_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles DateTime_To.Validating
    If ActiveControl IsNot Nothing AndAlso ActiveControl.Name <> "DateTime_To" And ActiveControl.Name <> "CloseButton" Then
      Dim inputText As String = DateTime_To.Text.Replace("/", "").Trim()

      If DateTypeCheck(inputText) Then
        DateTime_To.Text = DateTxt2DateTxt(inputText)
      Else
        MessageBox.Show("正しい日付形式を入力してください。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        DateTime_To.SelectAll()
        e.Cancel = True
      End If
    End If
  End Sub
  Private Sub OutputDetail()
    Dim fileName As String = ResultCsvPath & "Results.CSV"
    Dim isWriteHeader As Boolean = True
    Dim sql As String = String.Empty
    Dim OutputDb As New DataTable
    Dim OutputDt As New DataTable
    sql = SetResultsSelectSql()

    Try
      With OutputDb
        SqlServer.GetResult(OutputDt, sql)

        If OutputDt.Rows.Count = 0 Then
          MessageBox.Show("データが存在しません。", "確認", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Else
          'Dim encoding As System.Text.Encoding = System.Text.Encoding.GetEncoding("Shift_JIS")
          Dim encoding As System.Text.Encoding = New System.Text.UTF8Encoding(True)

          Using sr As New System.IO.StreamWriter(fileName, False, encoding)
            Dim colCount As Integer = OutputDt.Columns.Count
            Dim lastColIndex As Integer = colCount - 1

            ' ヘッダー出力
            If isWriteHeader Then
              For i As Integer = 0 To lastColIndex
                ' [] を削除して出力
                Dim header As String = OutputDt.Columns(i).Caption.Replace("[", "").Replace("]", "")
                Dim field As String = EncloseDoubleQuotes(header)
                sr.Write(field)
                If i < lastColIndex Then sr.Write(","c)
              Next
              sr.Write(vbCrLf)
            End If

            For Each row As DataRow In OutputDt.Rows
              For i As Integer = 0 To lastColIndex
                Dim field As String = EncloseDoubleQuotes(row(i).ToString())
                sr.Write(field)
                If i < lastColIndex Then sr.Write(","c)
              Next
              sr.Write(vbCrLf)
            Next
          End Using

          ' マクロ実行
          Dim macroFilePath As String = ReportMacro
          Process.Start(macroFilePath)
        End If
      End With
    Catch ex As Exception
      Call ComWriteErrLog([GetType]().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex)
      Throw
    Finally
      OutputDt.Dispose()
    End Try
  End Sub

  ''' <summary>
  ''' 文字列をダブルクォーテーションで囲む
  ''' </summary>
  Private Function EncloseDoubleQuotes(field As String) As String
    Return "" & field & ""
  End Function
  Private Function SetResultsSelectSql() As String
    Dim wkFromText As String
    Dim wkToText As String
    Dim fromScaleNumber As String = GetSelectedCode(FromScaleNumberComboBox)
    Dim toScaleNumber As String = GetSelectedCode(ToScaleNumberComboBox)
    Dim fromItemCode As String = GetSelectedCode(FromItemCodeComboBox)
    Dim toItemCode As String = GetSelectedCode(ToItemCodeComboBox)
    Dim sql As String = String.Empty
    wkFromText = DateTime_From.Text.Replace("/", "-")
    wkToText = DateTime_To.Text.Replace("/", "-")

    sql &= " SELECT "
    sql &= "     addition_date AS [日付], "
    sql &= "     addition_time AS [時刻], "
    sql &= "     terminal_number AS [端末機№], "
    sql &= "     call_code AS [呼出コード], "
    sql &= "     item_number AS [品番], "
    sql &= "     item_name AS [品名], "
    sql &= "     unit_weight AS [単重], "
    sql &= "     unit_weight_unit AS [単重単位], "
    sql &= "     quantity AS [個数], "
    sql &= "     packing AS [風袋], "
    sql &= "     packing_unit AS [風袋単位], "
    sql &= "     weight AS [重量], "
    sql &= "     weight_unit AS [重量単位], "
    sql &= "     manufacturer_code AS [製造者コード], "
    sql &= "     manufacturer_name AS [製造者名], "
    sql &= "     lot1 AS [ロット１], "
    sql &= "     lot2 AS [ロット２], "
    sql &= "     classification AS [区分], "
    sql &= "     upper_weight_limit  AS [単重上限値], "
    sql &= "     upper_weight_unit AS [単重上限値単位], "
    sql &= "     lower_weight_limit AS [単重下限値], "
    sql &= "     lower_weight_unit AS [単重下限値単位], "
    sql &= "     packing1_weight AS [風袋１重量], "
    sql &= "     packing1_weight_unit AS [風袋１重量単位], "
    sql &= "     packing2_weight AS [風袋２重量], "
    sql &= "     packing2_weight_unit AS [風袋２重量単位], "
    sql &= "     packing2_multiplication AS [風袋２の掛け算], "
    sql &= "     packing1_number AS [風袋１№], "
    sql &= "     packing1_name AS [風袋１名称], "
    sql &= "     packing2_number AS [風袋２№], "
    sql &= "     packing2_name AS [風袋２名称], "
    sql &= "     staff_number AS [担当者№], "
    sql &= "     staff_name AS [担当者名称], "
    sql &= "     free1_number AS [フリー１№], "
    sql &= "     free1_name AS [フリー１名称], "
    sql &= "     free2_number AS [フリー２№], "
    sql &= "     free2_name AS [フリー２名称], "
    sql &= "     free3_number AS [フリー３№], "
    sql &= "     free3_name AS [フリー３名称], "
    sql &= "     free4_number AS [フリー４№], "
    sql &= "     free4_name AS [フリー４名称], "
    sql &= "     free5_number AS [フリー５№], "
    sql &= "     free5_name AS [フリー５名称], "
    sql &= "     processing_date AS [加工日], "
    sql &= "     processing_time AS [加工時刻], "
    sql &= "     effective_date AS [有効日], "
    sql &= "     effective_time AS [有効時刻], "
    sql &= "     work_order_number AS [作業指示№], "
    sql &= "     detail_number AS [明細№], "
    sql &= "     instruction_qty AS [指示数], "
    sql &= "     actual_qty AS [実績数], "
    sql &= "     work_order_name AS [作業指示名称] "
    sql &= " FROM "
    sql &= "     TRN_Results "
    sql &= " WHERE "
    sql &= "     addition_date BETWEEN '" & wkFromText & "' AND '" & wkToText & "' "
    If Not String.IsNullOrWhiteSpace(fromScaleNumber) Then sql &= " AND terminal_number >= '" & EscapeSqlLiteral(fromScaleNumber) & "' "
    If Not String.IsNullOrWhiteSpace(toScaleNumber) Then sql &= " AND terminal_number <= '" & EscapeSqlLiteral(toScaleNumber) & "' "
    If Not String.IsNullOrWhiteSpace(fromItemCode) Then sql &= " AND call_code >= '" & EscapeSqlLiteral(fromItemCode) & "' "
    If Not String.IsNullOrWhiteSpace(toItemCode) Then sql &= " AND call_code <= '" & EscapeSqlLiteral(toItemCode) & "' "
    sql &= " ORDER BY "
    sql &= "     CAST(addition_date AS DATETIME) DESC, "
    sql &= "     CAST(addition_time AS DATETIME) DESC, "
    sql &= "     terminal_number; "


    Call WriteExecuteLog([GetType]().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, sql)
    Return sql
  End Function

  Private Sub SetPeriod()
    Dim year As Integer
    Dim month As Integer
    Dim closingDay As Integer
    If Not Integer.TryParse(YearText.Text, year) OrElse year < 1 OrElse year > 9999 OrElse
       Not Integer.TryParse(MonthText.Text, month) OrElse month < 1 OrElse month > 12 OrElse
       Not Integer.TryParse(ClosingDateText.Text, closingDay) OrElse closingDay < 1 OrElse closingDay > 31 Then
      Return
    End If

    Dim Day As String = year.ToString("D4") & month.ToString("D2") & closingDay.ToString("D2")
    Dim ClosingDate As Date
    Dim wkFromText As Date
    Dim wkToText As Date

    If closingDay = 31 Then
      DateTime_From.Text = New Date(year, month, 1)
      DateTime_To.Text = New Date(year, month, 1).AddMonths(1).AddDays(-1)
    Else
      If ClsFunction.DateTypeCheck(Day) Then
        ClosingDate = New Date(year, month, closingDay)
        wkFromText = ClosingDate.AddMonths(-1).AddDays(+1)
        wkToText = ClosingDate
        DateTime_From.Text = wkFromText
        DateTime_To.Text = wkToText
      Else
        MessageBox.Show("正しい日付を入力してください", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        ClosingDateText.Focus()
        Exit Sub
      End If
    End If
  End Sub

  Private Sub Form_monthlyReportOutput_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
    Select Case e.KeyCode
      Case Keys.F5
        StartButton.PerformClick()
      Case Keys.Escape
        Me.Close()
    End Select
  End Sub
  Private Sub ApplyCommonLayout_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
    Common.ClsFunction.ApplyScreenLayout(Me)
  End Sub
  Private Sub SetScaleNumberComboBoxes()
    Dim scaleData As New DataTable

    Try
      SqlServer.GetResult(scaleData, GetSelectScaleMasterSql())

      FromScaleNumberComboBox.Items.Clear()
      ToScaleNumberComboBox.Items.Clear()
      FromScaleNumberComboBox.Items.Add("")
      ToScaleNumberComboBox.Items.Add("")

      For Each row As DataRow In scaleData.Rows
        Dim scaleNumber As String = row("unit_number").ToString()
        FromScaleNumberComboBox.Items.Add(scaleNumber)
        ToScaleNumberComboBox.Items.Add(scaleNumber)
      Next

      FromScaleNumberComboBox.SelectedIndex = 0
      ToScaleNumberComboBox.SelectedIndex = 0
    Finally
      scaleData.Dispose()
    End Try
  End Sub
  Private Sub SetItemCodeComboBoxes()
    Dim itemData As New DataTable

    Try
      SqlServer.GetResult(itemData, GetSelectItemMasterSql())

      FromItemCodeComboBox.Items.Clear()
      ToItemCodeComboBox.Items.Clear()
      FromItemCodeComboBox.Items.Add("")
      ToItemCodeComboBox.Items.Add("")

      For Each row As DataRow In itemData.Rows
        Dim itemDisplay As String = row("call_code").ToString() & " " & row("item_name").ToString()
        FromItemCodeComboBox.Items.Add(itemDisplay)
        ToItemCodeComboBox.Items.Add(itemDisplay)
      Next

      FromItemCodeComboBox.SelectedIndex = 0
      ToItemCodeComboBox.SelectedIndex = 0
    Finally
      itemData.Dispose()
    End Try
  End Sub
  Private Function GetSelectScaleMasterSql() As String
    Dim sql As String = String.Empty
    sql &= " SELECT unit_number"
    sql &= " FROM MST_SCALE"
    sql &= " ORDER BY unit_number"
    Call WriteExecuteLog([GetType]().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, sql)
    Return sql
  End Function
  Private Function GetSelectItemMasterSql() As String
    Dim sql As String = String.Empty
    sql &= " SELECT call_code, item_name"
    sql &= " FROM MST_Item"
    sql &= " ORDER BY call_code"
    Call WriteExecuteLog([GetType]().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, sql)
    Return sql
  End Function
  Private Function GetSelectedCode(comboBox As ComboBox) As String
    If comboBox.SelectedIndex <= 0 OrElse String.IsNullOrWhiteSpace(comboBox.Text) Then Return String.Empty
    Return comboBox.Text.Split(" "c)(0)
  End Function
  Private Function CheckRangeConditions() As Boolean
    If FromScaleNumberComboBox.SelectedIndex > 0 AndAlso
       ToScaleNumberComboBox.SelectedIndex > 0 AndAlso
       FromScaleNumberComboBox.SelectedIndex > ToScaleNumberComboBox.SelectedIndex Then
      MessageBox.Show("号機(開始)は号機(終了)以前を指定してください。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
      FromScaleNumberComboBox.Focus()
      Return False
    End If

    If FromItemCodeComboBox.SelectedIndex > 0 AndAlso
       ToItemCodeComboBox.SelectedIndex > 0 AndAlso
       FromItemCodeComboBox.SelectedIndex > ToItemCodeComboBox.SelectedIndex Then
      MessageBox.Show("商品(開始)は商品(終了)以前を指定してください。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
      FromItemCodeComboBox.Focus()
      Return False
    End If

    Return True
  End Function
  Private Function ValidatePeriodInputs() As Boolean
    Dim year As Integer
    Dim month As Integer
    Dim closingDay As Integer
    Dim fromDate As Date
    Dim toDate As Date

    If Not Integer.TryParse(YearText.Text, year) OrElse year < 1 OrElse year > 9999 Then
      MessageBox.Show("年は1～9999の数字で入力してください。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
      YearText.Focus()
      Return False
    End If
    If Not Integer.TryParse(MonthText.Text, month) OrElse month < 1 OrElse month > 12 Then
      MessageBox.Show("月は1～12の数字で入力してください。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
      MonthText.Focus()
      Return False
    End If
    If Not Integer.TryParse(ClosingDateText.Text, closingDay) OrElse closingDay < 1 OrElse closingDay > 31 Then
      MessageBox.Show("締め日は1～31の数字で入力してください。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
      ClosingDateText.Focus()
      Return False
    End If
    If Not Date.TryParse(DateTime_From.Text, fromDate) Then
      MessageBox.Show("開始日を正しい日付形式で入力してください。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
      DateTime_From.Focus()
      Return False
    End If
    If Not Date.TryParse(DateTime_To.Text, toDate) Then
      MessageBox.Show("終了日を正しい日付形式で入力してください。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
      DateTime_To.Focus()
      Return False
    End If
    If fromDate > toDate Then
      MessageBox.Show("開始日は終了日以前を指定してください。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
      DateTime_From.Focus()
      Return False
    End If

    Return True
  End Function
  Private Function EscapeSqlValue(value As String) As String
    Return value.Replace("'", "''")
  End Function
End Class
