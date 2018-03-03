namespace NLog.Notification.Demo
{
    using System;
    using Microsoft.Extensions.Logging;

    public class Runner
    {
        private readonly ILogger<Runner> _logger;

        public Runner(ILogger<Runner> logger)
        {
            _logger = logger;
        }

        public void DoAction(string name)
        {
            try
            {
                _logger.LogDebug(20, "Doing hard work! {Action}", name);
                Calc(43, 0);
                throw new ArgumentException();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Exception!");
                throw;
            }
           
        }

        public void Calc(int i, int k)
        {
            int m = i / k;
        }
    }
}