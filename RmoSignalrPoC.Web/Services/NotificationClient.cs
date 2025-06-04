using Microsoft.AspNetCore.SignalR.Client;
using System.Threading.Tasks;

namespace RmoSignalrPoC.Web.Services
{
    public class NotificationClient
    {
        private readonly HubConnection _connection;
        private readonly ILogger<NotificationClient> _logger;

        // Public property to expose connection state
        public HubConnectionState ConnectionState => _connection.State;
        
        // Event to notify when connection state changes
        public event Action<HubConnectionState> ConnectionStateChanged;

        public NotificationClient(IConfiguration configuration, ILogger<NotificationClient> logger)
        {
            _logger = logger;
            var hubUrl = configuration["NotificationHubUrl"]
                         ?? "https://localhost:7085/Hubs/NotificationHub";

            _logger.LogInformation($"Connecting to hub at: {hubUrl}");

            _connection = new HubConnectionBuilder()
                .WithUrl(hubUrl)
                .WithAutomaticReconnect()
                .Build();

            // Register for connection state change events
            _connection.Closed += (error) => {
                ConnectionStateChanged?.Invoke(ConnectionState);
                return Task.CompletedTask;
            };
            
            _connection.Reconnecting += (error) => {
                ConnectionStateChanged?.Invoke(ConnectionState);
                return Task.CompletedTask;
            };
            
            _connection.Reconnected += (connectionId) => {
                ConnectionStateChanged?.Invoke(ConnectionState);
                return Task.CompletedTask;
            };

            // Start connection immediately
            ConnectAsync();
        }

        // Make Connect method awaitable and public
        public async Task<bool> ConnectAsync()
        {
            try
            {
                if (_connection.State == HubConnectionState.Disconnected)
                {
                    await _connection.StartAsync();
                    _logger.LogInformation("Connected to SignalR hub successfully");
                    ConnectionStateChanged?.Invoke(ConnectionState);
                    return true;
                }
                return _connection.State == HubConnectionState.Connected;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error connecting to the SignalR hub");
                ConnectionStateChanged?.Invoke(ConnectionState);
                return false;
            }
        }

        public async Task SendNotificationAsync(string user, string text, string category)
        {
            if (_connection.State != HubConnectionState.Connected)
            {
                // Try to connect first if not connected
                await ConnectAsync();
            }
            
            if (_connection.State == HubConnectionState.Connected)
            {
                await _connection.SendAsync("BroadcastNotification", user, text, category);
            }
            else
            {
                _logger.LogWarning("Cannot send notification: hub connection is not in Connected state");
            }
        }
    }
}
