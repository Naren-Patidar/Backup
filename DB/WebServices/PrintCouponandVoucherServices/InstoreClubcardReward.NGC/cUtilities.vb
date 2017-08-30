Imports Microsoft.Win32


Public Class cUtilities

    'Public Shared Function GetRegistrySetting(ByVal registryBranch As eRegistryBranch, ByVal strName As String) As String
    '    Dim strRegistrySetting As String
    '    Try
    '        Dim objFreetimeRegistry As RegistryKey = Registry.LocalMachine.OpenSubKey("Software").OpenSubKey("Freetime")

    '        'Open subkey below Freetime (eg, NGC)
    '        objFreetimeRegistry = objFreetimeRegistry.OpenSubKey(registryBranch.ToString)

    '        'Retrieve value for the retistry setting required
    '        strRegistrySetting = objFreetimeRegistry.GetValue(strName).ToString

    '        objFreetimeRegistry.Close()
    '    Catch ex As Exception
    '        Throw ex
    '    End Try

    '    Return strRegistrySetting

    'End Function 'GetRegistrySetting

    ' overload used for CSD - get from aplication setting instead 
    Public Shared Function GetRegistrySetting(ByVal name As String) As String

        ' added reference for System.configuration
        If String.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings(name)) Then
            Return ""
        Else
            Return System.Configuration.ConfigurationManager.AppSettings(name)
        End If

    End Function
    Public Enum eRegistryBranch
        Database
        NGC
        OLA
        SMTP
        PAF
    End Enum



    Public Shared Function DisplayCurrency(ByVal intValue As Integer) As String
        Return (intValue / 100).ToString("c")
    End Function

    Public Shared Function DisplayCurrency(ByVal dblValue As Double) As String
        Return dblValue.ToString("c")
    End Function

    Public Shared Function DisplayNBS(ByVal str As String) As String

        If str = "" Then
            Return "&nbsp;"
        Else
            Return str
        End If

    End Function
    Public Shared Function convertNull(ByVal obj As Object, ByVal type As Type) As Object
        If IsDBNull(obj) Then
            Select Case type.FullName
                Case "System.String"
                    Return ""       ' was &nbsp; but this should be handled when displaying
                Case "System.DateTime"
                    Dim dt As New DateTime
                    dt = Nothing
                    Return dt
                Case "System.Integer", "System.Int32", "System.Int64"
                    Return Nothing
                Case "System.Boolean"
                    Return Nothing
                Case Else
                    Return obj
            End Select
        Else
            Return obj
        End If
    End Function


    Public Shared Function trimCCNumber(ByVal ccNum As String) As String
        'Function to trium 00 from the start of clubcard numbers (used for credit cards retrieved from Tesco)
        If ccNum.Substring(0, 2) = "00" Then
            ccNum = ccNum.Substring(2, ccNum.Length - 2)
        End If

        Return ccNum
    End Function


    ' Code lifted from VerifyValidCard.cls (Original Matthew code)
    ' was based on variants so added object and added necessary conversions between strings and integers
    '
    Public Shared Function bCheckDigit(ByVal sClubCardNumber As String) As Boolean
        ' This function verifies the card check digit is correct
        On Error GoTo bCheckDigit_error
        Dim Loop_Count As Integer
        Dim Times_2_Units As Object
        Dim Times_1_Units As Object
        Dim Times_2_Tens As Object
        Dim Running_Total As Object
        Dim Current_Check_Digit As Object
        Dim Calculated_Check_Digit As Object
        Dim bFlag As Boolean

        Running_Total = 0
        bCheckDigit = False
        bFlag = True

        If Not IsNumeric(sClubCardNumber) Then
            ' Remove Spaces
            For Loop_Count = Len(sClubCardNumber) To 1 Step -1
                If Mid(sClubCardNumber, Loop_Count, 1) = " " Then
                    Select Case Loop_Count
                        Case 1
                            sClubCardNumber = Right(sClubCardNumber, Len(sClubCardNumber) - 1)
                        Case Len(sClubCardNumber)
                            sClubCardNumber = Left(sClubCardNumber, Loop_Count - 1)
                        Case Else
                            sClubCardNumber = Left(sClubCardNumber, Loop_Count - 1) & Right(sClubCardNumber, Len(sClubCardNumber) - Loop_Count)
                    End Select
                End If
            Next
            'If the string length is too short, go no further
            If Len(sClubCardNumber) > 15 Then
                'Check For Numerics
                For Loop_Count = Len(sClubCardNumber) To 1 Step -1
                    If Not IsNumeric(Mid(sClubCardNumber, Loop_Count, 1)) Then
                        bFlag = False
                    End If
                Next
            Else
                bFlag = False
            End If
        End If

        If bFlag And Len(sClubCardNumber) > 15 Then
            'Calculate Check Digit
            Current_Check_Digit = Right(sClubCardNumber, 1)
            For Loop_Count = Len(sClubCardNumber) - 1 To 1 Step -2
                Times_2_Units = Mid(sClubCardNumber, Loop_Count, 1)
                Times_2_Units = CInt(Times_2_Units) * 2
                If Len(Times_2_Units) > 1 Then
                    Times_2_Tens = Int(CInt(Times_2_Units) * 10 ^ -1)
                    Times_2_Units = CInt(Times_2_Units) - (CInt(Times_2_Tens) * 10)
                Else
                    Times_2_Tens = 0
                End If
                If Loop_Count > 1 Then Times_1_Units = Mid(sClubCardNumber, Loop_Count - 1, 1) Else Times_1_Units = 0
                Running_Total = CInt(Running_Total) + CInt(Times_2_Units) + CInt(Times_1_Units) + CInt(Times_2_Tens)
            Next


            Calculated_Check_Digit = 10 - CInt(Right(CStr(Running_Total), 1))
            If CInt(Running_Total) <> 0 Then
                If CInt(Calculated_Check_Digit) > 9 Then Calculated_Check_Digit = 0
                If CInt(Current_Check_Digit) = CInt(Calculated_Check_Digit) Then bCheckDigit = True
            End If
        End If

bCheckDigit_exit:
        Exit Function

bCheckDigit_error:
        bCheckDigit = False
        'App.LogEvent ("bCheckDigit : " & Err.Number & ", " & Err.Description & " sClubCardNumber=>" & sClubCardNumber & "<"), 1
        Resume bCheckDigit_exit

    End Function

    Public Shared Sub WriteEventLogEntry(ByVal EventSource As eEventLogSource, ByVal SourceCodeFile As String, ByVal EventText As String, ByVal EventSeverity As EventLogEntryType)

        Try

            Dim objEventLog As New EventLog("Freesale", ".", EventSource.ToString)

            objEventLog.WriteEntry(SourceCodeFile & ": " & EventText, EventSeverity)

        Catch ex As Exception

            Throw ex

        End Try

    End Sub

    Public Enum eEventLogSource
        Callback
        Freesale
        FreesaleClasses
        Giftcard
        OLAMessage
        NGCMessage
    End Enum


End Class

