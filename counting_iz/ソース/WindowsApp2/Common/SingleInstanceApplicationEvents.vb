Imports Microsoft.VisualBasic.ApplicationServices
Imports System.Windows.Forms

Namespace My
  Partial Friend Class MyApplication
    Private DuplicateMessageVisible As Integer

    ''' <summary>
    ''' 起動済みの画面をもう一度起動しようとした場合、既存画面を前面に戻して案内します。
    ''' </summary>
    Private Sub MyApplication_StartupNextInstance(
        sender As Object,
        e As StartupNextInstanceEventArgs
    ) Handles Me.StartupNextInstance
      e.BringToForeground = True

      If Me.MainForm Is Nothing OrElse Me.MainForm.IsDisposed Then
        Return
      End If

      If Me.MainForm.WindowState = FormWindowState.Minimized Then
        Me.MainForm.WindowState = FormWindowState.Normal
      End If

      Me.MainForm.Show()
      Me.MainForm.BringToFront()
      Me.MainForm.Activate()

      ' メッセージ表示中にも二重起動イベントが届くため、複数のメッセージを重ねて表示しない。
      If Threading.Interlocked.CompareExchange(DuplicateMessageVisible, 1, 0) <> 0 Then
        Return
      End If

      Dim originalTopMost As Boolean = Me.MainForm.TopMost
      Try
        ' メッセージ表示中だけ既存画面を最前面にし、背面に隠れることを防ぐ。
        Me.MainForm.TopMost = True
        Me.MainForm.BringToFront()
        Me.MainForm.Activate()

        System.Windows.Forms.MessageBox.Show(
          Me.MainForm,
          "この画面は既に起動中です。",
          "二重起動",
          MessageBoxButtons.OK,
          MessageBoxIcon.Information
        )
      Finally
        If Not Me.MainForm.IsDisposed Then
          Me.MainForm.TopMost = originalTopMost
        End If
        Threading.Interlocked.Exchange(DuplicateMessageVisible, 0)
      End Try
    End Sub
  End Class
End Namespace
