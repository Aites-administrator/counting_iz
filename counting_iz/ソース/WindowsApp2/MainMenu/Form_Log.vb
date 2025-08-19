Imports Common.ClsFunction
Public Class Form_Log
  Private Sub Form_Log_Load(sender As Object, e As EventArgs) Handles MyBase.Load
    MaximizeBox = False
    FormBorderStyle = FormBorderStyle.FixedSingle
    ' キーイベントをフォーム全体で受け取るようにする
    Me.KeyPreview = True
  End Sub
  Private Sub LogDisplayButton_Click(sender As Object, e As EventArgs) Handles LogDisplayButton.Click
    OpenForm("OTH04")
  End Sub

  Private Sub RealtimeConfirmation_Button_Click(sender As Object, e As EventArgs) Handles RealtimeConfirmation_Button.Click
    OpenForm("OTH05")
  End Sub

  Private Sub CloseButton_Click(sender As Object, e As EventArgs) Handles CloseButton.Click
    Close()
  End Sub

  Private Sub Form_Log_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
    Select Case e.KeyCode
      Case Keys.F1
        RealtimeConfirmation_Button.PerformClick()
      Case Keys.F2
        LogDisplayButton.PerformClick()
      Case Keys.Escape
        Me.Close()
    End Select
  End Sub
End Class