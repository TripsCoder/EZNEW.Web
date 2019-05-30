using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace EZNEW.Web.Mvc
{
    public enum InsertionMode
    {
        /// <summary>
        /// Replace the contents of the element.
        /// </summary>
        Replace = 0,

        /// <summary>
        /// Insert before the element.
        /// </summary>
        InsertBefore = 1,

        /// <summary>
        /// Insert after the element.
        /// </summary>
        InsertAfter = 2,

        /// <summary>
        /// Replace the entire element.
        /// </summary>
        ReplaceWith = 3
    }

    public class AjaxFormOptions
    {
        private static readonly Regex _idRegex = new Regex(@"[.:[\]]");
        private string _confirm;
        private string _httpMethod;
        private InsertionMode _insertionMode = InsertionMode.Replace;
        private string _loadingElementId;
        private string _onBegin;
        private string _onComplete;
        private string _onFailure;
        private string _onSuccess;
        private string _updateTargetId;
        private string _url;

        public string Confirm
        {
            get { return _confirm ?? String.Empty; }
            set { _confirm = value; }
        }

        public string HttpMethod
        {
            get { return _httpMethod ?? String.Empty; }
            set { _httpMethod = value; }
        }

        public InsertionMode InsertionMode
        {
            get { return _insertionMode; }
            set
            {
                switch (value)
                {
                    case InsertionMode.Replace:
                    case InsertionMode.InsertAfter:
                    case InsertionMode.InsertBefore:
                    case InsertionMode.ReplaceWith:
                        _insertionMode = value;
                        return;

                    default:
                        throw new ArgumentOutOfRangeException("value");
                }
            }
        }

        internal string InsertionModeString
        {
            get
            {
                switch (InsertionMode)
                {
                    case InsertionMode.Replace:
                        return "Sys.Mvc.InsertionMode.replace";
                    case InsertionMode.InsertBefore:
                        return "Sys.Mvc.InsertionMode.insertBefore";
                    case InsertionMode.InsertAfter:
                        return "Sys.Mvc.InsertionMode.insertAfter";
                    default:
                        return ((int)InsertionMode).ToString(CultureInfo.InvariantCulture);
                }
            }
        }

        internal string InsertionModeUnobtrusive
        {
            get
            {
                switch (InsertionMode)
                {
                    case InsertionMode.Replace:
                        return "replace";
                    case InsertionMode.InsertBefore:
                        return "before";
                    case InsertionMode.InsertAfter:
                        return "after";
                    case InsertionMode.ReplaceWith:
                        return "replace-with";
                    default:
                        return ((int)InsertionMode).ToString(CultureInfo.InvariantCulture);
                }
            }
        }

        public int LoadingElementDuration { get; set; }

        public string LoadingElementId
        {
            get { return _loadingElementId ?? String.Empty; }
            set { _loadingElementId = value; }
        }

        public string OnBegin
        {
            get { return _onBegin ?? String.Empty; }
            set { _onBegin = value; }
        }

        public string OnComplete
        {
            get { return _onComplete ?? String.Empty; }
            set { _onComplete = value; }
        }

        public string OnFailure
        {
            get { return _onFailure ?? String.Empty; }
            set { _onFailure = value; }
        }

        public string OnSuccess
        {
            get { return _onSuccess ?? String.Empty; }
            set { _onSuccess = value; }
        }

        public string UpdateTargetId
        {
            get { return _updateTargetId ?? String.Empty; }
            set { _updateTargetId = value; }
        }

        public string Url
        {
            get { return _url ?? String.Empty; }
            set { _url = value; }
        }

        public bool AllowCache { get; set; }

        internal string ToJavascriptString()
        {
            StringBuilder optionsBuilder = new StringBuilder("{");
            optionsBuilder.AppendFormat(CultureInfo.InvariantCulture, " insertionMode: {0},", InsertionModeString);
            optionsBuilder.Append(PropertyStringIfSpecified("confirm", Confirm));
            optionsBuilder.Append(PropertyStringIfSpecified("httpMethod", HttpMethod));
            optionsBuilder.Append(PropertyStringIfSpecified("loadingElementId", LoadingElementId));
            optionsBuilder.Append(PropertyStringIfSpecified("updateTargetId", UpdateTargetId));
            optionsBuilder.Append(PropertyStringIfSpecified("url", Url));
            optionsBuilder.Append(EventStringIfSpecified("onBegin", OnBegin));
            optionsBuilder.Append(EventStringIfSpecified("onComplete", OnComplete));
            optionsBuilder.Append(EventStringIfSpecified("onFailure", OnFailure));
            optionsBuilder.Append(EventStringIfSpecified("onSuccess", OnSuccess));
            optionsBuilder.Length--;
            optionsBuilder.Append(" }");
            return optionsBuilder.ToString();
        }

        public IDictionary<string, object> ToUnobtrusiveHtmlAttributes()
        {
            var result = new Dictionary<string, object>
            {
                { "data-ajax", "true" },
            };

            AddToDictionaryIfSpecified(result, "data-ajax-url", Url);
            AddToDictionaryIfSpecified(result, "data-ajax-method", HttpMethod);
            AddToDictionaryIfSpecified(result, "data-ajax-confirm", Confirm);

            AddToDictionaryIfSpecified(result, "data-ajax-begin", OnBegin);
            AddToDictionaryIfSpecified(result, "data-ajax-complete", OnComplete);
            AddToDictionaryIfSpecified(result, "data-ajax-failure", OnFailure);
            AddToDictionaryIfSpecified(result, "data-ajax-success", OnSuccess);

            if (AllowCache)
            {
                AddToDictionaryIfSpecified(result, "data-ajax-cache", "true");
            }

            if (!String.IsNullOrWhiteSpace(LoadingElementId))
            {
                result.Add("data-ajax-loading", EscapeIdSelector(LoadingElementId));

                if (LoadingElementDuration > 0)
                {
                    result.Add("data-ajax-loading-duration", LoadingElementDuration);
                }
            }

            if (!String.IsNullOrWhiteSpace(UpdateTargetId))
            {
                result.Add("data-ajax-update", EscapeIdSelector(UpdateTargetId));
                result.Add("data-ajax-mode", InsertionModeUnobtrusive);
            }

            return result;
        }

        // Helpers
        private static void AddToDictionaryIfSpecified(IDictionary<string, object> dictionary, string name, string value)
        {
            if (!String.IsNullOrWhiteSpace(value))
            {
                dictionary.Add(name, value);
            }
        }

        private static string EventStringIfSpecified(string propertyName, string handler)
        {
            if (!String.IsNullOrEmpty(handler))
            {
                return String.Format(CultureInfo.InvariantCulture, " {0}: Function.createDelegate(this, {1}),", propertyName, handler.ToString());
            }
            return String.Empty;
        }

        private static string PropertyStringIfSpecified(string propertyName, string propertyValue)
        {
            if (!String.IsNullOrEmpty(propertyValue))
            {
                string escapedPropertyValue = propertyValue.Replace("'", @"\'");
                return String.Format(CultureInfo.InvariantCulture, " {0}: '{1}',", propertyName, escapedPropertyValue);
            }
            return String.Empty;
        }

        private static string EscapeIdSelector(string selector)
        {
            return '#' + _idRegex.Replace(selector, @"\$&");
        }
    }
}
