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
End Class
