using System;
using System.Collections.Generic;
using System.Text;

namespace EZNEW.Web.FileAccess.Config
{
    /// <summary>
    /// File Access Path Config
    /// </summary>
    public class FileAccessConfig
    {
        /// <summary>
        /// Default Option
        /// </summary>
        public FileAccessOption Default
        {
            get; set;
        }

        /// <summary>
        /// file objects
        /// </summary>
        public List<FileObject> FileObjects
        {
            get; set;
        }
    }
}
