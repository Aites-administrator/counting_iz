Imports Common.ClsFunction
Public Class Form_InputPassword
  ReadOnly Password As String = ReadSettingIniFile("PW", "VALUE")
  Private Sub Form_InputPassword_Load(sender As Object, e As EventArgs) Handles MyBase.Load
    MaximizeBox = False
    ClearTextBox(Me)

    PW_TextBox.Text = "495344"
    FormBorderStyle = FormBorderStyle.FixedSingle
  End Sub
  Private Sub PW_TextBox_KeyPress(sender As Object, e As KeyPressEventArgs) Handles PW_TextBox.KeyPress
    If e.KeyChar = ChrW(Keys.Enter) Then
      If Password = PW_TextBox.Text Then
        OpenForm("OTH07")
        Close()
      Else
        MessageBox.Show("パスワードに誤りがあります。" & vbCrLf & "正しいパスワードを確認してください。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        PW_TextBox.Text = String.Empty
        PW_TextBox.Focus()
      End If
    End If
  End Sub

  Private Sub EnterButton_Click(sender As Object, e As EventArgs) Handles EnterButton.Click
    If Password = PW_TextBox.Text Then
      OpenForm("OTH07")
      Close()
    Else
      MessageBox.Show("パスワードに誤りがあります。" & vbCrLf & "正しいパスワードを確認してください。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
      PW_TextBox.Focus()
    End If
  End Sub
End Class