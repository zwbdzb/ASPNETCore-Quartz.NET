using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EqidManager.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger _logger;
        public HomeController(ILoggerFactory logFactory)
        {
            _logger = logFactory.CreateLogger<HomeController>();
        }

        public  IActionResult Index()
        {
            return Content("Please check the app logs.");
        }
        public IActionResult About()
        {
            ViewData["Message"] = System.Runtime.InteropServices.RuntimeInformation.OSDescription;
            return Content(ViewData["Message"].ToString());
        }

        public IActionResult Error()
        {
            var str = "There are some problems, please try again later.";
            var exceptionFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            if (exceptionFeature != null)
            {
                string routeWhereExceptionOccurred = exceptionFeature.Path;

                var exceptionThatOccurred = exceptionFeature.Error;
                _logger.LogError($"{routeWhereExceptionOccurred}:{exceptionThatOccurred}");
                str = exceptionThatOccurred.Message;
            }
            return Content(str);
        }
    }
}
