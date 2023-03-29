using RestSharp;
using CalculatorService.Models;
using CalculatorService.ServerAPI.Controllers;

namespace CalculatorService.Client
{
	class Program
	{
		private static TResponse SendRequestGetResponse<TRequest, TResponse>(RestClient client, string endPoint, Method method, (string propertyName, object propertyValue)[] propertyValues) where TRequest : new() where TResponse : class, new()
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
		private static T SendData<T>(RestClient client, string endpoint, Method method, object requestBody) where T : class, new()
		{
			var request = new RestRequest(endpoint, method);
			request.AddJsonBody(requestBody);

			var response = client.Execute<T>(request);
			if (response.StatusCode == System.Net.HttpStatusCode.OK)
			{
				return response.Data;
			}
			else
			{
				Console.WriteLine($"Error: {response.ErrorMessage}");
				return default(T);
			}
		}
		private static int GetUserInputInt(bool divisor = false)
		{
			int inputNum;
			while (true)
			{
				Console.Write("> ");
				var input = Console.ReadLine();

				if (NumberIsValid(input))
				{
					if(divisor && input.Equals("0"))
					{
						Console.WriteLine("You can't divide by zero!");
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

				if (NumberIsValid(input))
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
			var allValid = true;

			if (operands.Length >= 2)
			{
				foreach (var num in operands)
				{
					if (num.ToString().Length > MAX_DIGITS || !double.TryParse(num, out var addParsed))
					{
						Console.WriteLine($"Invalid number: {num}");
						allValid = false;
					}
				}
			}
			else
			{
				Console.WriteLine("Please enter at least 2 valid numbers separated by commas:");
				allValid = false;
			}

			return allValid;
		}
		private static bool NumberIsValid(string number)
		{
			var allValid = true;

			if(!string.IsNullOrEmpty(number))
			{
				if (!double.TryParse(number, out var parsed))
				{
					Console.WriteLine($"Invalid number: {number}");
					Console.WriteLine("Please enter a valid number:");
					allValid = false;
				}
			}
			else
			{
				Console.WriteLine("Please enter a number:");
				allValid = false;
			}

			return allValid;

		}
		private static void PrintResult(object obj)
		{
			if(obj != null){
				var type = obj.GetType();
				var properties = type.GetProperties();

				Console.WriteLine("{");
				foreach (var property in properties)
				{
					var value = property.GetValue(obj);
					Console.WriteLine($"  {property.Name}: {value}");
				}
				Console.WriteLine("}");
			}
			else
			{
				// Catch errors if the response is null
			}
		}

		public static void Main(string[] args)
		{
			var client = new RestClient("http://localhost:5199");
			var endPoint = "";
			Method method;

			while (true)
			{
				// Read user input
				Console.WriteLine("------------------------------------------------------------------------------------------------");
				Console.WriteLine("Select the operation you want to do: (Write 'add', 'subtract', 'multiply', 'divide' or 'sqroot')");
				Console.Write("> ");
				var operation = Console.ReadLine();
				Console.WriteLine("------------------------------------------------------------------------------------------------");

				switch (operation.ToLower())
				{
					case "add":
						// Input addends
						Console.WriteLine("Enter numbers to add (separated by commas):");
						var addends = GetUserInputList();

						// Data is sent and recieved
						endPoint = "Calculator/add";
						method = Method.Post;
						var additionResponse = SendRequestGetResponse<AdditionRequest, AdditionResponse>(client,endPoint,method,("Addends", addends)); //Need to fix
						//var additionRequestBody = BuildRequest<AdditionRequest>(("Addends", addends));
						//var additionResponse = SendData<AdditionResponse>(client,endPoint,method,additionRequestBody);

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
						method = Method.Post;
						var subtractionRequestBody = BuildRequest<SubtractionRequest>(("Minuend", minuend),("Subtrahend", subtrahend));
						var subtractionResponse = SendData<SubtractionResponse>(client, endPoint, method, subtractionRequestBody);

						// Print result
						PrintResult(subtractionResponse);
						break;

					case "multiply":
						// Input factors
						Console.WriteLine("Enter numbers to multiply (separated by commas):");
						var factors = GetUserInputList();

						// Data is sent and recieved
						endPoint = "Calculator/mult";
						method = Method.Post;
						var multiplicationRequestBody = BuildRequest<MultiplicationRequest>(("Factors", factors));
						var multiplicationResponse = SendData<MultiplicationResponse>(client,endPoint,method,multiplicationRequestBody);

						// Print result
						PrintResult(multiplicationResponse);
						break;

					case "divide":
						// Input dividend
						Console.WriteLine("Enter dividend:");
						int dividend =	GetUserInputInt();

						// Input divisor
						Console.WriteLine("Entero divisor:");
						int divisor = GetUserInputInt(divisor: true);

						// Data is sent and recieved
						endPoint = "Calculator/div";
						method = Method.Post;
						var divisionRequestBody = BuildRequest<DivisionRequest>(("Dividend", dividend), ("Divisor", divisor));
						var divisionResponse = SendData<DivisionResponse>(client,endPoint,method,divisionRequestBody);

						// Print result
						PrintResult(divisionResponse);
						break;

					case "sqroot":
						// Input number
						Console.WriteLine("Enter number: ");
						var numberSqrt = GetUserInputDouble();

						// Data is sent and recieved
						endPoint = "Calculator/sqrt";
						method = Method.Post;
						var squareRootRequestBody = BuildRequest<SquareRootRequest>(("Number", numberSqrt));
						var squareRootResponse = SendData<SquareRootResponse>(client,endPoint,method,squareRootRequestBody);

						// Print result
						PrintResult(squareRootResponse);
						break;
					default:
						Console.WriteLine("Please select one of the operations!");
						break;
				}
			}
		}
	}
}
