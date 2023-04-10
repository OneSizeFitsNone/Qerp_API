namespace Qerp.Interfaces
{
    public class ReturnResult
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public object? Object { get; set; }

        public ReturnResult(bool success, string message = "", object? oObject = null)
        {
            Success = success;
            Message = message;
            Object = oObject;
        }
    }
}
