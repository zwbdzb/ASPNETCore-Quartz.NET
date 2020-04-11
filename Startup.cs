using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Http;

namespace EqidManager
{
    public class Startup
    {
        public IConfiguration Configuration { get; set; }
        private readonly ILoggerFactory _loggerFactory;
        private readonly ILogger _logger;

        public Startup(IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            Configuration = configuration;
            _loggerFactory = loggerFactory;
            _logger = _loggerFactory.CreateLogger<Startup>();
        }

        public void ConfigureServices(IServiceCollection services)
        { 
            services.AddTransient<EqidCounterResetJob>();
            services.AddSingleton<QuartzStartup>();
            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime lifetime)
        {
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                app.UseExceptionHandler("/home/error");

            #region 程序启动和停止的任务
            var quartz = app.ApplicationServices.GetRequiredService<QuartzStartup>();
            lifetime.ApplicationStarted.Register(() =>
            {
                quartz.ScheduleJob();
            });
            lifetime.ApplicationStopped.Register(async () =>
            {
                _logger.LogCritical("The appaction is going down. please pay attention.");
                quartz.EndScheduler();
            });
            #endregion

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Please check the app logs.");
                });
            });
        }
    }
}