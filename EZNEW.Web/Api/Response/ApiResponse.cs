using EZNEW.Framework.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace EZNEW.Web.Api.Response
{
    /// <summary>
    /// api response
    /// </summary>
    public class ApiResponse
    {
        #region propertys

        /// <summary>
        /// code
        /// </summary>
        public string Code
        {
            get; set;
        }

        /// <summary>
        /// message
        /// </summary>
        public string Message
        {
            get; set;
        }

        /// <summary>
        /// success
        /// </summary>
        public bool Success
        {
            get; set;
        }

        #endregion

        /// <summary>
        /// get result by response
        /// </summary>
        /// <returns></returns>
        public Result GetResult(Action<Result> configuration = null)
        {
            var result = new Result()
            {
                Code = Code,
                Message = Message,
                Success = Success
            };
            configuration?.Invoke(result);
            return result;
        }

        /// <summary>
        /// get result by response
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public Result<T> GetResult<T>(Action<Result<T>> configuration = null)
        {
            var result = new Result<T>()
            {
                Code = Code,
                Message = Message,
                Success = Success
            };
            configuration?.Invoke(result);
            return result;
        }
    }

    /// <summary>
    /// api response
    /// </summary>
    /// <typeparam name="T">data type</typeparam>
    public class ApiResponse<T> : ApiResponse
    {
        #region propertys

        /// <summary>
        /// data
        /// </summary>
        public T Data
        {
            get; set;
        }

        #endregion
    }
}
