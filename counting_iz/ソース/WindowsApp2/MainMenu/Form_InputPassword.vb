Imports Common.ClsFunction
Public Class Form_InputPassword
  ReadOnly Password As String = ReadSettingIniFile("PW", "VALUE")
  Public Property TargetFormKey As String = "OTH07"
  Private Sub Form_InputPassword_Load(sender As Object, e As EventArgs) Handles MyBase.Load
    MaximizeBox = False
    ClearTextBox(Me)

    PW_TextBox.Clear()
    FormBorderStyle = FormBorderStyle.FixedDialog
  End Sub

  Private Sub EnterButton_Click(sender As Object, e As EventArgs) Handles EnterButton.Click
    If String.IsNullOrWhiteSpace(Password) Then
      MessageBox.Show("パスワードがINIファイルに設定されていません。", "設定エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
      Return
    End If

    If Password = PW_TextBox.Text Then
      OpenForm(TargetFormKey)
      DialogResult = DialogResult.OK
      Close()
    Else
      MessageBox.Show("パスワードに誤りがあります。" & vbCrLf & "正しいパスワードを確認してください。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
      PW_TextBox.Text = String.Empty
      PW_TextBox.Focus()
    End If
  End Sub
  Private Sub ApplyCommonLayout_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
    Common.ClsFunction.ApplyScreenLayout(Me)
    PW_TextBox.SelectAll()
    PW_TextBox.Focus()
  End Sub
End Class
