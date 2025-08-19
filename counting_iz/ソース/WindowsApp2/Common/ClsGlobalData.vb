Imports Common.ClsFunction
Public Class ClsGlobalData
    'Public Shared ReadOnly DB_DATASOURCE As String = "NN2205001\SQLEXPRESS"
    ''Public Shared ReadOnly DB_DATASOURCE As String = "(local)\SANPAIDX"
    'Public Shared ReadOnly DB_DEFAULTDATABASE As String = "SANPAI"
    'Public Shared ReadOnly DB_USERID As String = "sa"
    'Public Shared ReadOnly DB_PASSWORD As String = "495344"
    Public Shared ReadOnly DB_DATASOURCE As String = ReadConnectIniFile("DB_DATASOURCE", "VALUE")
    Public Shared ReadOnly DB_DEFAULTDATABASE As String = ReadConnectIniFile("DB_DEFAULTDATABASE", "VALUE")
    Public Shared ReadOnly DB_USERID As String = ReadConnectIniFile("DB_USERID", "VALUE")
    Public Shared ReadOnly DB_PASSWORD As String = ReadConnectIniFile("DB_PASSWORD", "VALUE")
End Class
