using LoggerService;
using System.Collections.Concurrent;

namespace CalculatorService.Models
{
	public class JournalResponse
	{
		private static readonly ILoggerManager _logger = new LoggerManager();
		public List<JournalOperation> Operations { get; set; } = new List<JournalOperation>();
		public string? Message { get; set; }

		public static JournalResponse FromJournal(ConcurrentBag<string[]> records)
		{
			var response = new JournalResponse();
			_logger.LogInfo("Adding records to journal...");
			foreach (var record in records)
			{
				response.Operations.Add(new JournalOperation
				{
					Operation = record[0],
					Calculation = record[1],
					Date = record[2]
				});
			}
			_logger.LogInfo("Sending response back.");
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