Imports System.Collections.Generic
Imports System.Text
Imports System.Runtime.Serialization
Imports System.Data


'Namespace Tesco.com.IntegrationServices.Messages
<Serializable()> _
Public NotInheritable Class GetUnusedVoucherDtlsRsp
    Private _dsResponse As DataSet

    Public Sub New()
        'Me.New(Nothing, Nothing, Nothing, Nothing)
        Me._dsResponse = dsResponse
    End Sub

    Public Sub New(ByVal dsResponse As DataSet)
        Me._dsResponse = dsResponse
        'Me.New(Nothing, Nothing, Nothing, dsResponse)
    End Sub

    'Public Sub New(ByVal errorLogID As String, ByVal errorStatusCode As String, ByVal errorMessage As String, ByVal dsResponse As DataSet)
    '    MyBase.New(errorLogID, errorStatusCode, errorMessage)
    '    Me._dsResponse = dsResponse
    'End Sub

    Public Property dsResponse() As DataSet
        Get
            Return Me._dsResponse
        End Get
        Set(ByVal value As DataSet)
            Me._dsResponse = value
        End Set
    End Property
End Class
'End Namespace
