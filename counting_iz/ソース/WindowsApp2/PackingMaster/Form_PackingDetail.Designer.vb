<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form_PackingDetail
    Inherits System.Windows.Forms.Form

    'フォームがコンポーネントの一覧をクリーンアップするために dispose をオーバーライドします。
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

    'Windows フォーム デザイナーで必要です。
    Private components As System.ComponentModel.IContainer

    'メモ: 以下のプロシージャは Windows フォーム デザイナーで必要です。
    'Windows フォーム デザイナーを使用して変更できます。  
    'コード エディターを使って変更しないでください。
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.NameText = New System.Windows.Forms.TextBox()
        Me.CodeText = New System.Windows.Forms.TextBox()
        Me.NameLabel = New System.Windows.Forms.Label()
        Me.CodeLabel = New System.Windows.Forms.Label()
        Me.OkButton = New System.Windows.Forms.Button()
        Me.TitleLabel = New System.Windows.Forms.Label()
        Me.WeightText = New System.Windows.Forms.TextBox()
        Me.WeightLabel = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.CloseButton = New System.Windows.Forms.Button()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.UnitComboBox = New System.Windows.Forms.ComboBox()
        Me.SuspendLayout()
        '
        'NameText
        '
        Me.NameText.Font = New System.Drawing.Font("Segoe UI", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.NameText.Location = New System.Drawing.Point(106, 94)
        Me.NameText.MaxLength = 20
        Me.NameText.Name = "NameText"
        Me.NameText.Size = New System.Drawing.Size(210, 33)
        Me.NameText.TabIndex = 4
        '
        'CodeText
        '
        Me.CodeText.Font = New System.Drawing.Font("Segoe UI", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CodeText.Location = New System.Drawing.Point(106, 55)
        Me.CodeText.Name = "CodeText"
        Me.CodeText.Size = New System.Drawing.Size(105, 33)
        Me.CodeText.TabIndex = 2
        '
        'NameLabel
        '
        Me.NameLabel.AutoSize = True
        Me.NameLabel.Font = New System.Drawing.Font("Segoe UI", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.NameLabel.Location = New System.Drawing.Point(12, 97)
        Me.NameLabel.Name = "NameLabel"
        Me.NameLabel.Size = New System.Drawing.Size(94, 30)
        Me.NameLabel.TabIndex = 3
        Me.NameLabel.Text = "名　称："
        '
        'CodeLabel
        '
        Me.CodeLabel.AutoSize = True
        Me.CodeLabel.Font = New System.Drawing.Font("Segoe UI", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CodeLabel.Location = New System.Drawing.Point(22, 58)
        Me.CodeLabel.Name = "CodeLabel"
        Me.CodeLabel.Size = New System.Drawing.Size(84, 30)
        Me.CodeLabel.TabIndex = 1
        Me.CodeLabel.Text = "コード："
        '
        'OkButton
        '
        Me.OkButton.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.OkButton.Location = New System.Drawing.Point(270, 263)
        Me.OkButton.Name = "OkButton"
        Me.OkButton.Size = New System.Drawing.Size(123, 50)
        Me.OkButton.TabIndex = 10
        Me.OkButton.Text = "F5" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "登録"
        Me.OkButton.UseVisualStyleBackColor = True
        '
        'TitleLabel
        '
        Me.TitleLabel.AutoSize = True
        Me.TitleLabel.Font = New System.Drawing.Font("Segoe UI", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TitleLabel.Location = New System.Drawing.Point(12, 9)
        Me.TitleLabel.Name = "TitleLabel"
        Me.TitleLabel.Size = New System.Drawing.Size(150, 30)
        Me.TitleLabel.TabIndex = 0
        Me.TitleLabel.Text = "風袋マスタ詳細"
        '
        'WeightText
        '
        Me.WeightText.Font = New System.Drawing.Font("Segoe UI", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.WeightText.Location = New System.Drawing.Point(106, 133)
        Me.WeightText.MaxLength = 6
        Me.WeightText.Name = "WeightText"
        Me.WeightText.Size = New System.Drawing.Size(105, 33)
        Me.WeightText.TabIndex = 6
        '
        'WeightLabel
        '
        Me.WeightLabel.AutoSize = True
        Me.WeightLabel.Font = New System.Drawing.Font("Segoe UI", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.WeightLabel.Location = New System.Drawing.Point(12, 136)
        Me.WeightLabel.Name = "WeightLabel"
        Me.WeightLabel.Size = New System.Drawing.Size(94, 30)
        Me.WeightLabel.TabIndex = 5
        Me.WeightLabel.Text = "重　量："
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Segoe UI", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(217, 136)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(47, 30)
        Me.Label2.TabIndex = 7
        Me.Label2.Text = "(kg)"
        '
        'CloseButton
        '
        Me.CloseButton.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.CloseButton.Location = New System.Drawing.Point(399, 263)
        Me.CloseButton.Name = "CloseButton"
        Me.CloseButton.Size = New System.Drawing.Size(123, 50)
        Me.CloseButton.TabIndex = 11
        Me.CloseButton.Text = "ESC" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "終了"
        Me.CloseButton.UseVisualStyleBackColor = True
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Font = New System.Drawing.Font("Segoe UI", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.Location = New System.Drawing.Point(12, 173)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(94, 30)
        Me.Label12.TabIndex = 8
        Me.Label12.Text = "単　位："
        '
        'UnitComboBox
        '
        Me.UnitComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.UnitComboBox.Font = New System.Drawing.Font("Segoe UI", 14.25!)
        Me.UnitComboBox.FormattingEnabled = True
        Me.UnitComboBox.Location = New System.Drawing.Point(106, 174)
        Me.UnitComboBox.Name = "UnitComboBox"
        Me.UnitComboBox.Size = New System.Drawing.Size(105, 33)
        Me.UnitComboBox.TabIndex = 12
        '
        'Form_PackingDetail
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(534, 325)
        Me.Controls.Add(Me.UnitComboBox)
        Me.Controls.Add(Me.Label12)
        Me.Controls.Add(Me.CloseButton)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.WeightText)
        Me.Controls.Add(Me.WeightLabel)
        Me.Controls.Add(Me.NameText)
        Me.Controls.Add(Me.CodeText)
        Me.Controls.Add(Me.NameLabel)
        Me.Controls.Add(Me.CodeLabel)
        Me.Controls.Add(Me.OkButton)
        Me.Controls.Add(Me.TitleLabel)
        Me.Name = "Form_PackingDetail"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Form_PackingDetail"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents NameText As TextBox
    Friend WithEvents CodeText As TextBox
    Friend WithEvents NameLabel As Label
    Friend WithEvents CodeLabel As Label
    Friend WithEvents OkButton As Button
    Friend WithEvents TitleLabel As Label
    Friend WithEvents WeightText As TextBox
    Friend WithEvents WeightLabel As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents CloseButton As Button
    Friend WithEvents Label12 As Label
    Friend WithEvents UnitComboBox As ComboBox
End Class
