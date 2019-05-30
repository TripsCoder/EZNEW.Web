using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace EZNEW.Web.Utility
{
    /// <summary>
    /// Client Helper
    /// </summary>
    public static class Client
    {
        /// <summary>
        /// host
        /// </summary>
        public static string Host
        {
            get
            {
                return HttpContextHelper.Current.Request.Host.Host;
            }
        }

        /// <summary>
        /// host and port
        /// </summary>
        public static string FullHost
        {
            get
            {
                var host = Host;
                var port = Port;
                if (port != 80)
                {
                    host = string.Format("{0}:{1}", host, port);
                }
                return host;
            }
        }

        /// <summary>
        /// port
        /// </summary>
        public static int Port
        {
            get
            {
                return HttpContextHelper.Current.Request.Host.Port ?? 80;
            }
        }

        /// <summary>
        /// client ip address
        /// </summary>
        public static string IP
        {
            get
            {
                return HttpContextHelper.Current.Connection.RemoteIpAddress.ToString();
            }
        }

        /// <summary>
        /// original url
        /// </summary>
        public static string RawUrl
        {
            get
            {
                return HttpContextHelper.Current.Request.GetEncodedUrl();
            }
        }

        /// <summary>
        /// request url
        /// </summary>
        public static string Url
        {
            get
            {
                return HttpContextHelper.Current.Request.GetDisplayUrl();
            }
        }

        /// <summary>
        /// protocol
        /// </summary>
        public static string Protocol
        {
            get
            {
                var https = HttpContextHelper.Current.Request.IsHttps;
                return https ? "https" : "http";
            }
        }

        /// <summary>
        /// get full path
        /// </summary>
        /// <param name="relativePath">relative path</param>
        /// <returns></returns>
        public static string GetFullPath(string relativePath)
        {
            var path = string.Format("{0}://{1}", Protocol, FullHost);
            if (string.IsNullOrWhiteSpace(relativePath))
            {
                return path;
            }
            return Path.Combine(path.Trim('/', '\\'), relativePath.Trim('\\', '/'));
        }
    }
}
