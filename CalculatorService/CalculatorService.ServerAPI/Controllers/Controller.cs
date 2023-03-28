using CalculatorService.ServerAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CalculatorService.ServerAPI.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class CalculatorController : ControllerBase
	{
		[HttpPost("add")]
		public ActionResult<AdditionResponse> Add(AdditionRequest request)
		{
			if (request.IsValid())
			{
				var result = request.Calculate();
				return AdditionResponse.FromAddition(result);
			}
			else
			{
				var modelState = new ModelStateDictionary();
				var errorKey = "addends";
				var errorMessage = "Invalid input parameters. Make sure you entered at least two numbers and they don't have more than nine digits.";
				modelState.AddModelError(errorKey, errorMessage);
			
				var badRequest = new CalculatorBadRequest(modelState);
				return BadRequest(badRequest);
			}
		}

		[HttpPost("sub")]
		public ActionResult<SubtractionResponse> Subtract(SubtractionRequest request)
		{
			var subtraction = new Subtraction();
			var result = subtraction.Calculate(request.Minuend,request.Subtrahend);
			return SubtractionResponse.FromSubtraction(result);
		}

		[HttpPost("mult")]
		public ActionResult<MultiplicationResponse> Multiply(MultiplicationRequest request) 
		{
			var multiplication = new Multiplication();
			var result = multiplication.Calculate(request.Factors.ToArray());
			return MultiplicationResponse.FromMultiplication(result);
		}

		[HttpPost("div")]
		public ActionResult<DivisionResponse> Division(DivisionRequest request) 
		{
			var division = new Division();
			var result = division.Calculate(request.Dividend, request.Divisor);
			return DivisionResponse.FromDivision(result.First(), result.Last());
		}

		[HttpPost("sqrt")]
		public ActionResult<SquareRootResponse> SquareRoot(SquareRootRequest request)
		{
			var squareRoot = new SquareRoot();
			var result = squareRoot.Calculate(request.Number);
			return SquareRootResponse.FromSquareRoot(result);
		}
	}
}
