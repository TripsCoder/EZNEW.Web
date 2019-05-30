using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EZNEW.Framework.Extension;
using System.Linq.Expressions;
using System.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using EZNEW.Framework.ExpressionUtil;

namespace EZNEW.Web.Mvc
{
    public static class InputExtensions
    {
        #region Checkbox

        static IHtmlContent CheckBoxInternal(IHtmlHelper htmlHelper, string name, IEnumerable<SelectListItem> items, IDictionary<string, object> cbxHtmlAttributes, IDictionary<string, object> lableHtmlAttributes, IDictionary<string, object> itemGroupAttributes)
        {
            if (items.IsNullOrEmpty())
            {
                return HtmlString.Empty;
            }
            string fullName = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);
            if (string.IsNullOrWhiteSpace(fullName))
            {
                throw new ArgumentException("CheckBox Control Name Is Null Or Empty");
            }
            var checkBoxBuilder = new HtmlContentBuilder();
            int i = 0;
            foreach (SelectListItem item in items)
            {
                string cbxId = string.Format("cbx-{0}-{1}", fullName, i.ToString());
                //group
                TagBuilder groupTag = new TagBuilder("span");
                groupTag.MergeAttribute("cbx-group-id", cbxId);
                groupTag.MergeAttributes(itemGroupAttributes);
                //cbx
                TagBuilder cbxTag = new TagBuilder("input");
                cbxTag.MergeAttribute("cbx-id", cbxId);
                cbxTag.MergeAttribute("id", cbxId);
                cbxTag.MergeAttributes(cbxHtmlAttributes);
                cbxTag.MergeAttribute("type", GetInputTypeString(InputType.CheckBox));
                cbxTag.MergeAttribute("value", item.Value);
                cbxTag.MergeAttribute("name", fullName);
                if (item.Selected)
                {
                    cbxTag.MergeAttribute("checked", "checked");
                }
                //lable
                TagBuilder labTag = new TagBuilder("label");
                labTag.MergeAttributes(lableHtmlAttributes);
                labTag.MergeAttribute("cbx-lable-id", cbxId);
                labTag.MergeAttribute("for", cbxId);
                labTag.InnerHtml.SetContent(item.Text);
                var groupInnerTag = new HtmlContentBuilder(2);
                groupInnerTag.AppendHtml(cbxTag);
                groupInnerTag.AppendHtml(labTag);
                groupTag.InnerHtml.SetHtmlContent(groupInnerTag);
                checkBoxBuilder.AppendHtml(groupTag);
                i++;
            }
            return checkBoxBuilder;
        }

        #region Enum To CheckBox

        /// <summary>
        /// Enum To CheckBox
        /// </summary>
        /// <param name="htmlHelper">htmlHelper</param>
        /// <param name="name">name</param>
        /// <param name="enumType">enumType</param>
        /// <param name="cbxHtmlAttributes">cbxHtmlAttributes</param>
        /// <param name="lableHtmlAttributes">lableHtmlAttributes</param>
        /// <param name="groupHtmlAttributes">groupHtmlAttributes</param>
        /// <param name="checkedValues">checkedValues</param>
        /// <returns></returns>
        public static IHtmlContent EnumToCheckBox(this IHtmlHelper htmlHelper, string name, Enum enumType, object cbxHtmlAttributes = null, object lableHtmlAttributes = null, object groupHtmlAttributes = null, params string[] checkedValues)
        {
            IList<SelectListItem> items = EnumHelper.GetSelectList(enumType.GetType(), checkedValues == null ? null : checkedValues.ToList());
            return CheckBoxInternal(htmlHelper, name, items, HtmlHelper.AnonymousObjectToHtmlAttributes(cbxHtmlAttributes), HtmlHelper.AnonymousObjectToHtmlAttributes(lableHtmlAttributes), HtmlHelper.AnonymousObjectToHtmlAttributes(groupHtmlAttributes));
        }

        /// <summary>
        /// Enum To CheckBox
        /// </summary>
        /// <param name="htmlHelper">htmlHelper</param>
        /// <param name="name">name</param>
        /// <param name="enumType">enumType</param>
        /// <param name="cbxHtmlAttributes">cbxHtmlAttributes</param>
        /// <param name="lableHtmlAttributes">lableHtmlAttributes</param>
        /// <param name="groupHtmlAttributes">groupHtmlAttributes</param>
        /// <param name="checkedValue">checkedValue</param>
        /// <returns></returns>
        public static IHtmlContent EnumToCheckBox(this IHtmlHelper htmlHelper, string name, Enum enumType, object cbxHtmlAttributes = null, object lableHtmlAttributes = null, object groupHtmlAttributes = null, Enum checkedValue = null)
        {
            IList<SelectListItem> items = EnumHelper.GetSelectList(enumType.GetType(), checkedValue);
            return CheckBoxInternal(htmlHelper, name, items, HtmlHelper.AnonymousObjectToHtmlAttributes(cbxHtmlAttributes), HtmlHelper.AnonymousObjectToHtmlAttributes(lableHtmlAttributes), HtmlHelper.AnonymousObjectToHtmlAttributes(groupHtmlAttributes));
        }

        /// <summary>
        /// Enum To CheckBox
        /// </summary>
        /// <param name="htmlHelper">htmlHelper</param>
        /// <param name="expression">Name Expression</param>
        /// <param name="enumType">enumType</param>
        /// <param name="cbxHtmlAttributes">cbxHtmlAttributes</param>
        /// <param name="lableHtmlAttributes">lableHtmlAttributes</param>
        /// <param name="groupHtmlAttributes">groupHtmlAttributes</param>
        /// <param name="checkedValues">checkedValues</param>
        /// <returns></returns>
        public static IHtmlContent EnumToCheckBox<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, Enum enumType, object cbxHtmlAttributes = null, object lableHtmlAttributes = null, object groupHtmlAttributes = null, params string[] checkedValues)
        {
            return EnumToCheckBox(htmlHelper, ExpressionHelper.GetExpressionText(expression), enumType, cbxHtmlAttributes, lableHtmlAttributes, groupHtmlAttributes, checkedValues);
        }

        /// <summary>
        /// Enum To CheckBox
        /// </summary>
        /// <param name="htmlHelper">htmlHelper</param>
        /// <param name="expression">Name Expression</param>
        /// <param name="enumType">enumType</param>
        /// <param name="cbxHtmlAttributes">cbxHtmlAttributes</param>
        /// <param name="lableHtmlAttributes">lableHtmlAttributes</param>
        /// <param name="groupHtmlAttributes">groupHtmlAttributes</param>
        /// <param name="checkedValue">checkedValue</param>
        /// <returns></returns>
        public static IHtmlContent EnumToCheckBox<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, Enum enumType, object cbxHtmlAttributes = null, object lableHtmlAttributes = null, object groupHtmlAttributes = null, Enum checkedValue = null)
        {
            return EnumToCheckBox(htmlHelper, ExpressionHelper.GetExpressionText(expression), enumType, cbxHtmlAttributes, lableHtmlAttributes, groupHtmlAttributes, checkedValue);
        }

        #endregion

        #region DataTable To CheckBox

        /// <summary>
        /// DataTable To CheckBox
        /// </summary>
        /// <param name="htmlHelper">htmlHelper</param>
        /// <param name="name">name</param>
        /// <param name="dataTable">dataTable</param>
        /// <param name="optionValueFieldName">Value Field</param>
        /// <param name="optionTextFieldName">Text Field</param>
        /// <param name="cbxHtmlAttributes">cbxHtmlAttributes</param>
        /// <param name="lableHtmlAttributes">lableHtmlAttributes</param>
        /// <param name="groupHtmlAttributes">groupHtmlAttributes</param>
        /// <param name="checkedValues"></param>
        /// <returns></returns>
        public static IHtmlContent DataTableToCheckBox(this IHtmlHelper htmlHelper, string name, DataTable dataTable, string optionValueFieldName, string optionTextFieldName, object cbxHtmlAttributes = null, object lableHtmlAttributes = null, object groupHtmlAttributes = null, params string[] checkedValues)
        {
            List<SelectListItem> listItems = new List<SelectListItem>();
            List<string> checkValueList = checkedValues == null ? new List<string>(0) : checkedValues.ToList();
            if (dataTable != null)
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    string rowValue = row[optionValueFieldName].ToString();
                    listItems.Add(new SelectListItem()
                    {
                        Value = rowValue,
                        Text = row[optionTextFieldName].ToString(),
                        Selected = checkValueList.Contains(rowValue)
                    });
                }
            }
            return CheckBoxInternal(htmlHelper, name, listItems, HtmlHelper.AnonymousObjectToHtmlAttributes(cbxHtmlAttributes), HtmlHelper.AnonymousObjectToHtmlAttributes(lableHtmlAttributes), HtmlHelper.AnonymousObjectToHtmlAttributes(groupHtmlAttributes));
        }

        /// <summary>
        /// DataTable To CheckBox
        /// </summary>
        /// <param name="htmlHelper">htmlHelper</param>
        /// <param name="expression">Name Expression</param>
        /// <param name="dataTable">dataTable</param>
        /// <param name="optionValueFieldName">Value Field</param>
        /// <param name="optionTextFieldName">Text Field</param>
        /// <param name="cbxHtmlAttributes">cbxHtmlAttributes</param>
        /// <param name="lableHtmlAttributes">lableHtmlAttributes</param>
        /// <param name="groupHtmlAttributes">groupHtmlAttributes</param>
        /// <param name="checkedValues"></param>
        /// <returns></returns>
        public static IHtmlContent DataTableToCheckBox<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, DataTable dataTable, string optionValueFieldName, string optionTextFieldName, object cbxHtmlAttributes = null, object lableHtmlAttributes = null, object groupHtmlAttributes = null, params string[] checkedValues)
        {
            return DataTableToCheckBox(htmlHelper, ExpressionHelper.GetExpressionText(expression), dataTable, optionValueFieldName, optionTextFieldName, cbxHtmlAttributes, lableHtmlAttributes, groupHtmlAttributes, checkedValues);
        }

        #endregion

        #endregion

        #region Radio

        static IHtmlContent RadioInternal(IHtmlHelper htmlHelper, string name, IEnumerable<SelectListItem> items, IDictionary<string, object> cbxHtmlAttributes, IDictionary<string, object> lableHtmlAttributes, IDictionary<string, object> itemGroupAttributes)
        {
            if (items.IsNullOrEmpty())
            {
                return HtmlString.Empty;
            }
            string fullName = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);
            if (string.IsNullOrWhiteSpace(fullName))
            {
                throw new ArgumentException("Radio Control Name Is Null Or Empty");
            }
            var radioBuilder = new HtmlContentBuilder();
            int i = 0;
            foreach (SelectListItem item in items)
            {
                string cbxId = string.Format("rdo-{0}-{1}", fullName, i.ToString());
                //group
                TagBuilder groupTag = new TagBuilder("span");
                groupTag.MergeAttribute("rdo-group-id", cbxId);
                groupTag.MergeAttributes(itemGroupAttributes);
                //cbx
                TagBuilder cbxTag = new TagBuilder("input");
                cbxTag.MergeAttribute("rdo-id", cbxId);
                cbxTag.MergeAttribute("id", cbxId);
                cbxTag.MergeAttributes(cbxHtmlAttributes);
                cbxTag.MergeAttribute("type", GetInputTypeString(InputType.Radio));
                cbxTag.MergeAttribute("value", item.Value);
                cbxTag.MergeAttribute("name", fullName);
                if (item.Selected)
                {
                    cbxTag.MergeAttribute("checked", "checked");
                }
                //lable
                TagBuilder labTag = new TagBuilder("label");
                labTag.MergeAttributes(lableHtmlAttributes);
                labTag.MergeAttribute("rdo-lable-id", cbxId);
                labTag.MergeAttribute("for", cbxId);
                labTag.InnerHtml.SetContent(item.Text);
                var groupInnerBuilder = new HtmlContentBuilder(2);
                groupInnerBuilder.AppendHtml(cbxTag);
                groupInnerBuilder.AppendHtml(labTag);
                groupTag.InnerHtml.SetHtmlContent(groupInnerBuilder);
                radioBuilder.AppendHtml(groupTag);
                i++;
            }
            return radioBuilder;
        }

        #region Enum To Radio

        /// <summary>
        /// Enum To Radio
        /// </summary>
        /// <param name="htmlHelper">htmlHelper</param>
        /// <param name="name">name</param>
        /// <param name="enumType">enumType</param>
        /// <param name="rdoHtmlAttributes">cbxHtmlAttributes</param>
        /// <param name="lableHtmlAttributes">lableHtmlAttributes</param>
        /// <param name="groupHtmlAttributes">groupHtmlAttributes</param>
        /// <param name="checkedValues">checkedValues</param>
        /// <returns></returns>
        public static IHtmlContent EnumToRadio(this IHtmlHelper htmlHelper, string name, Enum enumType, object rdoHtmlAttributes = null, object lableHtmlAttributes = null, object groupHtmlAttributes = null, string checkedValue = null)
        {
            IList<SelectListItem> items = EnumHelper.GetSelectList(enumType.GetType(), checkedValue == null ? null : new List<string>() { checkedValue });
            return RadioInternal(htmlHelper, name, items, HtmlHelper.AnonymousObjectToHtmlAttributes(rdoHtmlAttributes), HtmlHelper.AnonymousObjectToHtmlAttributes(lableHtmlAttributes), HtmlHelper.AnonymousObjectToHtmlAttributes(groupHtmlAttributes));
        }

        /// <summary>
        /// Enum To Radio
        /// </summary>
        /// <param name="htmlHelper">htmlHelper</param>
        /// <param name="name">name</param>
        /// <param name="enumType">enumType</param>
        /// <param name="rdoHtmlAttributes">cbxHtmlAttributes</param>
        /// <param name="lableHtmlAttributes">lableHtmlAttributes</param>
        /// <param name="groupHtmlAttributes">groupHtmlAttributes</param>
        /// <param name="checkedValues">checkedValues</param>
        /// <returns></returns>
        public static IHtmlContent EnumToRadio(this IHtmlHelper htmlHelper, string name, Enum enumType, object rdoHtmlAttributes = null, object lableHtmlAttributes = null, object groupHtmlAttributes = null, Enum checkedValue = null)
        {
            IList<SelectListItem> items = EnumHelper.GetSelectList(enumType.GetType(), checkedValue);
            return RadioInternal(htmlHelper, name, items, HtmlHelper.AnonymousObjectToHtmlAttributes(rdoHtmlAttributes), HtmlHelper.AnonymousObjectToHtmlAttributes(lableHtmlAttributes), HtmlHelper.AnonymousObjectToHtmlAttributes(groupHtmlAttributes));
        }

        /// <summary>
        /// Enum To Radio
        /// </summary>
        /// <param name="htmlHelper">htmlHelper</param>
        /// <param name="expression">Name Expression</param>
        /// <param name="enumType">enumType</param>
        /// <param name="rdoHtmlAttributes">cbxHtmlAttributes</param>
        /// <param name="lableHtmlAttributes">lableHtmlAttributes</param>
        /// <param name="groupHtmlAttributes">groupHtmlAttributes</param>
        /// <param name="checkedValues">checkedValues</param>
        /// <returns></returns>
        public static IHtmlContent EnumToRadio<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, Enum enumType, object rdoHtmlAttributes = null, object lableHtmlAttributes = null, object groupHtmlAttributes = null, string checkedValue = null)
        {
            return EnumToRadio(htmlHelper, ExpressionHelper.GetExpressionText(expression), enumType, rdoHtmlAttributes, lableHtmlAttributes, groupHtmlAttributes, checkedValue);
        }

        /// <summary>
        /// Enum To Radio
        /// </summary>
        /// <param name="htmlHelper">htmlHelper</param>
        /// <param name="expression">Name Expression</param>
        /// <param name="enumType">enumType</param>
        /// <param name="rdoHtmlAttributes">cbxHtmlAttributes</param>
        /// <param name="lableHtmlAttributes">lableHtmlAttributes</param>
        /// <param name="groupHtmlAttributes">groupHtmlAttributes</param>
        /// <param name="checkedValues">checkedValues</param>
        /// <returns></returns>
        public static IHtmlContent EnumToRadio<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, Enum enumType, object rdoHtmlAttributes = null, object lableHtmlAttributes = null, object groupHtmlAttributes = null, Enum checkedValue = null)
        {
            return EnumToRadio(htmlHelper, ExpressionHelper.GetExpressionText(expression), enumType, rdoHtmlAttributes, lableHtmlAttributes, groupHtmlAttributes, checkedValue);
        }

        #endregion

        #region DataTable To CheckBox

        /// <summary>
        /// DataTable To Radio
        /// </summary>
        /// <param name="htmlHelper">htmlHelper</param>
        /// <param name="name">name</param>
        /// <param name="dataTable">dataTable</param>
        /// <param name="optionValueFieldName">Value Field</param>
        /// <param name="optionTextFieldName">Text Field</param>
        /// <param name="cbxHtmlAttributes">cbxHtmlAttributes</param>
        /// <param name="lableHtmlAttributes">lableHtmlAttributes</param>
        /// <param name="groupHtmlAttributes">groupHtmlAttributes</param>
        /// <param name="checkedValues"></param>
        /// <returns></returns>
        public static IHtmlContent DataTableToRadio(this IHtmlHelper htmlHelper, string name, DataTable dataTable, string optionValueFieldName, string optionTextFieldName, object cbxHtmlAttributes = null, object lableHtmlAttributes = null, object groupHtmlAttributes = null, string checkedValue = null)
        {
            List<SelectListItem> listItems = new List<SelectListItem>();
            if (dataTable != null)
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    string rowValue = row[optionValueFieldName].ToString();
                    listItems.Add(new SelectListItem()
                    {
                        Value = rowValue,
                        Text = row[optionTextFieldName].ToString(),
                        Selected = rowValue == checkedValue
                    });
                }
            }
            return RadioInternal(htmlHelper, name, listItems, HtmlHelper.AnonymousObjectToHtmlAttributes(cbxHtmlAttributes), HtmlHelper.AnonymousObjectToHtmlAttributes(lableHtmlAttributes), HtmlHelper.AnonymousObjectToHtmlAttributes(groupHtmlAttributes));
        }

        /// <summary>
        /// DataTable To Radio
        /// </summary>
        /// <param name="htmlHelper">htmlHelper</param>
        /// <param name="expression">Name Expression</param>
        /// <param name="dataTable">dataTable</param>
        /// <param name="optionValueFieldName">Value Field</param>
        /// <param name="optionTextFieldName">Text Field</param>
        /// <param name="cbxHtmlAttributes">cbxHtmlAttributes</param>
        /// <param name="lableHtmlAttributes">lableHtmlAttributes</param>
        /// <param name="groupHtmlAttributes">groupHtmlAttributes</param>
        /// <param name="checkedValues"></param>
        /// <returns></returns>
        public static IHtmlContent DataTableToRadio<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, DataTable dataTable, string optionValueFieldName, string optionTextFieldName, object cbxHtmlAttributes = null, object lableHtmlAttributes = null, object groupHtmlAttributes = null, string checkedValue = null)
        {
            return DataTableToRadio(htmlHelper, ExpressionHelper.GetExpressionText(expression), dataTable, optionValueFieldName, optionTextFieldName, cbxHtmlAttributes, lableHtmlAttributes, groupHtmlAttributes, checkedValue);
        }

        #endregion

        #endregion

        #region GetInputTypeString

        private static string GetInputTypeString(InputType inputType)
        {
            switch (inputType)
            {
                case InputType.CheckBox:
                    return "checkbox";
                case InputType.Hidden:
                    return "hidden";
                case InputType.Password:
                    return "password";
                case InputType.Radio:
                    return "radio";
                case InputType.Text:
                    return "text";
                default:
                    return "text";
            }
        }

        #endregion
    }
}
