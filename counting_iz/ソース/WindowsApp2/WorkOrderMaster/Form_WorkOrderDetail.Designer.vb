<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form_WorkOrderDetail
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
        Me.CloseButton = New System.Windows.Forms.Button()
        Me.DetailIDText = New System.Windows.Forms.TextBox()
        Me.WorkOrderIDText = New System.Windows.Forms.TextBox()
        Me.NameLabel = New System.Windows.Forms.Label()
        Me.CodeLabel = New System.Windows.Forms.Label()
        Me.OkButton = New System.Windows.Forms.Button()
        Me.TitleLabel = New System.Windows.Forms.Label()
        Me.WorkOrderNameText = New System.Windows.Forms.TextBox()
        Me.OrderQuantityText = New System.Windows.Forms.TextBox()
        Me.IsDetailSubtotalCombo = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.ProductIDCombo = New System.Windows.Forms.ComboBox()
        Me.SuspendLayout()
        '
        'CloseButton
        '
        Me.CloseButton.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.CloseButton.Location = New System.Drawing.Point(455, 340)
        Me.CloseButton.Name = "CloseButton"
        Me.CloseButton.Size = New System.Drawing.Size(123, 50)
        Me.CloseButton.TabIndex = 17
        Me.CloseButton.Text = "ESC" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "終了"
        Me.CloseButton.UseVisualStyleBackColor = True
        '
        'DetailIDText
        '
        Me.DetailIDText.Font = New System.Drawing.Font("Segoe UI", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DetailIDText.Location = New System.Drawing.Point(208, 115)
        Me.DetailIDText.MaxLength = 2
        Me.DetailIDText.Name = "DetailIDText"
        Me.DetailIDText.Size = New System.Drawing.Size(206, 33)
        Me.DetailIDText.TabIndex = 13
        '
        'WorkOrderIDText
        '
        Me.WorkOrderIDText.Font = New System.Drawing.Font("Segoe UI", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.WorkOrderIDText.Location = New System.Drawing.Point(208, 75)
        Me.WorkOrderIDText.MaxLength = 2
        Me.WorkOrderIDText.Name = "WorkOrderIDText"
        Me.WorkOrderIDText.Size = New System.Drawing.Size(206, 33)
        Me.WorkOrderIDText.TabIndex = 11
        '
        'NameLabel
        '
        Me.NameLabel.AutoSize = True
        Me.NameLabel.Font = New System.Drawing.Font("Segoe UI", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.NameLabel.Location = New System.Drawing.Point(98, 115)
        Me.NameLabel.Name = "NameLabel"
        Me.NameLabel.Size = New System.Drawing.Size(107, 30)
        Me.NameLabel.TabIndex = 12
        Me.NameLabel.Text = "明細No："
        '
        'CodeLabel
        '
        Me.CodeLabel.AutoSize = True
        Me.CodeLabel.Font = New System.Drawing.Font("Segoe UI", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CodeLabel.Location = New System.Drawing.Point(54, 75)
        Me.CodeLabel.Name = "CodeLabel"
        Me.CodeLabel.Size = New System.Drawing.Size(151, 30)
        Me.CodeLabel.TabIndex = 10
        Me.CodeLabel.Text = "作業指示No："
        '
        'OkButton
        '
        Me.OkButton.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.OkButton.Location = New System.Drawing.Point(326, 340)
        Me.OkButton.Name = "OkButton"
        Me.OkButton.Size = New System.Drawing.Size(123, 50)
        Me.OkButton.TabIndex = 16
        Me.OkButton.Text = "F5" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "登録"
        Me.OkButton.UseVisualStyleBackColor = True
        '
        'TitleLabel
        '
        Me.TitleLabel.AutoSize = True
        Me.TitleLabel.Font = New System.Drawing.Font("Segoe UI", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TitleLabel.Location = New System.Drawing.Point(12, 10)
        Me.TitleLabel.Name = "TitleLabel"
        Me.TitleLabel.Size = New System.Drawing.Size(194, 30)
        Me.TitleLabel.TabIndex = 9
        Me.TitleLabel.Text = "作業指示マスタ詳細"
        '
        'WorkOrderNameText
        '
        Me.WorkOrderNameText.Font = New System.Drawing.Font("Segoe UI", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.WorkOrderNameText.Location = New System.Drawing.Point(208, 155)
        Me.WorkOrderNameText.MaxLength = 10
        Me.WorkOrderNameText.Name = "WorkOrderNameText"
        Me.WorkOrderNameText.Size = New System.Drawing.Size(206, 33)
        Me.WorkOrderNameText.TabIndex = 18
        '
        'OrderQuantityText
        '
        Me.OrderQuantityText.Font = New System.Drawing.Font("Segoe UI", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.OrderQuantityText.Location = New System.Drawing.Point(208, 275)
        Me.OrderQuantityText.MaxLength = 6
        Me.OrderQuantityText.Name = "OrderQuantityText"
        Me.OrderQuantityText.Size = New System.Drawing.Size(206, 33)
        Me.OrderQuantityText.TabIndex = 21
        '
        'IsDetailSubtotalCombo
        '
        Me.IsDetailSubtotalCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.IsDetailSubtotalCombo.Font = New System.Drawing.Font("Segoe UI", 14.25!)
        Me.IsDetailSubtotalCombo.FormattingEnabled = True
        Me.IsDetailSubtotalCombo.Location = New System.Drawing.Point(208, 195)
        Me.IsDetailSubtotalCombo.Name = "IsDetailSubtotalCombo"
        Me.IsDetailSubtotalCombo.Size = New System.Drawing.Size(206, 33)
        Me.IsDetailSubtotalCombo.TabIndex = 22
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Segoe UI", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(38, 155)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(167, 30)
        Me.Label1.TabIndex = 23
        Me.Label1.Text = "作業指示名称："
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Segoe UI", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(83, 195)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(123, 30)
        Me.Label2.TabIndex = 24
        Me.Label2.Text = "明細小計："
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Segoe UI", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(99, 235)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(107, 30)
        Me.Label3.TabIndex = 25
        Me.Label3.Text = "商品No："
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Segoe UI", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(99, 275)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(101, 30)
        Me.Label4.TabIndex = 26
        Me.Label4.Text = "指示数："
        '
        'ProductIDCombo
        '
        Me.ProductIDCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ProductIDCombo.Font = New System.Drawing.Font("Segoe UI", 14.25!)
        Me.ProductIDCombo.FormattingEnabled = True
        Me.ProductIDCombo.Location = New System.Drawing.Point(208, 235)
        Me.ProductIDCombo.Name = "ProductIDCombo"
        Me.ProductIDCombo.Size = New System.Drawing.Size(206, 33)
        Me.ProductIDCombo.TabIndex = 29
        '
        'Form_WorkOrderDetail
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(590, 402)
        Me.Controls.Add(Me.ProductIDCombo)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.IsDetailSubtotalCombo)
        Me.Controls.Add(Me.OrderQuantityText)
        Me.Controls.Add(Me.WorkOrderNameText)
        Me.Controls.Add(Me.CloseButton)
        Me.Controls.Add(Me.DetailIDText)
        Me.Controls.Add(Me.WorkOrderIDText)
        Me.Controls.Add(Me.NameLabel)
        Me.Controls.Add(Me.CodeLabel)
        Me.Controls.Add(Me.OkButton)
        Me.Controls.Add(Me.TitleLabel)
        Me.Name = "Form_WorkOrderDetail"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Form2"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents CloseButton As Button
    Friend WithEvents DetailIDText As TextBox
    Friend WithEvents WorkOrderIDText As TextBox
    Friend WithEvents NameLabel As Label
    Friend WithEvents CodeLabel As Label
    Friend WithEvents OkButton As Button
    Friend WithEvents TitleLabel As Label
    Friend WithEvents WorkOrderNameText As TextBox
    Friend WithEvents OrderQuantityText As TextBox
    Friend WithEvents IsDetailSubtotalCombo As ComboBox
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents ProductIDCombo As ComboBox
End Class
