using System;
using Microsoft.Extensions.Logging;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;

namespace EqidManager
{
    using IOCContainer = IServiceProvider;

    public class QuartzStartup
    {
        public IScheduler Scheduler { get; set; }

        private readonly ILogger _logger;
        private readonly IJobFactory iocJobfactory;
        public QuartzStartup(IOCContainer IocContainer, ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<QuartzStartup>();
            iocJobfactory = new IOCJobFactory(IocContainer);
            var schedulerFactory = new StdSchedulerFactory();
            Scheduler = schedulerFactory.GetScheduler().Result;
            Scheduler.JobFactory = iocJobfactory;
        }

        public void ScheduleJob()
        {
            _logger.LogInformation("Schedule job load as application start.");
            Scheduler.Start().Wait();
            var EqidCounterResetJob = JobBuilder.Create<EqidCounterResetJob>()
                .WithIdentity("EqidCounterResetJob")
                .Build();

            var EqidCounterResetJobTrigger = TriggerBuilder.Create()
                .WithIdentity("EqidCounterResetCron")
                .StartNow()                           // 该方法只是说该触发器在此时开始启用，触发由schedule决定
                //间隔30分钟
                .WithCronSchedule("0/30 * * * * ?")      // Seconds,Minutes,Hours，Day-of-Month，Month，Day-of-Week，Year（optional field）
                .Build();
            Scheduler.ScheduleJob(EqidCounterResetJob, EqidCounterResetJobTrigger).Wait();
        }

        public void EndScheduler()
        {
            if (Scheduler == null)
            {
                return;
            }

            if (Scheduler.Shutdown(waitForJobsToComplete: true).Wait(30000))
                Scheduler = null;
            else
            {
            }
            _logger.LogError("Schedule job upload as application stopped");
        }
    }
}
