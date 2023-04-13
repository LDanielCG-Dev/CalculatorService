using CalculatorService.Models;
using LoggerService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using NLog;


namespace CalculatorService.ServerAPI.Controllers
{
	// ipaddress/Calculator
	[Route("[controller]")]
	[ApiController]
	[NullExceptionFilter]
	public class CalculatorController : ControllerBase
	{
		private string trackingId = "";
		private string operation = "";
		private string calculation = "";
		private const string trackingIdName = "X-Evi-Tracking-Id";

		private static readonly ILoggerManager _logger = new LoggerManager();

		// ipaddress/Calculator/add
		[HttpPost("add")]
		public ActionResult<AdditionResponse> Add(AdditionRequest request)
		{
			operation = "add";
			_logger.LogInfo($"Recieved request for operation: {operation}.");

			// Check if the request is null or empty and returns a BadRequest 400 response
			if (request == null || request.IsEmpty())
			{
				var errorKey = "addends";
				return BadRequest(ThrowBadRequest400(errorKey));
			}

			if (request.IsValid() & !string.IsNullOrEmpty(HttpContext.Request.Headers[trackingIdName]))
			{
				trackingId = HttpContext.Request.Headers[trackingIdName];

				calculation = String.Join('+', request.Addends.ToArray());

				var result = request.Calculate();
				_logger.LogInfo($"The result of {calculation} is {result}.");

				// save to journal
				Journal.AddRecord(trackingId, operation, calculation + "=" + result);
				// Return result to client
				return AdditionResponse.FromAddition(result);
			}
			else
			{
				var errorKey = "addends";
				var errorMessage = "Missing tracking id or invalid input parameters. Make sure you entered at least two numbers.";
				return BadRequest(ThrowBadRequest400(errorKey, errorMessage));
			}

		}
		// ipaddress/Calculator/sub
		[HttpPost("sub")]
		public ActionResult<SubtractionResponse> Subtract(SubtractionRequest request)
		{
			operation = "sub";
			_logger.LogInfo($"Recieved request for operation: {operation}.");

			// Check if the request is null or empty and returns a BadRequest 400 response
			if (request == null || request.IsEmpty())
			{
				var errorKey = "subtraction";
				return BadRequest(ThrowBadRequest400(errorKey));
			}

			if (request.IsValid() & !string.IsNullOrEmpty(HttpContext.Request.Headers[trackingIdName]))
			{
				trackingId = HttpContext.Request.Headers[trackingIdName];

				calculation = request.Minuend + "-" + request.Subtrahend;

				var result = request.Calculate();
				_logger.LogInfo($"The result of {calculation} is {result}.");

				// save to journal
				Journal.AddRecord(trackingId, operation, calculation + "=" + result);
				// Return result to client
				return SubtractionResponse.FromSubtraction(result);
			}
			else
			{
				var errorKey = "subtraction";
				var errorMessage = "Missing tracking id or invalid input parameters. Make sure you entered only numbers and not other characters.";
				return BadRequest(ThrowBadRequest400(errorKey, errorMessage));
			}
		}
		// ipaddress/Calculator/mult
		[HttpPost("mult")]
		public ActionResult<MultiplicationResponse> Multiply(MultiplicationRequest request)
		{
			operation = "mult";
			_logger.LogInfo($"Recieved request for operation: {operation}.");

			// Check if the request is null or empty and returns a BadRequest 400 response
			if (request == null || request.IsEmpty())
			{
				var errorKey = "multiplication";
				return BadRequest(ThrowBadRequest400(errorKey));
			}

			if (request.IsValid() & !string.IsNullOrEmpty(HttpContext.Request.Headers[trackingIdName]))
			{
				trackingId = HttpContext.Request.Headers[trackingIdName];

				calculation = String.Join('*', request.Factors.ToArray());

				var result = request.Calculate();
				_logger.LogInfo($"The result of {calculation} is {result}.");

				// Save to journal
				Journal.AddRecord(trackingId, operation, calculation + "=" + result);
				// Return result to client
				return MultiplicationResponse.FromMultiplication(result);
			}
			else
			{
				var errorKey = "multiplication";
				var errorMessage = "Missing tracking id or invalid input parameters. Make sure you entered at least two numbers.";
				return BadRequest(ThrowBadRequest400(errorKey, errorMessage));
			}
		}
		// ipaddress/Calculator/div
		[HttpPost("div")]
		public ActionResult<DivisionResponse> Division(DivisionRequest request)
		{
			operation = "div";
			_logger.LogInfo($"Recieved request for operation: {operation}.");

			// Check if the request is null or empty and returns a BadRequest 400 response
			if (request == null || request.IsEmpty())
			{
				var errorKey = "division";
				return BadRequest(ThrowBadRequest400(errorKey));
			}

			if (request.IsValid() & !string.IsNullOrEmpty(HttpContext.Request.Headers[trackingIdName]))
			{
				trackingId = HttpContext.Request.Headers[trackingIdName];

				calculation = request.Dividend + "/" + request.Divisor;

				var result = request.Calculate();
				_logger.LogInfo($"The result of {calculation} is {result}.");

				// Save to journal
				Journal.AddRecord(trackingId, operation, calculation+"=" + " { Quotient:" + result.ToArray()[0] + ", Remainder: " + result.ToArray()[1] + " }");
				// Return result to client
				return DivisionResponse.FromDivision(result.First(), result.Last());
			}
			else
			{
				var errorKey = "division";
				var errorMessage = "Missing tracking id or invalid input parameters. Make sure you entered integers and you didn't enter a zero as the divisor!.";
				return BadRequest(ThrowBadRequest400(errorKey, errorMessage));
			}
		}
		// ipaddress/Calculator/sqrt
		[HttpPost("sqrt")]
		public ActionResult<SquareRootResponse> SquareRoot(SquareRootRequest request)
		{
			operation = "sqrt";
			_logger.LogInfo($"Recieved request for operation: {operation}.");

			// Check if the request is null or empty and returns an BadRequest 400 response
			if (request == null || request.IsEmpty())
			{
				var errorKey = "squareRoot";
				return BadRequest(ThrowBadRequest400(errorKey));
			}

			if (request.IsValid() & !string.IsNullOrEmpty(HttpContext.Request.Headers[trackingIdName]))
			{
				trackingId = HttpContext.Request.Headers[trackingIdName];

				calculation = "√" + request.Number;

				var result = request.Calculate();
				_logger.LogInfo($"The result of {calculation} is {result}.");

				// Save to journal
				Journal.AddRecord(trackingId, operation, calculation+"="+result);
				// Return result to client
				return SquareRootResponse.FromSquareRoot(result);
			}
			else
			{
				var errorKey = "squareRoot";
				var errorMessage = "Missing tracking id or invalid input parameters. Make sure you entered only numbers and not other characters.";
				return BadRequest(ThrowBadRequest400(errorKey, errorMessage));
			}
		}
		// ipaddress/Calculator/journal/query
		[HttpPost("journal/query")]
		public ActionResult<JournalResponse> JournalQuery(JournalRequest request)
		{
			_logger.LogInfo($"Recieved request for operation: Journal.");

			// Check if the request is null or empty and returns an InternalServerError 500 response
			if (request == null || request.IsEmpty())
			{
				var errorKey = "journal";
				return BadRequest(ThrowBadRequest400(errorKey));
			}

			if (request.HasTrackingId())
			{
				var records = Journal.GetRecordsForTrackingId(request.Id);
				if(records.Count > 1)
				{
					return JournalResponse.FromJournal(records);
				}
			}

			return new ObjectResult(new JournalResponse { Operations = new List<JournalOperation>(), Message = "The journal is currently empty." });
		}
		public static CalculatorBadRequest ThrowBadRequest400(string errorKey, string errorMessage = "The request that was sent is not valid, is null or it's empty.")
		{
			_logger.LogError($"ERROR 400: BadRequest. {errorMessage}");

			var modelState = new ModelStateDictionary();
			modelState.AddModelError(errorKey, errorMessage);
			return new CalculatorBadRequest(modelState);
		}

	}
}
