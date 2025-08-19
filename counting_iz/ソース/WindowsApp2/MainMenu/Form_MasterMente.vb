Imports Common.ClsFunction
Public Class Form_MasterMente

  Public ItemMasterMode As String = ReadSettingIniFile("ITEM_MASTER_MODE", "VALUE")

  Private Sub Form_MasterMente_Load(sender As Object, e As EventArgs) Handles MyBase.Load
    MaximizeBox = False
    FormBorderStyle = FormBorderStyle.FixedSingle
    ' キーイベントをフォーム全体で受け取るようにする
    Me.KeyPreview = True
  End Sub

  Private Sub ItemMasterButton_Click(sender As Object, e As EventArgs) Handles ItemMasterButton.Click
    OpenForm("M01")
  End Sub

  Private Sub WorkOrderButton_Click(sender As Object, e As EventArgs) Handles WorkOrderButton.Click
    OpenForm("M02")
  End Sub

  Private Sub StaffMasterButton_Click(sender As Object, e As EventArgs) Handles StaffMasterButton.Click
    OpenForm("M03")
  End Sub
  Private Sub ManufacturerMasterButton_Click(sender As Object, e As EventArgs) Handles ManufacturerMasterButton.Click
    OpenForm("M04")
  End Sub
  Private Sub PackingButton_Click(sender As Object, e As EventArgs) Handles PackingButton.Click
    OpenForm("M05")
  End Sub
  Private Sub ScalesMasterButton_Click(sender As Object, e As EventArgs) Handles ScalesMasterButton.Click
    OpenForm("M06")
  End Sub

  Private Sub Free1MasterButton_Click(sender As Object, e As EventArgs) Handles Free1MasterButton.Click
    OpenForm("M07")
  End Sub

  Private Sub Free2MasterButton_Click(sender As Object, e As EventArgs) Handles Free2MasterButton.Click
    OpenForm("M08")
  End Sub

  Private Sub Free3MasterButton_Click(sender As Object, e As EventArgs) Handles Free3MasterButton.Click
    OpenForm("M09")
  End Sub

  Private Sub Free4MasterButton_Click(sender As Object, e As EventArgs) Handles Free4MasterButton.Click
    OpenForm("M10")
  End Sub

  Private Sub Free5MasterButton_Click(sender As Object, e As EventArgs) Handles Free5MasterButton.Click
    OpenForm("M11")
  End Sub

  Private Sub CloseButton_Click(sender As Object, e As EventArgs) Handles CloseButton.Click
    Close()
  End Sub

  Private Sub Form_MasterMente_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
    Select Case e.KeyCode
      Case Keys.F1
        ItemMasterButton.PerformClick()
      Case Keys.F2
        WorkOrderButton.PerformClick()
      Case Keys.F3
        StaffMasterButton.PerformClick()
      Case Keys.F4
        PackingButton.PerformClick()
      Case Keys.F5
        ScalesMasterButton.PerformClick()
      Case Keys.F7
        Free1MasterButton.PerformClick()
      Case Keys.F8
        Free2MasterButton.PerformClick()
      Case Keys.F9
        Free3MasterButton.PerformClick()
      Case Keys.F10
        Free4MasterButton.PerformClick()
      Case Keys.F11
        Free5MasterButton.PerformClick()
      Case Keys.Escape
        Me.Close()
    End Select
  End Sub

End Class