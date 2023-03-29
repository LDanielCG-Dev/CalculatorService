namespace CalculatorService.ServerAPI.Controllers
{
	public class SquareRoot
	{
		public double Calculate(double number)
		{
			var result = Math.Sqrt(number);

			return Math.Round(result, 3);
		}
	}
}
