using RestSharp;
using CalculatorService.Models;
using System.Globalization;
using LoggerService;

namespace CalculatorService.Client
{
	public class Program
	{
		private static readonly ILoggerManager _logger = new LoggerManager();
		private static RestResponse<TResponse> SendRequestGetResponse<TRequest, TResponse>(RestClient client, string endPoint, Method method, params (string propertyName, object propertyValue)[] propertyValues) where TRequest : new() where TResponse : class, new()
		{
			var requestBody = BuildRequest<TRequest>(propertyValues);
			var response = SendData<TResponse>(client, endPoint, method, requestBody);
			return response;
		}
		private static T BuildRequest<T>(params (string propertyName, object propertyValue)[] propertyValues) where T : new()
		{
			_logger.LogInfo("Building request...");
			var instance = new T();
			foreach (var (propertyName, propertyValue) in propertyValues)
			{
				var property = typeof(T).GetProperty(propertyName);
				if (property != null)
				{
					property.SetValue(instance, propertyValue);
				}
			}
			_logger.LogInfo("Request built.");
			return instance;
		}
		private static RestResponse<T>? SendData<T>(RestClient client, string endpoint, Method method, object requestBody) where T : class, new()
		{
			_logger.LogInfo("Setting request headers...");
			var request = new RestRequest(endpoint, method);
			request.AddHeader("Accept", "application/json");
			request.AddHeader("Content-Type", "application/json");
			request.AddHeader("X-Evi-Tracking-Id", _trackingId);
			_logger.LogInfo("Setting request body...");
			request.AddJsonBody(requestBody);

			_logger.LogInfo("Sending data to server...");
			var response = client.Execute<T>(request);
			if (response.StatusCode == System.Net.HttpStatusCode.OK)
			{
				_logger.LogInfo("Data recieved from server. Returning response.");
				return response;
			}
			else
			{
				_logger.LogError($"An error has occured: {response.ErrorMessage}.");
				Console.WriteLine($"Error: {response.ErrorMessage}");
				return null;
			}
		}
		private static int GetUserInputInt(bool divisor = false)
		{
			_logger.LogInfo("User inputting data...");

			int inputNum;
			while (true)
			{
				Console.Write("> ");
				var input = Console.ReadLine();

				if (IntIsValid(input))
				{
					if (divisor && input.Equals("0"))
					{
						_logger.LogError("The inputted divisor was zero.");
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
			_logger.LogInfo("Returning validated data...");
			return inputNum;
		}
		private static IList<double> GetUserInputList()
		{
			_logger.LogInfo("User inputting data...");

			List<double> inputNum;
			while (true)
			{
				Console.Write("> ");
				var input = Console.ReadLine().Split(',');

				if (AddMultIsValid(input))
				{
					inputNum = Array.ConvertAll(input, s => Double.Parse(s, CultureInfo.GetCultureInfo("en-US"))).ToList();
					break;
				}
			}
			_logger.LogInfo("Returning validated data...");
			return inputNum;
		}
		private static double GetUserInputDouble()
		{
			_logger.LogInfo("User inputting data...");

			double inputNum = 0.0;
			string input = "";
			while (true)
			{
				Console.Write("> ");
				input = Console.ReadLine();

				if (DoubleIsValid(input))
				{
					if (input.Contains(','))
					{
						inputNum = double.Parse(input.Replace(',', '.'), CultureInfo.GetCultureInfo("en-US"));
					}
					inputNum = double.Parse(input);
					break;
				}
			}
			_logger.LogInfo("Returning validated data...");
			return inputNum;
		}
		private static bool AddMultIsValid(string[] input)
		{
			_logger.LogInfo("Validating input...");
			const int MAX_DIGITS = 9;
			var operands = input.ToArray();

			if (operands.Length >= 2)
			{
				foreach (var num in operands)
				{
					if (!double.TryParse(num, out var addParsed) || num.ToString().Length > MAX_DIGITS)
					{
						_logger.LogError($"Validation failed. Invalid number {num}.");
						Console.WriteLine($"Invalid number: {num}");
						return false;
					}
				}
			}
			else
			{
				_logger.LogError("Validation failed. Not enough input numbers.");
				Console.WriteLine("Please enter at least 2 valid numbers separated by commas:");
				return false;
			}
			_logger.LogInfo("Validation successful. The input is valid.");
			return true;
		}
		private static bool DoubleIsValid(string number)
		{
			_logger.LogInfo("Validating input...");
			if (!string.IsNullOrEmpty(number))
			{
				const int MAX_DIGITS = 9;
				if (!double.TryParse(number, out var parsed) || number.Length > MAX_DIGITS)
				{
					_logger.LogError($"Validation failed. Invalid number {number}.");
					Console.WriteLine($"Invalid number: {number}");
					Console.WriteLine("Please enter a valid number:");
					return false;
				}
			}
			else
			{
				_logger.LogError("User didn't input a number.");
				Console.WriteLine("Please enter a number:");
				return false;
			}
			_logger.LogInfo("Validation successful. The input is valid.");
			return true;
		}
		private static bool IntIsValid(string number)
		{
			_logger.LogInfo("Validating input...");
			if (!string.IsNullOrEmpty(number))
			{
				const int MAX_DIGITS = 9;
				if (!int.TryParse(number, out var parsed) || number.Length > MAX_DIGITS)
				{
					_logger.LogError($"Validation failed. Invalid number {number}.");
					Console.WriteLine($"Invalid number: {number}");
					Console.WriteLine("Please enter a valid number:");
					return false;
				}
			}
			else
			{
				_logger.LogError("User didn't input a number.");
				Console.WriteLine("Please enter a number:");
				return false;
			}
			_logger.LogInfo("Validation successful. The input is valid.");
			return true;
		}
		private static void PrintResult<T>(RestResponse<T> response) where T : class, new()
		{
			if (response != null)
			{
				// Print headers
				_logger.LogInfo("Printing headers...");
				Console.WriteLine($"HTTP/1.1 200 {response.StatusCode}");
				foreach (var header in response.ContentHeaders)
				{
					Console.WriteLine($"{header.Name}: {header.Value}");
				}

				// Print data
				_logger.LogInfo("Printing data...");
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
				_logger.LogError("ERROR: the response returned null.");
				Console.WriteLine("ERROR: the response returned null!");
			}
		}
		private static void PrintJournalResponse(RestResponse<JournalResponse> response)
		{
			_logger.LogInfo("Printing data...");
			Console.WriteLine("{");

			if(response != null)
			{
				var journalResponse = response.Data;
				if (journalResponse.Operations.Count > 0)
				{
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
				}
				else if (journalResponse.Message != null)
				{
					_logger.LogInfo(journalResponse.Message);
					Console.WriteLine($"  Message: {journalResponse.Message}");
				}
			}

			Console.WriteLine("}");
		}

		private static string _trackingId = new Random().Next(1000, 10000).ToString();
		public static void Main(string[] args)
		{
			_logger.LogInfo("Client started.");
			var ip = "http://localhost:5199";
			_logger.LogInfo($"Connecting to server with IP: {ip}");
			var client = new RestClient(ip);
			var endPoint = "";
			var method = Method.Post;
			_logger.LogInfo($"Selected method: {method}.");

			while (true)
			{
				// Read user input
				Console.WriteLine("-----------------------------------------------------------------------------------------------------------");
				Console.WriteLine("Select the operation you want to do: (Write 'add', 'subtract', 'multiply', 'divide', 'sqroot' or 'journal')");
				Console.Write("> ");
				var operation = Console.ReadLine();
				Console.WriteLine("-----------------------------------------------------------------------------------------------------------");
				_logger.LogInfo("Calculator menu displayed.");

				switch (operation.ToLower())
				{
					case "add":
						_logger.LogInfo("Operation selected: add");
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
						_logger.LogInfo("Operation selected: sub");
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
						_logger.LogInfo("Operation selected: mult");
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
						_logger.LogInfo("Operation selected: sub");
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
						_logger.LogInfo("Operation selected: sqrt");
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
						_logger.LogInfo("Operation selected: journal");
						// Data is sent and recieved
						endPoint = "Calculator/journal/query";
						var journalResponse = SendRequestGetResponse<JournalRequest, JournalResponse>(client, endPoint, method, ("Id", _trackingId));

						// Print result
						PrintJournalResponse(journalResponse);
						break;

					default:
						_logger.LogError("User didn't select one of the operations.");
						Console.WriteLine("Please select one of the operations!");
						break;
				}
			}
		}
	}
}
