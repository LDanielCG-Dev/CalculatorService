using CalculatorService.Models;
using LoggerService;

namespace CalculatorService.ServerAPI.Controllers
{
	public static class Division
	{
		private static readonly ILoggerManager _logger = new LoggerManager();
		public static List<int> Calculate(this DivisionRequest @this)
		{
			_logger.LogDebug("Calculating...");
			var quotient = int.Parse(@this.Dividend.ToString()) / int.Parse(@this.Divisor.ToString());
			var remainder = int.Parse(@this.Dividend.ToString()) % int.Parse(@this.Divisor.ToString());

			List<int> result = new()
			{
				quotient,
				remainder
			};

			return result;
		}

		public static bool IsValid(this DivisionRequest @this)
		{
			if (@this == null || @this.Divisor.ToString() == "0")
			{
				return false;
			}

			const int MAX_DIGITS = 9;
			if (@this.Dividend.ToString().Length > MAX_DIGITS || @this.Divisor.ToString().Length > MAX_DIGITS)
			{
				return false;
			}

			return int.TryParse(@this.Dividend.ToString(), out int dividendParsed) && int.TryParse(@this.Divisor.ToString(), out int divisorParsed);
		}

		public static bool IsEmpty(this DivisionRequest @this)
			=> @this.Dividend.ToString() == null || @this.Divisor.ToString() == null || @this.Dividend.ToString() == "0";
	}
}
