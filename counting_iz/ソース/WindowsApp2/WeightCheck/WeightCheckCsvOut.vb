Imports Common
Imports Common.ClsFunction
Imports Excel = Microsoft.Office.Interop.Excel
Imports System.Globalization
Imports System.IO

Public Class WeightCheckCsvOut
  Private ReadOnly MonthlyReportPath As String = ReadSettingIniFile("MONTHLY_REPORT_PATH", "VALUE")

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

  Private Sub WeightCheckCsvOut_Load(sender As Object, e As EventArgs) Handles MyBase.Load
    MaximizeBox = False
    Dim updateTime As DateTime = System.IO.File.GetLastWriteTime(System.Reflection.Assembly.GetExecutingAssembly().Location)
    Text = "重量検査一覧表　出力" & " ( " & updateTime & " ) "

    Me.KeyPreview = True
    'フォームが最大化されないようにする
    MaximizeBox = False

    Dim dtNow As DateTime = DateTime.Now
    DateTimeFrom.Text = New Date(dtNow.Year, dtNow.Month, 1)
    DateTimeTo.Text = New Date(dtNow.Year, dtNow.Month, 1).AddMonths(1).AddDays(-1)

    SetItemCodeComboBox()
  End Sub

  Private Sub CloseButton_Click(sender As Object, e As EventArgs) Handles CloseButton.Click
    Dispose()
  End Sub
  Private Sub SetItemCodeComboBox()
    Try
      ' 計量マスタからデータを取得
      Dim ItemData As DataTable = GetDataItemCode()

      ' 計量マスタからデータが取得できなかった場合
      If ItemData.Rows.Count = 0 Then
        ' エラーメッセージを表示して終了
        MessageBox.Show("商品マスタにデータが登録されていません。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
      Else

        ' ItemCode_ComboBox のアイテムをクリア
        ItemCode_ComboBox.Items.Clear()

        ' 空の項目を両方のComboBoxに追加
        'ItemCode_ComboBox.Items.Add("")

        ' ゴミコードデータをループして、それぞれのComboBoxに追加
        For Each row As DataRow In ItemData.Rows
          Dim ItemCode As String = row(0).ToString()
          ItemCode_ComboBox.Items.Add(ItemCode)
        Next

        ItemCode_ComboBox.SelectedIndex = 0
      End If
    Catch ex As Exception
      ' エラーログを書き込んで例外をスロー
      ComWriteErrLog(Me.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message)
      Throw New Exception(ex.Message)
    End Try
  End Sub
  Private Function GetDataItemCode() As DataTable
    ' データベース接続用の一時的なオブジェクトを作成
    Dim tmpDb As New ClsSqlServer

    ' データを格納するための一時的なデータテーブルを作成
    Dim tmpDt As New DataTable

    Try
      ' SQLクエリを実行して、計量マスタからデータをデータテーブルに取得
      SqlServer.GetResult(tmpDt, GetSelectItemMaster)

      ' 取得したデータテーブルを返す
      Return tmpDt
    Catch ex As Exception
      ' エラーログを書き込んで例外をスロー
      ComWriteErrLog(Me.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message)
      Throw New Exception(ex.Message)
    Finally
      ' 一時的なデータテーブルを解放
      tmpDt.Dispose()
    End Try
  End Function
  Private Function GetSelectItemMaster() As String
    Dim sql As String = String.Empty

    sql &= " SELECT CONVERT(VARCHAR, call_code) + ' ' + item_name"
    sql &= " FROM MST_Item "
    sql &= " ORDER BY call_code "

    Call WriteExecuteLog(Me.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, sql)
    Return sql
  End Function

  Private Sub DateTime_From_KeyPress(sender As Object, e As KeyPressEventArgs) Handles DateTimeFrom.KeyPress
    If Not (Char.IsDigit(e.KeyChar) Or e.KeyChar = ControlChars.Back) Then
      e.Handled = True
    End If
  End Sub

  Private Sub DateTime_To_KeyPress(sender As Object, e As KeyPressEventArgs) Handles DateTimeTo.KeyPress
    If Not (Char.IsDigit(e.KeyChar) Or e.KeyChar = ControlChars.Back) Then
      e.Handled = True
    End If
  End Sub

  Private Sub StartButton_Click(sender As Object, e As EventArgs) Handles StartButton.Click
    Dim sFromDate As String = DateTimeFrom.Text.Replace("/", "").Trim()
    Dim sToDate As String = DateTimeTo.Text.Replace("/", "").Trim()

    ' ComboBoxが未選択の場合にメッセージを表示
    If ItemCode_ComboBox.SelectedIndex = -1 Then
      MessageBox.Show("品名を選択してください", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning)
      Return
    End If

    ' From日付の必須入力チェック
    If String.IsNullOrEmpty(sFromDate) Then
      MessageBox.Show("(開始日)を入力してください。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
      DateTimeFrom.Focus()
      Return
    End If

    '' To日付の必須入力チェック
    'If String.IsNullOrEmpty(sToDate) Then
    '  MessageBox.Show("(終了日)を入力してください。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '  DateTime_To.Focus()
    '  Return
    'End If

    ' From日付のチェック
    If Not DateTypeCheck(sFromDate) Then
      MessageBox.Show("(開始日)正しい日付形式を入力してください。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
      DateTimeFrom.SelectAll()
      DateTimeFrom.Focus()
      Return
    End If

    If Not String.IsNullOrWhiteSpace(DateTimeTo.Text) Then
      ' To日付のチェック
      If Not DateTypeCheck(sToDate) Then
        MessageBox.Show("(終了日)正しい日付形式を入力してください。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        DateTimeTo.SelectAll()
        DateTimeTo.Focus()
        Return
      End If

      ' From日付とTo日付の相関チェック
      Dim dtFrom As Date = Date.ParseExact(sFromDate, "yyyyMMdd", Nothing)
      Dim dtTo As Date = Date.ParseExact(sToDate, "yyyyMMdd", Nothing)

      If dtFrom > dtTo Then
        MessageBox.Show("開始日は終了日より前でなければなりません。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        DateTimeFrom.SelectAll()
        DateTimeFrom.Focus()
        Return
      End If
    End If

    ' 出力処理呼び出し
    OutputDetail()
  End Sub

  'Private Sub OutputDetail()
  '  'Dim csvPath As String
  '  Dim isWriteHeader As Boolean = True
  '  'Dim wkTenantName As String
  '  Dim sql As String = String.Empty
  '  Dim OutputDb As New DataTable
  '  Dim OutputDt As New DataTable

  '  'wkTenantName = Detail.Rows(i).Cells(2).Value
  '  'wkTenantName = wkTenantName.Replace("/", "-")

  '  'csvPath = MeisaiPath & wkTenantName & "_" & Now.ToString("yyyyMMddHHmmss") & ".CSV"
  '  sql = SetResultsSelectSql()
  '  Try
  '    With OutputDb
  '      SqlServer.GetResult(OutputDt, sql)




  '    End With
  '  Catch ex As Exception
  '    Call ComWriteErrLog([GetType]().Name,
  '          Reflection.MethodBase.GetCurrentMethod().Name, ex.Message)
  '    Throw New Exception(ex.Message)
  '  Finally
  '    OutputDt.Dispose()
  '  End Try


  '  'MessageBox.Show("【" & MeisaiPath & "】に実績ファイルを保存しました。", "情報", MessageBoxButtons.OK, MessageBoxIcon.Information)
  '  'Process.Start("explorer.exe", MeisaiPath)
  'End Sub
  Private Sub OutputDetail()
    Dim sql As String = SetResultsSelectSql()
    Dim OutputDt As New DataTable
    Dim excelApp As Excel.Application = Nothing
    Dim excelWorkbook As Excel.Workbook = Nothing
    Dim excelWorksheet As Excel.Worksheet = Nothing
    Dim templatePath As String = "D:\remote-repo\measurement_iz\重量検查一覧表_サンプル.xlsx" ' テンプレートパスを指定
    Dim outputDir As String = "C:\KEIRYO_DX\WeightCheck"

    Try
      ' SQLの結果を取得
      SqlServer.GetResult(OutputDt, sql)

      If OutputDt.Rows.Count = 0 Then
        MessageBox.Show("出力データが存在しません。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
      Else
        ' Excelアプリケーションを起動
        excelApp = New Excel.Application()
        excelWorkbook = excelApp.Workbooks.Open(templatePath) ' テンプレートを開く
        excelWorksheet = CType(excelWorkbook.Sheets(1), Excel.Worksheet)

        ' 画面で入力した日付を取得
        Dim fromDate As Date = DateTime.Parse(DateTimeFrom.Text)

        Dim toDate As Nullable(Of Date) ' または Dim toDate As Date?

        If String.IsNullOrWhiteSpace(DateTimeTo.Text) Then
          toDate = Nothing ' 空白の代わりに "値なし" を表す
        Else
          toDate = DateTime.Parse(DateTimeTo.Text)
        End If

        ' 和暦変換（例: 令和7年2月18日）
        Dim jpCulture As New CultureInfo("ja-JP")
        jpCulture.DateTimeFormat.Calendar = New JapaneseCalendar()

        Dim fromDateJP As String = fromDate.ToString("ggyy年M月d日", jpCulture)

        Dim toDateJP As String

        If toDate.HasValue Then
          toDateJP = toDate.Value.ToString("ggyy年M月d日", jpCulture)
        Else
          toDateJP = "" ' toDate が空白の場合は toDateJP も空白にする
        End If


        ' マッピング（画面の日付を適用）
        excelWorksheet.Cells(4, 6) = fromDateJP  ' F4 (開始日)
        excelWorksheet.Cells(4, 8) = toDateJP   ' H4 (終了日)

        ' クエリの最初の行を取得
        Dim itemName As String = "Unknown"
        If OutputDt.Rows.Count > 0 Then
          itemName = OutputDt.Rows(0)("item_name").ToString() ' 品名
          excelWorksheet.Cells(6, 3) = itemName  ' C6
          excelWorksheet.Cells(6, 8) = OutputDt.Rows(0)("upper_limit").ToString() ' H6
          excelWorksheet.Cells(8, 2) = OutputDt.Rows(0)("reference_value").ToString() ' B8
          excelWorksheet.Cells(8, 7) = OutputDt.Rows(0)("average_value").ToString() ' G8
          excelWorksheet.Cells(8, 8) = OutputDt.Rows(0)("lower_limit").ToString() ' H8
        End If

        ' 連番などの開始位置
        Dim startRow As Integer = 10  ' B10 から開始
        Dim dataStartRow As Integer = 11 ' データは11行目から

        ' 連番・データ書き込み
        Dim rowIndex As Integer = dataStartRow
        For Each row As DataRow In OutputDt.Rows
          excelWorksheet.Cells(rowIndex, 2) = row("serial_number").ToString() ' B列（連番）
          excelWorksheet.Cells(rowIndex, 3) = row("addition_date").ToString() ' C列（日付）
          excelWorksheet.Cells(rowIndex, 4) = row("addition_time").ToString() ' D列（時刻）
          excelWorksheet.Cells(rowIndex, 5) = "'" & row("call_code").ToString() ' E列（呼出コード）
          excelWorksheet.Cells(rowIndex, 6) = row("weight").ToString() ' F列（重量）
          excelWorksheet.Cells(rowIndex, 7) = row("weight_unit").ToString() ' G列（重量単位）
          excelWorksheet.Cells(rowIndex, 8) = row("within_limit").ToString() ' H列（判定）
          rowIndex += 1
        Next

        ' フォルダが存在しない場合は作成
        If Not Directory.Exists(outputDir) Then
          Directory.CreateDirectory(outputDir)
        End If

        ' ファイル名を yyyymmddhhmmss_品名 形式にする
        Dim timestamp As String = DateTime.Now.ToString("yyyyMMddHHmmss")
        Dim safeItemName As String = itemName.Replace("/", "_").Replace("\", "_").Replace(":", "_").Replace("*", "_").Replace("?", "_").Replace("""", "_").Replace("<", "_").Replace(">", "_").Replace("|", "_")
        Dim savePath As String = Path.Combine(outputDir, "WeightCheck_" & timestamp & "_" & safeItemName & ".xlsx")

        ' Excelファイルを保存
        excelWorkbook.SaveAs(savePath)
        excelWorkbook.Close()
        excelApp.Quit()

        ' フォルダを開く
        Process.Start("explorer.exe", outputDir)

        MessageBox.Show("Excelファイルを作成しました: " & savePath, "情報", MessageBoxButtons.OK, MessageBoxIcon.Information)
      End If
    Catch ex As Exception
      MessageBox.Show("エラー発生: " & ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
    Finally
      ' リソース解放
      ReleaseObject(excelWorksheet)
      ReleaseObject(excelWorkbook)
      ReleaseObject(excelApp)
      OutputDt.Dispose()
    End Try
  End Sub

  ' Excelオブジェクト解放用
  Private Sub ReleaseObject(ByVal obj As Object)
    Try
      If obj IsNot Nothing Then
        System.Runtime.InteropServices.Marshal.ReleaseComObject(obj)
        obj = Nothing
      End If
    Catch ex As Exception
      obj = Nothing
    Finally
      GC.Collect()
    End Try
  End Sub


  Private Function EncloseDoubleQuotes(field As String) As String
    Return "" & field & ""
  End Function

  Private Function SetResultsSelectSql() As String
    Dim wkItemCode As String = String.Empty
    Dim wkFromText As String
    Dim wkToText As String
    Dim sql As String = String.Empty

    If ItemCode_ComboBox.Text <> "" Then
      wkItemCode = ItemCode_ComboBox.Text.Substring(0, ItemCode_ComboBox.Text.IndexOf(" "))
    End If

    wkFromText = DateTimeFrom.Text

    If String.IsNullOrWhiteSpace(DateTimeTo.Text) Then
      wkToText = "9999/12/31"
    Else
      wkToText = DateTimeTo.Text
    End If

    sql &= " SELECT"
    sql &= "     ROW_NUMBER() OVER(ORDER BY TR.addition_date, TR.addition_time) AS serial_number,"
    sql &= "     TR.addition_date,"
    sql &= "     TR.addition_time,"
    sql &= "     TR.call_code,"
    sql &= "     MI.item_name,"
    sql &= "     TR.weight,"
    sql &= "     TR.weight_unit,"
    sql &= "     MI.reference_value,"
    sql &= "     MI.upper_limit,"
    sql &= "     MI.lower_limit,"
    sql &= "     AVG(TRY_CONVERT(FLOAT, TR.weight)) OVER() AS average_value,"
    sql &= "     Case "
    sql &= "     WHEN TRY_CONVERT(FLOAT, TR.weight) BETWEEN MI.lower_limit And MI.upper_limit THEN '○'"
    sql &= "     Else '×'"
    sql &= "     End As within_limit"
    sql &= " FROM"
    sql &= "     TRN_Results TR"
    sql &= " LEFT JOIN"
    sql &= "     MST_Item MI"
    sql &= " ON"
    sql &= "     TR.call_code = MI.call_code"
    sql &= " WHERE"
    sql &= "     TR.addition_date BETWEEN '" & wkFromText & "' AND '" & wkToText & "' "
    sql &= "     AND TR.call_code = '" & wkItemCode & "' "
    sql &= "     AND TRY_CONVERT(FLOAT, TR.weight) IS NOT NULL"
    sql &= " ORDER BY"
    sql &= "     serial_number,"
    sql &= "     TR.addition_date,"
    sql &= "     TR.addition_time;"

    Call WriteExecuteLog([GetType]().Name, Reflection.MethodBase.GetCurrentMethod().Name, sql)
    Return sql
  End Function

  Private Sub DateTime_From_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles DateTimeFrom.Validating
    If ActiveControl IsNot Nothing AndAlso ActiveControl.Name <> "DateTime_From" AndAlso ActiveControl.Name <> "CloseButton" Then

      Dim inputText As String = DateTimeFrom.Text.Replace("/", "").Trim()

      ' 空白なら何もしない
      If String.IsNullOrWhiteSpace(inputText) Then Return

      If DateTypeCheck(inputText) Then
        DateTimeFrom.Text = DateTxt2DateTxt(inputText)
      Else
        MessageBox.Show("正しい日付形式を入力してください。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        DateTimeFrom.SelectAll()
        e.Cancel = True
      End If
    End If
  End Sub

  Private Sub DateTime_To_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles DateTimeTo.Validating
    If ActiveControl IsNot Nothing AndAlso ActiveControl.Name <> "DateTime_To" AndAlso ActiveControl.Name <> "CloseButton" Then

      Dim inputText As String = DateTimeTo.Text.Replace("/", "").Trim()

      ' 空白なら何もしない
      If String.IsNullOrWhiteSpace(inputText) Then Return

      If DateTypeCheck(inputText) Then
        DateTimeTo.Text = DateTxt2DateTxt(inputText)
      Else
        MessageBox.Show("正しい日付形式を入力してください。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        DateTimeTo.SelectAll()
        e.Cancel = True
      End If
    End If
  End Sub

  Private Sub WeightCheckCsvOut_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
    Select Case e.KeyCode
      Case Keys.F5
        StartButton.PerformClick()
      Case Keys.Escape
        Me.Close()
    End Select
  End Sub
End Class
