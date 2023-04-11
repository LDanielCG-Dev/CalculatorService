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
	public class CalculatorController : ControllerBase
	{
		string trackingId = "";
		string operation = "";
		string calculation = "";
		const string trackingIdName = "X-Evi-Tracking-Id";
		//private static readonly Serilog.ILogger Logger = LoggerConfig.Configure();
		private readonly ILoggerManager _logger;

		public CalculatorController(ILoggerManager logger)
		{
			_logger = logger;
		}

		// ipaddress/Calculator/add
		[HttpPost("add")]
		public ActionResult<AdditionResponse> Add(AdditionRequest request)
		{
			_logger.LogInfo("Operation selected: Add.");

			// Check if the request is null or empty and returns an InternalServerError 500 response
			if (request == null || request.IsEmpty())
			{
				_logger.LogError("ERROR 500: InternalServerError. Request is null or empty.");
				return new ObjectResult(new CalculatorInternalServerError());
			}

			if (request.IsValid())
			{
				trackingId = HttpContext.Request.Headers[trackingIdName];

				// Tracking ID log
				_logger.LogInfo($"TrackingID: {trackingId}.");

				operation = "add";
				calculation = String.Join('+', request.Addends.ToArray());

				// Calculation log
				_logger.LogDebug($"Adding {calculation}...");

				var result = request.Calculate();

				// Result log
				_logger.LogDebug($"The result of {calculation} is {result}.");

				// save to journal
				_logger.LogInfo("Adding operation to journal...");
				Journal.AddRecord(trackingId, operation, calculation+"="+result);
				_logger.LogInfo("Operation added to journal successfully.");
				// Return result to client
				return AdditionResponse.FromAddition(result);
			}
			else
			{
				var errorKey = "addends";
				var errorMessage = "Invalid input parameters. Make sure you entered at least two numbers and they don't have more than nine digits.";
				return BadRequest(ThrowBadRequest400(errorKey, errorMessage));
			}

		}
		// ipaddress/Calculator/sub
		[HttpPost("sub")]
		public ActionResult<SubtractionResponse> Subtract(SubtractionRequest request)
		{
			//Logger.Information("Operation selected: Sub.");

			// Check if the request is null or empty and returns an InternalServerError 500 response
			if (request == null || request.IsEmpty())
			{
				//Logger.Information("ERROR 500: InternalServerError. Request is null or empty.");
				return new ObjectResult(new CalculatorInternalServerError());
			}

			if (request.IsValid())
			{
				trackingId = HttpContext.Request.Headers[trackingIdName];

				// Tracking ID log
				//Logger.Information($"TrackingID: {trackingId}.");

				operation = "sub";
				calculation = request.Minuend + "-" + request.Subtrahend;

				// Calculation log
				//Logger.Information($"Subtracting {calculation}...");

				var result = request.Calculate();

				// Result log
				//Logger.Information($"The result of {calculation} is {result}.");

				// save to journal
				//Logger.Information("Adding operation to journal...");
				Journal.AddRecord(trackingId, operation, calculation+"="+result);
				//Logger.Information("Operation added to journal successfully.");
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
			//Logger.Information("Operation selected: Mult.");

			// Check if the request is null or empty and returns an InternalServerError 500 response
			if (request == null || request.IsEmpty())
			{
				//Logger.Information("ERROR 500: InternalServerError. Request is null or empty.");
				return new ObjectResult(new CalculatorInternalServerError());
			}

			if (request.IsValid())
			{
				trackingId = HttpContext.Request.Headers[trackingIdName];

				// Tracking ID log
				//Logger.Information($"TrackingID: {trackingId}.");

				operation = "mult";
				calculation = String.Join('*', request.Factors.ToArray());

				// Calculation log
				//Logger.Information($"Multiplicating {calculation}...");

				var result = request.Calculate();

				// Result log
				//Logger.Information($"The result of {calculation} is {result}.");

				// Save to journal
				//Logger.Information("Adding operation to journal...");
				Journal.AddRecord(trackingId, operation, calculation+"="+result);
				//Logger.Information("Operation added to journal successfully.");
				// Return result to client
				return MultiplicationResponse.FromMultiplication(result);
			}
			else
			{
				var errorKey = "multiplication";
				var errorMessage = "Invalid input parameters. Make sure you entered at least two numbers and they don't have more than nine digits.";
				return BadRequest(ThrowBadRequest400(errorKey, errorMessage));
			}
		}
		// ipaddress/Calculator/div
		[HttpPost("div")]
		public ActionResult<DivisionResponse> Division(DivisionRequest request)
		{
			//Logger.Information("Operation selected: Div.");

			// Check if the request is null or empty and returns an InternalServerError 500 response
			if (request == null || request.IsEmpty())
			{
				//Logger.Information("ERROR 500: InternalServerError. Request is null or empty.");
				return new ObjectResult(new CalculatorInternalServerError());
			}

			if (request.IsValid())
			{
				trackingId = HttpContext.Request.Headers[trackingIdName];

				// Tracking ID log
				//Logger.Information($"TrackingID: {trackingId}.");

				operation = "div";
				calculation = request.Dividend + "/" + request.Divisor;

				// Calculation log
				//Logger.Information($"Dividing {calculation}...");

				var result = request.Calculate();

				// Result log
				//Logger.Information($"The result of {calculation} is {result}.");

				// Save to journal
				//Logger.Information("Adding operation to journal...");
				Journal.AddRecord(trackingId, operation, calculation+"=" + " { Quotient:" + result.ToArray()[0] + ", Remainder: " + result.ToArray()[1] + " }");
				//Logger.Information("Operation added to journal successfully.");
				// Return result to client
				return DivisionResponse.FromDivision(result.First(), result.Last());
			}
			else
			{
				var errorKey = "division";
				var errorMessage = "Invalid input parameters. Make sure you didn't enter a zero as the divisor!.";
				return BadRequest(ThrowBadRequest400(errorKey, errorMessage));
			}
		}
		// ipaddress/Calculator/sqrt
		[HttpPost("sqrt")]
		public ActionResult<SquareRootResponse> SquareRoot(SquareRootRequest request)
		{
			//Logger.Information("Operation selected: Div.");

			// Check if the request is null or empty and returns an InternalServerError 500 response
			if (request == null || request.IsEmpty())
			{
				//Logger.Information("ERROR 500: InternalServerError. Request is null or empty.");
				return new ObjectResult(new CalculatorInternalServerError());
			}

			if (request.IsValid())
			{
				trackingId = HttpContext.Request.Headers[trackingIdName];

				// Tracking ID log
				//Logger.Information($"TrackingID: {trackingId}.");

				operation = "sqrt";
				calculation = "√" + request.Number;

				// Calculation log
				//Logger.Information($"Square rooting {calculation}...");

				var result = request.Calculate();

				// Result log
				//Logger.Information($"The result of {calculation} is {result}.");

				// Save to journal
				//Logger.Information("Adding operation to journal...");
				Journal.AddRecord(trackingId, operation, calculation+"="+result);
				//Logger.Information("Operation added to journal successfully.");
				// Return result to client
				return SquareRootResponse.FromSquareRoot(result);
			}
			else
			{
				var errorKey = "squareRoot";
				var errorMessage = "Invalid input parameters. Make sure you entered only numbers and not other characters.";
				return BadRequest(ThrowBadRequest400(errorKey, errorMessage));
			}
		} // For some reason if I send an empty body request, the sqrt returns the result as 0 instead of the error 500.
		// ipaddress/Calculator/journal/query
		[HttpPost("journal/query")]
		public ActionResult<JournalResponse> JournalQuery(JournalRequest request)
		{
			// Check if the request is null or empty and returns an InternalServerError 500 response
			if (request == null || request.IsEmpty())
			{
				return new ObjectResult(new CalculatorInternalServerError());
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
		public static CalculatorBadRequest ThrowBadRequest400(string errorKey, string errorMessage)
		{
			var modelState = new ModelStateDictionary();
			modelState.AddModelError(errorKey, errorMessage);
			//Logger.Information("ERROR 400: BadRequest. Invalid input parameters were provided.");
			return new CalculatorBadRequest(modelState);
		}

	}
}
