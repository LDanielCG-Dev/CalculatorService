using NLog;

namespace CalculatorService.Models
{
	public class MultiplicationResponse
	{
		private static ILogger _logger = LogManager.GetCurrentClassLogger();
		public double Product { get; set; }

		public static MultiplicationResponse FromMultiplication(double product)
		{
			_logger.Info("Sending response back.");
			return new MultiplicationResponse { Product = product };
		}
		public override string ToString()
		{
			return $"{nameof(Product)}: {Product}";
		}
	}
}
