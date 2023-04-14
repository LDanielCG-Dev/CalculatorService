namespace LoggerService
{
	public interface ILoggerManager
	{
		public void LogError(string message);
		public void LogWarning(string message);
		public void LogDebug(string message);
		public void LogInfo(string message);
	}
}
