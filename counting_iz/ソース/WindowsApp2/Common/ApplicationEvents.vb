Imports Microsoft.VisualBasic.ApplicationServices

Namespace My
  Partial Friend Class MyApplication
    Private Sub MyApplication_Startup(sender As Object, e As StartupEventArgs) Handles Me.Startup
      Global.Common.ClsFunction.InitializeExceptionLogging()
    End Sub
  End Class
End Namespace
