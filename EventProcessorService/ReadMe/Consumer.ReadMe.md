# 1. EventProcessorClient
1. Coordinate with other instances  using same consumer group
2. Each processor must not own more than 3 partitions for 1 CPU core of host.

## 1.1 BlobStorage

### Best Practice
1. disable blob versioning and soft delete
#### Why
Since processor client enumerates blob storage frequently , this will increase the performance

2. Use unique blob container for each event hub and consumer group 

## 1.2 Controlling Processor Identity
1. Id should be stable .. i.e hardcoded or configured in appsettings to enable better correlation in logging. 
2. allows the processor to recover partition ownership when an application or host instance is restarted

https://github.com/Azure/azure-sdk-for-net/blob/main/sdk/eventhub/Azure.Messaging.EventHubs.Processor/samples/Sample02_EventProcessorConfiguration.md#configuring-the-azure-storage-account

## 1.3 Influencing load balancing behavior

### Load Balancing Strategy 
1. Greedy: Processor will attempt agressively for partitioning ownership. can cause duplicates in processing events.
2. Balanced: more balanced attempt. slowly claiming partitions until a stabilized distribution is achieved.
             can be slow first time partition assigned.

https://learn.microsoft.com/en-us/dotnet/api/azure.messaging.eventhubs.processor.loadbalancingstrategy?view=azure-dotnet

## 1.4 Load Balancing intervals
 Two Types.

### 1.4.1 LoadBalancingUpdateInterval: 
controls how frequently a load balancing cycle is run.
EventProcessorClient will refresh its ownership record for each partitions it owns

### 1.4.2 PartitionOwnershipExpirationInterval
controls how long an ownership record is considered valid
If the processor does not update an ownership record before this interval elapses, 
the partition represented by this record is considered unowned and is eligible to be claimed by another processor.

<code>
  PartitionOwnershipExpirationInterval >=  LoadBalancingUpdateInterval
</code>


## 1.5 Configuring the client retry thresholds

exponential back-off strategy by default

<code>
  RetryOptions = new EventHubsRetryOptions 
		{
		  Mode = EventHubsRetryMode.Exponential,  // Exponential, Fixed
		  MaximumRetries = 5, 
		  Delay = TimeSpan.FromMilliseconds(800), // Delay b/w two retries
		  MaximumDelay = TimeSpan.FromSeconds(10) // Maximum Delay
		  TryTimeout = TimeSpan.FromMinutes(1) // timeout for communicating with the Event Hubs service. default 1 minute
		}
</code>

### TryTimeout
timeout for communicating with the Event Hubs service. 
**Note:**
This value is important to the EventProcessorClient when StopProcessingAsync is invoked. 
Because the processor will allow processing for each owned partition to complete when shutting down, 
if reading from the Event Hubs service is taking place when the processor attempts to stop, 
it may take up to the duration of the TryTimeout to complete.