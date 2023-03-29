//namespace CalculatorService.ServerAPI
//{
//	public class Program
//	{
//		public static void Main(string[] args)
//		{
//			var builder = WebApplication.CreateBuilder(args);

//			// Add services to the container.

//			builder.Services.AddControllers();
//			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//			builder.Services.AddEndpointsApiExplorer();
//			builder.Services.AddSwaggerGen();

//			var app = builder.Build();

//			// Configure the HTTP request pipeline.
//			if (app.Environment.IsDevelopment())
//			{
//				app.UseSwagger();
//				app.UseSwaggerUI(//options =>
//				//{
//				//	options.EnableValidator("http://localhost/");
//				//	options.
//				//}
//				);
//			}

//			app.UseAuthorization();


//			app.MapControllers();

//			app.Run();
//		}
//	}
//}

using CalculatorService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;

namespace CalculatorService.ServerAPI
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			builder.Services.AddControllers()
				.ConfigureApiBehaviorOptions(options =>
				{
					options.InvalidModelStateResponseFactory = context =>
					{
						var badRequest = new CalculatorBadRequest(context.ModelState);

						return new BadRequestObjectResult(badRequest);
					};
				});


			// Configure Swagger
			builder.Services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "Calculator API", Version = "v1" });
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
