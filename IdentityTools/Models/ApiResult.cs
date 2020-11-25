using IdentityTools.Comm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace IdentityTools.Models
{
    public class ApiResult
    {
        [JsonPropertyName("isSuccess")]
        public bool IsSuccess { get; set; }
        [JsonPropertyName("errorCode")]
        public int ErrorCode { get; set; }
        [JsonPropertyName("errorMsg")]
        public string ErrorMsg { get; set; }


        public static ApiResult BuildErrorApiResult(ErrorCode errorCode, string errorMsg = "")
        {
            ApiResult apiResult = new ApiResult();
            apiResult.IsSuccess = false;
            //apiResult.ErrorCode = errorCode.;
            apiResult.ErrorMsg = errorMsg;

            return apiResult;
        }
    }

    public class ApiResult<T> : ApiResult where T : new()
    {
        [JsonPropertyName("data")]
        public T Data { get; set; }

        public static ApiResult<T> BuildSuccessApiResult(T data)
        {
            return new ApiResult<T>()
            {
                IsSuccess = true,
                Data = data
            };
        }
    }
}
