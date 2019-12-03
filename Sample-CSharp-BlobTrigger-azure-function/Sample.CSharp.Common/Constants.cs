namespace Sample.CSharp.Common
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization; 

    public static class Constants
    {
        public const string BlobContainerName = "<ADD BLOB CONTAINER NAME HERE>";
        public const string ServiceBusQueueName = "<ADD SERVICE BUS QUEUE NAME HERE>";

        //Make sure below setting/connection strings are available in Azure Function App Settings.
        public const string AzureWebJobsStorage = "AzureWebJobsStorage";
        public const string ServiceBusConnectionString = "ServiceBusConnectionString";

        public static readonly JsonSerializerSettings JsonSerializerDefaultSettings = new JsonSerializerSettings
        {
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            },
            Formatting = Formatting.Indented,
            NullValueHandling = NullValueHandling.Ignore,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };
    }
}
