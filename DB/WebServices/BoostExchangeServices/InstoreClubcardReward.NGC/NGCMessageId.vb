Public Class MessageCode

    Overloads Shared Function ToString(ByVal MessageCode As NGCMessageCode) As String

        Select Case MessageCode
            Case NGCMessageCode.success
                Return "Success"
            Case NGCMessageCode.VoucherWrongLength
                Return "Voucher length is not equal to 22 digits / Alphacode is not equal to 12 digits."
            Case NGCMessageCode.VoucherNotSmart
                Return "Input Voucher is not a smart voucher	Voucher EAN Code is other than 96"
            Case NGCMessageCode.InvalidCheckDigit
                Return "Invalid Smart Voucher (Check Digit failure)	Voucher number failed check digit validation"
            Case NGCMessageCode.InvalidSupplierCode
                Return "Invalid Supplier Code	The supplier code provided is invalid (The supplier code provided does not match with the supplier code for the voucher in the NGC database)"
            Case NGCMessageCode.InvalidMailingDateCode
                Return "The mailing code provided is invalid (The mailing code provided does not match with the mailing code for the voucher in the NGC database)"
            Case NGCMessageCode.InvalidValueCode
                Return "The value code provided is invalid (The value code provided does not match with the value code for the voucher in the NGC database)"
            Case NGCMessageCode.VoucherNotFound
                Return "The keyed in voucher number is not found in the smart voucher database (for progress) / The input alpha code is not found in the smart voucher database (for dotcom)	The input voucher number / Aplha code  does not exist in the NGC database."
            Case NGCMessageCode.InvalidSourceVendor
                Return "Invalid message source vendor type combination	The input supplier is invalid for the type of message source. "
            Case NGCMessageCode.VoucherRedeemed
                Return "Voucher already redeemed through <redemptionChannel> on <timestamp>	Voucher has already been redeemed through a different redemption channel earlier."
            Case NGCMessageCode.VoucherReisseRequest
                Return "Voucher has been requested for reissue on <timestamp>	A reissue request is pending for the voucher. Hence cannot be allowed to be redeemed"
            Case NGCMessageCode.VoucherReissued
                Return "Voucher has been reissued on <timestamp>	The vouchers have been reissued on request and hence cannot be redeemed"
            Case NGCMessageCode.VoucherReissueRequestI
                Return "The reissue request made on <timestamp> is being investigated by the Clubcard team	The reissue request for the voucher is being investigated by the Clubcard team and hence cannot be allowed to be redeemed"
            Case NGCMessageCode.Vouchercancelled
                Return "Voucher has been cancelled on <timestamp>	The vouchers have been cancelled and hence cannot be allowed to be redeemed."
            Case NGCMessageCode.VoucherExpired
                Return "Voucher has expired on <timestamp>	The vouchers have expired and hence cannot be allowed to be redeemed."
            Case NGCMessageCode.VoucherReactivated
                Return "Voucher has been reactivated	Voucher status is successfully reversed by NGC system"
            Case NGCMessageCode.VoucherReactivateFailNGC
                Return "Voucher reactivation failed	Voucher status reversal failed at NGC system"
            Case NGCMessageCode.VoucherVoidFail
                Return "Voucher is already in active state	Attempt to void a voucher which is already in active state without attempting to redeem it."
            Case NGCMessageCode.VoucherReactivateFail
                Return "The keyed in voucher number is not found in the smart voucher database. The voucher cannot be reactivated	Attempt to void a voucher not available in smart voucher database. Possible during manual keying in of voucher numbers"
            Case NGCMessageCode.VoucherReissueTill
                Return "Voucher has been requested for reissue on <timestamp>. The voucher cannot be reactivated.	Attempt to void a voucher at till which is in void state due to a pending reissue request, without attempting to redeem it."
            Case NGCMessageCode.VoucherReissueInvestiage
                Return "The reissue request made on <timestamp> is being investigated by the Clubcard team.The voucher cannot be reactivated.	Attempt to void a voucher at till which is in void state due to a pending reissue request that is being investigated by Clubcard team, without attempting to redeem it."
            Case NGCMessageCode.VoucherReactivateTill
                Return "Voucher has been reissued on <timestamp>.  The voucher cannot be reactivated.	Attempt to void a voucher at till which is in void state due to a successful reissue, without attempting to redeem it."
            Case NGCMessageCode.VoucherCancelledTill
                Return "Voucher has been cancelled on <timestamp>. The voucher cannot be reactivated.	Attempt to void a voucher at till which is in void state due to a cancellation, without attempting to redeem it."
            Case NGCMessageCode.VoucherExpiredTill
                Return "Voucher has expired on <timestamp>. The voucher cannot be reactivated.	Attempt to void a voucher at till which is in void state due to a expiry, without attempting to redeem it."
            Case NGCMessageCode.VoucherReserved
                Return "Voucher has been reserved by <redemption channel> on <timestamp>. 	Attempt to redeem a voucher reserved by one of the dotcom redemption channels"
            Case Else
                Return ""

        End Select

    End Function

    Public Enum NGCMessageCode As Integer
        success = 0
        VoucherWrongLength = 1          'Voucher length is not equal to 22 digits / Alphacode is not equal to 12 digits.
        VoucherNotSmart = 2             'Input Voucher is not a smart voucher	Voucher EAN Code is other than 96
        InvalidCheckDigit = 3           'Invalid Smart Voucher (Check Digit failure)	Voucher number failed check digit validation
        InvalidSupplierCode = 4         'Invalid Supplier Code	The supplier code provided is invalid (The supplier code provided does not match with the supplier code for the voucher in the NGC database)
        InvalidMailingDateCode = 5      'The mailing code provided is invalid (The mailing code provided does not match with the mailing code for the voucher in the NGC database)
        InvalidValueCode = 6            'The value code provided is invalid (The value code provided does not match with the value code for the voucher in the NGC database)
        VoucherNotFound = 7             'The keyed in voucher number is not found in the smart voucher database (for progress) / The input alpha code is not found in the smart voucher database (for dotcom)	The input voucher number / Aplha code  does not exist in the NGC database.
        InvalidSourceVendor = 8         'Invalid message source vendor type combination	The input supplier is invalid for the type of message source. 
        VoucherRedeemed = 9             'Voucher already redeemed through <redemptionChannel> on <timestamp>	Voucher has already been redeemed through a different redemption channel earlier.
        VoucherReisseRequest = 10       'Voucher has been requested for reissue on <timestamp>	A reissue request is pending for the voucher. Hence cannot be allowed to be redeemed
        VoucherReissued = 11            'Voucher has been reissued on <timestamp>	The vouchers have been reissued on request and hence cannot be redeemed
        VoucherReissueRequestI = 12     'The reissue request made on <timestamp> is being investigated by the Clubcard team	The reissue request for the voucher is being investigated by the Clubcard team and hence cannot be allowed to be redeemed
        Vouchercancelled = 13           'Voucher has been cancelled on <timestamp>	The vouchers have been cancelled and hence cannot be allowed to be redeemed.
        VoucherExpired = 14             'Voucher has expired on <timestamp>	The vouchers have expired and hence cannot be allowed to be redeemed.
        VoucherReactivated = 15         'Voucher has been reactivated	Voucher status is successfully reversed by NGC system
        VoucherReactivateFailNGC = 16   'Voucher reactivation failed	Voucher status reversal failed at NGC system
        VoucherVoidFail = 17            'Voucher is already in active state	Attempt to void a voucher which is already in active state without attempting to redeem it.
        VoucherReactivateFail = 18      'The keyed in voucher number is not found in the smart voucher database. The voucher cannot be reactivated	Attempt to void a voucher not available in smart voucher database. Possible during manual keying in of voucher numbers
        VoucherReissueTill = 19         'Voucher has been requested for reissue on <timestamp>. The voucher cannot be reactivated.	"Attempt to void a voucher at till which is in void state due to a pending reissue request, without 
        'attempting to redeem it.
        VoucherReissueInvestiage = 20   'The reissue request made on <timestamp> is being investigated by the Clubcard team.The voucher cannot be reactivated.	"Attempt to void a voucher at till which is in void state due to a pending reissue request that is being investigated by Clubcard team, without 
        'attempting to redeem it.
        VoucherReactivateTill = 21      'Voucher has been reissued on <timestamp>.  The voucher cannot be reactivated.	Attempt to void a voucher at till which is in void state due to a successful reissue, without attempting to redeem it.
        VoucherCancelledTill = 22       'Voucher has been cancelled on <timestamp>. The voucher cannot be reactivated.	Attempt to void a voucher at till which is in void state due to a cancellation, without attempting to redeem it.
        VoucherExpiredTill = 23         'Voucher has expired on <timestamp>. The voucher cannot be reactivated.	Attempt to void a voucher at till which is in void state due to a expiry, without attempting to redeem it.
        VoucherReserved = 24            'Voucher has been reserved by <redemption channel> on <timestamp>. 	Attempt to redeem a voucher reserved by one of the dotcom redemption channels
    End Enum
End Class
Public Class StatusCode

    Overloads Shared Function ToString(ByVal MessageCode As voucherstatus) As String

        Select Case MessageCode
            Case VoucherStatus.Active
                Return "Active"
            Case VoucherStatus.Reserved
                Return "Reserved"
            Case VoucherStatus.Redeemed
                Return "Redeemed"
            Case VoucherStatus.RedeemedAlready
                Return "Already Redeemed" ' online booking vouchers may have already been redeemed
            Case VoucherStatus.Expired
                Return "Expired"
            Case VoucherStatus.Suspended
                Return "Suspended"
            Case VoucherStatus.Cancelled
                Return "Cancelled"
            Case VoucherStatus.Reissued
                Return "Reissued"
            Case VoucherStatus.RolledOver
                Return "Rolled over"
            Case Else
                Return ""
        End Select

    End Function

    Public Enum VoucherStatus As Integer
        Active = 0
        Reserved = 1
        Redeemed = 2
        RedeemedAlready = -2 ' online booking vouchers may have already been redeemed (FREETIME CREATED STATUS)
        NotFound = -1
        Expired = 3
        Suspended = 4       ' void - dotcom spec
        Cancelled = 5
        Reissued = 6
        RolledOver = 7
        ProcessedOffline = 100  'used when Freetime are unable to contact NGC to process vouchers
    End Enum

End Class
Public Class VoucherTypeCode

    Overloads Shared Function ToString(ByVal MessageCode As VoucherType) As String

        Select Case MessageCode
            Case VoucherType.Clubcard
                Return "Clubcard"
            Case VoucherType.TPF
                Return "TPF"
            Case VoucherType.XmasSaver
                Return "XmasSaver"

                ' ROI - map to same text 
            Case VoucherType.Clubcard_ROI_Lo Or VoucherType.Clubcard_ROI_Hi
                Return "Clubcard"
            Case VoucherType.TPF_ROI_Lo Or VoucherType.TPF_ROI_Hi
                Return "TPF"
            Case VoucherType.XmasSaver_ROI_Lo Or VoucherType.XmasSaver_ROI_Hi
                Return "XmasSaver"
            Case Else
                Return ""
        End Select

    End Function

    Public Enum VoucherType As Integer
        Clubcard = 11
        TPF = 22
        XmasSaver = 33

        ' ROI
        Clubcard_ROI_Lo = 50
        Clubcard_ROI_Hi = 51
        TPF_ROI_Lo = 52
        TPF_ROI_Hi = 53
        XmasSaver_ROI_Lo = 54
        XmasSaver_ROI_Hi = 55

    End Enum
    Public Shared Function XmasSaver(ByVal vt As VoucherType) As Boolean
        ' xmas club can't be used
        ' identify XMAS saver (both UK and ROI)
        If vt = VoucherType.XmasSaver Or vt = VoucherType.XmasSaver_ROI_Hi Or vt = VoucherType.XmasSaver_ROI_Lo Then
            Return True
        Else
            Return False
        End If

    End Function
End Class