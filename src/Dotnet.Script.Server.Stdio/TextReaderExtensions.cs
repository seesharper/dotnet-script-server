using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Dotnet.Script.Server.Stdio
{
    public static class TextReaderExtensions
    {
        public static async Task<Request> ReadRequestAsync(this TextReader reader)
        {
            var line = await reader.ReadLineAsync();
            return JsonConvert.DeserializeObject<Request>(line);
        }
    }
}