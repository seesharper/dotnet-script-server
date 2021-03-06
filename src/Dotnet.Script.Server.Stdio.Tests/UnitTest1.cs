using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Dotnet.Script.Server.CQRS;
using Dotnet.Script.Server.NuGet;
using Dotnet.Script.Server.Scaffolding.UnitTesting;
using FluentAssertions;
using Moq;
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
            var output = new TestTextWriter(message =>
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

        [Fact]
        public void ShouldHandleUnknownPackage()
        {
            var output = new TestTextWriter(message =>
            {
                var response = JsonConvert.DeserializeObject<Response>(message);
                var results = ((JArray) response.PayLoad).ToObject<PackageQueryResult[]>();
                results.Should().BeEmpty();
            });

            var application = AppBuilder.Default.UseStartup(new ConfigurableStartup(r =>
            {
                r.RegisterInstance(CreateReaderWithPackageQuery("UnknownPackage"), "input");
                r.RegisterInstance<TextWriter>(output, "output");
            })).Build();

            application.Run();
        }

        [Fact]
        public void ShouldWriteException()
        {
            const string errorMessage = "This is an error message";
            var throwingQueryHandler = new Mock<IQueryHandler<PackageQuery, PackageQueryResult[]>>();
            throwingQueryHandler.Setup(h => h.HandleAsync(It.IsAny<PackageQuery>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception(errorMessage));


            var output = new TestTextWriter(message =>
            {                
                var response = JsonConvert.DeserializeObject<Response>(message);
                response.IsSuccessful.Should().BeFalse();
                ((string) response.PayLoad).Should().Contain(errorMessage);                
            });

            var application = AppBuilder.Default.UseStartup(new ConfigurableStartup(r =>
            {
                r.RegisterInstance(CreateReaderWithPackageQuery("UnknownPackage"), "input");
                r.RegisterInstance<TextWriter>(output, "output");
                r.RegisterInstance(throwingQueryHandler.Object);
            })).Build();

            application.Run();
        }

        [Fact]
        public void ShouldCreateTestScript()
        {
            var output = new TestTextWriter(message =>
            {
                var response = JsonConvert.DeserializeObject<Response>(message);
                response.IsSuccessful.Should().BeTrue();
            });

            using (var disposableFolder = new DisposableFolder())
            {
                var application = AppBuilder.Default.UseStartup(new ConfigurableStartup(r =>
                {
                    r.RegisterInstance(CreateReaderWithCreateUnitTestCommand(disposableFolder.Path), "input");
                    r.RegisterInstance<TextWriter>(output, "output");
                })).Build();

                application.Run();
            }

                
        }


        private static TextReader CreateReaderWithPackageQuery(string packageId)
        {
            var query = new PackageQuery(packageId, Environment.CurrentDirectory, true);
            var request = new Request(1, "PackageQuery", query);
            var json = JsonConvert.SerializeObject(request);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(json);
            sb.AppendLine(JsonConvert.SerializeObject(new Request(2, "Stop")));

            var stringReader = new StringReader(sb.ToString()); 
            return stringReader;
        }

        private static TextReader CreateReaderWithCreateUnitTestCommand(string folder)
        {
            var command = new CreateUnitTestCommand(folder);
            var request = new Request(1, "CreateUnitTestCommand", command);
            var json = JsonConvert.SerializeObject(request);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(json);
            sb.AppendLine(JsonConvert.SerializeObject(new Request(2, "Stop")));

            var stringReader = new StringReader(sb.ToString());
            return stringReader;
        }
    }
}
