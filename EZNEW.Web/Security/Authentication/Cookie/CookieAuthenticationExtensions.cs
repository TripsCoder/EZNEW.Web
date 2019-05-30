using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System;
using System.Collections.Generic;
using System.Text;
using EZNEW.Web.Security.Authentication.Cookie;
using EZNEW.Web.Security.Authentication.Cookie.Ticket;
using EZNEW.Framework.IoC;
using EZNEW.Framework.Extension;
using EZNEW.Web.Utility;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class CookieAuthenticationExtensions
    {
        public static void AddCookieAuthentication(this AuthenticationBuilder builder, Action<CustomCookieOption> cookieOptionConfiguration)
        {
            var cookieOption = new CustomCookieOption();
            cookieOptionConfiguration?.Invoke(cookieOption);
            var configureOptions = cookieOption?.CookieConfiguration;
            void customDefaultConfigure(CookieAuthenticationOptions options)
            {
                CookieAuthenticationEventHandler.ForceValidatePrincipal = cookieOption.ForceValidatePrincipal;
                CookieAuthenticationEventHandler.OnValidatePrincipalAsync = cookieOption.ValidatePrincipalAsync;
                options.EventsType = typeof(CookieAuthenticationEventHandler);
                options.Cookie.HttpOnly = true;
                var storageModel = cookieOption?.StorageModel ?? CookieStorageModel.Default;
                switch (storageModel)
                {
                    case CookieStorageModel.Default:
                    default:
                        options.SessionStore = null;
                        break;
                    case CookieStorageModel.Distributed:
                        options.SessionStore = ContainerManager.Container.Instance<ITicketDistributedStore>();
                        break;
                    case CookieStorageModel.InMemory:
                        options.SessionStore = new CookieMemoryCacheTicketStore();
                        break;
                }
                if (options.Cookie.Name.IsNullOrEmpty())
                {
                    options.Cookie.Name = string.Format("{0}_{1}_{2}", Client.Host, Client.Port, "authenticationkey_~!@#$%^&*").Encrypt().ReplaceByRegex("[^0-9a-zA-Z]", "");
                }
            }
            if (configureOptions != null)
            {
                configureOptions += customDefaultConfigure;
            }
            else
            {
                configureOptions = customDefaultConfigure;
            }
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;

            }).AddCookie(configureOptions);
            builder.Services.AddSingleton<CookieAuthenticationEventHandler>();//注册Cookie事件类型
        }

        public static void AddCookieAuthentication(this IServiceCollection services, Action<CustomCookieOption> cookieOptionConfiguration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddCookieAuthentication(cookieOptionConfiguration);
        }
    }
}
