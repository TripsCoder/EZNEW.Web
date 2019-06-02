using EZNEW.Framework.IoC;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using System.Diagnostics;
using EZNEW.Web.Config;
using Microsoft.Extensions.Configuration;
using EZNEW.Web.Config.Upload;
using EZNEW.Web.Config.FileAccess;
using System.Runtime.Versioning;
using EZNEW.Framework.Extension;

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
                RegisterProjectReference();//project refrences
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
            ContainerManager.ServiceCollection?.Configure<FileAccessPathConfig>(configuration.GetSection("FileAccessPathConfig"));
        }

        /// <summary>
        /// Register Project Reference
        /// </summary>
        static void RegisterProjectReference()
        {
            string appPath = Directory.GetCurrentDirectory();
            IHostingEnvironment env = ContainerManager.Resolve<IHostingEnvironment>();
            var isDev = env?.IsDevelopment() ?? true;
            if (isDev)
            {
                appPath = Path.Combine(appPath, "bin");
                var debugPath = Path.Combine(appPath, "Debug");
                var relaeasePath = Path.Combine(appPath, "Release");
                DateTime debugLastWriteTime = DateTime.MinValue;
                DateTime releaseLastWriteTime = DateTime.MinValue;
                if (Directory.Exists(debugPath))
                {
                    debugLastWriteTime = Directory.GetLastWriteTime(debugPath);
                }
                if (Directory.Exists(relaeasePath))
                {
                    releaseLastWriteTime = Directory.GetLastWriteTime(relaeasePath);
                }
                appPath = debugLastWriteTime >= releaseLastWriteTime ? debugPath : relaeasePath;
                var frameworkName = Assembly.GetEntryAssembly().GetCustomAttribute<TargetFrameworkAttribute>()?.FrameworkName;
                if (frameworkName.IsNullOrEmpty())
                {
                    return;
                }
                var frameworkNameArray = frameworkName.Split(new string[] { ".NETCoreApp,Version=v" }, StringSplitOptions.RemoveEmptyEntries);
                if (frameworkNameArray.IsNullOrEmpty())
                {
                    return;
                }
                var frameworkVersion = frameworkNameArray[0];
                appPath = Path.Combine(appPath, "netcoreapp" + frameworkVersion);
            }
            List<Type> types = new List<Type>();
            if (!Directory.Exists(appPath))
            {
                appPath = Directory.GetCurrentDirectory();
            }
            var files = new DirectoryInfo(appPath).GetFiles("*.dll")?.Where(c =>
c.Name.IndexOf("DataAccess") >= 0
|| c.Name.IndexOf("Business") >= 0
|| c.Name.IndexOf("Repository") >= 0
|| c.Name.IndexOf("Service") >= 0
|| c.Name.IndexOf("Domain") >= 0) ?? new List<FileInfo>(0);
            foreach (var file in files)
            {
                types.AddRange(Assembly.LoadFrom(file.FullName).GetTypes());
            }

            foreach (Type type in types)
            {
                string typeName = type.Name;
                if (!typeName.StartsWith("I"))
                {
                    continue;
                }
                if (typeName.EndsWith("Service") || typeName.EndsWith("Business") || typeName.EndsWith("DbAccess") || typeName.EndsWith("Repository"))
                {
                    Type realType = types.FirstOrDefault(t => t.Name != type.Name && !t.IsInterface && type.IsAssignableFrom(t));
                    if (realType != null)
                    {
                        List<Type> behaviors = new List<Type>();
                        ContainerManager.Register(type, realType, behaviors: behaviors);
                    }
                }
                if (typeName.EndsWith("DataAccess"))
                {
                    List<Type> relateTypes = types.Where(t => t.Name != type.Name && !t.IsInterface && type.IsAssignableFrom(t)).ToList();
                    if (relateTypes != null && relateTypes.Count > 0)
                    {
                        Type providerType = relateTypes.FirstOrDefault(c => c.Name.EndsWith("Cache"));
                        providerType = providerType ?? relateTypes.First();
                        ContainerManager.Register(type, providerType);
                    }
                }
            }
        }
    }
}
