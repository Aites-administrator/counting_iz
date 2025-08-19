Public Class Form_Information
  Private Sub Form_Information_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    MaximizeBox = False
    FormBorderStyle = FormBorderStyle.FixedSingle

    Label_Title.Text = "IZデジタルスムーズ.DX"
    GroupBox_Support.Text = "サポート情報"
    Label_CompanyName.Text = "イシダアイテス株式会社"
    Label_TenantName.Text = "システム開発"
    Label_Tel.Text = "Tel : 075-752-0111"
    Label_Email.Text = "E-mail : sys-se@aites.co.jp"
    Label_SupportTime.Text = "(月) ～ (金) AM9：00 ～ PM5：30"
    Label_Announcement.Text = "本ソフトのご使用はパソコン1台のみとし、" & vbCrLf & "イシダアイテス株式会社の断りなく他の法人に" & vbCrLf & "転売・貸与・譲渡及びコピーし複数台でのご使用は許されません。" & vbCrLf & "又、解析・改変する事も許されません。"
  End Sub

  Private Sub CloseButton_Click(sender As Object, e As EventArgs) Handles CloseButton.Click
    Close()
  End Sub
End Class