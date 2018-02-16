namespace Dotnet.Script.Server.Stdio
{
    public class Response
    {
        public Response(long id, string type, object payLoad, bool isSuccessful = true)
        {
            Id = id;
            Type = type;
            PayLoad = payLoad;
            IsSuccessful = isSuccessful;
        }

        public long Id { get; }
        public string Type { get; }

        public object PayLoad { get; }

        public bool IsSuccessful { get; }
    }
}