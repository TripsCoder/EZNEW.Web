using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;
using EZNEW.Framework.Extension;
using EZNEW.Web.Security.Authorization;
using EZNEW.Framework.IoC;
using Microsoft.Extensions.Options;
using EZNEW.Web.Utility;
using System.Linq;
using EZNEW.Framework.Application;

namespace EZNEW.Web.Mvc
{
    public static class ButtonExtensions
    {
        #region Authorize Button

        /// <summary>
        /// 生成按钮
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static IHtmlContent AuthButton(this IHtmlHelper htmlHelper, AuthButtonOptions options)
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
            var btnTagBuilder = new TagBuilder("button");
            var btnHtmlAttributes = options.HtmlAttributes ?? new Dictionary<string, object>();
            if (!btnHtmlAttributes.ContainsKey("type"))
            {
                btnHtmlAttributes.Add("type", "button");
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

        #region Default Auth Button

        /// <summary>
        /// Auth Button
        /// </summary>
        /// <param name="htmlHelper">htmlHelper</param>
        /// <param name="text">text</param>
        /// <param name="authorizeFunc">authorize func</param>
        /// <param name="htmlAttributes">html attributes</param>
        /// <returns></returns>
        public static IHtmlContent AuthButton(this IHtmlHelper htmlHelper, string text, AuthorizeFunc authorizeFunc, object htmlAttributes = null)
        {
            return AuthButton(htmlHelper, new AuthButtonOptions()
            {
                Text = text,
                AuthorizeFunc = authorizeFunc,
                HtmlAttributes = htmlAttributes?.ObjectToDcitionary()
            });
        }

        /// <summary>
        /// Auth Button
        /// </summary>
        /// <param name="htmlHelper">html helper</param>
        /// <param name="text">text</param>
        /// <param name="controllerCode">controller code</param>
        /// <param name="actionCode">action code</param>
        /// <param name="htmlAttributes">html attributes</param>
        /// <returns></returns>
        public static IHtmlContent AuthButton(this IHtmlHelper htmlHelper, string text, string controllerCode, string actionCode, object htmlAttributes = null)
        {
            return AuthButton(htmlHelper, text, new AuthorizeFunc(controllerCode, actionCode), htmlAttributes);
        }

        /// <summary>
        /// Auth Button
        /// </summary>
        /// <param name="htmlHelper">html helper</param>
        /// <param name="text">text</param>
        /// <param name="allowAccess">allow access</param>
        /// <param name="htmlAttributes">html attributes</param>
        /// <returns></returns>
        public static IHtmlContent AuthButton(this IHtmlHelper htmlHelper, string text, bool allowAccess, object htmlAttributes = null)
        {
            return AuthButton(htmlHelper, new AuthButtonOptions()
            {
                Text = text,
                AllowAccess = allowAccess,
                UseNowVerifyResult = true,
                HtmlAttributes = htmlAttributes?.ObjectToDcitionary()
            });
        }

        #endregion

        #region Ico Auth Button

        /// <summary>
        /// Ico Auth Button
        /// </summary>
        /// <param name="htmlHelper">htmlHelper</param>
        /// <param name="text">text</param>
        /// <param name="authorizeFunc">authorize func</param>
        /// <param name="htmlAttributes">html attributes</param>
        /// <returns></returns>
        public static IHtmlContent AuthIcoButton(this IHtmlHelper htmlHelper, string text, AuthorizeFunc authorizeFunc, object htmlAttributes = null, object icoHtmlAttributes = null)
        {
            return AuthButton(htmlHelper, new AuthButtonOptions()
            {
                Text = text,
                AuthorizeFunc = authorizeFunc,
                HtmlAttributes = htmlAttributes?.ObjectToDcitionary(),
                IcoHtmlAttributes = icoHtmlAttributes?.ObjectToDcitionary(),
                UseIco = true
            });
        }

        /// <summary>
        /// Ico Auth Button
        /// </summary>
        /// <param name="htmlHelper">html helper</param>
        /// <param name="text">text</param>
        /// <param name="controllerCode">controller code</param>
        /// <param name="actionCode">action code</param>
        /// <param name="htmlAttributes">html attributes</param>
        /// <returns></returns>
        public static IHtmlContent AuthIcoButton(this IHtmlHelper htmlHelper, string text, string controllerCode, string actionCode, object htmlAttributes = null, object icoHtmlAttributes = null)
        {
            return AuthIcoButton(htmlHelper, text, new AuthorizeFunc(controllerCode, actionCode), htmlAttributes, icoHtmlAttributes);
        }

        /// <summary>
        /// Ico Auth Button
        /// </summary>
        /// <param name="htmlHelper">html helper</param>
        /// <param name="text">text</param>
        /// <param name="allowAccess">allow access</param>
        /// <param name="htmlAttributes">html attributes</param>
        /// <returns></returns>
        public static IHtmlContent AuthIcoButton(this IHtmlHelper htmlHelper, string text, bool allowAccess, object htmlAttributes = null, object icoHtmlAttributes = null)
        {
            return AuthButton(htmlHelper, new AuthButtonOptions()
            {
                Text = text,
                AllowAccess = allowAccess,
                UseNowVerifyResult = true,
                HtmlAttributes = htmlAttributes?.ObjectToDcitionary(),
                IcoHtmlAttributes = icoHtmlAttributes?.ObjectToDcitionary(),
                UseIco = true
            });
        }

        #endregion

        #region Pre Attribute Auth Button

        public static IHtmlContent PreAttributeAuthButton(this IHtmlHelper htmlHelper, string text, AuthorizeFunc authorizeFunc, string attrName, List<string> attrValues = null, object htmlAttributes = null, object icoHtmlAttributes = null)
        {
            var attributesDic = htmlAttributes?.ObjectToDcitionary() ?? new Dictionary<string, object>();
            if (!attrValues.IsNullOrEmpty())
            {
                if (attributesDic.ContainsKey(attrName))
                {
                    attributesDic[attrName] += string.Join(" ", attrValues.ToArray());
                }
                else
                {
                    attributesDic.Add(attrName, string.Join(" ", attrValues.ToArray()));
                }
            }
            return AuthButton(htmlHelper, new AuthButtonOptions()
            {
                Text = text,
                AuthorizeFunc = authorizeFunc,
                HtmlAttributes = attributesDic,
                UseIco = icoHtmlAttributes != null,
                IcoHtmlAttributes = icoHtmlAttributes?.ObjectToDcitionary()
            });
        }

        public static IHtmlContent PreAttributeAuthButton(this IHtmlHelper htmlHelper, string text, bool allowAccess, string attrName, List<string> attrValues = null, object htmlAttributes = null, object icoHtmlAttributes = null)
        {
            var attributesDic = htmlAttributes?.ObjectToDcitionary() ?? new Dictionary<string, object>();
            if (!attrValues.IsNullOrEmpty())
            {
                if (attributesDic.ContainsKey(attrName))
                {
                    attributesDic[attrName] += string.Join(" ", attrValues.ToArray());
                }
                else
                {
                    attributesDic.Add(attrName, string.Join(" ", attrValues.ToArray()));
                }
            }
            return AuthButton(htmlHelper, new AuthButtonOptions()
            {
                Text = text,
                UseNowVerifyResult = true,
                AllowAccess = allowAccess,
                HtmlAttributes = attributesDic,
                UseIco = icoHtmlAttributes != null,
                IcoHtmlAttributes = icoHtmlAttributes?.ObjectToDcitionary()
            });
        }

        public static IHtmlContent PreClassAuthButton(this IHtmlHelper htmlHelper, string text, AuthorizeFunc authorizeFunc, List<string> classValues = null, object htmlAttributes = null, object icoHtmlAttributes = null)
        {
            return PreAttributeAuthButton(htmlHelper, text, authorizeFunc, "class", classValues, htmlAttributes, icoHtmlAttributes);
        }

        public static IHtmlContent PreClassAuthButton(this IHtmlHelper htmlHelper, string text, bool allowAccess, List<string> classValues = null, object htmlAttributes = null, object icoHtmlAttributes = null)
        {
            return PreAttributeAuthButton(htmlHelper, text, allowAccess, "class", classValues, htmlAttributes, icoHtmlAttributes);
        }

        #endregion

        #region Primary

        public static IHtmlContent PrimaryAuthButton(this IHtmlHelper htmlHelper, string text, string controllerCode, string actionCode, object htmlAttributes = null, object icoHtmlAttributes = null)
        {
            return PreClassAuthButton(htmlHelper, text, new AuthorizeFunc(controllerCode, actionCode), new List<string>()
            {
                "btn",
                "btn-primary"
            }, htmlAttributes, icoHtmlAttributes);
        }

        public static IHtmlContent PrimaryAuthButton(this IHtmlHelper htmlHelper, string text, bool allowAccess, object htmlAttributes = null, object icoHtmlAttributes = null)
        {
            return PreClassAuthButton(htmlHelper, text, allowAccess, new List<string>()
            {
                "btn",
                "btn-primary"
            }, htmlAttributes, icoHtmlAttributes);
        }

        public static IHtmlContent SmallPrimaryAuthButton(this IHtmlHelper htmlHelper, string text, string controllerCode, string actionCode, object htmlAttributes = null, object icoHtmlAttributes = null)
        {
            return PreClassAuthButton(htmlHelper, text, new AuthorizeFunc(controllerCode, actionCode), new List<string>()
            {
                "btn",
                "btn-primary",
                "btn-sm"
            }, htmlAttributes, icoHtmlAttributes);
        }

        public static IHtmlContent SmallPrimaryAuthButton(this IHtmlHelper htmlHelper, string text, bool allowAccess, object htmlAttributes = null, object icoHtmlAttributes = null)
        {
            return PreClassAuthButton(htmlHelper, text, allowAccess, new List<string>()
            {
                "btn",
                "btn-primary",
                "btn-sm"
            }, htmlAttributes, icoHtmlAttributes);
        }

        #endregion

        #region Secondary

        public static IHtmlContent SecondaryAuthButton(this IHtmlHelper htmlHelper, string text, string controllerCode, string actionCode, object htmlAttributes = null, object icoHtmlAttributes = null)
        {
            return PreClassAuthButton(htmlHelper, text, new AuthorizeFunc(controllerCode, actionCode), new List<string>()
            {
                "btn",
                "btn-secondary"
            }, htmlAttributes, icoHtmlAttributes);
        }

        public static IHtmlContent SecondaryAuthButton(this IHtmlHelper htmlHelper, string text, bool allowAccess, object htmlAttributes = null, object icoHtmlAttributes = null)
        {
            return PreClassAuthButton(htmlHelper, text, allowAccess, new List<string>()
            {
                "btn",
                "btn-secondary"
            }, htmlAttributes, icoHtmlAttributes);
        }

        public static IHtmlContent SmallSecondaryAuthButton(this IHtmlHelper htmlHelper, string text, string controllerCode, string actionCode, object htmlAttributes = null, object icoHtmlAttributes = null)
        {
            return PreClassAuthButton(htmlHelper, text, new AuthorizeFunc(controllerCode, actionCode), new List<string>()
            {
                "btn",
                "btn-secondary",
                "btn-sm"
            }, htmlAttributes, icoHtmlAttributes);
        }

        public static IHtmlContent SmallSecondaryAuthButton(this IHtmlHelper htmlHelper, string text, bool allowAccess, object htmlAttributes = null, object icoHtmlAttributes = null)
        {
            return PreClassAuthButton(htmlHelper, text, allowAccess, new List<string>()
            {
                "btn",
                "btn-secondary",
                "btn-sm"
            }, htmlAttributes, icoHtmlAttributes);
        }

        #endregion

        #region Success

        public static IHtmlContent SuccessAuthButton(this IHtmlHelper htmlHelper, string text, string controllerCode, string actionCode, object htmlAttributes = null, object icoHtmlAttributes = null)
        {
            return PreClassAuthButton(htmlHelper, text, new AuthorizeFunc(controllerCode, actionCode), new List<string>()
            {
                "btn",
                "btn-success"
            }, htmlAttributes, icoHtmlAttributes);
        }

        public static IHtmlContent SuccessAuthButton(this IHtmlHelper htmlHelper, string text, bool allowAccess, object htmlAttributes = null, object icoHtmlAttributes = null)
        {
            return PreClassAuthButton(htmlHelper, text, allowAccess, new List<string>()
            {
                "btn",
                "btn-success"
            }, htmlAttributes, icoHtmlAttributes);
        }

        public static IHtmlContent SmallSuccessAuthButton(this IHtmlHelper htmlHelper, string text, string controllerCode, string actionCode, object htmlAttributes = null, object icoHtmlAttributes = null)
        {
            return PreClassAuthButton(htmlHelper, text, new AuthorizeFunc(controllerCode, actionCode), new List<string>()
            {
                "btn",
                "btn-success",
                "btn-sm"
            }, htmlAttributes, icoHtmlAttributes);
        }

        public static IHtmlContent SmallSuccessAuthButton(this IHtmlHelper htmlHelper, string text, bool allowAccess, object htmlAttributes = null, object icoHtmlAttributes = null)
        {
            return PreClassAuthButton(htmlHelper, text, allowAccess, new List<string>()
            {
                "btn",
                "btn-success",
                "btn-sm"
            }, htmlAttributes, icoHtmlAttributes);
        }

        #endregion

        #region Danger

        public static IHtmlContent DangerAuthButton(this IHtmlHelper htmlHelper, string text, string controllerCode, string actionCode, object htmlAttributes = null, object icoHtmlAttributes = null)
        {
            return PreClassAuthButton(htmlHelper, text, new AuthorizeFunc(controllerCode, actionCode), new List<string>()
            {
                "btn",
                "btn-danger"
            }, htmlAttributes, icoHtmlAttributes);
        }

        public static IHtmlContent DangerAuthButton(this IHtmlHelper htmlHelper, string text, bool allowAccess, object htmlAttributes = null, object icoHtmlAttributes = null)
        {
            return PreClassAuthButton(htmlHelper, text, allowAccess, new List<string>()
            {
                "btn",
                "btn-danger"
            }, htmlAttributes, icoHtmlAttributes);
        }

        public static IHtmlContent SmallDangerAuthButton(this IHtmlHelper htmlHelper, string text, string controllerCode, string actionCode, object htmlAttributes = null, object icoHtmlAttributes = null)
        {
            return PreClassAuthButton(htmlHelper, text, new AuthorizeFunc(controllerCode, actionCode), new List<string>()
            {
                "btn",
                "btn-danger",
                "btn-sm"
            }, htmlAttributes, icoHtmlAttributes);
        }

        public static IHtmlContent SmallDangerAuthButton(this IHtmlHelper htmlHelper, string text, bool allowAccess, object htmlAttributes = null, object icoHtmlAttributes = null)
        {
            return PreClassAuthButton(htmlHelper, text, allowAccess, new List<string>()
            {
                "btn",
                "btn-danger",
                "btn-sm"
            }, htmlAttributes, icoHtmlAttributes);
        }

        #endregion

        #region Warning

        public static IHtmlContent WarningAuthButton(this IHtmlHelper htmlHelper, string text, string controllerCode, string actionCode, object htmlAttributes = null, object icoHtmlAttributes = null)
        {
            return PreClassAuthButton(htmlHelper, text, new AuthorizeFunc(controllerCode, actionCode), new List<string>()
            {
                "btn",
                "btn-warning"
            }, htmlAttributes, icoHtmlAttributes);
        }

        public static IHtmlContent WarningAuthButton(this IHtmlHelper htmlHelper, string text, bool allowAccess, object htmlAttributes = null, object icoHtmlAttributes = null)
        {
            return PreClassAuthButton(htmlHelper, text, allowAccess, new List<string>()
            {
                "btn",
                "btn-warning"
            }, htmlAttributes, icoHtmlAttributes);
        }

        public static IHtmlContent SmallWarningAuthButton(this IHtmlHelper htmlHelper, string text, string controllerCode, string actionCode, object htmlAttributes = null, object icoHtmlAttributes = null)
        {
            return PreClassAuthButton(htmlHelper, text, new AuthorizeFunc(controllerCode, actionCode), new List<string>()
            {
                "btn",
                "btn-warning",
                "btn-sm"
            }, htmlAttributes, icoHtmlAttributes);
        }

        public static IHtmlContent SmallWarningAuthButton(this IHtmlHelper htmlHelper, string text, bool allowAccess, object htmlAttributes = null, object icoHtmlAttributes = null)
        {
            return PreClassAuthButton(htmlHelper, text, allowAccess, new List<string>()
            {
                "btn",
                "btn-warning",
                "btn-sm"
            }, htmlAttributes, icoHtmlAttributes);
        }

        #endregion

        #region Info

        public static IHtmlContent InfoAuthButton(this IHtmlHelper htmlHelper, string text, string controllerCode, string actionCode, object htmlAttributes = null, object icoHtmlAttributes = null)
        {
            return PreClassAuthButton(htmlHelper, text, new AuthorizeFunc(controllerCode, actionCode), new List<string>()
            {
                "btn",
                "btn-info"
            }, htmlAttributes, icoHtmlAttributes);
        }

        public static IHtmlContent InfoAuthButton(this IHtmlHelper htmlHelper, string text, bool allowAccess, object htmlAttributes = null, object icoHtmlAttributes = null)
        {
            return PreClassAuthButton(htmlHelper, text, allowAccess, new List<string>()
            {
                "btn",
                "btn-info"
            }, htmlAttributes, icoHtmlAttributes);
        }

        public static IHtmlContent SmallInfoAuthButton(this IHtmlHelper htmlHelper, string text, string controllerCode, string actionCode, object htmlAttributes = null, object icoHtmlAttributes = null)
        {
            return PreClassAuthButton(htmlHelper, text, new AuthorizeFunc(controllerCode, actionCode), new List<string>()
            {
                "btn",
                "btn-info",
                "btn-sm"
            }, htmlAttributes, icoHtmlAttributes);
        }

        public static IHtmlContent SmallInfoAuthButton(this IHtmlHelper htmlHelper, string text, bool allowAccess, object htmlAttributes = null, object icoHtmlAttributes = null)
        {
            return PreClassAuthButton(htmlHelper, text, allowAccess, new List<string>()
            {
                "btn",
                "btn-info",
                "btn-sm"
            }, htmlAttributes, icoHtmlAttributes);
        }

        #endregion

        #region Light

        public static IHtmlContent LightAuthButton(this IHtmlHelper htmlHelper, string text, string controllerCode, string actionCode, object htmlAttributes = null, object icoHtmlAttributes = null)
        {
            return PreClassAuthButton(htmlHelper, text, new AuthorizeFunc(controllerCode, actionCode), new List<string>()
            {
                "btn",
                "btn-light"
            }, htmlAttributes, icoHtmlAttributes);
        }

        public static IHtmlContent LightAuthButton(this IHtmlHelper htmlHelper, string text, bool allowAccess, object htmlAttributes = null, object icoHtmlAttributes = null)
        {
            return PreClassAuthButton(htmlHelper, text, allowAccess, new List<string>()
            {
                "btn",
                "btn-light"
            }, htmlAttributes, icoHtmlAttributes);
        }

        public static IHtmlContent SmallLightAuthButton(this IHtmlHelper htmlHelper, string text, string controllerCode, string actionCode, object htmlAttributes = null, object icoHtmlAttributes = null)
        {
            return PreClassAuthButton(htmlHelper, text, new AuthorizeFunc(controllerCode, actionCode), new List<string>()
            {
                "btn",
                "btn-light",
                "btn-sm"
            }, htmlAttributes, icoHtmlAttributes);
        }

        public static IHtmlContent SmallLightAuthButton(this IHtmlHelper htmlHelper, string text, bool allowAccess, object htmlAttributes = null, object icoHtmlAttributes = null)
        {
            return PreClassAuthButton(htmlHelper, text, allowAccess, new List<string>()
            {
                "btn",
                "btn-light",
                "btn-sm"
            }, htmlAttributes, icoHtmlAttributes);
        }

        #endregion

        #region Dark

        public static IHtmlContent DarkAuthButton(this IHtmlHelper htmlHelper, string text, string controllerCode, string actionCode, object htmlAttributes = null, object icoHtmlAttributes = null)
        {
            return PreClassAuthButton(htmlHelper, text, new AuthorizeFunc(controllerCode, actionCode), new List<string>()
            {
                "btn",
                "btn-dark"
            }, htmlAttributes, icoHtmlAttributes);
        }

        public static IHtmlContent DarkAuthButton(this IHtmlHelper htmlHelper, string text, bool allowAccess, object htmlAttributes = null, object icoHtmlAttributes = null)
        {
            return PreClassAuthButton(htmlHelper, text, allowAccess, new List<string>()
            {
                "btn",
                "btn-dark"
            }, htmlAttributes, icoHtmlAttributes);
        }

        public static IHtmlContent SmallDarkAuthButton(this IHtmlHelper htmlHelper, string text, string controllerCode, string actionCode, object htmlAttributes = null, object icoHtmlAttributes = null)
        {
            return PreClassAuthButton(htmlHelper, text, new AuthorizeFunc(controllerCode, actionCode), new List<string>()
            {
                "btn",
                "btn-dark",
                "btn-sm"
            }, htmlAttributes, icoHtmlAttributes);
        }

        public static IHtmlContent SmallDarkAuthButton(this IHtmlHelper htmlHelper, string text, bool allowAccess, object htmlAttributes = null, object icoHtmlAttributes = null)
        {
            return PreClassAuthButton(htmlHelper, text, allowAccess, new List<string>()
            {
                "btn",
                "btn-dark",
                "btn-sm"
            }, htmlAttributes, icoHtmlAttributes);
        }

        #endregion

        #region Link

        public static IHtmlContent LinkAuthButton(this IHtmlHelper htmlHelper, string text, string controllerCode, string actionCode, object htmlAttributes = null, object icoHtmlAttributes = null)
        {
            return PreClassAuthButton(htmlHelper, text, new AuthorizeFunc(controllerCode, actionCode), new List<string>()
            {
                "btn",
                "btn-link"
            }, htmlAttributes, icoHtmlAttributes);
        }

        public static IHtmlContent LinkAuthButton(this IHtmlHelper htmlHelper, string text, bool allowAccess, object htmlAttributes = null, object icoHtmlAttributes = null)
        {
            return PreClassAuthButton(htmlHelper, text, allowAccess, new List<string>()
            {
                "btn",
                "btn-link"
            }, htmlAttributes, icoHtmlAttributes);
        }

        public static IHtmlContent SmallLinkAuthButton(this IHtmlHelper htmlHelper, string text, string controllerCode, string actionCode, object htmlAttributes = null, object icoHtmlAttributes = null)
        {
            return PreClassAuthButton(htmlHelper, text, new AuthorizeFunc(controllerCode, actionCode), new List<string>()
            {
                "btn",
                "btn-link",
                "btn-sm"
            }, htmlAttributes, icoHtmlAttributes);
        }

        public static IHtmlContent SmallLinkAuthButton(this IHtmlHelper htmlHelper, string text, bool allowAccess, object htmlAttributes = null, object icoHtmlAttributes = null)
        {
            return PreClassAuthButton(htmlHelper, text, allowAccess, new List<string>()
            {
                "btn",
                "btn-link",
                "btn-sm"
            }, htmlAttributes, icoHtmlAttributes);
        }

        #endregion

        #endregion
    }

    /// <summary>
    /// Auth Button Options
    /// </summary>
    public class AuthButtonOptions
    {
        /// <summary>
        /// Text
        /// </summary>
        public string Text
        {
            get; set;
        }

        /// <summary>
        /// Size
        /// </summary>
        public ButtonSize Size
        {
            get; set;
        } = ButtonSize.Normal;

        /// <summary>
        /// Forbid Style
        /// </summary>
        public ForbidStyle ForbidStyle
        {
            get; set;
        }

        /// <summary>
        /// Authorize Func
        /// </summary>
        public AuthorizeFunc AuthorizeFunc
        {
            get; set;
        }

        /// <summary>
        /// Use NowVerifyResult
        /// </summary>
        public bool UseNowVerifyResult
        {
            get; set;
        }

        /// <summary>
        /// Allow Access
        /// </summary>
        public bool AllowAccess
        {
            get; set;
        }

        /// <summary>
        /// Use Ico
        /// </summary>
        public bool UseIco
        {
            get; set;
        }

        /// <summary>
        /// Html Attributes
        /// </summary>
        public IDictionary<string, object> HtmlAttributes
        {
            get; set;
        }

        /// <summary>
        /// Ico HtmlAttributes
        /// </summary>
        public IDictionary<string, object> IcoHtmlAttributes
        {
            get; set;
        }
    }

    /// <summary>
    /// Authorize Func
    /// </summary>
    public class AuthorizeFunc
    {
        /// <summary>
        /// Controller Code
        /// </summary>
        public string ControllerCode
        {
            get; set;
        }

        /// <summary>
        /// Action Code
        /// </summary>
        public string ActionCode
        {
            get; set;
        }

        public AuthorizeFunc(string controllerCode, string actionCode)
        {
            ControllerCode = controllerCode;
            ActionCode = actionCode;
        }
    }

    /// <summary>
    /// Button Size
    /// </summary>
    public enum ButtonSize
    {
        Normal = 2,
        Small = 4,
        Big = 8
    }

    /// <summary>
    /// Forbid Style
    /// </summary>
    public enum ForbidStyle
    {
        Remove = 2,
        Disable = 4
    }
}
