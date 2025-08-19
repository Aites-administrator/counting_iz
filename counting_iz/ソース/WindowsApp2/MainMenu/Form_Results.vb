Imports Common.ClsFunction
Public Class Form_Results
  Private Sub AchievementCreateButton_Click(sender As Object, e As EventArgs) Handles AchievementCreateButton.Click
    Form_InputPassword.ShowDialog()
  End Sub

  Private Sub Form_AchievementMente_Load(sender As Object, e As EventArgs) Handles MyBase.Load
    MaximizeBox = False
    FormBorderStyle = FormBorderStyle.FixedSingle
    ' キーイベントをフォーム全体で受け取るようにする
    Me.KeyPreview = True
  End Sub

  Private Sub CloseButton_Click(sender As Object, e As EventArgs) Handles CloseButton.Click
    Close()
  End Sub

  Private Sub Form_Results_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
    Select Case e.KeyCode
      Case Keys.F1
        AchievementCreateButton.PerformClick()
      Case Keys.Escape
        Me.Close()
    End Select
  End Sub
End Class