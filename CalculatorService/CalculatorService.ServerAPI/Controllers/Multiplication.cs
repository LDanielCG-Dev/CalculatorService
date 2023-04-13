using CalculatorService.Models;
using LoggerService;

namespace CalculatorService.ServerAPI.Controllers
{
	public static class Multiplication
	{
		private static readonly ILoggerManager _logger = new LoggerManager();
		public static double Calculate(this MultiplicationRequest @this)
		{
			_logger.LogInfo("Calculating...");
			var result = @this.Factors[0];
			for (var i = 1; i < @this.Factors.Count(); i++)
			{
				result *= @this.Factors[i];
			}
			return Math.Round(result, 3);
		}

		public static bool IsValid(this MultiplicationRequest @this)
		{
			_logger.LogInfo("Validating...");
			if(@this == null || @this.Factors.Count() < 2)
			{
				_logger.LogError("INVALID: The data is null or has less than two values.");
				return false;
			}

			const int MAX_DIGITS = 9;
			foreach (var factor in @this.Factors)
			{
				if (factor.ToString().Length > MAX_DIGITS || !double.TryParse(factor.ToString(), out double multParse))
				{
					_logger.LogError($"INVALID: The number lenght is larger than {MAX_DIGITS} or is not a digit.");
					return false;
				}
			}
			_logger.LogInfo("VALID: All the data is valid.");
			return true;
		}

		public static bool IsEmpty(this MultiplicationRequest @this)
			=> @this.GetType().GetProperties().All(prop => prop.GetValue(@this) == null || (prop.PropertyType == typeof(int?) && (int?)prop.GetValue(@this) == 0));
	}
}
