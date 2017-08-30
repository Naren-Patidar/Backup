Imports InstoreClubcardReward.NGC.NGCDirect
Imports Microsoft.Win32
Imports InstoreClubcardReward.NGC.cUtilities



Public Class callAuthorise

    ' Default timesouts. 100000 ms 100secs is the unset value
    Public Function cAuthorise(ByVal xml As String, ByVal agentId As Integer, ByVal Country As String) As String

        Dim wr As New MessagingWebService
        Dim guidstr As String = Guid.NewGuid().ToString()
        Dim response As String
        Dim source As String = "Tesco.com"

        ' switch source for ROI
        If Country = "ROI" Then
            source = "Tesco.ie"
        End If

        ' get the Web Reference for the authorisation gateway
        ' Ensure that this property is dynamic so that thiscan be set.
        ' test URL
        ' http://172.23.13.243/tesco.pos.authgateway.central/authorise.asmx
        ' live URL
        ' http://192.168.208.41/Tesco.POS.AuthGateway.Central/authorise.asmx

        wr.Url = cNGCRegistry.GetSetting("WebReference", Country)
        ' timeout in ms defaults to 100000
        wr.Timeout = cNGCRegistry.GetSetting("AGTimeout", Country)

        Dim requestId As Integer
        Try
            'save the sent message
            requestId = cSaveNGC.SaveNGC(agentId, wr.Url, xml, 0)
        Catch ex As Exception
            cUtilities.WriteEventLogEntry(eEventLogSource.NGCMessage, "callAuthorise.vb", "cAuthorise - SaveNGC: " & ex.ToString, EventLogEntryType.Error)
        End Try

        ' declare timing variables
        Dim beforeServiceCall As DateTime = Now()
        Dim afterServiceCall As DateTime

        Try

            response = wr.ProcessVouchers(xml, False)

            afterServiceCall = Now()
        Catch ex As Exception
            ' record after the exception so that there is something to save for timing
            afterServiceCall = Now()
            'Throw New System.Exception("An exception has occurred.")
            'System.Diagnostics.EventLog.WriteEntry("callAuthorise.vb", "cAuthorise (after.PerformAuthorisation) : " & ex.ToString)
            cUtilities.WriteEventLogEntry(eEventLogSource.NGCMessage, "callAuthorise.vb", "cAuthorise - PerformAuthorisation: " & ex.ToString, EventLogEntryType.Error)

            ' try and save the response before the throw ex
            ' at least get a time for it and the error event.
            ' For CSD saving to XML wrap in xml
            cSaveNGC.SaveNGC(agentId, wr.Url, "<ErrorThrown>" + ex.ToString + "</ErrorThrown>", requestId)

            ' save the timing
            cSaveNGC.SaveNGCTiming(requestId, beforeServiceCall, afterServiceCall)

            Throw ex
        End Try



        Try
            ' save the return message
            ' CSD Change..,.. For saving in xml ############
            response = response.Replace("utf-8", "utf-16")
            cSaveNGC.SaveNGC(agentId, wr.Url, response, requestId)
        Catch ex As Exception
            cUtilities.WriteEventLogEntry(eEventLogSource.NGCMessage, "callAuthorise.vb", "cAuthorise - PerformAuthorisation- SaveNGC: " & ex.ToString, EventLogEntryType.Error)
        End Try

        ' ################# TIMING OF WEBSERVICE ####################
        Try
            ' save the return message
            cSaveNGC.SaveNGCTiming(requestId, beforeServiceCall, afterServiceCall)
        Catch ex As Exception
            cUtilities.WriteEventLogEntry(eEventLogSource.NGCMessage, "callAuthorise.vb", "cAuthorise - PerformAuthorisation- SaveNGCTiming: " & ex.ToString, EventLogEntryType.Error)
        End Try



        Return response


    End Function

    ' without country default to UK
    Public Function cAuthorise(ByVal xml As String, ByVal agentId As Integer) As String

        Return cAuthorise(xml, agentId, "UK")

    End Function




End Class

Public Class callClubcardOnlineGetUnusedVoucherDetails
    Public Function GetUnusedVoucherDtls(ByVal ClubcardNumber As String, ByVal Country As String) As GetUnusedVoucherDtlsRsp
        Dim wr As MessagingWebService = Nothing
        Dim ds As DataSet = Nothing
        Dim response As GetUnusedVoucherDtlsRsp = Nothing
        Try
            response = New GetUnusedVoucherDtlsRsp()
            ds = New DataSet()
            wr = New MessagingWebService()
            wr.Url = cNGCRegistry.GetSetting("WebReference", Country)
            ds = wr.ClubcardOnlineGetUnusedVoucherDetails(ClubcardNumber)
            response = New GetUnusedVoucherDtlsRsp(ds)
        Catch ex As Exception
            Throw ex
            'Logger.Write(ex, "General", 1, 6500, System.Diagnostics.TraceEventType.[Error], "ClubcardNumber:" & ClubcardNumber)
        End Try

        Return response
    End Function


End Class


' put a wrapper around the call to get household details
Public Class callHouseholdDetails

    'TODO - better location - constants copied from ncg reuest wrapper used for dotom
    Private Const REQUEST_STORENO As String = "01955" ' "2643"
    Private Const REQUEST_CHANNEL As String = "in store exchange" ' "GHS"

    'Private Const REQUEST_VIRTUALSTORE As String = "" '"99"

    ' allow clubcard or ean 
    Public Function cProvideHouseholdDetails(ByVal cc As String, ByVal ean As String, ByVal agentId As Integer, ByVal Country As String) As NGCDirect.HouseholdDetailResponse


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


        ' load url dynamically
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



Public Class cDoubleUpWS


    ' call doubleUpwebservice
    ' this is used for creating and cancelling tokens. Difference is in the xml string passed 
    ' 
    ' follow same format as voucher validation in callAuthorise ie agent and country

    Public Function callService(ByVal xml As String, ByVal agentId As Integer, ByVal Country As String) As String

        Dim du As New DoubleUp.DoubleUpService
        'Dim du As New DoubleUpDirect.Service

        'Dim guidstr As String = Guid.NewGuid().ToString()
        Dim response As String

        du.Url = cNGCRegistry.GetSetting("DoubleUp", Country)
        ' timeout in ms defaults to 100000
        du.Timeout = cNGCRegistry.GetSetting("ISTimeout", Country)

        Dim requestId As Integer
        Try
            'save the sent message
            requestId = cSaveNGC.SaveNGC(agentId, du.Url, xml, 0)
        Catch ex As Exception
            cUtilities.WriteEventLogEntry(eEventLogSource.NGCMessage, "cDoubleUpWS.vb", "callService - SaveNGC: " & ex.ToString, EventLogEntryType.Error)
        End Try

        ' declare timing variables
        Dim beforeServiceCall As DateTime = Now()
        Dim afterServiceCall As DateTime

        Try

            response = du.DoubleUpRequest(xml)

            afterServiceCall = Now()
        Catch ex As Exception
            ' record after the exception so that there is something to save for timing
            afterServiceCall = Now()
            'Throw New System.Exception("An exception has occurred.")
            'System.Diagnostics.EventLog.WriteEntry("callAuthorise.vb", "cAuthorise (after.PerformAuthorisation) : " & ex.ToString)
            cUtilities.WriteEventLogEntry(eEventLogSource.NGCMessage, "cDoubleUpWS.vb", "callService - DoubleUpRequest: " & ex.ToString, EventLogEntryType.Error)

            ' try and save the response before the throw ex
            ' at least get a time for it and the error event.
            ' For CSD saving to XML wrap in xml
            cSaveNGC.SaveNGC(agentId, du.Url, "<ErrorThrown>" + ex.ToString + "</ErrorThrown>", requestId)

            ' save the timing
            cSaveNGC.SaveNGCTiming(requestId, beforeServiceCall, afterServiceCall)

            Throw ex
        End Try



        Try
            ' save the return message
            cSaveNGC.SaveNGC(agentId, du.Url, response, requestId)
        Catch ex As Exception
            cUtilities.WriteEventLogEntry(eEventLogSource.NGCMessage, "cDoubleUpWS.vb", "callService - DoubleUpRequest- SaveNGC: " & ex.ToString, EventLogEntryType.Error)
        End Try

        ' ################# TIMING OF WEBSERVICE ####################
        Try
            ' save the return message
            cSaveNGC.SaveNGCTiming(requestId, beforeServiceCall, afterServiceCall)
        Catch ex As Exception
            cUtilities.WriteEventLogEntry(eEventLogSource.NGCMessage, "cDoubleUpWS.vb", "callService - DoubleUpRequest- SaveNGCTiming: " & ex.ToString, EventLogEntryType.Error)
        End Try



        Return response

    End Function
    Public Function callService(ByVal xml As String, ByVal agentId As Integer) As String
        Return callService(xml, agentId, "UK")
    End Function


End Class







Public Class cNGCRegistry
    'Public Shared Function GetSetting(ByVal strSetting As String) As String
    '    Return GetSetting(strSetting, "UK")
    'End Function

    'Public Shared Function GetSetting(ByVal strSetting As String, ByVal country As String) As String
    '    Dim strSettingValue As String    ' registry setting value
    '    Try
    '        Dim objFreetimeRegistry As RegistryKey = Registry.LocalMachine.OpenSubKey("Software").OpenSubKey("Freetime").OpenSubKey("NGC")
    '        ' setting name has country suffix eg ROI
    '        If country <> "UK" Then
    '            strSetting += country
    '        End If
    '        strSettingValue = CType(objFreetimeRegistry.GetValue(strSetting), String)
    '        objFreetimeRegistry.Close()
    '    Catch ex As Exception
    '        cUtilities.WriteEventLogEntry(eEventLogSource.NGCMessage, "callAuthorise.vb", "cNGCRegistry:GetSetting " & strSetting & " " & ex.ToString, EventLogEntryType.Error)
    '        Throw ex
    '    End Try

    '    Return strSettingValue

    'End Function 'cNGCRegistry

    Public Shared Function GetSetting(ByVal strSetting As String, ByVal country As String) As String

        Dim strSettingValue As String    ' registry setting value

        ' get from app settings
        Dim countrySetting As String = String.Format("{0}{1}", strSetting, country)
        strSettingValue = cUtilities.GetRegistrySetting(countrySetting)

        Return strSettingValue

    End Function 'cNGCRegistry


End Class



Public Class cSaveNGC


    ' called by methods in callAuthorise.vb
    ' used to save NGC messages
    Shared Function SaveNGC(ByVal agentid As Integer, ByVal MessageURL As String, ByVal MessageXML As String, ByVal requestId As String) As Integer

        'Dim sp As String = "tccAppNGC"
        Dim sp As String = "InsertNGCMessage" ' CSD app NOTE change with xml being used


        Dim NCGId As Integer    ' return value
        Try

            Dim conn As New System.Data.SqlClient.SqlConnection(cConnection.GetConnection("Firstcall"))
            Dim Command As New SqlClient.SqlCommand(sp, conn)
            Command.CommandType = CommandType.StoredProcedure

            Command.Parameters.Add("@AgentId", SqlDbType.Int).Value = agentid
            Command.Parameters.Add("@MessageURL", SqlDbType.VarChar).Value = MessageURL
            Command.Parameters.Add("@MessageXML", SqlDbType.VarChar).Value = MessageXML
            'Command.Parameters.Add("@MessageXML", SqlDbType.Xml).Value = MessageXML     'CSD change to XML
            Command.Parameters.Add("@RequestId", SqlDbType.Int).Value = requestId
            ' record the machine that is running this code - hence makeing the call to NGC
            Command.Parameters.Add("@CallingMachine", SqlDbType.VarChar).Value = Left(System.Environment.MachineName, 255)
            Command.Parameters.Add("@NGCId", SqlDbType.Int) ' CSD require slight variable name change ############ NGC/NCG
            Command.Parameters.Item("@NGCId").Direction = ParameterDirection.Output
            Command.Connection.Open()
            Command.ExecuteScalar()
            'get return value
            NCGId = CType(Command.Parameters("@NGCId").Value, Integer)

            Command.Connection.Close()
            Command = Nothing
        Catch ex As Exception
            cUtilities.WriteEventLogEntry(eEventLogSource.NGCMessage, "callAuthorise.vb", "cSaveNGC - SaveNGC: " & ex.ToString, EventLogEntryType.Error)
            Throw ex
        End Try

        Return NCGId

    End Function 'SaveNGC

    Shared Sub SaveNGCTiming(ByVal requestId As Integer, ByVal beforeServiceCall As DateTime, ByVal afterServiceCall As DateTime)

        'Dim sp As String = "tccAppNGCTiming"
        Dim sp As String = "InsertNGCTiming" ' CSD app


        Try
            Dim conn As New System.Data.SqlClient.SqlConnection(cConnection.GetConnection("Firstcall"))
            Dim Command As New SqlClient.SqlCommand(sp, conn)
            Command.CommandType = CommandType.StoredProcedure

            Command.Parameters.Add("@RequestId", SqlDbType.Int).Value = requestId
            Command.Parameters.Add("@beforeServiceCall", SqlDbType.DateTime).Value = beforeServiceCall
            Command.Parameters.Add("@afterServiceCall", SqlDbType.DateTime).Value = afterServiceCall
            ' record the interval (ms)
            Command.Parameters.Add("@MsgInterval", SqlDbType.BigInt).Value = (afterServiceCall - beforeServiceCall).Ticks

            Command.Connection.Open()
            Command.ExecuteScalar()

            Command.Connection.Close()
            Command = Nothing
        Catch ex As Exception
            cUtilities.WriteEventLogEntry(eEventLogSource.NGCMessage, "callAuthorise.vb", "cSaveNGC - SaveNGCTiming: " & ex.ToString, EventLogEntryType.Error)
            Throw ex
        End Try

    End Sub 'SaveNGCTiming()

    Shared Sub SaveNGCHH(ByVal requestId As Long, ByVal response As HouseholdDetailResponse)

        Try
            Dim conn As New System.Data.SqlClient.SqlConnection(cConnection.GetConnection("Firstcall"))
            Dim Command As New SqlClient.SqlCommand("tccAppNGCHousehold", conn)
            Command.CommandType = CommandType.StoredProcedure

            Command.Parameters.Add("@RequestId", SqlDbType.Int).Value = requestId
            Command.Parameters.Add("@status", SqlDbType.VarChar).Value = response.Response.DealStatus

            Command.Parameters.Add("@Address1", SqlDbType.VarChar).Value = response.Response.AddressLine1
            Command.Parameters.Add("@Postcode", SqlDbType.VarChar).Value = response.Response.Postcode

            For Each cc As ClubcardNumbers In response.Response.ClubcardNumbers

                ' add the parameter as needed
                If Command.Parameters.Contains("@Clubcard") Then
                    Command.Parameters("@Clubcard").Value = cc.ClubcardNumber
                Else
                    Command.Parameters.Add("@Clubcard", SqlDbType.VarChar).Value = cc.ClubcardNumber
                End If

                Command.Connection.Open()
                Command.ExecuteScalar()
                Command.Connection.Close()

            Next


            Command = Nothing
        Catch ex As Exception
            cUtilities.WriteEventLogEntry(eEventLogSource.NGCMessage, "callAuthorise.vb", "cSaveNGC - SaveNGCHH: " & ex.ToString, EventLogEntryType.Error)
            Throw ex
        End Try

    End Sub 'SaveNGCTiming()
End Class