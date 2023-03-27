namespace CalculatorService.ServerAPI.Controllers
{
	public abstract class OperationBase
	{
		public abstract double Calculate(double[] operands);
	}
}