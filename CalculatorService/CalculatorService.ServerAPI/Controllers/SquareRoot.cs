using CalculatorService.Models;
using NLog;

namespace CalculatorService.ServerAPI.Controllers
{
	public static class SquareRoot
	{
		private static readonly NLog.ILogger _logger = LogManager.GetCurrentClassLogger();
		public static double Calculate(this SquareRootRequest @this)
		{
			_logger.Info("Calculating...");
			var result = Math.Sqrt(@this.Number);

			return Math.Round(result, 3);
		}

		public static bool IsValid(this SquareRootRequest @this)
		{
			_logger.Info("Validating...");
			if (@this == null)
			{
				_logger.Error("INVALID: The data is null.");
				return false;
			}

			const int MAX_DIGITS = 9;
			var dataIsNotInt = !int.TryParse(@this.Number.ToString(), out _);
			var dataTooLong = @this.Number.ToString().Length > MAX_DIGITS;
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
