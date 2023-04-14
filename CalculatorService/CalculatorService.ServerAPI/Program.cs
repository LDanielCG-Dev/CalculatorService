using CalculatorService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using LoggerService;
using NLog;

namespace CalculatorService.ServerAPI
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));

			builder.Services.AddScoped<ILoggerManager, LoggerManager>();

			// Add services to the container.
			builder.Services.AddControllers()
				.ConfigureApiBehaviorOptions(options =>
				{
					options.InvalidModelStateResponseFactory = context =>
					{
						var badRequest = new CalculatorBadRequest(context.ModelState);

						return new BadRequestObjectResult(badRequest);
					};

					// Handle InternalServerError (500) error
					options.SuppressMapClientErrors = true;
					options.SuppressModelStateInvalidFilter = true;
					options.ClientErrorMapping[StatusCodes.Status500InternalServerError].Link = "https://localhost:5199/swagger/v1/swagger.json#/CalculatorInternalServerError";
				});


			// Configure Swagger
			builder.Services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "Calculator API", Version = "v1" });

				// Add a response model for InternalServerError (500)
				c.MapType<CalculatorInternalServerError>(() => new OpenApiSchema { Type = "object" });
				c.OperationFilter<AddInternalServerErrorResponse>();
			});


			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI(c =>
				{
					c.SwaggerEndpoint("/swagger/v1/swagger.json", "Calculator API V1");
				});
			}

			app.UseRouting();
			app.UseAuthorization();
			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});

			app.Run();
		}
	}
}
