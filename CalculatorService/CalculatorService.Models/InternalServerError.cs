using LoggerService;
using Microsoft.AspNetCore.Mvc.ModelBinding;
public class CalculatorInternalServerError
{
	private static readonly ILoggerManager _logger = new LoggerManager();
	public string ErrorCode { get; set; }
	public int ErrorStatus { get; set; }
	public string ErrorMessage { get; set; }

	public CalculatorInternalServerError()
	{
		_logger.LogError("ERROR 500: InternalServerError. Request is null or empty.");
		ErrorCode = "InternalError";
		ErrorStatus = 500;
		ErrorMessage = "An unexpected error condition was triggered which made impossible to fulfill the request. Please try again or contact support.";
	}
}
