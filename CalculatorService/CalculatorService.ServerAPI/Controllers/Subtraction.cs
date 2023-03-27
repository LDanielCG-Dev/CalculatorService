namespace CalculatorService.ServerAPI.Controllers
{
	public class Subtraction
	{
		public double Calculate(double minuend, double subtrahend)
		{
			double result = minuend - subtrahend;
			
			return result;
		}
	}
}