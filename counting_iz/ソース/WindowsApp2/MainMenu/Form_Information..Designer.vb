<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form_Information
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
        Me.Label_Title = New System.Windows.Forms.Label()
        Me.GroupBox_Support = New System.Windows.Forms.GroupBox()
        Me.Label_SupportTime = New System.Windows.Forms.Label()
        Me.Label_Email = New System.Windows.Forms.Label()
        Me.Label_Tel = New System.Windows.Forms.Label()
        Me.Label_TenantName = New System.Windows.Forms.Label()
        Me.Label_CompanyName = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label_Announcement = New System.Windows.Forms.Label()
        Me.CloseButton = New System.Windows.Forms.Button()
        Me.GroupBox_Support.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label_Title
        '
        Me.Label_Title.AutoSize = True
        Me.Label_Title.Font = New System.Drawing.Font("Segoe UI", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label_Title.Location = New System.Drawing.Point(6, 9)
        Me.Label_Title.Name = "Label_Title"
        Me.Label_Title.Size = New System.Drawing.Size(231, 30)
        Me.Label_Title.TabIndex = 0
        Me.Label_Title.Text = "計量量管理システム(DX)"
        '
        'GroupBox_Support
        '
        Me.GroupBox_Support.Controls.Add(Me.Label_SupportTime)
        Me.GroupBox_Support.Controls.Add(Me.Label_Email)
        Me.GroupBox_Support.Controls.Add(Me.Label_Tel)
        Me.GroupBox_Support.Controls.Add(Me.Label_TenantName)
        Me.GroupBox_Support.Controls.Add(Me.Label_CompanyName)
        Me.GroupBox_Support.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.GroupBox_Support.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox_Support.Location = New System.Drawing.Point(18, 52)
        Me.GroupBox_Support.Name = "GroupBox_Support"
        Me.GroupBox_Support.Size = New System.Drawing.Size(331, 168)
        Me.GroupBox_Support.TabIndex = 2
        Me.GroupBox_Support.TabStop = False
        Me.GroupBox_Support.Text = "サポート情報"
        '
        'Label_SupportTime
        '
        Me.Label_SupportTime.AutoSize = True
        Me.Label_SupportTime.Location = New System.Drawing.Point(15, 132)
        Me.Label_SupportTime.Name = "Label_SupportTime"
        Me.Label_SupportTime.Size = New System.Drawing.Size(253, 21)
        Me.Label_SupportTime.TabIndex = 4
        Me.Label_SupportTime.Text = "(月) ～ (金) AM9：00 ～ PM5：00"
        '
        'Label_Email
        '
        Me.Label_Email.AutoSize = True
        Me.Label_Email.Location = New System.Drawing.Point(15, 108)
        Me.Label_Email.Name = "Label_Email"
        Me.Label_Email.Size = New System.Drawing.Size(202, 21)
        Me.Label_Email.TabIndex = 3
        Me.Label_Email.Text = "E-mail : sys-info@aites.co.jp"
        '
        'Label_Tel
        '
        Me.Label_Tel.AutoSize = True
        Me.Label_Tel.Location = New System.Drawing.Point(15, 86)
        Me.Label_Tel.Name = "Label_Tel"
        Me.Label_Tel.Size = New System.Drawing.Size(141, 21)
        Me.Label_Tel.TabIndex = 2
        Me.Label_Tel.Text = "Tel : 075-752-0111"
        '
        'Label_TenantName
        '
        Me.Label_TenantName.AutoSize = True
        Me.Label_TenantName.Location = New System.Drawing.Point(83, 54)
        Me.Label_TenantName.Name = "Label_TenantName"
        Me.Label_TenantName.Size = New System.Drawing.Size(98, 21)
        Me.Label_TenantName.TabIndex = 1
        Me.Label_TenantName.Text = "システム開発"
        '
        'Label_CompanyName
        '
        Me.Label_CompanyName.AutoSize = True
        Me.Label_CompanyName.Location = New System.Drawing.Point(15, 32)
        Me.Label_CompanyName.Name = "Label_CompanyName"
        Me.Label_CompanyName.Size = New System.Drawing.Size(167, 21)
        Me.Label_CompanyName.TabIndex = 0
        Me.Label_CompanyName.Text = "イシダアイテス株式会社"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(16, 208)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(0, 12)
        Me.Label7.TabIndex = 3
        '
        'Label_Announcement
        '
        Me.Label_Announcement.AutoSize = True
        Me.Label_Announcement.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label_Announcement.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label_Announcement.Location = New System.Drawing.Point(16, 233)
        Me.Label_Announcement.Name = "Label_Announcement"
        Me.Label_Announcement.Size = New System.Drawing.Size(280, 21)
        Me.Label_Announcement.TabIndex = 4
        Me.Label_Announcement.Text = "本ソフトのご使用はパソコン1台のみとし、"
        '
        'CloseButton
        '
        Me.CloseButton.Font = New System.Drawing.Font("Segoe UI", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CloseButton.Location = New System.Drawing.Point(372, 326)
        Me.CloseButton.Name = "CloseButton"
        Me.CloseButton.Size = New System.Drawing.Size(123, 43)
        Me.CloseButton.TabIndex = 10
        Me.CloseButton.Text = "終了"
        Me.CloseButton.UseVisualStyleBackColor = True
        '
        'Form_Information
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(507, 381)
        Me.Controls.Add(Me.CloseButton)
        Me.Controls.Add(Me.Label_Announcement)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.GroupBox_Support)
        Me.Controls.Add(Me.Label_Title)
        Me.Name = "Form_Information"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "サポート情報"
        Me.GroupBox_Support.ResumeLayout(False)
        Me.GroupBox_Support.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Label_Title As Label
    Friend WithEvents GroupBox_Support As GroupBox
    Friend WithEvents Label_SupportTime As Label
    Friend WithEvents Label_Email As Label
    Friend WithEvents Label_Tel As Label
    Friend WithEvents Label_TenantName As Label
    Friend WithEvents Label_CompanyName As Label
    Friend WithEvents Label7 As Label
    Friend WithEvents Label_Announcement As Label
    Friend WithEvents CloseButton As Button
End Class
