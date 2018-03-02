using System.IO;
using System.Reflection;

namespace Dotnet.Script.Server.Scaffolding
{
    internal static class TemplateLoader
    {
        public static string ReadTemplate(string name)
        {
            var resourceStream = typeof(TemplateLoader).GetTypeInfo().Assembly.GetManifestResourceStream($"Dotnet.Script.Server.Scaffolding.{name}");
            using (var streamReader = new StreamReader(resourceStream))
            {
                return streamReader.ReadToEnd();
            }
        }
    }
}