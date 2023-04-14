using System.Globalization;
using NLog;

namespace CalculatorService.Utils
{
	public static class UserInput
	{
		private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();
		public static int GetUserInputInt()
		{
			_logger.Info("User inputting data...");

			int inputNum;
			while (true)
			{
				Console.Write("> ");
				var input = Console.ReadLine();

				if (IntIsValid(input))
				{
					inputNum = int.Parse(input);
					break;
				}
			}
			_logger.Info("Returning validated data...");
			return inputNum;
		}
		public static IList<double> GetUserInputList()
		{
			_logger.Info("User inputting data...");

			List<double> inputNum;
			while (true)
			{
				Console.Write("> ");
				var input = Console.ReadLine()?.Split(',');

				if (AddMultIsValid(input))
				{
					inputNum = Array.ConvertAll(input, s => Double.Parse(s, CultureInfo.GetCultureInfo("en-US"))).ToList();
					break;
				}
			}
			_logger.Info("Returning validated data...");
			return inputNum;
		}
		public static double GetUserInputDouble()
		{
			_logger.Info("User inputting data...");

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
					else
					{
						inputNum = double.Parse(input);
					}
					break;
				}
			}
			_logger.Info("Returning validated data...");
			return inputNum;
		}
		private static bool AddMultIsValid(string[] input)
		{
			_logger.Info("Validating input...");
			const int MAX_DIGITS = 9;
			var operands = input.ToArray();

			if (operands.Length >= 2)
			{
				foreach (var num in operands)
				{
					if (!double.TryParse(num, out var addParsed) || num.ToString().Length > MAX_DIGITS)
					{
						_logger.Error($"Validation failed. Invalid number {num}.");
						Console.WriteLine($"Invalid number: {num}");
						return false;
					}
				}
			}
			else
			{
				_logger.Error("Validation failed. Not enough input numbers.");
				Console.WriteLine("Please enter at least 2 valid numbers separated by commas:");
				return false;
			}
			_logger.Info("Validation successful. The input is valid.");
			return true;
		}
		private static bool DoubleIsValid(string number)
		{
			_logger.Info("Validating input...");
			if (!string.IsNullOrEmpty(number))
			{
				const int MAX_DIGITS = 9;
				if (!double.TryParse(number, out var parsed) || number.Length > MAX_DIGITS)
				{
					_logger.Error($"Validation failed. Invalid number {number}.");
					Console.WriteLine($"Invalid number: {number}");
					Console.WriteLine("Please enter a valid number:");
					return false;
				}
			}
			else
			{
				_logger.Error("User didn't input a number.");
				Console.WriteLine("Please enter a number:");
				return false;
			}
			_logger.Info("Validation successful. The input is valid.");
			return true;
		}
		private static bool IntIsValid(string number)
		{
			_logger.Info("Validating input...");
			if (!string.IsNullOrEmpty(number))
			{
				const int MAX_DIGITS = 9;
				if (!int.TryParse(number, out var parsed) || number.Length > MAX_DIGITS)
				{
					_logger.Error($"Validation failed. Invalid number {number}.");
					Console.WriteLine($"Invalid number: {number}");
					Console.WriteLine("Please enter a valid number:");
					return false;
				}
			}
			else
			{
				_logger.Error("User didn't input a number.");
				Console.WriteLine("Please enter a number:");
				return false;
			}
			_logger.Info("Validation successful. The input is valid.");
			return true;
		}
	}
}
