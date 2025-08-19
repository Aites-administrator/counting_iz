'Imports GarbageCategoryMaster
'Imports GarbageTypeMaster
'Imports TenantMaster
'Imports AreaMaster
'Imports CorporateMaster
'Imports DetailOutput
'Imports MonthlyReportOutput
'Imports PackingLabelPrint
'Imports LogDisplay
'Imports PackingMaster
'Imports ScaleMaster
Imports Common.ClsFunction

Public Class Form_MainMenu
    Private Sub Form_MainMenu_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        MaximizeBox = False
        Dim updateTime As DateTime = System.IO.File.GetLastWriteTime(System.Reflection.Assembly.GetExecutingAssembly().Location)
        Text = "メインメニュー" & " ( " & updateTime & " ) "

    End Sub

    Private Sub CloseButton_Click(sender As Object, e As EventArgs) Handles CloseButton.Click
        Close()
    End Sub

    Private Sub GarbageTypeMasterButton_Click(sender As Object, e As EventArgs) Handles GarbageTypeMasterButton.Click
        'Dim cForm As New Form_GarbageTypeList
        'cForm.ShowDialog()
        'Dim appPath As String = System.Reflection.Assembly.GetExecutingAssembly().Location
        'ReadIniFile("M01")
        'Dim exePath As String
        'exePath = "C:\Users\1233\Desktop\remote-repo\KANSAI_ISHIDA_SANPAI\ソース\WindowsApp2\GarbageTypeMaster\bin\Debug\GarbageTypeMaster.exe"
        OpenForm("M01")
    End Sub

    Private Sub GarbageCategoryMasterButton_Click(sender As Object, e As EventArgs) Handles GarbageCategoryMasterButton.Click
        OpenForm("M02")
    End Sub
    Private Sub TenantMasterButton_Click(sender As Object, e As EventArgs) Handles TenantMasterButton.Click
        OpenForm("M03")
    End Sub

    Private Sub AreaMasterButton_Click(sender As Object, e As EventArgs) Handles AreaMasterButton.Click
        OpenForm("M04")
    End Sub

    Private Sub CorporateMasterButton_Click(sender As Object, e As EventArgs) Handles CorporateMasterButton.Click
        OpenForm("M05")
    End Sub
    Private Sub PackingMasterButton_Click(sender As Object, e As EventArgs) Handles PackingMasterButton.Click
        OpenForm("M06")
    End Sub

    Private Sub ScaleMasterButton_Click(sender As Object, e As EventArgs) Handles ScaleMasterButton.Click
        OpenForm("M07")
    End Sub

    Private Sub DetailOutputButton_Click(sender As Object, e As EventArgs) Handles DetailOutputButton.Click
        OpenForm("OTH01")
    End Sub

    Private Sub MonthlyReportOutputButton_Click(sender As Object, e As EventArgs) Handles MonthlyReportOutputButton.Click
        OpenForm("OTH02")
    End Sub

    Private Sub PackingLabelPrintButton_Click(sender As Object, e As EventArgs) Handles PackingLabelPrintButton.Click
        OpenForm("OTH03")
    End Sub

    Private Sub LogDisplayButton_Click(sender As Object, e As EventArgs) Handles LogDisplayButton.Click
        OpenForm("OTH04")
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        OpenForm("OTH05")
    End Sub

    'Private Sub GarbageTypeMasterButton_MouseEnter(sender As Object, e As EventArgs) Handles GarbageTypeMasterButton.MouseEnter
    '    GarbageTypeMasterButton.ForeColor = Color.Black
    '    GarbageTypeMasterButton.Font = New Font("MS UI Gothic", 12, FontStyle.Bold)
    'End Sub

    'Private Sub GarbageTypeMasterButton_MouseLeave(sender As Object, e As EventArgs) Handles GarbageTypeMasterButton.MouseLeave
    '    GarbageTypeMasterButton.ForeColor = Color.Black
    '    GarbageTypeMasterButton.Font = New Font("MS UI Gothic", 9, FontStyle.Regular)
    'End Sub

    'Private Sub OpenForm(StrKey As String)
    '    Dim exePath As String = ReadIniFile(StrKey)
    '    Dim psi As New ProcessStartInfo(exePath)
    '    System.Diagnostics.Process.Start(psi)
    'End Sub

    'Private Function ReadIniFile(strKey As String)
    '    Dim strPath As String = System.IO.Path.GetDirectoryName(Application.ExecutablePath) & "\MainMenu.ini"
    '    Dim exeName As String = GetIniString(strKey, "EXE", strPath)
    '    Return exeName
    'End Function

End Class
