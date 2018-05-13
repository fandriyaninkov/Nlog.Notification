namespace NLog.Notification.TelegramNotification
{
    using System.Net;
    using System.Net.Http;
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

        public string Address { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

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
            {
                if (string.IsNullOrWhiteSpace(Address))
                {
                    _telegramManager = new TelegramManager(Token, ChatId);
                    return;
                }
                var httpClient = GetHttpClient();
                _telegramManager = new TelegramManager(Token, ChatId, httpClient);
            }
                
        }

        private HttpClient GetHttpClient()
        {
            var proxy = new WebProxy(Address, Port);
            if (!string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Password))
            {
                proxy.Credentials = new NetworkCredential(Username, Password);
            }
            var handler = new HttpClientHandler
            {
                Proxy = proxy,
                UseProxy = true
            };
            return new HttpClient(handler);
        }
    }
}