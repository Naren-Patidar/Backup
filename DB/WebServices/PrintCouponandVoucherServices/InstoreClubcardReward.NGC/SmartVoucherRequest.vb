Imports System.Xml.Serialization

' 
<XmlRoot("DotComSmartVoucherRequest", IsNullable:= False)> _
Public Class SmartVoucherRequest

    Public TransactionID As String
    Public TransactionDateTime As DateTime
    Public Source As String
    <XmlElement("Request")> _
    Public requests() As Request



    Public Class Request
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
    End Class


End Class
