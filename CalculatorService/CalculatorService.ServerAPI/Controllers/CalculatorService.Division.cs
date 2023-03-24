namespace CalculatorService.ServerAPI.Controllers
{
	public class Division : OperationBase
	{
		public override double Calculate(double[] operands)
		{
			if (operands.Length == 0)
			{
				throw new ArgumentException("You must provide at least one operand.");
			}

			double result = operands[0];
			for (int i = 1; i < operands.Length; i++)
			{
				if (operands[i] == 0)
				{
					throw new DivideByZeroException("You can't divide by zero!");
				}
				result /= operands[i];
			}
			return result;
		}
	}
}
