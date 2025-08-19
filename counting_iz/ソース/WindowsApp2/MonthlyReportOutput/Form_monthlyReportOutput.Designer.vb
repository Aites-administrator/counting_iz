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
        Me.SuspendLayout()
        '
        'TitleLabel
        '
        Me.TitleLabel.AutoSize = True
        Me.TitleLabel.Font = New System.Drawing.Font("Segoe UI", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TitleLabel.Location = New System.Drawing.Point(12, 9)
        Me.TitleLabel.Name = "TitleLabel"
        Me.TitleLabel.Size = New System.Drawing.Size(101, 30)
        Me.TitleLabel.TabIndex = 0
        Me.TitleLabel.Text = "月報出力"
        '
        'StartButton
        '
        Me.StartButton.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.StartButton.Location = New System.Drawing.Point(220, 251)
        Me.StartButton.Name = "StartButton"
        Me.StartButton.Size = New System.Drawing.Size(123, 50)
        Me.StartButton.TabIndex = 13
        Me.StartButton.Text = "F5" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "出力"
        Me.StartButton.UseVisualStyleBackColor = True
        '
        'PeriodLabel
        '
        Me.PeriodLabel.AutoSize = True
        Me.PeriodLabel.Font = New System.Drawing.Font("Segoe UI", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.PeriodLabel.Location = New System.Drawing.Point(12, 174)
        Me.PeriodLabel.Name = "PeriodLabel"
        Me.PeriodLabel.Size = New System.Drawing.Size(100, 30)
        Me.PeriodLabel.TabIndex = 9
        Me.PeriodLabel.Text = "期　間 ："
        '
        'FiscalMonthLabel
        '
        Me.FiscalMonthLabel.AutoSize = True
        Me.FiscalMonthLabel.Font = New System.Drawing.Font("Segoe UI", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FiscalMonthLabel.Location = New System.Drawing.Point(12, 131)
        Me.FiscalMonthLabel.Name = "FiscalMonthLabel"
        Me.FiscalMonthLabel.Size = New System.Drawing.Size(100, 30)
        Me.FiscalMonthLabel.TabIndex = 4
        Me.FiscalMonthLabel.Text = "月　度 ："
        '
        'YearText
        '
        Me.YearText.Font = New System.Drawing.Font("Segoe UI", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.YearText.Location = New System.Drawing.Point(103, 131)
        Me.YearText.MaxLength = 4
        Me.YearText.Name = "YearText"
        Me.YearText.Size = New System.Drawing.Size(63, 33)
        Me.YearText.TabIndex = 5
        '
        'YearLabel
        '
        Me.YearLabel.AutoSize = True
        Me.YearLabel.Font = New System.Drawing.Font("Segoe UI", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.YearLabel.Location = New System.Drawing.Point(168, 131)
        Me.YearLabel.Name = "YearLabel"
        Me.YearLabel.Size = New System.Drawing.Size(35, 30)
        Me.YearLabel.TabIndex = 6
        Me.YearLabel.Text = "年"
        '
        'MonthLabel
        '
        Me.MonthLabel.AutoSize = True
        Me.MonthLabel.Font = New System.Drawing.Font("Segoe UI", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.MonthLabel.Location = New System.Drawing.Point(238, 131)
        Me.MonthLabel.Name = "MonthLabel"
        Me.MonthLabel.Size = New System.Drawing.Size(35, 30)
        Me.MonthLabel.TabIndex = 8
        Me.MonthLabel.Text = "月"
        '
        'MonthText
        '
        Me.MonthText.Font = New System.Drawing.Font("Segoe UI", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.MonthText.Location = New System.Drawing.Point(205, 131)
        Me.MonthText.MaxLength = 2
        Me.MonthText.Name = "MonthText"
        Me.MonthText.Size = New System.Drawing.Size(31, 33)
        Me.MonthText.TabIndex = 7
        '
        'ClosingDateLabel
        '
        Me.ClosingDateLabel.AutoSize = True
        Me.ClosingDateLabel.Font = New System.Drawing.Font("Segoe UI", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ClosingDateLabel.Location = New System.Drawing.Point(326, 29)
        Me.ClosingDateLabel.Name = "ClosingDateLabel"
        Me.ClosingDateLabel.Size = New System.Drawing.Size(103, 30)
        Me.ClosingDateLabel.TabIndex = 1
        Me.ClosingDateLabel.Text = "締め日 ："
        '
        'ClosingDateText
        '
        Me.ClosingDateText.Font = New System.Drawing.Font("Segoe UI", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ClosingDateText.Location = New System.Drawing.Point(424, 26)
        Me.ClosingDateText.MaxLength = 2
        Me.ClosingDateText.Name = "ClosingDateText"
        Me.ClosingDateText.Size = New System.Drawing.Size(33, 33)
        Me.ClosingDateText.TabIndex = 2
        Me.ClosingDateText.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'DescriptionLabel
        '
        Me.DescriptionLabel.AutoSize = True
        Me.DescriptionLabel.Font = New System.Drawing.Font("Segoe UI", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DescriptionLabel.Location = New System.Drawing.Point(273, 62)
        Me.DescriptionLabel.Name = "DescriptionLabel"
        Me.DescriptionLabel.Size = New System.Drawing.Size(199, 30)
        Me.DescriptionLabel.TabIndex = 3
        Me.DescriptionLabel.Text = "※月末の場合、「31」"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Segoe UI", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(260, 175)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(35, 30)
        Me.Label1.TabIndex = 11
        Me.Label1.Text = "～"
        '
        'CloseButton
        '
        Me.CloseButton.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.CloseButton.Location = New System.Drawing.Point(349, 251)
        Me.CloseButton.Name = "CloseButton"
        Me.CloseButton.Size = New System.Drawing.Size(123, 50)
        Me.CloseButton.TabIndex = 14
        Me.CloseButton.Text = "ESC" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "終了"
        Me.CloseButton.UseVisualStyleBackColor = True
        '
        'DateTime_From
        '
        Me.DateTime_From.Font = New System.Drawing.Font("Segoe UI", 14.25!)
        Me.DateTime_From.Location = New System.Drawing.Point(103, 174)
        Me.DateTime_From.MaxLength = 10
        Me.DateTime_From.Name = "DateTime_From"
        Me.DateTime_From.Size = New System.Drawing.Size(151, 33)
        Me.DateTime_From.TabIndex = 10
        '
        'DateTime_To
        '
        Me.DateTime_To.Font = New System.Drawing.Font("Segoe UI", 14.25!)
        Me.DateTime_To.Location = New System.Drawing.Point(301, 175)
        Me.DateTime_To.MaxLength = 10
        Me.DateTime_To.Name = "DateTime_To"
        Me.DateTime_To.Size = New System.Drawing.Size(151, 33)
        Me.DateTime_To.TabIndex = 12
        '
        'Form_monthlyReportOutput
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(484, 313)
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
        Me.Name = "Form_monthlyReportOutput"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
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
End Class
