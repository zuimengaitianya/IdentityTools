//using IdentityTools.Models;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Filters;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using System.Text;
//using System.Threading.Tasks;
//using System.Web.Http.ModelBinding;

//namespace IdentityTools.Filters
//{
//    public class ModelActionFilter : ActionFilterAttribute
//    {
//        public override void OnActionExecuting(ActionExecutingContext context)
//        {
//            //base.OnActionExecuted(context);
//            //if (context.ModelState.IsValid == false)
//            //{
//            //    var errors = new Dictionary<string, IEnumerable<string>>();
//            //    //context.ModelState.ErrorCount

//            //    //foreach (KeyValuePair<string, ModelState> keyValue in context.ModelState)
//            //    //{
//            //    //    errors[keyValue.Key] = keyValue.Value.Errors.Select(e => e.ErrorMessage);
//            //    //}

//            //    context.HttpContext.Response.Body = context.HttpContext.Request.CreateResponse(HttpStatusCode.BadRequest, new
//            //    {
//            //        code = HttpStatusCode.BadRequest,//返回客户端的状态码
//            //        success = false,
//            //        error = errors//显示验证错误的信息
//            //    });
//            //}
//        }

//    }

//    public class APIResultFile : IResultFilter
//    {
//        public void OnResultExecuted(ResultExecutedContext context)
//        {

//        }

//        public void OnResultExecuting(ResultExecutingContext context)
//        {
//            if (context.Result is BadRequestObjectResult)
//            {
//                BadRequestObjectResult res = (BadRequestObjectResult)context.Result;
//                SerializableError obj = res.Value as SerializableError;
//                StringBuilder sb = new StringBuilder();
//                foreach (var item in obj)
//                {
//                    var vals = item.Value as string[];
//                    if (vals != null)
//                    {
//                        sb.AppendLine(vals[0]);
//                    }
//                }
//                context.Result = new JsonResult(ApiResult.BuildErrorApiResult(Comm.ErrorCode.ModelError, sb.ToString())) { StatusCode = 400 };
//                return;
//            }
//        }
//    }
//}
