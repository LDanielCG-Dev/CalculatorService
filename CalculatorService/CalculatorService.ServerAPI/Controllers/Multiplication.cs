namespace CalculatorService.ServerAPI.Controllers
{
	public class Multiplication
	{
		public double Calculate(double[] operands)
		{
			double result = operands[0];
			for (int i = 1; i < operands.Length; i++)
			{
				result *= operands[i];
			}
			return result;
		}
	}
}
