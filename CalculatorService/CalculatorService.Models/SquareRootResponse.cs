using NLog;

namespace CalculatorService.Models
{
	public class SquareRootResponse
	{
		private static ILogger _logger = LogManager.GetCurrentClassLogger();
		public double Square { get; set; }

		public static SquareRootResponse FromSquareRoot(double square)
		{
			_logger.Info("Sending response back.");
			return new SquareRootResponse { Square = square };
		}
		public override string ToString()
		{
			return $"{nameof(Square)}: {Square}";
		}
	}
}
