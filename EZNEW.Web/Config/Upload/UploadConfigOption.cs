using EZNEW.Framework.Code;
using System;
using System.Collections.Generic;
using System.Text;

namespace EZNEW.Web.Config.Upload
{
    /// <summary>
    /// Upload Config Option
    /// </summary>
    public class UploadConfigOption
    {
        /// <summary>
        /// remote upload
        /// </summary>
        public bool Remote
        {
            get; set;
        }

        /// <summary>
        /// save path
        /// </summary>
        public string SavePath
        {
            get; set;
        }

        /// <summary>
        /// save to content root folder
        /// </summary>
        public bool SaveToContentRoot
        {
            get; set;
        } = true;

        /// <summary>
        /// content root folder path
        /// </summary>
        public string ContentRootPath
        {
            get; set;
        } = "wwwroot";

        /// <summary>
        /// remote configs
        /// </summary>
        public List<RemoteUploadOption> RemoteConfigs
        {
            get; set;
        }

        /// <summary>
        /// get random remote upload server
        /// </summary>
        /// <returns></returns>
        public RemoteUploadOption GetRandomRemoteUploadServer()
        {
            if (RemoteConfigs == null || RemoteConfigs.Count <= 0)
            {
                return null;
            }
            int serverCount = RemoteConfigs.Count;
            if (serverCount == 1)
            {
                return RemoteConfigs[0];
            }
            Random random = new Random();
            int ranIndex = random.Next(0, serverCount);
            return RemoteConfigs[ranIndex];
        }
    }
}
