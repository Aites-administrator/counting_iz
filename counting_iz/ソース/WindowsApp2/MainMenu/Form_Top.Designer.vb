<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form_Top
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form_Top))
        Me.PictureBox_Log = New System.Windows.Forms.PictureBox()
        Me.PictureBox_MasterMente = New System.Windows.Forms.PictureBox()
        Me.PictureBox_OutPut = New System.Windows.Forms.PictureBox()
        Me.LogoPictureBox = New System.Windows.Forms.PictureBox()
        Me.PictureBox_AchievementMente = New System.Windows.Forms.PictureBox()
        Me.MenuGroupBox = New System.Windows.Forms.GroupBox()
        Me.LogButton = New System.Windows.Forms.Button()
        Me.OutPutButton = New System.Windows.Forms.Button()
        Me.ResultsButton = New System.Windows.Forms.Button()
        Me.MasterMenteButton = New System.Windows.Forms.Button()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.CloseButton = New System.Windows.Forms.Button()
        Me.PictureBox_Close = New System.Windows.Forms.PictureBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.MenuStrip2 = New System.Windows.Forms.MenuStrip()
        Me.Support_MenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.RunSqlServerBat = New System.Windows.Forms.ToolStripMenuItem()
        Me.Label3 = New System.Windows.Forms.Label()
        CType(Me.PictureBox_Log, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox_MasterMente, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox_OutPut, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LogoPictureBox, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox_AchievementMente, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.MenuGroupBox.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox_Close, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.MenuStrip2.SuspendLayout()
        Me.SuspendLayout()
        '
        'PictureBox_Log
        '
        Me.PictureBox_Log.Image = CType(resources.GetObject("PictureBox_Log.Image"), System.Drawing.Image)
        Me.PictureBox_Log.Location = New System.Drawing.Point(27, 415)
        Me.PictureBox_Log.Name = "PictureBox_Log"
        Me.PictureBox_Log.Size = New System.Drawing.Size(78, 70)
        Me.PictureBox_Log.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.PictureBox_Log.TabIndex = 17
        Me.PictureBox_Log.TabStop = False
        '
        'PictureBox_MasterMente
        '
        Me.PictureBox_MasterMente.Image = CType(resources.GetObject("PictureBox_MasterMente.Image"), System.Drawing.Image)
        Me.PictureBox_MasterMente.Location = New System.Drawing.Point(27, 53)
        Me.PictureBox_MasterMente.Name = "PictureBox_MasterMente"
        Me.PictureBox_MasterMente.Size = New System.Drawing.Size(78, 70)
        Me.PictureBox_MasterMente.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.PictureBox_MasterMente.TabIndex = 18
        Me.PictureBox_MasterMente.TabStop = False
        Me.PictureBox_MasterMente.Tag = "test"
        '
        'PictureBox_OutPut
        '
        Me.PictureBox_OutPut.Image = CType(resources.GetObject("PictureBox_OutPut.Image"), System.Drawing.Image)
        Me.PictureBox_OutPut.Location = New System.Drawing.Point(27, 293)
        Me.PictureBox_OutPut.Name = "PictureBox_OutPut"
        Me.PictureBox_OutPut.Size = New System.Drawing.Size(78, 70)
        Me.PictureBox_OutPut.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.PictureBox_OutPut.TabIndex = 19
        Me.PictureBox_OutPut.TabStop = False
        '
        'LogoPictureBox
        '
        Me.LogoPictureBox.Image = CType(resources.GetObject("LogoPictureBox.Image"), System.Drawing.Image)
        Me.LogoPictureBox.Location = New System.Drawing.Point(671, 48)
        Me.LogoPictureBox.Name = "LogoPictureBox"
        Me.LogoPictureBox.Size = New System.Drawing.Size(246, 60)
        Me.LogoPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.LogoPictureBox.TabIndex = 24
        Me.LogoPictureBox.TabStop = False
        '
        'PictureBox_AchievementMente
        '
        Me.PictureBox_AchievementMente.Image = CType(resources.GetObject("PictureBox_AchievementMente.Image"), System.Drawing.Image)
        Me.PictureBox_AchievementMente.Location = New System.Drawing.Point(27, 172)
        Me.PictureBox_AchievementMente.Name = "PictureBox_AchievementMente"
        Me.PictureBox_AchievementMente.Size = New System.Drawing.Size(78, 70)
        Me.PictureBox_AchievementMente.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.PictureBox_AchievementMente.TabIndex = 25
        Me.PictureBox_AchievementMente.TabStop = False
        Me.PictureBox_AchievementMente.Tag = "test"
        '
        'MenuGroupBox
        '
        Me.MenuGroupBox.Controls.Add(Me.LogButton)
        Me.MenuGroupBox.Controls.Add(Me.OutPutButton)
        Me.MenuGroupBox.Controls.Add(Me.ResultsButton)
        Me.MenuGroupBox.Controls.Add(Me.MasterMenteButton)
        Me.MenuGroupBox.Controls.Add(Me.PictureBox_MasterMente)
        Me.MenuGroupBox.Controls.Add(Me.PictureBox_Log)
        Me.MenuGroupBox.Controls.Add(Me.PictureBox_AchievementMente)
        Me.MenuGroupBox.Controls.Add(Me.PictureBox_OutPut)
        Me.MenuGroupBox.Font = New System.Drawing.Font("Segoe UI", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.MenuGroupBox.ForeColor = System.Drawing.Color.Black
        Me.MenuGroupBox.Location = New System.Drawing.Point(27, 98)
        Me.MenuGroupBox.Name = "MenuGroupBox"
        Me.MenuGroupBox.Size = New System.Drawing.Size(448, 531)
        Me.MenuGroupBox.TabIndex = 3
        Me.MenuGroupBox.TabStop = False
        Me.MenuGroupBox.Text = "選択"
        '
        'LogButton
        '
        Me.LogButton.BackColor = System.Drawing.Color.White
        Me.LogButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.LogButton.Font = New System.Drawing.Font("Segoe UI", 20.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LogButton.Location = New System.Drawing.Point(126, 415)
        Me.LogButton.Name = "LogButton"
        Me.LogButton.Size = New System.Drawing.Size(293, 70)
        Me.LogButton.TabIndex = 3
        Me.LogButton.Text = "F4 : 計量器管理　　"
        Me.LogButton.UseVisualStyleBackColor = False
        '
        'OutPutButton
        '
        Me.OutPutButton.BackColor = System.Drawing.Color.White
        Me.OutPutButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.OutPutButton.Font = New System.Drawing.Font("Segoe UI", 20.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.OutPutButton.Location = New System.Drawing.Point(126, 293)
        Me.OutPutButton.Name = "OutPutButton"
        Me.OutPutButton.Size = New System.Drawing.Size(293, 70)
        Me.OutPutButton.TabIndex = 2
        Me.OutPutButton.Text = "F3 : 累計データ管理"
        Me.OutPutButton.UseVisualStyleBackColor = False
        '
        'ResultsButton
        '
        Me.ResultsButton.BackColor = System.Drawing.Color.White
        Me.ResultsButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ResultsButton.Font = New System.Drawing.Font("Segoe UI", 20.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ResultsButton.Location = New System.Drawing.Point(126, 172)
        Me.ResultsButton.Name = "ResultsButton"
        Me.ResultsButton.Size = New System.Drawing.Size(293, 70)
        Me.ResultsButton.TabIndex = 1
        Me.ResultsButton.Text = "F2 : 実績メンテナンス"
        Me.ResultsButton.UseVisualStyleBackColor = False
        '
        'MasterMenteButton
        '
        Me.MasterMenteButton.BackColor = System.Drawing.Color.White
        Me.MasterMenteButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.MasterMenteButton.Font = New System.Drawing.Font("Segoe UI", 20.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.MasterMenteButton.Location = New System.Drawing.Point(126, 53)
        Me.MasterMenteButton.Name = "MasterMenteButton"
        Me.MasterMenteButton.Size = New System.Drawing.Size(293, 70)
        Me.MasterMenteButton.TabIndex = 0
        Me.MasterMenteButton.Text = "F1 : マスタメンテナンス"
        Me.MasterMenteButton.UseVisualStyleBackColor = False
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(525, 114)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(392, 378)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox1.TabIndex = 27
        Me.PictureBox1.TabStop = False
        '
        'CloseButton
        '
        Me.CloseButton.BackColor = System.Drawing.Color.White
        Me.CloseButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.CloseButton.Font = New System.Drawing.Font("Segoe UI", 20.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CloseButton.Location = New System.Drawing.Point(624, 513)
        Me.CloseButton.Name = "CloseButton"
        Me.CloseButton.Size = New System.Drawing.Size(293, 70)
        Me.CloseButton.TabIndex = 4
        Me.CloseButton.Text = "ESC : 終了"
        Me.CloseButton.UseVisualStyleBackColor = False
        '
        'PictureBox_Close
        '
        Me.PictureBox_Close.Image = CType(resources.GetObject("PictureBox_Close.Image"), System.Drawing.Image)
        Me.PictureBox_Close.Location = New System.Drawing.Point(525, 513)
        Me.PictureBox_Close.Name = "PictureBox_Close"
        Me.PictureBox_Close.Size = New System.Drawing.Size(78, 70)
        Me.PictureBox_Close.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.PictureBox_Close.TabIndex = 30
        Me.PictureBox_Close.TabStop = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.BackColor = System.Drawing.Color.Gray
        Me.Label1.Font = New System.Drawing.Font("Segoe UI", 24.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.White
        Me.Label1.Location = New System.Drawing.Point(29, 40)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(320, 45)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "IZデジタルスムーズ.DX"
        '
        'MenuStrip2
        '
        Me.MenuStrip2.ImageScalingSize = New System.Drawing.Size(24, 24)
        Me.MenuStrip2.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.Support_MenuItem, Me.RunSqlServerBat})
        Me.MenuStrip2.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip2.Name = "MenuStrip2"
        Me.MenuStrip2.Size = New System.Drawing.Size(958, 24)
        Me.MenuStrip2.TabIndex = 0
        Me.MenuStrip2.Text = "MenuStrip2"
        '
        'Support_MenuItem
        '
        Me.Support_MenuItem.Name = "Support_MenuItem"
        Me.Support_MenuItem.Size = New System.Drawing.Size(93, 20)
        Me.Support_MenuItem.Text = "サポート情報(&S)"
        '
        'RunSqlServerBat
        '
        Me.RunSqlServerBat.Name = "RunSqlServerBat"
        Me.RunSqlServerBat.Size = New System.Drawing.Size(123, 20)
        Me.RunSqlServerBat.Text = "データのバックアップ(&B)"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Segoe UI", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(355, 53)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(177, 32)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "【カウンティング】"
        '
        'Form_Top
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Window
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.ClientSize = New System.Drawing.Size(958, 658)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.CloseButton)
        Me.Controls.Add(Me.PictureBox_Close)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.MenuGroupBox)
        Me.Controls.Add(Me.LogoPictureBox)
        Me.Controls.Add(Me.MenuStrip2)
        Me.ForeColor = System.Drawing.SystemColors.ControlText
        Me.KeyPreview = True
        Me.Name = "Form_Top"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "IZデジタルスムーズ.DX（カウンティング）"
        CType(Me.PictureBox_Log, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox_MasterMente, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox_OutPut, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LogoPictureBox, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox_AchievementMente, System.ComponentModel.ISupportInitialize).EndInit()
        Me.MenuGroupBox.ResumeLayout(False)
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox_Close, System.ComponentModel.ISupportInitialize).EndInit()
        Me.MenuStrip2.ResumeLayout(False)
        Me.MenuStrip2.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents PictureBox_Log As PictureBox
    Friend WithEvents PictureBox_MasterMente As PictureBox
    Friend WithEvents PictureBox_OutPut As PictureBox
    Friend WithEvents LogoPictureBox As PictureBox
    Friend WithEvents PictureBox_AchievementMente As PictureBox
    Friend WithEvents MenuGroupBox As GroupBox
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents LogButton As Button
    Friend WithEvents OutPutButton As Button
    Friend WithEvents ResultsButton As Button
    Friend WithEvents MasterMenteButton As Button
    Friend WithEvents CloseButton As Button
    Friend WithEvents PictureBox_Close As PictureBox
    Friend WithEvents Label1 As Label
    Friend WithEvents MenuStrip2 As MenuStrip
    Friend WithEvents Support_MenuItem As ToolStripMenuItem
    Friend WithEvents RunSqlServerBat As ToolStripMenuItem
    Friend WithEvents Label3 As Label
End Class
