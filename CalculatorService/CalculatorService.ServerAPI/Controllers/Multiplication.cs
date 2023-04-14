using CalculatorService.Models;
using NLog;

namespace CalculatorService.ServerAPI.Controllers
{
	public static class Multiplication
	{
		private static readonly NLog.ILogger _logger = LogManager.GetCurrentClassLogger();
		public static double Calculate(this MultiplicationRequest @this)
		{
			_logger.Info("Calculating...");
			var result = @this.Factors[0];
			for (var i = 1; i < @this.Factors.Count(); i++)
			{
				result *= @this.Factors[i];
			}
			return Math.Round(result, 3);
		}

		public static bool IsValid(this MultiplicationRequest @this)
		{
			_logger.Info("Validating...");
			if(@this == null || @this.Factors.Count() < 2)
			{
				_logger.Error("INVALID: The data is null or has less than two values.");
				return false;
			}

			const int MAX_DIGITS = 9;
			foreach (var factor in @this.Factors)
			{
				if (factor.ToString().Length > MAX_DIGITS || !double.TryParse(factor.ToString(), out _))
				{
					_logger.Error("INVALID: The number lenght is larger than {0} or is not a digit.", MAX_DIGITS);
					return false;
				}
			}
			_logger.Info("VALID: All the data is valid.");
			return true;
		}
	}
}
