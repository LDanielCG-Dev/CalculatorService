using System.Collections.Concurrent;
using NLog;

namespace CalculatorService.Models
{
	public class JournalResponse
	{
		private static ILogger _logger = LogManager.GetCurrentClassLogger();
		public List<JournalOperation> Operations { get; set; } = new List<JournalOperation>();
		public string? Message { get; set; }

		public static JournalResponse FromJournal(ConcurrentBag<string[]> records)
		{
			var response = new JournalResponse();
			_logger.Info("Adding records to journal...");
			foreach (var record in records)
			{
				response.Operations.Add(new JournalOperation
				{
					Operation = record[0],
					Calculation = record[1],
					Date = record[2]
				});
			}
			_logger.Info("Sending response back.");
			return response;
		}
	}
	public class JournalOperation
	{
		public string? Operation { get; set; }
		public string? Calculation { get; set; }
		public string? Date { get; set; }
	}
}