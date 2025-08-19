<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form_ResultList
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
        Me.CreateButton = New System.Windows.Forms.Button()
        Me.DateTimeTo = New System.Windows.Forms.TextBox()
        Me.DateTimeFrom = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.ToItemCode_ComboBox = New System.Windows.Forms.ComboBox()
        Me.FromItemCode_ComboBox = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Scale_ComboBox = New System.Windows.Forms.ComboBox()
        Me.ScaleLabel = New System.Windows.Forms.Label()
        Me.PeriodLabel = New System.Windows.Forms.Label()
        Me.SearchButton = New System.Windows.Forms.Button()
        Me.TitleLabel = New System.Windows.Forms.Label()
        Me.CloseButton = New System.Windows.Forms.Button()
        Me.DeleteButton = New System.Windows.Forms.Button()
        Me.UpdateButton = New System.Windows.Forms.Button()
        Me.ResultDetail = New System.Windows.Forms.DataGridView()
        Me.CsvExportButton = New System.Windows.Forms.Button()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.lot1TextBox = New System.Windows.Forms.TextBox()
        Me.lot2TextBox = New System.Windows.Forms.TextBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.ToStaffCode_ComboBox = New System.Windows.Forms.ComboBox()
        Me.FromStaffCode_ComboBox = New System.Windows.Forms.ComboBox()
        Me.Label9 = New System.Windows.Forms.Label()
        CType(Me.ResultDetail, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'CreateButton
        '
        Me.CreateButton.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.CreateButton.Location = New System.Drawing.Point(1095, 389)
        Me.CreateButton.Name = "CreateButton"
        Me.CreateButton.Size = New System.Drawing.Size(123, 50)
        Me.CreateButton.TabIndex = 22
        Me.CreateButton.Text = "F5" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "新規"
        Me.CreateButton.UseVisualStyleBackColor = True
        Me.CreateButton.Visible = False
        '
        'DateTimeTo
        '
        Me.DateTimeTo.Font = New System.Drawing.Font("Segoe UI", 14.25!)
        Me.DateTimeTo.Location = New System.Drawing.Point(339, 47)
        Me.DateTimeTo.MaxLength = 10
        Me.DateTimeTo.Name = "DateTimeTo"
        Me.DateTimeTo.Size = New System.Drawing.Size(160, 33)
        Me.DateTimeTo.TabIndex = 4
        '
        'DateTimeFrom
        '
        Me.DateTimeFrom.Font = New System.Drawing.Font("Segoe UI", 14.25!)
        Me.DateTimeFrom.Location = New System.Drawing.Point(138, 47)
        Me.DateTimeFrom.MaxLength = 10
        Me.DateTimeFrom.Name = "DateTimeFrom"
        Me.DateTimeFrom.Size = New System.Drawing.Size(160, 33)
        Me.DateTimeFrom.TabIndex = 2
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("MS UI Gothic", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Label2.Location = New System.Drawing.Point(302, 55)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(31, 21)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "～"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("MS UI Gothic", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Label3.Location = New System.Drawing.Point(302, 96)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(31, 21)
        Me.Label3.TabIndex = 13
        Me.Label3.Text = "～"
        '
        'ToItemCode_ComboBox
        '
        Me.ToItemCode_ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ToItemCode_ComboBox.Font = New System.Drawing.Font("Segoe UI", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.ToItemCode_ComboBox.FormattingEnabled = True
        Me.ToItemCode_ComboBox.Location = New System.Drawing.Point(339, 87)
        Me.ToItemCode_ComboBox.Name = "ToItemCode_ComboBox"
        Me.ToItemCode_ComboBox.Size = New System.Drawing.Size(160, 33)
        Me.ToItemCode_ComboBox.TabIndex = 14
        '
        'FromItemCode_ComboBox
        '
        Me.FromItemCode_ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.FromItemCode_ComboBox.Font = New System.Drawing.Font("Segoe UI", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.FromItemCode_ComboBox.FormattingEnabled = True
        Me.FromItemCode_ComboBox.Location = New System.Drawing.Point(138, 86)
        Me.FromItemCode_ComboBox.Name = "FromItemCode_ComboBox"
        Me.FromItemCode_ComboBox.Size = New System.Drawing.Size(160, 33)
        Me.FromItemCode_ComboBox.TabIndex = 12
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Segoe UI", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(16, 89)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(123, 30)
        Me.Label1.TabIndex = 11
        Me.Label1.Text = "呼出ｺｰﾄﾞ："
        '
        'Scale_ComboBox
        '
        Me.Scale_ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.Scale_ComboBox.Font = New System.Drawing.Font("Segoe UI", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Scale_ComboBox.FormattingEnabled = True
        Me.Scale_ComboBox.Location = New System.Drawing.Point(605, 47)
        Me.Scale_ComboBox.Name = "Scale_ComboBox"
        Me.Scale_ComboBox.Size = New System.Drawing.Size(160, 33)
        Me.Scale_ComboBox.TabIndex = 6
        '
        'ScaleLabel
        '
        Me.ScaleLabel.AutoSize = True
        Me.ScaleLabel.Font = New System.Drawing.Font("Segoe UI", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.ScaleLabel.Location = New System.Drawing.Point(520, 50)
        Me.ScaleLabel.Name = "ScaleLabel"
        Me.ScaleLabel.Size = New System.Drawing.Size(94, 30)
        Me.ScaleLabel.TabIndex = 5
        Me.ScaleLabel.Text = "号　機："
        '
        'PeriodLabel
        '
        Me.PeriodLabel.AutoSize = True
        Me.PeriodLabel.Font = New System.Drawing.Font("Segoe UI", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.PeriodLabel.Location = New System.Drawing.Point(16, 50)
        Me.PeriodLabel.Name = "PeriodLabel"
        Me.PeriodLabel.Size = New System.Drawing.Size(94, 30)
        Me.PeriodLabel.TabIndex = 1
        Me.PeriodLabel.Text = "日　付："
        '
        'SearchButton
        '
        Me.SearchButton.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.SearchButton.Location = New System.Drawing.Point(1095, 165)
        Me.SearchButton.Name = "SearchButton"
        Me.SearchButton.Size = New System.Drawing.Size(123, 50)
        Me.SearchButton.TabIndex = 19
        Me.SearchButton.Text = "F1" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "検索"
        Me.SearchButton.UseVisualStyleBackColor = True
        '
        'TitleLabel
        '
        Me.TitleLabel.AutoSize = True
        Me.TitleLabel.Font = New System.Drawing.Font("Segoe UI", 16.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.TitleLabel.Location = New System.Drawing.Point(12, 9)
        Me.TitleLabel.Name = "TitleLabel"
        Me.TitleLabel.Size = New System.Drawing.Size(105, 30)
        Me.TitleLabel.TabIndex = 0
        Me.TitleLabel.Text = "実績一覧"
        '
        'CloseButton
        '
        Me.CloseButton.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.CloseButton.Location = New System.Drawing.Point(1095, 639)
        Me.CloseButton.Name = "CloseButton"
        Me.CloseButton.Size = New System.Drawing.Size(123, 50)
        Me.CloseButton.TabIndex = 25
        Me.CloseButton.Text = "ESC" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "終了"
        Me.CloseButton.UseVisualStyleBackColor = True
        '
        'DeleteButton
        '
        Me.DeleteButton.Enabled = False
        Me.DeleteButton.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.DeleteButton.Location = New System.Drawing.Point(1095, 501)
        Me.DeleteButton.Name = "DeleteButton"
        Me.DeleteButton.Size = New System.Drawing.Size(123, 50)
        Me.DeleteButton.TabIndex = 24
        Me.DeleteButton.Text = "F7" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "削除"
        Me.DeleteButton.UseVisualStyleBackColor = True
        '
        'UpdateButton
        '
        Me.UpdateButton.Enabled = False
        Me.UpdateButton.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.UpdateButton.Location = New System.Drawing.Point(1095, 445)
        Me.UpdateButton.Name = "UpdateButton"
        Me.UpdateButton.Size = New System.Drawing.Size(123, 50)
        Me.UpdateButton.TabIndex = 23
        Me.UpdateButton.Text = "F6" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "修正"
        Me.UpdateButton.UseVisualStyleBackColor = True
        '
        'ResultDetail
        '
        Me.ResultDetail.AllowUserToAddRows = False
        Me.ResultDetail.AllowUserToDeleteRows = False
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.ResultDetail.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.ResultDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.ResultDetail.DefaultCellStyle = DataGridViewCellStyle2
        Me.ResultDetail.Location = New System.Drawing.Point(18, 165)
        Me.ResultDetail.Name = "ResultDetail"
        Me.ResultDetail.ReadOnly = True
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Segoe UI", 12.0!)
        DataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.ResultDetail.RowHeadersDefaultCellStyle = DataGridViewCellStyle3
        Me.ResultDetail.RowTemplate.Height = 21
        Me.ResultDetail.Size = New System.Drawing.Size(1071, 524)
        Me.ResultDetail.TabIndex = 21
        Me.ResultDetail.TabStop = False
        '
        'CsvExportButton
        '
        Me.CsvExportButton.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.CsvExportButton.Location = New System.Drawing.Point(1095, 221)
        Me.CsvExportButton.Name = "CsvExportButton"
        Me.CsvExportButton.Size = New System.Drawing.Size(123, 50)
        Me.CsvExportButton.TabIndex = 20
        Me.CsvExportButton.Text = "F2" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "CSV出力"
        Me.CsvExportButton.UseVisualStyleBackColor = True
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Segoe UI", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(520, 89)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(92, 25)
        Me.Label6.TabIndex = 7
        Me.Label6.Text = "ロット１："
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Segoe UI", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(520, 128)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(92, 25)
        Me.Label7.TabIndex = 9
        Me.Label7.Text = "ロット２："
        '
        'lot1TextBox
        '
        Me.lot1TextBox.Font = New System.Drawing.Font("Segoe UI", 14.25!)
        Me.lot1TextBox.Location = New System.Drawing.Point(605, 86)
        Me.lot1TextBox.MaxLength = 20
        Me.lot1TextBox.Name = "lot1TextBox"
        Me.lot1TextBox.Size = New System.Drawing.Size(160, 33)
        Me.lot1TextBox.TabIndex = 8
        '
        'lot2TextBox
        '
        Me.lot2TextBox.Font = New System.Drawing.Font("Segoe UI", 14.25!)
        Me.lot2TextBox.Location = New System.Drawing.Point(605, 125)
        Me.lot2TextBox.MaxLength = 20
        Me.lot2TextBox.Name = "lot2TextBox"
        Me.lot2TextBox.Size = New System.Drawing.Size(160, 33)
        Me.lot2TextBox.TabIndex = 10
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("MS UI Gothic", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Label8.Location = New System.Drawing.Point(302, 135)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(31, 21)
        Me.Label8.TabIndex = 28
        Me.Label8.Text = "～"
        '
        'ToStaffCode_ComboBox
        '
        Me.ToStaffCode_ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ToStaffCode_ComboBox.Font = New System.Drawing.Font("Segoe UI", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.ToStaffCode_ComboBox.FormattingEnabled = True
        Me.ToStaffCode_ComboBox.Location = New System.Drawing.Point(339, 126)
        Me.ToStaffCode_ComboBox.Name = "ToStaffCode_ComboBox"
        Me.ToStaffCode_ComboBox.Size = New System.Drawing.Size(160, 33)
        Me.ToStaffCode_ComboBox.TabIndex = 29
        '
        'FromStaffCode_ComboBox
        '
        Me.FromStaffCode_ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.FromStaffCode_ComboBox.Font = New System.Drawing.Font("Segoe UI", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.FromStaffCode_ComboBox.FormattingEnabled = True
        Me.FromStaffCode_ComboBox.Location = New System.Drawing.Point(138, 125)
        Me.FromStaffCode_ComboBox.Name = "FromStaffCode_ComboBox"
        Me.FromStaffCode_ComboBox.Size = New System.Drawing.Size(160, 33)
        Me.FromStaffCode_ComboBox.TabIndex = 27
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Segoe UI", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Label9.Location = New System.Drawing.Point(16, 128)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(101, 30)
        Me.Label9.TabIndex = 26
        Me.Label9.Text = "担当者："
        '
        'Form_ResultList
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1224, 701)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.ToStaffCode_ComboBox)
        Me.Controls.Add(Me.FromStaffCode_ComboBox)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.lot2TextBox)
        Me.Controls.Add(Me.lot1TextBox)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.CsvExportButton)
        Me.Controls.Add(Me.ResultDetail)
        Me.Controls.Add(Me.CloseButton)
        Me.Controls.Add(Me.DeleteButton)
        Me.Controls.Add(Me.UpdateButton)
        Me.Controls.Add(Me.DateTimeTo)
        Me.Controls.Add(Me.DateTimeFrom)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.ToItemCode_ComboBox)
        Me.Controls.Add(Me.FromItemCode_ComboBox)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Scale_ComboBox)
        Me.Controls.Add(Me.ScaleLabel)
        Me.Controls.Add(Me.PeriodLabel)
        Me.Controls.Add(Me.SearchButton)
        Me.Controls.Add(Me.TitleLabel)
        Me.Controls.Add(Me.CreateButton)
        Me.Name = "Form_ResultList"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Form1"
        CType(Me.ResultDetail, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents CreateButton As Button
    Friend WithEvents DateTimeTo As TextBox
    Friend WithEvents DateTimeFrom As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents ToItemCode_ComboBox As ComboBox
    Friend WithEvents FromItemCode_ComboBox As ComboBox
    Friend WithEvents Label1 As Label
    Friend WithEvents Scale_ComboBox As ComboBox
    Friend WithEvents ScaleLabel As Label
    Friend WithEvents PeriodLabel As Label
    Friend WithEvents SearchButton As Button
    Friend WithEvents TitleLabel As Label
    Friend WithEvents CloseButton As Button
    Friend WithEvents DeleteButton As Button
    Friend WithEvents UpdateButton As Button
    Friend WithEvents ResultDetail As DataGridView
    Friend WithEvents CsvExportButton As Button
    Friend WithEvents Label6 As Label
    Friend WithEvents Label7 As Label
    Friend WithEvents lot1TextBox As TextBox
    Friend WithEvents lot2TextBox As TextBox
    Friend WithEvents Label8 As Label
    Friend WithEvents ToStaffCode_ComboBox As ComboBox
    Friend WithEvents FromStaffCode_ComboBox As ComboBox
    Friend WithEvents Label9 As Label
End Class
