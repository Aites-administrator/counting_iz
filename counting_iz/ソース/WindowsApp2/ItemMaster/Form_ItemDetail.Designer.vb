<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form_ItemDetail
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
        Me.CloseButton = New System.Windows.Forms.Button()
        Me.OkButton = New System.Windows.Forms.Button()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.itemCodeText = New System.Windows.Forms.TextBox()
        Me.Label21 = New System.Windows.Forms.Label()
        Me.itemNameText = New System.Windows.Forms.TextBox()
        Me.Label22 = New System.Windows.Forms.Label()
        Me.codeText = New System.Windows.Forms.TextBox()
        Me.Label23 = New System.Windows.Forms.Label()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.maxText = New System.Windows.Forms.TextBox()
        Me.minText = New System.Windows.Forms.TextBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.standardText = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.targetQtyText = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.safetyFactorText = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.weightMinText = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.weightMinUnitCombo = New System.Windows.Forms.ComboBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.weightMaxText = New System.Windows.Forms.TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.weightMaxUnitCombo = New System.Windows.Forms.ComboBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.weightText = New System.Windows.Forms.TextBox()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.weightUnitCombo = New System.Windows.Forms.ComboBox()
        Me.Label24 = New System.Windows.Forms.Label()
        Me.GroupBox4 = New System.Windows.Forms.GroupBox()
        Me.tareWeightUnitTextBox = New System.Windows.Forms.TextBox()
        Me.tareWeightTextBox = New System.Windows.Forms.TextBox()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.subtotalCountText = New System.Windows.Forms.TextBox()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.subtotalTargetText = New System.Windows.Forms.TextBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.tareCombo = New System.Windows.Forms.ComboBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.GroupBox4.SuspendLayout()
        Me.SuspendLayout()
        '
        'TitleLabel
        '
        Me.TitleLabel.AutoSize = True
        Me.TitleLabel.Font = New System.Drawing.Font("Segoe UI", 15.75!)
        Me.TitleLabel.Location = New System.Drawing.Point(12, 9)
        Me.TitleLabel.Name = "TitleLabel"
        Me.TitleLabel.Size = New System.Drawing.Size(150, 30)
        Me.TitleLabel.TabIndex = 0
        Me.TitleLabel.Text = "商品マスタ詳細"
        '
        'CloseButton
        '
        Me.CloseButton.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.CloseButton.Location = New System.Drawing.Point(620, 608)
        Me.CloseButton.Name = "CloseButton"
        Me.CloseButton.Size = New System.Drawing.Size(123, 50)
        Me.CloseButton.TabIndex = 6
        Me.CloseButton.Text = "ESC" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "終了"
        Me.CloseButton.UseVisualStyleBackColor = True
        '
        'OkButton
        '
        Me.OkButton.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.OkButton.Location = New System.Drawing.Point(491, 608)
        Me.OkButton.Name = "OkButton"
        Me.OkButton.Size = New System.Drawing.Size(123, 50)
        Me.OkButton.TabIndex = 5
        Me.OkButton.Text = "F5" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "登録"
        Me.OkButton.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.itemCodeText)
        Me.GroupBox1.Controls.Add(Me.Label21)
        Me.GroupBox1.Controls.Add(Me.itemNameText)
        Me.GroupBox1.Controls.Add(Me.Label22)
        Me.GroupBox1.Controls.Add(Me.codeText)
        Me.GroupBox1.Controls.Add(Me.Label23)
        Me.GroupBox1.Location = New System.Drawing.Point(42, 42)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(344, 269)
        Me.GroupBox1.TabIndex = 1
        Me.GroupBox1.TabStop = False
        '
        'itemCodeText
        '
        Me.itemCodeText.Font = New System.Drawing.Font("Segoe UI", 14.25!)
        Me.itemCodeText.Location = New System.Drawing.Point(183, 97)
        Me.itemCodeText.MaxLength = 20
        Me.itemCodeText.Name = "itemCodeText"
        Me.itemCodeText.Size = New System.Drawing.Size(147, 33)
        Me.itemCodeText.TabIndex = 3
        '
        'Label21
        '
        Me.Label21.AutoSize = True
        Me.Label21.Font = New System.Drawing.Font("Segoe UI", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label21.ForeColor = System.Drawing.Color.Black
        Me.Label21.Location = New System.Drawing.Point(6, 100)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(57, 30)
        Me.Label21.TabIndex = 2
        Me.Label21.Text = "品番"
        '
        'itemNameText
        '
        Me.itemNameText.Font = New System.Drawing.Font("Segoe UI", 14.25!)
        Me.itemNameText.Location = New System.Drawing.Point(183, 57)
        Me.itemNameText.MaxLength = 99
        Me.itemNameText.Name = "itemNameText"
        Me.itemNameText.Size = New System.Drawing.Size(147, 33)
        Me.itemNameText.TabIndex = 5
        '
        'Label22
        '
        Me.Label22.AutoSize = True
        Me.Label22.Font = New System.Drawing.Font("Segoe UI", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label22.ForeColor = System.Drawing.Color.Red
        Me.Label22.Location = New System.Drawing.Point(6, 60)
        Me.Label22.Name = "Label22"
        Me.Label22.Size = New System.Drawing.Size(57, 30)
        Me.Label22.TabIndex = 4
        Me.Label22.Text = "品名"
        '
        'codeText
        '
        Me.codeText.Font = New System.Drawing.Font("Segoe UI", 14.25!)
        Me.codeText.Location = New System.Drawing.Point(183, 17)
        Me.codeText.MaxLength = 8
        Me.codeText.Name = "codeText"
        Me.codeText.Size = New System.Drawing.Size(147, 33)
        Me.codeText.TabIndex = 1
        '
        'Label23
        '
        Me.Label23.AutoSize = True
        Me.Label23.Font = New System.Drawing.Font("Segoe UI", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label23.ForeColor = System.Drawing.Color.Red
        Me.Label23.Location = New System.Drawing.Point(6, 20)
        Me.Label23.Name = "Label23"
        Me.Label23.Size = New System.Drawing.Size(106, 30)
        Me.Label23.TabIndex = 0
        Me.Label23.Text = "呼出コード"
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.maxText)
        Me.GroupBox2.Controls.Add(Me.minText)
        Me.GroupBox2.Controls.Add(Me.Label9)
        Me.GroupBox2.Controls.Add(Me.standardText)
        Me.GroupBox2.Controls.Add(Me.Label5)
        Me.GroupBox2.Controls.Add(Me.targetQtyText)
        Me.GroupBox2.Controls.Add(Me.Label1)
        Me.GroupBox2.Controls.Add(Me.safetyFactorText)
        Me.GroupBox2.Controls.Add(Me.Label2)
        Me.GroupBox2.Controls.Add(Me.Label12)
        Me.GroupBox2.Location = New System.Drawing.Point(42, 317)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(344, 269)
        Me.GroupBox2.TabIndex = 3
        Me.GroupBox2.TabStop = False
        '
        'maxText
        '
        Me.maxText.Font = New System.Drawing.Font("Segoe UI", 14.25!)
        Me.maxText.Location = New System.Drawing.Point(183, 97)
        Me.maxText.MaxLength = 6
        Me.maxText.Name = "maxText"
        Me.maxText.Size = New System.Drawing.Size(147, 33)
        Me.maxText.TabIndex = 10
        '
        'minText
        '
        Me.minText.Font = New System.Drawing.Font("Segoe UI", 14.25!)
        Me.minText.Location = New System.Drawing.Point(183, 177)
        Me.minText.MaxLength = 6
        Me.minText.Name = "minText"
        Me.minText.Size = New System.Drawing.Size(147, 33)
        Me.minText.TabIndex = 9
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Segoe UI", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(6, 180)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(79, 30)
        Me.Label9.TabIndex = 8
        Me.Label9.Text = "下限値"
        '
        'standardText
        '
        Me.standardText.Font = New System.Drawing.Font("Segoe UI", 14.25!)
        Me.standardText.Location = New System.Drawing.Point(183, 137)
        Me.standardText.MaxLength = 6
        Me.standardText.Name = "standardText"
        Me.standardText.Size = New System.Drawing.Size(147, 33)
        Me.standardText.TabIndex = 7
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Segoe UI", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(6, 140)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(79, 30)
        Me.Label5.TabIndex = 6
        Me.Label5.Text = "基準値"
        '
        'targetQtyText
        '
        Me.targetQtyText.Font = New System.Drawing.Font("Segoe UI", 14.25!)
        Me.targetQtyText.Location = New System.Drawing.Point(183, 17)
        Me.targetQtyText.MaxLength = 6
        Me.targetQtyText.Name = "targetQtyText"
        Me.targetQtyText.Size = New System.Drawing.Size(147, 33)
        Me.targetQtyText.TabIndex = 3
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Segoe UI", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.Black
        Me.Label1.Location = New System.Drawing.Point(6, 20)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(101, 30)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "目標個数"
        '
        'safetyFactorText
        '
        Me.safetyFactorText.Font = New System.Drawing.Font("Segoe UI", 14.25!)
        Me.safetyFactorText.Location = New System.Drawing.Point(183, 57)
        Me.safetyFactorText.MaxLength = 6
        Me.safetyFactorText.Name = "safetyFactorText"
        Me.safetyFactorText.Size = New System.Drawing.Size(147, 33)
        Me.safetyFactorText.TabIndex = 1
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Segoe UI", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(6, 60)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(101, 30)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "安全計数"
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Font = New System.Drawing.Font("Segoe UI", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.Location = New System.Drawing.Point(6, 100)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(79, 30)
        Me.Label12.TabIndex = 4
        Me.Label12.Text = "上限値"
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.weightMinText)
        Me.GroupBox3.Controls.Add(Me.Label3)
        Me.GroupBox3.Controls.Add(Me.weightMinUnitCombo)
        Me.GroupBox3.Controls.Add(Me.Label4)
        Me.GroupBox3.Controls.Add(Me.weightMaxText)
        Me.GroupBox3.Controls.Add(Me.Label7)
        Me.GroupBox3.Controls.Add(Me.weightMaxUnitCombo)
        Me.GroupBox3.Controls.Add(Me.Label11)
        Me.GroupBox3.Controls.Add(Me.weightText)
        Me.GroupBox3.Controls.Add(Me.Label15)
        Me.GroupBox3.Controls.Add(Me.weightUnitCombo)
        Me.GroupBox3.Controls.Add(Me.Label24)
        Me.GroupBox3.Location = New System.Drawing.Point(398, 42)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(343, 269)
        Me.GroupBox3.TabIndex = 2
        Me.GroupBox3.TabStop = False
        '
        'weightMinText
        '
        Me.weightMinText.Font = New System.Drawing.Font("Segoe UI", 14.25!)
        Me.weightMinText.Location = New System.Drawing.Point(183, 181)
        Me.weightMinText.MaxLength = 10
        Me.weightMinText.Name = "weightMinText"
        Me.weightMinText.Size = New System.Drawing.Size(147, 33)
        Me.weightMinText.TabIndex = 9
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Segoe UI", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(6, 180)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(123, 30)
        Me.Label3.TabIndex = 8
        Me.Label3.Text = "単重下限値"
        '
        'weightMinUnitCombo
        '
        Me.weightMinUnitCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.weightMinUnitCombo.Enabled = False
        Me.weightMinUnitCombo.Font = New System.Drawing.Font("Segoe UI", 14.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.weightMinUnitCombo.FormattingEnabled = True
        Me.weightMinUnitCombo.Location = New System.Drawing.Point(183, 221)
        Me.weightMinUnitCombo.Name = "weightMinUnitCombo"
        Me.weightMinUnitCombo.Size = New System.Drawing.Size(147, 33)
        Me.weightMinUnitCombo.TabIndex = 11
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Segoe UI", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(6, 220)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(167, 30)
        Me.Label4.TabIndex = 10
        Me.Label4.Text = "単重下限値単位"
        '
        'weightMaxText
        '
        Me.weightMaxText.Font = New System.Drawing.Font("Segoe UI", 14.25!)
        Me.weightMaxText.Location = New System.Drawing.Point(183, 97)
        Me.weightMaxText.MaxLength = 10
        Me.weightMaxText.Name = "weightMaxText"
        Me.weightMaxText.Size = New System.Drawing.Size(147, 33)
        Me.weightMaxText.TabIndex = 5
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Segoe UI", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(6, 100)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(123, 30)
        Me.Label7.TabIndex = 4
        Me.Label7.Text = "単重上限値"
        '
        'weightMaxUnitCombo
        '
        Me.weightMaxUnitCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.weightMaxUnitCombo.Enabled = False
        Me.weightMaxUnitCombo.Font = New System.Drawing.Font("Segoe UI", 14.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.weightMaxUnitCombo.FormattingEnabled = True
        Me.weightMaxUnitCombo.Location = New System.Drawing.Point(183, 141)
        Me.weightMaxUnitCombo.Name = "weightMaxUnitCombo"
        Me.weightMaxUnitCombo.Size = New System.Drawing.Size(147, 33)
        Me.weightMaxUnitCombo.TabIndex = 7
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Font = New System.Drawing.Font("Segoe UI", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.Location = New System.Drawing.Point(6, 140)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(167, 30)
        Me.Label11.TabIndex = 6
        Me.Label11.Text = "単重上限値単位"
        '
        'weightText
        '
        Me.weightText.Font = New System.Drawing.Font("Segoe UI", 14.25!)
        Me.weightText.Location = New System.Drawing.Point(183, 17)
        Me.weightText.MaxLength = 10
        Me.weightText.Name = "weightText"
        Me.weightText.Size = New System.Drawing.Size(147, 33)
        Me.weightText.TabIndex = 1
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Font = New System.Drawing.Font("Segoe UI", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label15.ForeColor = System.Drawing.Color.Black
        Me.Label15.Location = New System.Drawing.Point(6, 20)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(57, 30)
        Me.Label15.TabIndex = 0
        Me.Label15.Text = "単重"
        '
        'weightUnitCombo
        '
        Me.weightUnitCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.weightUnitCombo.Enabled = False
        Me.weightUnitCombo.Font = New System.Drawing.Font("Segoe UI", 14.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.weightUnitCombo.FormattingEnabled = True
        Me.weightUnitCombo.Location = New System.Drawing.Point(183, 57)
        Me.weightUnitCombo.Name = "weightUnitCombo"
        Me.weightUnitCombo.Size = New System.Drawing.Size(147, 33)
        Me.weightUnitCombo.TabIndex = 3
        '
        'Label24
        '
        Me.Label24.AutoSize = True
        Me.Label24.Font = New System.Drawing.Font("Segoe UI", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label24.ForeColor = System.Drawing.Color.Black
        Me.Label24.Location = New System.Drawing.Point(6, 60)
        Me.Label24.Name = "Label24"
        Me.Label24.Size = New System.Drawing.Size(101, 30)
        Me.Label24.TabIndex = 2
        Me.Label24.Text = "単重単位"
        '
        'GroupBox4
        '
        Me.GroupBox4.Controls.Add(Me.tareWeightUnitTextBox)
        Me.GroupBox4.Controls.Add(Me.tareWeightTextBox)
        Me.GroupBox4.Controls.Add(Me.Label14)
        Me.GroupBox4.Controls.Add(Me.subtotalCountText)
        Me.GroupBox4.Controls.Add(Me.Label13)
        Me.GroupBox4.Controls.Add(Me.subtotalTargetText)
        Me.GroupBox4.Controls.Add(Me.Label10)
        Me.GroupBox4.Controls.Add(Me.tareCombo)
        Me.GroupBox4.Controls.Add(Me.Label6)
        Me.GroupBox4.Controls.Add(Me.Label8)
        Me.GroupBox4.Location = New System.Drawing.Point(397, 317)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(344, 269)
        Me.GroupBox4.TabIndex = 4
        Me.GroupBox4.TabStop = False
        '
        'tareWeightUnitTextBox
        '
        Me.tareWeightUnitTextBox.Enabled = False
        Me.tareWeightUnitTextBox.Font = New System.Drawing.Font("Segoe UI", 14.25!)
        Me.tareWeightUnitTextBox.Location = New System.Drawing.Point(183, 97)
        Me.tareWeightUnitTextBox.MaxLength = 6
        Me.tareWeightUnitTextBox.Name = "tareWeightUnitTextBox"
        Me.tareWeightUnitTextBox.Size = New System.Drawing.Size(147, 33)
        Me.tareWeightUnitTextBox.TabIndex = 16
        '
        'tareWeightTextBox
        '
        Me.tareWeightTextBox.Enabled = False
        Me.tareWeightTextBox.Font = New System.Drawing.Font("Segoe UI", 14.25!)
        Me.tareWeightTextBox.Location = New System.Drawing.Point(183, 58)
        Me.tareWeightTextBox.MaxLength = 6
        Me.tareWeightTextBox.Name = "tareWeightTextBox"
        Me.tareWeightTextBox.Size = New System.Drawing.Size(147, 33)
        Me.tareWeightTextBox.TabIndex = 15
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Font = New System.Drawing.Font("Segoe UI", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label14.Location = New System.Drawing.Point(6, 20)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(79, 30)
        Me.Label14.TabIndex = 14
        Me.Label14.Text = "風袋名"
        '
        'subtotalCountText
        '
        Me.subtotalCountText.Font = New System.Drawing.Font("Segoe UI", 14.25!)
        Me.subtotalCountText.Location = New System.Drawing.Point(183, 177)
        Me.subtotalCountText.MaxLength = 6
        Me.subtotalCountText.Name = "subtotalCountText"
        Me.subtotalCountText.Size = New System.Drawing.Size(147, 33)
        Me.subtotalCountText.TabIndex = 13
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Font = New System.Drawing.Font("Segoe UI", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.Location = New System.Drawing.Point(6, 180)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(145, 30)
        Me.Label13.TabIndex = 12
        Me.Label13.Text = "小計目標回数"
        '
        'subtotalTargetText
        '
        Me.subtotalTargetText.Font = New System.Drawing.Font("Segoe UI", 14.25!)
        Me.subtotalTargetText.Location = New System.Drawing.Point(183, 137)
        Me.subtotalTargetText.MaxLength = 6
        Me.subtotalTargetText.Name = "subtotalTargetText"
        Me.subtotalTargetText.Size = New System.Drawing.Size(147, 33)
        Me.subtotalTargetText.TabIndex = 11
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Segoe UI", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(6, 140)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(123, 30)
        Me.Label10.TabIndex = 10
        Me.Label10.Text = "小計目標値"
        '
        'tareCombo
        '
        Me.tareCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.tareCombo.Enabled = False
        Me.tareCombo.Font = New System.Drawing.Font("Segoe UI", 14.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.tareCombo.FormattingEnabled = True
        Me.tareCombo.Location = New System.Drawing.Point(183, 17)
        Me.tareCombo.Name = "tareCombo"
        Me.tareCombo.Size = New System.Drawing.Size(147, 33)
        Me.tareCombo.TabIndex = 1
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Segoe UI", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(6, 60)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(101, 30)
        Me.Label6.TabIndex = 0
        Me.Label6.Text = "風袋重量"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Segoe UI", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(6, 100)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(101, 30)
        Me.Label8.TabIndex = 2
        Me.Label8.Text = "風袋単位"
        '
        'Form_ItemDetail
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(754, 670)
        Me.Controls.Add(Me.GroupBox4)
        Me.Controls.Add(Me.GroupBox3)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.CloseButton)
        Me.Controls.Add(Me.OkButton)
        Me.Controls.Add(Me.TitleLabel)
        Me.Name = "Form_ItemDetail"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "ItemDetail"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.GroupBox4.ResumeLayout(False)
        Me.GroupBox4.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents TitleLabel As Label
    Friend WithEvents CloseButton As Button
    Friend WithEvents OkButton As Button
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents itemCodeText As TextBox
    Friend WithEvents Label21 As Label
    Friend WithEvents itemNameText As TextBox
    Friend WithEvents Label22 As Label
    Friend WithEvents codeText As TextBox
    Friend WithEvents Label23 As Label
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents targetQtyText As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents safetyFactorText As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents Label12 As Label
    Friend WithEvents GroupBox3 As GroupBox
    Friend WithEvents weightMinText As TextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents weightMinUnitCombo As ComboBox
    Friend WithEvents Label4 As Label
    Friend WithEvents weightMaxText As TextBox
    Friend WithEvents Label7 As Label
    Friend WithEvents weightMaxUnitCombo As ComboBox
    Friend WithEvents Label11 As Label
    Friend WithEvents weightText As TextBox
    Friend WithEvents Label15 As Label
    Friend WithEvents weightUnitCombo As ComboBox
    Friend WithEvents Label24 As Label
    Friend WithEvents GroupBox4 As GroupBox
    Friend WithEvents tareCombo As ComboBox
    Friend WithEvents Label6 As Label
    Friend WithEvents Label8 As Label
    Friend WithEvents minText As TextBox
    Friend WithEvents Label9 As Label
    Friend WithEvents standardText As TextBox
    Friend WithEvents Label5 As Label
    Friend WithEvents subtotalTargetText As TextBox
    Friend WithEvents Label10 As Label
    Friend WithEvents subtotalCountText As TextBox
    Friend WithEvents Label13 As Label
    Friend WithEvents maxText As TextBox
    Friend WithEvents tareWeightTextBox As TextBox
    Friend WithEvents Label14 As Label
    Friend WithEvents tareWeightUnitTextBox As TextBox
End Class
