using System;
using Microsoft.Extensions.Logging;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;

namespace EqidManager
{
    using IOCContainer = IServiceProvider;
    /// <summary>
    /// 为实现在Timer触发的时候使用依赖注入生成对应的Job任务，必须自定义Jobfactory;
    /// </summary>
    public class IOCJobFactory : IJobFactory
    {
        protected readonly IOCContainer Container;

        public IOCJobFactory(IOCContainer container)
        {
            Container = container;
        }

        //Called by the scheduler at the time of the trigger firing, in order to produce
        //     a Quartz.IJob instance on which to call Execute.
        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            return Container.GetService(bundle.JobDetail.JobType) as IJob;
        }

        // Allows the job factory to destroy/cleanup the job if needed.
        public void ReturnJob(IJob job)
        {
            // if the job implement IDisposable interface，you can dispose the job here.
        }
    }
}
