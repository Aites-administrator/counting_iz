Imports Common
Imports Common.ClsFunction
Public Class Form_ManufacturerDetail

  Public CodeDigits As Integer = ReadSettingIniFile("MANUFACTURER_DIGITS", "VALUE")
  '新規:１ 、変更:２
  Public InputMode As Integer

  Public CodeTextValue As String
  Public NameTextValue As String

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

  Private Sub Form_ManufacturerDetail_Load(sender As Object, e As EventArgs) Handles MyBase.Load
    MaximizeBox = False
    Dim updateTime As DateTime = System.IO.File.GetLastWriteTime(System.Reflection.Assembly.GetExecutingAssembly().Location)
    Text = "製造者マスタ詳細" & " ( " & updateTime & " ) "
    Me.KeyPreview = True
    FormBorderStyle = FormBorderStyle.FixedSingle
    MarkRequiredLabels(CodeLabel, NameLabel)
    AlignDetailActionButtons(Me, OkButton, CloseButton)
    SetInitialProperty()
  End Sub

  Private Sub SetInitialProperty()
    Select Case InputMode
      Case 1
        ClearTextBox(Me)
        CodeText.Enabled = True
      Case 2
        CodeText.Enabled = False
        CodeText.Text = CodeTextValue
        NameText.Text = NameTextValue
    End Select
  End Sub

  Private Sub CloseButton_Click(sender As Object, e As EventArgs) Handles CloseButton.Click
    Me.Dispose()
  End Sub

  Private Sub OkButton_Click(sender As Object, e As EventArgs) Handles OkButton.Click
    If Not CheckValue() Then Exit Sub

    Select Case InputMode
      Case 1
        '新規登録メソッド呼出し
        InsertManufacturerMaster()
      Case 2
        '更新メソッド呼出し
        UpdateManufacturerMaster()
    End Select
  End Sub
  Function CheckValue() As Boolean
    Dim errorMessages As New List(Of String)
    Dim code As String = CodeText.Text.Trim()
    Dim name As String = NameText.Text.Trim()

    If String.IsNullOrEmpty(code) Then
      errorMessages.Add("製造者コードを入力してください。")
    ElseIf Not IsDigitsOnly(code) OrElse code.Length > CodeDigits Then
      errorMessages.Add("製造者コードは1～" & CodeDigits & "桁の数字で入力してください。")
    ElseIf code.All(Function(c) c = "0"c) Then
      errorMessages.Add("製造者コードに全て0は登録できません。")
    End If

    If String.IsNullOrEmpty(name) Then
      errorMessages.Add("製造者名を入力してください。")
    ElseIf name.Length > 48 Then
      errorMessages.Add("製造者名は48文字以内で入力してください。")
    End If

    If errorMessages.Count > 0 Then
      MessageBox.Show(String.Join(vbCrLf, errorMessages), "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)

      If String.IsNullOrEmpty(code) OrElse Not IsDigitsOnly(code) OrElse code.Length > CodeDigits OrElse code.All(Function(c) c = "0"c) Then
        Common.ClsFunction.FocusInputError(CodeText)
      Else
        Common.ClsFunction.FocusInputError(NameText)
      End If

      Return False
    End If

    CodeText.Text = code.PadLeft(CodeDigits, "0"c)
    NameText.Text = name

    If InputMode = 1 AndAlso Form_ManufacturerList.ManufacturerDetail.Rows.Count > 0 Then
      For i As Integer = 0 To Form_ManufacturerList.ManufacturerDetail.Rows.Count - 1
        If CodeText.Text.Equals(Form_ManufacturerList.ManufacturerDetail.Rows(i).Cells(0).Value) Then
          MessageBox.Show("既に登録されている製造者コードです。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
          Common.ClsFunction.FocusInputError(CodeText)
          Return False
        End If
      Next
    End If

    Return True
  End Function

  Private Sub InsertManufacturerMaster()
    Dim sql As String = String.Empty
    'Dim rowSelectionCode As String = String.Empty
    'rowSelectionCode = CodeText.Text
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
            Form_ManufacturerList.SelectManufacturerMaster()
            ' 一覧画面件数更新
            'Form_ManufacturerList.GetRowCount(Form_ManufacturerList.DeletedDisplayCheckBox.Checked)

            Close()
          Else
            ' 登録失敗
            MessageBox.Show("製造者マスタの登録に失敗しました。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
          End If
        Else
          Exit Sub
        End If
      Catch ex As Exception
        Call ComWriteErrLog([GetType]().Name,
                                      System.Reflection.MethodBase.GetCurrentMethod().Name, ex)
        Throw
      End Try
    End With
  End Sub

  Private Function GetInsertSql() As String
    Dim sql As String = String.Empty
    Dim CodeTxtInt As String = EscapeSqlLiteral(CodeText.Text)
    Dim NameTxt As String = EscapeSqlLiteral(NameText.Text)
    Dim tmpdate As DateTime = CDate(ComGetProcTime())

    sql &= " INSERT INTO MST_Manufacturer("
    sql &= "     Manufacturer_Code,"
    sql &= "     Manufacturer_Name,"
    sql &= "     create_date,"
    sql &= "     update_date"
    sql &= " )"
    sql &= " VALUES("
    sql &= "     '" & CodeTxtInt & "',"
    sql &= "     '" & EscapeSqlLiteral(NameTxt) & "',"
    sql &= "     '" & tmpdate & "',"
    sql &= "     '" & tmpdate & "'"
    sql &= " )"

    Call WriteExecuteLog([GetType]().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, sql)
    Return sql
  End Function

  Private Sub UpdateManufacturerMaster()
    Dim sql As String = String.Empty
    'Dim rowSelectionCode As String = String.Empty
    'rowSelectionCode = CodeText.Text
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
            Dim CurrentRow As Integer = Form_ManufacturerList.ManufacturerDetail.CurrentRow.Index
            Form_ManufacturerList.SelectManufacturerMaster()
            Form_ManufacturerList.ManufacturerDetail.Rows(CurrentRow).Selected = True
            Form_ManufacturerList.ManufacturerDetail.FirstDisplayedScrollingRowIndex = CurrentRow
            '選択している行の行番号の取得
            Form_ManufacturerList.ManufacturerDetail.CurrentCell = Form_ManufacturerList.ManufacturerDetail.Rows(CurrentRow).Cells(0)
            Close()
          Else
            ' 更新失敗
            MessageBox.Show("製造者マスタの更新に失敗しました。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
          End If
        Else
          Exit Sub
        End If
      Catch ex As Exception
        Call ComWriteErrLog([GetType]().Name,
                      System.Reflection.MethodBase.GetCurrentMethod().Name, ex)
        Throw
      End Try
    End With
  End Sub
  Private Function GetUpdateSql() As String
    Dim sql As String = String.Empty
    Dim CodeTxt As String = EscapeSqlLiteral(CodeText.Text)
    Dim NameTxt As String = EscapeSqlLiteral(NameText.Text)
    Dim tmpdate As DateTime = CDate(ComGetProcTime())

    sql &= " UPDATE MST_Manufacturer SET"
    sql &= "     Manufacturer_Name = '" & EscapeSqlLiteral(NameTxt) & "',"
    sql &= "     update_date = '" & tmpdate & "'"
    sql &= " WHERE Manufacturer_Code = '" & EscapeSqlLiteral(CodeTxt) & "'"

    Call WriteExecuteLog([GetType]().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, sql)
    Return sql
  End Function

  Private Sub CodeText_KeyPress(sender As Object, e As KeyPressEventArgs) Handles CodeText.KeyPress
    'キーが [0]～[9] または [BackSpace] 以外の場合イベントをキャンセル
    If Not (("0"c <= e.KeyChar And e.KeyChar <= "9"c) Or e.KeyChar = ControlChars.Back) Then
      e.Handled = True
    End If
  End Sub

  Private Sub CodeText_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles CodeText.Validating
    If String.IsNullOrEmpty(CodeText.Text) = False Then
      CodeText.Text = CodeText.Text.PadLeft(CodeDigits, "0"c)
    End If
  End Sub

  Private Sub Form_ManufacturerDetail_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
    Select Case e.KeyCode
      Case Keys.F5
        OkButton.PerformClick()
      Case Keys.Escape
        Me.Close()
    End Select
  End Sub
  Private Sub ApplyCommonLayout_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
    Common.ClsFunction.ApplyScreenLayout(Me)
  End Sub
End Class
