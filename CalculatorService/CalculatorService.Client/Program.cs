using RestSharp;
using CalculatorService.Models;
using CalculatorService.ServerAPI.Controllers;

namespace CalculatorService.Client
{
	class Program
	{
		private static RestResponse<TResponse> SendRequestGetResponse<TRequest, TResponse>(RestClient client, string endPoint, Method method, params (string propertyName, object propertyValue)[] propertyValues) where TRequest : new() where TResponse : class, new()
		{
			var requestBody = BuildRequest<TRequest>(propertyValues);
			var response = SendData<TResponse>(client, endPoint, method, requestBody);

			return response;
		}
		private static T BuildRequest<T>(params (string propertyName, object propertyValue)[] propertyValues) where T : new()
		{
			var instance = new T();
			foreach (var (propertyName, propertyValue) in propertyValues)
			{
				var property = typeof(T).GetProperty(propertyName);
				if (property != null)
				{
					property.SetValue(instance, propertyValue);
				}
			}
			return instance;
		}
		private static RestResponse<T> SendData<T>(RestClient client, string endpoint, Method method, object requestBody) where T : class, new()
		{
			var request = new RestRequest(endpoint, method);
			request.AddHeader("Accept", "application/json");
			request.AddHeader("Content-Type", "application/json");
			request.AddHeader("X-Evi-Tracking-Id", trackingId); // Need to implement
			request.AddJsonBody(requestBody);

			var response = client.Execute<T>(request);
			if (response.StatusCode == System.Net.HttpStatusCode.OK)
			{
				return response;
			}
			else
			{
				Console.WriteLine($"Error: {response.ErrorMessage}");
				return null;
			}
		}
		private static int GetUserInputInt(bool divisor = false)
		{
			int inputNum;
			while (true)
			{
				Console.Write("> ");
				var input = Console.ReadLine();

				if (IntIsValid(input))
				{
					if (divisor && input.Equals("0"))
					{
						Console.WriteLine("You can't divide by zero!");
						Console.WriteLine("Please enter a valid number:");
					}
					else
					{
						inputNum = int.Parse(input);
						break;
					}
				}
			}
			return inputNum;
		}
		private static List<double> GetUserInputList()
		{
			List<double> inputNum;
			while (true)
			{
				Console.Write("> ");
				var input = Console.ReadLine().Split(',');

				if (AddMultIsValid(input))
				{
					inputNum = Array.ConvertAll(input, Double.Parse).ToList();
					break;
				}
			}
			return inputNum;
		}
		private static double GetUserInputDouble()
		{
			double inputNum;
			while (true)
			{
				Console.Write("> ");
				var input = Console.ReadLine();

				if (DoubleIsValid(input))
				{
					if (input.Contains(','))
					{
						input = input.Replace(',', '.');
					}
					inputNum = double.Parse(input);
					break;
				}
			}
			return inputNum;
		}
		private static bool AddMultIsValid(string[] input)
		{
			const int MAX_DIGITS = 9;
			var operands = input.ToArray();

			if (operands.Length >= 2)
			{
				foreach (var num in operands)
				{
					if (num.ToString().Length > MAX_DIGITS || !double.TryParse(num, out var addParsed))
					{
						Console.WriteLine($"Invalid number: {num}");
						return false;
					}
				}
			}
			else
			{
				Console.WriteLine("Please enter at least 2 valid numbers separated by commas:");
				return false;
			}

			return true;
		}
		private static bool DoubleIsValid(string number)
		{
			if (!string.IsNullOrEmpty(number))
			{
				if (!double.TryParse(number, out var parsed))
				{
					Console.WriteLine($"Invalid number: {number}");
					Console.WriteLine("Please enter a valid number:");
					return false;
				}
			}
			else
			{
				Console.WriteLine("Please enter a number:");
				return false;
			}

			return true;
		}
		private static bool IntIsValid(string number)
		{
			if (!string.IsNullOrEmpty(number))
			{
				if (!int.TryParse(number, out var parsed))
				{
					Console.WriteLine($"Invalid number: {number}");
					Console.WriteLine("Please enter a valid number:");
					return false;
				}
			}
			else
			{
				Console.WriteLine("Please enter a number:");
				return false;
			}

			return true;
		}
		private static void PrintResult<T>(RestResponse<T> response) where T : class, new()
		{
			if (response != null)
			{
				// Print headers
				Console.WriteLine($"HTTP/1.1 200 {response.StatusCode}");
				foreach (var header in response.ContentHeaders)
				{
					Console.WriteLine($"{header.Name}: {header.Value}");
				}

				// Print data
				var data = response.Data;
				var type = data.GetType();
				var properties = type.GetProperties();

				Console.WriteLine("{");
				foreach (var property in properties)
				{
					var value = property.GetValue(data);
					Console.WriteLine($"  {property.Name}: {value}");
				}
				Console.WriteLine("}");
			}
			else
			{
				Console.WriteLine("ERROR: the response returned null!");
			}
		}
		public static void PrintJournalResponse(RestResponse<JournalResponse> response)
		{
			var journalResponse = response.Data;
			Console.WriteLine("{");
			Console.WriteLine("  Operations: [");

			for (int i = 0; i < journalResponse.Operations.Count; i++)
			{
				var record = journalResponse.Operations[i];
				Console.WriteLine("    {");
				Console.WriteLine($"      Operation: {record.Operation},");
				Console.WriteLine($"      Calculation: {record.Calculation},");
				Console.WriteLine($"      Date: {record.Date}");
				Console.Write("    }");
				if (i != journalResponse.Operations.Count - 1)
				{
					Console.WriteLine(",");
				}
				else
				{
					Console.WriteLine();
				}
			}

			Console.WriteLine("  ]");
			Console.WriteLine("}");
		}

		public static string trackingId = new Random().Next(1000, 10000).ToString();
		public static void Main(string[] args)
		{
			var client = new RestClient("http://localhost:5199");
			var endPoint = "";
			var method = Method.Post;

			while (true)
			{
				// Read user input
				Console.WriteLine("-----------------------------------------------------------------------------------------------------------");
				Console.WriteLine("Select the operation you want to do: (Write 'add', 'subtract', 'multiply', 'divide', 'sqroot' or 'journal')");
				Console.Write("> ");
				var operation = Console.ReadLine();
				Console.WriteLine("-----------------------------------------------------------------------------------------------------------");

				switch (operation.ToLower())
				{
					case "add":
						// Input addends
						Console.WriteLine("Enter numbers to add (separated by commas):");
						var addends = GetUserInputList();

						// Data is sent and recieved
						endPoint = "Calculator/add";
						var additionResponse = SendRequestGetResponse<AdditionRequest, AdditionResponse>(client, endPoint, method, ("Addends", addends));

						// Print result
						PrintResult(additionResponse);
						break;

					case "subtract":
						// Input minuend
						Console.WriteLine("Enter minuend:");
						var minuend = GetUserInputDouble();

						// Input subtrahend
						Console.WriteLine("Enter subtrahend:");
						var subtrahend = GetUserInputDouble();

						// Data is sent and recieved
						endPoint = "Calculator/sub";
						var subtractionResponse = SendRequestGetResponse<SubtractionRequest, SubtractionResponse>(client, endPoint, method, ("Minuend", minuend), ("Subtrahend", subtrahend));

						// Print result
						PrintResult(subtractionResponse);
						break;

					case "multiply":
						// Input factors
						Console.WriteLine("Enter numbers to multiply (separated by commas):");
						var factors = GetUserInputList();

						// Data is sent and recieved
						endPoint = "Calculator/mult";
						var multiplicationResponse = SendRequestGetResponse<MultiplicationRequest, MultiplicationResponse>(client, endPoint, method, ("Factors", factors));

						// Print result
						PrintResult(multiplicationResponse);
						break;

					case "divide":
						// Input dividend
						Console.WriteLine("Enter dividend:");
						int dividend = GetUserInputInt();

						// Input divisor
						Console.WriteLine("Entero divisor:");
						int divisor = GetUserInputInt(divisor: true);

						// Data is sent and recieved
						endPoint = "Calculator/div";
						var divisionResponse = SendRequestGetResponse<DivisionRequest, DivisionResponse>(client, endPoint, method, ("Dividend", dividend), ("Divisor", divisor));

						// Print result
						PrintResult(divisionResponse);
						break;

					case "sqroot":
						// Input number
						Console.WriteLine("Enter number: ");
						var numberSqrt = GetUserInputDouble();

						// Data is sent and recieved
						endPoint = "Calculator/sqrt";
						var squareRootResponse = SendRequestGetResponse<SquareRootRequest, SquareRootResponse>(client, endPoint, method, ("Number", numberSqrt));

						// Print result
						PrintResult(squareRootResponse);
						break;

					case "journal":

						// Data is sent and recieved
						endPoint = "Calculator/journal/query";
						var journalResponse = SendRequestGetResponse<JournalRequest, JournalResponse>(client, endPoint, method, ("Id", trackingId));

						// Print result
						PrintJournalResponse(journalResponse);
						break;

					default:
						Console.WriteLine("Please select one of the operations!");
						break;
				}
			}
		}
	}
}
