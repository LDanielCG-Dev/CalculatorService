namespace CalculatorService.ServerAPI.Controllers
{
	public class Addition //: OperationBase
	{
		public /*override*/ double Calculate(double[] operands)
		{
			double result = 0;
			foreach (double operand in operands)
			{
				result += operand;
			}
			return result;
		}
	}
}