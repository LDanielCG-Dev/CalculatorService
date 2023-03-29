namespace CalculatorService.ServerAPI.Controllers
{
	public class Subtraction
	{
		public double Calculate(double minuend, double subtrahend)
		{
			var result = minuend - subtrahend;

			return Math.Round(result,3);
		}
	}
}