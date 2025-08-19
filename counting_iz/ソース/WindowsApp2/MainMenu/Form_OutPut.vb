Imports Common.ClsFunction
Public Class Form_OutPut
  Private Sub Form_OutPut_Load(sender As Object, e As EventArgs) Handles MyBase.Load
    MaximizeBox = False
    FormBorderStyle = FormBorderStyle.FixedSingle
    ' キーイベントをフォーム全体で受け取るようにする
    Me.KeyPreview = True
  End Sub

  Private Sub DetailOutputButton_Click(sender As Object, e As EventArgs) Handles DetailOutputButton.Click
    OpenForm("OTH01")
  End Sub
  Private Sub MonthlyReportOutputButton_Click(sender As Object, e As EventArgs) Handles MonthlyReportOutputButton.Click
    OpenForm("OTH02")
  End Sub

  Private Sub PackingLabelPrintButton_Click(sender As Object, e As EventArgs) 
    OpenForm("OTH03")
  End Sub

  Private Sub CloseButton_Click(sender As Object, e As EventArgs) Handles CloseButton.Click
    Close()
  End Sub

  Private Sub WeightCheckButton_Click(sender As Object, e As EventArgs) Handles WeightCheckButton.Click
    OpenForm("OTH03")
  End Sub

  Private Sub Form_OutPut_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
    Select Case e.KeyCode
      Case Keys.F1
        MonthlyReportOutputButton.PerformClick()
      'Case Keys.F2
      '  WeightCheckButton.PerformClick()
      Case Keys.Escape
        Me.Close()
    End Select
  End Sub
End Class