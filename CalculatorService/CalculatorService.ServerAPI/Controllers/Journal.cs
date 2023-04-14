using CalculatorService.Models;
using NLog;
using System.Collections.Concurrent;

namespace CalculatorService.ServerAPI.Controllers
{
	public static class Journal
	{
		private static readonly NLog.ILogger _logger = LogManager.GetCurrentClassLogger();

		private static ConcurrentDictionary<string, ConcurrentBag<string[]>> _records = new ConcurrentDictionary<string, ConcurrentBag<string[]>>();

		public static void AddRecord(string trackingId, string operation, string calculation)
		{

			var date = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ");
			var record = new string[] { operation, calculation, date };

			// Get or create the bag of records for the specified trackingId
			ConcurrentBag<string[]> recordsForTrackingId = _records.GetOrAdd(trackingId, new ConcurrentBag<string[]>());

			// Add the new record to the bag
			_logger.Info("Adding operation to journal with tracking id {0}...", trackingId);
			recordsForTrackingId.Add(record);
			_logger.Info("Operation added to journal successfully.");
		}

		public static ConcurrentBag<string[]> TryGetRecordsForTrackingId(string trackingId)
		{
			_logger.Info("Checking TrackingID...");
			// Get the bag of records for the specified trackingId
			if (_records.TryGetValue(trackingId, out ConcurrentBag<string[]> recordsForTrackingId))
			{
				return recordsForTrackingId;
			}

			return null;
		}

		public static bool HasTrackingId(this JournalRequest @this)
		{
			_logger.Info("Validating TrackingID...");

			return !string.IsNullOrWhiteSpace(@this.Id);
		}

	}
}
