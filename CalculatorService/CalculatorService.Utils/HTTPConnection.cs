using RestSharp;
using NLog;

namespace CalculatorService.Utils
{
	public static class HTTPConnection
	{
		private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();
		public static RestResponse<TResponse> SendRequestGetResponse<TRequest, TResponse>(RestClient client, string endPoint, Method method, string trackingID, params (string propertyName, object propertyValue)[] propertyValues) where TRequest : new() where TResponse : class, new()
		{
			var requestBody = BuildRequest<TRequest>(propertyValues);
			var response = SendData<TResponse>(client, endPoint, method, trackingID, requestBody);
			return response;
		}
		private static T BuildRequest<T>(params (string propertyName, object propertyValue)[] propertyValues) where T : new()
		{
			_logger.Info("Building request...");
			var instance = new T();
			foreach (var (propertyName, propertyValue) in propertyValues)
			{
				var property = typeof(T).GetProperty(propertyName);
				if (property != null)
				{
					property.SetValue(instance, propertyValue);
				}
			}
			_logger.Info("Request built.");
			return instance;
		}
		private static RestResponse<T>? SendData<T>(RestClient client, string endpoint, Method method, string trackingID, object requestBody) where T : class, new()
		{
			_logger.Info("Setting request headers...");
			var request = new RestRequest(endpoint, method);
			request.AddHeader("Accept", "application/json");
			request.AddHeader("Content-Type", "application/json");
			request.AddHeader("X-Evi-Tracking-Id", trackingID);
			_logger.Info("Setting request body...");
			request.AddJsonBody(requestBody);

			_logger.Info("Sending data to server...");
			var response = client.Execute<T>(request);
			if (response.StatusCode == System.Net.HttpStatusCode.OK)
			{
				_logger.Info("Data recieved from server. Returning response.");
				return response;
			}
			else
			{
				_logger.Error($"An error has occured: {response.ErrorMessage}.");
				Console.WriteLine($"Error: {response.ErrorMessage}");
				return null;
			}
		}
	}
}