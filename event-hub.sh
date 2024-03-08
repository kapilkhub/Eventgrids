
rgName="kapilkhubchandani$RANDOM"
region="westeurope"
az group create --name $rgName --location $region

# Create an Event Hubs namespace. Specify a name for the Event Hubs namespace.
namespaceName="khubchandanins"
az eventhubs namespace create --name $namespaceName --resource-group $rgName -l $region

# Create an event hub. Specify a name for the event hub. 
eventhubName="kapilehub"
az eventhubs eventhub create --name $eventhubName --resource-group $rgName --namespace-name $namespaceName

az group delete --name $rgName