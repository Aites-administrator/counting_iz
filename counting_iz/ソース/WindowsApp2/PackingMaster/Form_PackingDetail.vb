Imports Common
Imports Common.ClsFunction
Public Class Form_PackingDetail
  Public InputMode As Integer
  Public CodeTextValue As String
  Public NameTextValue As String
  Public WeightTextValue As String
  Public weightUnitComboValue As String

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
  Private Sub Form_PackingDetail_Load(sender As Object, e As EventArgs) Handles MyBase.Load
    MaximizeBox = False
    Dim updateTime As DateTime = System.IO.File.GetLastWriteTime(System.Reflection.Assembly.GetExecutingAssembly().Location)
    Text = "風袋マスタ詳細" & " ( " & updateTime & " ) "
    Me.KeyPreview = True

    FormBorderStyle = FormBorderStyle.FixedSingle

    SetUnitComboBox()
    SetInitialProperty()

  End Sub
  Private Sub OkButton_Click(sender As Object, e As EventArgs) Handles OkButton.Click
    Select Case InputMode
      Case 1
        If CheckValue() = False Then
          Exit Sub
        End If
        '新規登録メソッド呼出し
        InsertPackingMaster()
      Case 2
        '更新メソッド呼出し
        UpdatePackingMaster()
    End Select
  End Sub

  Private Sub SetUnitComboBox()
    ' 共通のアイテムリスト
    Dim items As String() = {"mg", "g", "kg", "t"}

    ' コンボボックスのアイテムをクリア
    UnitComboBox.Items.Clear()

    ' アイテムをコンボボックスに追加
    For Each item As String In items
      UnitComboBox.Items.Add(item)
    Next

    ' 初期値を "g" に設定
    UnitComboBox.SelectedItem = "g"

    'PackingComboBox.Items.Add(10.0)
  End Sub

  Private Sub CodeText_KeyPress(sender As Object, e As KeyPressEventArgs) Handles CodeText.KeyPress
    'キーが [0]～[9] または [BackSpace] 以外の場合イベントをキャンセル
    If Not (("0"c <= e.KeyChar And e.KeyChar <= "9"c) Or e.KeyChar = ControlChars.Back) Then
      e.Handled = True
    End If
  End Sub
  Private Sub WeightText_TextChanged(sender As Object, e As EventArgs) Handles WeightText.TextChanged
    If IsNumeric(WeightText.Text) = True Then
      If CDec(WeightText.Text) * 100 < Math.Ceiling(CDec(WeightText.Text) * 100) Then
        WeightText.Text = WeightText.Text.Remove(WeightText.Text.Length - 1, 1)
        WeightText.SelectionStart = WeightText.Text.Length
      End If
    End If
    If CountChar(WeightText.Text, ".") > 1 Then
      WeightText.Text = WeightText.Text.Remove(WeightText.Text.Length - 1, 1)
      WeightText.SelectionStart = WeightText.Text.Length
    End If
  End Sub
  Private Sub WeightText_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles WeightText.Validating
    Dim wkWeight As Double
    If ActiveControl.Name <> "WeightText" And ActiveControl.Name <> "CloseButton" Then
      If String.IsNullOrEmpty(WeightText.Text) = False Then

        wkWeight = Double.Parse(WeightText.Text)

        WeightText.Text = wkWeight.ToString("0.00")
      End If
    End If
  End Sub
  Private Sub WeightText_KeyPress(sender As Object, e As KeyPressEventArgs) Handles WeightText.KeyPress
    'キーが [0]～[9] または [BackSpace] 以外の場合イベントをキャンセル
    If Not (("0"c <= e.KeyChar And e.KeyChar <= "9"c) Or e.KeyChar = ControlChars.Back Or e.KeyChar = ".") Then
      e.Handled = True
    End If
  End Sub
  Private Sub CloseButton_Click(sender As Object, e As EventArgs) Handles CloseButton.Click
    Close()
  End Sub
  Private Sub SetInitialProperty()
    CodeText.Text = CodeTextValue
    NameText.Text = NameTextValue
    WeightText.Text = WeightTextValue
    UnitComboBox.SelectedItem = weightUnitComboValue

    Select Case InputMode
      Case 1
        ClearTextBox(Me)
        'コード入力不可
        CodeText.Enabled = True

      Case 2
        'コード入力不可
        CodeText.Enabled = False
        '名称入力不可
        NameText.Enabled = True
    End Select
  End Sub
  Function CheckValue()
    Dim CheckResult As Boolean = True
    '必須項目チェック
    If String.IsNullOrEmpty(CodeText.Text) = True Then
      MessageBox.Show("コードを入力してください", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
      CheckResult = False
    End If
    '重複チェック
    If Form_PackingList.PackingDetail.Rows.Count > 0 Then
      For i As Integer = 0 To Form_PackingList.PackingDetail.Rows.Count - 1
        If CodeText.Text.Equals(Form_PackingList.PackingDetail.Rows(i).Cells(0).Value) Then
          MessageBox.Show("既に登録されているコードです。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
          CheckResult = False
          CodeText.Focus()
          Exit For
        End If
      Next
    End If
    Return CheckResult
  End Function
  Private Sub InsertPackingMaster()
    Dim sql As String = String.Empty
    Dim rowSelectionCode As String = String.Empty
    rowSelectionCode = CodeText.Text
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
            Form_PackingList.SelectPackingMaster()
            Close()
          Else
            ' 登録失敗
            MessageBox.Show("風袋マスタの登録に失敗しました。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
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
    Dim tmpdate As DateTime = CDate(ComGetProcTime())
    Dim upperUnitWeightUnit As String = UnitComboBox.SelectedItem.ToString()

    sql &= " INSERT INTO MST_PACKING("
    sql &= "     PackingNo,"
    sql &= "     PackingName,"
    sql &= "     PackingWeight,"
    sql &= "     PackingWeightUnit,"
    sql &= "     create_date,"
    sql &= "     update_date"
    sql &= " )"
    sql &= " VALUES("
    sql &= "     '" & CodeText.Text & "',"
    sql &= "     '" & NameText.Text & "',"
    sql &= "     '" & WeightText.Text & "',"
    sql &= "     '" & upperUnitWeightUnit & "',"
    sql &= "     '" & tmpdate & "',"
    sql &= "     '" & tmpdate & "'"
    sql &= " )"
    Call WriteExecuteLog([GetType]().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, sql)
    Return sql
  End Function
  Private Sub UpdatePackingMaster()
    Dim sql As String = String.Empty
    Dim rowSelectionCode As String = String.Empty
    rowSelectionCode = CodeText.Text
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
            Dim CurrentRow As Integer = Form_PackingList.PackingDetail.CurrentRow.Index
            Form_PackingList.SelectPackingMaster()
            Form_PackingList.PackingDetail.Rows(CurrentRow).Selected = True
            Form_PackingList.PackingDetail.FirstDisplayedScrollingRowIndex = CurrentRow
            Form_PackingList.PackingDetail.CurrentCell = Form_PackingList.PackingDetail.Rows(CurrentRow).Cells(0)
            Close()
          Else
            ' 削除失敗
            MessageBox.Show("風袋マスタの更新に失敗しました。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
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
    Dim tmpdate As DateTime = CDate(ComGetProcTime())
    Dim upperUnitWeightUnit As String = UnitComboBox.SelectedItem.ToString()

    sql &= " UPDATE MST_PACKING"
    sql &= "    SET PackingNo = '" & CodeText.Text & "'"
    sql &= "       ,PackingName = '" & NameText.Text & "'"
    sql &= "       ,PackingWeight = '" & WeightText.Text & "'"
    sql &= "       ,PackingWeightUnit = '" & upperUnitWeightUnit & "'"
    sql &= "       ,update_date = '" & tmpdate & "'"
    sql &= " WHERE PackingNo = '" & CodeText.Text & "' "
    Call WriteExecuteLog([GetType]().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, sql)
    Return sql
  End Function

  Private Sub Form_PackingDetail_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
    Select Case e.KeyCode
      Case Keys.F5
        OkButton.PerformClick()
      Case Keys.Escape
        Me.Close()
    End Select
  End Sub
End Class