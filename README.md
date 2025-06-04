# RmoSignalR-PoC

**Proof of Concept für ein Online-Benachrichtigungssystem mit SignalR**

---

## Projektübersicht

Dieses Repository enthält den Proof-of-Concept (PoC) „RmoSignalR-PoC“ der RA-MICRO Software AG. Ziel des Projekts ist es, die Machbarkeit einer Echtzeit-Push-Kommunikation mittels SignalR zu demonstrieren. Es besteht aus drei Hauptkomponenten:

1. **NotificationHubHost** (SignalR-Hub-Host)
2. **RmoSignalR-PoC.Web** (Admin-Frontend, ASP.NET Core MVC)
3. **RmoSignalRClientApp** (Windows-Forms-Client in VB.NET)

Das System ermöglicht Administratoren über eine Web-UI, Benachrichtigungen zu erstellen und in Real Time an alle verbundenen Desktop-Clients zu senden. 


## Voraussetzungen

- **.NET 9 SDK** (empfohlen: neueste Version)
- **Visual Studio 2022** (oder höher) mit Unterstützung für:
  - ASP.NET Core-Webanwendungen
  - VB.NET Windows-Forms-Anwendungen
- **SQL Server** (z. B. LocalDB) oder ein SQL-Server-Instance, auf den sowohl Hub als auch Web-UI zugreifen können
- **Internetverbindung** (nur zum Herunterladen von NuGet-Paketen)
