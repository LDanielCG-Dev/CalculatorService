using CalculatorService.Models;
using LoggerService;
using System.Numerics;

namespace CalculatorService.ServerAPI.Controllers
{
	public static class Subtraction
	{
		private static readonly ILoggerManager _logger = new LoggerManager();
		public static double Calculate(this SubtractionRequest @this)
		{
			_logger.LogDebug("Calculating...");
			var result = Double.Parse(@this.Minuend.ToString()) - Double.Parse(@this.Subtrahend.ToString());

			return Math.Round(result,3);
		}

		public static bool IsValid(this SubtractionRequest @this)
		{
			_logger.LogInfo("Validating...");
			if (@this == null || @this.Minuend.ToString() == "" || @this.Subtrahend.ToString() == "")
			{
				_logger.LogError("INVALID: The data is null or is empty.");
				return false;
			}

			const int MAX_DIGITS = 9;
			var dataIsNotDouble = !double.TryParse(@this.Minuend.ToString(), out double minuendParse) || !double.TryParse(@this.Subtrahend.ToString(), out double subtrahendParse);
			var dataTooLong = @this.Minuend.ToString().Length > MAX_DIGITS || @this.Subtrahend.ToString().Length > MAX_DIGITS;

			if (dataIsNotDouble || dataTooLong)
			{
				_logger.LogError($"INVALID: The number lenght is larger than {MAX_DIGITS} or is not a digit.");
				return false;
			}
			_logger.LogInfo("VALID: All the data is valid.");
			return true;
		}

		public static bool IsEmpty(this SubtractionRequest @this)
			=> @this.GetType().GetProperties().All(prop => prop.GetValue(@this) == null || (prop.PropertyType == typeof(int?) && (int?)prop.GetValue(@this) == 0));
	}
}