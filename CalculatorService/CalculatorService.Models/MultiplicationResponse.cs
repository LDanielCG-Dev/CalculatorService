using LoggerService;

namespace CalculatorService.Models
{
	public class MultiplicationResponse
	{
		private static readonly ILoggerManager _logger = new LoggerManager();
		public double Product { get; set; }

		public static MultiplicationResponse FromMultiplication(double product)
		{
			_logger.LogInfo("Sending response back.");
			return new MultiplicationResponse { Product = product };
		}
	}
}
