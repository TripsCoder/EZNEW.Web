using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Linq;

namespace EZNEW.Web.Config.Upload
{
    /// <summary>
    /// Upload Config
    /// </summary>
    public class UploadConfig
    {
        /// <summary>
        /// Default
        /// </summary>
        public UploadConfigOption Default
        {
            get; set;
        }

        /// <summary>
        /// upload config options
        /// </summary>
        public Dictionary<string, UploadConfigOption> Items
        {
            get; set;
        } = new Dictionary<string, UploadConfigOption>();

        /// <summary>
        /// get upload option
        /// </summary>
        /// <param name="key">键值</param>
        /// <returns></returns>
        public UploadConfigOption GetOption(string key)
        {
            if (string.IsNullOrWhiteSpace(key) || Items == null || !Items.ContainsKey(key))
            {
                return Default;
            }
            return Items[key];
        }
    }
}
