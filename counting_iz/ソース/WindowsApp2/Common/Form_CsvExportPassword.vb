Imports Common.ClsFunction
Imports System.Windows.Forms

Public Class Form_CsvExportPassword
  Private ReadOnly ConfiguredPassword As String

  Public Sub New()
    InitializeComponent()
    ConfiguredPassword = ReadSettingIniFile("CSV_EXPORT_PASSWORD", "VALUE")
  End Sub

  Private Sub Form_CsvExportPassword_Load(sender As Object, e As EventArgs) Handles MyBase.Load
    PasswordTextBox.Clear()
    PasswordTextBox.Focus()
  End Sub

  Private Sub AuthenticateButton_Click(sender As Object, e As EventArgs) Handles AuthenticateButton.Click
    If String.IsNullOrWhiteSpace(ConfiguredPassword) Then
      MessageBox.Show("CSV出力用パスワードがINIファイルに設定されていません。", "設定エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
      Return
    End If

    If String.Equals(PasswordTextBox.Text, ConfiguredPassword, StringComparison.Ordinal) Then
      Me.DialogResult = System.Windows.Forms.DialogResult.OK
      Close()
      Return
    End If

    MessageBox.Show("パスワードに誤りがあります。" & vbCrLf & "正しいパスワードを入力してください。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
    PasswordTextBox.Clear()
    PasswordTextBox.Focus()
  End Sub
End Class
