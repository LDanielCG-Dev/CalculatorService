using NLog;

namespace CalculatorService.Models
{
	public class SubtractionResponse
	{
		private static ILogger _logger = LogManager.GetCurrentClassLogger();
		public double Difference { get; set; }

		public static SubtractionResponse FromSubtraction(double difference)
		{
			_logger.Info("Sending response back.");
			return new SubtractionResponse { Difference = difference };
		}
		public override string ToString()
		{
			return $"{nameof(Difference)}: {Difference}";
		}
	}
}
