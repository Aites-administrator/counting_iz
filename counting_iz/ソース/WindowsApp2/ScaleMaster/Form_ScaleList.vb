Imports Common
Imports Common.ClsFunction
Public Class Form_ScaleList
  Dim AllRowDisplayFlg As Boolean = False

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
  Private Sub Form_ScaleMaster_Load(sender As Object, e As EventArgs) Handles MyBase.Load
    MaximizeBox = False
    Dim updateTime As DateTime = System.IO.File.GetLastWriteTime(System.Reflection.Assembly.GetExecutingAssembly().Location)
    Text = "計量器マスタ一覧" & " ( " & updateTime & " ) "
    Me.KeyPreview = True
    ScaleDetail.RowHeadersVisible = False
    FormBorderStyle = FormBorderStyle.FixedSingle
    'ユーザーからのデータ追加を不可にしておく
    ScaleDetail.AllowUserToAddRows = False
    ScaleDetail.ColumnCount = 2
    ScaleDetail.Columns(0).HeaderText = "号機番号"
    ScaleDetail.Columns(1).HeaderText = "IP_ADDRESS"

    'カラムの幅指定
    ScaleDetail.Columns(0).Width = 120
    ScaleDetail.Columns(1).Width = 260

    'ヘッダーの整列設定
    For i As Integer = 0 To 1
      ScaleDetail.Columns(i).DefaultCellStyle.Alignment =
   DataGridViewContentAlignment.MiddleCenter
      ScaleDetail.Columns(i).HeaderCell.Style.Alignment =
  DataGridViewContentAlignment.MiddleCenter
    Next

    SelectScaleMaster()

    'マルチ選択不可
    ScaleDetail.MultiSelect = False

    '選択モード設定(全カラム)
    ScaleDetail.SelectionMode = DataGridViewSelectionMode.FullRowSelect
    If ScaleDetail.Rows.Count > 0 Then
      ScaleDetail.CurrentCell = ScaleDetail.Rows(0).Cells(0)
      ScaleDetail.Rows(0).Selected = True
    End If


    CustomizeDataGridViewHeader() ' ヘッダーのデザイン変更

  End Sub
  ' DataGridView のヘッダーのデザインを変更
  Private Sub CustomizeDataGridViewHeader()
    With ScaleDetail
      ' ヘッダーの背景色を変更
      .EnableHeadersVisualStyles = False ' デフォルトの Windows スタイルを無効化
      .ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue ' ヘッダーの背景色
      .ColumnHeadersDefaultCellStyle.ForeColor = Color.Black ' ヘッダーの文字色
      .ColumnHeadersDefaultCellStyle.Font = New Font("Meiryo", 10, FontStyle.Bold) ' フォント変更
      .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter ' ヘッダー中央寄せ
    End With
  End Sub
  Private Sub CreateButton_Click(sender As Object, e As EventArgs) Handles CreateButton.Click
    Form_ScaleDetail.InputMode = 1
    Form_ScaleDetail.ShowDialog()
  End Sub
  Private Sub UpdateButton_Click(sender As Object, e As EventArgs) Handles UpdateButton.Click
    '詳細画面の項目値セット
    SetListData()
    Form_ScaleDetail.InputMode = 2
    Form_ScaleDetail.ShowDialog()
  End Sub
  Private Sub DeleteButton_Click(sender As Object, e As EventArgs) Handles DeleteButton.Click
    DeleteScaleMaster()
  End Sub
  'Private Sub ScaleDetail_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles ScaleDetail.CellClick
  '    If ScaleDetail.CurrentRow.Cells(2).Value Then
  '        DeleteButton.Text = "削除取消"
  '    Else
  '        DeleteButton.Text = "削除"
  '    End If
  'End Sub
  'Private Sub DeletedDisplayCheckBox_CheckedChanged(sender As Object, e As EventArgs) Handles DeletedDisplayCheckBox.CheckedChanged
  '    If DeletedDisplayCheckBox.Checked Then
  '        AllRowDisplayFlg = True
  '        DisPlayDeleteRow(AllRowDisplayFlg)
  '        GetRowCount(True)
  '    Else
  '        AllRowDisplayFlg = False
  '        DisPlayDeleteRow(AllRowDisplayFlg)
  '        GetRowCount(False)
  '    End If
  'End Sub
  Private Sub ScaleDetail_DoubleClick(sender As Object, e As EventArgs) Handles ScaleDetail.DoubleClick
    '詳細画面の項目値セット
    SetListData()
    Form_ScaleDetail.InputMode = 2
    Form_ScaleDetail.ShowDialog()
  End Sub
  Private Sub CloseButton_Click(sender As Object, e As EventArgs) Handles CloseButton.Click
    Close()
  End Sub

  Private Sub SetListData()
    '選択している行の行番号の取得
    Dim i As Integer = ScaleDetail.CurrentRow.Index
    Form_ScaleDetail.UnitNumberTextValue = ScaleDetail.Rows(i).Cells(0).Value
    Form_ScaleDetail.IpAddressTextValue = ScaleDetail.Rows(i).Cells(1).Value
  End Sub
  Public Sub SelectScaleMaster()
    Dim sql As String = String.Empty
    sql = GetAllSelectSql()
    Try
      With tmpDb
        SqlServer.GetResult(tmpDt, sql)
        If tmpDt.Rows.Count = 0 Then
          MessageBox.Show("計量器マスタにデータが登録されていません。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Else
          WriteDetail(tmpDt, ScaleDetail)
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
    sql &= "     UNIT_NUMBER,"
    sql &= "     IP_ADDRESS"
    'sql &= "     DELETE_FLG"
    sql &= " FROM"
    sql &= "     MST_SCALE"
    sql &= " ORDER BY  "
    sql &= "     UNIT_NUMBER"
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
            SelectScaleMaster()
          Else
            ' 削除失敗
            MessageBox.Show("計量マスタの削除に失敗しました。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
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
    Dim wkUNIT_NUMBER As String
    Dim CurrentRow As Integer = ScaleDetail.SelectedCells(0).RowIndex
    wkUNIT_NUMBER = ScaleDetail.Rows(CurrentRow).Cells(0).Value
    sql &= " DELETE "
    sql &= " FROM "
    sql &= "     MST_SCALE "
    sql &= " WHERE "
    sql &= "     UNIT_NUMBER = " & wkUNIT_NUMBER
    Call WriteExecuteLog([GetType]().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, sql)
    Return sql
  End Function

  Private Sub Form_ScaleList_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
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
