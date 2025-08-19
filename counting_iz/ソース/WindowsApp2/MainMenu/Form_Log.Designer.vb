<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form_Log
    Inherits System.Windows.Forms.Form

    'フォームがコンポーネントの一覧をクリーンアップするために dispose をオーバーライドします。
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Windows フォーム デザイナーで必要です。
    Private components As System.ComponentModel.IContainer

    'メモ: 以下のプロシージャは Windows フォーム デザイナーで必要です。
    'Windows フォーム デザイナーを使用して変更できます。  
    'コード エディターを使って変更しないでください。
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.RealtimeConfirmation_Button = New System.Windows.Forms.Button()
        Me.LogDisplayButton = New System.Windows.Forms.Button()
        Me.TitleLabel = New System.Windows.Forms.Label()
        Me.CloseButton = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'RealtimeConfirmation_Button
        '
        Me.RealtimeConfirmation_Button.Font = New System.Drawing.Font("Segoe UI", 18.0!)
        Me.RealtimeConfirmation_Button.Location = New System.Drawing.Point(12, 72)
        Me.RealtimeConfirmation_Button.Name = "RealtimeConfirmation_Button"
        Me.RealtimeConfirmation_Button.Size = New System.Drawing.Size(407, 79)
        Me.RealtimeConfirmation_Button.TabIndex = 6
        Me.RealtimeConfirmation_Button.Text = "F1" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "計量器通信"
        Me.RealtimeConfirmation_Button.UseVisualStyleBackColor = True
        '
        'LogDisplayButton
        '
        Me.LogDisplayButton.Font = New System.Drawing.Font("Segoe UI", 18.0!)
        Me.LogDisplayButton.Location = New System.Drawing.Point(426, 72)
        Me.LogDisplayButton.Name = "LogDisplayButton"
        Me.LogDisplayButton.Size = New System.Drawing.Size(407, 79)
        Me.LogDisplayButton.TabIndex = 7
        Me.LogDisplayButton.Text = "F2" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "ログ一覧"
        Me.LogDisplayButton.UseVisualStyleBackColor = True
        '
        'TitleLabel
        '
        Me.TitleLabel.AutoSize = True
        Me.TitleLabel.Font = New System.Drawing.Font("Segoe UI", 24.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TitleLabel.Location = New System.Drawing.Point(12, 9)
        Me.TitleLabel.Name = "TitleLabel"
        Me.TitleLabel.Size = New System.Drawing.Size(185, 45)
        Me.TitleLabel.TabIndex = 15
        Me.TitleLabel.Text = "計量器管理"
        Me.TitleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'CloseButton
        '
        Me.CloseButton.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.CloseButton.Location = New System.Drawing.Point(710, 586)
        Me.CloseButton.Name = "CloseButton"
        Me.CloseButton.Size = New System.Drawing.Size(123, 50)
        Me.CloseButton.TabIndex = 16
        Me.CloseButton.Text = "ESC" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "終了"
        Me.CloseButton.UseVisualStyleBackColor = True
        '
        'Form_Log
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(845, 648)
        Me.Controls.Add(Me.CloseButton)
        Me.Controls.Add(Me.TitleLabel)
        Me.Controls.Add(Me.RealtimeConfirmation_Button)
        Me.Controls.Add(Me.LogDisplayButton)
        Me.Name = "Form_Log"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "計量器管理"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents RealtimeConfirmation_Button As Button
    Friend WithEvents LogDisplayButton As Button
    Friend WithEvents TitleLabel As Label
    Friend WithEvents CloseButton As Button
End Class
