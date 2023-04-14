using CalculatorService.Models;
using LoggerService;
using System.Collections.Concurrent;

namespace CalculatorService.ServerAPI.Controllers
{
	public static class Journal
	{
		private static readonly ILoggerManager _logger = new LoggerManager();

		private static ConcurrentDictionary<string, ConcurrentBag<string[]>> _records = new ConcurrentDictionary<string, ConcurrentBag<string[]>>();

		public static void AddRecord(string trackingId, string operation, string calculation)
		{

			var date = DateTime.Now.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ");
			var record = new string[] { operation, calculation, date };

			// Get or create the bag of records for the specified trackingId
			ConcurrentBag<string[]> recordsForTrackingId = _records.GetOrAdd(trackingId, new ConcurrentBag<string[]>());

			// Add the new record to the bag
			_logger.LogInfo($"Adding operation to journal with tracking id {trackingId}...");
			recordsForTrackingId.Add(record);
			_logger.LogInfo("Operation added to journal successfully.");
		}

		public static ConcurrentBag<string[]> GetRecordsForTrackingId(string trackingId)
		{
			_logger.LogInfo("Checking TrackingID...");
			// Get the bag of records for the specified trackingId
			if (_records.TryGetValue(trackingId, out ConcurrentBag<string[]> recordsForTrackingId))
			{
				return recordsForTrackingId;
			}
			_logger.LogInfo("TrackingID not found.");
			var emptyBag = new ConcurrentBag<string[]>();
			emptyBag.Add(new string[] { "The journal is empty." });
			return emptyBag;
		}

		public static bool HasTrackingId(this JournalRequest @this)
		{
			_logger.LogInfo("Validating TrackingID...");

			return !string.IsNullOrEmpty(@this.Id?.Trim());
		}

		public static bool IsEmpty(this JournalRequest @this)
			=> @this.GetType().GetProperties().All(prop => prop.GetValue(@this) == null || (prop.PropertyType == typeof(int?) && (int?)prop.GetValue(@this) == 0));
	}
}
