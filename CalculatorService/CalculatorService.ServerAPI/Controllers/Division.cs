namespace CalculatorService.ServerAPI.Controllers
{
	public class Division
	{
		public List<int> Calculate(int dividend, int divisor)
		{
			var quotient = dividend / divisor;
			var remainder = dividend % divisor;

			List<int> result = new()
			{
				quotient,
				remainder
			};

			return result;
		}
	}
}
