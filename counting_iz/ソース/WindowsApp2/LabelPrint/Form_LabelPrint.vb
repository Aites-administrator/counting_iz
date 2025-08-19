Public Class Form_LabelPrint
  Private Sub Form_LabelPrint_Load(sender As Object, e As EventArgs) Handles MyBase.Load
    MaximizeBox = False
    Dim updateTime As DateTime = System.IO.File.GetLastWriteTime(System.Reflection.Assembly.GetExecutingAssembly().Location)
    Text = "ラベル印刷枚数設定" & " ( " & updateTime & " ) "
    StartPosition = FormStartPosition.CenterScreen
    FormBorderStyle = FormBorderStyle.FixedSingle
    PagesNumberText.Text = 1
  End Sub
  Private Sub PagesNumberText_KeyPress(sender As Object, e As KeyPressEventArgs) Handles PagesNumberText.KeyPress
    'キーが [0]～[9] または [BackSpace] 以外の場合イベントをキャンセル
    If Not (("0"c <= e.KeyChar And e.KeyChar <= "9"c) Or e.KeyChar = ControlChars.Back) Then
      'コントロールの既定の処理を省略する場合は true
      e.Handled = True
    End If
  End Sub
  Private Sub CloseButton_Click(sender As Object, e As EventArgs)
    Close()
  End Sub
End Class
