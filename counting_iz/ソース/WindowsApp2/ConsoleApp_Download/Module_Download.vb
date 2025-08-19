Imports System.IO
Imports System.Text
Imports System.Windows.Forms
Imports Common
Imports Common.ClsFunction
Imports Microsoft.VisualBasic.FileIO
Module Module_Download
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
  ReadOnly FtpDownloadPath As String = ReadSettingIniFile("FTP_DOWNLOAD_PATH", "VALUE")
  ReadOnly FtpBackupPath As String = ReadSettingIniFile("FTP_BACKUP_PATH", "VALUE")
  ReadOnly FtpDelete As String = ReadSettingIniFile("FTP_DELETE", "VALUE")

  ReadOnly FileNameDigits As String = ReadSettingIniFile("FILENAME_DIGITS", "VALUE")
  ReadOnly CutFileNameDigits As String = ReadSettingIniFile("CUT_FILENAME_DIGITS", "VALUE")

  Dim UnitNumberString As String
  Dim IpAddressString As String
  Dim UnitNumberArray() As String
  Dim IpAddressArray() As String
  Dim ErrorJudFlg As Boolean = False

  Sub Main(ScaleNumber() As String)
    Dim Para_ScaleNumber As String = String.Empty
    If ScaleNumber.Length > 0 Then
      For i As Integer = 0 To ScaleNumber.Length - 1
        If i = 0 Then
          Para_ScaleNumber = ScaleNumber(i)
        Else
          Para_ScaleNumber = Para_ScaleNumber + "," + ScaleNumber(i)
        End If
      Next
    End If

    '「計量器マスタ」取得
    SelectScaleMaster(Para_ScaleNumber)
    '引数定義
    Dim dtNow As DateTime = DateTime.Now
    Dim DownloadPath As String
    Dim BackupPath As String
    Dim UpLoadFile As String
    Dim URL As String

    '計量器毎にループ（実績）
    For j As Integer = 0 To UnitNumberArray.Length - 1
      DownloadPath = FtpDownloadPath & "/40TRAN" & FileNameDigits & UnitNumberArray(j) & ".CSV"
      BackupPath = FtpBackupPath & "/40TRAN" & FileNameDigits & UnitNumberArray(j) & "_" & dtNow.ToString("yyMMddHHmmss") & ".CSV"
      URL = "ftp://" & IpAddressArray(j) & "/40TRAN" & FileNameDigits & UnitNumberArray(j) & ".CSV"
      DownloadFtp(DownloadPath, BackupPath, URL, UnitNumberArray(j))
      System.Threading.Thread.Sleep(2000)
      'ログ登録　＆ 削除ファイル送信
      If ErrorJudFlg Then
        InsertTRNLOG(UnitNumberArray(j), "NG", "", "実績受信失敗")

      Else
        InsertTRNLOG(UnitNumberArray(j), "OK", "", "実績受信成功")

        UpLoadFile = FtpDelete & "/40TRAN" & FileNameDigits & UnitNumberArray(j) & ".DEL"
        URL = "ftp://" & IpAddressArray(j) & "/40TRAN" & FileNameDigits & UnitNumberArray(j) & ".DEL"
        DelUploadFtp(UpLoadFile, URL, UnitNumberArray(j))
        System.Threading.Thread.Sleep(2000)
      End If
    Next
  End Sub
  Private Sub DownloadFtp(DownloadPath As String, BackupPath As String, URL As String, UnitNumber As String)
    Dim FILE_NAME As String = Strings.Right(URL, CutFileNameDigits)
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
      Dim ftpRes As System.Net.FtpWebResponse =
          CType(ftpReq.GetResponse(), System.Net.FtpWebResponse)
      'ファイルをダウンロードするためのStreamを取得
      Dim resStrm As System.IO.Stream = ftpRes.GetResponseStream()

      InsertTRNLOG(UnitNumber, "", FILE_NAME, "FTPログイン成功")
      'ダウンロードしたファイルを書き込むためのFileStreamを作成
      Dim fs As New System.IO.FileStream(
          DownloadPath, System.IO.FileMode.Create, System.IO.FileAccess.Write)
      'ダウンロードしたデータを書き込む
      Dim buffer(1023) As Byte
      While True
        Dim readSize As Integer = resStrm.Read(buffer, 0, buffer.Length)
        If readSize = 0 Then
          Exit While
        End If
        fs.Write(buffer, 0, readSize)
      End While
      fs.Close()
      resStrm.Close()

      ftpRes.Close()


      InsertTRNLOG(UnitNumber, "", FILE_NAME, "FTP処理終了")

      'ダウンロード実績ファイルコピー
      MoveToBackUpLoadFile(DownloadPath, BackupPath)

      '実績CSVを実績テーブルに登録
      InsertResultTable(DownloadPath, UnitNumber)

      'System.IO.File.Delete(DownloadPath)

      InsertTRNLOG(UnitNumber, "", FILE_NAME, ftpRes.StatusCode & " " & ftpRes.StatusDescription)
      ErrorJudFlg = False
    Catch ex As Exception
      InsertTRNLOG(UnitNumber, "", FILE_NAME, ex.Message)
      ErrorJudFlg = True

    End Try
  End Sub

  Private Sub DelUploadFtp(UpLoadFile As String, URL As String, UnitNumber As String)
    Dim FILE_NAME As String = Strings.Right(URL, CutFileNameDigits)
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
      Dim reqStrm As System.IO.Stream = ftpReq.GetRequestStream()
      'アップロードするファイルを開く
      Dim fs As New System.IO.FileStream(UpLoadFile, System.IO.FileMode.Open, System.IO.FileAccess.Read)
      'アップロードStreamに書き込む
      Dim buffer(1023) As Byte
      While True
        Dim readSize As Integer = fs.Read(buffer, 0, buffer.Length)
        If readSize = 0 Then
          Exit While
        End If
        reqStrm.Write(buffer, 0, readSize)
      End While
      fs.Close()
      reqStrm.Close()

      'FtpWebResponseを取得
      Dim ftpRes As System.Net.FtpWebResponse = CType(ftpReq.GetResponse(), System.Net.FtpWebResponse)
      '閉じる
      ftpRes.Close()
      InsertTRNLOG(UnitNumber, "", FILE_NAME, ftpRes.StatusCode & " " & ftpRes.StatusDescription)
    Catch ex As Exception
      '正常処理終了フラグ変更★★★
      Call ComWriteErrLog("Module_Download",
                        System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message)
      InsertTRNLOG(UnitNumber, "", FILE_NAME, ex.Message)
    End Try
  End Sub
  Public Sub SelectScaleMaster(Para_ScaleNumber As String)
    Dim sql As String = String.Empty
    sql = GetMstScaleSelectSql(Para_ScaleNumber)
    Try
      With tmpDb
        SqlServer.GetResult(tmpDt, sql)
        If tmpDt.Rows.Count = 0 Then
          MessageBox.Show("計量器マスタにデータが登録されていません。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
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
      Call ComWriteErrLog("Module_Download",
                        System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message)
      Throw New Exception(ex.Message)
      InsertTRNLOG("", "", "", "計量器マスタ取得失敗")
    Finally
      tmpDt.Dispose()
    End Try
  End Sub
  Private Function GetMstScaleSelectSql(Para_ScaleNumber As String) As String
    Dim sql As String = String.Empty
    sql &= " SELECT"
    sql &= "     UNIT_NUMBER,"
    sql &= "     IP_ADDRESS"
    sql &= " FROM"
    sql &= "     MST_SCALE"
    sql &= " WHERE"
    sql &= "     1=1"
    If Para_ScaleNumber.Length <> 0 Then
      sql &= "     AND UNIT_NUMBER IN(" & Para_ScaleNumber & ")"
    End If
    sql &= " ORDER BY  "
    sql &= "     UNIT_NUMBER"
    Call WriteExecuteLog("Module_Download", System.Reflection.MethodBase.GetCurrentMethod().Name, sql)
    Return sql
  End Function
  Private Sub InsertResultTable(DownloadPath As String, UnitNumber As String)
    Dim dt As New DataTable("TRN_Results")

    ' データベースのカラム名を定義
    Dim columnNames() As String = {"addition_date", "addition_time", "terminal_number", "call_code", "item_number", "item_name",
                                   "unit_weight", "unit_weight_unit", "quantity", "packing", "packing_unit", "weight", "weight_unit",
                                   "manufacturer_code", "manufacturer_name", "lot1", "lot2", "classification", "upper_weight_limit",
                                   "upper_weight_unit", "lower_weight_limit", "lower_weight_unit", "packing1_weight", "packing1_weight_unit",
                                   "packing2_weight", "packing2_weight_unit", "packing2_multiplication", "packing1_number", "packing1_name",
                                   "packing2_number", "packing2_name", "staff_number", "staff_name", "free1_number", "free1_name",
                                   "free2_number", "free2_name", "free3_number", "free3_name", "free4_number", "free4_name",
                                   "free5_number", "free5_name", "processing_date", "processing_time", "effective_date",
                                   "effective_time", "work_order_number", "detail_number", "instruction_qty", "actual_qty",
                                   "work_order_name", "create_date", "update_date"}

    ' データテーブルにカラムを追加
    For Each columnName As String In columnNames
      dt.Columns.Add(New DataColumn(columnName, GetType(String)))
    Next

    ' CSVファイルからデータを読み込み
    Using textParser As New TextFieldParser(DownloadPath, System.Text.Encoding.GetEncoding("Shift-JIS"))
      textParser.TextFieldType = FieldType.Delimited
      textParser.SetDelimiters(",")

      ' ヘッダー行を読み飛ばす
      If Not textParser.EndOfData Then
        textParser.ReadFields()
      End If

      While Not textParser.EndOfData
        Dim row As String() = textParser.ReadFields()
        Dim dr As DataRow = dt.NewRow()

        For i As Integer = 0 To row.Length - 1
          dr(i) = row(i)
        Next

        ' 実行時の時刻を追加
        dr("create_date") = DateTime.Now.ToString("yyyy-MM-dd")
        dr("update_date") = DateTime.Now.ToString("yyyy-MM-dd")

        dt.Rows.Add(dr)
      End While
    End Using

    ' データベースに挿入
    Dim sql As String = String.Empty
    For Each dr As DataRow In dt.Rows
      sql = GetInsertSql(columnNames, dr)
      With tmpDb
        Try
          If .Execute(sql) = 1 Then
            ' 更新成功
            .TrnCommit()
            InsertTRNLOG(UnitNumber, "", "", "実績登録完了")
          Else
            ' 削除失敗
            Throw New Exception("実績管理の登録処理に失敗しました。")
          End If
        Catch ex As Exception
          Call ComWriteErrLog("Module_Download",
                System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message)
          InsertTRNLOG(UnitNumber, "", "", ex.Message)
        End Try
      End With
    Next

    Dim message As String
    If dt.Rows.Count = 0 Then
      message = "現在取り込み可能なデータがありません。（0件）"
      MessageBox.Show(message, "情報", MessageBoxButtons.OK, MessageBoxIcon.Information)
    Else
      message = "現在取り込み中です。" & vbCrLf & "現在の取り込み件数：" & (dt.Rows.Count).ToString() & " 件"
      MessageBox.Show(message, "情報", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End If

  End Sub

  Function GetInsertSql(columnNames() As String, dr As DataRow) As String
    Dim sql As String = "INSERT INTO TRN_Results ("
    Dim values As String = "VALUES ("

    ' カラム名と値をセット
    For i As Integer = 0 To columnNames.Length - 1

      Dim value As String = dr(i).ToString().Replace("'", "''")

      ' freeX_numberの場合は先頭に000を付与
      If columnNames(i) Like "free?_number" Then
        value = "000" & value
      End If

      sql &= columnNames(i)
      values &= "'" & value & "'"

      If i < columnNames.Length - 1 Then
        sql &= ", "
        values &= ", "
      End If
    Next

    sql &= ") " & values & ")"
    Call WriteExecuteLog("Module_Download", System.Reflection.MethodBase.GetCurrentMethod().Name, sql)
    Return sql
  End Function

  Private Sub MoveToBackUpLoadFile(DownloadPath As String, BackupPath As String)
    System.IO.File.Copy(DownloadPath, BackupPath)
  End Sub
  Private Sub InsertTRNLOG(UNIT_NUMBER As String, RESULT As String, FILE_NAME As String, NOTE As String)
    Dim sql As String = String.Empty
    sql = GetInsertTRNLOGSql(UNIT_NUMBER, RESULT, FILE_NAME, NOTE)
    With tmpDb
      Try
        If .Execute(sql) = 1 Then
          ' 更新成功
          .TrnCommit()
        Else
          ' 削除失敗
          Throw New Exception("ログの登録処理に失敗しました。")
        End If
      Catch ex As Exception
        Call ComWriteErrLog("Module_Download",
                      System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message)
        InsertTRNLOG("", "", "", ex.Message & " " & "(ログ登録失敗)")
      End Try
    End With
  End Sub
  Private Function GetInsertTRNLOGSql(UNIT_NUMBER As String, RESULT As String, FILE_NAME As String, NOTE As String)
    Dim sql As String = String.Empty
    Dim tmpdate As DateTime = CDate(ComGetProcTime())
    Dim PROCESS_DATE As String = tmpdate.ToString("yyyy-MM-dd")
    Dim PROCESS_TIME As String = tmpdate.ToString("HH:mm:ss.ss")
    Dim ACHIEVEMENT_RECEIVE_TIME As String = tmpdate.ToString("yyyy-MM-dd HH:mm:ss.ss")

    sql &= " INSERT INTO TRN_LOG("
    sql &= "             PROCESS_DATE,"
    sql &= "             MACHINE_NO,"
    sql &= "             PROCESS_TIME,"
    sql &= "             FILE_NAME,"
    sql &= "             ACHIEVEMENT_RECEIVE_TIME,"
    sql &= "             ACHIEVEMENT_RESULT,"
    sql &= "             NOTE,"
    sql &= "             CREATE_DATE,"
    sql &= "             UPDATE_DATE"
    sql &= " )"
    sql &= " VALUES("
    sql &= "     '" & PROCESS_DATE & "',"
    sql &= "     '" & UNIT_NUMBER & "',"
    sql &= "     '" & PROCESS_TIME & "',"
    sql &= "     '" & FILE_NAME & "',"
    sql &= "     '" & ACHIEVEMENT_RECEIVE_TIME & "',"
    sql &= "     '" & RESULT & "',"
    sql &= "     '" & NOTE.Replace("'", "") & "',"
    sql &= "     '" & tmpdate & "',"
    sql &= "     '" & tmpdate & "'"
    sql &= " )"
    Call WriteExecuteLog("Module_Download", System.Reflection.MethodBase.GetCurrentMethod().Name, sql)
    Return sql
  End Function

End Module
