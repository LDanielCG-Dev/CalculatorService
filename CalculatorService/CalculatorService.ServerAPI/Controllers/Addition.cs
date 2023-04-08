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
			if (@this == null || @this.Addends.Count() < 2)
			{
				return false;
			}

			const int MAX_DIGITS = 9;
			foreach (var num in @this.Addends)
			{
				if (num.ToString().Length > MAX_DIGITS || !double.TryParse(num.ToString(), out var addParsed))
				{
					return false;
				}
			}

			return true;
		}

		public static bool IsEmpty(this AdditionRequest @this)
			=> @this.GetType().GetProperties().All(prop => prop.GetValue(@this) == null || (prop.PropertyType == typeof(int?) && (int?)prop.GetValue(@this) == 0));
	}
}