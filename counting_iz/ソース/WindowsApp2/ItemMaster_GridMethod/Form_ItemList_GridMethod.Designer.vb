<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form_ItemList_GridMethod
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
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.ItemDetail = New System.Windows.Forms.DataGridView()
        Me.CloseButton = New System.Windows.Forms.Button()
        Me.TitleLabel = New System.Windows.Forms.Label()
        Me.OkButton = New System.Windows.Forms.Button()
        Me.AddRowButton = New System.Windows.Forms.Button()
        Me.DeleteButton = New System.Windows.Forms.Button()
        Me.CopyRowButton = New System.Windows.Forms.Button()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.AddRowTextBox = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.NumberCheckBox = New System.Windows.Forms.CheckBox()
        CType(Me.ItemDetail, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'ItemDetail
        '
        Me.ItemDetail.AllowUserToAddRows = False
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Segoe UI", 12.0!)
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.ItemDetail.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.ItemDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Segoe UI", 12.0!)
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.ItemDetail.DefaultCellStyle = DataGridViewCellStyle2
        Me.ItemDetail.Location = New System.Drawing.Point(12, 47)
        Me.ItemDetail.Name = "ItemDetail"
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Segoe UI", 12.0!)
        DataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.ItemDetail.RowHeadersDefaultCellStyle = DataGridViewCellStyle3
        Me.ItemDetail.RowHeadersWidth = 40
        Me.ItemDetail.RowTemplate.Height = 30
        Me.ItemDetail.Size = New System.Drawing.Size(1711, 568)
        Me.ItemDetail.TabIndex = 1
        '
        'CloseButton
        '
        Me.CloseButton.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.CloseButton.Location = New System.Drawing.Point(1600, 633)
        Me.CloseButton.Name = "CloseButton"
        Me.CloseButton.Size = New System.Drawing.Size(123, 54)
        Me.CloseButton.TabIndex = 7
        Me.CloseButton.Text = "ESC" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "終了"
        Me.CloseButton.UseVisualStyleBackColor = True
        '
        'TitleLabel
        '
        Me.TitleLabel.AutoSize = True
        Me.TitleLabel.Font = New System.Drawing.Font("Segoe UI", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TitleLabel.Location = New System.Drawing.Point(12, 9)
        Me.TitleLabel.Name = "TitleLabel"
        Me.TitleLabel.Size = New System.Drawing.Size(150, 30)
        Me.TitleLabel.TabIndex = 0
        Me.TitleLabel.Text = "商品マスタ一覧"
        '
        'OkButton
        '
        Me.OkButton.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.OkButton.Location = New System.Drawing.Point(1280, 633)
        Me.OkButton.Name = "OkButton"
        Me.OkButton.Size = New System.Drawing.Size(123, 54)
        Me.OkButton.TabIndex = 5
        Me.OkButton.Text = "F5" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "登録"
        Me.OkButton.UseVisualStyleBackColor = True
        '
        'AddRowButton
        '
        Me.AddRowButton.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.AddRowButton.Location = New System.Drawing.Point(117, 634)
        Me.AddRowButton.Name = "AddRowButton"
        Me.AddRowButton.Size = New System.Drawing.Size(123, 54)
        Me.AddRowButton.TabIndex = 3
        Me.AddRowButton.Text = "F1" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "行追加(新規)"
        Me.AddRowButton.UseVisualStyleBackColor = True
        '
        'DeleteButton
        '
        Me.DeleteButton.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.DeleteButton.Location = New System.Drawing.Point(1409, 633)
        Me.DeleteButton.Name = "DeleteButton"
        Me.DeleteButton.Size = New System.Drawing.Size(123, 54)
        Me.DeleteButton.TabIndex = 6
        Me.DeleteButton.Text = "F6" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "削除"
        Me.DeleteButton.UseVisualStyleBackColor = True
        '
        'CopyRowButton
        '
        Me.CopyRowButton.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.CopyRowButton.Location = New System.Drawing.Point(246, 634)
        Me.CopyRowButton.Name = "CopyRowButton"
        Me.CopyRowButton.Size = New System.Drawing.Size(123, 54)
        Me.CopyRowButton.TabIndex = 4
        Me.CopyRowButton.Text = "F2" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "行追加(コピー)"
        Me.CopyRowButton.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.AddRowTextBox)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.NumberCheckBox)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 621)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(99, 67)
        Me.GroupBox1.TabIndex = 2
        Me.GroupBox1.TabStop = False
        '
        'AddRowTextBox
        '
        Me.AddRowTextBox.Font = New System.Drawing.Font("メイリオ", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.AddRowTextBox.Location = New System.Drawing.Point(54, 34)
        Me.AddRowTextBox.MaxLength = 2
        Me.AddRowTextBox.Name = "AddRowTextBox"
        Me.AddRowTextBox.Size = New System.Drawing.Size(38, 27)
        Me.AddRowTextBox.TabIndex = 0
        Me.AddRowTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(6, 37)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(44, 21)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "行数"
        '
        'NumberCheckBox
        '
        Me.NumberCheckBox.AutoSize = True
        Me.NumberCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.NumberCheckBox.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.NumberCheckBox.Location = New System.Drawing.Point(6, 6)
        Me.NumberCheckBox.Name = "NumberCheckBox"
        Me.NumberCheckBox.Size = New System.Drawing.Size(63, 25)
        Me.NumberCheckBox.TabIndex = 1
        Me.NumberCheckBox.Text = "採番"
        Me.NumberCheckBox.UseVisualStyleBackColor = True
        '
        'Form_ItemList_GridMethod
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1735, 696)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.CopyRowButton)
        Me.Controls.Add(Me.DeleteButton)
        Me.Controls.Add(Me.AddRowButton)
        Me.Controls.Add(Me.OkButton)
        Me.Controls.Add(Me.ItemDetail)
        Me.Controls.Add(Me.CloseButton)
        Me.Controls.Add(Me.TitleLabel)
        Me.Name = "Form_ItemList_GridMethod"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "商品マスタ一覧"
        CType(Me.ItemDetail, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents ItemDetail As DataGridView
    Friend WithEvents CloseButton As Button
    Friend WithEvents TitleLabel As Label
    Friend WithEvents OkButton As Button
    Friend WithEvents AddRowButton As Button
    Friend WithEvents DeleteButton As Button
    Friend WithEvents CopyRowButton As Button
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents AddRowTextBox As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents NumberCheckBox As CheckBox
End Class
