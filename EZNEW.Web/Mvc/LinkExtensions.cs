using EZNEW.Framework.IoC;
using EZNEW.Web.Security.Authorization;
using EZNEW.Web.Utility;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EZNEW.Framework.Extension;
using EZNEW.Framework.Application;

namespace EZNEW.Web.Mvc
{
    public static class LinkExtensions
    {
        /// <summary>
        /// Auth Link
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static IHtmlContent AuthLink(this IHtmlHelper htmlHelper, AuthButtonOptions options)
        {
            if (options == null || (options.UseNowVerifyResult && !options.AllowAccess))
            {
                return HtmlString.Empty;
            }
            if (!options.UseNowVerifyResult)
            {
                var allowAccess = AuthorizeManager.AuthorizeVerifyAsync(new AuthorizeVerifyRequest()
                {
                    ActionCode = options.AuthorizeFunc?.ActionCode,
                    ControllerCode = options.AuthorizeFunc?.ControllerCode,
                    Application = ApplicationManager.Current,
                    Claims = HttpContextHelper.Current.User.Claims.ToDictionary(c => c.Type, c => c.Value)
                }).Result?.AllowAccess ?? false;
                if (!allowAccess)
                {
                    return HtmlString.Empty;
                }
            }
            var btnTagBuilder = new TagBuilder("a");
            var btnHtmlAttributes = options.HtmlAttributes ?? new Dictionary<string, object>();
            if (!btnHtmlAttributes.ContainsKey("href"))
            {
                btnHtmlAttributes.Add("href", "javascript:void(0)");
            }
            btnTagBuilder.MergeAttributes(btnHtmlAttributes);
            if (options.UseIco)
            {
                var icoTagBuilder = new TagBuilder("i");
                icoTagBuilder.MergeAttributes(options.IcoHtmlAttributes);
                btnTagBuilder.InnerHtml.AppendHtml(icoTagBuilder);
                btnTagBuilder.InnerHtml.Append(" ");
            }
            btnTagBuilder.InnerHtml.Append(options.Text);
            return btnTagBuilder;
        }

        /// <summary>
        /// Dropdown Auth Link
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="text"></param>
        /// <param name="authorizeFunc"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static IHtmlContent DropdownAuthLink(this IHtmlHelper htmlHelper, string text, AuthorizeFunc authorizeFunc, object htmlAttributes = null,object icoHtmlAttributes=null)
        {
            return AuthLink(htmlHelper, new AuthButtonOptions()
            {
                Text = text,
                AuthorizeFunc = authorizeFunc,
                HtmlAttributes = htmlAttributes?.ObjectToDcitionary(),
                UseIco=icoHtmlAttributes!=null,
                IcoHtmlAttributes=icoHtmlAttributes?.ObjectToDcitionary()
            });
        }

        /// <summary>
        /// Dropdown Auth Link
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="text"></param>
        /// <param name="controllerCode"></param>
        /// <param name="actionCode"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static IHtmlContent DropdownAuthLink(this IHtmlHelper htmlHelper, string text, string controllerCode, string actionCode, object htmlAttributes = null,object icoHtmlAttributes=null)
        {
            return DropdownAuthLink(htmlHelper, text, new AuthorizeFunc(controllerCode, actionCode), htmlAttributes, icoHtmlAttributes);
        }
    }
}
