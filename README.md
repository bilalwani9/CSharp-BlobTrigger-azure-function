# Sample C# BlobTrigger Azure Function
 

This is a Sample Azure Function Project developed in .Net Core, the project contains BlobTrigger Azure Function which reads blob json files and converts it to sample Employee Object with the help of BlobParser and converts the Employee Object to Json Message and finally publishes it to Azure Service Bus Queue.

## Prerequisites
To run this project, make sure that you have:

Azure subscription. If you don't have one, [create a free account](https://azure.microsoft.com/en-us/free/) before you begin.

1. Visual Studio 2019) or later.
2. .NET Standard SDK, version 2.0 or later.
3.  Already created Blob Container in Azure, if you haven't created please follow [Create a Blob container](https://docs.microsoft.com/en-us/azure/storage/blobs/storage-quickstart-blobs-portal)
4.  Already created Azure Service Bus Queue, if you haven't created please follow [Create a Queue in Azure Service Bus](https://dzone.com/articles/windows-azure-service-bus-1)

## Blob Trigger Azure Function in C#

```
namespace Sample.CSharp.BlobTrigger.AzureFunctions
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.ServiceBus;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using Sample.CSharp.Common;
    using Sample.CSharp.Models;
    using static Sample.CSharp.Common.Constants;
    /// <summary>
    /// This is sample azure function which is BlobTrigger and it reads .json files from Blob Storage and publishes the messages to Azure Service Bus Queue.
    /// </summary>
    public static class BlobTriggerAzureFunction
    {
        [FunctionName(nameof(BlobTriggerAzureFunction))]
        public static async Task Run([BlobTrigger(BlobContainerName + " /{ name}.json", Connection = AzureWebJobsStorage)]Stream employeeBlob, string name,
            [ServiceBus(ServiceBusQueueName, Connection = ServiceBusConnectionString, EntityType = EntityType.Queue)] IAsyncCollector<string> output,
            ILogger log)
        {
            var blobName = $"{name}.json";

            //Log Information
            log.LogInformation($"C# Blob trigger function Processed blob\n Name:{blobName} \n Size: {employeeBlob.Length} Bytes");

            try
            {
                // initialise Inversion of Control (IoC) Container.
                var container = IoCContainer.Create();

                //Get Blob Parser service from IoC Container
                var blobParser = container.GetRequiredService<IBlobParser<Employee>>();

                //Blob Parser Reads the input Stream and converts it to Employee Object
                var employee =  blobParser.ReadAndParse(employeeBlob);

                if (employee != null)
                {
                    //Serialize Employee Object to Json
                    var outBondJson = JsonConvert.SerializeObject(employee, JsonSerializerDefaultSettings);
                    var outBondSize = System.Text.Encoding.UTF8.GetByteCount(outBondJson);
                    log.LogInformation($"Outbound JSON Size is {outBondSize} bytes.");

                    //Add outBondJson Message to Azure Service Bus Queue
                    await output.AddAsync(outBondJson);

                    log.LogInformation($"Record Add successfully to Azure Service Bus Queue");
                }
            }
            catch (Exception e)
            {
                //Log Exception
                log.LogError($"Error: {e.Message}");
            }
        }
    }
}

```

## Publish Azure Function From Visual Studio to Azure

Right Click On Project AzureFuns.EventHub.Example and Click on Publish Follow documentation on Microsoft [How to Publish Azure Function from Visual Studio](https://tutorials.visualstudio.com/first-azure-function/publish)

### Update Azure Function App Setting with below Key Values
        a) AzureWebJobsStorage
        b) ServiceBusConnectionString
        

# Key Concepts used
1. Design pattern
2. Dependency Injection and IoC Containers
3. Separation of Concern
4. SOLID Principles
5. Azure Functions (Blob Trigger)
6. Blob Containers
7. Azure Service Bus
