namespace Intuit.Api.Models
{
    public class OperationResult
    {
        public bool Result { get; set; } = false;
        public int Status { get; set; }
        public string Title { get; set; } = default!;
        public string Detail { get; set; } = default!;
    }
}
