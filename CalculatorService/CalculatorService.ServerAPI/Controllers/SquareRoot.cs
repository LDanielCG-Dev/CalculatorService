using CalculatorService.Models;

namespace CalculatorService.ServerAPI.Controllers
{
	public static class SquareRoot
	{
		public static double Calculate(this SquareRootRequest @this)
		{
			var result = Math.Sqrt(@this.Number);

			return Math.Round(result, 3);
		}

		public static bool IsValid(this SquareRootRequest @this) 
		{
			if (@this == null)
			{
				return false;
			}

			const int MAX_DIGITS = 9;
			if(@this.Number.ToString().Length > MAX_DIGITS) 
			{
				return false;
			}

			return true;
		}

		public static bool IsEmpty(this SquareRootRequest @this)
			=> @this == null || @this.Number == null;
	}
}
