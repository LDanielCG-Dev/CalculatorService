using CalculatorService.Models;

namespace CalculatorService.ServerAPI.Controllers
{
	public static class Subtraction
	{
		public static double Calculate(this SubtractionRequest @this)
		{
			var result = @this.Minuend - @this.Subtrahend;

			return Math.Round(result,3);
		}
		
		public static bool IsValid(this SubtractionRequest @this)
		{
			if (@this != null)
			{
				if (@this.Minuend.GetType() == typeof(double) && @this.Subtrahend.GetType() == typeof(double))
				{
					const int MAX_DIGITS = 9;
					if (@this.Minuend.ToString().Length <= MAX_DIGITS && @this.Subtrahend.ToString().Length <= MAX_DIGITS)
					{
						return true;
					}
				}
			}
			return false;
		}

		public static bool IsEmpty(this SubtractionRequest @this)
			=> @this.GetType().GetProperties().All(prop => prop.GetValue(@this) == null || (prop.PropertyType == typeof(int?) && (int?)prop.GetValue(@this) == 0));
	}
}