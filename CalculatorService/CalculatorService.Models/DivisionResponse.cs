using LoggerService;

namespace CalculatorService.Models
{
	public class DivisionResponse
	{
		private static readonly ILoggerManager _logger = new LoggerManager();
		public int Quotient { get; set; }
		public int Remainder { get; set; }

		public static DivisionResponse FromDivision(int quotient, int remainder)
		{
			_logger.LogInfo("Sending response back.");
			return new DivisionResponse { Quotient = quotient, Remainder = remainder };
		}
	}

}
