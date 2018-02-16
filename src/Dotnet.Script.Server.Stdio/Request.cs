namespace Dotnet.Script.Server.Stdio
{
    public class Request
    {
        public Request(long id, string type, object payload = null)
        {
            Id = id;
            Type = type;
            Payload = payload;            
        }

        public string Type { get;  }

        public object Payload { get; }
        public long Id { get; }
    }
}