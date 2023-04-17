using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using CalculatorService.Models;
using NLog;

namespace CalculatorService.ServerAPI.Controllers
{
	// ipaddress/Calculator
	[Route("[controller]")]
	[ApiController]
	[NullExceptionFilter]
	public class CalculatorController : ControllerBase
	{
		private static readonly NLog.ILogger _logger = LogManager.GetCurrentClassLogger();
		private string _trackingId = "";
		private string _calculation = "";

		private const string TRACKING_ID_NAME = "X-Evi-Tracking-Id";
		private const string ADD = "add";
		private const string SUB = "sub";
		private const string MULT = "mult";
		private const string DIV = "div";
		private const string SQRT = "sqrt";
		private const string JOURNAL_QUERY = "journal/query";

		public static CalculatorBadRequest ThrowBadRequest400(string errorKey, string errorMessage = "The request that was sent is not valid, is null or it's empty.")
		{
			_logger.Error("ERROR 400: BadRequest. {0}", errorMessage);

			var modelState = new ModelStateDictionary();
			modelState.AddModelError(errorKey, errorMessage);
			return new CalculatorBadRequest(modelState);
		}
		// ipaddress/Calculator/add
		[HttpPost(ADD)]
		public ActionResult<AdditionResponse> Add(AdditionRequest request)
		{
			_logger.Info("Recieved request for operation: {0}.", ADD);

			// Check if the request is null or empty and returns a BadRequest 400 response
			if (string.IsNullOrWhiteSpace(request?.Addends.ToString()))
			{
				return BadRequest(ThrowBadRequest400(ADD));
			}

			if (request.IsValid() & !string.IsNullOrEmpty(HttpContext.Request.Headers[TRACKING_ID_NAME]))
			{
				_trackingId = HttpContext.Request.Headers[TRACKING_ID_NAME];

				_calculation = String.Join('+', request.Addends.ToArray());

				var result = request.Calculate();
				_logger.Info("The result of {0} is {1}.", _calculation, result);

				// save to journal
				Journal.AddRecord(_trackingId, ADD, _calculation + " = " + result);
				// Return result to client
				return AdditionResponse.FromAddition(result);
			}
			else
			{
				var errorMessage = "Missing tracking id or invalid input parameters. Make sure you entered at least two numbers.";
				return BadRequest(ThrowBadRequest400(ADD, errorMessage));
			}

		}
		// ipaddress/Calculator/sub
		[HttpPost(SUB)]
		public ActionResult<SubtractionResponse> Subtract(SubtractionRequest request)
		{
			_logger.Info("Recieved request for operation: {0}.", SUB);

			// Check if the request is null or empty and returns a BadRequest 400 response
			if (string.IsNullOrWhiteSpace(request?.Minuend.ToString()) || string.IsNullOrEmpty(request?.Subtrahend.ToString()))
			{
				return BadRequest(ThrowBadRequest400(SUB));
			}

			if (request.IsValid() & !string.IsNullOrEmpty(HttpContext.Request.Headers[TRACKING_ID_NAME]))
			{
				_trackingId = HttpContext.Request.Headers[TRACKING_ID_NAME];

				_calculation = request.Minuend + "-" + request.Subtrahend;

				var result = request.Calculate();
				_logger.Info("The result of {0} is {1}.", _calculation, result);

				// save to journal
				Journal.AddRecord(_trackingId, SUB, _calculation + " = " + result);
				// Return result to client
				return SubtractionResponse.FromSubtraction(result);
			}
			else
			{
				var errorMessage = "Missing tracking id or invalid input parameters. Make sure you entered only numbers and not other characters.";
				return BadRequest(ThrowBadRequest400(SUB, errorMessage));
			}
		}
		// ipaddress/Calculator/mult
		[HttpPost(MULT)]
		public ActionResult<MultiplicationResponse> Multiply(MultiplicationRequest request)
		{
			_logger.Info("Recieved request for operation: {0}.", MULT);

			// Check if the request is null or empty and returns a BadRequest 400 response
			if (string.IsNullOrWhiteSpace(request?.Factors.ToString()))
			{
				return BadRequest(ThrowBadRequest400(MULT));
			}

			if (request.IsValid() & !string.IsNullOrEmpty(HttpContext.Request.Headers[TRACKING_ID_NAME]))
			{
				_trackingId = HttpContext.Request.Headers[TRACKING_ID_NAME];

				_calculation = String.Join('*', request.Factors.ToArray());

				var result = request.Calculate();
				_logger.Info("The result of {0} is {1}.", _calculation, result);

				// Save to journal
				Journal.AddRecord(_trackingId, MULT, _calculation + " = " + result);
				// Return result to client
				return MultiplicationResponse.FromMultiplication(result);
			}
			else
			{
				var errorMessage = "Missing tracking id or invalid input parameters. Make sure you entered at least two numbers.";
				return BadRequest(ThrowBadRequest400(MULT, errorMessage));
			}
		}
		// ipaddress/Calculator/div
		[HttpPost(DIV)]
		public ActionResult<DivisionResponse> Division(DivisionRequest request)
		{
			_logger.Info("Recieved request for operation: {0}.", DIV);

			// Check if the request is null or empty and returns a BadRequest 400 response
			if (string.IsNullOrWhiteSpace(request?.Dividend.ToString()) || string.IsNullOrWhiteSpace(request?.Dividend.ToString()))
			{
				return BadRequest(ThrowBadRequest400(DIV));
			}

			if (request.IsValid() & !string.IsNullOrEmpty(HttpContext.Request.Headers[TRACKING_ID_NAME]))
			{
				_trackingId = HttpContext.Request.Headers[TRACKING_ID_NAME];

				_calculation = request.Dividend + "/" + request.Divisor;

				var result = request.Calculate();
				_logger.Info("The result of {0} is {1}.", _calculation, "Quotient: " + result.ToArray()[0] + ", Remainder: " + result.ToArray()[1]);

				// Save to journal
				Journal.AddRecord(_trackingId, DIV, _calculation + " = " + " { Quotient:" + result.ToArray()[0] + ", Remainder: " + result.ToArray()[1] + " }");
				// Return result to client
				return DivisionResponse.FromDivision(result.First(), result.Last());
			}
			else
			{
				var errorMessage = "Missing tracking id or invalid input parameters. Make sure you entered integers and you didn't enter a zero as the divisor!.";
				return BadRequest(ThrowBadRequest400(DIV, errorMessage));
			}
		}
		// ipaddress/Calculator/sqrt
		[HttpPost(SQRT)]
		public ActionResult<SquareRootResponse> SquareRoot(SquareRootRequest request)
		{
			_logger.Info("Recieved request for operation: {0}.", SQRT);

			// Check if the request is null or empty and returns an BadRequest 400 response
			if (string.IsNullOrWhiteSpace(request?.Number.ToString()))
			{
				return BadRequest(ThrowBadRequest400(SQRT));
			}

			if (request.IsValid() & !string.IsNullOrEmpty(HttpContext.Request.Headers[TRACKING_ID_NAME]))
			{
				_trackingId = HttpContext.Request.Headers[TRACKING_ID_NAME];

				_calculation = "√" + request.Number;

				var result = request.Calculate();
				_logger.Info("The result of {0} is {1}.", _calculation, result);

				// Save to journal
				Journal.AddRecord(_trackingId, SQRT, _calculation + " = " + result);
				// Return result to client
				return SquareRootResponse.FromSquareRoot(result);
			}
			else
			{
				var errorMessage = "Missing tracking id or invalid input parameters. Make sure you entered only numbers and not other characters.";
				return BadRequest(ThrowBadRequest400(SQRT, errorMessage));
			}
		}
		// ipaddress/Calculator/journal/query
		[HttpPost(JOURNAL_QUERY)]
		public ActionResult<JournalResponse> JournalQuery(JournalRequest request)
		{
			_logger.Info("Recieved request for operation: {0}.", JOURNAL_QUERY);

			// Check if the request is null or empty and returns an InternalServerError 500 response
			if (string.IsNullOrWhiteSpace(request?.Id))
			{
				return BadRequest(ThrowBadRequest400(JOURNAL_QUERY));
			}

			if (request.HasTrackingId())
			{
				var records = Journal.TryGetRecordsForTrackingId(request.Id);
				if (records != null)
				{
					return JournalResponse.FromJournal(records);
				}
			}

			return new ObjectResult(new JournalResponse { Operations = new List<JournalOperation>(), Message = "The journal is currently empty." });
		}
	}
}
