using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CalculatorService.ServerAPI.Models;

namespace CalculatorService.Client
{
	class Program
	{
		static async Task Main(string[] args)
		{
			// Crear HttpClient
			var client = new HttpClient();
			client.BaseAddress = new Uri("http://localhost:5199/");

			// Leer la entrada del usuario
			Console.WriteLine("¿Qué operación desea realizar? (Escriba 'add', 'subtract', 'multiply' o 'divide')");
			string operation = Console.ReadLine();

			// Crear la solicitud
			var request = new CalculatorServiceRequest();
			switch (operation.ToLower())
			{
				case "suma":
					request.Operation = "addition";
					Console.WriteLine("Introduzca los números que desea sumar separados por comas:");
					string[] addends = Console.ReadLine().Split(',');
					request.Addends = new List<double>();
					foreach (string addend in addends)
					{
						request.Addends.Add(double.Parse(addend.Trim()));
					}
					break;
				case "resta":
					request.Operation = "subtraction";
					Console.WriteLine("Introduzca el minuendo:");
					var minuend = double.Parse(Console.ReadLine());
					Console.WriteLine("Introduzca el sustraendo:");
					var subtrahend = double.Parse(Console.ReadLine());

					request.Subtraction = new List<double>
					{
						minuend,
						subtrahend
					};

					break;
				case "multiplicacion":
					request.Operation = "multiplication";
					Console.WriteLine("Introduzca los números que desea multiplicar separados por comas:");
					string[] factors = Console.ReadLine().Split(',');
					request.Factors = new List<double>();
					foreach (string factor in factors)
					{
						request.Factors.Add(double.Parse(factor.Trim()));
					}
					break;
				case "division":
					request.Operation = "division";
					Console.WriteLine("Introduzca el dividendo:");
					request.Dividend = double.Parse(Console.ReadLine());
					Console.WriteLine("Introduzca el divisor:");
					request.Divisor = double.Parse(Console.ReadLine());
					break;
				default:
					Console.WriteLine("Operación no válida.");
					return;
			}

			// Serializar la solicitud a JSON y enviarla al servidor
			var jsonRequest = JsonSerializer.Serialize(request);
			var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
			var response = await client.PostAsync($"CalculatorService/{request.Operation}", content);

			// Leer la respuesta del servidor y deserializarla a un objeto de respuesta
			var jsonResponse = await response.Content.ReadAsStringAsync();
			var calculatorResponse = JsonSerializer.Deserialize<CalculatorServiceResponse>(jsonResponse);

			// Mostrar el resultado al usuario
			switch (operation.ToLower())
			{
				case "add":
					Console.WriteLine($"El resultado de la suma es: {calculatorResponse.Sum}");
					break;
				case "subtract":
					Console.WriteLine($"El resultado de la resta es: {calculatorResponse.Difference}");
					break;
				case "multiply":
					Console.WriteLine($"El resultado de la multiplicación es: {calculatorResponse.Product}");
					break;
				case "divide":
					Console.WriteLine($"El cociente de la división es: {calculatorResponse.Quotient}, y el resto es: {calculatorResponse.Remainder}");
					break;
			}
		}
	}
}

