namespace Maersk.Rule.Engine.Models
{
    public class Response
    {
        public string Status { get; set; }
        public string StatusMessage { get; set; }

        public Response(string status, string statusMessage = "")
        {
            Status = status;
            StatusMessage = statusMessage;
        }
    }
}
