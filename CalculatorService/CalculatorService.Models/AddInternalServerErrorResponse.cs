using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

public class AddInternalServerErrorResponse : IOperationFilter
{
	public void Apply(OpenApiOperation operation, OperationFilterContext context)
	{
		operation.Responses.Add("500", new OpenApiResponse
		{
			Description = "Internal Server Error",
			Content = new Dictionary<string, OpenApiMediaType>
			{
				{ "application/json", new OpenApiMediaType
					{
						Schema = context.SchemaGenerator.GenerateSchema(typeof(CalculatorInternalServerError),
						context.SchemaRepository)
					}
				}
			}
		});
	}
}
