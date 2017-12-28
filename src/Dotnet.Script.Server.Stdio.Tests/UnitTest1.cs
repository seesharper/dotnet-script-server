using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Dotnet.Script.Server.NuGet;
using FluentAssertions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Dotnet.Script.Server.Stdio.Tests
{
    public class UnitTest1
    {

        [Fact]
        public void ShouldHandleNuGetPackageSearch()
        {                        
            var output = new TestTextWriter((message) =>
            {
                var response = JsonConvert.DeserializeObject<Response>(message);
                var results = ((JArray) response.PayLoad).ToObject<PackageQueryResult[]>();
                results.Should().Contain(result => result.Id == "LightInject");
            });
            var application = AppBuilder.Default.UseStartup(new ConfigurableStartup(r =>
            {
                r.RegisterInstance(CreateReaderWithPackageQuery("LightInject"), "input");
                r.RegisterInstance<TextWriter>(output, "output");
            })).Build();
            application.Run();            
        }

        private TextReader CreateReaderWithPackageQuery(string packageId)
        {
            var query = new PackageQuery() { PackageId = packageId };
            var request = new Request("PackageQuery", query);
            var json = JsonConvert.SerializeObject(request);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(json);
            sb.AppendLine(JsonConvert.SerializeObject(new Request("Stop")));

            var stringReader = new StringReader(sb.ToString()); 
            return stringReader;
        }
    }
}
