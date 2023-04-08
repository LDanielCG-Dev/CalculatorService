using CalculatorService.Models;

namespace CalculatorService.ServerAPI.Controllers
{
	public static class Division
	{
		public static List<int> Calculate(this DivisionRequest @this)
		{
			var quotient = @this.Dividend / @this.Divisor;
			var remainder = @this.Dividend % @this.Divisor;

			List<int> result = new()
			{
				quotient,
				remainder
			};

			return result;
		}

		public static bool IsValid(this DivisionRequest @this)
		{
			if (@this == null || @this.Divisor == 0)
			{
				return false;
			}

			const int MAX_DIGITS = 9;
			if (@this.Dividend.ToString().Length > MAX_DIGITS || @this.Divisor.ToString().Length > MAX_DIGITS)
			{
				return false;
			}

			return int.TryParse(@this.Dividend.ToString(), out var dividendParsed) && int.TryParse(@this.Divisor.ToString(), out var divisorParsed);
		}

		public static bool IsEmpty(this DivisionRequest @this)
			=> @this.Dividend == null || @this.Divisor == null || @this.Dividend == 0;
	}
}
