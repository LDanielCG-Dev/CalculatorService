using CalculatorService.ServerAPI.Models;
using RestSharp;

namespace CalculatorService.ServerAPI.Controllers
{
	public class Addition
	{
		public double Calculate(double[] operands)
		{
			double result = 0;
			foreach (double operand in operands)
			{
				result += operand;
			}
			return result;
		}



		//public bool Validate(double[] operands) {
		//
		//}
	}
}