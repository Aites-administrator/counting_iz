Imports Common
Imports Common.ClsFunction
Imports Excel = Microsoft.Office.Interop.Excel
Imports System.Runtime.InteropServices
Imports System.IO

Public Class Form_WorkOrderList
  '入力モード
  '新規：1
  '修正：2
  ReadOnly InputMode As Integer
  ReadOnly tmpDb As New ClsSqlServer
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
  Private Sub Form_WorkOrderMaster_Load(sender As Object, e As EventArgs) Handles MyBase.Load
    MaximizeBox = False
    Dim updateTime As DateTime = System.IO.File.GetLastWriteTime(System.Reflection.Assembly.GetExecutingAssembly().Location)
    Text = "作業指示マスタ一覧" & " ( " & updateTime & " ) "
    Me.KeyPreview = True
    FormBorderStyle = FormBorderStyle.FixedSingle
    WorkOrderDetail.RowHeadersVisible = False
    MaximizeBox = False

    'ユーザーからのデータ追加を不可にしておく
    WorkOrderDetail.AllowUserToAddRows = False
    WorkOrderDetail.ColumnCount = 9
    WorkOrderDetail.Columns(0).HeaderText = "作業指示№"
    WorkOrderDetail.Columns(1).HeaderText = "明細№"
    WorkOrderDetail.Columns(2).HeaderText = "作業指示名称"
    WorkOrderDetail.Columns(3).HeaderText = "明細小計"
    WorkOrderDetail.Columns(4).HeaderText = "商品№"
    WorkOrderDetail.Columns(5).HeaderText = "商品名"
    WorkOrderDetail.Columns(6).HeaderText = "指示数"
    WorkOrderDetail.Columns(7).HeaderText = "登録日"
    WorkOrderDetail.Columns(8).HeaderText = "更新日"


    'ヘッダーの整列設定
    For i As Integer = 0 To 8
      WorkOrderDetail.Columns(i).DefaultCellStyle.Alignment =
   DataGridViewContentAlignment.MiddleCenter
      WorkOrderDetail.Columns(i).HeaderCell.Style.Alignment =
  DataGridViewContentAlignment.MiddleCenter
    Next

    SelectWorkOrderMaster()

    ''マルチ選択不可
    'WorkOrderDetail.MultiSelect = False

    '選択モード設定(全カラム)
    WorkOrderDetail.SelectionMode = DataGridViewSelectionMode.FullRowSelect

    '検索結果が存在する場合、先頭行選択
    If WorkOrderDetail.Rows.Count > 0 Then
      WorkOrderDetail.CurrentCell = WorkOrderDetail.Rows(0).Cells(0)
      WorkOrderDetail.Rows(0).Selected = True
    End If

    CustomizeDataGridViewHeader() ' ヘッダーのデザイン変更()
  End Sub
  ' DataGridView のヘッダーのデザインを変更
  Private Sub CustomizeDataGridViewHeader()
    With WorkOrderDetail
      ' ヘッダーの背景色を変更
      .EnableHeadersVisualStyles = False ' デフォルトの Windows スタイルを無効化
      .ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue ' ヘッダーの背景色
      .ColumnHeadersDefaultCellStyle.ForeColor = Color.Black ' ヘッダーの文字色
      .ColumnHeadersDefaultCellStyle.Font = New Font("Meiryo", 10, FontStyle.Bold) ' フォント変更
      .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter ' ヘッダー中央寄せ
    End With
  End Sub
  Private Sub CreateButton_Click(sender As Object, e As EventArgs) Handles CreateButton.Click
    Form_WorkOrderDetail.InputMode = 1
    Form_WorkOrderDetail.ShowDialog()
  End Sub
  Private Sub UpdateButton_Click(sender As Object, e As EventArgs) Handles UpdateButton.Click
    '詳細画面の項目値セット
    SetListData()
    Form_WorkOrderDetail.InputMode = 2
    Form_WorkOrderDetail.ShowDialog()
  End Sub
  Private Sub DeleteButton_Click(sender As Object, e As EventArgs) Handles DeleteButton.Click
    DeleteWorkOrderMaster()
  End Sub
  Private Sub WorkOrderDetail_DoubleClick(sender As Object, e As EventArgs) Handles WorkOrderDetail.DoubleClick
    '詳細画面の項目値セット
    SetListData()
    Form_WorkOrderDetail.InputMode = 2
    Form_WorkOrderDetail.ShowDialog()
  End Sub
  Private Sub CloseButton_Click(sender As Object, e As EventArgs) Handles CloseButton.Click
    Close()
  End Sub

  Private Sub SetListData()
    '選択している行の行番号の取得
    Dim i As Integer = WorkOrderDetail.CurrentRow.Index
    Form_WorkOrderDetail.WorkOrderIDTextValue = WorkOrderDetail.Rows(i).Cells(0).Value
    Form_WorkOrderDetail.DetailIDTextValue = WorkOrderDetail.Rows(i).Cells(1).Value
    Form_WorkOrderDetail.WorkOrderWorkOrderNameTextValue = WorkOrderDetail.Rows(i).Cells(2).Value
    Form_WorkOrderDetail.IsDetailSubtotalComboValue = WorkOrderDetail.Rows(i).Cells(3).Value
    Form_WorkOrderDetail.ProductIDTextValue = WorkOrderDetail.Rows(i).Cells(4).Value & " " & WorkOrderDetail.Rows(i).Cells(5).Value
    Form_WorkOrderDetail.OrderQuantityTextValue = WorkOrderDetail.Rows(i).Cells(6).Value
  End Sub

  Public Sub SelectWorkOrderMaster()
    Dim sql As String = String.Empty
    sql = GetAllSelectSql()
    Try
      With tmpDb
        SqlServer.GetResult(tmpDt, sql)
        If tmpDt.Rows.Count = 0 Then
          MessageBox.Show("作業指示マスタにデータが登録されていません。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Else
          WriteDetail(tmpDt, WorkOrderDetail)
        End If

        WorkOrderDetail.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells)
        WorkOrderDetail.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False
        WorkOrderDetail.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        WorkOrderDetail.ColumnHeadersHeight = 20

      End With
    Catch ex As Exception
      Call ComWriteErrLog([GetType]().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message)
      Throw New Exception(ex.Message)
    Finally
      tmpDt.Dispose()
    End Try
  End Sub

  Private Function GetAllSelectSql() As String

    Dim sql As String = String.Empty

    sql &= " SELECT"
    sql &= "     wo.WorkOrderID,"
    sql &= "     wo.DetailID,"
    sql &= "     wo.WorkOrderName,"
    sql &= "     CASE WHEN wo.IsDetailSubtotal = 1 THEN N'する' ELSE N'しない' END AS DetailSubtotalDisplay,"
    sql &= "     wo.ProductID,"
    sql &= "     mi.item_name,"
    sql &= "     wo.OrderQuantity,"
    sql &= "     wo.CreateDate,"
    sql &= "     wo.UpdateDate"
    sql &= " FROM"
    sql &= "     MST_WorkOrder wo"
    sql &= " INNER JOIN"
    sql &= "     MST_Item mi"
    sql &= "     ON wo.ProductID = mi.call_code"
    sql &= " ORDER BY"
    sql &= "     wo.WorkOrderID"

    Call WriteExecuteLog([GetType]().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, sql)
    Return sql

  End Function

  Private Sub DeleteWorkOrderMaster()
    Dim confirmation As DialogResult = MessageBox.Show("選択された行を削除します。よろしいですか？", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
    If confirmation <> DialogResult.Yes Then Exit Sub

    If WorkOrderDetail.SelectedRows.Count = 0 Then
      MessageBox.Show("削除する行を選択してください。", "情報", MessageBoxButtons.OK, MessageBoxIcon.Information)
      Return
    End If

    Try
      For Each row As DataGridViewRow In WorkOrderDetail.SelectedRows
        If Not row.IsNewRow Then
          Dim WorkOrderID As String = row.Cells(0).Value.ToString()
          Dim DetailID As String = row.Cells(1).Value.ToString()
          Dim sql As String = GetDeleteSql(WorkOrderID, DetailID)

          If tmpDb.Execute(sql) <> 1 Then
            MessageBox.Show("一部の削除に失敗しました。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
          End If
        End If
      Next

      tmpDb.TrnCommit()
      MessageBox.Show("削除処理が完了しました。", "完了", MessageBoxButtons.OK, MessageBoxIcon.Information)
      SelectWorkOrderMaster()
      RefreshText()

    Catch ex As Exception
      Call ComWriteErrLog([GetType]().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message)
      Throw
    End Try
  End Sub

  Private Function GetDeleteSql(WorkOrderID As String, DetailID As String) As String
    Dim sql As String = String.Empty

    sql &= " DELETE"
    sql &= " FROM"
    sql &= "     MST_WorkOrder"
    sql &= " WHERE"
    sql &= "     WorkOrderID = '" & WorkOrderID & "' "
    sql &= "     AND DetailID = '" & DetailID & "' "

    Call WriteExecuteLog([GetType]().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, sql)
    Return sql
  End Function


  Private Sub RefreshText()
    For i As Integer = 0 To WorkOrderDetail.Rows.Count - 1
      WorkOrderDetail.Rows(i).Selected = True
      WorkOrderDetail.FirstDisplayedScrollingRowIndex = i
      WorkOrderDetail.CurrentCell = WorkOrderDetail.Rows(i).Cells(0)
      Exit For
    Next
  End Sub

  Private Sub Form_WorkOrderList_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
    Select Case e.KeyCode
      Case Keys.F5
        CreateButton.PerformClick()
      Case Keys.F6
        UpdateButton.PerformClick()
      Case Keys.F7
        DeleteButton.PerformClick()
      Case Keys.F8
        AllDeleteButton.PerformClick()
      Case Keys.F8
        ExportButton.PerformClick()
      Case Keys.Escape
        Me.Close()
    End Select
  End Sub

  Private Sub AllDeleteButton_Click(sender As Object, e As EventArgs) Handles AllDeleteButton.Click
    AllDeleteWorkOrderMaster()
  End Sub

  Private Sub AllDeleteWorkOrderMaster()
    Dim sql As String = String.Empty
    Dim rowSelectionCode As String = String.Empty
    Dim confirmation As String
    Dim msg1 As String
    Dim msg2 As String
    With tmpDb
      Try
        sql = GetAllDeleteSql(True)
        msg1 = "【全件】削除します。" & vbCrLf & "よろしいでしょうか。"
        msg2 = "削除処理完了しました。"

        confirmation = MessageBox.Show(msg1, "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If confirmation = DialogResult.Yes Then
          ' SQL実行結果が1件か？
          If .Execute(sql) = 1 Then
            ' 更新成功
            .TrnCommit()
            MessageBox.Show(msg2, "完了", MessageBoxButtons.OK, MessageBoxIcon.Information)
            SelectWorkOrderMaster()
            RefreshText()
          Else
            ' 削除失敗
            MessageBox.Show("作業指示マスタの削除に失敗しました。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
          End If
        Else
          Exit Sub
        End If
      Catch ex As Exception
        Call ComWriteErrLog([GetType]().Name,
                      System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message)
        Throw New Exception(ex.Message)
      End Try
    End With
  End Sub
  Private Function GetAllDeleteSql(DeleteFlg As Boolean) As String
    Dim sql As String = String.Empty

    sql &= " DELETE"
    sql &= " FROM"
    sql &= "     MST_WorkOrder"

    Call WriteExecuteLog([GetType]().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, sql)
    Return sql
  End Function

  Private Sub WorkOrderDetail_KeyDown(sender As Object, e As KeyEventArgs) Handles WorkOrderDetail.KeyDown
    If e.KeyCode = Keys.Delete Then
      DeleteWorkOrderMaster()
      e.Handled = True
    End If
  End Sub

  Private Sub ExportButton_Click(sender As Object, e As EventArgs) Handles ExportButton.Click
    Try
      ExportPickingList()
    Catch ex As Exception
      MessageBox.Show("出力エラー: " & ex.Message)
    End Try
  End Sub
  Private Sub ExportPickingList()
    Dim xlApp As New Excel.Application
    Dim xlBook As Excel.Workbook = xlApp.Workbooks.Add()
    Dim xlSheet As Excel.Worksheet = CType(xlBook.Sheets(1), Excel.Worksheet)

    ' 出力時間（G2: ラベル、H2: 日時）
    xlSheet.Range("G2").Value = "出力時間"
    xlSheet.Range("H2").Value = Now.ToString("yyyy/MM/dd HH:mm")
    With xlSheet.Range("G2:H2")
      .Font.Name = "メイリオ"
      .Font.Size = 20
      .Font.Bold = True
      .HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
      .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
    End With

    ' タイトル（A3:H3）
    With xlSheet.Range("A3:H3")
      .Merge()
      .Value = "ピッキングリスト"
      .Font.Name = "メイリオ"
      .Font.Size = 32
      .Font.Bold = True
      .Font.Color = RGB(0, 51, 153)
      .HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
      .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
      .Interior.Color = RGB(200, 220, 255)
      .Borders.LineStyle = Excel.XlLineStyle.xlContinuous
      .RowHeight = 45
    End With

    ' ヘッダー（A6:H6）
    Dim headers As String() = {"バーコード", "作業指示No.", "明細No.", "作業指示名称", "商品No.", "商品名", "指示数", "チェック"}
    Dim startRow As Integer = 6
    For i As Integer = 0 To headers.Length - 1
      xlSheet.Cells(startRow, i + 1).Value = headers(i)
    Next
    With xlSheet.Range("A6:H6")
      .Font.Name = "メイリオ"
      .Font.Size = 24
      .Font.Bold = True
      .HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
      .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
      .Interior.Color = RGB(180, 210, 250)
      .Borders.LineStyle = Excel.XlLineStyle.xlContinuous
      .RowHeight = 40
    End With

    ' 明細行
    Dim rowIndex As Integer = 7
    For i As Integer = 0 To WorkOrderDetail.Rows.Count - 1
      Dim row As DataGridViewRow = WorkOrderDetail.Rows(i)
      If row.IsNewRow Then Continue For

      Dim wo As String = row.Cells(0).Value?.ToString()
      Dim dtl As String = row.Cells(1).Value?.ToString()

      ' バーコード（A列）
      With xlSheet.Cells(rowIndex, 1)
        .Value = "*" & wo & dtl & "*"
        .NumberFormat = "@"
        .Font.Name = "Code39"
        .Font.Size = 36
        .Font.Bold = True
        .HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
        .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
      End With

      ' B～H列の内容
      xlSheet.Cells(rowIndex, 2).Value = wo
      xlSheet.Cells(rowIndex, 3).Value = dtl
      xlSheet.Cells(rowIndex, 4).Value = row.Cells(2).Value
      xlSheet.Cells(rowIndex, 5).Value = row.Cells(4).Value
      xlSheet.Cells(rowIndex, 6).Value = row.Cells(5).Value
      xlSheet.Cells(rowIndex, 7).Value = row.Cells(6).Value
      xlSheet.Cells(rowIndex, 8).Value = "☐"

      For col As Integer = 2 To 8
        With xlSheet.Cells(rowIndex, col)
          .Font.Name = "メイリオ"
          .Font.Size = 24
          .HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
          .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
        End With
      Next

      With xlSheet.Range("A" & rowIndex & ":H" & rowIndex)
        .RowHeight = 80
        .Borders.LineStyle = Excel.XlLineStyle.xlContinuous
        If i Mod 2 = 0 Then .Interior.Color = RGB(245, 250, 255)
      End With

      rowIndex += 1
    Next

    ' 列幅自動調整 & 最小幅調整
    xlSheet.Columns("A:H").AutoFit()
    Dim SetMinWidth As Action(Of String, Double) =
        Sub(col, minW)
          If xlSheet.Columns(col).ColumnWidth < minW Then
            xlSheet.Columns(col).ColumnWidth = minW
          End If
        End Sub
    SetMinWidth("A", 28)
    SetMinWidth("B", 16)
    SetMinWidth("C", 12)
    SetMinWidth("D", 30)
    SetMinWidth("E", 16)
    SetMinWidth("F", 26)
    SetMinWidth("G", 10)
    SetMinWidth("H", 10)

    ' 印刷設定
    With xlSheet.PageSetup
      .Orientation = Excel.XlPageOrientation.xlLandscape
      .Zoom = 50
    End With

    ' グリッド線非表示
    xlApp.ActiveWindow.DisplayGridlines = False

    ' 保存パス作成（文字列補間ではなく Format 使用）
    Dim dirPath As String = "C:\COUNTING_DX\PICKING_LIST"
    If Not Directory.Exists(dirPath) Then Directory.CreateDirectory(dirPath)
    Dim fileName As String = String.Format("ピッキングリスト_{0}.xlsx", Now.ToString("yyMMddHHmmss"))
    Dim filePath As String = Path.Combine(dirPath, fileName)

    ' 保存
    xlBook.SaveAs(filePath)
    xlBook.Close(False)
    xlApp.Quit()

    ' リソース解放
    Marshal.ReleaseComObject(xlSheet)
    Marshal.ReleaseComObject(xlBook)
    Marshal.ReleaseComObject(xlApp)

    ' フォルダを開く
    Process.Start("explorer.exe", "/select," & filePath)

    ' 出力完了メッセージ
    MessageBox.Show("ピッキングリストを出力しました。", "出力完了", MessageBoxButtons.OK, MessageBoxIcon.Information)
  End Sub



End Class