namespace CalculatorService.ServerAPI.Controllers
{
	public class Division
	{
		public List<int> Calculate(int dividend, int divisor)
		{
			int quotient = dividend / divisor;
			int remainder = dividend % divisor;

			List<int> result = new List<int>
			{
				quotient,
				remainder
			};

			return result;
		}
	}
}
