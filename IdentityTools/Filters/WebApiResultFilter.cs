using IdentityTools.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Web.Http.Filters;

namespace IdentityTools.Filters
{
    public class WebApiResultFilter : ActionFilterAttribute
    {

        public override void OnResultExecuting(ResultExecutingContext context)
        {
            //base.OnResultExecuting(context);
            //根据实际需求进行具体实现
            if (context.Result is ObjectResult)
            {
                var objectResult = context.Result as ObjectResult;
                if (objectResult.Value == null)
                {
                    context.Result = new ObjectResult(new ApiResult { IsSuccess = false, ErrorCode = 404, ErrorMsg = "未找到资源" });
                }
                else
                {
                    context.Result = new ObjectResult(ApiResult<object>.BuildSuccessApiResult(objectResult.Value));
                }
            }
            else if (context.Result is EmptyResult)
            {
                context.Result = new ObjectResult(new ApiResult { IsSuccess = false, ErrorCode = 404, ErrorMsg = "未找到资源" });
            }
            else if (context.Result is ContentResult)
            {
                context.Result = new ObjectResult(ApiResult<object>.BuildSuccessApiResult(context.Result));
            }
            else if (context.Result is StatusCodeResult)
            {
                context.Result = new ObjectResult(new ApiResult() { IsSuccess = true, ErrorCode = (context.Result as StatusCodeResult).StatusCode });
            }
            else if (context.Result is BadRequestObjectResult)
            {
                BadRequestObjectResult res = (BadRequestObjectResult)context.Result;
                SerializableError obj = res.Value as SerializableError;
                StringBuilder sb = new StringBuilder();
                foreach (var item in obj)
                {
                    var vals = item.Value as string[];
                    if (vals != null)
                    {
                        sb.AppendLine(vals[0]);
                    }
                }
                context.Result = new JsonResult(ApiResult.BuildErrorApiResult(Comm.ErrorCode.ModelError, sb.ToString())) { StatusCode = 400 };

            }
        }

    }
}
