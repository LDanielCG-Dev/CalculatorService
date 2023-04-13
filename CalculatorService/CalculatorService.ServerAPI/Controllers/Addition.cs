using CalculatorService.Models;
using LoggerService;

namespace CalculatorService.ServerAPI.Controllers
{
	public static class Addition
	{
		private static readonly ILoggerManager _logger = new LoggerManager();
		public static double Calculate(this AdditionRequest @this)
		{
			_logger.LogInfo("Calculating...");
			var result = 0.0;
			foreach (var operand in @this.Addends.ToArray())
			{
				result += operand;
			}
			return Math.Round(result, 3);
		}

		public static bool IsValid(this AdditionRequest @this)
		{
			_logger.LogInfo("Validating...");
			if (@this == null || @this.Addends.Count() < 2)
			{
				_logger.LogError("INVALID: The data is null or has less than two values.");
				return false;
			}

			const int MAX_DIGITS = 9;
			foreach (var num in @this.Addends)
			{
				if (num.ToString().Length > MAX_DIGITS || !double.TryParse(num.ToString(), out double addParsed))
				{
					_logger.LogError($"INVALID: The number lenght is larger than {MAX_DIGITS} or is not a digit.");
					return false;
				}
			}
			_logger.LogInfo("VALID: All the data is valid.");
			return true;
		}

		public static bool IsEmpty(this AdditionRequest @this)
			=> @this.GetType().GetProperties().All(prop => prop.GetValue(@this) == null || (prop.PropertyType == typeof(int?) && (int?)prop.GetValue(@this) == 0));
	}
}