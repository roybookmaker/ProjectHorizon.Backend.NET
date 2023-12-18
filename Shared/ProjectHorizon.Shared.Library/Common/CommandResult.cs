namespace ProjectHorizon.Shared.Library.Common
{
    public class CommandResult
    {
        public bool IsSuccessful { get; set; }
        public string Message { get; set; }
        public string Type { get; set; }
        public int ErrorCode { get; set; }
        public object Payload { get; set; }
        public bool IsDownloadFile { get; set; }
        public string Content { get; set; }
        public string? DocumentId { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public static CommandResult Ok()
        {
            return new CommandResult() { IsSuccessful = true };
        }
        public static CommandResult Ok(object payload)
        {
            return new CommandResult() { IsSuccessful = true, Payload = payload };
        }
        public static CommandResult OkWithDownloadFile(string content, string contentType, string filename, object payload = null)
        {
            return new CommandResult() { IsSuccessful = true, IsDownloadFile = true, Content = content, ContentType = contentType, FileName = filename, Payload = payload };
        }
        public static CommandResult Error(string message, int errorCode, string type)
        {
            return new CommandResult() { IsSuccessful = false, Message = message, ErrorCode = errorCode, Type = type };
        }

        public static CommandResult Error(string message, int errorCode)
        {
            return new CommandResult() { IsSuccessful = false, Message = message, ErrorCode = errorCode };
        }

        public static CommandResult Error(string message, string type)
        {
            return new CommandResult() { IsSuccessful = false, Message = message, Type = type };
        }

        public static CommandResult Error(string message)
        {
            return new CommandResult() { IsSuccessful = false, Message = message };
        }
    }
}
