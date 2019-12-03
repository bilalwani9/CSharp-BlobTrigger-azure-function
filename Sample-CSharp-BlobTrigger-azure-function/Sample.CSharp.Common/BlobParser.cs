namespace Sample.CSharp.Common
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using Sample.CSharp.Models; 
    using System.IO;

    /// <summary>
    /// Employee Blob Parser 
    /// </summary>
    public class BlobParser : IBlobParser<Employee>
    {
        private readonly JsonSerializerSettings _settings;

        public BlobParser()
        {
            var resolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };

            _settings = new JsonSerializerSettings
            {
                ContractResolver = resolver,
                Formatting = Formatting.Indented
            };
        }

        public Employee Parse(string input)
        {
            return JsonConvert.DeserializeObject<Employee>(input, _settings);
        }

        public Employee ReadAndParse(Stream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                var input = reader.ReadToEnd();
                return Parse(input);
            }
        }
    }
}
