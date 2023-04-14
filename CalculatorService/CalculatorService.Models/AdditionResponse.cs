using NLog;

namespace CalculatorService.Models
{
	public class AdditionResponse
	{
		private static ILogger _logger = LogManager.GetCurrentClassLogger();
		public double Sum { get; set; }

		public static AdditionResponse FromAddition(double sum)
		{
			_logger.Info("Sending response back.");
			return new AdditionResponse { Sum = sum };
		}
		public override string ToString()
		{
			return $"{nameof(Sum)}: {Sum}";
		}
	}
}
