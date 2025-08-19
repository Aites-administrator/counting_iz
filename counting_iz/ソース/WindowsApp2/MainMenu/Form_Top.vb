Imports Common
Imports Common.ClsFunction
Public Class Form_Top
  Private ReadOnly SqlServerBatFile As String = ReadSettingIniFile("SQLSERVER_BATFILE", "VALUE")
  Private Sub Form_Top_Load(sender As Object, e As EventArgs) Handles MyBase.Load
    MaximizeBox = False
    FormBorderStyle = FormBorderStyle.FixedSingle
    ' キーイベントをフォーム全体で受け取るようにする
    Me.KeyPreview = True
  End Sub
  Private Sub PictureBox_MasterMente_Click(sender As Object, e As EventArgs) Handles PictureBox_MasterMente.Click
    Form_MasterMente.ShowDialog()
  End Sub
  Private Sub PictureBox_AchievementMente_Click(sender As Object, e As EventArgs) Handles PictureBox_AchievementMente.Click
    Form_Results.ShowDialog()
  End Sub
  Private Sub PictureBox_OutPut_Click(sender As Object, e As EventArgs) Handles PictureBox_OutPut.Click
    Form_OutPut.ShowDialog()
  End Sub

  Private Sub PictureBox_Log_Click(sender As Object, e As EventArgs) Handles PictureBox_Log.Click
    Form_Log.ShowDialog()
  End Sub

  Private Sub PictureBox_MasterMente_MouseMove(sender As Object, e As MouseEventArgs) Handles PictureBox_MasterMente.MouseMove
    PictureBox_MasterMente.BorderStyle = BorderStyle.FixedSingle
  End Sub
  Private Sub PictureBox_MasterMente_MouseLeave(sender As Object, e As EventArgs) Handles PictureBox_MasterMente.MouseLeave
    PictureBox_MasterMente.BorderStyle = BorderStyle.None
  End Sub
  Private Sub PictureBox_MasterMente_MouseEnter(sender As Object, e As EventArgs) Handles PictureBox_MasterMente.MouseEnter
    PictureBox_MasterMente.BorderStyle = BorderStyle.FixedSingle
  End Sub

  Private Sub PictureBox_MasterMente_MouseUp(sender As Object, e As MouseEventArgs) Handles PictureBox_MasterMente.MouseUp
    PictureBox_MasterMente.BorderStyle = BorderStyle.None
  End Sub
  Private Sub PictureBox_OutPut_MouseMove(sender As Object, e As MouseEventArgs) Handles PictureBox_OutPut.MouseMove
    PictureBox_OutPut.BorderStyle = BorderStyle.FixedSingle
  End Sub
  Private Sub PictureBox_OutPut_MouseLeave(sender As Object, e As EventArgs) Handles PictureBox_OutPut.MouseLeave
    PictureBox_OutPut.BorderStyle = BorderStyle.None
  End Sub
  Private Sub PictureBox_OutPut_MouseEnter(sender As Object, e As EventArgs) Handles PictureBox_OutPut.MouseEnter
    PictureBox_OutPut.BorderStyle = BorderStyle.FixedSingle
  End Sub
  Private Sub PictureBox_OutPut_MouseUp(sender As Object, e As MouseEventArgs) Handles PictureBox_OutPut.MouseUp
    PictureBox_OutPut.BorderStyle = BorderStyle.None
  End Sub

  Private Sub PictureBox_Log_MouseMove(sender As Object, e As MouseEventArgs) Handles PictureBox_Log.MouseMove
    PictureBox_Log.BorderStyle = BorderStyle.FixedSingle
  End Sub

  Private Sub PictureBox_Log_MouseLeave(sender As Object, e As EventArgs) Handles PictureBox_Log.MouseLeave
    PictureBox_Log.BorderStyle = BorderStyle.None
  End Sub

  Private Sub PictureBox_Log_MouseEnter(sender As Object, e As EventArgs) Handles PictureBox_Log.MouseEnter
    PictureBox_Log.BorderStyle = BorderStyle.FixedSingle
  End Sub

  Private Sub PictureBox_Log_MouseUp(sender As Object, e As MouseEventArgs) Handles PictureBox_Log.MouseUp
    PictureBox_Log.BorderStyle = BorderStyle.None
  End Sub

  Private Sub Form_Top_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
    Select Case e.KeyCode
      Case Keys.F1
        MasterMenteButton.PerformClick()
      Case Keys.F2
        ResultsButton.PerformClick()
      Case Keys.F3
        OutPutButton.PerformClick()
      Case Keys.F4
        LogButton.PerformClick()
      Case Keys.Escape
        Me.Close()
      Case Keys.S
        Support_MenuItem.PerformClick()
      Case Keys.B
        RunSqlServerBat.PerformClick()
    End Select
  End Sub

  Private Sub PictureBox_AchievementMente_MouseEnter(sender As Object, e As EventArgs) Handles PictureBox_AchievementMente.MouseEnter
    PictureBox_AchievementMente.BorderStyle = BorderStyle.FixedSingle
  End Sub
  Private Sub PictureBox_AchievementMente_MouseLeave(sender As Object, e As EventArgs) Handles PictureBox_AchievementMente.MouseLeave
    PictureBox_AchievementMente.BorderStyle = BorderStyle.None
  End Sub
  Private Sub PictureBox_AchievementMente_MouseMove(sender As Object, e As MouseEventArgs) Handles PictureBox_AchievementMente.MouseMove
    PictureBox_AchievementMente.BorderStyle = BorderStyle.FixedSingle
  End Sub
  Private Sub PictureBox_AchievementMente_MouseUp(sender As Object, e As MouseEventArgs) Handles PictureBox_AchievementMente.MouseUp
    PictureBox_AchievementMente.BorderStyle = BorderStyle.None
  End Sub
  Private Sub PictureBox_Close_Click(sender As Object, e As EventArgs) Handles PictureBox_Close.Click
    Close()
  End Sub
  Private Sub CloseButton_Click(sender As Object, e As EventArgs) Handles CloseButton.Click
    Close()
  End Sub
  Private Sub PictureBox_Close_MouseEnter(sender As Object, e As EventArgs) Handles PictureBox_Close.MouseEnter
    PictureBox_Close.BorderStyle = BorderStyle.FixedSingle
  End Sub
  Private Sub PictureBox_Close_MouseLeave(sender As Object, e As EventArgs) Handles PictureBox_Close.MouseLeave
    PictureBox_Close.BorderStyle = BorderStyle.None
  End Sub
  Private Sub PictureBox_Close_MouseMove(sender As Object, e As MouseEventArgs) Handles PictureBox_Close.MouseMove
    PictureBox_Close.BorderStyle = BorderStyle.FixedSingle
  End Sub
  Private Sub PictureBox_Close_MouseUp(sender As Object, e As MouseEventArgs) Handles PictureBox_Close.MouseUp
    PictureBox_Close.BorderStyle = BorderStyle.None
  End Sub
  Private Sub MasterMenteButton_Click(sender As Object, e As EventArgs) Handles MasterMenteButton.Click
    Form_MasterMente.ShowDialog()
  End Sub
  Private Sub ResultsButton_Click(sender As Object, e As EventArgs) Handles ResultsButton.Click
    Form_Results.ShowDialog()
  End Sub
  Private Sub OutPutButton_Click(sender As Object, e As EventArgs) Handles OutPutButton.Click
    Form_OutPut.ShowDialog()
  End Sub
  Private Sub LogButton_Click(sender As Object, e As EventArgs) Handles LogButton.Click
    Form_Log.ShowDialog()
  End Sub
  Private Sub Support_MenuItem_Click(sender As Object, e As EventArgs) Handles Support_MenuItem.Click
    Form_Information.ShowDialog()
  End Sub

  Private Sub RunSqlServerBat_Click(sender As Object, e As EventArgs) Handles RunSqlServerBat.Click
    ExecuteBatchFile()
  End Sub


  Private Sub ExecuteBatchFile()
    Dim result As DialogResult = MessageBox.Show(
        "アプリケーションを終了してバックアップを実行しますか？",
        "バックアップの確認",
        MessageBoxButtons.OKCancel,
        MessageBoxIcon.Question
    )

    If result = DialogResult.OK Then
      Dim batFilePath As String = SqlServerBatFile

      If System.IO.File.Exists(batFilePath) Then
        Dim psi As New ProcessStartInfo()
        psi.FileName = "cmd.exe"
        psi.Arguments = "/C timeout /t 3 & """ & batFilePath & """"
        psi.UseShellExecute = True
        psi.WindowStyle = ProcessWindowStyle.Normal

        Process.Start(psi)

        ' アプリケーション終了（再起動はバッチの最後で行う）
        Application.Exit()
      Else
        MessageBox.Show("バッチファイルが見つかりませんでした: " & batFilePath, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
      End If
    Else
      MessageBox.Show("バックアップはキャンセルされました。", "キャンセル", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End If
  End Sub
End Class