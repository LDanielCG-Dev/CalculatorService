namespace CalculatorService.ServerAPI.Controllers
{
	public interface IOperation
	{
		double Calculate(double[] operands);
	}
}
