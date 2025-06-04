namespace NotificationHubHost.Models
{
    /// <summary>
    ///Repräsentiert eine Chat-Nachricht in der Datenbank.
    /// </summary>

    public class NotificationEntity
    {
        public int Id { get; set; }
        public string User { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public string Category { get; set; } = string.Empty;
    }
}
