using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

public class NullExceptionFilterAttribute : ExceptionFilterAttribute
{
	public override void OnException(ExceptionContext context)
	{
		if (context.Exception is NullReferenceException)
		{
			context.Result = new ObjectResult(new CalculatorInternalServerError());
		}
	}
}
