namespace NLog.Notification.TelegramNotification
{
    using Message;
    using NLog.Notification.Properties;
    using System;
    using System.IO;
    using Telegram.Bot;
    using Telegram.Bot.Types.InputFiles;

    internal class TelegramManager : ITelegramManager
    {
        private readonly string _chatId;
        private readonly ITelegramBotClient _botClient;
        private static Random _random = new Random(); 

        public TelegramManager(string token, string chatId)
        {
            _chatId = chatId;
            _botClient = new TelegramBotClient(token);
        }

        public async void SendLogInfoAsync(LogMessage logMessage)
        {
            await _botClient.SendTextMessageAsync(_chatId, logMessage.Message);
        }

        public void SendErrorInfo(ErrorMessage errorMessage)
        {
            var message = _botClient.SendTextMessageAsync(_chatId, errorMessage.Message).Result;
            var result = _botClient.SendTextMessageAsync(_chatId, errorMessage.StackTrace).Result;
            SendFileImage();
        }

        /// <summary>
        /// Select random file from resource and send to chat
        /// </summary>
        private void SendFileImage()
        {
            var fileName = _random.Next(1, 8);
            var fileStream = (byte[])Resources.ResourceManager.GetObject($"_{fileName}");
            if (fileStream == null) return;
            using (var file = new MemoryStream(fileStream))
            {
                var inputFile = new InputOnlineFile(file);
                var result = _botClient.SendPhotoAsync(_chatId, inputFile).Result;
            }
        }
    }
}