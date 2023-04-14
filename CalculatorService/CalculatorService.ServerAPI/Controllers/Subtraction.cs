using CalculatorService.Models;
using NLog;

namespace CalculatorService.ServerAPI.Controllers
{
	public static class Subtraction
	{
		private static readonly NLog.ILogger _logger = LogManager.GetCurrentClassLogger();
		public static double Calculate(this SubtractionRequest @this)
		{
			_logger.Info("Calculating...");
			var result = Double.Parse(@this.Minuend.ToString()) - Double.Parse(@this.Subtrahend.ToString());

			return Math.Round(result,3);
		}

		public static bool IsValid(this SubtractionRequest @this)
		{
			_logger.Info("Validating...");
			if (@this == null || @this.Minuend.ToString() == "" || @this.Subtrahend.ToString() == "")
			{
				_logger.Error("INVALID: The data is null or is empty.");
				return false;
			}

			const int MAX_DIGITS = 9;
			var dataIsNotDouble = !double.TryParse(@this.Minuend.ToString(), out _) || !double.TryParse(@this.Subtrahend.ToString(), out _);
			var dataTooLong = @this.Minuend.ToString().Length > MAX_DIGITS || @this.Subtrahend.ToString().Length > MAX_DIGITS;

			if (dataIsNotDouble || dataTooLong)
			{
				_logger.Error("INVALID: The number lenght is larger than {0} or is not a digit.", MAX_DIGITS);
				return false;
			}
			_logger.Info("VALID: All the data is valid.");
			return true;
		}
	}
}