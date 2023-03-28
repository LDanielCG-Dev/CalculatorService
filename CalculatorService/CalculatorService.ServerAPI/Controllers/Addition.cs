using CalculatorService.ServerAPI.Models;
using RestSharp;
using System.Net;
using System.Runtime.CompilerServices;

namespace CalculatorService.ServerAPI.Controllers
{
	public static class Addition
	{
		public static double Calculate(this AdditionRequest @this)
		{
			double result = 0;
			foreach (double operand in @this.Addends.ToArray())
			{
				result += operand;
			}
			return result;
		}

		public static bool IsValid(this AdditionRequest @this)
		{
			const int MAX_DIGITS = 9;
			var operands = @this.Addends.ToArray();
			var allValid = true;

			if (operands.Length >= 2)
			{ 
				foreach (double num in operands)
				{
					if(num.ToString().Length > MAX_DIGITS)
					{
						allValid = false;
					}
				}
			}
			else
			{
				allValid = false;
			}

			return allValid;
		}
	}
}