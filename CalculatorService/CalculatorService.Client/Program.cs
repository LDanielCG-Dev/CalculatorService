using RestSharp;
using CalculatorService.Models;
using CalculatorService.Utils;
using NLog;

namespace CalculatorService.Client
{
	public class Program
	{
		private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();
		private static void PrintJournalResponse(RestResponse<JournalResponse> response)
		{
			_logger.Info("Printing data...");
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
					_logger.Info(journalResponse.Message);
					Console.WriteLine($"  Message: {journalResponse.Message}");
				}
			}

			Console.WriteLine("}");
		}

		private static readonly string _trackingId = new Random().Next(1000, 10000).ToString();
		public static void Main(string[] args)
		{
			_logger.Info("Client started.");
			var ip = "http://localhost:5199";
			_logger.Info("Connecting to server with IP: {0}", ip);
			var client = new RestClient(ip);
			var method = Method.Post;
			_logger.Info("Selected method: {0}.", method);

			while (true)
			{
				// Read user input
				Console.WriteLine("-----------------------------------------------------------------------------------------------------------");
				Console.WriteLine("Select the operation you want to do: (Write 'add', 'subtract', 'multiply', 'divide', 'sqroot' or 'journal')");
				Console.Write("> ");
				var operation = Console.ReadLine();
				Console.WriteLine("-----------------------------------------------------------------------------------------------------------");
				_logger.Info("Calculator menu displayed.");

				string? endPoint;
				switch (operation?.ToLower())
				{
					case "add":
						_logger.Info("Operation selected: add");
						// Input addends
						Console.WriteLine("Enter numbers to add (separated by commas):");
						var addends = UserInput.GetUserInputList();

						// Data is sent and recieved
						endPoint = "Calculator/add";
						var additionResponse = HTTPConnection.SendRequestGetResponse<AdditionRequest, AdditionResponse>(client, endPoint, method, _trackingId, ("Addends", addends));

						// Print result
						Console.WriteLine(additionResponse.Data);
						break;

					case "subtract":
						_logger.Info("Operation selected: sub");
						// Input minuend
						Console.WriteLine("Enter minuend:");
						var minuend = UserInput.GetUserInputDouble();

						// Input subtrahend
						Console.WriteLine("Enter subtrahend:");
						var subtrahend = UserInput.GetUserInputDouble();

						// Data is sent and recieved
						endPoint = "Calculator/sub";
						var subtractionResponse = HTTPConnection.SendRequestGetResponse<SubtractionRequest, SubtractionResponse>(client, endPoint, method, _trackingId, ("Minuend", minuend), ("Subtrahend", subtrahend));

						// Print result
						Console.WriteLine(subtractionResponse.Data);
						break;

					case "multiply":
						_logger.Info("Operation selected: mult");
						// Input factors
						Console.WriteLine("Enter numbers to multiply (separated by commas):");
						var factors = UserInput.GetUserInputList();

						// Data is sent and recieved
						endPoint = "Calculator/mult";
						var multiplicationResponse = HTTPConnection.SendRequestGetResponse<MultiplicationRequest, MultiplicationResponse>(client, endPoint, method, _trackingId, ("Factors", factors));

						// Print result
						Console.WriteLine(multiplicationResponse.Data);
						break;

					case "divide":
						_logger.Info("Operation selected: sub");
						// Input dividend
						Console.WriteLine("Enter dividend:");
						int dividend = UserInput.GetUserInputInt();
						int divisor;
						// Input divisor
						while (true)
						{
							Console.WriteLine("Enter divisor:");
							divisor = UserInput.GetUserInputInt();

							if (divisor == 0)
							{
								_logger.Error("The inputted divisor was zero.");
								Console.WriteLine("You can't divide by zero!");
								Console.WriteLine("Please enter a valid number:");
							}
							else
							{
								break;
							}
						}

						// Data is sent and recieved
						endPoint = "Calculator/div";
						var divisionResponse = HTTPConnection.SendRequestGetResponse<DivisionRequest, DivisionResponse>(client, endPoint, method, _trackingId, ("Dividend", dividend), ("Divisor", divisor));

						// Print result
						Console.WriteLine(divisionResponse.Data);
						break;

					case "sqroot":
						_logger.Info("Operation selected: sqrt");
						// Input number
						Console.WriteLine("Enter number: ");
						var numberSqrt = UserInput.GetUserInputDouble();

						// Data is sent and recieved
						endPoint = "Calculator/sqrt";
						var squareRootResponse = HTTPConnection.SendRequestGetResponse<SquareRootRequest, SquareRootResponse>(client, endPoint, method, _trackingId, ("Number", numberSqrt));

						// Print result
						Console.WriteLine(squareRootResponse.Data);
						break;

					case "journal":
						_logger.Info("Operation selected: journal");
						// Data is sent and recieved
						endPoint = "Calculator/journal/query";
						var journalResponse = HTTPConnection.SendRequestGetResponse<JournalRequest, JournalResponse>(client, endPoint, method, _trackingId, ("Id", _trackingId));

						// Print result
						PrintJournalResponse(journalResponse);
						break;

					default:
						_logger.Error("User didn't select one of the operations.");
						Console.WriteLine("Please select one of the operations!");
						break;
				}
			}
		}
	}
}
