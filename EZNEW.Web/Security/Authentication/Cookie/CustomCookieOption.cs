using Microsoft.AspNetCore.Authentication.Cookies;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EZNEW.Web.Security.Authentication.Cookie
{
    /// <summary>
    /// 自定义Cookie配置
    /// </summary>
    public class CustomCookieOption
    {
        #region 属性

        /// <summary>
        /// Cookie配置
        /// </summary>
        public Action<CookieAuthenticationOptions> CookieConfiguration
        {
            get; set;
        }

        /// <summary>
        /// Cookie数据存取方式
        /// </summary>
        public CookieStorageModel StorageModel
        {
            get; set;
        } = CookieStorageModel.Default;

        /// <summary>
        /// 验证Cookie凭据方法
        /// </summary>
        public Func<CookieValidatePrincipalContext, Task<bool>> ValidatePrincipalAsync
        {
            get; set;
        }

        /// <summary>
        /// 是否强制执行自定义凭据验证，若设置为true则必须设置凭据验证方法，否则会自动验证失败
        /// </summary>
        public bool ForceValidatePrincipal
        {
            get; set;
        } = false;

        #endregion
    }
}
