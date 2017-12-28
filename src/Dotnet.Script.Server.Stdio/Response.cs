namespace Dotnet.Script.Server.Stdio
{
    public class Response
    {
        public Response(string type, object payLoad, bool isSuccessful = true)
        {
            Type = type;
            PayLoad = payLoad;
            IsSuccessful = isSuccessful;
        }

        public string Type { get; }

        public object PayLoad { get; }

        public bool IsSuccessful { get; }
    }
}