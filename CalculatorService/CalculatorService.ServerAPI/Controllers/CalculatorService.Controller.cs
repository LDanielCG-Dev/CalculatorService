using CalculatorService.ServerAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CalculatorService.ServerAPI.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class CalculatorServiceController : ControllerBase
	{
		[HttpPost("Addition")]
		public ActionResult<AdditionResponse> Add(AdditionRequest request)
		{
			var addition = new Addition();
			double result = addition.Calculate(request.Addends.ToArray());
			return AdditionResponse.FromAddition(result);
		}

		[HttpPost("Subtraction")]
		public ActionResult<SubtractionResponse> Subtract(SubtractionRequest request)
		{
			var subtraction = new Subtraction();
			double result = subtraction.Calculate(request.Minuend,request.Subtrahend);
			return SubtractionResponse.FromSubtraction(result);
		}

	}
}
