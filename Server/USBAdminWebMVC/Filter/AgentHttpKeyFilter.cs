using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using USBCommon;

namespace USBAdminWebMVC
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AgentHttpKeyFilter : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //Debugger.Break();

            var headers = context.HttpContext.Request.Headers;

            if (!CheckAgentHttpKey(headers))
            {
                context.Result = new BadRequestObjectResult("no access right.") { StatusCode = 401 };
                return;
            }
            else
            {
                await next();
                return;
            }           
        }

        private bool CheckAgentHttpKey(IHeaderDictionary headers)
        {
            if (headers.TryGetValue("AgentHttpKey", out Microsoft.Extensions.Primitives.StringValues keyValue))
            {
                string key = keyValue[0];

                if (string.IsNullOrWhiteSpace(key))
                {
                    return false;
                }

                if (key == USBAdminHelp.AgentHttpKey)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
