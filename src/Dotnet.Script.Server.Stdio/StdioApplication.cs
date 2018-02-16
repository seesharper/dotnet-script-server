using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Dotnet.Script.Server.CQRS;
using Dotnet.Script.Server.Logging;
using Dotnet.Script.Server.NuGet;
using Newtonsoft.Json.Linq;

namespace Dotnet.Script.Server.Stdio
{
    public class StdioApplication 
    {
        private readonly TextWriter _output;
        private readonly TextReader _input;
        private readonly Logger _logger;

        private readonly Dictionary<string, Func<object, Task<object>>> _requestHandlers = new Dictionary<string, Func<object, Task<object>>>();

        public StdioApplication(TextWriter output, TextReader input, IQueryExecutor queryExecutor, Logger logger)
        {
            _output = output;
            _input = input;
            _logger = logger;
            _requestHandlers.Add(RequestType.PackageQuery, async (query) => await queryExecutor.ExecuteAsync(((JObject)query).ToObject<PackageQuery>()));            
        }

        public void Run()
        {
            var cancellationTokenSource = new CancellationTokenSource();

            Task.Run(async () =>
            {
                while (!cancellationTokenSource.IsCancellationRequested)
                {
                    var request = await _input.ReadRequestAsync();                    
                    if (request.Type == RequestType.Stop)
                    {
                        cancellationTokenSource.Cancel();
                    }
                    else
                    {
                        await HandleRequest(request);
                    }                    
                }
            });

            _logger.Info("Server started");
            cancellationTokenSource.Token.WaitHandle.WaitOne();
        }

        private async Task HandleRequest(Request request)
        {
            try
            {
                var result = await _requestHandlers[request.Type](request.Payload);
                var response = new Response(request.Id, request.Type, result);
                await _output.WriteResponseAsync(response);
            }
            catch (Exception e)
            {
                var response = new Response(request.Id, request.Type, e.ToString(), false);
                await _output.WriteResponseAsync(response);
            }            
        }
        
        private static class RequestType
        {
            public const string PackageQuery = "PackageQuery";
            public const string Stop = "Stop";
        }
    }
}