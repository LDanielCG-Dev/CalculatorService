using Microsoft.AspNetCore.Mvc.ModelBinding;
public class CalculatorInternalServerError
{
	public string ErrorCode { get; set; }
	public int ErrorStatus { get; set; }
	public string ErrorMessage { get; set; }

	public CalculatorInternalServerError()
	{
		ErrorCode = "InternalError";
		ErrorStatus = 500;
		ErrorMessage = "An unexpected error condition was triggered which made impossible to fulfill the request. Please try again.";
	}
}
