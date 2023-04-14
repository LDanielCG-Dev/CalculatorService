using LoggerService;

namespace CalculatorService.Models
{
	public class AdditionResponse
	{
		private static readonly ILoggerManager _logger = new LoggerManager();
		public double Sum { get; set; }

		public static AdditionResponse FromAddition(double sum)
		{
			_logger.LogInfo("Sending response back.");
			return new AdditionResponse { Sum = sum };
		}
	}
}
