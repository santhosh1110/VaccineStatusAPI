using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Api.Helpers
{
    public class LoggingActionFilter : IActionFilter
    {
        ILogger _logger;
        public LoggingActionFilter(ILoggerFactory loggerFactory)
        {

            _logger = loggerFactory.CreateLogger<LoggingActionFilter>();
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            string actionParameters=string.Empty;
            if(context.ActionArguments.Values.Count>0)
                actionParameters= "Parameters : ";
            foreach(object arg in context.ActionArguments.Values)
                actionParameters+= $"{arg.ToString()} ";

            // do something before the action executes
            _logger.LogInformation($"Action :'{context.ActionDescriptor.DisplayName} { actionParameters } ' starting");
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // do something after the action executes
            _logger.LogInformation($"Action '{context.ActionDescriptor.DisplayName}' completed");
        }
    }
}
