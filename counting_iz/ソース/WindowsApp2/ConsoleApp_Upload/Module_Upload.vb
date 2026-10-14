Imports System.IO
Imports System.Text
Imports System.Net.NetworkInformation
Imports System.Windows.Forms
Imports Common
Imports Common.ClsFunction
Module Module_Upload
  Private ReadOnly tmpDb As New ClsSqlServer
  Dim tmpDt As New DataTable
  ' SQLサーバー操作オブジェクト
  Private _SqlServer As ClsSqlServer
  Private ReadOnly Property SqlServer As ClsSqlServer
    Get
      If _SqlServer Is Nothing Then
        _SqlServer = New ClsSqlServer
      End If
      Return _SqlServer
    End Get
  End Property

  ReadOnly FtpId As String = ReadSettingIniFile("FTP_ID", "VALUE")
  ReadOnly FtpPw As String = ReadSettingIniFile("FTP_PW", "VALUE")

  ReadOnly FtpUploadPath As String = ReadSettingIniFile("FTP_UPLOAD_PATH", "VALUE")
  ReadOnly FtpDownloadPath As String = ReadSettingIniFile("FTP_DOWNLOAD_PATH", "VALUE")
  ReadOnly FtpAnsPath As String = ReadSettingIniFile("FTP_BACKUP_PATH", "VALUE")

  ReadOnly FileNameDigits As String = ReadSettingIniFile("FILENAME_DIGITS", "VALUE")
  ReadOnly CutFileNameDigits As String = ReadSettingIniFile("CUT_FILENAME_DIGITS", "VALUE")

  Dim UnitNumberString As String
  Dim IpAddressString As String
  Dim UnitNumberArray() As String
  Dim IpAddressArray() As String
  Dim PathName As String
  Dim TableName As String
  Dim DefText As String
  Dim ErrorJudFlg As Boolean
  Private ReadOnly GeneratedMasterFiles As New HashSet(Of String)(StringComparer.OrdinalIgnoreCase)
  Dim AnsErrorJudFlg As Boolean

  Sub Main(args() As String)
    InitializeExceptionLogging()
    Try

    '  受信引数を1つの文字列として結合
    Dim allArgs As String = String.Join(" ", args)
    Dim parts() As String = allArgs.Split("|"c)
    '  スケール番号部分をスペース区切りで取得
    Dim scaleNumbers() As String = parts(0).Trim().Split(" "c)
    Dim isFreeMaster As Boolean = False
    If parts.Length > 1 Then Boolean.TryParse(parts(1).Trim(), isFreeMaster)

    Dim Para_ScaleNumber As String = String.Empty
    Dim DownloadPath1 As String
    Dim DownloadPath2 As String
    If scaleNumbers.Length > 0 Then
      For i As Integer = 0 To scaleNumbers.Length - 1
        If i = 0 Then
          Para_ScaleNumber = scaleNumbers(i)
        Else
          Para_ScaleNumber = Para_ScaleNumber + "," + scaleNumbers(i)
        End If
      Next
    End If

    '「計量器マスタ」取得
    SelectScaleMaster(Para_ScaleNumber)
    '「商品マスタ」CSV作成
    CreateItemMasterCSV()
    '「製造者マスタ」のCSVデータ作成
    CreateManufacturerMasterCSV()
    '「担当者マスタ」のCSVデータ作成
    CreateStaffMasterCSV()
    '「風袋マスタ」のCSVデータ作成
    CreatePackingMasterCSV()
    '「作業指示マスタ」のCSVデータ作成
    CreateWorkOrderMasterCSV()

    If isFreeMaster Then
      ' 「フリー1マスタ」CSV作成
      CreateFree1MasterCSV()
      ' 「フリー2マスタ」CSV作成
      CreateFree2MasterCSV()
      ' 「フリー3マスタ」CSV作成
      CreateFree3MasterCSV()
      ' 「フリー4マスタ」CSV作成
      CreateFree4MasterCSV()
      ' 「フリー5マスタ」CSV作成
      CreateFree5MasterCSV()
    End If

    ''「実績」のDEF作成
    'CreateResultsCSV()

    If GeneratedMasterFiles.Count = 0 Then
      For Each unitNumber In UnitNumberArray
        InsertTRNLOG(unitNumber, "OK", "", "データが存在する送信対象マスタはありません")
      Next
      Return
    End If
    System.Threading.Thread.Sleep(1000)
    '引数定義
    Dim UploadPath As String
    Dim URL As String
    '計量器毎にループ(（マスタ）
    For j As Integer = 0 To UnitNumberArray.Length - 1
      '正常処理終了フラグ初期化★★★
      ErrorJudFlg = False
      AnsErrorJudFlg = False

      Dim prefixes As New List(Of String) From {"40MAS1", "40MAS2", "40TARE", "40OPTR", "40INST"}
      If isFreeMaster Then prefixes.AddRange(New String() {"40FRE1", "40FRE2", "40FRE3", "40FRE4", "40FRE5"})
      Dim suffix As String = FormatUnitNumberForFileName(UnitNumberArray(j), FileNameDigits)
      For Each prefix As String In prefixes
        For Each extension As String In New String() {".DEF", ".CSV"}
          UploadPath = FtpUploadPath & prefix & suffix & extension
          If Not GeneratedMasterFiles.Contains(UploadPath) Then Continue For
          URL = "ftp://" & IpAddressArray(j) & "/" & prefix & suffix & extension
          UploadFtp(UploadPath, URL, UnitNumberArray(j))
          If ErrorJudFlg Then Exit For
          System.Threading.Thread.Sleep(3000)
        Next
        If ErrorJudFlg Then Exit For
      Next
      'ログ登録　＆ アンサーファイル受信
      If ErrorJudFlg Then
        InsertTRNLOG(UnitNumberArray(j), "NG", "", "マスタ送信失敗")
      Else
        InsertTRNLOG(UnitNumberArray(j), "OK", "", "マスタ送信成功")

        DownloadPath1 = FtpAnsPath & "/40ANS1" & FormatUnitNumberForFileName(UnitNumberArray(j), FileNameDigits) & ".CSV"
        DownloadPath2 = FtpAnsPath & "/40ANS2" & FormatUnitNumberForFileName(UnitNumberArray(j), FileNameDigits) & ".CSV"
        URL = "ftp://" & IpAddressArray(j) & "/40ANS1" & FormatUnitNumberForFileName(UnitNumberArray(j), FileNameDigits) & ".CSV"
        AnsDownloadFtp(DownloadPath1, URL, UnitNumberArray(j))
        System.Threading.Thread.Sleep(2000)
        URL = "ftp://" & IpAddressArray(j) & "/40ANS2" & FormatUnitNumberForFileName(UnitNumberArray(j), FileNameDigits) & ".CSV"
        AnsDownloadFtp(DownloadPath2, URL, UnitNumberArray(j))
        System.Threading.Thread.Sleep(2000)
        If AnsErrorJudFlg Then
          InsertTRNLOG(UnitNumberArray(j), "", "", "アンサーファイル受信失敗")
        Else
          InsertTRNLOG(UnitNumberArray(j), "", "", "アンサーファイル受信成功")
        End If
      End If
    Next
    Catch ex As Exception
      Environment.ExitCode = 1
      ComWriteErrLog("ConsoleApp_Upload", "Main", ex)
      ShowErrorMessage("通信処理に失敗しました。" & vbCrLf & ex.Message)
    End Try
  End Sub

  Private Sub UploadFtp(UploadPath As String, URL As String, UnitNumber As String)
    Dim FILE_NAME As String = Path.GetFileName(New Uri(URL).AbsolutePath)
    Try
      'アップロード先のURI
      Dim URI As New Uri(URL)
      'FtpWebRequestの作成
      Dim ftpReq As System.Net.FtpWebRequest = CType(System.Net.WebRequest.Create(URI), System.Net.FtpWebRequest)
      'ログインユーザー名とパスワードを設定
      ftpReq.Credentials = New System.Net.NetworkCredential(FtpId, FtpPw)
      'MethodにWebRequestMethods.Ftp.UploadFile("STOR")を設定
      ftpReq.Method = System.Net.WebRequestMethods.Ftp.UploadFile
      '要求の完了後に接続を閉じる
      ftpReq.KeepAlive = False
      'ASCIIモードで転送する
      ftpReq.UseBinary = False
      'PASVモードを無効にする
      ftpReq.UsePassive = False
      'ファイルをアップロードするためのStreamを取得
      Using reqStrm As Stream = ftpReq.GetRequestStream(), fs As New FileStream(UploadPath, FileMode.Open, FileAccess.Read)
        fs.CopyTo(reqStrm)
      End Using
      Using ftpRes As System.Net.FtpWebResponse = CType(ftpReq.GetResponse(), System.Net.FtpWebResponse)
        InsertTRNLOG(UnitNumber, "", FILE_NAME, "マスタ送信完了（" & ftpRes.StatusDescription.Trim() & "）")
      End Using
    Catch ex As Exception
      '正常処理終了フラグ変更★★★
      Call ComWriteErrLog("Module_Upload",
                        System.Reflection.MethodBase.GetCurrentMethod().Name, ex)
      ErrorJudFlg = True
      InsertTRNLOG(UnitNumber, "", FILE_NAME, ex.Message)

    End Try
  End Sub

  Private Sub AnsDownloadFtp(DownloadPath As String, URL As String, UnitNumber As String)
    '★ファイルの名前命名によって桁数が違う
    Dim FILE_NAME As String = Path.GetFileName(New Uri(URL).AbsolutePath)
    Try
      'ダウンロードするファイルのURI
      Dim URI As New Uri(URL)
      'FtpWebRequestの作成
      Dim ftpReq As System.Net.FtpWebRequest =
          CType(System.Net.WebRequest.Create(URI), System.Net.FtpWebRequest)
      'ログインユーザー名とパスワードを設定
      ftpReq.Credentials = New System.Net.NetworkCredential(FtpId, FtpPw)
      'MethodにWebRequestMethods.Ftp.DownloadFile("RETR")を設定
      ftpReq.Method = System.Net.WebRequestMethods.Ftp.DownloadFile
      '要求の完了後に接続を閉じる
      ftpReq.KeepAlive = False
      'ASCIIモードで転送する
      ftpReq.UseBinary = False
      'PASSIVEモードを無効にする
      ftpReq.UsePassive = False
      Directory.CreateDirectory(Path.GetDirectoryName(Path.GetFullPath(DownloadPath)))
      Using ftpRes As System.Net.FtpWebResponse = CType(ftpReq.GetResponse(), System.Net.FtpWebResponse),
            resStrm As Stream = ftpRes.GetResponseStream(), fs As New FileStream(DownloadPath, FileMode.Create, FileAccess.Write)
        resStrm.CopyTo(fs)
        InsertTRNLOG(UnitNumber, "", FILE_NAME, "アンサーファイル受信完了（" & ftpRes.StatusDescription.Trim() & "）")
      End Using
    Catch ex As Exception
      AnsErrorJudFlg = True
      InsertTRNLOG(UnitNumber, "", FILE_NAME, ex.Message)
      Call ComWriteErrLog("Module_Upload",
                        System.Reflection.MethodBase.GetCurrentMethod().Name, ex)
      Console.WriteLine(ex.Message)
    End Try

  End Sub
  Public Sub SelectScaleMaster(Para_ScaleNumber As String)
    Dim sql As String = String.Empty
    sql = GetMstScaleSelectSql(Para_ScaleNumber)
    Try
      With tmpDb
        SqlServer.GetResult(tmpDt, sql)
        If tmpDt.Rows.Count = 0 Then
          Throw New InvalidOperationException("計量器マスタにデータが登録されていません。")
        Else
          For i As Integer = 0 To tmpDt.Rows.Count - 1
            If i = 0 Then
              UnitNumberString = tmpDt.Rows(i)(0)
              IpAddressString = tmpDt.Rows(i)(1)
            Else
              UnitNumberString = UnitNumberString + "," + tmpDt.Rows(i)(0)
              IpAddressString = IpAddressString + "," + tmpDt.Rows(i)(1)
            End If
          Next
        End If
      End With
      UnitNumberArray = UnitNumberString.Split(","c)
      IpAddressArray = IpAddressString.Split(","c)
      InsertTRNLOG("", "", "", "計量器マスタ取得")
    Catch ex As Exception
      Call ComWriteErrLog("Module_Upload",
                        System.Reflection.MethodBase.GetCurrentMethod().Name, ex)
      Throw
      InsertTRNLOG("", "", "", "計量器マスタ取得失敗")
    Finally
      tmpDt.Dispose()
    End Try
  End Sub
  Private Function GetMstScaleSelectSql(Para_ScaleNumber As String) As String
    Dim sql As String = String.Empty
    sql &= " SELECT"
    sql &= "     unit_number,"
    sql &= "     ip_address"
    sql &= " FROM"
    sql &= "     MST_Scale"
    sql &= " WHERE"
    sql &= "     1=1"
    If Para_ScaleNumber.Length <> 0 Then
      sql &= "     AND unit_number IN(" & Para_ScaleNumber & ")"
    End If
    sql &= " ORDER BY  "
    sql &= "     unit_number"
    Call WriteExecuteLog("Module_Upload", System.Reflection.MethodBase.GetCurrentMethod().Name, sql)
    Return sql

  End Function
  Private Sub CreateItemMasterCSV()
    PathName = "40MAS1"
    TableName = "MST_Item"
    DefText = "呼出コード:40001,品番:40002,単重:40003,単重単位:40004,安全計数:40005,目標個数:40006,風袋:40007,風袋単位:40008,上限値:40009,基準値:40011,下限値:40013,小計目標値:40028,小計目標回数:40030,品名:40031,単重上限値:40201,単重上限値単位:40202,単重下限値:40203,単重下限値単位:40204"
    CreateCsv(PathName, TableName, DefText)
  End Sub
  Private Sub CreateManufacturerMasterCSV()
    PathName = "40MAS2"
    TableName = "MST_Manufacturer"
    DefText = "製造者コード:40051,製造者名:40052"
    CreateCsv(PathName, TableName, DefText)
  End Sub

  Private Sub CreateStaffMasterCSV()
    PathName = "40OPTR"
    TableName = "MST_Staff"
    DefText = "担当者№:40361,担当者名称:40362"
    CreateCsv(PathName, TableName, DefText)
  End Sub
  Private Sub CreatePackingMasterCSV()
    PathName = "40TARE"
    TableName = "MST_Packing"
    DefText = "風袋№:40371,風袋重量:40372,風袋重量単位:40373,風袋名称:40374"
    CreateCsv(PathName, TableName, DefText)
  End Sub
  Private Sub CreateWorkOrderMasterCSV()
    PathName = "40INST"
    TableName = "MST_WorkOrder"
    DefText = "作業指示№:40400,明細№:40401,作業指示名称:40402,明細小計(する/しない):40403,商品№:40404,指示数:40405"
    CreateCsv(PathName, TableName, DefText)
  End Sub

  Private Sub CreateFree1MasterCSV()
    PathName = "40FRE1"
    TableName = "MST_Free1"
    DefText = "フリー１№:40311,フリー１名称:40312"
    CreateCsv(PathName, TableName, DefText)
  End Sub

  Private Sub CreateFree2MasterCSV()
    PathName = "40FRE2"
    TableName = "MST_Free2"
    DefText = "フリー２№:40321,フリー２名称:40322"
    CreateCsv(PathName, TableName, DefText)
  End Sub

  Private Sub CreateFree3MasterCSV()
    PathName = "40FRE3"
    TableName = "MST_Free3"
    DefText = "フリー３№:40331,フリー３名称:40332
"
    CreateCsv(PathName, TableName, DefText)
  End Sub

  Private Sub CreateFree4MasterCSV()
    PathName = "40FRE4"
    TableName = "MST_Free4"
    DefText = "フリー４№:40341,フリー４名称:40342"
    CreateCsv(PathName, TableName, DefText)
  End Sub

  Private Sub CreateFree5MasterCSV()
    PathName = "40FRE5"
    TableName = "MST_Free5"
    DefText = "フリー５№:40351,フリー５名称:40352"
    CreateCsv(PathName, TableName, DefText)
  End Sub

  'Private Sub CreateResultsCSV()
  '    PathName = "40TRAN"
  '    TableName = "TRN_Results"
  '    DefText = "日付:40101,時刻:40102,端末機ナンバー:40103,呼出コード:40001,品番:40002,品名:40031,風袋:40007,風袋単位:40008,重量:40105,重量単位:40106,製造者コード:40051,製造者名:40052,ロット１:40107,ロット２:40108,区分:40111,グロス重量:40127,グロス重量単位:40128,風袋１重量:40375,風袋１重量単位:40376,風袋２重量:40378,風袋２重量単位:40379,風袋２の掛け数:40242,風袋１№:40240,風袋１名称:40377,風袋２№:40241,風袋２名称:40380,担当者№:40361,担当者名称:40362,フリー１№:40244,フリー１名称:40312,フリー２№:40245,フリー２名称:40322,フリー３№:40246,フリー３名称:40332,フリー４№:40247,フリー４名称:40342,フリー５№:40248,フリー５名称:40352,加工日:40220,加工時刻:40221,有効日:40228,有効時刻:40229,作業指示№:40400,明細№:40401,指示数:40405,実績数:40406,作業指示名称:40402"

  '    CreateCsv(PathName, TableName, DefText)
  'End Sub

  Private Sub CreateCsv(PathName As String, TableName As String, DefText As String)
    Dim sql As String = String.Empty
    Select Case TableName
      Case "MST_Item"
        sql = GetItemMasterSelectSql()
      Case "MST_Manufacturer"
        sql = GetManufacturerMasterSelectSql()
      Case "MST_Packing"
        sql = GetMSTPackingSelectSql()
      Case "MST_Staff"
        sql = GetStaffMasterSelectSql()
      Case "MST_WorkOrder"
        sql = GetMSTWorkOrderSelectSql()
      Case "MST_Free1"
        sql = GetMST_Free1MasterSelectSql()
      Case "MST_Free2"
        sql = GetMST_Free2MasterSelectSql()
      Case "MST_Free3"
        sql = GetMST_Free3MasterSelectSql()
      Case "MST_Free4"
        sql = GetMST_Free4MasterSelectSql()
      Case "MST_Free5"
        sql = GetMST_Free5MasterSelectSql()
      Case Else
        Throw New InvalidOperationException("送信対象マスタが不明です: " & TableName)
    End Select
    Using output As New DataTable
      SqlServer.GetResult(output, sql)
      If output.Rows.Count = 0 Then
        For Each unitNumber In UnitNumberArray
          InsertTRNLOG(unitNumber, "", "", TableName & "が0件のため送信対象外")
        Next
        Return
      End If
      Directory.CreateDirectory(FtpUploadPath)
      Dim encoding As Encoding = Encoding.GetEncoding("Shift_JIS")
      For Each unitNumber In UnitNumberArray
        Dim suffix As String = FormatUnitNumberForFileName(unitNumber, FileNameDigits)
        Dim csvPath As String = FtpUploadPath & PathName & suffix & ".CSV"
        Dim defPath As String = FtpUploadPath & PathName & suffix & ".DEF"
        Using writer As New StreamWriter(csvPath, False, encoding)
          writer.WriteLine(String.Join(",", output.Columns.Cast(Of DataColumn)().Select(Function(c) EncloseDoubleQuotes(c.Caption))))
          For Each row As DataRow In output.Rows
            writer.WriteLine(String.Join(",", row.ItemArray.Select(Function(value) EncloseDoubleQuotes(Convert.ToString(value)))))
          Next
        End Using
        Using writer As New StreamWriter(defPath, False, encoding)
          writer.Write(DefText)
        End Using
        GeneratedMasterFiles.Add(csvPath)
        GeneratedMasterFiles.Add(defPath)
        InsertTRNLOG(unitNumber, "", Path.GetFileName(csvPath), "マスタファイル作成完了（" & output.Rows.Count & "件）")
      Next
    End Using
  End Sub
  Private Function EncloseDoubleQuotes(field As String) As String
    Return "" & field & ""
  End Function
  Private Function GetItemMasterSelectSql() As String
    Dim sql As String = String.Empty
    sql &= " SELECT "
    sql &= "     call_code AS 呼出コード, "
    sql &= "     item_number AS 品番, "
    sql &= "     unit_weight AS 単重, "
    sql &= "     unit_weight_unit AS 単重単位, "
    sql &= "     safety_factor AS 安全計数, "
    sql &= "     target_qty AS 目標個数, "
    sql &= "     packing AS 風袋, "
    sql &= "     packing_unit AS 風袋単位, "
    sql &= "     upper_limit AS 上限値, "
    sql &= "     standard_value AS 基準値, "
    sql &= "     lower_limit AS 下限値, "
    sql &= "     subtotal_target_qty AS 小計目標値, "
    sql &= "     subtotal_target_cnt AS 小計目標回数, "
    sql &= "     item_name AS 品名, "
    sql &= "     upper_unit_weight AS 単重上限値, "
    sql &= "     upper_unit_weight_unit AS 単重上限値単位, "
    sql &= "     lower_unit_weight AS 単重下限値, "
    sql &= "     lower_unit_weight_unit AS 単重下限値単位 "
    sql &= " FROM "
    sql &= "     MST_Item "
    Call WriteExecuteLog("Module_Upload", System.Reflection.MethodBase.GetCurrentMethod().Name, sql)
    Return sql
  End Function
  Private Function GetManufacturerMasterSelectSql() As String
    Dim sql As String = String.Empty
    sql &= " SELECT"
    sql &= "     Manufacturer_Code As 製造者コード,"
    sql &= "     Manufacturer_Name As 製造者名"
    sql &= " FROM"
    sql &= "     MST_Manufacturer"
    Call WriteExecuteLog("Module_Upload", System.Reflection.MethodBase.GetCurrentMethod().Name, sql)
    Return sql
  End Function
  Private Function GetStaffMasterSelectSql() As String
    Dim sql As String = String.Empty
    sql &= " SELECT"
    sql &= "     Staff_Number As [担当者№],"
    sql &= "     Staff_Name As [担当者名称]"
    sql &= " FROM"
    sql &= "     MST_Staff"
    Call WriteExecuteLog("Module_Upload", System.Reflection.MethodBase.GetCurrentMethod().Name, sql)
    Return sql
  End Function

  Private Function GetMSTPackingSelectSql() As String
    Dim sql As String = String.Empty
    sql &= " SELECT"
    sql &= "     PackingNo As [風袋№],"
    sql &= "     PackingWeight As [風袋重量],"
    sql &= "     PackingWeightUnit As [風袋重量単位],"
    sql &= "     PackingName As [風袋名称]"
    sql &= " FROM"
    sql &= "     MST_Packing"
    Call WriteExecuteLog("Module_Upload", System.Reflection.MethodBase.GetCurrentMethod().Name, sql)
    Return sql
  End Function
  Private Function GetMSTWorkOrderSelectSql() As String
    Dim sql As String = String.Empty
    sql &= " SELECT"
    sql &= "     WorkOrderID As [作業指示№],"
    sql &= "     DetailID As [明細№],"
    sql &= "     WorkOrderName As [作業指示名称],"
    sql &= "     IsDetailSubtotal As [明細小計(する/しない)],"
    sql &= "     ProductID As [商品№],"
    sql &= "     OrderQuantity As [指示数]"
    sql &= " FROM"
    sql &= "     dbo.MST_WorkOrder"
    Call WriteExecuteLog("Module_Upload", System.Reflection.MethodBase.GetCurrentMethod().Name, sql)
    Return sql
  End Function

  ' フリー1マスター取得
  Private Function GetMST_Free1MasterSelectSql() As String
    Dim sql As String = String.Empty
    sql &= " SELECT"
    sql &= "     free1_number AS [フリー１№],"
    sql &= "     free1_name AS [フリー１名称]"
    sql &= " FROM"
    sql &= "     MST_Free1"
    Call WriteExecuteLog("Module_Upload", System.Reflection.MethodBase.GetCurrentMethod().Name, sql)
    Return sql
  End Function

  ' フリー2マスター取得
  Private Function GetMST_Free2MasterSelectSql() As String
    Dim sql As String = String.Empty
    sql &= " SELECT"
    sql &= "     free2_number AS [フリー２№],"
    sql &= "     free2_name AS [フリー２名称]"
    sql &= " FROM"
    sql &= "     MST_Free2"
    Call WriteExecuteLog("Module_Upload", System.Reflection.MethodBase.GetCurrentMethod().Name, sql)
    Return sql
  End Function

  ' フリー3マスター取得
  Private Function GetMST_Free3MasterSelectSql() As String
    Dim sql As String = String.Empty
    sql &= " SELECT"
    sql &= "     free3_number AS [フリー３№],"
    sql &= "     free3_name AS [フリー３名称]"
    sql &= " FROM"
    sql &= "     MST_Free3"
    Call WriteExecuteLog("Module_Upload", System.Reflection.MethodBase.GetCurrentMethod().Name, sql)
    Return sql
  End Function

  ' フリー4マスター取得
  Private Function GetMST_Free4MasterSelectSql() As String
    Dim sql As String = String.Empty
    sql &= " SELECT"
    sql &= "     free4_number AS [フリー４№],"
    sql &= "     free4_name AS [フリー４名称]"
    sql &= " FROM"
    sql &= "     MST_Free4"
    Call WriteExecuteLog("Module_Upload", System.Reflection.MethodBase.GetCurrentMethod().Name, sql)
    Return sql
  End Function

  ' フリー5マスター取得
  Private Function GetMST_Free5MasterSelectSql() As String
    Dim sql As String = String.Empty
    sql &= " SELECT"
    sql &= "     free5_number AS [フリー５№],"
    sql &= "     free5_name AS [フリー５名称]"
    sql &= " FROM"
    sql &= "     MST_Free5"
    Call WriteExecuteLog("Module_Upload", System.Reflection.MethodBase.GetCurrentMethod().Name, sql)
    Return sql
  End Function
  Private Sub InsertTRNLOG(UNIT_NUMBER As String, RESULT As String, FILE_NAME As String, NOTE As String)
    Dim sql As String = String.Empty
    sql = GetInsertSql(UNIT_NUMBER, RESULT, FILE_NAME, NOTE)
    With tmpDb
      Try
        If .Execute(sql) = 1 Then
          ' 更新成功
          .TrnCommit()
        Else
          ' 削除失敗
          Throw New Exception("ログの作成処理に失敗しました")
        End If
      Catch ex As Exception
        Call ComWriteErrLog("Module_Upload",
                          Reflection.MethodBase.GetCurrentMethod().Name, ex)
        ' ログ登録失敗はファイルログへ記録し、再帰呼出ししない。
      End Try
    End With
  End Sub

  Private Function GetInsertSql(UNIT_NUMBER As String, RESULT As String, FILE_NAME As String, NOTE As String)
    Dim sql As String = String.Empty
    Dim tmpdate As DateTime = CDate(ComGetProcTime())
    Dim PROCESS_DATE As String = tmpdate.ToString("yyyy-MM-dd")
    Dim PROCESS_TIME As String = tmpdate.ToString("HH:mm:ss.ss")
    Dim UPLOAD_TIME As String = tmpdate.ToString("yyyy-MM-dd HH:mm:ss.ss ")

    sql &= " INSERT INTO TRN_LOG("
    sql &= "             PROCESS_DATE,"
    sql &= "             MACHINE_NO,"
    sql &= "             PROCESS_TIME,"
    sql &= "             FILE_NAME,"
    sql &= "             MASTER_SEND_TIME,"
    sql &= "             MASTER_RESULT,"
    sql &= "             NOTE,"
    sql &= "             CREATE_DATE,"
    sql &= "             UPDATE_DATE"
    sql &= " )"
    sql &= " VALUES("
    sql &= "     '" & PROCESS_DATE & "',"
    sql &= "     '" & UNIT_NUMBER & "',"
    sql &= "     '" & PROCESS_TIME & "',"
    sql &= "     '" & FILE_NAME & "',"
    sql &= "     '" & UPLOAD_TIME & "',"
    sql &= "     '" & RESULT & "',"
    sql &= "     '" & NOTE.Replace("'", "") & "',"
    sql &= "     '" & tmpdate & "',"
    sql &= "     '" & tmpdate & "'"
    sql &= " )"
    Call WriteExecuteLog("Module_Upload", System.Reflection.MethodBase.GetCurrentMethod().Name, sql)
    Return sql
  End Function
End Module
