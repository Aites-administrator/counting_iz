Imports Common
Imports Common.ClsFunction

Public Class Form_PackingList
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
  Private Sub Form_PackingMaster_Load(sender As Object, e As EventArgs) Handles MyBase.Load
    MaximizeBox = False
    Dim updateTime As DateTime = System.IO.File.GetLastWriteTime(System.Reflection.Assembly.GetExecutingAssembly().Location)
    Text = "風袋マスタ一覧" & " ( " & updateTime & " ) "
    Me.KeyPreview = True
    FormBorderStyle = FormBorderStyle.FixedSingle
    PackingDetail.RowHeadersVisible = False
    MaximizeBox = False

    'ユーザーからのデータ追加を不可にしておく
    PackingDetail.AllowUserToAddRows = False
    PackingDetail.ColumnCount = 4
    PackingDetail.Columns(0).HeaderText = "コード"
    PackingDetail.Columns(1).HeaderText = "名称"
    PackingDetail.Columns(2).HeaderText = "重量"
    PackingDetail.Columns(3).HeaderText = "単位"

    'カラムの幅指定
    PackingDetail.Columns(0).Width = 100
    PackingDetail.Columns(1).Width = 210
    PackingDetail.Columns(2).Width = 140
    PackingDetail.Columns(3).Width = 100

    'ヘッダーの整列設定
    For i As Integer = 0 To 3
      PackingDetail.Columns(i).DefaultCellStyle.Alignment =
   DataGridViewContentAlignment.MiddleCenter
      PackingDetail.Columns(i).HeaderCell.Style.Alignment =
  DataGridViewContentAlignment.MiddleCenter
    Next

    SelectPackingMaster()

    'マルチ選択不可
    PackingDetail.MultiSelect = False

    '選択モード設定(全カラム)
    PackingDetail.SelectionMode = DataGridViewSelectionMode.FullRowSelect

    '検索結果が存在する場合、先頭行選択
    If PackingDetail.Rows.Count > 0 Then
      PackingDetail.CurrentCell = PackingDetail.Rows(0).Cells(0)
      PackingDetail.Rows(0).Selected = True
    End If

    CustomizeDataGridViewHeader()
  End Sub
  ' DataGridView のヘッダーのデザインを変更
  Private Sub CustomizeDataGridViewHeader()
    With PackingDetail
      ' ヘッダーの背景色を変更
      .EnableHeadersVisualStyles = False ' デフォルトの Windows スタイルを無効化
      .ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue ' ヘッダーの背景色
      .ColumnHeadersDefaultCellStyle.ForeColor = Color.Black ' ヘッダーの文字色
      .ColumnHeadersDefaultCellStyle.Font = New Font("Meiryo", 10, FontStyle.Bold) ' フォント変更
      .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter ' ヘッダー中央寄せ
    End With
  End Sub
  Private Sub CreateButton_Click(sender As Object, e As EventArgs) Handles CreateButton.Click
    Form_PackingDetail.InputMode = 1
    Form_PackingDetail.ShowDialog()
  End Sub
  Private Sub UpdateButton_Click(sender As Object, e As EventArgs) Handles UpdateButton.Click
    '詳細画面の項目値セット
    SetListData()
    Form_PackingDetail.InputMode = 2
    Form_PackingDetail.ShowDialog()
  End Sub
  Private Sub DeleteButton_Click(sender As Object, e As EventArgs) Handles DeleteButton.Click
    DeleteScaleMaster()
  End Sub
  Private Sub PackingDetail_DoubleClick(sender As Object, e As EventArgs) Handles PackingDetail.DoubleClick
    '詳細画面の項目値セット
    SetListData()
    Form_PackingDetail.InputMode = 2
    Form_PackingDetail.ShowDialog()
  End Sub
  Private Sub CloseButton_Click(sender As Object, e As EventArgs) Handles CloseButton.Click
    Close()
  End Sub

  Private Sub SetListData()
    '選択している行の行番号の取得
    Dim i As Integer = PackingDetail.CurrentRow.Index
    Form_PackingDetail.CodeTextValue = PackingDetail.Rows(i).Cells(0).Value
    Form_PackingDetail.NameTextValue = PackingDetail.Rows(i).Cells(1).Value
    Form_PackingDetail.WeightTextValue = PackingDetail.Rows(i).Cells(2).Value
    Form_PackingDetail.weightUnitComboValue = PackingDetail.Rows(i).Cells(3).Value
  End Sub

  Public Sub SelectPackingMaster()
    Dim sql As String = String.Empty
    sql = GetAllSelectSql()
    Try
      With tmpDb
        SqlServer.GetResult(tmpDt, sql)
        If tmpDt.Rows.Count = 0 Then
          MessageBox.Show("風袋マスタにデータが登録されていません。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Else
          WriteDetail(tmpDt, PackingDetail)
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

  Private Function GetAllSelectSql() As String

    Dim sql As String = String.Empty

    sql &= " SELECT"
    sql &= "     PackingNo,"
    sql &= "     PackingName,"
    sql &= "     PackingWeight,"
    sql &= "     PackingWeightUnit"
    sql &= " FROM"
    sql &= "     MST_PACKING"
    sql &= " ORDER BY  "
    sql &= "     PackingNo"
    Call WriteExecuteLog([GetType]().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, sql)
    Return sql

  End Function

  Private Sub DeleteScaleMaster()
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
            SelectPackingMaster()
            RefreshText()
          Else
            ' 削除失敗
            MessageBox.Show("風袋マスタの削除に失敗しました。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
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
    Dim currentRow As Integer = PackingDetail.SelectedCells(0).RowIndex
    Dim packingCode As String = PackingDetail.Rows(currentRow).Cells(0).Value

    sql &= " DELETE"
    sql &= " FROM"
    sql &= "     MST_PACKING"
    sql &= " WHERE"
    sql &= "     PackingNo = '" & packingCode & "' "

    Call WriteExecuteLog([GetType]().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, sql)
    Return sql
  End Function

  Private Sub RefreshText()
    For i As Integer = 0 To PackingDetail.Rows.Count - 1
      PackingDetail.Rows(i).Selected = True
      PackingDetail.FirstDisplayedScrollingRowIndex = i
      PackingDetail.CurrentCell = PackingDetail.Rows(i).Cells(0)
      Exit For
    Next
  End Sub

  Private Sub Form_PackingList_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
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
