Imports InstoreClubcardReward.NGC.NGCDirect
Imports Microsoft.Win32
Imports InstoreClubcardReward.NGC.cUtilities


' put a wrapper around the call to get household details
Public Class callHouseholdDetails

    'TODO - better location - constants copied from ncg reuest wrapper used for dotom
    Private Const REQUEST_STORENO As String = "01955" ' "2643"
    Private Const REQUEST_CHANNEL As String = "Postal Deals" ' "GHS"
    'Private Const REQUEST_VIRTUALSTORE As String = "" '"99"

    ' allow clubcard or ean 
    Public Function cProvideHouseholdDetails(ByVal cc As String, ByVal ean As String, ByVal agentId As Integer, ByVal Country As String) As NGCDirect.HouseholdDetailResponse
        'TODO pass in agentId
        'Dim Agentid As Integer = 1


        Dim request As New MessagingWebService
        Dim hr As New HouseholdRequest
        hr.Source = "Freetime"
        hr.TransactionDateTime = Now()
        hr.TransactionID = Guid.NewGuid.ToString

        Dim hdr As New HouseholdDetailRequest
        ' supply both - blank as required
        hdr.ClubcardNo = cc
        hdr.VoucherNo = ean

        hdr.StoreNo = REQUEST_STORENO
        hdr.Channel = REQUEST_CHANNEL
        hdr.RequestID = Guid.NewGuid.ToString
        hr.Request = hdr


        'TODO load url dynamically
        ' set URL to use from the app.config in.... WAProcessOnlineDeals!!!!!!!!
        ' this is in
        'Dim url As String = System.Configuration.ConfigurationSettings.AppSettings.GetValues("NGCMessage.NGCDirect.MessagingWebService")(0)
        'request.Url = url

        'test url
        'request.Url = "http://172.22.83.184/NGCWebService/NGCUKMessagingService.asmx"

        request.Url = cNGCRegistry.GetSetting("NGCDirect", Country)
        ' timeout in ms defaults to 100000
        request.Timeout = cNGCRegistry.GetSetting("NGCTimeout", Country)

        ' production url
        'request.Url = "http://192.168.208.43/NGCWebService/NGCUKMessagingService.asmx"



        Dim response As New HouseholdDetailResponse


        ' get to the xml from the object....
        ' -------------   DEBUG / TESTING --------------------
        ' -------------   OUTPUT TO FILE AND TO STRING --------------------
        Dim sn As New System.Xml.Serialization.XmlSerializerNamespaces
        Dim serializer As New System.Xml.Serialization.XmlSerializer(GetType(HouseholdRequest))
        sn.Add(String.Empty, String.Empty)
        ' serialize to file
        'Dim writer As New System.IO.StreamWriter("c:\temp\HouseholdRequestIn.xml")
        'serializer.Serialize(writer, hr, sn)
        'writer.Close()
        ' end serialize to file

        ' serialize to string (reuse serializer defined above)
        Dim sr As New System.IO.StringWriter
        serializer.Serialize(sr, hr, sn)
        sr.Close()
        Dim sb As New System.Text.StringBuilder
        sb = sr.GetStringBuilder
        Dim xmlstring As String = sb.ToString


        ' -------------   DEBUG / TESTING --------------------
        ' Note need serialization to string for saving
        Dim requestId As Integer
        Try
            ' record the xml
            requestId = cSaveNGC.SaveNGC(agentId, request.Url, xmlstring, 0)
        Catch ex As Exception
            cUtilities.WriteEventLogEntry(eEventLogSource.NGCMessage, "callAuthorise.vb", "cProvideHouseholdDetails - SaveNGC: " & ex.ToString, EventLogEntryType.Error)
            Throw ex
        End Try

        ' declare timing variables
        Dim beforeServiceCall As DateTime = Now()
        Dim afterServiceCall As DateTime


        Try
            response = request.ProvideHouseholdDetails(hr)
            afterServiceCall = Now()

        Catch ex As Exception
            afterServiceCall = Now()

            cUtilities.WriteEventLogEntry(eEventLogSource.NGCMessage, "callAuthorise.vb", "cProvideHouseholdDetails - ProvideHouseholdDetails: " & ex.ToString, EventLogEntryType.Error)
            'System.Diagnostics.EventLog.WriteEntry("callAuthorise.vb", "cProvideHouseholdDetails (after ProvideHouseholdDetails) : " & ex.ToString)

            ' save the return message
            cSaveNGC.SaveNGC(agentId, request.Url, "ERROR THROWN: " + ex.ToString, requestId)

            ' save the timing
            cSaveNGC.SaveNGCTiming(requestId, beforeServiceCall, afterServiceCall)

            Throw ex
        End Try

        afterServiceCall = Now()

        'TODO save xml response to db #######################
        ' -------------   DEBUG / TESTING --------------------
        ' -------------   OUTPUT TO FILE AND TO STRING --------------------
        serializer = New System.Xml.Serialization.XmlSerializer(GetType(HouseholdDetailResponse))
        sn.Add(String.Empty, String.Empty)
        ' serialize to file
        'writer = New System.IO.StreamWriter("c:\temp\HouseholdRequestOut.xml")
        'serializer.Serialize(writer, response, sn)
        'writer.Close()
        ' end serialize to file

        ' serialize to string (reuse serializer defined above)
        sr = New System.IO.StringWriter
        serializer.Serialize(sr, response, sn)
        sr.Close()
        sb = New System.Text.StringBuilder
        sb = sr.GetStringBuilder
        xmlstring = sb.ToString
        ' -------------   DEBUG / TESTING --------------------
        Try
            ' record the return message
            cSaveNGC.SaveNGC(agentId, request.Url, xmlstring, requestId)
        Catch ex As Exception
            cUtilities.WriteEventLogEntry(eEventLogSource.NGCMessage, "callAuthorise.vb", "cProvideHouseholdDetails - ProvideHouseholdDetails - SaveNGC: " & ex.ToString, EventLogEntryType.Error)
            Throw ex
        End Try


        ' ################# TIMING OF WEBSERVICE ####################
        Try
            ' save the return message
            cSaveNGC.SaveNGCTiming(requestId, beforeServiceCall, afterServiceCall)
        Catch ex As Exception
            cUtilities.WriteEventLogEntry(eEventLogSource.NGCMessage, "callAuthorise.vb", "cProvideHouseholdDetails - ProvideHouseholdDetails- SaveNGCTiming: " & ex.ToString, EventLogEntryType.Error)
        End Try


        Try
            '    ' Save some basic household data
            cSaveNGC.SaveNGCHH(requestId, response)
        Catch ex As Exception
            cUtilities.WriteEventLogEntry(eEventLogSource.NGCMessage, "callAuthorise.vb", "cProvideHouseholdDetails - ProvideHouseholdDetails- SaveNGCHH: " & ex.ToString, EventLogEntryType.Error)
        End Try


        Return response



    End Function

    ' without country default to UK
    ' DO NOT USE THIS INTERFACE - COMMENT OUT TO TEST
    ' LEFT IN FOR THREAD CODE IN ONLINEDEALSCONSOLE
    ' ####################### CHECK ##################
    Public Function cProvideHouseholdDetails(ByVal cc As String, ByVal ean As String, ByVal agentId As Integer) As NGCDirect.HouseholdDetailResponse

        Return cProvideHouseholdDetails(cc, ean, agentId, "UK")

    End Function


End Class
