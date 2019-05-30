using System;
using System.Collections.Generic;
using System.Text;

namespace EZNEW.Web.Mvc
{
    /// <summary>
    /// Menu
    /// </summary>
    public class Menu
    {
        /// <summary>
        /// Ico
        /// </summary>
        public string Ico
        {
            get;set;
        }

        /// <summary>
        /// Name
        /// </summary>
        public string Name
        {
            get;set;
        }

        /// <summary>
        /// Tip Text
        /// </summary>
        public string TipText
        {
            get;set;
        }

        /// <summary>
        /// Controller
        /// </summary>
        public string Controller
        {
            get;set;
        }

        /// <summary>
        /// Action
        /// </summary>
        public string Action
        {
            get;set;
        }

        /// <summary>
        /// Child Menus
        /// </summary>
        public List<Menu> ChildMenus
        {
            get;set;
        }
    }
}
