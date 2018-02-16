using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Dotnet.Script.Server.Stdio
{
    public static class TextWriterExtensions
    {
        public static async Task WriteResponseAsync(this TextWriter textWriter, Response response)
        {
            var json = JsonConvert.SerializeObject(response,new JsonSerializerSettings(){ContractResolver = new CamelCasePropertyNamesContractResolver()});
            await textWriter.WriteLineAsync(json);
        }
    }
}