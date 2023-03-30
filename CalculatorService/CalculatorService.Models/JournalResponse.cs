using System.Collections.Concurrent;

namespace CalculatorService.Models
{
	public class JournalResponse
	{
		public List<JournalOperation> Operations { get; set; } = new List<JournalOperation>();

		public static JournalResponse FromJournal(ConcurrentBag<string[]> records)
		{
			var response = new JournalResponse();

			foreach (var record in records)
			{
				response.Operations.Add(new JournalOperation
				{
					Operation = record[0],
					Calculation = record[1],
					Date = record[2]
				});
			}

			return response;
		}
	}

	public class JournalOperation
	{
		public string Operation { get; set; }
		public string Calculation { get; set; }
		public string Date { get; set; }
	}
}