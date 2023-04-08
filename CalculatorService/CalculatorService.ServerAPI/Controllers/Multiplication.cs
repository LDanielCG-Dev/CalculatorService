using CalculatorService.Models;

namespace CalculatorService.ServerAPI.Controllers
{
	public static class Multiplication
	{
		public static double Calculate(this MultiplicationRequest @this)
		{
			var result = @this.Factors[0];
			for (var i = 1; i < @this.Factors.Count(); i++)
			{
				result *= @this.Factors[i];
			}
			return Math.Round(result, 3);
		}

		public static bool IsValid(this MultiplicationRequest @this)
		{
			if(@this == null || @this.Factors.Count() < 2)
			{
				return false;
			}

			const int MAX_DIGITS = 9;
			foreach (var factor in @this.Factors)
			{
				if (factor.ToString().Length > MAX_DIGITS || !double.TryParse(factor.ToString(), out var multParse))
				{
					return false;
				}
			}

			return true;
		}

		public static bool IsEmpty(this MultiplicationRequest @this)
			=> @this.GetType().GetProperties().All(prop => prop.GetValue(@this) == null || (prop.PropertyType == typeof(int?) && (int?)prop.GetValue(@this) == 0));
	}
}
