namespace CalculatorService.ServerAPI.Controllers
{
	public class SquareRoot
	{
		public double Calculate(double number)
		{
			double result = Math.Sqrt(number);
			
			return result;
		}
	}
}
