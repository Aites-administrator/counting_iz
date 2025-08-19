Imports Common
Imports Common.ClsFunction
Public Class Form_ItemList
  Private CheckboxExistFlg As New Boolean
  ReadOnly tmpDb As New ClsSqlServer
  ReadOnly tmpDt As New DataTable
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

  Private Sub Form_ItemList_Load(sender As Object, e As EventArgs) Handles MyBase.Load
    MaximizeBox = False
    Dim updateTime As DateTime = System.IO.File.GetLastWriteTime(System.Reflection.Assembly.GetExecutingAssembly().Location)
    Text = "商品マスタ一覧" & " ( " & updateTime & " ) "

    Me.KeyPreview = True

    ItemDetail.RowHeadersVisible = False
    FormBorderStyle = FormBorderStyle.FixedSingle
    ItemDetail.AllowUserToAddRows = False

    ItemDetail.ColumnCount = 19
    ' 残りのヘッダーテキストを設定
    ItemDetail.Columns(0).HeaderText = "呼出コード"
    ItemDetail.Columns(1).HeaderText = "品番"
    ItemDetail.Columns(2).HeaderText = "品名"
    ItemDetail.Columns(3).HeaderText = "単重"
    ItemDetail.Columns(4).HeaderText = "単重単位"
    ItemDetail.Columns(5).HeaderText = "安全計数"
    ItemDetail.Columns(6).HeaderText = "目標個数"
    ItemDetail.Columns(7).HeaderText = "風袋"
    ItemDetail.Columns(8).HeaderText = "風袋単位"
    ItemDetail.Columns(9).HeaderText = "上限値"
    ItemDetail.Columns(10).HeaderText = "基準値"
    ItemDetail.Columns(11).HeaderText = "下限値"
    ItemDetail.Columns(12).HeaderText = "小計目標値"
    ItemDetail.Columns(13).HeaderText = "小計目標回数"
    ItemDetail.Columns(14).HeaderText = "単重上限値"
    ItemDetail.Columns(15).HeaderText = "単重上限値単位"
    ItemDetail.Columns(16).HeaderText = "単重下限値"
    ItemDetail.Columns(17).HeaderText = "単重下限値単位"
    ItemDetail.Columns(18).HeaderText = "風袋情報"

    ' 削除フラグの列を非表示にする
    ItemDetail.Columns(5).Visible = False
    ItemDetail.Columns(7).Visible = False
    ItemDetail.Columns(8).Visible = False
    ItemDetail.Columns(9).Visible = False
    ItemDetail.Columns(10).Visible = False
    ItemDetail.Columns(11).Visible = False
    ItemDetail.Columns(12).Visible = False
    ItemDetail.Columns(13).Visible = False
    ItemDetail.Columns(14).Visible = False
    ItemDetail.Columns(15).Visible = False
    ItemDetail.Columns(16).Visible = False
    ItemDetail.Columns(17).Visible = False
    ItemDetail.Columns(18).Visible = False

    'カラムの幅指定
    ItemDetail.Columns(0).Width = 120
    ItemDetail.Columns(1).Width = 100
    ItemDetail.Columns(2).Width = 250
    ItemDetail.Columns(3).Width = 100
    ItemDetail.Columns(4).Width = 100
    ItemDetail.Columns(6).Width = 100

    'カラムの整列設定
    For i As Integer = 0 To 18
      ItemDetail.Columns(i).DefaultCellStyle.Alignment =
      DataGridViewContentAlignment.MiddleCenter
    Next

    'ヘッダーの整列設定
    For i As Integer = 0 To 18
      ItemDetail.Columns(i).HeaderCell.Style.Alignment =
      DataGridViewContentAlignment.MiddleCenter
    Next

    SelectItemMaster()

    ' 選択モードを全カラム選択に設定
    ItemDetail.SelectionMode = DataGridViewSelectionMode.FullRowSelect
    If ItemDetail.Rows.Count > 0 Then
      ItemDetail.CurrentCell = ItemDetail.Rows(0).Cells(0)
      ItemDetail.Rows(0).Selected = True
    End If

    CustomizeDataGridViewHeader() ' ヘッダーのデザイン変更

  End Sub
  ' DataGridView のヘッダーのデザインを変更
  Private Sub CustomizeDataGridViewHeader()
    With ItemDetail
      ' ヘッダーの背景色を変更
      .EnableHeadersVisualStyles = False ' デフォルトの Windows スタイルを無効化
      .ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue ' ヘッダーの背景色
      .ColumnHeadersDefaultCellStyle.ForeColor = Color.Black ' ヘッダーの文字色
      .ColumnHeadersDefaultCellStyle.Font = New Font("Meiryo", 10, FontStyle.Bold) ' フォント変更
      .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter ' ヘッダー中央寄せ
    End With
  End Sub
  Public Sub SelectItemMaster()
    Dim sql As String = String.Empty
    sql = GetAllSelectSql()
    Try
      With tmpDb
        SqlServer.GetResult(tmpDt, sql)

        If tmpDt.Rows.Count = 0 Then
          MessageBox.Show("商品マスタにデータが登録されていません。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Else
          WriteDetail(tmpDt, ItemDetail)
          UpdateButton.Enabled = True
          DeleteButton.Enabled = True
        End If
      End With
    Catch ex As Exception
      Call ComWriteErrLog([GetType]().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message)
      Throw New Exception(ex.Message)
    Finally
      tmpDt.Dispose()
    End Try

    'DisPlayDeleteRow(AllRowDisplayFlg)

    '検索結果が存在する場合、先頭行選択
    If ItemDetail.Rows.Count > 0 Then
      ItemDetail.CurrentCell = ItemDetail.Rows(0).Cells(0)
      ItemDetail.Rows(0).Selected = True
    End If
  End Sub

  Private Function GetAllSelectSql() As String

    Dim sql As String = String.Empty

    sql &= " SELECT"
    sql &= "     I.call_code,"
    sql &= "     I.item_number,"
    sql &= "     I.item_name,"
    sql &= "     I.unit_weight,"
    sql &= "     I.unit_weight_unit,"
    sql &= "     I.safety_factor,"
    sql &= "     I.target_qty,"
    sql &= "     P.PackingWeight,"
    sql &= "     P.PackingWeightUnit,"
    sql &= "     I.upper_limit,"
    sql &= "     I.standard_value,"
    sql &= "     I.lower_limit,"
    sql &= "     I.subtotal_target_qty,"
    sql &= "     I.subtotal_target_cnt,"
    sql &= "     I.upper_unit_weight,"
    sql &= "     I.upper_unit_weight_unit,"
    sql &= "     I.lower_unit_weight,"
    sql &= "     I.lower_unit_weight_unit,"
    sql &= "     CONVERT(VARCHAR, P.PackingNo) + ' ' + P.PackingName AS DisplayText"
    sql &= " FROM"
    sql &= "     MST_Item I"
    sql &= "     LEFT JOIN MST_Packing P ON I.packing_code = P.PackingNo"
    sql &= " ORDER BY"
    sql &= "     I.call_code"

    Call WriteExecuteLog([GetType]().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, sql)
    Return sql
  End Function

  Private Sub CreateButton_Click(sender As Object, e As EventArgs) Handles CreateButton.Click
    Form_ItemDetail.InputMode = 1
    Form_ItemDetail.ShowDialog()
  End Sub
  Private Sub UpdateButton_Click(sender As Object, e As EventArgs) Handles UpdateButton.Click
    '詳細画面の項目値セット
    SetListData()
    Form_ItemDetail.InputMode = 2
    Form_ItemDetail.ShowDialog()
  End Sub
  Private Sub SetListData()
    ' 選択している行の行番号の取得
    Dim i As Integer = ItemDetail.CurrentRow.Index

    Form_ItemDetail.codeTextValue = ItemDetail.Rows(i).Cells(0).Value.ToString()
    Form_ItemDetail.itemCodeTextValue = ItemDetail.Rows(i).Cells(1).Value.ToString()
    Form_ItemDetail.itemNameTextValue = ItemDetail.Rows(i).Cells(2).Value.ToString()

    Form_ItemDetail.weightTextValue = Convert.ToDecimal(ItemDetail.Rows(i).Cells(3).Value)
    Form_ItemDetail.weightUnitComboValue = ItemDetail.Rows(i).Cells(4).Value.ToString()

    Form_ItemDetail.safetyFactorTextValue = Convert.ToDecimal(ItemDetail.Rows(i).Cells(5).Value)
    Form_ItemDetail.targetQtyTextValue = Convert.ToDecimal(ItemDetail.Rows(i).Cells(6).Value)

    Form_ItemDetail.tareComboValue = ItemDetail.Rows(i).Cells(18).Value.ToString()
    'Form_ItemDetail.tareUnitComboValue = ItemDetail.Rows(i).Cells(8).Value.ToString()

    Form_ItemDetail.maxTextValue = Convert.ToDecimal(ItemDetail.Rows(i).Cells(9).Value)
    Form_ItemDetail.standardTextValue = Convert.ToDecimal(ItemDetail.Rows(i).Cells(10).Value)
    Form_ItemDetail.minTextValue = Convert.ToDecimal(ItemDetail.Rows(i).Cells(11).Value)

    Form_ItemDetail.subtotalTargetTextValue = Convert.ToDecimal(ItemDetail.Rows(i).Cells(12).Value)
    Form_ItemDetail.subtotalCountTextValue = Convert.ToDecimal(ItemDetail.Rows(i).Cells(13).Value)

    Form_ItemDetail.weightMaxTextValue = Convert.ToDecimal(ItemDetail.Rows(i).Cells(14).Value)
    Form_ItemDetail.weightMaxUnitComboValue = ItemDetail.Rows(i).Cells(15).Value.ToString()

    Form_ItemDetail.weightMinTextValue = Convert.ToDecimal(ItemDetail.Rows(i).Cells(16).Value)
    Form_ItemDetail.weightMinUnitComboValue = ItemDetail.Rows(i).Cells(17).Value.ToString()
  End Sub


  Private Sub CloseButton_Click(sender As Object, e As EventArgs) Handles CloseButton.Click
    Close()
  End Sub

  Private Sub DeleteButton_Click(sender As Object, e As EventArgs) Handles DeleteButton.Click
    DeleteItemMaster()
  End Sub

  Private Sub DeleteItemMaster()
    Dim sql As String = String.Empty
    Dim rowSelectionCode As String = String.Empty
    Dim confirmation As String
    Dim msg1 As String
    Dim msg2 As String
    With tmpDb
      Try
        sql = GetDeleteSql(True)
        msg1 = "削除します。" & vbCrLf & "よろしいでしょうか。"
        msg2 = "削除処理完了しました。"

        confirmation = MessageBox.Show(msg1, "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If confirmation = DialogResult.Yes Then
          ' SQL実行結果が1件か？
          If .Execute(sql) = 1 Then
            ' 更新成功
            .TrnCommit()
            MessageBox.Show(msg2, "完了", MessageBoxButtons.OK, MessageBoxIcon.Information)
            SelectItemMaster()
          Else
            ' 削除失敗
            MessageBox.Show("商品マスタの削除に失敗しました。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
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
  Private Function GetDeleteSql(DeleteFlg As Boolean) As String
    Dim sql As String = String.Empty
    Dim currentRow As Integer = ItemDetail.SelectedCells(0).RowIndex
    Dim callCode As String = ItemDetail.Rows(currentRow).Cells(0).Value

    sql &= " DELETE"
    sql &= " FROM"
    sql &= "     MST_Item"
    sql &= " WHERE"
    sql &= "     call_code = " & callCode

    Call WriteExecuteLog([GetType]().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, sql)
    Return sql
  End Function

  Private Sub ItemDetail_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles ItemDetail.CellDoubleClick
    ' 詳細画面の項目値セット
    SetListData()
    ' 入力モードを設定
    Form_ItemDetail.InputMode = 2
    ' 詳細画面を表示
    Form_ItemDetail.ShowDialog()
  End Sub

  Private Sub Form_ItemList_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
    Select Case e.KeyCode
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
End Class
