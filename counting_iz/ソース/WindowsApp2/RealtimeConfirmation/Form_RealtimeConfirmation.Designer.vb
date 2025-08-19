<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form_RealtimeConfirmation
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
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.TitleLabel = New System.Windows.Forms.Label()
        Me.ResultDetail = New System.Windows.Forms.DataGridView()
        Me.SendButton = New System.Windows.Forms.Button()
        Me.ReceiveButton = New System.Windows.Forms.Button()
        Me.CloseButton = New System.Windows.Forms.Button()
        Me.USB_ReceiveButton = New System.Windows.Forms.Button()
        Me.USB_SendButton = New System.Windows.Forms.Button()
        Me.chkSendFreeMaster = New System.Windows.Forms.CheckBox()
        CType(Me.ResultDetail, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'TitleLabel
        '
        Me.TitleLabel.AutoSize = True
        Me.TitleLabel.Font = New System.Drawing.Font("Segoe UI", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TitleLabel.Location = New System.Drawing.Point(12, 9)
        Me.TitleLabel.Name = "TitleLabel"
        Me.TitleLabel.Size = New System.Drawing.Size(123, 30)
        Me.TitleLabel.TabIndex = 0
        Me.TitleLabel.Text = "計量器通信"
        '
        'ResultDetail
        '
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.ResultDetail.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle3
        Me.ResultDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle4.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.ResultDetail.DefaultCellStyle = DataGridViewCellStyle4
        Me.ResultDetail.Location = New System.Drawing.Point(18, 51)
        Me.ResultDetail.Name = "ResultDetail"
        Me.ResultDetail.ReadOnly = True
        Me.ResultDetail.RowTemplate.Height = 30
        Me.ResultDetail.Size = New System.Drawing.Size(668, 368)
        Me.ResultDetail.TabIndex = 1
        Me.ResultDetail.TabStop = False
        '
        'SendButton
        '
        Me.SendButton.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.SendButton.Location = New System.Drawing.Point(695, 107)
        Me.SendButton.Name = "SendButton"
        Me.SendButton.Size = New System.Drawing.Size(123, 50)
        Me.SendButton.TabIndex = 3
        Me.SendButton.Text = "F2" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "マスタ送信"
        Me.SendButton.UseVisualStyleBackColor = True
        '
        'ReceiveButton
        '
        Me.ReceiveButton.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.ReceiveButton.Location = New System.Drawing.Point(695, 51)
        Me.ReceiveButton.Name = "ReceiveButton"
        Me.ReceiveButton.Size = New System.Drawing.Size(123, 50)
        Me.ReceiveButton.TabIndex = 2
        Me.ReceiveButton.Text = "F1" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "実績受信"
        Me.ReceiveButton.UseVisualStyleBackColor = True
        '
        'CloseButton
        '
        Me.CloseButton.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.CloseButton.Location = New System.Drawing.Point(695, 369)
        Me.CloseButton.Name = "CloseButton"
        Me.CloseButton.Size = New System.Drawing.Size(123, 50)
        Me.CloseButton.TabIndex = 7
        Me.CloseButton.Text = "ESC" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "終了"
        Me.CloseButton.UseVisualStyleBackColor = True
        '
        'USB_ReceiveButton
        '
        Me.USB_ReceiveButton.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.USB_ReceiveButton.Location = New System.Drawing.Point(697, 213)
        Me.USB_ReceiveButton.Name = "USB_ReceiveButton"
        Me.USB_ReceiveButton.Size = New System.Drawing.Size(123, 50)
        Me.USB_ReceiveButton.TabIndex = 5
        Me.USB_ReceiveButton.Text = "F3" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "実績受信(USB)"
        Me.USB_ReceiveButton.UseVisualStyleBackColor = True
        '
        'USB_SendButton
        '
        Me.USB_SendButton.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.USB_SendButton.Location = New System.Drawing.Point(697, 269)
        Me.USB_SendButton.Name = "USB_SendButton"
        Me.USB_SendButton.Size = New System.Drawing.Size(123, 50)
        Me.USB_SendButton.TabIndex = 6
        Me.USB_SendButton.Text = "F4" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "マスタ送信(USB)"
        Me.USB_SendButton.UseVisualStyleBackColor = True
        '
        'chkSendFreeMaster
        '
        Me.chkSendFreeMaster.AutoSize = True
        Me.chkSendFreeMaster.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkSendFreeMaster.Location = New System.Drawing.Point(697, 161)
        Me.chkSendFreeMaster.Name = "chkSendFreeMaster"
        Me.chkSendFreeMaster.Size = New System.Drawing.Size(95, 25)
        Me.chkSendFreeMaster.TabIndex = 4
        Me.chkSendFreeMaster.Text = "フリー含む"
        Me.chkSendFreeMaster.UseVisualStyleBackColor = True
        '
        'Form_RealtimeConfirmation
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(832, 431)
        Me.Controls.Add(Me.chkSendFreeMaster)
        Me.Controls.Add(Me.USB_SendButton)
        Me.Controls.Add(Me.USB_ReceiveButton)
        Me.Controls.Add(Me.CloseButton)
        Me.Controls.Add(Me.ReceiveButton)
        Me.Controls.Add(Me.ResultDetail)
        Me.Controls.Add(Me.SendButton)
        Me.Controls.Add(Me.TitleLabel)
        Me.Name = "Form_RealtimeConfirmation"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "計量器通信"
        CType(Me.ResultDetail, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents TitleLabel As Label
    Friend WithEvents ResultDetail As DataGridView
    Friend WithEvents SendButton As Button
    Friend WithEvents ReceiveButton As Button
    Friend WithEvents CloseButton As Button
    Friend WithEvents USB_ReceiveButton As Button
    Friend WithEvents USB_SendButton As Button
    Friend WithEvents chkSendFreeMaster As CheckBox
End Class
