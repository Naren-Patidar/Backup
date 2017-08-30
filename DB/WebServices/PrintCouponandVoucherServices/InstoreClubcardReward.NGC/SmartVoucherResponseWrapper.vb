imports System
Imports System.Xml.Serialization
Imports System.IO



namespace Freetime.AuthorisationGatewayAdapter

	
	<XmlRoot("DotComSmartVoucherResponse", IsNullable:=false)> _
	public class SmartVoucherResponseWrapper

		private  _transactionDateTime as DateTime

		sub new 'SmartVoucherResponseWrapper()

			_transactionDateTime = DateTime.Now

        End Sub

        Public Shared Function readXML(ByVal response As String) As SmartVoucherResponseWrapper

            Dim responseObj As SmartVoucherResponseWrapper
            Dim serializer As New System.Xml.Serialization.XmlSerializer(GetType(SmartVoucherResponseWrapper))
            Dim reader As System.IO.StringReader = New System.IO.StringReader(response)


            Try
                responseObj = (CType(serializer.Deserialize(reader), SmartVoucherResponseWrapper))
            Catch ex As Exception
                Throw ex
            End Try

            Return responseObj

        End Function

        <XmlElement("TransactionID")> _
        Public TransactionID As String

        <XmlIgnore()> _
        Public Property TransactionDateTime() As DateTime

            Get
                Return _transactionDateTime
            End Get

            Set(ByVal Value As DateTime)

                Throw New ApplicationException("Not Supported")
            End Set

        End Property

        ' Workaround for NGC sending us an empty element instead of TransactionCode = 0
        <XmlElement("TransactionResponseCode")> _
        Public Property TransactionResponseCodeString() As String
            Get
                Throw New ApplicationException("Not Supported, Use TransactionResponseCode int property")
            End Get
            Set(ByVal Value As String)
                If (Value <> String.Empty And Not IsDBNull(Value)) Then
                    TransactionResponseCode = CType(Value, Integer)
                End If
            End Set
        End Property

        <XmlIgnore()> _
        Public TransactionResponseCode As Integer = 0

        <XmlElement("Response")> _
        Public Responses As SmartVoucherResponseWrapperNode()
    End Class

	public class SmartVoucherResponseWrapperNode

        <XmlElement("RequestID")> _
        Public RequestID As String
    
        <XmlElement("ResponseCode")> _
        Public ResponseCode As Integer

        <XmlElement("VoucherDetails")> _
        Public VoucherDetails As SmartVoucherResponseWrapperDetails
    End Class

    Public Class SmartVoucherResponseWrapperDetails

        <XmlElement("AlphaNumericID")> _
        Public AlphaNumericID As String

        <XmlElement("VoucherNo")> _
        Public VoucherNo As String

        <XmlIgnore()> _
        Public Type As Integer

        <XmlElement("Type")> _
        Public Property TypeString() As String

			get
                Throw New ApplicationException("Not Supported, Use Type int property")
            End Get
            Set(ByVal Value As String)

                If (Value <> String.Empty) Then

                    Type = Convert.ToInt32(Value)
                End If
            End Set
        End Property

        <XmlElement("ClubcardNo")> _
        Public ClubcardNo As String

        <XmlIgnore()> _
        Public Status As Integer

        <XmlElement("Status")> _
        Public Property StatusString() As String

			get
                Throw New ApplicationException("Not Supported, Use Status int property")
            End Get
            Set(ByVal Value As String)

                If (Value <> String.Empty) Then
                    Status = Convert.ToInt32(Value)
                End If
            End Set
end property

        <XmlIgnore()> _
        Public ExpiryDate As DateTime

        <XmlElement("ExpiryDate")> _
        Public Property ExpiryDateString() As String

			get
				throw new ApplicationException("Not Supported, Use ExpiryDate DateTime property")
            End Get
            Set(ByVal Value As String)

                If (Value = String.Empty) Then
                    ExpiryDate = DateTime.MaxValue
                Else
                    ExpiryDate = DateTime.Parse(Value, New System.Globalization.CultureInfo("en-GB"))
                End If
            End Set
        End Property

        <XmlIgnore()> _
        Public Value As Decimal

        <XmlElement("Value")> _
        Public Property ValueString() As String

            Get
                Throw New ApplicationException("Not Supported, Use Value decimal property")
            End Get
            Set(ByVal Value As String)
                If (Value <> String.Empty) Then
                    Me.Value = Convert.ToDecimal(Value)
                End If
            End Set
        End Property

        <XmlElement("VoucherUsage")> _
        Public VoucherUsage As SmartVoucherResponseWrapperUsage


        Overrides Function ToString() As String

            Return "AlphaNumericID :" + Me.AlphaNumericID + vbCrLf + _
                    "VoucherNo :" + Me.VoucherNo + vbCrLf + _
                    "ClubcardNo :" + Me.ClubcardNo + vbCrLf + _
                    "ExpiryDate :" + CType(Me.ExpiryDate, String) + vbCrLf + _
                    "Status :" + Me.Status.ToString + " " + StatusCode.ToString(Me.Status) + vbCrLf + _
                    "Type :" + Me.Type.ToString + " " + VoucherTypeCode.ToString(Me.Type) + vbCrLf + _
                    "VoucherUsage..." + Me.VoucherUsage.ToString


        End Function


    End Class

    Public Class SmartVoucherResponseWrapperUsage

        <XmlElement("StoreNo")> _
        Public Property StoreNoString() As String

			get
                Throw New ApplicationException("Not Supported, Use StoreNo int property")
            End Get
            Set(ByVal Value As String)
                If (value <> String.Empty) Then
                    StoreNo = Convert.ToInt32(Value)
                End If
            End Set
        End Property

        <XmlIgnore()> _
        Public StoreNo As Integer

        <XmlElement("Channel")> _
        Public Channel As String

        <XmlElement("VirtualStore")> _
        Public VirtualStore As String

        <XmlIgnore()> _
        Public DateTime As DateTime

        <XmlElement("DateTime")> _
        Public Property DateTimeString() As String
            Get
                Throw New ApplicationException("Not Supported, Use ExpiryDate DateTime property")
            End Get
            Set(ByVal Value As String)
                If (value <> String.Empty) Then
                    DateTime = Date.Parse(Value, New System.Globalization.CultureInfo("en-GB"))
                End If
            End Set
        End Property

        Overrides Function ToString() As String

            Return "Channel: " + Me.Channel + vbCrLf + _
                    "Date: " + CType(Me.DateTime, String) + vbCrLf + _
                    "StoreNo: " + CType(Me.StoreNo, String) + vbCrLf + _
                    "VirtualStore: " + Me.VirtualStore

        End Function

    End Class

    Public Enum SmartVoucherResponseCode As Integer
        Success = 0
        NotFound = 7
        ReserveOrRedeemReissued = 11
        ReserveOrRedeemSuspended = 12
        ReserveOrRedeemCancelled = 13
        RedeemReservedElsewhere = 24
        RedeemRedeemed = 9
        ReserveReserved = 25
        ReserveRedeemed = 26
        UnreserveReservedElsewhere = 27
        UnreserveRedeemedElsewhere = 28
        UnreserveActive = 17
        UnreserveSuspended = 20
        UnreserveReissued = 21
        UnreserveCancelled = 22
    End Enum


    'Public Enum VoucherStatus As Integer
    '    Active = 0
    '    Reserved = 1
    '    Redeemed = 2
    '    Expired = 3
    '    Suspended = 4       ' void - dotcom spec
    '    Cancelled = 5
    '    Reissued = 6
    '    RolledOver = 7

    'End Enum

    'Public Enum VoucherType As Integer
    '    Clubcard = 11
    '    TPF = 22
    '    XmasSaver = 33

    'End Enum

end namespace
