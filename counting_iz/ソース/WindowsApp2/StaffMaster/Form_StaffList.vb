Imports Common
Imports Common.ClsFunction

Public Class Form_StaffList
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

  Private Sub Form_StaffList_Load(sender As Object, e As EventArgs) Handles MyBase.Load
    MaximizeBox = False
    Dim updateTime As DateTime = System.IO.File.GetLastWriteTime(System.Reflection.Assembly.GetExecutingAssembly().Location)
    Text = "担当者マスタ一覧" & " ( " & updateTime & " ) "
    Me.KeyPreview = True
    StaffDetail.RowHeadersVisible = False
    FormBorderStyle = FormBorderStyle.FixedSingle


    StaffDetail.AllowUserToAddRows = False

    StaffDetail.ColumnCount = 2

    ' 残りのヘッダーテキストを設定
    StaffDetail.Columns(0).HeaderText = "担当者ｺｰﾄﾞ"
    StaffDetail.Columns(1).HeaderText = "担当者名"


    ' カラムの幅指定
    StaffDetail.Columns(0).Width = 150
    StaffDetail.Columns(1).Width = 200

    'カラムの整列設定
    For i As Integer = 0 To 1
      StaffDetail.Columns(i).DefaultCellStyle.Alignment =
      DataGridViewContentAlignment.MiddleCenter
    Next

    'ヘッダーの整列設定
    For i As Integer = 0 To 1
      StaffDetail.Columns(i).HeaderCell.Style.Alignment =
      DataGridViewContentAlignment.MiddleCenter
    Next

    SelectStaffMaster()

    ' 選択モードを全カラム選択に設定
    StaffDetail.SelectionMode = DataGridViewSelectionMode.FullRowSelect
    If StaffDetail.Rows.Count > 0 Then
      StaffDetail.CurrentCell = StaffDetail.Rows(0).Cells(0)
      StaffDetail.Rows(0).Selected = True
    End If

    CustomizeDataGridViewHeader() ' ヘッダーのデザイン変更

  End Sub

  Public Sub SelectStaffMaster()
    Dim sql As String = String.Empty
    sql = GetAllSelectSql()
    Try
      With tmpDb
        SqlServer.GetResult(tmpDt, sql)

        If tmpDt.Rows.Count = 0 Then
          MessageBox.Show("担当者マスタにデータが登録されていません。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Else
          WriteDetail(tmpDt, StaffDetail)
        End If
      End With
    Catch ex As Exception
      Call ComWriteErrLog([GetType]().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message)
      Throw New Exception(ex.Message)
    Finally
      tmpDt.Dispose()
    End Try

    '検索結果が存在する場合、先頭行選択
    If StaffDetail.Rows.Count > 0 Then
      StaffDetail.CurrentCell = StaffDetail.Rows(0).Cells(0)
      StaffDetail.Rows(0).Selected = True

    End If
  End Sub
  Private Function GetAllSelectSql() As String

    Dim sql As String = String.Empty

    sql &= " SELECT"
    sql &= "     Staff_Number,"
    sql &= "     Staff_Name"
    sql &= " FROM"
    sql &= "     MST_Staff"
    sql &= " ORDER BY"
    sql &= "     Staff_Number"

    Call WriteExecuteLog([GetType]().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, sql)
    Return sql
  End Function
  Private Sub CustomizeDataGridViewHeader()
    With StaffDetail
      ' ヘッダーの背景色を変更
      .EnableHeadersVisualStyles = False ' デフォルトの Windows スタイルを無効化
      .ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue ' ヘッダーの背景色
      .ColumnHeadersDefaultCellStyle.ForeColor = Color.Black ' ヘッダーの文字色
      .ColumnHeadersDefaultCellStyle.Font = New Font("Meiryo", 10, FontStyle.Bold) ' フォント変更
      .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter ' ヘッダー中央寄せ
    End With
  End Sub

  Private Sub CreateButton_Click(sender As Object, e As EventArgs) Handles CreateButton.Click
    Form_StaffDetail.InputMode = 1
    Form_StaffDetail.ShowDialog()
  End Sub

  Private Sub UpdateButton_Click(sender As Object, e As EventArgs) Handles UpdateButton.Click
    '詳細画面の項目値セット
    SetListData()
    Form_StaffDetail.InputMode = 2
    Form_StaffDetail.ShowDialog()
  End Sub

  Private Sub SetListData()
    '選択している行の行番号の取得
    Dim i As Integer = StaffDetail.CurrentRow.Index
    Form_StaffDetail.CodeTextValue = StaffDetail.Rows(i).Cells(0).Value
    Form_StaffDetail.NameTextValue = StaffDetail.Rows(i).Cells(1).Value
  End Sub

  Private Sub DeleteButton_Click(sender As Object, e As EventArgs) Handles DeleteButton.Click
    DeleteManufacturerMaster()
  End Sub
  Private Sub DeleteManufacturerMaster()
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
            SelectStaffMaster()
          Else
            ' 削除失敗
            MessageBox.Show("担当者マスタの削除に失敗しました。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
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
  Private Sub RefreshText()
    For i As Integer = 0 To StaffDetail.Rows.Count - 1
      StaffDetail.Rows(i).Selected = True
      StaffDetail.FirstDisplayedScrollingRowIndex = i
      StaffDetail.CurrentCell = StaffDetail.Rows(i).Cells(0)
      Exit For
    Next
  End Sub
  Private Function GetDeleteSql(DeleteFlg As Boolean) As String
    Dim sql As String = String.Empty
    Dim currentRow As Integer = StaffDetail.SelectedCells(0).RowIndex
    Dim codeInt As Integer = StaffDetail.Rows(currentRow).Cells(0).Value

    sql &= " DELETE"
    sql &= " FROM"
    sql &= "     MST_Staff"
    sql &= " WHERE"
    sql &= "     Staff_Number = " & codeInt

    Call WriteExecuteLog([GetType]().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, sql)
    Return sql
  End Function

  Private Sub CloseButton_Click(sender As Object, e As EventArgs) Handles CloseButton.Click
    Close()
  End Sub

  Private Sub StaffDetail_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles StaffDetail.CellDoubleClick
    '詳細画面の項目値セット
    SetListData()
    Form_StaffDetail.InputMode = 2
    Form_StaffDetail.ShowDialog()
  End Sub

  Private Sub Form_ManufacturerList_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
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