using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Dotnet.Script.Server.Stdio
{
    public static class TextWriterExtensions
    {
        public static async Task WriteResponseAsync(this TextWriter textWriter, Response response)
        {
            var json = JsonConvert.SerializeObject(response);
            await textWriter.WriteLineAsync(json);
        }
    }
}