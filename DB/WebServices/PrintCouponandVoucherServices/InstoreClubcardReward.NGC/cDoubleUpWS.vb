Imports InstoreClubcardReward.NGC.NGCDirect
Imports Microsoft.Win32
Imports InstoreClubcardReward.NGC.cUtilities




Public Class cDoubleUpWS


    ' call doubleUpwebservice
    ' this is used for creating and cancelling tokens. Difference is in the xml string passed 
    ' 
    ' follow same format as voucher validation in callAuthorise ie agent and country

    Public Function callService(ByVal xml As String, ByVal agentId As Integer, ByVal Country As String) As String

        Dim du As New DoubleUp.DoubleUpService
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
            'TODO save a link between the send and receive message
            ' save the return message
            cSaveNGC.SaveNGC(agentId, du.Url, response, requestId)
        Catch ex As Exception
            cUtilities.WriteEventLogEntry(eEventLogSource.NGCMessage, "cDoubleUpWS.vb", "callService - DoubleUpRequest- SaveNGC: " & ex.ToString, EventLogEntryType.Error)
        End Try

        ' ################# TIMING OF WEBSERVICE ####################
        Try
            'TODO save a link between the send and receive message
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
