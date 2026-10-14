Public Class ClsSqlServer

    Inherits ClsComDatabase
    Public Sub New()
        DataSource = ClsGlobalData.DB_DATASOURCE
        DefaultDatabase = ClsGlobalData.DB_DEFAULTDATABASE
        UserId = ClsGlobalData.DB_USERID
        Password = ClsGlobalData.DB_PASSWORD
    End Sub

  ' 接続文字列を返すメソッド（追加）
  Public Function GetConnectionString() As String
    Dim connStr As String = "Data Source=" & ClsGlobalData.DB_DATASOURCE &
                                ";Initial Catalog=" & ClsGlobalData.DB_DEFAULTDATABASE &
                                ";User ID=" & ClsGlobalData.DB_USERID &
                                ";Password=" & ClsGlobalData.DB_PASSWORD
    Return connStr
  End Function

  ''' <summary>
  ''' 実績CSVの全行を1トランザクションで登録する。
  ''' 同一号機・日付・時刻の実績は、再受信データとして登録対象外にする。
  ''' </summary>
  Public Sub InsertResultsIfNotExists(rows As DataTable,
                                      columnNames As String(),
                                      ByRef insertedCount As Integer,
                                      ByRef duplicateCount As Integer)
    If rows Is Nothing Then Throw New ArgumentNullException(NameOf(rows))
    If columnNames Is Nothing OrElse columnNames.Length = 0 Then
      Throw New ArgumentException("実績の列定義がありません。", NameOf(columnNames))
    End If

    Dim requiredColumns As String() = {"addition_date", "addition_time", "terminal_number"}
    For Each requiredColumn As String In requiredColumns
      If Array.IndexOf(columnNames, requiredColumn) < 0 Then
        Throw New ArgumentException("重複判定に必要な列がありません: " & requiredColumn,
                                    NameOf(columnNames))
      End If
    Next

    Dim quotedColumns As New List(Of String)()
    Dim parameterNames As New List(Of String)()
    For index As Integer = 0 To columnNames.Length - 1
      quotedColumns.Add(QuoteIdentifier(columnNames(index)))
      parameterNames.Add("@p" & index.ToString())
    Next

    Dim dateIndex As Integer = Array.IndexOf(columnNames, "addition_date")
    Dim timeIndex As Integer = Array.IndexOf(columnNames, "addition_time")
    Dim terminalIndex As Integer = Array.IndexOf(columnNames, "terminal_number")
    Dim commandText As String =
      "INSERT INTO TRN_Results (" & String.Join(",", quotedColumns) & ") " &
      "SELECT " & String.Join(",", parameterNames) & " " &
      "WHERE NOT EXISTS (" &
      "SELECT 1 FROM TRN_Results WITH (UPDLOCK, HOLDLOCK) " &
      "WHERE terminal_number = @p" & terminalIndex.ToString() & " " &
      "AND addition_date = @p" & dateIndex.ToString() & " " &
      "AND addition_time = @p" & timeIndex.ToString() & ")"

    insertedCount = 0
    duplicateCount = 0
    For Each key In requiredColumns
      If Not rows.Columns.Contains(key) Then
        Throw New ArgumentException("実績データに必要な列がありません: " & key, NameOf(rows))
      End If
    Next
    For Each row As DataRow In rows.Rows
      For Each key In requiredColumns
        If String.IsNullOrWhiteSpace(Convert.ToString(row(key))) Then
          Throw New InvalidOperationException("実績の重複判定項目が空です: " & key)
        End If
      Next
    Next
    If rows.Rows.Count = 0 Then Return
    Using connection As New System.Data.SqlClient.SqlConnection(GetConnectionString())
      connection.Open()
      Using transaction As System.Data.SqlClient.SqlTransaction = connection.BeginTransaction()
        Try
          For Each row As DataRow In rows.Rows
            Using command As New System.Data.SqlClient.SqlCommand(commandText, connection, transaction)
              For index As Integer = 0 To columnNames.Length - 1
                Dim value As String = Convert.ToString(row(columnNames(index)))
                If columnNames(index) Like "free?_number" Then value = "000" & value
                command.Parameters.AddWithValue(parameterNames(index), value)
              Next

              If command.ExecuteNonQuery() = 1 Then
                insertedCount += 1
              Else
                duplicateCount += 1
              End If
            End Using
          Next
          transaction.Commit()
        Catch ex As Exception
          Try
            transaction.Rollback()
          Catch rollbackException As Exception
            ClsFunction.ComWriteErrLog(NameOf(ClsSqlServer),
                                       NameOf(InsertResultsIfNotExists), rollbackException)
          End Try
          insertedCount = 0
          duplicateCount = 0
          ClsFunction.ComWriteErrLog(NameOf(ClsSqlServer),
                                     NameOf(InsertResultsIfNotExists), ex)
          Throw
        End Try
      End Using
    End Using

    ClsFunction.WriteExecuteLog(NameOf(ClsSqlServer), NameOf(InsertResultsIfNotExists),
      "TRN_Results CSV取込: 登録=" & insertedCount.ToString() &
      "件、重複除外=" & duplicateCount.ToString() & "件")
  End Sub

  Private Shared Function QuoteIdentifier(identifier As String) As String
    If String.IsNullOrWhiteSpace(identifier) Then
      Throw New ArgumentException("列名が空です。", NameOf(identifier))
    End If
    For Each character As Char In identifier
      If Not Char.IsLetterOrDigit(character) AndAlso character <> "_"c Then
        Throw New ArgumentException("使用できない列名です: " & identifier, NameOf(identifier))
      End If
    Next
    Return "[" & identifier & "]"
  End Function
End Class
