namespace ProjectHorizon.Shared.Library.Common
{
    public class QueryResult
    {
        public bool IsSuccessful { get; set; }
        public string? Message { get; set; }
        public string? Type { get; set; }
        public int? Code { get; set; }
        public int? ErrorCode { get; set; }
        public object? Payload { get; set; }

        public static QueryResult Ok(object payload)
        {
            return new QueryResult() { IsSuccessful = true, Payload = payload };
        }

        public static QueryResult Ok(object payload, int code)
        {
            return new QueryResult() { IsSuccessful = true, Payload = payload, Code = code };
        }

        public static QueryResult Error(int errorCode)
        {
            return new QueryResult() { IsSuccessful = true, ErrorCode = errorCode };
        }

        public static QueryResult Error(string message, int errorCode)
        {
            return new QueryResult() { IsSuccessful = false, Message = message, ErrorCode = errorCode };
        }

        public static QueryResult Error(string message, int errorCode, string type)
        {
            return new QueryResult() { IsSuccessful = false, Message = message, ErrorCode = errorCode, Type = type };
        }

        public static QueryResult Error(string message, string type)
        {
            return new QueryResult() { IsSuccessful = false, Message = message, Type = type };
        }

        public static QueryResult Error(string message)
        {
            return new QueryResult() { IsSuccessful = false, Message = message };
        }
    }
}
