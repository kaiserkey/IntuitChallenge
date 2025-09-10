using System.Text.Json;
using Intuit.Api.Logging;

namespace Intuit.Api.Data.Repository
{
    public class Repository
    {
        protected IntuitDBContext Context { get; set; } = default!;
        protected ILogService<LogService> LogService { get; set; } = default!;
        public string ToJson(object data)
        {
            return JsonSerializer.Serialize(data);
        }
    }
}
