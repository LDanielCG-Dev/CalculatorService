using CalculatorService.Models;
using LoggerService;

namespace CalculatorService.ServerAPI.Controllers
{
	public static class SquareRoot
	{
		private static readonly ILoggerManager _logger = new LoggerManager();
		public static double Calculate(this SquareRootRequest @this)
		{
			_logger.LogDebug("Calculating...");
			var result = Math.Sqrt(@this.Number);

			return Math.Round(result, 3);
		}

		public static bool IsValid(this SquareRootRequest @this)
		{
			_logger.LogInfo("Validating...");
			if (@this == null)
			{
				_logger.LogError("INVALID: The data is null.");
				return false;
			}

			const int MAX_DIGITS = 9;
			var dataIsNotInt = !int.TryParse(@this.Number.ToString(), out int numberParsed);
			var dataTooLong = @this.Number.ToString().Length > MAX_DIGITS;
			if (dataIsNotInt || dataTooLong)
			{
				_logger.LogError($"INVALID: The number lenght is larger than {MAX_DIGITS} or is not a digit.");
				return false;
			}
			_logger.LogInfo("VALID: All the data is valid.");
			return true;
		}

		public static bool IsEmpty(this SquareRootRequest @this)
			=> @this == null || @this.Number == null;
	}
}
