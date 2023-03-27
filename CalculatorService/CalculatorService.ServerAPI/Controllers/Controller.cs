using CalculatorService.ServerAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace CalculatorService.ServerAPI.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class CalculatorServiceController : ControllerBase
	{
		[HttpPost("add")]
		public ActionResult<AdditionResponse> Add(AdditionRequest request)
		{
			var addition = new Addition();
			var result = addition.Calculate(request.Addends.ToArray());
			return AdditionResponse.FromAddition(result);
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
			return DivisionResponse.FromDivision(result.ToArray()[0], result.ToArray()[1]);
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
