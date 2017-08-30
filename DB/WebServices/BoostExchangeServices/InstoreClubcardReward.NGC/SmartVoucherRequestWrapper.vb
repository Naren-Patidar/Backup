Imports System
Imports System.Xml.Serialization


Namespace Freetime.AuthorisationGatewayAdapter


    Public Structure ISmartVoucherRequest
        'Public RequestID As String
        'Public RequestType As Integer
        Public AlphaNumericID As String
        Public Ean As String
        Public ClubcardNumber As String
        Public DeliveryPostcode As String
        Public OrderNumber As Integer
        Public Channel As String        ' not constant 
    End Structure


    <XmlRoot("DotComSmartVoucherRequest", IsNullable:=False)> _
   Public Class SmartVoucherRequestWrapper

        Private Const REQUEST_SOURCE As String = "Tesco.com Shop"   ' "Freetime" 
        Public TransactionID As String
        Public TransactionDateTime As DateTime
        Public Source As String

        <XmlElement("Request")> _
        Public RequestNodes As SmartVoucherRequestWrapperNode()
        Sub New() 'SmartVoucherRequestWrapper
            TransactionID = Guid.NewGuid().ToString()
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
        ' To use create an array of request nodes then pass
        ' create the smartvoucherrequest
        Sub New(ByVal requestArray As ISmartVoucherRequest(), ByVal requestType As SmartVoucherServiceRequestType, ByVal country As String) 'SmartVoucherRequestWrapper( requestArray as ISmartVoucherRequest[],  requestType as SmartVoucherServiceRequestType) : this()
            Me.New(country)
            ReDim Me.RequestNodes(requestArray.Length - 1)
            Dim i As Integer
            For i = 0 To requestArray.Length - 1
                RequestNodes(i) = New SmartVoucherRequestWrapperNode(requestArray(i), requestType)
            Next
        End Sub


        ' make the request to the web service and return the string
        Public Function callVoucherRequest(ByVal agentId As Integer, ByVal country As String) As String

            ' keep a record of the vouchers being sent to NGC
            Call saveVoucherRequests()

            Dim serializer As New System.Xml.Serialization.XmlSerializer(GetType(Freetime.AuthorisationGatewayAdapter.SmartVoucherRequestWrapper))
            ' serialize to file 
            'Dim writer As New System.IO.StreamWriter("c:\temp\voucherMessage.xml")
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

            ' redeem vouchers
            Dim redeemVouchers As New callAuthorise


            Dim response As String
            Try
                response = redeemVouchers.cAuthorise(xmlstring, agentId, country)
            Catch ex As Exception
                Throw ex
            End Try

            Return response

        End Function



        Private Sub saveVoucherRequests()

            'Dim sp As String = "spAppVoucherRequest"
            Dim sp As String = "InsertVoucherRequest" 'CSD


            Try

                ' set up connection parameters for saving vouchers
                Dim conn As New System.Data.SqlClient.SqlConnection(cConnection.GetConnection("Freesale"))
                Dim Command As New SqlClient.SqlCommand(sp, conn)
                Command.CommandType = CommandType.StoredProcedure
                Command.Parameters.Add("@RequestType", SqlDbType.VarChar)
                Command.Parameters.Add("@EAN", SqlDbType.VarChar)
                Command.Parameters.Add("@Alpha", SqlDbType.VarChar)

                ' for CSD @VRId is an output
                Command.Parameters.Add("@VRId", SqlDbType.Int)
                Command.Parameters.Item("@VRId").Direction = ParameterDirection.Output


                ' loop through requests and record the vouchers
                For Each rn As SmartVoucherRequestWrapperNode In Me.RequestNodes
                    Command.Parameters.Item("@RequestType").Value = rn.RequestType
                    Command.Parameters.Item("@EAN").Value = rn.VoucherNo
                    Command.Parameters.Item("@Alpha").Value = rn.AlphaNumericID

                    ' open , execute and close
                    Command.Connection.Open()
                    Command.ExecuteScalar()
                    Command.Connection.Close()

                Next

            Catch ex As Exception
                Throw ex

            End Try

        End Sub 'recordVoucherRequest
    End Class



    Public Class SmartVoucherRequestWrapperNode

        Public RequestID As String
        Public RequestType As Integer
        Public AlphaNumericID As String
        Public VoucherNo As String
        Public ClubcardNo As String
        Public DeliveryPostcode As String
        Public OrderNo As Integer
        Public StoreNo As Integer
        Public Channel As String
        Public VirtualStore As String

        Private Const REQUEST_STORENO As String = "01955" ' "2643"
        Private Const REQUEST_CHANNEL As String = "Website" ' "GHS"
        Private Const REQUEST_VIRTUALSTORE As String = "Freetime" '"99"


        Sub New() 'SmartVoucherRequestWrapperNode()
            RequestID = Guid.NewGuid().ToString()
            StoreNo = REQUEST_STORENO
            VirtualStore = REQUEST_VIRTUALSTORE
        End Sub


        Sub New(ByVal voucherRequest As ISmartVoucherRequest, ByVal requestType As SmartVoucherServiceRequestType) 'SmartVoucherRequestWrapperNode( voucherRequest as ISmartVoucherRequest,  requestType as SmartVoucherServiceRequestType) : this()
            Me.new()
            Me.RequestType = CType(requestType, Integer)
            AlphaNumericID = voucherRequest.AlphaNumericID
            VoucherNo = IIf(voucherRequest.Ean = Nothing, String.Empty, voucherRequest.Ean)
            ClubcardNo = IIf(voucherRequest.ClubcardNumber = Nothing, String.Empty, voucherRequest.ClubcardNumber)
            DeliveryPostcode = String.Empty
            OrderNo = voucherRequest.OrderNumber
            Channel = voucherRequest.channel
        End Sub

        Shared Function requestchannel(ByVal channel As Integer) As String

            Select Case channel
                Case 1
                    Return "store"
                Case 2
                    Return "in store exchange"  ' new channel SR 23/7/10
                Case 6
                    Return "online entertainment"
                Case 7
                    Return "online Deals"
                Case 10
                    Return "GHS"
                Case 11
                    Return "online books"
                Case 12
                    Return "online flowers"
                Case 13
                    Return "online wine"
                Case 20
                    Return "Postal Deals"
                Case 30
                    Return "phone Deals"
                Case 40
                    Return "Air Miles"
                Case 41
                    Return "BA miles"
                Case 50
                    Return "TPF insurance"
                Case Else
                    Return ""
            End Select
        End Function
    End Class


    Public Enum SmartVoucherServiceRequestType As Integer
        Validation = 10
        Reservation = 11
        Redemption = 12
        Unreservation = 13
    End Enum


    Public Enum RequestChannel As Integer

        store = 1
        instoreexchange = 2     ' new channel SR 23/7/10
        onlineentertainment = 6
        onlineDeals = 7
        GHS = 10
        onlinebooks = 11
        onlineflowers = 12
        onlinewine = 13
        PostalDeals = 20
        PhoneDeals = 30
        AirMiles = 40
        BAmiles = 41
        TPFinsurance = 50

    End Enum





End Namespace
