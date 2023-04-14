using CalculatorService.Models;
using NLog;

namespace CalculatorService.ServerAPI.Controllers
{
	public static class Addition
	{
		private static readonly NLog.ILogger _logger = LogManager.GetCurrentClassLogger();
		public static double Calculate(this AdditionRequest @this)
		{
			_logger.Info("Calculating...");
			var result = 0.0;
			foreach (var operand in @this.Addends.ToArray())
			{
				result += operand;
			}
			return Math.Round(result, 3);
		}

		public static bool IsValid(this AdditionRequest @this)
		{
			_logger.Info("Validating...");
			if (@this == null || @this.Addends.Count() < 2)
			{
				_logger.Error("INVALID: The data is null or has less than two values.");
				return false;
			}

			const int MAX_DIGITS = 9;
			foreach (var num in @this.Addends)
			{
				if (num.ToString().Length > MAX_DIGITS || !double.TryParse(num.ToString(), out _))
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