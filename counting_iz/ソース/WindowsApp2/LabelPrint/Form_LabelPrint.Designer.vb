<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form_LabelPrint
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
        Me.TitleLabel = New System.Windows.Forms.Label()
        Me.PagesNumberLabel = New System.Windows.Forms.Label()
        Me.PagesNumberText = New System.Windows.Forms.TextBox()
        Me.PrintButton = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'TitleLabel
        '
        Me.TitleLabel.AutoSize = True
        Me.TitleLabel.Font = New System.Drawing.Font("MS UI Gothic", 20.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.TitleLabel.Location = New System.Drawing.Point(12, 9)
        Me.TitleLabel.Name = "TitleLabel"
        Me.TitleLabel.Size = New System.Drawing.Size(237, 27)
        Me.TitleLabel.TabIndex = 0
        Me.TitleLabel.Text = "ラベル印刷枚数設定"
        '
        'PagesNumberLabel
        '
        Me.PagesNumberLabel.AutoSize = True
        Me.PagesNumberLabel.Font = New System.Drawing.Font("MS UI Gothic", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.PagesNumberLabel.Location = New System.Drawing.Point(28, 73)
        Me.PagesNumberLabel.Name = "PagesNumberLabel"
        Me.PagesNumberLabel.Size = New System.Drawing.Size(70, 21)
        Me.PagesNumberLabel.TabIndex = 1
        Me.PagesNumberLabel.Text = "枚　数:"
        '
        'PagesNumberText
        '
        Me.PagesNumberText.Font = New System.Drawing.Font("MS UI Gothic", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.PagesNumberText.Location = New System.Drawing.Point(102, 70)
        Me.PagesNumberText.Name = "PagesNumberText"
        Me.PagesNumberText.Size = New System.Drawing.Size(64, 28)
        Me.PagesNumberText.TabIndex = 2
        Me.PagesNumberText.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'PrintButton
        '
        Me.PrintButton.Font = New System.Drawing.Font("MS UI Gothic", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.PrintButton.Location = New System.Drawing.Point(102, 164)
        Me.PrintButton.Name = "PrintButton"
        Me.PrintButton.Size = New System.Drawing.Size(123, 43)
        Me.PrintButton.TabIndex = 3
        Me.PrintButton.Text = "印刷"
        Me.PrintButton.UseVisualStyleBackColor = True
        '
        'Form_LabelPrint
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(350, 240)
        Me.Controls.Add(Me.PrintButton)
        Me.Controls.Add(Me.PagesNumberText)
        Me.Controls.Add(Me.PagesNumberLabel)
        Me.Controls.Add(Me.TitleLabel)
        Me.Name = "Form_LabelPrint"
        StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents TitleLabel As Label
    Friend WithEvents PagesNumberLabel As Label
    Friend WithEvents PagesNumberText As TextBox
    Friend WithEvents PrintButton As Button
End Class
