using RestSharp;
using CalculatorService.ServerAPI.Models;

namespace CalculatorService.Client
{
	class Program
	{
		static void Main(string[] args)
		{
			var client = new RestClient("http://localhost:5199");

			while(true)
			{
				// Leer la entrada del usuario
				Console.WriteLine("------------------------------------------------------------------------------------------------");
				Console.WriteLine("Select the operation you want to do: (Write 'add', 'subtract', 'multiply', 'divide' or 'sqroot')");
				Console.Write("> ");
				string operation = Console.ReadLine();
				Console.WriteLine("------------------------------------------------------------------------------------------------");

				switch (operation.ToLower())
				{
					case "add":
						Console.WriteLine("Enter numbers to add (separated by commas):");
						List<double> addends;

						// Input addends
						while (true)
						{
							Console.Write("> ");
							string input = Console.ReadLine();
							string[] numbers = input.Split(',');

							addends = new List<double>();
							bool allValid = true;
							if (numbers.Length < 2)
							{
								allValid = false;
							}
							else
							{
								foreach (string num in numbers)
								{
									if (double.TryParse(num, out double addParsed))
									{
										addends.Add(addParsed);
									}
									else
									{
										Console.WriteLine($"Invalid number: {num}");
										allValid = false;
									}
								}
							}

							if (allValid)
							{
								break;
							}
							else
							{
								Console.WriteLine("Please enter at least 2 valid numbers separated by commas:");
							}
						}

						// Data is sent and recieved
						AdditionResponse additionResponse = SendData<AdditionResponse>(client, "CalculatorService/add", Method.Post, new { Addends = addends });

						// Print result
						if (additionResponse != null)
						{
							Console.WriteLine($"{{\n  Sum: {additionResponse.Sum} \n}}");
						}
						break;

					case "subtract":
						// Input minuend
						Console.WriteLine("Enter minuend:");
						double minuend;
						while (true)
						{
							Console.Write("> ");
							string input = Console.ReadLine();
							if (double.TryParse(input, out double parsed))
							{
								minuend = parsed;
								break;
							}
							else
							{
								Console.WriteLine($"Invalid number: {input}");
								Console.WriteLine("Please enter a valid number:");
							}
						}

						// Input subtrahend
						Console.WriteLine("Enter subtrahend:");
						double subtrahend;
						while (true)
						{
							Console.Write("> ");
							string input = Console.ReadLine();
							if (double.TryParse(input, out double parsed))
							{
								subtrahend = parsed;
								break;
							}
							else
							{
								Console.WriteLine($"Invalid number: {input}");
								Console.WriteLine("Please enter a valid number:");
							}
						}

						// Data is sent and recieved
						SubtractionRequest subtractionRequest = new SubtractionRequest() { Minuend = minuend, Subtrahend = subtrahend };
						SubtractionResponse subtractionResponse = SendData<SubtractionResponse>(client, "CalculatorService/sub", Method.Post, subtractionRequest);

						// Print result
						if (subtractionRequest != null)
						{
							Console.WriteLine($"{{\n  Difference: {subtractionResponse.Difference} \n}}");
						}
						break;

					case "multiply":
						Console.WriteLine("Enter numbers to multiply (separated by commas):");
						List<double> factors;

						// Input factors
						while (true)
						{
							Console.Write("> ");
							string input = Console.ReadLine();
							string[] numbers = input.Split(',');

							factors = new List<double>();
							bool allValid = true;

							if (numbers.Length < 2)
							{
								allValid = false;
							}
							else
							{
								foreach (string number in numbers)
								{
									if (double.TryParse(number, out double facParsed))
									{
										factors.Add(facParsed);
									}
									else
									{
										Console.WriteLine($"Invalid number: {number}");
										allValid = false;
									}
								}
							}
							if (allValid)
							{
								break;
							}
							else
							{
								Console.WriteLine("Please enter valid numbers separated by commas:");
							}
						}

						// Data is sent and recieved
						MultiplicationResponse multiplicationResponse = SendData<MultiplicationResponse>(client, "CalculatorService/mult", Method.Post, new { Factors = factors });

						// Print result
						if (multiplicationResponse != null)
						{
							Console.WriteLine($"{{\n  Product: {multiplicationResponse.Product} \n}}");
						}
						break;

					case "divide":
						// Input dividend
						Console.WriteLine("Enter dividend:");
						int dividend;
						while (true)
						{
							Console.Write("> ");
							string input = Console.ReadLine();
							if (int.TryParse(input, out int parsed))
							{
								dividend = parsed;
								break;
							}
							else
							{
								Console.WriteLine($"Invalid number: {input}");
								Console.WriteLine("Please enter a valid dividend:");
							}
						}

						// Input divisor
						Console.WriteLine("Entero divisor:");
						int divisor;
						while (true)
						{
							Console.Write("> ");
							string input = Console.ReadLine();
							if (int.TryParse(input, out int parsed) && parsed != 0)
							{
								divisor = parsed;
								break;
							}
							else
							{
								Console.WriteLine($"Invalid number: {input}");
								Console.WriteLine("Please enter a valid divisor:");
							}
						}

						// Data is sent and recieved
						DivisionRequest divisionRequest = new DivisionRequest() { Dividend = dividend, Divisor = divisor };
						DivisionResponse divisionResponse = SendData<DivisionResponse>(client, "CalculatorService/div", Method.Post, divisionRequest);

						// Print result
						if (divisionResponse != null)
						{
							Console.WriteLine($"{{\n  Quotient: {divisionResponse.Quotient},\n  Remainder: {divisionResponse.Remainder} \n}}");
						}
						break;

					case "sqroot":
						// Input number
						Console.WriteLine("Enter number: ");
						double numberSqrt;
						while (true)
						{
							Console.Write("> ");
							string input = Console.ReadLine();
							if (double.TryParse(input, out double parsed))
							{
								numberSqrt = parsed;
								break;
							}
							else
							{
								Console.WriteLine($"Invalid number: {input}");
								Console.WriteLine("Please enter a valid dividend:");
							}
						}

						// Data is sent and recieved
						SquareRootResponse squareRootResponse = SendData<SquareRootResponse>(client, "CalculatorService/sqrt", Method.Post, new { Number = numberSqrt });

						// Print result
						if (squareRootResponse != null)
						{
							Console.WriteLine($"{{\n  Square: {squareRootResponse.Square} \n}}");
						}

						break;
					default:
						Console.WriteLine("Please select one of the operations!");
						break;
				}
			}
		}
		public static T SendData<T>(RestClient client, string endpoint, Method method, object requestBody) where T : class, new()
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
	}
}
