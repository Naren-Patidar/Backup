Imports System
Imports System.Xml.Serialization

<XmlRoot("DoubleUpResponse", IsNullable:=False)> _
Public Class cDoubleUpResponse

    ' <XmlElement("TransactionID")> _
    'Private _TransactionID As String
    '    <XmlIgnore()> _
    '    Public ReadOnly Property TransactionID() As Nullable(Of Guid)
    '        Get
    '            If _TransactionID = "" Then
    '               Return Nothing
    '           Else
    '              Return New Guid(_TransactionID)
    '         End If
    '    End Get
    'End Property
    Public TransactionID As Guid

    '<XmlElement("TransactionDateTime")> _
    'Private _TransactionDateTime As String
    '<XmlIgnore()> _
    Public TransactionDateTime As DateTime

    '<XmlElement("TransactionResponseCode")> _
    'Private _TransactionResponseCode As String
    '<XmlIgnore()> _
    Public TransactionResponseCode As Integer

    <XmlElement("Response")> _
    Public ResponseNodes As cDoubleUpResponseNode()

    <XmlIgnore()> _
    Public RequestType As Integer   ' passed and set when parsing (the way of knowing whether a token record is a create or cancel) 


    ' convert string (returned from IL) into double up token structure
    Public Shared Function readXML(ByVal response As String, ByVal RequestType As Integer) As cDoubleUpResponse

        Dim responseObj As cDoubleUpResponse
        Dim serializer As New System.Xml.Serialization.XmlSerializer(GetType(cDoubleUpResponse))
        Dim reader As System.IO.StringReader = New System.IO.StringReader(response)

        Try
            responseObj = (CType(serializer.Deserialize(reader), cDoubleUpResponse))

            ' set the request type
            responseObj.RequestType = RequestType

        Catch ex As Exception
            Throw
        End Try

        Return responseObj

    End Function

    Public Sub Save()

        For Each token As cDoubleUpResponseNode In ResponseNodes
            token.Save(TransactionID, TransactionDateTime, RequestType)
        Next

    End Sub

End Class

Public Class cDoubleUpResponseNode

    <XmlElementAttribute("RequestID")> _
    Public _RequestID As String
    <XmlIgnore()> _
    Public ReadOnly Property RequestID() As Nullable(Of Guid)
        Get
            If _RequestID = "" Then
                Return Nothing
            Else
                Return New Guid(_RequestID)
            End If
        End Get
    End Property
    'Public RequestID As Nullable(Of Guid)

    <XmlElement("ResponseCode")> _
    Public _ResponseCode As String
    <XmlIgnore()> _
    Public ReadOnly Property ResponseCode() As Nullable(Of Integer)
        Get
            If _ResponseCode = "" Then
                Return Nothing
            Else
                Return CInt(_ResponseCode)
            End If
        End Get
    End Property
    'Public ResponseCode As Integer

    '<XmlElement("VoucherNo")> _
    'Private _VoucherNo As String
    '<XmlIgnore()> _
    Public VoucherNo As String

    Public AlphaNumericID As String


    Overrides Function ToString() As String

        Return "RequestID :" + Me.RequestID.ToString + vbCrLf + _
                "ResponseCode :" + Me.ResponseCode.ToString + vbCrLf + _
                "VoucherNo :" + Me.VoucherNo + vbCrLf + _
                "AlphaNumericID :" + Me.AlphaNumericID
    End Function


    Public Sub Save(ByVal transactionId As Guid, ByVal TransactionDateTime As DateTime, ByVal RequestType As Integer)

        Call spAppDoubleUpToken(RequestID, ResponseCode, VoucherNo, AlphaNumericID, RequestType)

    End Sub


    ' development routine used to get double up messages
    ' these messages then desirialized for debugging / checking
    Public Shared Function spSelMessage() As String()

        Dim sp As String = "spSelNGCDoubleUpToken"      ' get messages
        Dim dr As System.Data.SqlClient.SqlDataReader
        Dim conn As New System.Data.SqlClient.SqlConnection
        Dim messages As String()

        conn.ConnectionString = cConnection.GetConnection("Freesale")


        Dim Command As New SqlClient.SqlCommand(sp, conn)

        Using (conn)
            Using (Command)
                Command.CommandType = CommandType.StoredProcedure

                Command.Connection.Open()
                dr = Command.ExecuteReader
                Dim i As Integer = 0

                While dr.Read()
                    ReDim Preserve messages(i)
                    messages(i) = dr(0)
                    i += 1
                End While

                ' Call Close when done reading.
                dr.Close()

            End Using
        End Using

        Return messages


    End Function


    Public Shared Sub spAppDoubleUpToken(ByVal RequestID As Guid, _
                                     ByVal ResponseCode As Integer, _
                                     ByVal VoucherNo As String, _
                                     ByVal AlphaNumericID As String, _
                                     ByVal RequestType As Integer)

        'Dim sp As String = "spUpdDoubleUpToken"
        Dim sp As String = "UpdateDoubleUpToken" 'CSD
        Dim ds As New DataSet
        Dim conn As New System.Data.SqlClient.SqlConnection


        conn.ConnectionString = cConnection.GetConnection("Freesale")


        Dim Command As New SqlClient.SqlCommand(sp, conn)

        Dim retryCount As Integer = 2
        Dim successful As Boolean = False

        While retryCount > 0 And successful = False
            Try

                Using (conn)
                    Using (Command)
                        Command.CommandType = CommandType.StoredProcedure
                        Command.Parameters.Add("@RequestId", SqlDbType.UniqueIdentifier).Value = RequestID
                        Command.Parameters.Add("@ResponseCode", SqlDbType.Int).Value = ResponseCode
                        Command.Parameters.Add("@VoucherNo", SqlDbType.VarChar).Value = VoucherNo
                        Command.Parameters.Add("@AlphanumericId", SqlDbType.VarChar).Value = AlphaNumericID
                        Command.Parameters.Add("@RequestType", SqlDbType.Int).Value = RequestType

                        Command.Connection.Open()
                        Command.ExecuteScalar()
                    End Using
                End Using
                successful = True
            Catch ex As SqlClient.SqlException

                ' Check for Deadlock victim if so, retry again after a 50ms delay
                If ex.Number = 1205 Then
                    System.Threading.Thread.Sleep(50)
                    retryCount = retryCount - 1
                Else
                    Throw ex
                End If
            Catch ex As Exception
                Throw ex
            End Try
        End While


        
    End Sub

End Class
