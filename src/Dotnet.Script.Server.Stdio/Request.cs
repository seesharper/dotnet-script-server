namespace Dotnet.Script.Server.Stdio
{
    public class Request
    {
        public Request(string type, object payload = null)
        {
            Type = type;
            Payload = payload;
        }

        public string Type { get;  }

        public object Payload { get; }
    }
}