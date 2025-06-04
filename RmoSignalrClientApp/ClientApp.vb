Imports System.Drawing.Drawing2D
Imports Microsoft.AspNetCore.SignalR.Client

Public Class ClientApp

    Private connection As HubConnection


    ''' <summary>
    ''' Setzt die Region des Panels auf eine Ellipse, damit das Panel rund wird.
    ''' </summary>
    Private Sub MakePanelRound()
        ' Lege einen Pfad als Ellipse in der Größe des Panels an
        Using path As New GraphicsPath()
            path.AddEllipse(0, 0, pnlStatus.Width - 1, pnlStatus.Height - 1)
            pnlStatus.Region = New Region(path)
        End Using
    End Sub

    ''' <summary>
    ''' Aktualisiert Farbe und Text des Status-Indikators thread-sicher.
    ''' </summary>
    Private Sub UpdateStatus(newColor As Color, Optional statusText As String = "")
        ' Farbe setzen
        If pnlStatus.InvokeRequired Then
            pnlStatus.Invoke(Sub() pnlStatus.BackColor = newColor)
        Else
            pnlStatus.BackColor = newColor
        End If

        ' Status-Text setzen
        If lblStatusText.InvokeRequired Then
            lblStatusText.Invoke(Sub() lblStatusText.Text = statusText)
        Else
            lblStatusText.Text = statusText
        End If
    End Sub

    ''' <summary>
    ''' Fügt eine neue Zeile in die ListBox ein, thread-sicher.
    ''' </summary>
    Private Sub AddNotification(message As String)
        If lstNotifications.InvokeRequired Then
            lstNotifications.Invoke(Sub() lstNotifications.Items.Add(message))
        Else
            lstNotifications.Items.Add(message)
        End If
    End Sub

    ''' <summary>
    ''' Lädt die Form, richtet die SignalR-Verbindung ein und startet sie.
    ''' </summary>
    Private Async Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Initialen Status auf Rot setzen
        UpdateStatus(Color.Red, "Nicht verbunden")
        MakePanelRound()

        ' Hub-Verbindung erstellen (URL ggf. anpassen)
        connection = New HubConnectionBuilder() _
            .WithUrl("https://localhost:7085/Hubs/NotificationHub") _
            .Build()

        ' Notification-Handler registrieren
        ' Update to match server's 4 parameters (user, text, category, timestamp)
        connection.On(Of String, String, String, DateTime)("ReceiveNotification", AddressOf OnReceiveNotification)

        ' Verbindungs-Events registrieren
        AddHandler connection.Closed, AddressOf Connection_Closed
        AddHandler connection.Reconnecting, AddressOf Connection_Reconnecting
        AddHandler connection.Reconnected, AddressOf Connection_Reconnected

        ' Verbindung starten
        Try
            Await connection.StartAsync()
            UpdateStatus(Color.Green, "Verbunden")

        Catch ex As Exception
            UpdateStatus(Color.Red, "Verbindung fehlgeschlagen")
            MessageBox.Show("Fehler beim Verbinden: " & ex.Message)
        End Try
    End Sub

    ''' <summary>
    ''' Empfängt neue Notifications live vom Hub.
    ''' </summary>
    Private Sub OnReceiveNotification(user As String, text As String, category As String, timestamp As DateTime)
        If lblUpdates.InvokeRequired Then
            lblUpdates.Invoke(Sub() lblUpdates.Text = category)
        Else
            lblUpdates.Text = category
        End If

        ' Use the timestamp sent from the server instead of DateTime.Now
        Dim formattedMessage As String =
            $"{timestamp.ToLocalTime():yyyy-MM-dd HH:mm} {user}: {text}"
        AddNotification(formattedMessage)
    End Sub

    ''' <summary>
    ''' Button-Click: Holt die letzten X Nachrichten nach.
    ''' </summary>
    Private Async Sub BtnLoadHistory_Click(sender As Object, e As EventArgs) _
        Handles BtnLoadHistory.Click

        btnLoadHistory.Enabled = False ' Disable after first click

        Try
            Dim history = Await connection.InvokeAsync(Of List(Of NotificationDto))(
                "GetRecentNotifications", 10)

            For Each n In history
                ' Convert the timestamp to local time before formatting
                AddNotification(
                    $"{n.Timestamp.ToLocalTime():yyyy-MM-dd HH:mm} {n.User}: {n.Text}")
            Next
        Catch ex As Exception
            MessageBox.Show("Fehler beim Laden der Historie: " & ex.Message)
        End Try
    End Sub

    ''' <summary>
    ''' Wird aufgerufen, wenn die Verbindung komplett geschlossen wird.
    ''' </summary>
    Private Function Connection_Closed(ex As Exception) As Task
        UpdateStatus(Color.Red, "Verbindung geschlossen")
        Return Task.CompletedTask
    End Function

    ''' <summary>
    ''' Wird aufgerufen, wenn SignalR versucht, neu zu verbinden.
    ''' </summary>
    Private Function Connection_Reconnecting(ex As Exception) As Task
        UpdateStatus(Color.Red, "Verbindung unterbrochen")
        Return Task.CompletedTask
    End Function

    ''' <summary>
    ''' Wird aufgerufen, wenn die Verbindung erfolgreich wiederhergestellt wurde.
    ''' </summary>
    Private Function Connection_Reconnected(connectionId As String) As Task
        UpdateStatus(Color.Green, "Wieder verbunden")
        Return Task.CompletedTask
    End Function

    ''' <summary>
    ''' Beim Schließen der Form sauber die Verbindung beenden.
    ''' </summary>
    Private Async Sub ClientApp_FormClosing(sender As Object, e As FormClosingEventArgs) _
        Handles MyBase.FormClosing

        If connection IsNot Nothing Then
            Try
                Await connection.StopAsync()
                Await connection.DisposeAsync()
            Catch
                ' Fehler ggf. protokollieren, aber ignorieren
            End Try
        End If
    End Sub


    ''' <summary>
    ''' DTO für die Deserialisierung von nachträglich geladenen Notifications.
    ''' </summary>
    Public Class NotificationDto
        Public Property User As String
        Public Property Text As String
        Public Property Timestamp As DateTime
        Public Property Category As String
    End Class

End Class