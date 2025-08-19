Imports Common
Imports Common.ClsFunction
Public Class Form_Free3Detail
  '新規:１ 、変更:２
  Public InputMode As Integer

  Public free3CodeTextValue As String
  Public free3NameTextValue As String

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
  Private Sub Form_Free3Detail_Load(sender As Object, e As EventArgs) Handles MyBase.Load
    MaximizeBox = False
    Dim updateTime As DateTime = System.IO.File.GetLastWriteTime(System.Reflection.Assembly.GetExecutingAssembly().Location)
    Text = "フリーマスタ３詳細" & " ( " & updateTime & " ) "
    FormBorderStyle = FormBorderStyle.FixedSingle

    ' キーイベントをフォーム全体で受け取るようにする
    Me.KeyPreview = True

    SetInitialProperty()
  End Sub
  Private Sub SetInitialProperty()
    Select Case InputMode
      Case 1
        ClearTextBox(Me)
        CodeText.Enabled = True
      Case 2
        CodeText.Enabled = False
        CodeText.Text = free3CodeTextValue
        NameText.Text = free3NameTextValue
    End Select
  End Sub

  Private Sub OkButton_Click(sender As Object, e As EventArgs) Handles OkButton.Click
    Select Case InputMode
      Case 1
        If CheckValue() = False Then
          Exit Sub
        End If
        '新規登録メソッド呼出し
        InsertFree3Master()
      Case 2
        '更新メソッド呼出し
        UpdateFree3Master()
    End Select
  End Sub
  Function CheckValue() As Boolean
    Dim errorMessages As New List(Of String)

    ' 必須項目チェック
    If String.IsNullOrEmpty(CodeText.Text) Then
      errorMessages.Add("フリー3コードを入力してください。")
      CodeText.Focus()
    ElseIf CodeText.Text = "0000" Then
      errorMessages.Add("フリー3コードに0000は登録できません。")
      CodeText.Focus()
    End If

    ' 必須項目のエラーがある場合、まとめてメッセージを表示
    If errorMessages.Count > 0 Then
      MessageBox.Show(String.Join(vbCrLf, errorMessages), "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)

      ' エラー項目にフォーカスを設定
      If String.IsNullOrEmpty(CodeText.Text) Then
        CodeText.Focus()
      ElseIf String.IsNullOrEmpty(NameText.Text) Then
        NameText.Focus()
      End If

      Return False
    End If

    ' 重複チェック
    If Form_Free3List.Free3Detail.Rows.Count > 0 Then
      For Each row As DataGridViewRow In Form_Free3List.Free3Detail.Rows
        If CodeText.Text.Equals(row.Cells(0).Value?.ToString()) Then
          MessageBox.Show("既に登録されているフリー3コードです。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
          CodeText.Focus()
          Return False
        End If
      Next
    End If

    Return True
  End Function
  Private Sub InsertFree3Master()
    Dim sql As String = String.Empty
    With tmpDb
      Try
        sql = GetInsertSql()
        Dim confirmation As String
        confirmation = MessageBox.Show("登録します。" & vbCrLf & "よろしいでしょうか。", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If confirmation = DialogResult.Yes Then
          ' SQL実行結果が1件か？
          If .Execute(sql) = 1 Then
            ' 更新成功
            .TrnCommit()
            MessageBox.Show("登録処理完了しました。", "完了", MessageBoxButtons.OK, MessageBoxIcon.Information)
            ' 一覧画面データ更新
            Form_Free3List.SelectFree3Master()

            Close()
          Else
            ' 登録失敗
            MessageBox.Show("フリー3マスタの登録に失敗しました。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
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

  Private Function GetInsertSql() As String
    Dim sql As String = String.Empty
    Dim CodeTxt As String = CodeText.Text
    Dim NameTxt As String = NameText.Text
    Dim tmpdate As DateTime = CDate(ComGetProcTime())

    sql &= " INSERT INTO MST_Free3("
    sql &= "     Free3_number,"
    sql &= "     Free3_name,"
    sql &= "     create_date,"
    sql &= "     update_date"
    sql &= " )"
    sql &= " VALUES("
    sql &= "     '" & CodeTxt & "',"
    sql &= "     '" & NameTxt & "',"
    sql &= "     '" & tmpdate & "',"
    sql &= "     '" & tmpdate & "'"
    sql &= " )"

    Call WriteExecuteLog([GetType]().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, sql)
    Return sql
  End Function

  Private Sub UpdateFree3Master()
    Dim sql As String = String.Empty
    With tmpDb
      Try
        sql = GetUpdateSql()
        Dim confirmation As String
        confirmation = MessageBox.Show("更新します。" & vbCrLf & "よろしいでしょうか。", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If confirmation = DialogResult.Yes Then
          ' SQL実行結果が1件か？
          If .Execute(sql) = 1 Then
            ' 更新成功
            .TrnCommit()
            MessageBox.Show("更新処理完了しました。", "完了", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Dim CurrentRow As Integer = Form_Free3List.Free3Detail.CurrentRow.Index
            Form_Free3List.SelectFree3Master()
            Form_Free3List.Free3Detail.Rows(CurrentRow).Selected = True
            Form_Free3List.Free3Detail.FirstDisplayedScrollingRowIndex = CurrentRow
            '選択している行の行番号の取得
            Form_Free3List.Free3Detail.CurrentCell = Form_Free3List.Free3Detail.Rows(CurrentRow).Cells(0)
            Close()
          Else
            ' 更新失敗
            MessageBox.Show("フリー3マスタの更新に失敗しました。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
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
  Private Function GetUpdateSql() As String
    Dim sql As String = String.Empty
    Dim CodeTxt As String = CodeText.Text
    Dim NameTxt As String = NameText.Text
    Dim tmpdate As DateTime = CDate(ComGetProcTime())

    sql &= " UPDATE MST_Free3 SET"
    sql &= "     Free3_name = '" & NameTxt & "',"
    sql &= "     update_date = '" & tmpdate & "'"
    sql &= " WHERE free3_number = '" & CodeTxt & "'"

    Call WriteExecuteLog([GetType]().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, sql)
    Return sql
  End Function
  Private Sub CloseButton_Click(sender As Object, e As EventArgs) Handles CloseButton.Click
    Me.Dispose()
  End Sub

  Private Sub CodeText_KeyPress(sender As Object, e As KeyPressEventArgs) Handles CodeText.KeyPress
    'キーが [0]～[9] または [BackSpace] 以外の場合イベントをキャンセル
    If Not (("0"c <= e.KeyChar And e.KeyChar <= "9"c) Or e.KeyChar = ControlChars.Back) Then
      e.Handled = True
    End If
  End Sub

  Private Sub CodeText_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles CodeText.Validating
    If String.IsNullOrEmpty(CodeText.Text) = False Then
      CodeText.Text = CodeText.Text.PadLeft(4, "0"c)
    End If
  End Sub

  Private Sub Form_Free3Detail_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
    Select Case e.KeyCode
      Case Keys.F5
        OkButton.PerformClick()
      Case Keys.Escape
        Me.Close()
    End Select
  End Sub
End Class
