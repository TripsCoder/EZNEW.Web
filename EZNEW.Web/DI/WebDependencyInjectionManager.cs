using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using EZNEW.Web.FileAccess;
using EZNEW.Framework.Upload;
using EZNEW.Framework.IoC;
using EZNEW.Framework.Upload.Config;
using EZNEW.Web.FileAccess.Config;

namespace EZNEW.Web.DI
{
    public class WebDependencyInjectionManager
    {
        /// <summary>
        /// Default Register
        /// </summary>
        public static void RegisterDefaultService()
        {
            try
            {
                ConfigRegister();//configs
                ContainerManager.BuildServiceProvider();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Register Configs
        /// </summary>
        static void ConfigRegister()
        {
            var configuration = ContainerManager.Resolve<IConfiguration>();
            if (configuration == null)
            {
                return;
            }
            //Upload Config
            ContainerManager.ServiceCollection?.Configure<UploadConfig>(configuration.GetSection("UploadConfig"));
            //FileAccess Config
            ContainerManager.ServiceCollection?.Configure<FileAccessConfig>(configuration.GetSection("FileAccessConfig"));
        }
    }
}
