<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form_OutPut
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
        Me.MonthlyReportOutputButton = New System.Windows.Forms.Button()
        Me.DetailOutputButton = New System.Windows.Forms.Button()
        Me.TitleLabel = New System.Windows.Forms.Label()
        Me.CloseButton = New System.Windows.Forms.Button()
        Me.WeightCheckButton = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'MonthlyReportOutputButton
        '
        Me.MonthlyReportOutputButton.Font = New System.Drawing.Font("Segoe UI", 18.0!)
        Me.MonthlyReportOutputButton.Location = New System.Drawing.Point(12, 72)
        Me.MonthlyReportOutputButton.Name = "MonthlyReportOutputButton"
        Me.MonthlyReportOutputButton.Size = New System.Drawing.Size(407, 79)
        Me.MonthlyReportOutputButton.TabIndex = 5
        Me.MonthlyReportOutputButton.Text = "F1" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "月報出力"
        Me.MonthlyReportOutputButton.UseVisualStyleBackColor = True
        '
        'DetailOutputButton
        '
        Me.DetailOutputButton.Font = New System.Drawing.Font("Segoe UI", 20.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DetailOutputButton.Location = New System.Drawing.Point(12, 260)
        Me.DetailOutputButton.Name = "DetailOutputButton"
        Me.DetailOutputButton.Size = New System.Drawing.Size(407, 79)
        Me.DetailOutputButton.TabIndex = 4
        Me.DetailOutputButton.Text = "明細出力"
        Me.DetailOutputButton.UseVisualStyleBackColor = True
        Me.DetailOutputButton.Visible = False
        '
        'TitleLabel
        '
        Me.TitleLabel.AutoSize = True
        Me.TitleLabel.Font = New System.Drawing.Font("Segoe UI", 24.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TitleLabel.Location = New System.Drawing.Point(12, 9)
        Me.TitleLabel.Name = "TitleLabel"
        Me.TitleLabel.Size = New System.Drawing.Size(229, 45)
        Me.TitleLabel.TabIndex = 15
        Me.TitleLabel.Text = "累計データ管理"
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
        'WeightCheckButton
        '
        Me.WeightCheckButton.Font = New System.Drawing.Font("Segoe UI", 18.0!)
        Me.WeightCheckButton.Location = New System.Drawing.Point(426, 72)
        Me.WeightCheckButton.Name = "WeightCheckButton"
        Me.WeightCheckButton.Size = New System.Drawing.Size(407, 79)
        Me.WeightCheckButton.TabIndex = 17
        Me.WeightCheckButton.Text = "F2" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "重量検査一覧表出力"
        Me.WeightCheckButton.UseVisualStyleBackColor = True
        Me.WeightCheckButton.Visible = False
        '
        'Form_OutPut
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(845, 648)
        Me.Controls.Add(Me.WeightCheckButton)
        Me.Controls.Add(Me.CloseButton)
        Me.Controls.Add(Me.TitleLabel)
        Me.Controls.Add(Me.MonthlyReportOutputButton)
        Me.Controls.Add(Me.DetailOutputButton)
        Me.Name = "Form_OutPut"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "累計データ管理"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents MonthlyReportOutputButton As Button
    Friend WithEvents DetailOutputButton As Button
    Friend WithEvents TitleLabel As Label
    Friend WithEvents CloseButton As Button
    Friend WithEvents WeightCheckButton As Button
End Class
