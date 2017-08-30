Imports System
Imports System.Xml.Serialization

Namespace Freetime.AuthorisationGatewayAdapter

    <XmlRoot("HouseholdResponse", IsNullable:=False)> _
        Public Class HouseholdResponseWrapper
        Private _transactionDateTime As DateTime

        Sub New() 'SmartVoucherResponseWrapper()

            _transactionDateTime = DateTime.Now

        End Sub

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
                If (Value <> String.Empty And Value <> vbNull) Then
                    TransactionResponseCode = CType(Value, Integer)
                End If
            End Set
        End Property

        <XmlIgnore()> _
        Public TransactionResponseCode As Integer = 0

        <XmlElement("Response")> _
        Public Responses As HouseholdResponseWrapperNode()
    End Class

    Public Class HouseholdResponseWrapperNode

        <XmlElement("RequestID")> _
        Public RequestID As String

        <XmlElement("ResponseCode")> _
        Public ResponseCode As Integer

        <XmlElement("OLAResponseCode")> _
        Public OLAResponseCode As String

        <XmlElement("HouseholdDetails")> _
        Public HouseholdDetails As HouseholdWrapperDetails
    End Class

    Public Class HouseholdWrapperDetails

        <XmlElement("HouseholdStatusCode")> _
        Public HouseholdStatusCode As String

        <XmlElement("RewardType")> _
        Public RewardType As String

        <XmlElement("RewardTypeOptinDate")> _
        Public RewardTypeOptinDate As DateTime

        <XmlElement("AddressLine1")> _
        Public AddressLine1 As String

        <XmlElement("Postcode")> _
        Public Postcode As String

        <XmlElement("PhoneNumber")> _
        Public PhoneNumber As String

        <XmlElement("EmailAddress")> _
        Public EmailAddress As String

        <XmlElement("CustomerFlag")> _
        Public CustomerFlag As CustomerFlags

        <XmlElement("DealStatus")> _
        Public DealStatus As String

        <XmlElement("DealSpend")> _
        Public DealSpend As Integer

        <XmlElement("PreviousDealStatus")> _
        Public PreviousDealStatus As String

        <XmlElement("PreviousDealSpend")> _
        Public PreviousDealSpend As Integer

        <XmlElement("RemainingDealsSpend")> _
        Public RemainingDealsSpend As Integer

        <XmlElement("AverageWeeklyDealsSpend")> _
        Public AverageWeeklyDealsSpend As Integer

        <XmlElement("SuppliedClubcardDetails")> _
        Public SuppliedClubcardDetails As SuppliedClubcardDetails

        <XmlElement("ClubcardNumbers")> _
        Public ClubcardNumbers As ClubcardNumbers()

        <XmlElement("VendorSpecificDetails")> _
        Public VendorSpecificDetails As VendorSpecificDetails

        <XmlElement("OldVoucherValueUnredeemed")> _
        Public OldVoucherValueUnredeemed As Integer

    End Class

    Public Class CustomerFlags


        <XmlElement("ContactforMarketResearch")> _
        Public ContactforMarketResearch As String

        <XmlElement("ContactbyPhoneforMarketResearch")> _
        Public ContactbyPhoneforMarketResearch As String

        <XmlElement("ContactonlybyTesco")> _
        Public ContactonlybyTesco As String

        <XmlElement("Contactby3rdparties")> _
        Public Contactby3rdparties As String

        <XmlElement("Diabetic")> _
        Public Diabetic As String

        <XmlElement("Vegetarian")> _
        Public Vegetarian As String

        <XmlElement("Teetotal")> _
        Public Teetotal As String

        <XmlElement("Kosher")> _
        Public Kosher As String

        <XmlElement("Halal")> _
        Public Halal As String

        <XmlElement("Organic")> _
        Public Organic As String

        <XmlElement("UnicornExcl")> _
        Public UnicornExcl As String

        <XmlElement("HouseholdInPromotion")> _
        Public HouseholdInPromotion As String

        <XmlElement("BabyClub")> _
        Public BabyClub As String

        <XmlElement("ToddlerClub")> _
        Public ToddlerClub As String

        <XmlElement("KidsClub")> _
        Public KidsClub As String

        <XmlElement("WineClub")> _
        Public WineClub As String

        <XmlElement("future1")> _
        Public future1 As String

        <XmlElement("future2")> _
        Public future2 As String

        <XmlElement("future3")> _
        Public future3 As String

        <XmlElement("future4")> _
        Public future4 As String


    End Class


    Public Class SuppliedClubcardDetails

        <XmlElement("SuppliedClubcardNumber")> _
        Public SuppliedClubcardNumber As String

        <XmlElement("SuppliedClubcardStatus")> _
        Public SuppliedClubcardStatus As String

        <XmlElement("SuppliedClubcardType")> _
        Public SuppliedClubcardType As String

        <XmlElement("SuppliedClubcardHolderDetails")> _
        Public SuppliedClubcardHolderDetails As SuppliedClubcardHolderDetails

    End Class



    Public Class SuppliedClubcardHolderDetails

        <XmlElement("Title")> _
        Public Title As String

        <XmlElement("Initials")> _
        Public Initials As String

        <XmlElement("Forename")> _
        Public Forename As String

        <XmlElement("Surname")> _
        Public Surname As String


    End Class

    Public Class ClubcardNumbers

        <XmlElement("ClubcardNumber")> _
        Public ClubcardNumber As String


    End Class

    Public Class VendorSpecificDetails

        <XmlElement("VendorId")> _
        Public VendorId As Integer

        <XmlElement("CurrentPointsBalance")> _
        Public CurrentPointsBalance As Integer

        <XmlElement("CurrentPointsBalanceDate")> _
        Public CurrentPointsBalanceDate As DateTime

        <XmlElement("SmartVoucherDetails")> _
        Public SmartVoucherDetails As SmartVoucherDetails

    End Class




    Public Class SmartVoucherDetails

        <XmlElement("VendorNo")> _
        Public VendorNo As String

        <XmlElement("AlphaNumericID")> _
        Public AlphaNumericID As String

        <XmlElement("Value")> _
        Public Value As Integer

        <XmlElement("ExpiryDate")> _
        Public ExpiryDate As DateTime

        <XmlElement("Status")> _
        Public Status As Integer

    End Class


End Namespace