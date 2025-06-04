# RmoSignalR-PoC

**Proof of Concept für ein Online-Benachrichtigungssystem mit SignalR**

---

## Projektübersicht

Dieses Repository enthält den Proof-of-Concept (PoC) „RmoSignalR-PoC“ der RA-MICRO Software AG. Ziel des Projekts ist es, die Machbarkeit einer Echtzeit-Push-Kommunikation mittels SignalR zu demonstrieren. Es besteht aus drei Hauptkomponenten:

1. **NotificationHubHost** (SignalR-Hub-Host)
2. **RmoSignalR-PoC.Web** (Admin-Frontend, ASP.NET Core MVC)
3. **RmoSignalRClientApp** (Windows-Forms-Client in VB.NET)

Das System ermöglicht Administratoren über eine Web-UI, Benachrichtigungen zu erstellen und in Echtzeit an alle verbundenen Desktop-Clients zu senden. Verspätet angemeldete Clients können per „Load History“ verpasste Nachrichten nachladen.

---

## Ordnerstruktur

/
├── NotificationHubHost/
│ ├── Program.cs
│ ├── NotificationHub.cs
│ ├── AppDbContext.cs
│ ├── Migrations/
│ └── appsettings.json
│
├── RmoSignalR-PoC.Web/
│ ├── Controllers/
│ │ └── NotificationController.cs
│ ├── Views/
│ │ └── Notification/
│ │ └── Index.cshtml
│ ├── wwwroot/
│ ├── Program.cs
│ ├── appsettings.json
│ └── RmoSignalR-PoC.Web.csproj
│
├── RmoSignalRClientApp/
│ ├── Form1.vb
│ ├── NotificationDto.vb
│ ├── App.config
│ └── RmoSignalRClientApp.vbproj
│
├── README.md


yaml
Kopieren
Bearbeiten

---

## Voraussetzungen

- **.NET 9 SDK** (empfohlen: neueste Version)
- **Visual Studio 2022** (oder höher) mit Unterstützung für:
  - ASP.NET Core-Webanwendungen
  - VB.NET Windows-Forms-Anwendungen
- **SQL Server** (z. B. LocalDB) oder ein SQL-Server-Instance, auf den sowohl Hub als auch Web-UI zugreifen können
- **Internetverbindung** (nur zum Herunterladen von NuGet-Paketen)

---

## Installation & Einrichtung

### 1. Repository klonen

```bash
git clone https://github.com/DEIN_USERNAME/RmoSignalR-PoC.git
cd RmoSignalR-PoC
2. Datenbankverbindung konfigurieren
Öffne in beiden Projekten (NotificationHubHost/appsettings.json und RmoSignalR-PoC.Web/appsettings.json) die ConnectionString-Sektion und passe sie an deine SQL-Server-Umgebung an. Beispiel:

jsonc
Kopieren
Bearbeiten
// appsettings.json (NotificationHubHost & RmoSignalR-PoC.Web)
{
  "ConnectionStrings": {
    "NotificationDb": "Server=(localdb)\\mssqllocaldb;Database=NotificationDb;Trusted_Connection=True;"
  },
  "HubUrl": "https://localhost:5002/notificationHub"
}
Hinweis: HubUrl muss im Admin-Frontend (RmoSignalR-PoC.Web) auf den laufenden SignalR-Hub verweisen (Standard: https://localhost:5002/notificationHub).

3. Migrationen anwenden (NotificationHubHost)
Öffne die Developer Command Prompt oder ein Terminal in Visual Studio.

Wechsle in den Ordner NotificationHubHost:

bash
Kopieren
Bearbeiten
cd NotificationHubHost
Erstelle die Datenbank und wende Migrationen an:

bash
Kopieren
Bearbeiten
dotnet ef database update
Damit werden die Tabellen (Notifications) in der konfigurierten SQL-Datenbank angelegt.

Komponenten & Ausführen
A) NotificationHubHost (SignalR-Hub)
Öffne das Projekt in Visual Studio (Ordner NotificationHubHost).

Stelle sicher, dass die ConnectionString-Einstellungen korrekt sind.

Starte das Projekt im Debug- oder Release-Modus:

In Visual Studio: Rechtsklick auf NotificationHubHost → Debug starten.

Im Terminal:

bash
Kopieren
Bearbeiten
cd NotificationHubHost
dotnet run
Der Hub hört standardmäßig auf https://localhost:5002/notificationHub.

B) RmoSignalR-PoC.Web (Admin-Frontend)
Öffne das Projekt in Visual Studio (Ordner RmoSignalR-PoC.Web).

Überprüfe appsettings.json auf korrekten NotificationDb-ConnectionString und HubUrl.

Starte das Projekt:

In Visual Studio: Rechtsklick auf RmoSignalR-PoC.Web → Debug starten.

Im Terminal:

bash
Kopieren
Bearbeiten
cd RmoSignalR-PoC.Web
dotnet run
Die Web-UI ist erreichbar unter https://localhost:5001 (Standard-Ports können abweichen).

Funktion:

Wähle eine Kategorie (z. B. „Updates“, „Neue Gesetze“, „Neuigkeiten“).

Gib einen Nachrichtentext ein.

Klicke auf Senden → Die Nachricht wird in die Datenbank geschrieben und sofort an alle verbundenen Desktop-Clients gesendet.

Darunter erscheint eine Tabelle der zuletzt gesendeten Nachrichten.

C) RmoSignalRClientApp (VB.NET Windows Forms)
Öffne das Projekt in Visual Studio (Ordner RmoSignalRClientApp).

Passe in App.config die HubUrl an, falls nötig (Standard: https://localhost:5002/notificationHub).

Starte die Anwendung:

In Visual Studio: Rechtsklick auf RmoSignalRClientApp → Debug starten.

Funktion:

Beim Start versucht die App automatisch, eine Verbindung zum Hub herzustellen.

Das Statuspanel (oberer Bereich) zeigt „Grün“ (verbunden) oder „Rot“ (getrennt).

Eintreffende Nachrichten erscheinen sofort in der ListBox im Format:

makefile
Kopieren
Bearbeiten
HH:mm [Kategorie] Benutzer: Nachrichtentext
Bei Verbindungsunterbrechung wechselt das Panel auf Rot. Nach Wiederherstellung (Reconnected) lädt die App per GetRecentNotifications alle verpassten Nachrichten (bis zu 10) automatisch nach.

Über den History-Button können jederzeit die letzten 10 Einträge manuell abgerufen werden.

Verwendete Technologien
.NET 9

ASP.NET Core MVC (Admin-Frontend)

SignalR (ASP.NET Core) (Hub & Clients)

Entity Framework Core (EF Core) mit SQL Server

VB.NET Windows Forms (Desktop-Client)

xUnit (Unit-Tests für Hub-Methoden)

Bootstrap 5 (grundlegendes Styling der Web-UI)

Konfiguration
Ports

Standardmäßig läuft das Hub-Host auf https://localhost:5002.

Die Web-UI ist unter https://localhost:5001 verfügbar.

CORS

Im Hub-Host ist CORS so konfiguriert, dass Anfragen von https://localhost:5001 (Admin-UI) erlaubt sind. Andernfalls schlägt die Verbindung fehl.

Appsettings

NotificationHubHost/appsettings.json und RmoSignalR-PoC.Web/appsettings.json enthalten jeweils den ConnectionString und den HubUrl-Eintrag.

Beispiel:

jsonc
Kopieren
Bearbeiten
{
  "ConnectionStrings": {
    "NotificationDb": "Server=(localdb)\\mssqllocaldb;Database=NotificationDb;Trusted_Connection=True;"
  },
  "HubUrl": "https://localhost:5002/notificationHub"
}
Projektstruktur im Überblick
NotificationHubHost

Program.cs: Konfiguriert WebHost, CORS und Hub-Mapping.

NotificationHub.cs: SignalR-Hub mit Methoden

BroadcastNotification(user, text, category)

GetRecentNotifications(count)

AppDbContext.cs: EF Core-DbContext mit NotificationEntity.

Migrations/: EF Core-Migrationen (InitialCreate, AddNotificationCategory).

RmoSignalR-PoC.Web

Controllers/NotificationController.cs: Annahme von POST-Formulardaten, speichert Nachricht über EF Core, ruft Hub-Methode auf.

Views/Notification/Index.cshtml: Formular (Kategorie, Text), Tabelle der letzten Nachrichten, Statusanzeige.

Program.cs: Registriert MVC, SignalR-Client-Abhängigkeiten.

RmoSignalRClientApp

Form1.vb:

Aufbau der HubConnection

Event-Handler für

ReceiveNotification → Aktualisierung der ListBox

Closed, Reconnecting, Reconnected → Statuspanel aktualisieren; History-Nachrichten nachladen

GetRecentNotificationsAsync() für manuellen bzw. automatischen Nachladen.

NotificationDto.vb: Definition des Datenübertragungsobjekts (User, Text, Category, Timestamp).

Nutzung & Ablauf
Hub-Host starten

Legt die SignalR-Endpunkte und Datenbanktabellen an.

Admin-UI (Web) starten

Erzeugt neue Benachrichtigungen, sendet sie per SignalR an verbundenen Hub.

Speichert Objekte in der SQL-Datenbank.

Client-App starten

Stellt permanente WebSocket-Verbindung zum Hub her.

Empfängt „Live“-Nachrichten und zeigt sie direkt an.

Lädt beim Wiederverbinden (oder über History-Button) verpasste Nachrichten der letzten 10 Einträge automatisch nach.

Lizenz
Dieses Projekt ist lizenziert unter der MIT-Lizenz. Details siehe LICENSE.

Autor
Anas Sharbaji
Fachinformatiker Anwendungsentwicklung – RA-MICRO Software AG
E-Mail: anas.sharbaji@example.com

Weiterführende Links & Dokumentation
Original-Projektdokumentation (PDF/Word): Siehe /docs/AbschlussprojektDoku 2025.docx

SignalR-Dokumentation:

Hub-Entwicklung: https://docs.microsoft.com/aspnet/core/signalr/hubs

Client-Integration: https://docs.microsoft.com/aspnet/core/signalr/dotnet-client

EF Core Migrationen: https://docs.microsoft.com/ef/core/managing-schemas/migrations

Hinweis: Diese README-Datei liefert eine kompakte Übersicht zum PoC „RmoSignalR-PoC“. Für detaillierte Architekturdiagramme, Testprotokolle und Benutzer- bzw. Entwicklerhandbuch siehe die Anhänge in der Projektdokumentation. ```
