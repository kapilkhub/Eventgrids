using Azure.Messaging.EventHubs;
using Azure.Storage.Blobs;

namespace EventProcessorService
{
	internal  class EventProcessor
	{
		internal static EventProcessorClient CreateEventProcessorClient(IConfiguration configuration)
		{
			// Replace configuration values in appsettings.json.

			var storageClient = new BlobContainerClient(
				configuration.GetValue<string>("Storage:ConnectionString"),
				configuration.GetValue<string>("Storage:ContainerName"));

			return new EventProcessorClient(
				storageClient,
				configuration.GetValue<string>("EventHub:ConsumerGroup"),
				configuration.GetValue<string>("EventHub:ConnectionString"),
				configuration.GetValue<string>("EventHub:HubName"));
		}
	}
}
