Imports System.Windows.Forms

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form_InputPassword
  Inherits System.Windows.Forms.Form

  <System.Diagnostics.DebuggerNonUserCode()> _
  Protected Overrides Sub Dispose(ByVal disposing As Boolean)
    Try
      If disposing AndAlso components IsNot Nothing Then
        components.Dispose()
      End If
    Finally
      MyBase.Dispose(disposing)
    End Try
  End Sub

  Private components As System.ComponentModel.IContainer

  <System.Diagnostics.DebuggerStepThrough()> _
  Private Sub InitializeComponent()
    Me.MessageLabel = New System.Windows.Forms.Label()
    Me.PW_TextBox = New System.Windows.Forms.TextBox()
    Me.EnterButton = New System.Windows.Forms.Button()
    Me.CancelActionButton = New System.Windows.Forms.Button()
    Me.SuspendLayout()
    '
    'MessageLabel
    '
    Me.MessageLabel.AutoSize = True
    Me.MessageLabel.Font = New System.Drawing.Font("Meiryo UI", 12.0!)
    Me.MessageLabel.Location = New System.Drawing.Point(25, 24)
    Me.MessageLabel.Name = "MessageLabel"
    Me.MessageLabel.Size = New System.Drawing.Size(250, 21)
    Me.MessageLabel.TabIndex = 0
    Me.MessageLabel.Text = "パスワードを入力してください。"
    '
    'PW_TextBox
    '
    Me.PW_TextBox.Font = New System.Drawing.Font("Meiryo UI", 14.25!)
    Me.PW_TextBox.Location = New System.Drawing.Point(29, 59)
    Me.PW_TextBox.MaxLength = 20
    Me.PW_TextBox.Name = "PW_TextBox"
    Me.PW_TextBox.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
    Me.PW_TextBox.Size = New System.Drawing.Size(326, 33)
    Me.PW_TextBox.TabIndex = 1
    '
    'EnterButton
    '
    Me.EnterButton.Font = New System.Drawing.Font("Meiryo UI", 10.0!)
    Me.EnterButton.Location = New System.Drawing.Point(179, 112)
    Me.EnterButton.Name = "EnterButton"
    Me.EnterButton.Size = New System.Drawing.Size(85, 36)
    Me.EnterButton.TabIndex = 2
    Me.EnterButton.Text = "OK"
    Me.EnterButton.UseVisualStyleBackColor = True
    '
    'CancelActionButton
    '
    Me.CancelActionButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
    Me.CancelActionButton.Font = New System.Drawing.Font("Meiryo UI", 10.0!)
    Me.CancelActionButton.Location = New System.Drawing.Point(270, 112)
    Me.CancelActionButton.Name = "CancelActionButton"
    Me.CancelActionButton.Size = New System.Drawing.Size(85, 36)
    Me.CancelActionButton.TabIndex = 3
    Me.CancelActionButton.Text = "キャンセル"
    Me.CancelActionButton.UseVisualStyleBackColor = True
    '
    'Form_InputPassword
    '
    Me.AcceptButton = Me.EnterButton
    Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
    Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    Me.CancelButton = Me.CancelActionButton
    Me.ClientSize = New System.Drawing.Size(384, 171)
    Me.Controls.Add(Me.CancelActionButton)
    Me.Controls.Add(Me.EnterButton)
    Me.Controls.Add(Me.PW_TextBox)
    Me.Controls.Add(Me.MessageLabel)
    Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
    Me.MaximizeBox = False
    Me.MinimizeBox = False
        Me.Font = New System.Drawing.Font("Meiryo UI", 9.0!)
        Me.ControlBox = False
    Me.Name = "Form_InputPassword"
    Me.ShowInTaskbar = False
    Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
    Me.Text = "パスワード入力"
    Me.ResumeLayout(False)
    Me.PerformLayout()
  End Sub

  Friend WithEvents MessageLabel As Label
  Friend WithEvents PW_TextBox As TextBox
  Friend WithEvents EnterButton As Button
  Friend WithEvents CancelActionButton As Button
End Class
