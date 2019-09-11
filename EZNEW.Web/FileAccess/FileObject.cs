using System;
using System.Collections.Generic;
using System.Text;

namespace EZNEW.Web.FileAccess
{
    /// <summary>
    /// file object
    /// </summary>
    public class FileObject
    {
        /// <summary>
        /// file object name
        /// </summary>
        public string Name
        {
            get;set;
        }

        /// <summary>
        /// file access option
        /// </summary>
        public FileAccessOption FileAccessOption
        {
            get;set;
        }
    }
}
