using LoggerService;

namespace CalculatorService.Models
{
	public class SquareRootResponse
	{
		private static readonly ILoggerManager _logger = new LoggerManager();
		public double Square { get; set; }

		public static SquareRootResponse FromSquareRoot(double square)
		{
			_logger.LogInfo("Sending response back.");
			return new SquareRootResponse { Square = square };
		}
	}
}
