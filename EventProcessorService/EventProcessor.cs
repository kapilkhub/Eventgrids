using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Processor;
using Azure.Storage.Blobs;

namespace EventProcessorService
{
	internal  class EventProcessor
	{
		internal static EventProcessorClient CreateEventProcessorClient(IConfiguration configuration)
		{
			var processorOptions = new EventProcessorClientOptions
			{
				Identifier = Guid.NewGuid().ToString(),// should be stable Id instead of Random Guid 
				#region  Load balancing options
				LoadBalancingStrategy = LoadBalancingStrategy.Greedy,
				LoadBalancingUpdateInterval = TimeSpan.FromSeconds(10),
				PartitionOwnershipExpirationInterval = TimeSpan.FromSeconds(30),  //PartitionOwnershipExpirationInterval >= 3(LoadBalancingUpdateInterval)
				#endregion

				#region Retry Options
				RetryOptions = new EventHubsRetryOptions 
				{
					Mode = EventHubsRetryMode.Exponential,
					MaximumRetries = 5,
					Delay = TimeSpan.FromMilliseconds(800),
					MaximumDelay = TimeSpan.FromSeconds(10),
					TryTimeout = TimeSpan.FromMinutes(1)
				}
				#endregion

			};
			// Replace configuration values in appsettings.json.

			var storageClient = new BlobContainerClient(
				configuration.GetValue<string>("Storage:ConnectionString"),
				configuration.GetValue<string>("Storage:ContainerName"));

			return new EventProcessorClient(
				storageClient,
				configuration.GetValue<string>("EventHub:ConsumerGroup"),
				configuration.GetValue<string>("EventHub:ConnectionString"),
				configuration.GetValue<string>("EventHub:HubName"),
				processorOptions
				);
		}
	}
}
