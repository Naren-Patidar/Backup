Imports Microsoft.Win32
Imports System.Configuration

Public Class cConnection
    ' get the database connection
    Public Shared Function GetConnection(ByVal strConnection As String) As String
        'Dim strURL As String
        'Try
        '    Dim objFreetimeRegistry As RegistryKey = Registry.LocalMachine.OpenSubKey("Software").OpenSubKey("Freetime").OpenSubKey("Database")
        '    strURL = CType(objFreetimeRegistry.GetValue(strConnection), String)
        '    objFreetimeRegistry.Close()
        'Catch ex As Exception
        '    Throw ex
        'End Try

        'Return strURL


        ' just the one connection - allows freetime database names to be left ie no change
        Return ConfigurationManager.ConnectionStrings("InstoreRewardNGC").ConnectionString

    End Function 'GetConnection


End Class