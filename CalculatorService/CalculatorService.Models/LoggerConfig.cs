//using Serilog;
//using Serilog.Formatting.Json;

//namespace CalculatorService.ServerAPI.Controllers
//{
//	public class LoggerConfig
//	{
//		public static Serilog.ILogger Configure()
//		{
//			return new LoggerConfiguration()
//				.MinimumLevel.Debug()
//				.WriteTo.Console()
//				.WriteTo.File(new JsonFormatter(), "logs/log-.json", rollingInterval: RollingInterval.Day)
//				.CreateLogger();
//		}
//	}
//}

//using NLog;
//using NLog.Config;
//using NLog.Layouts;
//using NLog.Targets;

//namespace CalculatorService.Models
//{
//	public class LoggerConfig
//	{
//		public static NLog.ILogger Configure()
//		{
//			var config = new LoggingConfiguration();

//			// Path of logs folder
//			var logDir = "${basedir}/logs";

//			// // Targets where to log to: console and file
//			var consoleTarget = new ColoredConsoleTarget("console")
//			{
//				Layout = new SimpleLayout("${date:format=yyyy-MM-dd HH\\:mm\\:ss} ${uppercase:${level}} ${message}${exception}")
//			};
//			config.AddTarget(consoleTarget);

//			var fileTarget = new FileTarget("file")
//			{
//				FileName = $"{logDir}/log-${{shortdate}}.json",
//				Layout = new JsonLayout()
//			};
//			config.AddTarget(fileTarget);

//			// Rules for mapping loggers to targets
//			config.AddRule(NLog.LogLevel.Debug, NLog.LogLevel.Fatal, consoleTarget);
//			config.AddRule(NLog.LogLevel.Debug, NLog.LogLevel.Fatal, fileTarget);

//			// Apply config
//			LogManager.Configuration = config;

//			// Create and return logger
//			return LogManager.GetCurrentClassLogger();
//		}
//	}
//}


