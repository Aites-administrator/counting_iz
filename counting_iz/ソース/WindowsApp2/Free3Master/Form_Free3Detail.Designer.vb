<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form_Free3Detail
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
        Me.CodeText = New System.Windows.Forms.TextBox()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.NameText = New System.Windows.Forms.TextBox()
        Me.NameLabel = New System.Windows.Forms.Label()
        Me.CodeLabel = New System.Windows.Forms.Label()
        Me.CloseButton = New System.Windows.Forms.Button()
        Me.OkButton = New System.Windows.Forms.Button()
        Me.TitleLabel = New System.Windows.Forms.Label()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'CodeText
        '
        Me.CodeText.Font = New System.Drawing.Font("Segoe UI", 14.25!)
        Me.CodeText.Location = New System.Drawing.Point(100, 20)
        Me.CodeText.MaxLength = 4
        Me.CodeText.Name = "CodeText"
        Me.CodeText.Size = New System.Drawing.Size(201, 33)
        Me.CodeText.TabIndex = 1
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.NameText)
        Me.GroupBox1.Controls.Add(Me.NameLabel)
        Me.GroupBox1.Controls.Add(Me.CodeText)
        Me.GroupBox1.Controls.Add(Me.CodeLabel)
        Me.GroupBox1.Location = New System.Drawing.Point(17, 42)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(366, 110)
        Me.GroupBox1.TabIndex = 1
        Me.GroupBox1.TabStop = False
        '
        'NameText
        '
        Me.NameText.Font = New System.Drawing.Font("Segoe UI", 14.25!)
        Me.NameText.Location = New System.Drawing.Point(100, 62)
        Me.NameText.MaxLength = 99
        Me.NameText.Name = "NameText"
        Me.NameText.Size = New System.Drawing.Size(201, 33)
        Me.NameText.TabIndex = 3
        '
        'NameLabel
        '
        Me.NameLabel.AutoSize = True
        Me.NameLabel.Font = New System.Drawing.Font("Segoe UI", 15.75!)
        Me.NameLabel.Location = New System.Drawing.Point(6, 65)
        Me.NameLabel.Name = "NameLabel"
        Me.NameLabel.Size = New System.Drawing.Size(57, 30)
        Me.NameLabel.TabIndex = 2
        Me.NameLabel.Text = "名称"
        '
        'CodeLabel
        '
        Me.CodeLabel.AutoSize = True
        Me.CodeLabel.Font = New System.Drawing.Font("Segoe UI", 15.75!)
        Me.CodeLabel.Location = New System.Drawing.Point(6, 23)
        Me.CodeLabel.Name = "CodeLabel"
        Me.CodeLabel.Size = New System.Drawing.Size(62, 30)
        Me.CodeLabel.TabIndex = 0
        Me.CodeLabel.Text = "コード"
        '
        'CloseButton
        '
        Me.CloseButton.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.CloseButton.Location = New System.Drawing.Point(260, 198)
        Me.CloseButton.Name = "CloseButton"
        Me.CloseButton.Size = New System.Drawing.Size(123, 50)
        Me.CloseButton.TabIndex = 3
        Me.CloseButton.Text = "ESC" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "終了"
        Me.CloseButton.UseVisualStyleBackColor = True
        '
        'OkButton
        '
        Me.OkButton.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.OkButton.Location = New System.Drawing.Point(131, 198)
        Me.OkButton.Name = "OkButton"
        Me.OkButton.Size = New System.Drawing.Size(123, 50)
        Me.OkButton.TabIndex = 2
        Me.OkButton.Text = "F5" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "登録"
        Me.OkButton.UseVisualStyleBackColor = True
        '
        'TitleLabel
        '
        Me.TitleLabel.AutoSize = True
        Me.TitleLabel.Font = New System.Drawing.Font("Segoe UI", 15.75!)
        Me.TitleLabel.Location = New System.Drawing.Point(12, 9)
        Me.TitleLabel.Name = "TitleLabel"
        Me.TitleLabel.Size = New System.Drawing.Size(174, 30)
        Me.TitleLabel.TabIndex = 0
        Me.TitleLabel.Text = "フリーマスタ３詳細"
        '
        'Form_Free3Detail
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(395, 260)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.CloseButton)
        Me.Controls.Add(Me.OkButton)
        Me.Controls.Add(Me.TitleLabel)
        Me.Name = "Form_Free3Detail"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Form1"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents CodeText As TextBox
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents NameText As TextBox
    Friend WithEvents NameLabel As Label
    Friend WithEvents CodeLabel As Label
    Friend WithEvents CloseButton As Button
    Friend WithEvents OkButton As Button
    Friend WithEvents TitleLabel As Label
End Class
