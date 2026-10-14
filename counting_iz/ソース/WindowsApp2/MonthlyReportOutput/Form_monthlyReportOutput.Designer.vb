<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form_monthlyReportOutput
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
        Me.TitleLabel = New System.Windows.Forms.Label()
        Me.StartButton = New System.Windows.Forms.Button()
        Me.PeriodLabel = New System.Windows.Forms.Label()
        Me.FiscalMonthLabel = New System.Windows.Forms.Label()
        Me.YearText = New System.Windows.Forms.TextBox()
        Me.YearLabel = New System.Windows.Forms.Label()
        Me.MonthLabel = New System.Windows.Forms.Label()
        Me.MonthText = New System.Windows.Forms.TextBox()
        Me.ClosingDateLabel = New System.Windows.Forms.Label()
        Me.ClosingDateText = New System.Windows.Forms.TextBox()
        Me.DescriptionLabel = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.CloseButton = New System.Windows.Forms.Button()
        Me.DateTime_From = New System.Windows.Forms.TextBox()
        Me.DateTime_To = New System.Windows.Forms.TextBox()
        Me.ConditionGroupBox = New System.Windows.Forms.GroupBox()
        Me.ToItemCodeComboBox = New System.Windows.Forms.ComboBox()
        Me.ItemRangeLabel = New System.Windows.Forms.Label()
        Me.FromItemCodeComboBox = New System.Windows.Forms.ComboBox()
        Me.ItemLabel = New System.Windows.Forms.Label()
        Me.ToScaleNumberComboBox = New System.Windows.Forms.ComboBox()
        Me.ScaleRangeLabel = New System.Windows.Forms.Label()
        Me.FromScaleNumberComboBox = New System.Windows.Forms.ComboBox()
        Me.ScaleNumberLabel = New System.Windows.Forms.Label()
        Me.ConditionGroupBox.SuspendLayout()
        Me.SuspendLayout()
        '
        'TitleLabel
        '
        Me.TitleLabel.AutoSize = True
        Me.TitleLabel.Font = New System.Drawing.Font("Meiryo UI", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TitleLabel.Location = New System.Drawing.Point(12, 9)
        Me.TitleLabel.Name = "TitleLabel"
        Me.TitleLabel.Size = New System.Drawing.Size(101, 30)
        Me.TitleLabel.TabIndex = 0
        Me.TitleLabel.Text = "月報出力"
        '
        'StartButton
        '
        Me.StartButton.Font = New System.Drawing.Font("Meiryo UI", 9.75!)
        Me.StartButton.Location = New System.Drawing.Point(340, 399)
        Me.StartButton.Name = "StartButton"
        Me.StartButton.Size = New System.Drawing.Size(123, 50)
        Me.StartButton.TabIndex = 13
        Me.StartButton.Text = "F5" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "出力"
        Me.StartButton.UseVisualStyleBackColor = True
        '
        'PeriodLabel
        '
        Me.PeriodLabel.AutoSize = True
        Me.PeriodLabel.Font = New System.Drawing.Font("Meiryo UI", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.PeriodLabel.Location = New System.Drawing.Point(12, 174)
        Me.PeriodLabel.Name = "PeriodLabel"
        Me.PeriodLabel.Size = New System.Drawing.Size(100, 30)
        Me.PeriodLabel.TabIndex = 9
        Me.PeriodLabel.AutoSize = False
        Me.PeriodLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.PeriodLabel.Text = "期間："
        '
        'FiscalMonthLabel
        '
        Me.FiscalMonthLabel.AutoSize = True
        Me.FiscalMonthLabel.Font = New System.Drawing.Font("Meiryo UI", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FiscalMonthLabel.Location = New System.Drawing.Point(12, 131)
        Me.FiscalMonthLabel.Name = "FiscalMonthLabel"
        Me.FiscalMonthLabel.Size = New System.Drawing.Size(100, 30)
        Me.FiscalMonthLabel.TabIndex = 4
        Me.FiscalMonthLabel.AutoSize = False
        Me.FiscalMonthLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.FiscalMonthLabel.Text = "月度："
        '
        'YearText
        '
        Me.YearText.Font = New System.Drawing.Font("Meiryo UI", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.YearText.Location = New System.Drawing.Point(118, 131)
        Me.YearText.MaxLength = 4
        Me.YearText.Name = "YearText"
        Me.YearText.Size = New System.Drawing.Size(63, 33)
        Me.YearText.TabIndex = 5
        '
        'YearLabel
        '
        Me.YearLabel.AutoSize = True
        Me.YearLabel.Font = New System.Drawing.Font("Meiryo UI", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.YearLabel.Location = New System.Drawing.Point(183, 131)
        Me.YearLabel.Name = "YearLabel"
        Me.YearLabel.Size = New System.Drawing.Size(35, 30)
        Me.YearLabel.TabIndex = 6
        Me.YearLabel.Text = "年"
        '
        'MonthLabel
        '
        Me.MonthLabel.AutoSize = True
        Me.MonthLabel.Font = New System.Drawing.Font("Meiryo UI", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.MonthLabel.Location = New System.Drawing.Point(253, 131)
        Me.MonthLabel.Name = "MonthLabel"
        Me.MonthLabel.Size = New System.Drawing.Size(35, 30)
        Me.MonthLabel.TabIndex = 8
        Me.MonthLabel.Text = "月"
        '
        'MonthText
        '
        Me.MonthText.Font = New System.Drawing.Font("Meiryo UI", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.MonthText.Location = New System.Drawing.Point(220, 131)
        Me.MonthText.MaxLength = 2
        Me.MonthText.Name = "MonthText"
        Me.MonthText.Size = New System.Drawing.Size(31, 33)
        Me.MonthText.TabIndex = 7
        '
        'ClosingDateLabel
        '
        Me.ClosingDateLabel.AutoSize = True
        Me.ClosingDateLabel.Font = New System.Drawing.Font("Meiryo UI", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ClosingDateLabel.Location = New System.Drawing.Point(438, 29)
        Me.ClosingDateLabel.Name = "ClosingDateLabel"
        Me.ClosingDateLabel.Size = New System.Drawing.Size(103, 30)
        Me.ClosingDateLabel.TabIndex = 1
        Me.ClosingDateLabel.AutoSize = False
        Me.ClosingDateLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ClosingDateLabel.Text = "締め日："
        '
        'ClosingDateText
        '
        Me.ClosingDateText.Font = New System.Drawing.Font("Meiryo UI", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ClosingDateText.Location = New System.Drawing.Point(541, 26)
        Me.ClosingDateText.MaxLength = 2
        Me.ClosingDateText.Name = "ClosingDateText"
        Me.ClosingDateText.Size = New System.Drawing.Size(33, 33)
        Me.ClosingDateText.TabIndex = 2
        Me.ClosingDateText.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'DescriptionLabel
        '
        Me.DescriptionLabel.AutoSize = True
        Me.DescriptionLabel.Font = New System.Drawing.Font("Meiryo UI", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DescriptionLabel.Location = New System.Drawing.Point(375, 62)
        Me.DescriptionLabel.Name = "DescriptionLabel"
        Me.DescriptionLabel.Size = New System.Drawing.Size(199, 30)
        Me.DescriptionLabel.TabIndex = 3
        Me.DescriptionLabel.Text = "※月末の場合、「31」"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Meiryo UI", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(275, 175)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(35, 30)
        Me.Label1.TabIndex = 11
        Me.Label1.Text = "～"
        '
        'CloseButton
        '
        Me.CloseButton.Font = New System.Drawing.Font("Meiryo UI", 9.75!)
        Me.CloseButton.Location = New System.Drawing.Point(469, 399)
        Me.CloseButton.Name = "CloseButton"
        Me.CloseButton.Size = New System.Drawing.Size(123, 50)
        Me.CloseButton.TabIndex = 14
        Me.CloseButton.Text = "ESC" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "終了"
        Me.CloseButton.UseVisualStyleBackColor = True
        '
        'DateTime_From
        '
        Me.DateTime_From.Font = New System.Drawing.Font("Meiryo UI", 14.25!)
        Me.DateTime_From.Location = New System.Drawing.Point(118, 174)
        Me.DateTime_From.MaxLength = 10
        Me.DateTime_From.Name = "DateTime_From"
        Me.DateTime_From.Size = New System.Drawing.Size(151, 33)
        Me.DateTime_From.TabIndex = 10
        '
        'DateTime_To
        '
        Me.DateTime_To.Font = New System.Drawing.Font("Meiryo UI", 14.25!)
        Me.DateTime_To.Location = New System.Drawing.Point(316, 175)
        Me.DateTime_To.MaxLength = 10
        Me.DateTime_To.Name = "DateTime_To"
        Me.DateTime_To.Size = New System.Drawing.Size(151, 33)
        Me.DateTime_To.TabIndex = 12
        '
        'ConditionGroupBox
        '
        Me.ConditionGroupBox.Controls.Add(Me.ToItemCodeComboBox)
        Me.ConditionGroupBox.Controls.Add(Me.ItemRangeLabel)
        Me.ConditionGroupBox.Controls.Add(Me.FromItemCodeComboBox)
        Me.ConditionGroupBox.Controls.Add(Me.ItemLabel)
        Me.ConditionGroupBox.Controls.Add(Me.ToScaleNumberComboBox)
        Me.ConditionGroupBox.Controls.Add(Me.ScaleRangeLabel)
        Me.ConditionGroupBox.Controls.Add(Me.FromScaleNumberComboBox)
        Me.ConditionGroupBox.Controls.Add(Me.ScaleNumberLabel)
        Me.ConditionGroupBox.Location = New System.Drawing.Point(15, 229)
        Me.ConditionGroupBox.Name = "ConditionGroupBox"
        Me.ConditionGroupBox.Size = New System.Drawing.Size(577, 152)
        Me.ConditionGroupBox.TabIndex = 13
        Me.ConditionGroupBox.TabStop = False
        '
        'ToItemCodeComboBox
        '
        Me.ToItemCodeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ToItemCodeComboBox.Font = New System.Drawing.Font("Meiryo UI", 14.25!)
        Me.ToItemCodeComboBox.FormattingEnabled = True
        Me.ToItemCodeComboBox.Location = New System.Drawing.Point(350, 91)
        Me.ToItemCodeComboBox.Name = "ToItemCodeComboBox"
        Me.ToItemCodeComboBox.Size = New System.Drawing.Size(209, 33)
        Me.ToItemCodeComboBox.TabIndex = 7
        '
        'ItemRangeLabel
        '
        Me.ItemRangeLabel.AutoSize = True
        Me.ItemRangeLabel.Font = New System.Drawing.Font("Meiryo UI", 15.75!)
        Me.ItemRangeLabel.Location = New System.Drawing.Point(313, 91)
        Me.ItemRangeLabel.Name = "ItemRangeLabel"
        Me.ItemRangeLabel.Size = New System.Drawing.Size(35, 30)
        Me.ItemRangeLabel.TabIndex = 6
        Me.ItemRangeLabel.Text = "～"
        '
        'FromItemCodeComboBox
        '
        Me.FromItemCodeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.FromItemCodeComboBox.Font = New System.Drawing.Font("Meiryo UI", 14.25!)
        Me.FromItemCodeComboBox.FormattingEnabled = True
        Me.FromItemCodeComboBox.Location = New System.Drawing.Point(101, 91)
        Me.FromItemCodeComboBox.Name = "FromItemCodeComboBox"
        Me.FromItemCodeComboBox.Size = New System.Drawing.Size(209, 33)
        Me.FromItemCodeComboBox.TabIndex = 5
        '
        'ItemLabel
        '
        Me.ItemLabel.AutoSize = True
        Me.ItemLabel.Font = New System.Drawing.Font("Meiryo UI", 15.75!)
        Me.ItemLabel.Location = New System.Drawing.Point(10, 91)
        Me.ItemLabel.Name = "ItemLabel"
        Me.ItemLabel.Size = New System.Drawing.Size(100, 30)
        Me.ItemLabel.TabIndex = 4
        Me.ItemLabel.AutoSize = False
        Me.ItemLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ItemLabel.Text = "商品："
        '
        'ToScaleNumberComboBox
        '
        Me.ToScaleNumberComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ToScaleNumberComboBox.Font = New System.Drawing.Font("Meiryo UI", 14.25!)
        Me.ToScaleNumberComboBox.FormattingEnabled = True
        Me.ToScaleNumberComboBox.Location = New System.Drawing.Point(350, 37)
        Me.ToScaleNumberComboBox.Name = "ToScaleNumberComboBox"
        Me.ToScaleNumberComboBox.Size = New System.Drawing.Size(209, 33)
        Me.ToScaleNumberComboBox.TabIndex = 3
        '
        'ScaleRangeLabel
        '
        Me.ScaleRangeLabel.AutoSize = True
        Me.ScaleRangeLabel.Font = New System.Drawing.Font("Meiryo UI", 15.75!)
        Me.ScaleRangeLabel.Location = New System.Drawing.Point(313, 37)
        Me.ScaleRangeLabel.Name = "ScaleRangeLabel"
        Me.ScaleRangeLabel.Size = New System.Drawing.Size(35, 30)
        Me.ScaleRangeLabel.TabIndex = 2
        Me.ScaleRangeLabel.Text = "～"
        '
        'FromScaleNumberComboBox
        '
        Me.FromScaleNumberComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.FromScaleNumberComboBox.Font = New System.Drawing.Font("Meiryo UI", 14.25!)
        Me.FromScaleNumberComboBox.FormattingEnabled = True
        Me.FromScaleNumberComboBox.Location = New System.Drawing.Point(101, 37)
        Me.FromScaleNumberComboBox.Name = "FromScaleNumberComboBox"
        Me.FromScaleNumberComboBox.Size = New System.Drawing.Size(209, 33)
        Me.FromScaleNumberComboBox.TabIndex = 1
        '
        'ScaleNumberLabel
        '
        Me.ScaleNumberLabel.AutoSize = True
        Me.ScaleNumberLabel.Font = New System.Drawing.Font("Meiryo UI", 15.75!)
        Me.ScaleNumberLabel.Location = New System.Drawing.Point(10, 37)
        Me.ScaleNumberLabel.Name = "ScaleNumberLabel"
        Me.ScaleNumberLabel.Size = New System.Drawing.Size(100, 30)
        Me.ScaleNumberLabel.TabIndex = 0
        Me.ScaleNumberLabel.AutoSize = False
        Me.ScaleNumberLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ScaleNumberLabel.Text = "号機："
        '
        'Form_monthlyReportOutput
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(607, 461)
        Me.Controls.Add(Me.ConditionGroupBox)
        Me.Controls.Add(Me.DateTime_To)
        Me.Controls.Add(Me.DateTime_From)
        Me.Controls.Add(Me.CloseButton)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.DescriptionLabel)
        Me.Controls.Add(Me.ClosingDateText)
        Me.Controls.Add(Me.ClosingDateLabel)
        Me.Controls.Add(Me.MonthText)
        Me.Controls.Add(Me.MonthLabel)
        Me.Controls.Add(Me.YearLabel)
        Me.Controls.Add(Me.YearText)
        Me.Controls.Add(Me.FiscalMonthLabel)
        Me.Controls.Add(Me.PeriodLabel)
        Me.Controls.Add(Me.StartButton)
        Me.Controls.Add(Me.TitleLabel)
        Me.ControlBox = False
        Me.Font = New System.Drawing.Font("Meiryo UI", 9.0!)
        Me.Name = "Form_monthlyReportOutput"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.ConditionGroupBox.ResumeLayout(False)
        Me.ConditionGroupBox.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents TitleLabel As Label
    Friend WithEvents StartButton As Button
    Friend WithEvents PeriodLabel As Label
    Friend WithEvents FiscalMonthLabel As Label
  Friend WithEvents YearText As TextBox
  Friend WithEvents YearLabel As Label
  Friend WithEvents MonthLabel As Label
  Friend WithEvents MonthText As TextBox
    Friend WithEvents ClosingDateLabel As Label
    Friend WithEvents ClosingDateText As TextBox
    Friend WithEvents DescriptionLabel As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents CloseButton As Button
    Friend WithEvents DateTime_From As TextBox
    Friend WithEvents DateTime_To As TextBox
    Friend WithEvents ConditionGroupBox As GroupBox
    Friend WithEvents ToItemCodeComboBox As ComboBox
    Friend WithEvents ItemRangeLabel As Label
    Friend WithEvents FromItemCodeComboBox As ComboBox
    Friend WithEvents ItemLabel As Label
    Friend WithEvents ToScaleNumberComboBox As ComboBox
    Friend WithEvents ScaleRangeLabel As Label
    Friend WithEvents FromScaleNumberComboBox As ComboBox
    Friend WithEvents ScaleNumberLabel As Label
End Class
