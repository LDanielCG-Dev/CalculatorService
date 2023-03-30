using CalculatorService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;


namespace CalculatorService.ServerAPI.Controllers
{
	// ipaddress/Calculator
	[Route("[controller]")]
	[ApiController]
	public class CalculatorController : ControllerBase
	{
		string trackingId = "";
		string operation = "";
		string calculation = "";

		// ipaddress/Calculator/add
		[HttpPost("add")]
		public ActionResult<AdditionResponse> Add(AdditionRequest request)
		{
			if (request.IsValid())
			{
				var result = request.Calculate();
				trackingId = HttpContext.Request.Headers["X-Evi-Tracking-Id"];
				operation = "add";
				calculation = String.Join('+',request.Addends.ToArray())+"="+result;
				// save to journal
				Journal.AddRecord(trackingId,operation,calculation);
				// Return result to client
				return AdditionResponse.FromAddition(result);
			}
			else
			{
				var errorKey = "addends";
				var errorMessage = "Invalid input parameters. Make sure you entered at least two numbers and they don't have more than nine digits.";
				return BadRequest(ThrowBadRequest400(errorKey, errorMessage));
			}

			// Change to something like this whenever is ready:
			/*
			 * if (request.notNull()
			 * {
			 *		if(request.IsValid())
			 *		{
			 *			var result = request.Calculate();
			 *			return AdditionResponse.FromAddition(result);
			 *		}
			 *		else
			 *		{
			 *			var modelState = new ModelStateDictionary();
			 *			var errorKey = "addends";
			 *			var errorMessage = "Invalid input parameters. Make sure you entered at least two numbers and they don't have more than nine digits.";
			 *			modelState.AddModelError(errorKey, errorMessage);
			 *
			 *			var badRequest = new CalculatorBadRequest(modelState);
			 *			return BadRequest(badRequest);
			 *		}
			 * }
			 * else
			 * {
			 *		// Error 500
			 * }
			 */


		}
		// ipaddress/Calculator/sub
		[HttpPost("sub")]
		public ActionResult<SubtractionResponse> Subtract(SubtractionRequest request)
		{
			if (request.IsValid())
			{
				var result = request.Calculate();
				trackingId = HttpContext.Request.Headers["X-Evi-Tracking-Id"];
				operation = "sub";
				calculation = request.Minuend + "-" + request.Subtrahend + "=" + result;
				// save to journal
				Journal.AddRecord(trackingId, operation, calculation);
				// Return result to client
				return SubtractionResponse.FromSubtraction(result);
			}
			else
			{
				var errorKey = "subtraction";
				var errorMessage = "Invalid input parameters. Make sure you entered only numbers and not other characters.";
				return BadRequest(ThrowBadRequest400(errorKey, errorMessage));
			}
		}
		// ipaddress/Calculator/mult
		[HttpPost("mult")]
		public ActionResult<MultiplicationResponse> Multiply(MultiplicationRequest request)
		{
			var multiplication = new Multiplication();
			var result = multiplication.Calculate(request.Factors.ToArray());
			return MultiplicationResponse.FromMultiplication(result);
		}
		// ipaddress/Calculator/div
		[HttpPost("div")]
		public ActionResult<DivisionResponse> Division(DivisionRequest request)
		{
			var division = new Division();
			var result = division.Calculate(request.Dividend, request.Divisor);
			return DivisionResponse.FromDivision(result.First(), result.Last());
		}
		// ipaddress/Calculator/sqrt
		[HttpPost("sqrt")]
		public ActionResult<SquareRootResponse> SquareRoot(SquareRootRequest request)
		{
			var squareRoot = new SquareRoot();
			var result = squareRoot.Calculate(request.Number);
			return SquareRootResponse.FromSquareRoot(result);
		}
		// ipaddress/Calculator/journal/query
		[HttpPost("journal/query")]
		public ActionResult<JournalResponse> JournalQuery(JournalRequest request)
		{
			if (request.HasTrackingId())
			{
				var records = Journal.GetRecordsForTrackingId(request.Id);
				return JournalResponse.FromJournal(records);
			}

			return null;
		}

		public static CalculatorBadRequest ThrowBadRequest400(string errorKey, string errorMessage)
		{
			var modelState = new ModelStateDictionary();
			modelState.AddModelError(errorKey, errorMessage);

			return new CalculatorBadRequest(modelState);
		}
	}
}
