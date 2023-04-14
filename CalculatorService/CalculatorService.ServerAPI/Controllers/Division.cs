using CalculatorService.Models;
using NLog;

namespace CalculatorService.ServerAPI.Controllers
{
	public static class Division
	{
		private static readonly NLog.ILogger _logger = LogManager.GetCurrentClassLogger();
		public static List<int> Calculate(this DivisionRequest @this)
		{
			_logger.Info("Calculating...");
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
			_logger.Info("Validating...");
			if (@this == null || @this.Divisor.ToString() == "0")
			{
				_logger.Error("INVALID: The data is null or the divisor is zero.");
				return false;
			}

			const int MAX_DIGITS = 9;
			var dataIsNotInt = !int.TryParse(@this.Dividend.ToString(), out _) || !int.TryParse(@this.Divisor.ToString(), out _);
			var dataTooLong = @this.Dividend.ToString().Length > MAX_DIGITS || @this.Divisor.ToString().Length > MAX_DIGITS;

			if (dataIsNotInt || dataTooLong)
			{
				_logger.Error("INVALID: The number lenght is larger than {0} or is not a digit.", MAX_DIGITS);
				return false;
			}
			_logger.Info("VALID: All the data is valid.");
			return true;
		}
	}
}
