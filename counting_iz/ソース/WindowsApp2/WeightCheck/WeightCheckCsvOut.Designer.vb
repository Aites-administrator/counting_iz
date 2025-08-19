<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class WeightCheckCsvOut
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
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.DateTimeFrom = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.StartButton = New System.Windows.Forms.Button()
        Me.CloseButton = New System.Windows.Forms.Button()
        Me.ItemCode_ComboBox = New System.Windows.Forms.ComboBox()
        Me.DateTimeTo = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Segoe UI", 15.75!)
        Me.Label1.Location = New System.Drawing.Point(12, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(226, 30)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "重量検査一覧表　出力"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Segoe UI", 15.75!)
        Me.Label2.Location = New System.Drawing.Point(46, 70)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(57, 30)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "品名"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Segoe UI", 15.75!)
        Me.Label3.Location = New System.Drawing.Point(46, 110)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(57, 30)
        Me.Label3.TabIndex = 3
        Me.Label3.Text = "期間"
        '
        'DateTimeFrom
        '
        Me.DateTimeFrom.Font = New System.Drawing.Font("Segoe UI", 14.25!)
        Me.DateTimeFrom.Location = New System.Drawing.Point(126, 111)
        Me.DateTimeFrom.Name = "DateTimeFrom"
        Me.DateTimeFrom.Size = New System.Drawing.Size(151, 33)
        Me.DateTimeFrom.TabIndex = 4
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Segoe UI", 15.75!)
        Me.Label4.Location = New System.Drawing.Point(292, 114)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(35, 30)
        Me.Label4.TabIndex = 5
        Me.Label4.Text = "～"
        '
        'StartButton
        '
        Me.StartButton.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.StartButton.Location = New System.Drawing.Point(246, 181)
        Me.StartButton.Name = "StartButton"
        Me.StartButton.Size = New System.Drawing.Size(123, 50)
        Me.StartButton.TabIndex = 7
        Me.StartButton.Text = "F5" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "出力"
        Me.StartButton.UseVisualStyleBackColor = True
        '
        'CloseButton
        '
        Me.CloseButton.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.CloseButton.Location = New System.Drawing.Point(375, 181)
        Me.CloseButton.Name = "CloseButton"
        Me.CloseButton.Size = New System.Drawing.Size(123, 50)
        Me.CloseButton.TabIndex = 8
        Me.CloseButton.Text = "ESC" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "終了"
        Me.CloseButton.UseVisualStyleBackColor = True
        '
        'ItemCode_ComboBox
        '
        Me.ItemCode_ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ItemCode_ComboBox.Font = New System.Drawing.Font("Segoe UI", 14.25!)
        Me.ItemCode_ComboBox.FormattingEnabled = True
        Me.ItemCode_ComboBox.Location = New System.Drawing.Point(126, 67)
        Me.ItemCode_ComboBox.Name = "ItemCode_ComboBox"
        Me.ItemCode_ComboBox.Size = New System.Drawing.Size(366, 33)
        Me.ItemCode_ComboBox.TabIndex = 2
        '
        'DateTimeTo
        '
        Me.DateTimeTo.Font = New System.Drawing.Font("Segoe UI", 14.25!)
        Me.DateTimeTo.Location = New System.Drawing.Point(341, 115)
        Me.DateTimeTo.Name = "DateTimeTo"
        Me.DateTimeTo.Size = New System.Drawing.Size(151, 33)
        Me.DateTimeTo.TabIndex = 6
        '
        'WeightCheckCsvOut
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(510, 243)
        Me.Controls.Add(Me.DateTimeTo)
        Me.Controls.Add(Me.ItemCode_ComboBox)
        Me.Controls.Add(Me.CloseButton)
        Me.Controls.Add(Me.StartButton)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.DateTimeFrom)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Name = "WeightCheckCsvOut"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Form1"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents DateTimeFrom As TextBox
    Friend WithEvents Label4 As Label
    Friend WithEvents StartButton As Button
    Friend WithEvents CloseButton As Button
    Friend WithEvents ItemCode_ComboBox As ComboBox
    Friend WithEvents DateTimeTo As TextBox
End Class
