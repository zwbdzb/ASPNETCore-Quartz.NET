using Microsoft.Extensions.Logging;
using Quartz;
using System.Threading.Tasks;

namespace EqidManager
{
    /// <summary>
    /// 
    /// </summary>
    public class EqidCounterResetJob : IJob
    {
        private readonly ILogger _logger;

        public EqidCounterResetJob(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<EqidCounterResetJob>();
        }

        public async Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation($"{nameof(EqidCounterResetJob)} Schedule job executed.");
            //TODO
            await Task.CompletedTask;
        }
    }
}
