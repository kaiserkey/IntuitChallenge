using System.Text.Json;
using Intuit.Api.Interfaces;
using Intuit.Api.Logging;

namespace Intuit.Api.Services
{
    public class AppService
    {
        protected ILogService<LogService> LogService { get; set; } = default!;
        protected IClientRepository ClientRepository { get; set; } = default!;

        public string ToJson(object data)
        {
            return JsonSerializer.Serialize(data);
        }

    }
}
