namespace NLog.Notification.TelegramNotification.Message
{
    internal class ErrorMessage : LogMessage
    {
        public string StackTrace { get; set; }
    }
}