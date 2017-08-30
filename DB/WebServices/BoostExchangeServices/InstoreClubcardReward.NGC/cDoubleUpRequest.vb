Imports System
Imports System.Xml.Serialization

<XmlRoot("DoubleUpRequest", IsNullable:=False)> _
Public Class cDoubleUpRequest

    Private Const REQUEST_SOURCE As String = "Tesco.com Shop"   ' "Freetime" 
    Public TransactionID As Guid
    Public TransactionDateTime As Date
    Public Source As String

    <XmlElement("Request")> _
    Public RequestNodes As cDoubleUpRequestNode()
    Sub New() 'SmartVoucherRequestWrapper
        TransactionID = Guid.NewGuid()
        TransactionDateTime = DateTime.Now
        Source = REQUEST_SOURCE

    End Sub

    ' adjust the message depending on the country
    Sub New(ByVal country As String)
        Me.New()
        ' switch source for ROI
        If country = "ROI" Then
            Me.Source = "Tesco.ie Shop"
        End If
    End Sub


    ' make the request to the web service and return the string
    Public Function callService(ByVal agentId As Integer, ByVal country As String) As String



        Dim serializer As New System.Xml.Serialization.XmlSerializer(GetType(cDoubleUpRequest))
        ' serialize to file 
        'Dim writer As New System.IO.StreamWriter("c:\temp\doublupMessage.xml")
        'serializer.Serialize(writer, Me)
        'writer.Close()
        ' end serialize to file

        ' serialize to string (reuse serializer defined above)
        Dim sr As New System.IO.StringWriter
        serializer.Serialize(sr, Me)
        sr.Close()
        Dim sb As New System.Text.StringBuilder
        sb = sr.GetStringBuilder
        Dim xmlstring As String = sb.ToString
        ' end serialize to string

        ' object to make the call
        Dim callWS As New cDoubleUpWS


        Dim response As String


        Try
            ' save to database
            Me.Save()
            ' make the call to the web service
            response = callWS.callService(xmlstring, agentId, country)
        Catch ex As Exception
            Throw
        End Try

        Return response

    End Function


    Public Sub Save()

        For Each token As cDoubleUpRequestNode In RequestNodes
            token.Save(TransactionID, TransactionDateTime)
        Next

    End Sub

End Class


Public Class cDoubleUpRequestNode

    Public RequestID As Guid
    Public RequestType As Integer
    Public AlphaNumericID As String = ""
    Public VoucherNo As String = ""
    Public Value As String = ""
    Public VendorCode As Integer
    Public ClubcardNo As String = ""
    Public ExpiryDate As String = ""
    <XmlIgnore()> _
    Private _dExpiryDate As Nullable(Of Date)
    <XmlIgnore()> _
    Public ReadOnly Property dExpiryDate() As Nullable(Of Date)
        Get
            If ExpiryDate = "" Then
                _dExpiryDate = Nothing
            Else
                _dExpiryDate = CDate(ExpiryDate)
            End If
            Return _dExpiryDate
        End Get
    End Property

    Sub New()
        RequestID = Guid.NewGuid()
    End Sub

    Public Sub Save(ByVal transactionId As Guid, ByVal TransactionDateTime As Date)

        Call spAppDoubleUpToken(transactionId, _
                                TransactionDateTime, _
                                RequestID, _
                                RequestType, _
                                AlphaNumericID, _
                                VoucherNo, _
                                Value, _
                                VendorCode, _
                                ClubcardNo, _
                                dExpiryDate)

    End Sub


    Public Shared Sub spAppDoubleUpToken(ByVal lTransactionId As Guid, _
                                 ByVal lTransactionDateTime As Date, _
                                 ByVal lRequestID As Guid, _
                                 ByVal lRequestType As Integer, _
                                 ByVal lAlphaNumericID As String, _
                                 ByVal lVoucherNo As String, _
                                 ByVal lValue As String, _
                                 ByVal lVendorCode As Integer, _
                                 ByVal lClubcardNo As String, _
                                 ByVal lExpiryDate As Nullable(Of Date))

        'Dim sp As String = "spAppDoubleUpToken"
        Dim sp As String = "InsertDoubleUpToken"     'CSD
        Dim ds As New DataSet
        Dim conn As New System.Data.SqlClient.SqlConnection


        conn.ConnectionString = cConnection.GetConnection("Freesale")


        Dim Command As New SqlClient.SqlCommand(sp, conn)

        Using (conn)
            Using (Command)
                With Command
                    .CommandType = CommandType.StoredProcedure
                    .Parameters.Add("@TransactionId", SqlDbType.UniqueIdentifier).Value = lTransactionId
                    .Parameters.Add("@TransactionDateTime", SqlDbType.DateTime).Value = lTransactionDateTime
                    .Parameters.Add("@RequestId", SqlDbType.UniqueIdentifier).Value = lRequestID
                    .Parameters.Add("@RequestType", SqlDbType.Int).Value = lRequestType
                    .Parameters.Add("@AlphanumericId", SqlDbType.VarChar).Value = lAlphaNumericID
                    .Parameters.Add("@VoucherNo", SqlDbType.VarChar).Value = lVoucherNo

                    ' change of parameter nasme for CSD
                    '.Parameters.Add("@Value", SqlDbType.VarChar).Value = CInt(lValue)
                    .Parameters.Add("@VoucherValue", SqlDbType.VarChar).Value = CInt(lValue)

                    ' for some fields only set a value if a valid value
                    .Parameters.Add("@VendorCode", SqlDbType.Int)
                    If lVendorCode = 0 Then
                        .Parameters("@VendorCode").Value = Nothing
                    Else
                        .Parameters("@VendorCode").Value = CInt(lVendorCode)
                    End If
                    .Parameters.Add("@ClubcardNo", SqlDbType.VarChar)
                    If lClubcardNo = "" Then
                        .Parameters("@ClubcardNo").Value = Nothing
                    Else
                        .Parameters("@ClubcardNo").Value = lClubcardNo
                    End If
                    .Parameters.Add("@ExpiryDate", SqlDbType.DateTime)
                    If lExpiryDate Is Nothing Then
                        .Parameters("@ExpiryDate").Value = Nothing
                    Else
                        .Parameters("@ExpiryDate").Value = lExpiryDate
                    End If
                    'CSD @Id is an output
                    Command.Parameters.Add("@Id", SqlDbType.Int)
                    Command.Parameters.Item("@Id").Direction = ParameterDirection.Output

                    .Connection.Open()
                    .ExecuteScalar()
                End With
            End Using
        End Using

    End Sub


    Overrides Function ToString() As String

        Return "RequestID :" + Me.RequestID.ToString + vbCrLf + _
                "RequestType :" + Me.RequestType.ToString + vbCrLf + _
                "AlphaNumericID :" + Me.AlphaNumericID + vbCrLf + _
                "VoucherNo :" + Me.VoucherNo + vbCrLf + _
                "Value :" + Me.Value.ToString + vbCrLf + _
                "VendorCode :" + Me.VendorCode + vbCrLf + _
                "ClubcardNo :" + Me.ClubcardNo + vbCrLf + _
                "ExpiryDate :" + Me.ExpiryDate.ToString
    End Function

End Class



Public Enum eDoubleUpMessageRequestType As Integer
    GenerateToken = 1
    CancelToken = 2
End Enum


Public Enum eDoubleUpMessageResponseCode As Integer
    Success = 0
    ConfigurationReaderError = 101
    SystemGeneratedError = 102
    SVNumberNotAvailable = 103          ' IL, 11 Digit SV number and 12 character Alpha number not available
    InvalidXMLFreetime = 104            ' IL, Invalid XML message received from FreeTime
    InvalidXMLSV = 105                  ' IL, Invalid XML message received from Smart Voucher

End Enum

Public Enum eDoubleUpResponseCode As Integer

    RandomMissing = 21      'random number not specified or incorrect in the input xml for issue
    AlphaMissing = 22       'Alpha code not specified or incorrect in the input xml
    ValueMissing = 23       'Value not specified in the input xml
    VendorMissing = 24      'Vendor code not specified in the input xml
    ChannelMissing = 25     'Channel code not specified in the input xml
    ClubcardNotFound = 27   'Clubcard number not found in smart vouchers
    DuffCheckDigit = 28     'Cannot calculate Check Digit, please try again
    RecordExists = 29       'Record already exists in SV, please try again
    DuplicateAlpha = 30     'Trying to duplicate Alpha code, please try again
    NoRandomNumber = 31     'random number not specified or incorrect in the input xml for cancellation
    NotActive = 32          'Voucher not in active status
    NotFound = 33           'Could not find the voucher specified, please check and retry
    NotDoubleUp = 34        'the attemped voucher is not of type double up

End Enum