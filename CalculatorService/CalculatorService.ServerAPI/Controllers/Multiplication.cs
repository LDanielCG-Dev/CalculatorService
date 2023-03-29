namespace CalculatorService.ServerAPI.Controllers
{
	public class Multiplication
	{
		public double Calculate(double[] operands)
		{
			var result = operands[0];
			for (var i = 1; i < operands.Length; i++)
			{
				result *= operands[i];
			}
			return Math.Round(result, 3);
		}
	}
}
