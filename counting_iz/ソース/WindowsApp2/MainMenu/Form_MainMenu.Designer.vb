<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form_MainMenu
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form_MainMenu))
        Me.TitleLabel = New System.Windows.Forms.Label()
        Me.DetailOutputButton = New System.Windows.Forms.Button()
        Me.MonthlyReportOutputButton = New System.Windows.Forms.Button()
        Me.PackingLabelPrintButton = New System.Windows.Forms.Button()
        Me.LogDisplayButton = New System.Windows.Forms.Button()
        Me.CloseButton = New System.Windows.Forms.Button()
        Me.LogoPictureBox = New System.Windows.Forms.PictureBox()
        Me.MasterGroupBox = New System.Windows.Forms.GroupBox()
        Me.ScaleMasterButton = New System.Windows.Forms.Button()
        Me.PackingMasterButton = New System.Windows.Forms.Button()
        Me.CorporateMasterButton = New System.Windows.Forms.Button()
        Me.AreaMasterButton = New System.Windows.Forms.Button()
        Me.TenantMasterButton = New System.Windows.Forms.Button()
        Me.GarbageCategoryMasterButton = New System.Windows.Forms.Button()
        Me.GarbageTypeMasterButton = New System.Windows.Forms.Button()
        Me.Button1 = New System.Windows.Forms.Button()
        CType(Me.LogoPictureBox, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.MasterGroupBox.SuspendLayout()
        Me.SuspendLayout()
        '
        'TitleLabel
        '
        Me.TitleLabel.AutoSize = True
        Me.TitleLabel.Font = New System.Drawing.Font("MS UI Gothic", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.TitleLabel.Location = New System.Drawing.Point(218, 8)
        Me.TitleLabel.Name = "TitleLabel"
        Me.TitleLabel.Size = New System.Drawing.Size(275, 19)
        Me.TitleLabel.TabIndex = 0
        Me.TitleLabel.Text = "<　うめきた広場ゴミ計量システム　>"
        Me.TitleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'DetailOutputButton
        '
        Me.DetailOutputButton.Location = New System.Drawing.Point(40, 62)
        Me.DetailOutputButton.Name = "DetailOutputButton"
        Me.DetailOutputButton.Size = New System.Drawing.Size(250, 36)
        Me.DetailOutputButton.TabIndex = 1
        Me.DetailOutputButton.Text = "明　細　出　力"
        Me.DetailOutputButton.UseVisualStyleBackColor = True
        '
        'MonthlyReportOutputButton
        '
        Me.MonthlyReportOutputButton.Location = New System.Drawing.Point(40, 123)
        Me.MonthlyReportOutputButton.Name = "MonthlyReportOutputButton"
        Me.MonthlyReportOutputButton.Size = New System.Drawing.Size(250, 36)
        Me.MonthlyReportOutputButton.TabIndex = 2
        Me.MonthlyReportOutputButton.Text = "月　報　出　力"
        Me.MonthlyReportOutputButton.UseVisualStyleBackColor = True
        '
        'PackingLabelPrintButton
        '
        Me.PackingLabelPrintButton.Location = New System.Drawing.Point(40, 243)
        Me.PackingLabelPrintButton.Name = "PackingLabelPrintButton"
        Me.PackingLabelPrintButton.Size = New System.Drawing.Size(250, 36)
        Me.PackingLabelPrintButton.TabIndex = 3
        Me.PackingLabelPrintButton.Text = "風　袋　用　ラ　ベ　ル　印　刷"
        Me.PackingLabelPrintButton.UseVisualStyleBackColor = True
        '
        'LogDisplayButton
        '
        Me.LogDisplayButton.Location = New System.Drawing.Point(40, 423)
        Me.LogDisplayButton.Name = "LogDisplayButton"
        Me.LogDisplayButton.Size = New System.Drawing.Size(250, 36)
        Me.LogDisplayButton.TabIndex = 5
        Me.LogDisplayButton.Text = "送　信　用　ロ　グ　表　示"
        Me.LogDisplayButton.UseVisualStyleBackColor = True
        '
        'CloseButton
        '
        Me.CloseButton.Location = New System.Drawing.Point(394, 506)
        Me.CloseButton.Name = "CloseButton"
        Me.CloseButton.Size = New System.Drawing.Size(250, 36)
        Me.CloseButton.TabIndex = 7
        Me.CloseButton.Text = "終　了"
        Me.CloseButton.UseVisualStyleBackColor = True
        '
        'LogoPictureBox
        '
        Me.LogoPictureBox.Image = CType(resources.GetObject("LogoPictureBox.Image"), System.Drawing.Image)
        Me.LogoPictureBox.Location = New System.Drawing.Point(40, 522)
        Me.LogoPictureBox.Name = "LogoPictureBox"
        Me.LogoPictureBox.Size = New System.Drawing.Size(123, 32)
        Me.LogoPictureBox.TabIndex = 11
        Me.LogoPictureBox.TabStop = False
        '
        'MasterGroupBox
        '
        Me.MasterGroupBox.Controls.Add(Me.ScaleMasterButton)
        Me.MasterGroupBox.Controls.Add(Me.PackingMasterButton)
        Me.MasterGroupBox.Controls.Add(Me.CorporateMasterButton)
        Me.MasterGroupBox.Controls.Add(Me.AreaMasterButton)
        Me.MasterGroupBox.Controls.Add(Me.TenantMasterButton)
        Me.MasterGroupBox.Controls.Add(Me.GarbageCategoryMasterButton)
        Me.MasterGroupBox.Controls.Add(Me.GarbageTypeMasterButton)
        Me.MasterGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.MasterGroupBox.Location = New System.Drawing.Point(358, 40)
        Me.MasterGroupBox.Name = "MasterGroupBox"
        Me.MasterGroupBox.Size = New System.Drawing.Size(320, 448)
        Me.MasterGroupBox.TabIndex = 6
        Me.MasterGroupBox.TabStop = False
        Me.MasterGroupBox.Text = "マスタメンテ画面"
        '
        'ScaleMasterButton
        '
        Me.ScaleMasterButton.Location = New System.Drawing.Point(36, 383)
        Me.ScaleMasterButton.Name = "ScaleMasterButton"
        Me.ScaleMasterButton.Size = New System.Drawing.Size(250, 36)
        Me.ScaleMasterButton.TabIndex = 6
        Me.ScaleMasterButton.Text = "計　量　器　マ　ス　タ"
        Me.ScaleMasterButton.UseVisualStyleBackColor = True
        '
        'PackingMasterButton
        '
        Me.PackingMasterButton.Location = New System.Drawing.Point(36, 325)
        Me.PackingMasterButton.Name = "PackingMasterButton"
        Me.PackingMasterButton.Size = New System.Drawing.Size(250, 36)
        Me.PackingMasterButton.TabIndex = 5
        Me.PackingMasterButton.Text = "風　袋　マ　ス　タ"
        Me.PackingMasterButton.UseVisualStyleBackColor = True
        '
        'CorporateMasterButton
        '
        Me.CorporateMasterButton.Location = New System.Drawing.Point(36, 265)
        Me.CorporateMasterButton.Name = "CorporateMasterButton"
        Me.CorporateMasterButton.Size = New System.Drawing.Size(250, 36)
        Me.CorporateMasterButton.TabIndex = 4
        Me.CorporateMasterButton.Text = "法　人　マ　ス　タ"
        Me.CorporateMasterButton.UseVisualStyleBackColor = True
        '
        'AreaMasterButton
        '
        Me.AreaMasterButton.Location = New System.Drawing.Point(36, 203)
        Me.AreaMasterButton.Name = "AreaMasterButton"
        Me.AreaMasterButton.Size = New System.Drawing.Size(250, 36)
        Me.AreaMasterButton.TabIndex = 3
        Me.AreaMasterButton.Text = "エ　リ　ア　マ　ス　タ"
        Me.AreaMasterButton.UseVisualStyleBackColor = True
        '
        'TenantMasterButton
        '
        Me.TenantMasterButton.Location = New System.Drawing.Point(36, 143)
        Me.TenantMasterButton.Name = "TenantMasterButton"
        Me.TenantMasterButton.Size = New System.Drawing.Size(250, 36)
        Me.TenantMasterButton.TabIndex = 2
        Me.TenantMasterButton.Text = "テ　ナ　ン　ト　マ　ス　タ"
        Me.TenantMasterButton.UseVisualStyleBackColor = True
        '
        'GarbageCategoryMasterButton
        '
        Me.GarbageCategoryMasterButton.Location = New System.Drawing.Point(36, 83)
        Me.GarbageCategoryMasterButton.Name = "GarbageCategoryMasterButton"
        Me.GarbageCategoryMasterButton.Size = New System.Drawing.Size(250, 36)
        Me.GarbageCategoryMasterButton.TabIndex = 1
        Me.GarbageCategoryMasterButton.Text = "ゴ　ミ　分　類　マ　ス　タ"
        Me.GarbageCategoryMasterButton.UseVisualStyleBackColor = True
        '
        'GarbageTypeMasterButton
        '
        Me.GarbageTypeMasterButton.Location = New System.Drawing.Point(36, 22)
        Me.GarbageTypeMasterButton.Name = "GarbageTypeMasterButton"
        Me.GarbageTypeMasterButton.Size = New System.Drawing.Size(250, 36)
        Me.GarbageTypeMasterButton.TabIndex = 0
        Me.GarbageTypeMasterButton.Text = "ゴ　ミ　種　マ　ス　タ"
        Me.GarbageTypeMasterButton.UseVisualStyleBackColor = True
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(40, 365)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(250, 36)
        Me.Button1.TabIndex = 4
        Me.Button1.Text = "送　受　信　確　認"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Form_MainMenu
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(704, 584)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.MasterGroupBox)
        Me.Controls.Add(Me.LogoPictureBox)
        Me.Controls.Add(Me.CloseButton)
        Me.Controls.Add(Me.LogDisplayButton)
        Me.Controls.Add(Me.PackingLabelPrintButton)
        Me.Controls.Add(Me.MonthlyReportOutputButton)
        Me.Controls.Add(Me.DetailOutputButton)
        Me.Controls.Add(Me.TitleLabel)
        Me.Name = "Form_MainMenu"
        StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        CType(Me.LogoPictureBox, System.ComponentModel.ISupportInitialize).EndInit()
        Me.MasterGroupBox.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents TitleLabel As Label
    Friend WithEvents DetailOutputButton As Button
    Friend WithEvents MonthlyReportOutputButton As Button
    Friend WithEvents PackingLabelPrintButton As Button
    Friend WithEvents LogDisplayButton As Button
    Friend WithEvents CloseButton As Button
    Friend WithEvents LogoPictureBox As PictureBox
    Friend WithEvents MasterGroupBox As GroupBox
    Friend WithEvents CorporateMasterButton As Button
    Friend WithEvents AreaMasterButton As Button
    Friend WithEvents TenantMasterButton As Button
    Friend WithEvents GarbageCategoryMasterButton As Button
    Friend WithEvents GarbageTypeMasterButton As Button
    Friend WithEvents ScaleMasterButton As Button
    Friend WithEvents PackingMasterButton As Button
    Friend WithEvents Button1 As Button
End Class
