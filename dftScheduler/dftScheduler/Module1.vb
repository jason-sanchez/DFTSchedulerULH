Imports System.Diagnostics
Imports System.IO

'20140319 - wave3 test version for sysfeed5.
'20140817 - mods for W3 Production.
'20140818 - added file3 to include itwchargesNet.exe

'20150413 - VS2013 version

Module Module1
    Dim processit As Boolean = False
    Dim strLogString As String = ""
    Private fullinipath As String = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory, "..\..\..\Configs\ULH\HL7Mapper.ini")) ' New test
    'Private fullinipath As String = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory, "..\..\..\..\..\..\..\..\..\Configs\ULH\HL7Mapper.ini")) ' local
    Public objIniFile As New INIFile(fullinipath) '20140817 - New Test

    'Dim objIniFile As New INIFile("d:\W3Production\HL7Mapper.ini") '20140817
    Dim file1 As String = objIniFile.GetString("DFT Scheduler", "file1", "(none)")
    Dim file2 As String = objIniFile.GetString("DFT Scheduler", "file2", "(none)")

    Dim file3 As String = objIniFile.GetString("DFT Scheduler", "file3", "(none)") '20140818

    Dim strLogDirectory As String = objIniFile.GetString("Settings", "logs", "(none)") '20140319

    Sub main()
        runProcesses()




    End Sub

    Public Sub writeToLog(ByVal logText As String, ByVal eventType As Integer)

        '20140319 - use a text file to log errors instead of the event log
        Dim file As System.IO.StreamWriter
        Dim strMsg As String = logText
        Dim tempLogFileName As String = strLogDirectory & "DFTScheduler_log.txt"
        file = My.Computer.FileSystem.OpenTextFileWriter(tempLogFileName, True)
        file.WriteLine(DateTime.Now & " : " & strMsg)
        file.Close()

        'Dim myLog As New EventLog()
        'Try
        '' check for the existence of the log that the user wants to create.
        '' Create the source, if it does not already exist.
        'If Not EventLog.SourceExists("DFTScheduler") Then
        'EventLog.CreateEventSource("DFTScheduler", "SchedulerEvents")
        ' End If

        '' Create an EventLog instance and assign its source.

        'myLog.Source = "DFTScheduler"

        '' Write an informational entry to the event log.
        'If eventType = 1 Then
        'myLog.WriteEntry(logText, EventLogEntryType.Error, 1)
        'ElseIf eventType = 2 Then
        'myLog.WriteEntry(logText, EventLogEntryType.Warning, 2)
        'ElseIf eventType = 3 Then
        'myLog.WriteEntry(logText, EventLogEntryType.Information, 3)
        'End If


        'Finally
        'myLog.Close()
        'End Try
    End Sub
    Public Sub runTheProcess(ByVal thefile As String)
        If thefile <> "" Then
            Dim dftProcess As Process = Process.Start(thefile)
            Try

                System.Windows.Forms.Application.DoEvents()
                'Dim splitterProcess As Process = Process.Start("c:\newfeeds\programs\splitter.exe")

                'If Len(txtStatus.Text) > 64000 Then txtStatus.Text = ""
                'txtStatus.AppendText("Starting " & thefile & vbCrLf)
                'System.Windows.Forms.Application.DoEvents()
                dftProcess.WaitForExit()
                'System.Windows.Forms.Application.DoEvents()
                'txtStatus.AppendText("Stopping " & thefile & vbCrLf)

            Catch ex As Exception
                strLogString = strLogString & "Scheduler Error: " & thefile & vbCrLf
                strLogString = strLogString & ex.Message & vbCrLf
                writeToLog(strLogString, 1)
                strLogString = ""

            Finally
                If Not dftProcess.HasExited Then
                    'dftProcess.Finalize()
                    dftProcess.Kill()
                End If

                '20100128 make permanent 1 sec delay
                'If onedelay.Checked Then
                'System.Windows.Forms.Application.DoEvents()
                'txtStatus.AppendText("sleep for 1 sec.")
                System.Threading.Thread.Sleep(1000)
                System.Windows.Forms.Application.DoEvents()
                'txtStatus.AppendText("Awake.")
                'End If

                System.GC.Collect()
                'System.GC.WaitForPendingFinalizers()
                'System.GC.SetProcessWorkingSetSize(Process.GetCurrentProcess().Handle, -1, -1)
            End Try


        End If
    End Sub
    Public Sub runProcesses()
        Try
            runTheProcess(file1)
            runTheProcess(file2)
            runTheProcess(file3) '20140818



        Finally
        End Try
    End Sub
End Module
