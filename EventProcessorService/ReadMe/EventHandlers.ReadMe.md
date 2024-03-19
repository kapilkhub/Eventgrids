# 2. Event Handlers

## 2.1 MaximumWaitTimeProperty
https://learn.microsoft.com/en-us/dotnet/api/azure.messaging.eventhubs.eventprocessorclientoptions.maximumwaittime?view=azure-dotnet#Azure_Messaging_EventHubs_EventProcessorClientOptions_MaximumWaitTime

When set, if no events are received before the timeout, ProcessEventAsync is called with a ProcessEventArgs instance that does not contain any event data.

property **HasEvent**  is intended to help detect this.
