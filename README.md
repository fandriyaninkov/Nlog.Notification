# Nlog.Notification

# Getting started

  - Create Telegram bot with help [BotFather](https://t.me/botfather) and get bot token
  - Install as NuGet package: [Install-Package NLog.Notification](https://www.nuget.org/packages/NLog.Notification/)
  - Add Nlog target "TelegramNotification" to nlog.config 
  ``` xml
    <targets>
    ...
    <target name="target3" xsi:type="TelegramNotification" Token="YOUR_BOT_TOKEN" ChatId="YOUR_CHAT_ID"/>
  </targets>
```

- Add target to nlog rules
``` xml
  <rules>
    ...
    <logger name="*" minlevel="Error" writeTo="target3"/>
  </rules>
```
- Register manually extension. Do this a soon as possible, e.g. main(), or app_start
``` c#
Target.Register<TelegramTarget>("TelegramNotification");
```

# Example of notification

![alt text](https://github.com/schlawiner92/Nlog.Notification/blob/master/readme_result.png)
