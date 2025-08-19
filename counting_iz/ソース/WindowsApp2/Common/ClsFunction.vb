Imports System.IO
Imports System.Text
Imports System.Windows.Forms
Public Class ClsFunction
  Public Shared DirName As String
  Public Shared LogDir As String
  Public Shared LogFileName As String

  Public Shared ErrorLogPath As String = ReadSettingIniFile("ERROR_LOG_PATH", "VALUE")
  Public Shared SqlLogPath As String = ReadSettingIniFile("SQL_LOG_PATH", "VALUE")
  Public Shared Function DateTypeCheck(Day As String)
    Dim CheckResult As Boolean
    Dim yearPart As String = String.Empty
    Dim monthPart As String = String.Empty
    Dim dayPart As String = String.Empty

    Select Case Day.Length
      Case 6
        yearPart = Day.Substring(0, 2)
        monthPart = Day.Substring(2, 2)
        dayPart = Day.Substring(4, 2)
      Case 8
        yearPart = Day.Substring(0, 4)
        monthPart = Day.Substring(4, 2)
        dayPart = Day.Substring(6, 2)
    End Select

    If Not IsValidDate(yearPart, monthPart, dayPart) Then
      CheckResult = False
    Else
      CheckResult = True
    End If

    Return CheckResult
  End Function

  Public Shared Function IsValidDate(year As String, month As String, day As String) As Boolean
    Dim dt As Date
    If year.Length = 2 Then
      year = "20" & year
    End If

    If Date.TryParseExact($"{year}-{month}-{day}", "yyyy-MM-dd", Nothing, Globalization.DateTimeStyles.None, dt) Then
      Return True
    Else
      Return False
    End If
  End Function

  Public Shared Function DateTxt2DateTxt(Day As String)
    Dim formattedDate As String = String.Empty
    Dim yearPart As String
    Dim monthPart As String
    Dim dayPart As String

    Select Case Day.Length
      Case 6
        yearPart = Day.Substring(0, 2)
        monthPart = Day.Substring(2, 2)
        dayPart = Day.Substring(4, 2)
        formattedDate = $"20{yearPart}/{monthPart}/{dayPart}"
      Case 8
        yearPart = Day.Substring(0, 4)
        monthPart = Day.Substring(4, 2)
        dayPart = Day.Substring(6, 2)
        formattedDate = $"{yearPart}/{monthPart}/{dayPart}"
    End Select

    Return formattedDate
  End Function

  Public Shared Function ComGetProcDay() As String
    Return Date.Parse(ComGetProcTime()).ToString("dd")
  End Function

  Public Shared Function ComGetProcYear() As String
    Return Date.Parse(ComGetProcTime()).ToString("yyyy")
  End Function

  Public Shared Function ComGetProcYearMonth() As String
    Return Date.Parse(ComGetProcTime()).ToString("yyyy/MM")
  End Function

  'エラーログファイルを出力(文字列出力)
  Public Shared Sub ComWriteErrLog(ByVal strMsg As Exception,
                                   Optional ByVal filePath As String = "")
    If filePath = "" Then filePath = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "err.log")
    Call ComWriteLog(strMsg.Message, filePath)
  End Sub

  Public Shared Function ComGetProcTime() As String
    Dim dtNow As Date = Date.Now
    Return dtNow
  End Function

  Public Shared Sub ComWriteLog(ByVal desc As String, ByVal filePath As String)
    On Error Resume Next
    Dim strWr As New System.IO.StreamWriter(filePath, True)
    strWr.WriteLine(desc)
    strWr.Close()
    strWr.Dispose()
    strWr = Nothing
    On Error GoTo 0
  End Sub

  Public Shared Sub WriteDetail(dt As DataTable, dgView As DataGridView,
  Optional ByVal columnCtl As Boolean = False)
    dgView.Rows.Clear()
    Try
      For i As Integer = 0 To dt.Rows.Count - 1
        dgView.Rows.Add()
        For j = 0 To dt.Columns.Count - 1
          'フィールドの取得

          'Dim field = dt.Rows(i).Item(j).ToString()

          Dim field = dt.Rows(i)(j).ToString()

          If field Is DBNull.Value Then
            If columnCtl Then
              dgView.Rows(i).Cells(j + 1).Value = field.ToString()
            Else
              dgView.Rows(i).Cells(j).Value = field.ToString()
            End If
          Else
            If columnCtl Then
              dgView.Rows(i).Cells(j + 1).Value = field
            Else
              dgView.Rows(i).Cells(j).Value = field
            End If
          End If
        Next
      Next
      '書き込むファイルを開く
    Catch ex As Exception
      OutLog(ex.Message, "明細表示処理")
      MsgBox(ex.Message)
    End Try
  End Sub

  Public Shared Sub OutLog(msg As String, dataKind As String)

    Dim sw As New StreamWriter(DirName & LogDir & LogFileName, True, System.Text.Encoding.GetEncoding("shift_jis"))
    Try
      sw.WriteLine(DateTime.Now.ToString & " " & dataKind & "：" & msg)
    Catch ex As Exception
      MessageBox.Show(ex.Message, "ログ出力処理")
    End Try
    sw.Close()

  End Sub

  Public Shared Function CountChar(ByVal s As String, ByVal c As Char) As String
    Return s.Length - s.Replace(c.ToString(), "").Length
  End Function

  Public Shared Sub ClearTextBox(ByVal hParent As Control)
    For Each cControl As Control In hParent.Controls
      If cControl.HasChildren Then
        ClearTextBox(cControl)
      End If
      If TypeOf cControl Is TextBoxBase Then
        cControl.Text = String.Empty
      End If
    Next cControl
  End Sub

  ' ------------------------------------------------------
  ' ini ファイル読み込み
  ' ------------------------------------------------------
  '指定のIniファイルの指定キーの値を取得(文字列)
  ' AUTO版 GetPrivateProfileString
  Private Declare Auto Function GetPrivateProfileString Lib "kernel32" _
      (ByVal lpAppName As String,
          ByVal lpKeyName As String,
              ByVal lpDefault As String,
                  ByVal lpReturnedString As StringBuilder,
                      ByVal nSize As Integer,
                          ByVal lpFileName As String) As Integer

  'プロファイル文字列書込み  
  Private Declare Function WritePrivateProfileString Lib "kernel32" _
      Alias "WritePrivateProfileStringA" _
      (ByVal lpApplicationName As String, ByVal lpKeyName As String,
       ByVal lpString As String, ByVal lpFileName As String) As Long

  ''' <summary>
  ''' iniファイルから取得する
  ''' </summary>
  ''' <param name="lpAppName"></param>
  ''' <param name="lpKeyName"></param>
  ''' <param name="strPath"></param>
  ''' <returns></returns>
  Public Shared Function GetIniString(ByVal lpAppName As String, ByVal lpKeyName As String, strPath As String) As String
    Dim sb As New StringBuilder(256)
    Try
      Call GetPrivateProfileString(lpAppName, lpKeyName, "", sb, Convert.ToUInt32(sb.Capacity), strPath)
      Return sb.ToString
    Catch ex As Exception
      Return sb.ToString
    End Try
  End Function

  ''' <summary>
  ''' iniファイルに書き込む
  ''' </summary>
  ''' <param name="lpAppName"></param>
  ''' <param name="lpKeyName"></param>
  ''' <param name="lpValue"></param>
  ''' <param name="strPath"></param>
  ''' <returns></returns>
  Public Shared Function PutIniString(ByVal lpAppName As String, lpKeyName As String, ByVal lpValue As String, ByVal strPath As String) As Boolean
    Try
      Dim result As Long = WritePrivateProfileString(lpAppName, lpKeyName, lpValue, strPath)
      Return result <> 0
    Catch ex As Exception
      Return False
    End Try
  End Function

  Public Shared Sub OpenForm(StrKey As String)
    Dim exePath As String = ReadMenuIniFile(StrKey)
    Dim psi As New ProcessStartInfo(exePath)
    System.Diagnostics.Process.Start(psi)
  End Sub
  Public Shared Function ReadMenuIniFile(strKey As String)
    Dim strPath As String = "C:\COUNTING_DX\INI\menu.ini"
    Dim exeName As String = GetIniString(strKey, "EXE", strPath)
    Return exeName
  End Function

  Public Shared Function ReadSettingIniFile(strKey As String, keyName As String)
    Dim strPath As String = "C:\COUNTING_DX\INI\setting.ini"
    Dim stringValue As String = GetIniString(strKey, keyName, strPath)
    Return stringValue
  End Function

  Public Shared Function ReadConnectIniFile(strKey As String, keyName As String)
    Dim strPath As String = "C:\COUNTING_DX\INI\connect.ini"
    Dim stringValue As String = GetIniString(strKey, keyName, strPath)
    Return stringValue
  End Function

  'エラーログファイルを出力
  Public Shared Sub ComWriteErrLog(ByVal prmSource As String,
                                 ByVal prmTargetSite As String,
                                 ByVal prmMessage As String)

    Dim tmpExeFileName As String = System.IO.Path.GetFileName(System.Windows.Forms.Application.ExecutablePath)

    Call ComWriteLog("【Date】" & DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") _
                   & "【Exe】" & tmpExeFileName _
                   & "【Source】" & prmSource _
                   & "【Method】" & prmTargetSite _
                   & "【Description】" & prmMessage, ErrorLogPath)
  End Sub
  ''' <summary>
  ''' SQL実行ログ保存
  ''' </summary>
  ''' <param name="prmSql">SQL文</param>
  Public Shared Sub WriteExecuteLog(ByVal prmSource As String, ByVal prmTargetSite As String, prmSql As String)
    Try
      Dim tmpProcTime As String = ComGetProcTime()
      Dim tmpExeFileName As String = System.IO.Path.GetFileName(System.Windows.Forms.Application.ExecutablePath)
      Dim logText As String = "【Date】" & tmpProcTime _
                            & "【PC】" & System.Net.Dns.GetHostName() _
                            & "【Exe】" & tmpExeFileName _
                            & "【Source】" & prmSource _
                            & "【Method】" & prmTargetSite _
                            & "【Sql】" & prmSql
      ComWriteLog(logText, SqlLogPath)
    Catch ex As Exception
    End Try
  End Sub
End Class
