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
