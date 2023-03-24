namespace CalculatorService.ServerAPI.Controllers
{
	public abstract class OperationBase : IOperation
	{
		public abstract double Calculate(double[] operands);
	}
}