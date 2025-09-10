namespace Intuit.Api.Models
{
    public class ServiceResult
    {
        public bool Result { get; init; }
        public string? Message { get; init; }      
        public object? Data { get; init; }         
        public int Status { get; init; } 


        public static ServiceResult Ok(object? data = null, string? message = null)
            => new() { Result = true, Message = message, Data = data, Status = StatusCodes.Status200OK };

        public static ServiceResult NotFound(string? message = null)
            => new() { Result = false, Message = message, Status = StatusCodes.Status404NotFound };

        public static ServiceResult Conflict(string message)
            => new() { Result = false, Message = message, Status = StatusCodes.Status409Conflict };

        public static ServiceResult Validation(string message)
            => new() { Result = false, Message = message, Status = StatusCodes.Status400BadRequest };

        public static ServiceResult Unexpected(string message)
            => new() { Result = false, Message = message, Status = StatusCodes.Status500InternalServerError };
    }
}
