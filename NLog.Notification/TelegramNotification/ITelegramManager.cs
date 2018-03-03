namespace NLog.Notification.TelegramNotification
{
    using Message;

    internal interface ITelegramManager
    {
        void SendLogInfoAsync(LogMessage logMessage);
        void SendErrorInfo(ErrorMessage errorMessage);
    }
}