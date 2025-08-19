Imports System.IO
Imports Common
Imports Common.ClsFunction
Imports Microsoft.Office.Interop
Imports System.Runtime.InteropServices
Public Class Form_ResultList
  Private ReadOnly ItemDigits As Integer = ReadSettingIniFile("ITEM_DIGITS", "VALUE")
  Private ReadOnly ManufacturerDigits As Integer = ReadSettingIniFile("MANUFACTURER_DIGITS", "VALUE")
  Private ReadOnly StaffDigits As Integer = ReadSettingIniFile("STAFF_DIGITS", "VALUE")
  Private ReadOnly ResultCsvPath As String = ReadSettingIniFile("RESULT_CSV_PATH", "VALUE")

  Private ReadOnly ReportMacro As String = ReadSettingIniFile("REPORTMACRO", "VALUE")

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
  Private Sub ResultList_Load(sender As Object, e As EventArgs) Handles MyBase.Load
    ' フォームの最大化ボタンを無効にする
    MaximizeBox = False

    ' アセンブリの最終更新日時を取得し、フォームのタイトルに表示するテキストを設定
    Dim updateTime As DateTime = System.IO.File.GetLastWriteTime(System.Reflection.Assembly.GetExecutingAssembly().Location)
    Text = "実績一覧" & " ( " & updateTime & " ) "

    Me.KeyPreview = True

    ' データベースアクセスのための一時的なオブジェクトを作成
    Dim tmpDb As New ClsSqlServer
    Dim tmpDt As New DataTable

    ' フォームのボーダースタイルを固定サイズに設定
    FormBorderStyle = FormBorderStyle.FixedSingle

    ' 行ヘッダーを非表示にする
    ResultDetail.RowHeadersVisible = False

    Dim dtNow As DateTime = DateTime.Now
    DateTimeFrom.Text = New Date(dtNow.Year, dtNow.Month, 1)
    DateTimeTo.Text = New Date(dtNow.Year, dtNow.Month, 1).AddMonths(1).AddDays(-1)

    ' コンボボックスの選択肢を設定する関数を呼び出し
    SetScaleNumberComboBox()
    SetItemCodeComboBox()
    SetStaffNumberComboBox
    'SetManufacturerCodeComboBox()

    ' ユーザーからのデータ追加を許可しない
    ResultDetail.AllowUserToAddRows = False

    ' DataGridViewの列数を設定
    ResultDetail.ColumnCount = 52

    ResultDetail.Columns(0).HeaderText = "日付"
    ResultDetail.Columns(1).HeaderText = "時刻"
    ResultDetail.Columns(2).HeaderText = "号機"
    ResultDetail.Columns(3).HeaderText = "呼出ｺｰﾄﾞ"
    ResultDetail.Columns(4).HeaderText = "品番"
    ResultDetail.Columns(5).HeaderText = "品名"
    ResultDetail.Columns(6).HeaderText = "単重"
    ResultDetail.Columns(7).HeaderText = "単位"
    ResultDetail.Columns(8).HeaderText = "個数"
    ResultDetail.Columns(9).HeaderText = "風袋"
    ResultDetail.Columns(10).HeaderText = "風袋単位"
    ResultDetail.Columns(11).HeaderText = "重量"
    ResultDetail.Columns(12).HeaderText = "単位"
    ResultDetail.Columns(13).HeaderText = "製造者ｺｰﾄﾞ"
    ResultDetail.Columns(14).HeaderText = "製造者名"
    ResultDetail.Columns(15).HeaderText = "ﾛｯﾄ1"
    ResultDetail.Columns(16).HeaderText = "ﾛｯﾄ2"
    ResultDetail.Columns(17).HeaderText = "区分"
    ResultDetail.Columns(18).HeaderText = "単重上限値"
    ResultDetail.Columns(19).HeaderText = "単重上限値単位"
    ResultDetail.Columns(20).HeaderText = "単重下限値"
    ResultDetail.Columns(21).HeaderText = "単重下限値単位"
    ResultDetail.Columns(22).HeaderText = "風袋１重量"
    ResultDetail.Columns(23).HeaderText = "風袋１重量単位"
    ResultDetail.Columns(24).HeaderText = "風袋２重量"
    ResultDetail.Columns(25).HeaderText = "風袋２重量単位"
    ResultDetail.Columns(26).HeaderText = "風袋２の掛け算"
    ResultDetail.Columns(27).HeaderText = "風袋１№"
    ResultDetail.Columns(28).HeaderText = "風袋１名称"
    ResultDetail.Columns(29).HeaderText = "風袋２№"
    ResultDetail.Columns(30).HeaderText = "風袋２名称"
    ResultDetail.Columns(31).HeaderText = "担当者№"
    ResultDetail.Columns(32).HeaderText = "担当者名"
    ResultDetail.Columns(33).HeaderText = "フリー１№"
    ResultDetail.Columns(34).HeaderText = "フリー１名称"
    ResultDetail.Columns(35).HeaderText = "フリー２№"
    ResultDetail.Columns(36).HeaderText = "フリー２名称"
    ResultDetail.Columns(37).HeaderText = "フリー３№"
    ResultDetail.Columns(38).HeaderText = "フリー３名称"
    ResultDetail.Columns(39).HeaderText = "フリー４№"
    ResultDetail.Columns(40).HeaderText = "フリー４名称"
    ResultDetail.Columns(41).HeaderText = "フリー５№"
    ResultDetail.Columns(42).HeaderText = "フリー５名称"
    ResultDetail.Columns(43).HeaderText = "加工日"
    ResultDetail.Columns(44).HeaderText = "加工時刻"
    ResultDetail.Columns(45).HeaderText = "有効日"
    ResultDetail.Columns(46).HeaderText = "有効時刻"
    ResultDetail.Columns(47).HeaderText = "作業指示№"
    ResultDetail.Columns(48).HeaderText = "明細№"
    ResultDetail.Columns(49).HeaderText = "指示数"
    ResultDetail.Columns(50).HeaderText = "実績数"
    ResultDetail.Columns(51).HeaderText = "作業指示名称"


    ' 表示したい列のインデックス
    Dim visibleIndexes As New HashSet(Of Integer) From {
    0, 1, 2, 3, 4, 5, 6, 7, 8,
    11, 12, 15, 16, 31, 32}

    ' 全列をループして、表示・非表示を切り替える
    For i As Integer = 0 To ResultDetail.Columns.Count - 1
      ResultDetail.Columns(i).Visible = visibleIndexes.Contains(i)
    Next


    ' ヘッダーとセルの内容を中央寄せに設定
    For i As Integer = 0 To 51
      ResultDetail.Columns(i).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
      ResultDetail.Columns(i).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
    Next

    ' マルチ選択を無効にする
    ResultDetail.MultiSelect = False

    ' 選択モードを全カラム選択に設定
    ResultDetail.SelectionMode = DataGridViewSelectionMode.FullRowSelect

    ' データを読み込むための関数を呼び出し
    SelectResults()

    If ResultDetail.Rows.Count > 0 Then
      ResultDetail.CurrentCell = ResultDetail.Rows(0).Cells(0)
      ResultDetail.Rows(0).Selected = True
    End If

    ' コンボボックスの幅を調整する関数を呼び出し
    'ChangeComboBoxWidth()

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
  Private Sub CreateButton_Click(sender As Object, e As EventArgs) Handles CreateButton.Click
    Form_ResultDetail.InputMode = 1
    Form_ResultDetail.ShowDialog()
  End Sub
  Private Sub UpdateButton_Click(sender As Object, e As EventArgs) Handles UpdateButton.Click
    '詳細画面の項目値セット
    SetListData()
    Form_ResultDetail.InputMode = 2
    Form_ResultDetail.ShowDialog()
  End Sub

  Private Sub SetScaleNumberComboBox()
    Try
      ' 計量マスタからデータを取得
      Dim scaleData As DataTable = GetDataFromScaleNumber()

      ' 計量マスタからデータが取得できなかった場合
      If scaleData.Rows.Count = 0 Then
        ' エラーメッセージを表示して終了
        MessageBox.Show("計量マスタにデータが登録されていません。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
      Else
        ' ComboBoxのアイテムをクリア
        Scale_ComboBox.Items.Clear()

        ' 空の項目をComboBoxに追加
        Scale_ComboBox.Items.Add("")

        ' 計量マスタから取得したデータをComboBoxに追加
        For Each row As DataRow In scaleData.Rows
          Scale_ComboBox.Items.Add(row(0).ToString())
        Next
      End If
    Catch ex As Exception
      ' エラーログを書き込んで例外をスロー
      ComWriteErrLog(Me.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message)
      Throw New Exception(ex.Message)
    End Try
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

        ' FromItemCode_ComboBox のアイテムをクリア
        FromItemCode_ComboBox.Items.Clear()
        ' ToItemCode_ComboBox のアイテムをクリア
        ToItemCode_ComboBox.Items.Clear()

        ' 空の項目を両方のComboBoxに追加
        FromItemCode_ComboBox.Items.Add("")
        ToItemCode_ComboBox.Items.Add("")

        ' ゴミコードデータをループして、それぞれのComboBoxに追加
        For Each row As DataRow In ItemData.Rows
          Dim ItemCode As String = row(0).ToString()
          FromItemCode_ComboBox.Items.Add(ItemCode)
          ToItemCode_ComboBox.Items.Add(ItemCode)
        Next

      End If
    Catch ex As Exception
      ' エラーログを書き込んで例外をスロー
      ComWriteErrLog(Me.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message)
      Throw New Exception(ex.Message)
    End Try
  End Sub
  Private Sub SetStaffNumberComboBox()
    Try
      ' 担当者マスタからデータを取得
      Dim StaffNumberData As DataTable = GetDataStaffNumber()

      ' 担当者マスタからデータが取得できなかった場合
      If StaffNumberData.Rows.Count = 0 Then
        ' エラーメッセージを表示して終了
        MessageBox.Show("担当者マスタにデータが登録されていません。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
      Else
        'FromStaffCode_ComboBox のアイテムをクリア
        FromStaffCode_ComboBox.Items.Clear()
        ' ToStaffCode_ComboBox のアイテムをクリア
        ToStaffCode_ComboBox.Items.Clear()

        ' 空の項目を両方のComboBoxに追加
        FromStaffCode_ComboBox.Items.Add("")
        ToStaffCode_ComboBox.Items.Add("")

        ' 担当者ｺｰﾄﾞデータをループして、それぞれのComboBoxに追加
        For Each row As DataRow In StaffNumberData.Rows
          Dim StaffNumber As String = row(0).ToString()
          FromStaffCode_ComboBox.Items.Add(StaffNumber)
          ToStaffCode_ComboBox.Items.Add(StaffNumber)
        Next
      End If
    Catch ex As Exception
      ' エラーログを書き込んで例外をスロー
      ComWriteErrLog(Me.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message)
      Throw New Exception(ex.Message)
    End Try
  End Sub
  Private Function GetDataStaffNumber() As DataTable
    ' データベース接続用の一時的なオブジェクトを作成
    Dim tmpDb As New ClsSqlServer

    ' データを格納するための一時的なデータテーブルを作成
    Dim tmpDt As New DataTable

    Try
      ' SQLクエリを実行して、担当者マスタからデータをデータテーブルに取得
      SqlServer.GetResult(tmpDt, GetSelectStaffMaster)

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

  'Private Sub SetManufacturerCodeComboBox()
  '  Try
  '    ' 計量マスタからデータを取得
  '    Dim ManufacturerCodeData As DataTable = GetDataManufacturerCode()

  '    ' 計量マスタからデータが取得できなかった場合
  '    If ManufacturerCodeData.Rows.Count = 0 Then
  '      ' エラーメッセージを表示して終了
  '      MessageBox.Show("製造者マスタにデータが登録されていません。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
  '    Else
  '      FromManuCode_ComboBox.Items.Clear()
  '      ToManuCode_ComboBox.Items.Clear()

  '      ' 空の項目を両方のComboBoxに追加
  '      FromManuCode_ComboBox.Items.Add("")
  '      ToManuCode_ComboBox.Items.Add("")

  '      ' ゴミコードデータをループして、それぞれのComboBoxに追加
  '      For Each row As DataRow In ManufacturerCodeData.Rows
  '        Dim ManufacturerCode As String = row(0).ToString()
  '        FromManuCode_ComboBox.Items.Add(ManufacturerCode)
  '        ToManuCode_ComboBox.Items.Add(ManufacturerCode)
  '      Next
  '    End If
  '  Catch ex As Exception
  '    ' エラーログを書き込んで例外をスロー
  '    ComWriteErrLog(Me.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message)
  '    Throw New Exception(ex.Message)
  '  End Try
  'End Sub

  Private Function GetDataFromScaleNumber() As DataTable
    ' データベース接続用の一時的なオブジェクトを作成
    Dim tmpDb As New ClsSqlServer

    ' データを格納するための一時的なデータテーブルを作成
    Dim tmpDt As New DataTable

    Try
      ' SQLクエリを実行して、計量マスタからデータをデータテーブルに取得
      SqlServer.GetResult(tmpDt, GetSelectScaleMaster)

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

  'Private Function GetDataManufacturerCode() As DataTable
  '  ' データベース接続用の一時的なオブジェクトを作成
  '  Dim tmpDb As New ClsSqlServer

  '  ' データを格納するための一時的なデータテーブルを作成
  '  Dim tmpDt As New DataTable

  '  Try
  '    ' SQLクエリを実行して、計量マスタからデータをデータテーブルに取得
  '    SqlServer.GetResult(tmpDt, GetSelectManufacturerMaster)

  '    ' 取得したデータテーブルを返す
  '    Return tmpDt
  '  Catch ex As Exception
  '    ' エラーログを書き込んで例外をスロー
  '    ComWriteErrLog(Me.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message)
  '    Throw New Exception(ex.Message)
  '  Finally
  '    ' 一時的なデータテーブルを解放
  '    tmpDt.Dispose()
  '  End Try
  'End Function

  Private Function GetSelectScaleMaster() As String
    Dim sql As String = String.Empty

    sql &= " SELECT unit_number "
    sql &= " FROM MST_SCALE "
    sql &= " ORDER BY unit_number "

    Call WriteExecuteLog(Me.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, sql)
    Return sql
  End Function

  Private Function GetSelectItemMaster() As String
    Dim sql As String = String.Empty

    sql &= " SELECT CONVERT(VARCHAR, call_code) + ' ' + item_name"
    sql &= " FROM MST_Item "
    sql &= " ORDER BY call_code "

    Call WriteExecuteLog(Me.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, sql)
    Return sql
  End Function
  Private Function GetSelectStaffMaster() As String
    Dim sql As String = String.Empty

    sql &= " SELECT CONVERT(VARCHAR, Staff_Number) + ' ' + Staff_Name"
    sql &= " FROM MST_Staff "
    sql &= " ORDER BY Staff_Number "

    Call WriteExecuteLog(Me.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, sql)
    Return sql
  End Function

  'Private Function GetSelectManufacturerMaster() As String
  '  Dim sql As String = String.Empty

  '  sql &= " SELECT CONVERT(VARCHAR, Manufacturer_Code) + ' ' + Manufacturer_Name"
  '  sql &= " FROM MST_Manufacturer "
  '  sql &= " ORDER BY Manufacturer_Code "

  '  Call WriteExecuteLog(Me.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, sql)
  '  Return sql
  'End Function

  Public Sub SelectResults()
    Dim tmpDb As New ClsSqlServer
    Dim tmpDt As New DataTable
    Try
      With tmpDb
        SqlServer.GetResult(tmpDt, GetSelectSql)
        WriteDetail(tmpDt, ResultDetail)
        If tmpDt.Rows.Count = 0 Then
          UpdateButton.Enabled = False
          DeleteButton.Enabled = False
        Else
          WriteDetail(tmpDt, ResultDetail)
          UpdateButton.Enabled = True
          DeleteButton.Enabled = True
        End If

        ResultDetail.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells)
        ResultDetail.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False
        ResultDetail.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        ResultDetail.ColumnHeadersHeight = 20

      End With
    Catch ex As Exception
      Call ComWriteErrLog(Me.GetType().Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message)
      Throw New Exception(ex.Message)
    Finally
      tmpDt.Dispose()
    End Try
  End Sub

  Private Sub AddTotalRow()
    ' データグリッドビューの参照
    Dim dataGridView As DataGridView = Me.ResultDetail

    ' 合計値を計算する列のインデックス (35列目なので、インデックスは34)
    Dim columnIndex As Integer = 34

    ' 合計を計算する変数
    Dim total As Decimal = 0

    ' データグリッドビューの各行をループして合計を計算
    For Each row As DataGridViewRow In dataGridView.Rows
      If Not row.IsNewRow Then
        Dim cellValue As Object = row.Cells(columnIndex).Value
        Dim cellValueDecimal As Decimal

        If Decimal.TryParse(cellValue.ToString(), cellValueDecimal) Then
          total += cellValueDecimal
        End If
      End If
    Next

    ' 合計行を追加
    Dim totalRow As DataGridViewRow = CType(dataGridView.Rows(0).Clone(), DataGridViewRow)

    ' すべてのセルに空の値を設定し、合計を表示するセルに合計値を設定
    For i As Integer = 0 To dataGridView.Columns.Count - 1
      totalRow.Cells(i).Value = ""
    Next
    totalRow.Cells(columnIndex).Value = "合計: " & total.ToString()

    ' 合計行をデータグリッドビューに追加
    dataGridView.Rows.Add(totalRow)
  End Sub


  Private Function GetSelectSql() As String
    Dim tmpDb As New ClsSqlServer

    Dim sql As String = String.Empty

    Dim wkFromDate As String = DateTimeFrom.Text
    Dim wkToDate As String = DateTimeTo.Text

    ' フィルタリング条件に使用する一時的な変数を宣言
    Dim wkFromItemCode As String = String.Empty
    Dim wkToItemCode As String = String.Empty
    Dim wkFromStaffCode As String = String.Empty
    Dim wkToStaffCode As String = String.Empty
    'Dim wkFromManufacturerCode As String
    'Dim wkToManufacturerCode As String

    ' ComboBoxのテキストから特定の部分を抽出し、条件変数に格納
    If FromItemCode_ComboBox.Text <> "" Then
      wkFromItemCode = FromItemCode_ComboBox.Text.Substring(0, FromItemCode_ComboBox.Text.IndexOf(" "))
    ElseIf FromItemCode_ComboBox.Items.Count > 0 Then
      wkFromItemCode = FromItemCode_ComboBox.Items(0).ToString().Split(" "c)(0)
    Else
      wkFromItemCode = 0
    End If

    If ToItemCode_ComboBox.Text <> "" Then
      wkToItemCode = ToItemCode_ComboBox.Text.Substring(0, ToItemCode_ComboBox.Text.IndexOf(" "))
    ElseIf ToItemCode_ComboBox.Items.Count > 0 Then
      wkToItemCode = ToItemCode_ComboBox.Items(ToItemCode_ComboBox.Items.Count - 1).ToString().Split(" "c)(0)
    End If

    ' StaffCode の範囲を設定
    If FromStaffCode_ComboBox.Text <> "" Then
      wkFromStaffCode = FromStaffCode_ComboBox.Text.Substring(0, FromStaffCode_ComboBox.Text.IndexOf(" "))
    ElseIf FromStaffCode_ComboBox.Items.Count > 0 Then
      wkFromStaffCode = FromStaffCode_ComboBox.Items(0).ToString().Split(" "c)(0)
    Else
      wkFromStaffCode = 0
    End If

    If ToStaffCode_ComboBox.Text <> "" Then
      wkToStaffCode = ToStaffCode_ComboBox.Text.Substring(0, ToStaffCode_ComboBox.Text.IndexOf(" "))
    ElseIf ToStaffCode_ComboBox.Items.Count > 0 Then
      wkToStaffCode = ToStaffCode_ComboBox.Items(ToStaffCode_ComboBox.Items.Count - 1).ToString().Split(" "c)(0)
    End If

    sql &= " SELECT "
    sql &= "     addition_date, "
    sql &= "     addition_time, "
    sql &= "     terminal_number, "
    sql &= "     call_code, "
    sql &= "     item_number, "
    sql &= "     item_name, "
    sql &= "     unit_weight, "
    sql &= "     unit_weight_unit, "
    sql &= "     quantity, "
    sql &= "     packing, "
    sql &= "     packing_unit, "
    sql &= "     weight, "
    sql &= "     weight_unit, "
    sql &= "     manufacturer_code, "
    sql &= "     manufacturer_name, "
    sql &= "     lot1, "
    sql &= "     lot2, "
    sql &= "     classification, "
    sql &= "     upper_weight_limit, "
    sql &= "     upper_weight_unit, "
    sql &= "     lower_weight_limit, "
    sql &= "     lower_weight_unit, "
    sql &= "     packing1_weight, "
    sql &= "     packing1_weight_unit, "
    sql &= "     packing2_weight, "
    sql &= "     packing2_weight_unit, "
    sql &= "     packing2_multiplication, "
    sql &= "     packing1_number, "
    sql &= "     packing1_name, "
    sql &= "     packing2_number, "
    sql &= "     packing2_name, "
    sql &= "     staff_number, "
    sql &= "     staff_name, "
    sql &= "     free1_number, "
    sql &= "     free1_name, "
    sql &= "     free2_number, "
    sql &= "     free2_name, "
    sql &= "     free3_number, "
    sql &= "     free3_name, "
    sql &= "     free4_number, "
    sql &= "     free4_name, "
    sql &= "     free5_number, "
    sql &= "     free5_name, "
    sql &= "     processing_date, "
    sql &= "     processing_time, "
    sql &= "     effective_date, "
    sql &= "     effective_time, "
    sql &= "     work_order_number, "
    sql &= "     detail_number, "
    sql &= "     instruction_qty, "
    sql &= "     actual_qty, "
    sql &= "     work_order_name "
    sql &= " FROM "
    sql &= "     TRN_Results "
    sql &= " WHERE "
    sql &= "    addition_date BETWEEN '" & wkFromDate & "' AND '" & wkToDate & "' "
    If Scale_ComboBox.Text <> "" Then
      sql &= "    AND terminal_number = " & Scale_ComboBox.Text
    End If
    sql &= "    AND call_code  BETWEEN  '" & wkFromItemCode & "' AND '" & wkToItemCode & "'"
    sql &= "    AND staff_number BETWEEN '" & wkFromStaffCode & "' AND '" & wkToStaffCode & "'"
    'sql &= "    AND manufacturer_code BETWEEN '" & wkFromManufacturerCode & "' AND '" & wkToManufacturerCode & "'"
    'sql &= "    AND ("
    'sql &= "         PACKING_COUNT != 0"
    'sql &= "      OR PACKING_WEIGHT != 0"
    'sql &= "      OR MEASURING_COUNT != 0"
    'sql &= "      OR MEASURING_WEIGHT != 0"
    'sql &= "     )"
    If lot1TextBox.Text <> "" Then
      sql &= " AND lot1 LIKE '%" & lot1TextBox.Text & "%'"
    End If

    If lot2TextBox.Text <> "" Then
      sql &= " AND lot2 LIKE '%" & lot2TextBox.Text & "%'"
    End If
    sql &= " ORDER BY "
    sql &= "     addition_date DESC, "
    sql &= "     addition_time DESC, "
    sql &= "     terminal_number "

    Call WriteExecuteLog(Me.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, sql)
    Return sql
  End Function

  Private Sub CloseButton_Click(sender As Object, e As EventArgs) Handles CloseButton.Click
    Close()
  End Sub

  Private Sub SearchButton_Click(sender As Object, e As EventArgs) Handles SearchButton.Click
    If Not CheckValue() Then
      Exit Sub
    End If

    SelectResults()
  End Sub

  Private Sub DateTimeFrom_KeyPress(sender As Object, e As KeyPressEventArgs) Handles DateTimeFrom.KeyPress
    If Not (Char.IsDigit(e.KeyChar) Or e.KeyChar = ControlChars.Back Or e.KeyChar = "/"c) Then
      e.Handled = True
    End If
  End Sub
  Private Sub DateTimeTo_KeyPress(sender As Object, e As KeyPressEventArgs) Handles DateTimeTo.KeyPress
    If Not (Char.IsDigit(e.KeyChar) Or e.KeyChar = ControlChars.Back Or e.KeyChar = "/"c) Then
      e.Handled = True
    End If
  End Sub

  Private Sub DateTimeFrom_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles DateTimeFrom.Validating
    If ActiveControl.Name <> "DateTimeFrom" And ActiveControl.Name <> "CloseButton" Then

      Dim inputText As String = DateTimeFrom.Text.Replace("/", "").Trim()

      If DateTypeCheck(inputText) Then
        DateTimeFrom.Text = DateTxt2DateTxt(inputText)
      Else
        MessageBox.Show("正しい日付形式を入力してください。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        DateTimeFrom.SelectAll()
        e.Cancel = True
      End If
    End If
  End Sub

  Private Sub DateTimeTo_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles DateTimeTo.Validating
    If ActiveControl.Name <> "DateTimeTo" And ActiveControl.Name <> "CloseButton" Then
      Dim inputText As String = DateTimeTo.Text.Replace("/", "").Trim()

      If DateTypeCheck(inputText) Then
        DateTimeTo.Text = DateTxt2DateTxt(inputText)
      Else
        MessageBox.Show("正しい日付形式を入力してください。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        DateTimeTo.SelectAll()
        e.Cancel = True
      End If
    End If
  End Sub
  Private Function CheckValue() As Boolean
    Dim dtFrom As DateTime
    Dim dtTo As DateTime
    Dim fromItemCode As String
    Dim toItemCode As String = ""
    Dim fromStaffCode As String
    Dim ToStaffCode As String = ""
    'Dim fromManufacturerCode As String
    'Dim toManufacturerCode As String = ""

    dtFrom = DateTime.Parse(DateTimeFrom.Text)
    dtTo = DateTime.Parse(DateTimeTo.Text)

    If FromItemCode_ComboBox.SelectedIndex = -1 Or FromItemCode_ComboBox.SelectedIndex = 0 Then
      fromItemCode = 1.ToString("D" & ItemDigits)
    Else
      fromItemCode = FromItemCode_ComboBox.Text.Substring(0, ItemDigits)
    End If

    If ToItemCode_ComboBox.SelectedIndex = -1 Or ToItemCode_ComboBox.SelectedIndex = 0 Then
      toItemCode = toItemCode.PadLeft(ItemDigits, "9"c)
    Else
      toItemCode = ToItemCode_ComboBox.Text.Substring(0, ItemDigits)
    End If

    If FromStaffCode_ComboBox.SelectedIndex = -1 Or FromStaffCode_ComboBox.SelectedIndex = 0 Then
      fromStaffCode = 1.ToString("D" & StaffDigits)
    Else
      fromStaffCode = FromStaffCode_ComboBox.Text.Substring(0, StaffDigits)
    End If

    If ToStaffCode_ComboBox.SelectedIndex = -1 Or ToStaffCode_ComboBox.SelectedIndex = 0 Then
      ToStaffCode = ToStaffCode.PadLeft(StaffDigits, "9"c)
    Else
      ToStaffCode = ToStaffCode_ComboBox.Text.Substring(0, StaffDigits)
    End If

    'If FromManuCode_ComboBox.SelectedIndex = -1 Or FromManuCode_ComboBox.SelectedIndex = 0 Then
    '  fromManufacturerCode = 1.ToString("D" & ManufacturerDigits)
    'Else
    '  fromManufacturerCode = FromManuCode_ComboBox.Text.Substring(0, ManufacturerDigits)
    'End If

    'If ToManuCode_ComboBox.SelectedIndex = -1 Or ToManuCode_ComboBox.SelectedIndex = 0 Then
    '  toManufacturerCode = toManufacturerCode.PadLeft(ManufacturerDigits, "9"c)
    'Else
    '  toManufacturerCode = ToManuCode_ComboBox.Text.Substring(0, ManufacturerDigits)
    'End If
    Dim CheckResult As Boolean = True

    '日付の相関チェック
    If dtFrom > dtTo Then
      MessageBox.Show("開始日は終了日より前の日付を指定してください。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
      CheckResult = False
    End If

    '呼出の相関チェック
    If fromItemCode > toItemCode Then
      MessageBox.Show("呼出コード(開始)は呼出コード(終了)より前のコードを指定してください。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
      CheckResult = False
    End If

    '担当者の相関チェック
    If fromStaffCode > ToStaffCode Then
      MessageBox.Show("担当者コード(開始)は担当者コード(終了)より前のコードを指定してください。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
      CheckResult = False
    End If

    ''製造者の相関チェック
    'If fromManufacturerCode > toManufacturerCode Then
    '  MessageBox.Show("製造者コード(開始)は製造者コード(終了)より前のコードを指定してください。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '  CheckResult = False
    'End If
    Return CheckResult
  End Function
  'Private Sub SetListData()
  '  '選択している行の行番号の取得
  '  Dim i As Integer = ResultDetail.CurrentRow.Index
  '  Form_ResultDetail.additionDateTxTValue = ResultDetail.Rows(i).Cells(1).Value
  '  Form_ResultDetail.additionTimeTxTValue = ResultDetail.Rows(i).Cells(2).Value
  '  Form_ResultDetail.terminalNumberTxTValue = ResultDetail.Rows(i).Cells(3).Value
  '  Form_ResultDetail.callCodeTxTValue = ResultDetail.Rows(i).Cells(4).Value
  '  Form_ResultDetail.itemNoTxTValue = ResultDetail.Rows(i).Cells(5).Value
  '  Form_ResultDetail.itemNameTxTValue = ResultDetail.Rows(i).Cells(6).Value

  '  Form_ResultDetail.packingTxTValue = ResultDetail.Rows(i).Cells(7).Value
  '  Form_ResultDetail.packingUnitTxTValue = ResultDetail.Rows(i).Cells(8).Value
  '  Form_ResultDetail.packing1WeightTxTValue = ResultDetail.Rows(i).Cells(9).Value
  '  Form_ResultDetail.packing1WeightUnitTxTValue = ResultDetail.Rows(i).Cells(10).Value
  '  Form_ResultDetail.packing2WeightTxTValue = ResultDetail.Rows(i).Cells(11).Value
  '  Form_ResultDetail.packing2WeightUnitTxTValue = ResultDetail.Rows(i).Cells(12).Value
  '  Form_ResultDetail.packing2MultiplicationTxTValue = ResultDetail.Rows(i).Cells(13).Value
  '  Form_ResultDetail.packing1NumberTxTValue = ResultDetail.Rows(i).Cells(14).Value
  '  Form_ResultDetail.packing1NameTxTValue = ResultDetail.Rows(i).Cells(15).Value
  '  Form_ResultDetail.packing2NumberTxTValue = ResultDetail.Rows(i).Cells(16).Value
  '  Form_ResultDetail.packing2NameTxTValue = ResultDetail.Rows(i).Cells(17).Value

  '  Form_ResultDetail.free1NumberTxTValue = ResultDetail.Rows(i).Cells(18).Value
  '  Form_ResultDetail.free1NameTxTValue = ResultDetail.Rows(i).Cells(19).Value
  '  Form_ResultDetail.free2NumberTxTValue = ResultDetail.Rows(i).Cells(20).Value
  '  Form_ResultDetail.free2NameTxTValue = ResultDetail.Rows(i).Cells(21).Value
  '  Form_ResultDetail.free3NumberTxTValue = ResultDetail.Rows(i).Cells(22).Value
  '  Form_ResultDetail.free3NameTxTValue = ResultDetail.Rows(i).Cells(23).Value
  '  Form_ResultDetail.free4NumberTxTValue = ResultDetail.Rows(i).Cells(24).Value
  '  Form_ResultDetail.free4NameTxTValue = ResultDetail.Rows(i).Cells(25).Value
  '  Form_ResultDetail.free5NumberTxTValue = ResultDetail.Rows(i).Cells(26).Value
  '  Form_ResultDetail.free5NameTxTValue = ResultDetail.Rows(i).Cells(27).Value

  '  Form_ResultDetail.manufacturerCodeTxTValue = ResultDetail.Rows(i).Cells(28).Value
  '  Form_ResultDetail.manufacturerNameTxTValue = ResultDetail.Rows(i).Cells(29).Value
  '  Form_ResultDetail.staffNumberTxTValue = ResultDetail.Rows(i).Cells(30).Value
  '  Form_ResultDetail.staffNameTxTValue = ResultDetail.Rows(i).Cells(31).Value
  '  Form_ResultDetail.lot1TxTValue = ResultDetail.Rows(i).Cells(32).Value
  '  Form_ResultDetail.lot2TxTValue = ResultDetail.Rows(i).Cells(33).Value
  '  Form_ResultDetail.classificationTxTValue = ResultDetail.Rows(i).Cells(34).Value

  '  Form_ResultDetail.weightTxTValue = ResultDetail.Rows(i).Cells(35).Value
  '  Form_ResultDetail.weightUnitTxTValue = ResultDetail.Rows(i).Cells(36).Value
  '  Form_ResultDetail.grossWeightTxTValue = ResultDetail.Rows(i).Cells(37).Value
  '  Form_ResultDetail.grossWeightUnitTxTValue = ResultDetail.Rows(i).Cells(38).Value
  '  Form_ResultDetail.temperatureTxTValue = ResultDetail.Rows(i).Cells(39).Value
  '  Form_ResultDetail.temperatureUnitTxTValue = ResultDetail.Rows(i).Cells(40).Value

  '  Form_ResultDetail.processingDateTxTValue = ResultDetail.Rows(i).Cells(41).Value
  '  Form_ResultDetail.processingTimeTxTValue = ResultDetail.Rows(i).Cells(42).Value
  '  Form_ResultDetail.effectiveDateTxTValue = ResultDetail.Rows(i).Cells(43).Value
  '  Form_ResultDetail.effectiveTimeTxTValue = ResultDetail.Rows(i).Cells(44).Value
  '  Form_ResultDetail.workOrderNumberTxTValue = ResultDetail.Rows(i).Cells(45).Value
  '  Form_ResultDetail.detailNumberTxTValue = ResultDetail.Rows(i).Cells(46).Value
  '  Form_ResultDetail.instructionQtyTxTValue = ResultDetail.Rows(i).Cells(47).Value
  '  Form_ResultDetail.actualQtyTxTValue = ResultDetail.Rows(i).Cells(48).Value
  '  Form_ResultDetail.workOrderNameTxTValue = ResultDetail.Rows(i).Cells(49).Value
  'End Sub

  Private Sub SetListData()
    Dim i As Integer = ResultDetail.CurrentRow.Index

    Form_ResultDetail.additionDateTxTValue = ResultDetail.Rows(i).Cells(0).Value
    Form_ResultDetail.additionTimeTxTValue = ResultDetail.Rows(i).Cells(1).Value
    Form_ResultDetail.terminalNumberTxTValue = ResultDetail.Rows(i).Cells(2).Value
    Form_ResultDetail.callCodeTxTValue = ResultDetail.Rows(i).Cells(3).Value
    Form_ResultDetail.itemNoTxTValue = ResultDetail.Rows(i).Cells(4).Value
    Form_ResultDetail.itemNameTxTValue = ResultDetail.Rows(i).Cells(5).Value
    'Form_ResultDetail.unitWeightTxTValue = ResultDetail.Rows(i).Cells(6).Value
    'Form_ResultDetail.unitWeightUnitTxTValue = ResultDetail.Rows(i).Cells(7).Value
    'Form_ResultDetail.quantityTxTValue = ResultDetail.Rows(i).Cells(8).Value
    Form_ResultDetail.packingTxTValue = ResultDetail.Rows(i).Cells(9).Value
    Form_ResultDetail.packingUnitTxTValue = ResultDetail.Rows(i).Cells(10).Value
    Form_ResultDetail.weightTxTValue = ResultDetail.Rows(i).Cells(11).Value
    Form_ResultDetail.weightUnitTxTValue = ResultDetail.Rows(i).Cells(12).Value
    Form_ResultDetail.manufacturerCodeTxTValue = ResultDetail.Rows(i).Cells(13).Value
    Form_ResultDetail.manufacturerNameTxTValue = ResultDetail.Rows(i).Cells(14).Value
    Form_ResultDetail.lot1TxTValue = ResultDetail.Rows(i).Cells(15).Value
    Form_ResultDetail.lot2TxTValue = ResultDetail.Rows(i).Cells(16).Value
    Form_ResultDetail.classificationTxTValue = ResultDetail.Rows(i).Cells(17).Value
    'Form_ResultDetail.upperWeightLimitTxTValue = ResultDetail.Rows(i).Cells(18).Value
    'Form_ResultDetail.upperWeightUnitTxTValue = ResultDetail.Rows(i).Cells(19).Value
    'Form_ResultDetail.lowerWeightLimitTxTValue = ResultDetail.Rows(i).Cells(20).Value
    'Form_ResultDetail.lowerWeightUnitTxTValue = ResultDetail.Rows(i).Cells(21).Value
    Form_ResultDetail.packing1WeightTxTValue = ResultDetail.Rows(i).Cells(22).Value
    Form_ResultDetail.packing1WeightUnitTxTValue = ResultDetail.Rows(i).Cells(23).Value
    Form_ResultDetail.packing2WeightTxTValue = ResultDetail.Rows(i).Cells(24).Value
    Form_ResultDetail.packing2WeightUnitTxTValue = ResultDetail.Rows(i).Cells(25).Value
    Form_ResultDetail.packing2MultiplicationTxTValue = ResultDetail.Rows(i).Cells(26).Value
    Form_ResultDetail.packing1NumberTxTValue = ResultDetail.Rows(i).Cells(27).Value
    Form_ResultDetail.packing1NameTxTValue = ResultDetail.Rows(i).Cells(28).Value
    Form_ResultDetail.packing2NumberTxTValue = ResultDetail.Rows(i).Cells(29).Value
    Form_ResultDetail.packing2NameTxTValue = ResultDetail.Rows(i).Cells(30).Value
    Form_ResultDetail.staffNumberTxTValue = ResultDetail.Rows(i).Cells(31).Value
    Form_ResultDetail.staffNameTxTValue = ResultDetail.Rows(i).Cells(32).Value
    Form_ResultDetail.free1NumberTxTValue = ResultDetail.Rows(i).Cells(33).Value
    Form_ResultDetail.free1NameTxTValue = ResultDetail.Rows(i).Cells(34).Value
    Form_ResultDetail.free2NumberTxTValue = ResultDetail.Rows(i).Cells(35).Value
    Form_ResultDetail.free2NameTxTValue = ResultDetail.Rows(i).Cells(36).Value
    Form_ResultDetail.free3NumberTxTValue = ResultDetail.Rows(i).Cells(37).Value
    Form_ResultDetail.free3NameTxTValue = ResultDetail.Rows(i).Cells(38).Value
    Form_ResultDetail.free4NumberTxTValue = ResultDetail.Rows(i).Cells(39).Value
    Form_ResultDetail.free4NameTxTValue = ResultDetail.Rows(i).Cells(40).Value
    Form_ResultDetail.free5NumberTxTValue = ResultDetail.Rows(i).Cells(41).Value
    Form_ResultDetail.free5NameTxTValue = ResultDetail.Rows(i).Cells(42).Value
    Form_ResultDetail.processingDateTxTValue = ResultDetail.Rows(i).Cells(43).Value
    Form_ResultDetail.processingTimeTxTValue = ResultDetail.Rows(i).Cells(44).Value
    Form_ResultDetail.effectiveDateTxTValue = ResultDetail.Rows(i).Cells(45).Value
    Form_ResultDetail.effectiveTimeTxTValue = ResultDetail.Rows(i).Cells(46).Value
    Form_ResultDetail.workOrderNumberTxTValue = ResultDetail.Rows(i).Cells(47).Value
    Form_ResultDetail.detailNumberTxTValue = ResultDetail.Rows(i).Cells(48).Value
    Form_ResultDetail.instructionQtyTxTValue = ResultDetail.Rows(i).Cells(49).Value
    Form_ResultDetail.actualQtyTxTValue = ResultDetail.Rows(i).Cells(50).Value
    Form_ResultDetail.workOrderNameTxTValue = ResultDetail.Rows(i).Cells(51).Value
  End Sub


  Private Sub CsvExportButton_Click(sender As Object, e As EventArgs) Handles CsvExportButton.Click
    If ResultDetail.Rows.Count = 0 Then
      MessageBox.Show("CSVファイルを出力するデータがありません。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
      Exit Sub
    End If

    Dim fileName As String = ResultCsvPath & "Results" & ".CSV"

    Try
      ExportToCSV(ResultDetail, fileName, ResultCsvPath)
    Catch ex As Exception
      MessageBox.Show("エラーが発生しました: " & ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
    End Try
  End Sub
  Private Sub ExportToCSV(dgv As DataGridView, baseFileName As String, filePath As String)
    ' タイムスタンプ付きファイル名作成（例：Results_20250606114654.csv）
    Dim timestamp As String = DateTime.Now.ToString("yyyyMMddHHmmss")
    Dim fullFileName As String = Path.Combine(filePath, baseFileName & "_" & timestamp & ".csv")

    ' ファイルに書き込み
    Using writer As New StreamWriter(fullFileName, False, System.Text.Encoding.UTF8)
      ' ヘッダー書き込み
      For i As Integer = 0 To dgv.Columns.Count - 1
        writer.Write(dgv.Columns(i).HeaderText)
        If i < dgv.Columns.Count - 1 Then writer.Write(",")
      Next
      writer.WriteLine()

      ' データ行書き込み
      For Each row As DataGridViewRow In dgv.Rows
        If Not row.IsNewRow Then
          For i As Integer = 0 To row.Cells.Count - 1
            writer.Write(row.Cells(i).Value?.ToString())
            If i < row.Cells.Count - 1 Then writer.Write(",")
          Next
          writer.WriteLine()
        End If
      Next
    End Using

    ' メッセージ表示
    MessageBox.Show("CSVファイルの出力が完了しました。", "完了", MessageBoxButtons.OK, MessageBoxIcon.Information)

    ' フォルダを開く
    Process.Start("explorer.exe", Path.GetDirectoryName(fullFileName))
  End Sub

  Private Sub ReleaseObject(ByVal obj As Object)
    Try
      Marshal.ReleaseComObject(obj)
      obj = Nothing
    Catch ex As Exception
      obj = Nothing
    Finally
      GC.Collect()
    End Try
  End Sub
  Private Sub DeleteButton_Click(sender As Object, e As EventArgs) Handles DeleteButton.Click
    Dim message As String = "削除します。" & vbCrLf & "よろしいでしょうか。"

    Dim resultS As DialogResult = MessageBox.Show(message, "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

    If resultS = DialogResult.Yes Then
      DeleteTRNResults()
    Else
      MessageBox.Show("処理がキャンセルされました。", "キャンセル", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
      Exit Sub
    End If
  End Sub
  Private Sub DeleteTRNResults()
    Dim sql As String = String.Empty
    With tmpDb
      Try
        sql = GetDeleteSql()
        If .Execute(sql) <> 0 Then
          .TrnCommit()
          MessageBox.Show("削除処理完了しました。", "完了", MessageBoxButtons.OK, MessageBoxIcon.Information)
          ' 一覧画面データ更新
          SelectResults()
        End If
      Catch ex As Exception
        Call ComWriteErrLog([GetType]().Name,
                                      System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message)
        Throw New Exception(ex.Message)
      End Try
    End With
  End Sub
  Private Function GetDeleteSql() As String
    Dim sql As String = String.Empty

    Dim wkAdditionDate As String = ResultDetail.Rows(ResultDetail.CurrentRow.Index).Cells(0).Value
    Dim wkAdditionTime As String = ResultDetail.Rows(ResultDetail.CurrentRow.Index).Cells(1).Value
    Dim wkTerminalNumber As String = ResultDetail.Rows(ResultDetail.CurrentRow.Index).Cells(2).Value
    Dim wkCallCode As String = ResultDetail.Rows(ResultDetail.CurrentRow.Index).Cells(3).Value

    sql &= " DELETE "
    sql &= " FROM "
    sql &= "     TRN_Results "
    sql &= " WHERE "
    sql &= "     addition_date = '" & wkAdditionDate & "'"
    sql &= " AND addition_time = '" & wkAdditionTime & "'"
    sql &= " AND terminal_number = '" & wkTerminalNumber & "'"
    sql &= " AND call_code = '" & wkCallCode & "'"

    Call WriteExecuteLog([GetType]().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, sql)
    Return sql
  End Function

  Private Sub ResultDetail_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles ResultDetail.CellDoubleClick
    '詳細画面の項目値セット
    SetListData()
    Form_ResultDetail.InputMode = 2
    Form_ResultDetail.ShowDialog()
  End Sub

  Private Sub Form_ResultList_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
    Select Case e.KeyCode
      Case Keys.F1
        SearchButton.PerformClick()
      Case Keys.F2
        CsvExportButton.PerformClick()
      Case Keys.F5
        CreateButton.PerformClick()
      Case Keys.F6
        UpdateButton.PerformClick()
      Case Keys.F7
        DeleteButton.PerformClick()
      Case Keys.Escape
        Me.Close()
    End Select
  End Sub

  Private Sub FromItemCode_ComboBox_DropDown(sender As Object, e As EventArgs) Handles FromItemCode_ComboBox.DropDown
    AdjustDropDownWidth(FromItemCode_ComboBox)
  End Sub

  Private Sub ToItemCode_ComboBox_DropDown(sender As Object, e As EventArgs) Handles ToItemCode_ComboBox.DropDown
    AdjustDropDownWidth(ToItemCode_ComboBox)
  End Sub

  Private Sub FromStaffCode_ComboBox_DropDown(sender As Object, e As EventArgs) Handles FromStaffCode_ComboBox.DropDown
    AdjustDropDownWidth(FromStaffCode_ComboBox)
  End Sub

  Private Sub ToStaffCode_ComboBox_DropDown(sender As Object, e As EventArgs) Handles ToStaffCode_ComboBox.DropDown
    AdjustDropDownWidth(ToStaffCode_ComboBox)
  End Sub
  Private Sub AdjustDropDownWidth(cb As ComboBox)
    Dim maxItemWidth As Integer = 0
    Using g As Graphics = cb.CreateGraphics()
      Dim font As Font = cb.Font
      For Each item In cb.Items
        Dim itemWidth As Integer = CInt(g.MeasureString(item.ToString(), font).Width)
        If itemWidth > maxItemWidth Then
          maxItemWidth = itemWidth
        End If
      Next
    End Using

    ' 現在の幅より狭いときだけ拡張する（）
    If maxItemWidth > cb.Width Then
      cb.DropDownWidth = maxItemWidth + 10
    Else
      cb.DropDownWidth = cb.Width
    End If
  End Sub
End Class
