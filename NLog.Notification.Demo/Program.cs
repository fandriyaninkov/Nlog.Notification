using System;

namespace NLog.Notification.Demo
{
    using Extensions.Logging;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using NLog.Config;
    using NLog.Notification.TelegramNotification;
    using NLog.Targets;

    class Program
    {
        static void Main(string[] args)
        {
            Target.Register<TelegramTarget>("TelegramNotification");
            // var telega = new TelegramTarget();
            var servicesProvider = BuildDi();
            var runner = servicesProvider.GetRequiredService<Runner>();

            for (int i = 0; i < 10; i++)
            {
                runner.DoAction($"Action{i}");
            }
            
            Console.WriteLine("Press ANY key to exit");
            
            Console.ReadLine();
        }

        private static IServiceProvider BuildDi()
        {
            var services = new ServiceCollection();

            //Runner is the custom class
            services.AddTransient<Runner>();

            services.AddSingleton<ILoggerFactory, LoggerFactory>();
            services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
            services.AddLogging((builder) => builder.SetMinimumLevel(LogLevel.Trace));

            var serviceProvider = services.BuildServiceProvider();

            var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();

            //configure NLog
            loggerFactory.AddNLog(new NLogProviderOptions { CaptureMessageTemplates = true, CaptureMessageProperties = true });
            loggerFactory.ConfigureNLog("nlog.config");

            return serviceProvider;
        }
    }
}
