using CalculatorService.Models;

namespace CalculatorService.ServerAPI.Controllers
{
	public static class Addition
	{
		public static double Calculate(this AdditionRequest @this)
		{
			var result = 0.0;
			foreach (var operand in @this.Addends.ToArray())
			{
				result += operand;
			}
			return Math.Round(result, 3);
		}

		public static bool IsValid(this AdditionRequest @this)
		{
			const int MAX_DIGITS = 9;
			var operands = @this.Addends.ToArray();
			var allValid = true;

			if (operands.Length >= 2)
			{
				foreach (var num in operands)
				{
					if(num.ToString().Length > MAX_DIGITS || !double.TryParse(num.ToString(), out var addParsed))
					{
						allValid = false;
					}
				}
			}
			else
			{
				allValid = false;
			}

			return allValid;
		}
	}
}