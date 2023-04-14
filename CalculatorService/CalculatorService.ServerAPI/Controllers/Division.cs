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
			_logger.LogInfo("Validating...");
			if (@this == null || @this.Divisor.ToString() == "0")
			{
				_logger.LogError("INVALID: The data is null or the divisor is zero.");
				return false;
			}

			const int MAX_DIGITS = 9;
			var dataIsNotInt = !int.TryParse(@this.Dividend.ToString(), out int dividendParsed) || !int.TryParse(@this.Divisor.ToString(), out int divisorParsed);
			var dataTooLong = @this.Dividend.ToString().Length > MAX_DIGITS || @this.Divisor.ToString().Length > MAX_DIGITS;

			if (dataIsNotInt || dataTooLong)
			{
				_logger.LogError($"INVALID: The number lenght is larger than {MAX_DIGITS} or is not a digit.");
				return false;
			}
			_logger.LogInfo("VALID: All the data is valid.");
			return true;
		}

		public static bool IsEmpty(this DivisionRequest @this)
			=> @this.Dividend.ToString() == null || @this.Divisor.ToString() == null || @this.Dividend.ToString() == "0";
	}
}
