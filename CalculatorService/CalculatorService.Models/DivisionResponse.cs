using NLog;

namespace CalculatorService.Models
{
	public class DivisionResponse
	{
		private static ILogger _logger = LogManager.GetCurrentClassLogger();

		public int Quotient { get; set; }
		public int Remainder { get; set; }

		public static DivisionResponse FromDivision(int quotient, int remainder)
		{
			_logger.Info("Sending response back.");
			return new DivisionResponse { Quotient = quotient, Remainder = remainder };
		}
		public override string ToString()
		{
			return $"{nameof(Quotient)}: {Quotient}, {nameof(Remainder)}: {Remainder}";
		}
	}

}
