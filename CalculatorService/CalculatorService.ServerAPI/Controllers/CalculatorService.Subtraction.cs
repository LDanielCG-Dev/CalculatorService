namespace CalculatorService.ServerAPI.Controllers
{
	public class Subtraction //: OperationBase
	{
		public /*override*/ double Calculate(double minuend, double subtrahend)
		{
			double result = minuend - subtrahend;
			
			return result;
		}
	}
}