namespace NLog.Notification.TelegramNotification
{
    using Config;
    using Message;
    using Targets;

    [Target("TelegramNotification")]
    public class TelegramTarget : TargetWithLayout
    {
        [RequiredParameter]
        public string Token { get; set; }

        [RequiredParameter]
        public string ChatId { get; set; }

        private ITelegramManager _telegramManager;

        protected override void Write(LogEventInfo logEvent)
        {
            CreateTelegramManagerIfNotExist();
            if (logEvent.Level == LogLevel.Error || logEvent.Level == LogLevel.Fatal)
            {
                var errorInfo = GetErrorMessage(logEvent);
                _telegramManager.SendErrorInfo(errorInfo);
            }
            else
            {
                var logMessage = GetLogMessage(logEvent);
                _telegramManager.SendLogInfoAsync(logMessage);
            }
            
        }

        private LogMessage GetLogMessage(LogEventInfo logEvent)
        {
            return new LogMessage {Message = Layout.Render(logEvent)};
        }

        private ErrorMessage GetErrorMessage(LogEventInfo logEvent)
        {
            return new ErrorMessage
            {
                Message = Layout.Render(logEvent),
                StackTrace = logEvent.Exception?.StackTrace
            };
        }

        private void CreateTelegramManagerIfNotExist()
        {
            if (_telegramManager == null)
                _telegramManager = new TelegramManager(Token, ChatId);
        }
    }
}