using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using EZNEW.Framework.ExpressionUtil;

namespace EZNEW.Web.Mvc
{
    /// <summary>
    /// MVC Html Extensions
    /// </summary>
    public static class SelectExtensions
    {
        #region Enum To Select

        /// <summary>
        /// Enum to Select
        /// </summary>
        /// <param name="htmlHelper">IHtmlHelper</param>
        /// <param name="name">Select Control Name</param>
        /// <param name="enumType">Enum Type</param>
        /// <param name="htmlAttributes">Html Attributes</param>
        /// <param name="firstOptionValue">First Option Value</param>
        /// <param name="firstOptionText">First Option Text</param>
        /// <param name="selectedValue">Already Selected Value</param>
        /// <returns>IHtmlContent</returns>
        public static IHtmlContent EnumToSelect(this IHtmlHelper htmlHelper, string name, Enum enumType, object htmlAttributes=null, string firstOptionValue = null, string firstOptionText = null, Enum selectedValue = null)
        {
            return EnumToSelect(htmlHelper, name, enumType, htmlAttributes, firstOptionValue, firstOptionText, selectedValue == null ? null : selectedValue.ToString("d"));
        }

        /// <summary>
        /// Enum to Select
        /// </summary>
        /// <param name="htmlHelper">IHtmlHelper</param>
        /// <param name="name">Select Control Name</param>
        /// <param name="enumType">Enum Type</param>
        /// <param name="htmlAttributes">Html Attributes</param>
        /// <param name="firstOptionValue">First Option Value</param>
        /// <param name="firstOptionText">First Option Text</param>
        /// <param name="selectedValue">Already Selected Value</param>
        /// <returns>IHtmlContent</returns>
        private static IHtmlContent EnumToSelect(this IHtmlHelper htmlHelper, string name, Enum enumType, object htmlAttributes = null, string firstOptionValue = null, string firstOptionText = null, string selectedValue = null)
        {
            List<SelectListItem> listItems = new List<SelectListItem>();
            if (firstOptionValue != null && firstOptionText != null)
            {
                listItems.Add(new SelectListItem()
                {
                    Value = firstOptionValue,
                    Text = firstOptionText,
                    Selected = selectedValue == null ? true : selectedValue == firstOptionValue
                });
            }
            listItems.AddRange(EnumHelper.GetSelectList(enumType.GetType(), selectedValue == null ? null : new List<string>() { selectedValue }));
            return htmlHelper.DropDownList(name, listItems, null, htmlAttributes);
        }

        /// <summary>
        /// EnumToSelect
        /// </summary>
        /// <typeparam name="TModel">Data Model</typeparam>
        /// <typeparam name="TProperty">Data Property</typeparam>
        /// <param name="htmlHelper">IHtmlHelper</param>
        /// <param name="expression">PropertyExpression</param>
        /// <param name="enumType">Enum Type</param>
        /// <param name="htmlAttributes">HtmlAttributes</param>
        /// <param name="firstOptionValue">First Option Value</param>
        /// <param name="firstOptionText">First Option Text</param>
        /// <param name="selectedValue">Selected Value</param>
        /// <returns></returns>
        public static IHtmlContent EnumToSelect<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, Enum enumType, object htmlAttributes = null, string firstOptionValue = null, string firstOptionText = null, string selectedValue = null)
        {
            return EnumToSelect(htmlHelper, ExpressionHelper.GetExpressionText(expression), enumType, htmlAttributes, firstOptionValue, firstOptionText, selectedValue);
        }

        /// <summary>
        /// EnumToSelect
        /// </summary>
        /// <typeparam name="TModel">Data Model</typeparam>
        /// <typeparam name="TProperty">Data Property</typeparam>
        /// <param name="htmlHelper">IHtmlHelper</param>
        /// <param name="expression">PropertyExpression</param>
        /// <param name="enumType">Enum Type</param>
        /// <param name="htmlAttributes">HtmlAttributes</param>
        /// <param name="firstOptionValue">First Option Value</param>
        /// <param name="firstOptionText">First Option Text</param>
        /// <param name="selectedValue">Selected Value</param>
        /// <returns></returns>
        public static IHtmlContent EnumToSelect<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, Enum enumType, object htmlAttributes = null, string firstOptionValue = null, string firstOptionText = null, Enum selectedValue = null)
        {
            return EnumToSelect(htmlHelper, ExpressionHelper.GetExpressionText(expression), enumType, htmlAttributes, firstOptionValue, firstOptionText, selectedValue);
        }

        /// <summary>
        /// Enum to MultipleSelect
        /// </summary>
        /// <param name="htmlHelper">IHtmlHelper</param>
        /// <param name="name">Select Control Name</param>
        /// <param name="enumType">Enum Type</param>
        /// <param name="htmlAttributes">Html Attributes</param>
        /// <param name="firstOptionValue">First Option Value</param>
        /// <param name="firstOptionText">First Option Text</param>
        /// <param name="selectedValues">Already Selected Value</param>
        /// <returns>IHtmlContent</returns>
        public static IHtmlContent EnumToMultipleSelect(this IHtmlHelper htmlHelper, string name, Enum enumType, object htmlAttributes = null, string firstOptionValue = null, string firstOptionText = null, params Enum[] selectedValues)
        {
            return EnumToMultipleSelect(htmlHelper, name, enumType, htmlAttributes, firstOptionValue, firstOptionText, selectedValues == null ? null : selectedValues.Select(c => c.ToString("d")).ToArray());
        }

        /// <summary>
        /// Enum to MultipleSelect
        /// </summary>
        /// <param name="htmlHelper">IHtmlHelper</param>
        /// <param name="expression">Name Expression</param>
        /// <param name="enumType">Enum Type</param>
        /// <param name="htmlAttributes">Html Attributes</param>
        /// <param name="firstOptionValue">First Option Value</param>
        /// <param name="firstOptionText">First Option Text</param>
        /// <param name="selectedValues">Already Selected Value</param>
        /// <returns>IHtmlContent</returns>
        public static IHtmlContent EnumToMultipleSelect<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, Enum enumType, object htmlAttributes = null, string firstOptionValue = null, string firstOptionText = null, params Enum[] selectedValues)
        {
            return EnumToMultipleSelect(htmlHelper, ExpressionHelper.GetExpressionText(expression), enumType, htmlAttributes, firstOptionValue, firstOptionText, selectedValues);
        }

        /// <summary>
        /// Enum to MultipleSelect
        /// </summary>
        /// <param name="htmlHelper">IHtmlHelper</param>
        /// <param name="name">Select Control Name</param>
        /// <param name="enumType">Enum Type</param>
        /// <param name="htmlAttributes">Html Attributes</param>
        /// <param name="firstOptionValue">First Option Value</param>
        /// <param name="firstOptionText">First Option Text</param>
        /// <param name="selectedValues">Already Selected Value</param>
        /// <returns>IHtmlContent</returns>
        private static IHtmlContent EnumToMultipleSelect(this IHtmlHelper htmlHelper, string name, Enum enumType, object htmlAttributes = null, string firstOptionValue = null, string firstOptionText = null, params string[] selectedValues)
        {
            List<SelectListItem> listItems = new List<SelectListItem>();
            if (firstOptionValue != null && firstOptionText != null)
            {
                listItems.Add(new SelectListItem()
                {
                    Value = firstOptionValue,
                    Text = firstOptionText
                });
            }
            listItems.AddRange(EnumHelper.GetSelectList(enumType.GetType(), selectedValues.ToList()));
            return htmlHelper.ListBox(name, listItems, htmlAttributes);
        }

        /// <summary>
        /// Enum to MultipleSelect
        /// </summary>
        /// <param name="htmlHelper">IHtmlHelper</param>
        /// <param name="expression">Name Expression</param>
        /// <param name="enumType">Enum Type</param>
        /// <param name="htmlAttributes">Html Attributes</param>
        /// <param name="firstOptionValue">First Option Value</param>
        /// <param name="firstOptionText">First Option Text</param>
        /// <param name="selectedValues">Already Selected Value</param>
        /// <returns>IHtmlContent</returns>
        private static IHtmlContent EnumToMultipleSelect<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, Enum enumType, object htmlAttributes = null, string firstOptionValue = null, string firstOptionText = null, params string[] selectedValues)
        {
            return EnumToMultipleSelect(htmlHelper, ExpressionHelper.GetExpressionText(expression), enumType, htmlAttributes, firstOptionValue, firstOptionText, selectedValues);
        }

        #endregion

        #region DataTable To Select

        /// <summary>
        /// DataTable To Select
        /// </summary>
        /// <param name="htmlHelper">IHtmlHelper</param>
        /// <param name="name">Select Control Name</param>
        /// <param name="dataTable">DataTable</param>
        /// <param name="optionValueFieldName">Value Field Name</param>
        /// <param name="optionTextFieldName">Text Field Name</param>
        /// <param name="htmlAttributes">Html Attributes</param>
        /// <param name="firstOptionValue">First Option Value</param>
        /// <param name="firstOptionText">First Option Text</param>
        /// <param name="selectedValues">Already Selected Value</param>
        /// <returns>IHtmlContent</returns>
        public static IHtmlContent DataTableToSelect(this IHtmlHelper htmlHelper, string name, DataTable dataTable, string optionValueFieldName, string optionTextFieldName, object htmlAttributes=null, string firstOptionValue = null, string firstOptionText = null, string selectedValue = null)
        {
            List<SelectListItem> listItems = new List<SelectListItem>();
            if (firstOptionValue != null && firstOptionText != null)
            {
                listItems.Add(new SelectListItem()
                {
                    Value = firstOptionValue,
                    Text = firstOptionText,
                    Selected = firstOptionValue == selectedValue
                });
            }
            if (dataTable != null)
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    string rowValue = row[optionValueFieldName].ToString();
                    listItems.Add(new SelectListItem()
                    {
                        Value = rowValue,
                        Text = row[optionTextFieldName].ToString(),
                        Selected = rowValue == selectedValue
                    });
                }
            }
            return htmlHelper.DropDownList(name, listItems, null, htmlAttributes);
        }

        /// <summary>
        /// DataTable To Select
        /// </summary>
        /// <param name="htmlHelper">IHtmlHelper</param>
        /// <param name="expression">Name Expression</param>
        /// <param name="dataTable">DataTable</param>
        /// <param name="optionValueFieldName">Value Field Name</param>
        /// <param name="optionTextFieldName">Text Field Name</param>
        /// <param name="htmlAttributes">Html Attributes</param>
        /// <param name="firstOptionValue">First Option Value</param>
        /// <param name="firstOptionText">First Option Text</param>
        /// <param name="selectedValues">Already Selected Value</param>
        /// <returns>IHtmlContent</returns>
        public static IHtmlContent DataTableToSelect<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, DataTable dataTable, string optionValueFieldName, string optionTextFieldName, object htmlAttributes=null, string firstOptionValue = null, string firstOptionText = null, string selectedValue = null)
        {
            return DataTableToSelect(htmlHelper, ExpressionHelper.GetExpressionText(expression), dataTable, optionValueFieldName, optionTextFieldName, htmlAttributes, firstOptionValue, firstOptionText, selectedValue);
        }

        /// <summary>
        /// DataTable To Select
        /// </summary>
        /// <param name="htmlHelper">IHtmlHelper</param>
        /// <param name="name">Select Control Name</param>
        /// <param name="dataTable">DataTable</param>
        /// <param name="optionValueFieldName">Value Field Name</param>
        /// <param name="optionTextFieldName">Text Field Name</param>
        /// <param name="htmlAttributes">Html Attributes</param>
        /// <param name="firstOptionValue">First Option Value</param>
        /// <param name="firstOptionText">First Option Text</param>
        /// <param name="selectedValues">Already Selected Value</param>
        /// <returns>IHtmlContent</returns>
        public static IHtmlContent DataTableToMultipleSelect(this IHtmlHelper htmlHelper, string name, DataTable dataTable, string optionValueFieldName, string optionTextFieldName, object htmlAttributes=null, string firstOptionValue = null, string firstOptionText = null, params string[] selectedValues)
        {
            bool hasSelected = selectedValues != null && selectedValues.Length > 0;
            List<SelectListItem> listItems = new List<SelectListItem>();
            if (firstOptionValue != null && firstOptionText != null)
            {
                listItems.Add(new SelectListItem()
                {
                    Value = firstOptionValue,
                    Text = firstOptionText,
                    Selected = hasSelected ? selectedValues.Contains(firstOptionValue) : hasSelected
                });
            }
            if (dataTable != null)
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    string rowValue = row[optionValueFieldName].ToString();
                    listItems.Add(new SelectListItem()
                    {
                        Value = rowValue,
                        Text = row[optionTextFieldName].ToString(),
                        Selected = hasSelected ? selectedValues.Contains(rowValue) : hasSelected
                    });
                }
            }
            return htmlHelper.ListBox(name, listItems, htmlAttributes);
        }

        /// <summary>
        /// DataTable To Select
        /// </summary>
        /// <param name="htmlHelper">IHtmlHelper</param>
        /// <param name="expression">Name Expression</param>
        /// <param name="dataTable">DataTable</param>
        /// <param name="optionValueFieldName">Value Field Name</param>
        /// <param name="optionTextFieldName">Text Field Name</param>
        /// <param name="htmlAttributes">Html Attributes</param>
        /// <param name="firstOptionValue">First Option Value</param>
        /// <param name="firstOptionText">First Option Text</param>
        /// <param name="selectedValues">Already Selected Value</param>
        /// <returns>IHtmlContent</returns>
        public static IHtmlContent DataTableToMultipleSelect<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, DataTable dataTable, string optionValueFieldName, string optionTextFieldName, object htmlAttributes=null, string firstOptionValue = null, string firstOptionText = null, params string[] selectedValues)
        {
            return DataTableToMultipleSelect(htmlHelper, ExpressionHelper.GetExpressionText(expression), dataTable, optionValueFieldName, optionTextFieldName, htmlAttributes, firstOptionValue, firstOptionText, selectedValues);
        }

        #endregion

        #region DataTable To TreeSelect

        public static IHtmlContent DataTableToTreeSelect(this IHtmlHelper htmlHelper, string name, DataTable dataTable, string valueField, string textField, string parentField, string topValue, object htmlAttributes=null, string firstOptionValue = null, string firstOptionText = null, string selectedValue = null)
        {
            List<SelectListItem> itemList = new List<SelectListItem>(dataTable.Rows.Count + 1);
            if (firstOptionValue != null && firstOptionText != null)
            {
                itemList.Add(new SelectListItem()
                {
                    Text = string.Format("├{0}", firstOptionText),
                    Value = firstOptionValue,
                    Selected = firstOptionValue == selectedValue
                });
            }
            if (dataTable != null)
            {
                DataRow[] topDatas = dataTable.Select(parentField + "='" + topValue + "'");
                foreach (var dataRow in topDatas)
                {
                    string value = dataRow[valueField].ToString();
                    string text = string.Format("├{0}", dataRow[textField].ToString());
                    itemList.Add(GetNewItem(htmlHelper,text, value, value == selectedValue, 1));
                    CreateChildOption(htmlHelper,dataTable, valueField, textField, parentField, value, new List<string>() { selectedValue }, 1, itemList);
                }
            }
            return htmlHelper.DropDownList(name, itemList, null, htmlAttributes);
        }

        public static IHtmlContent DataTableToTreeSelect<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, DataTable dataTable, string valueField, string textField, string parentField, string topValue, object htmlAttributes=null, string firstOptionValue = null, string firstOptionText = null, string selectedValue = null)
        {
            return DataTableToTreeSelect(htmlHelper, ExpressionHelper.GetExpressionText(expression), dataTable, valueField, textField, parentField, topValue, htmlAttributes, firstOptionValue, firstOptionText, selectedValue);
        }

        public static IHtmlContent DataTableToMultipleTreeSelect(this IHtmlHelper htmlHelper, string name, DataTable dataTable, string valueField, string textField, string parentField, string topValue, object htmlAttributes=null, string firstOptionValue = null, string firstOptionText = null, params string[] selectedValues)
        {
            if (dataTable == null)
            {
                return HtmlString.Empty;
            }

            List<SelectListItem> itemList = new List<SelectListItem>(dataTable.Rows.Count + 1);
            List<string> selectedValueList = selectedValues == null ? new List<string>(0) : selectedValues.ToList();
            if (firstOptionValue != null && firstOptionText != null)
            {
                itemList.Add(new SelectListItem()
                {
                    Text = string.Format("├{0}", firstOptionText),
                    Value = firstOptionValue,
                    Selected = selectedValueList.Contains(firstOptionValue)
                });
            }
            if (dataTable != null)
            {
                DataRow[] topDatas = dataTable.Select(parentField + "='" + topValue + "'");
                foreach (var dataRow in topDatas)
                {
                    string value = dataRow[valueField].ToString();
                    string text = string.Format("├{0}", dataRow[textField].ToString());
                    itemList.Add(GetNewItem(htmlHelper,text, value, selectedValueList.Contains(value), 1));
                    CreateChildOption(htmlHelper,dataTable, valueField, textField, parentField, value, selectedValueList, 1, itemList);
                }
            }
            return htmlHelper.ListBox(name, itemList, htmlAttributes);
        }

        static void CreateChildOption(IHtmlHelper htmlHelper,DataTable dataTable, string valueField, string textField, string parentField, string parentValue, List<string> selectedValues, int parentLevel, List<SelectListItem> allListItems)
        {
            DataRow[] childRows = dataTable.Select(string.Format("{0}='{1}'", parentField, parentValue));
            if (childRows == null || childRows.Length <= 0)
            {
                return;
            }
            var nowLevel = parentLevel+1;
            foreach (DataRow dataRow in childRows)
            {
                string value = dataRow[valueField].ToString();
                string text = string.Format("{0}∟{1}", new string('　', parentLevel), dataRow[textField].ToString());
                allListItems.Add(GetNewItem(htmlHelper,text, value, selectedValues.Contains(value), nowLevel));
                CreateChildOption(htmlHelper,dataTable, valueField, textField, parentField, value, selectedValues, nowLevel, allListItems);
            }
        }

        static SelectListItem GetNewItem(IHtmlHelper htmlHelper,string text, string value, bool selected, int level)
        {
            return new SelectListItem()
            {
                Text = text,
                Value = value,
                Selected = selected
            };
        }

        #endregion

        #region Model To Select

        public static IHtmlContent ModelListToSelect<TModel, TValueProperty, TTextProperty>(this IHtmlHelper htmlHelper, string name, IEnumerable<TModel> modelList, Expression<Func<TModel, TValueProperty>> valuePropertyExpression, Expression<Func<TModel, TTextProperty>> textPropertyExpression, object htmlAttributes, string firstOptionValue = null, string firstOptionText = null, string selectedValue = null)
        {
            List<SelectListItem> listItems = new List<SelectListItem>();
            if (firstOptionValue != null && firstOptionText != null)
            {
                listItems.Add(new SelectListItem()
                {
                    Value = firstOptionValue,
                    Text = firstOptionText,
                    Selected = firstOptionValue == selectedValue
                });
            }
            if (modelList != null)
            {
                var modelType = typeof(TModel);
                string valuePropertyName = ExpressionHelper.GetExpressionText(valuePropertyExpression);
                string textPropertyName = ExpressionHelper.GetExpressionText(textPropertyExpression);
                PropertyInfo valueProperty = modelType.GetProperty(valuePropertyName);
                PropertyInfo textProperty = modelType.GetProperty(textPropertyName);
                foreach (var model in modelList)
                {
                    string value = valueProperty.GetValue(model).ToString();
                    string text = textProperty.GetValue(model).ToString();
                    listItems.Add(new SelectListItem()
                    {
                        Value = value,
                        Text = text,
                        Selected = value == selectedValue
                    });
                }
            }
            return htmlHelper.DropDownList(name, listItems, null, htmlAttributes);
        }

        public static IHtmlContent ModelListToSelect<TModel, TNameModel, TNameProperty, TValueProperty, TTextProperty>(this IHtmlHelper<TNameModel> htmlHelper, Expression<Func<TNameModel, TNameProperty>> namePropertyExpression, IEnumerable<TModel> modelList, Expression<Func<TModel, TValueProperty>> valuePropertyExpression, Expression<Func<TModel, TTextProperty>> textPropertyExpression, object htmlAttributes, string firstOptionValue = null, string firstOptionText = null, string selectedValue = null)
        {
            return ModelListToSelect(htmlHelper, ExpressionHelper.GetExpressionText(namePropertyExpression), modelList, valuePropertyExpression, textPropertyExpression, htmlAttributes, firstOptionValue, firstOptionText, selectedValue);
        }

        public static IHtmlContent ModelListToMultipleSelect<TModel, TValueProperty, TTextProperty>(this IHtmlHelper htmlHelper, string name, IEnumerable<TModel> modelList, Expression<Func<TModel, TValueProperty>> valuePropertyExpression, Expression<Func<TModel, TTextProperty>> textPropertyExpression, object htmlAttributes, string firstOptionValue = null, string firstOptionText = null, params string[] selectedValues)
        {
            List<SelectListItem> listItems = new List<SelectListItem>();
            List<string> selectedValueList = selectedValues == null ? new List<string>(0) : selectedValues.ToList();
            if (firstOptionValue != null && firstOptionText != null)
            {
                listItems.Add(new SelectListItem()
                {
                    Value = firstOptionValue,
                    Text = firstOptionText,
                    Selected = selectedValueList.Contains(firstOptionValue)
                });
            }
            if (modelList != null)
            {
                var modelType = typeof(TModel);
                string valuePropertyName = ExpressionHelper.GetExpressionText(valuePropertyExpression);
                string textPropertyName = ExpressionHelper.GetExpressionText(textPropertyExpression);
                PropertyInfo valueProperty = modelType.GetProperty(valuePropertyName);
                PropertyInfo textProperty = modelType.GetProperty(textPropertyName);
                foreach (var model in modelList)
                {
                    string value = valueProperty.GetValue(model).ToString();
                    string text = textProperty.GetValue(model).ToString();
                    listItems.Add(new SelectListItem()
                    {
                        Value = value,
                        Text = text,
                        Selected = selectedValueList.Contains(value)
                    });
                }
            }
            return htmlHelper.ListBox(name, listItems, htmlAttributes);
        }

        public static IHtmlContent ModelListToMultipleSelect<TModel, TNameModel, TNameProperty, TValueProperty, TTextProperty>(this IHtmlHelper<TNameModel> htmlHelper, Expression<Func<TNameModel, TNameProperty>> namePropertyExpression, IEnumerable<TModel> modelList, Expression<Func<TModel, TValueProperty>> valuePropertyExpression, Expression<Func<TModel, TTextProperty>> textPropertyExpression, object htmlAttributes, string firstOptionValue = null, string firstOptionText = null, params string[] selectedValues)
        {
            return ModelListToMultipleSelect(htmlHelper, ExpressionHelper.GetExpressionText(namePropertyExpression), modelList, valuePropertyExpression, textPropertyExpression, htmlAttributes, firstOptionValue, firstOptionText, selectedValues);
        }

        #endregion

        #region Model To TreeSelect

        public static IHtmlContent ModelListToTreeSelect<TModel, TValueProperty, TTextProperty, TParentProperty>(this IHtmlHelper htmlHelper, string name, IEnumerable<TModel> modelList, Expression<Func<TModel, TValueProperty>> valuePropertyExpression, Expression<Func<TModel, TTextProperty>> textPropertyExpression, Expression<Func<TModel, TParentProperty>> parentPropertyExpression, string topValue, object htmlAttributes=null, string firstOptionValue = null, string firstOptionText = null, string selectedValue = null)
        {
            List<SelectListItem> itemList = new List<SelectListItem>();
            if (firstOptionValue != null && firstOptionText != null)
            {
                itemList.Add(new SelectListItem()
                {
                    Text = string.Format("├{0}", firstOptionText),
                    Value = firstOptionValue,
                    Selected = firstOptionValue == selectedValue
                });
            }
            var modelType = typeof(TModel);
            var valueProperty = modelType.GetProperty(ExpressionHelper.GetExpressionText(valuePropertyExpression));
            var textProperty = modelType.GetProperty(ExpressionHelper.GetExpressionText(textPropertyExpression));
            var parentProperty = modelType.GetProperty(ExpressionHelper.GetExpressionText(parentPropertyExpression));
            if (modelList != null)
            {
                IEnumerable<TModel> topModelList = modelList.Where(c => parentProperty.GetValue(c).ToString() == topValue);
                foreach (var model in topModelList)
                {
                    string value = valueProperty.GetValue(model).ToString();
                    string text = string.Format("├{0}", textProperty.GetValue(model).ToString());
                    itemList.Add(GetNewItem(htmlHelper,text, value, value == selectedValue, 1));
                    CreateChildOption(htmlHelper,modelList, valueProperty, textProperty, parentProperty, value, new List<string>() { selectedValue }, 1, itemList);
                }
            }
            return htmlHelper.DropDownList(name, itemList, null, htmlAttributes);
        }

        public static IHtmlContent ModelListToMultipleTreeSelect<TModel, TValueProperty, TTextProperty, TParentProperty>(this IHtmlHelper htmlHelper, string name, IEnumerable<TModel> modelList, Expression<Func<TModel, TValueProperty>> valuePropertyExpression, Expression<Func<TModel, TTextProperty>> textPropertyExpression, Expression<Func<TModel, TParentProperty>> parentPropertyExpression, string topValue, object htmlAttributes=null, string firstOptionValue = null, string firstOptionText = null, params string[] selectedValues)
        {
            List<SelectListItem> itemList = new List<SelectListItem>();
            List<string> selectedValueList = selectedValues == null ? new List<string>(0) : selectedValues.ToList();
            if (firstOptionValue != null && firstOptionText != null)
            {
                itemList.Add(new SelectListItem()
                {
                    Text = string.Format("├{0}", firstOptionText),
                    Value = firstOptionValue,
                    Selected = selectedValueList.Contains(firstOptionValue)
                });
            }
            var modelType = typeof(TModel);
            var valueProperty = modelType.GetProperty(ExpressionHelper.GetExpressionText(valuePropertyExpression));
            var textProperty = modelType.GetProperty(ExpressionHelper.GetExpressionText(textPropertyExpression));
            var parentProperty = modelType.GetProperty(ExpressionHelper.GetExpressionText(parentPropertyExpression));
            if (modelList != null)
            {
                IEnumerable<TModel> topModelList = modelList.Where(c => parentProperty.GetValue(c).ToString() == topValue);
                foreach (var model in topModelList)
                {
                    string value = valueProperty.GetValue(model).ToString();
                    string text = string.Format("├{0}", textProperty.GetValue(model).ToString());
                    itemList.Add(GetNewItem(htmlHelper,text, value, selectedValueList.Contains(value), 1));
                    CreateChildOption(htmlHelper,modelList, valueProperty, textProperty, parentProperty, value, selectedValueList, 1, itemList);
                }
            }
            return htmlHelper.ListBox(name, itemList, htmlAttributes);
        }

        static void CreateChildOption<TModel>(IHtmlHelper htmlHelper,IEnumerable<TModel> modelList, PropertyInfo valueProperty, PropertyInfo textProperty, PropertyInfo parentProperty, string parentValue, List<string> selectedValues, int parentLevel, List<SelectListItem> allListItems)
        {
            IEnumerable<TModel> childModelList = modelList.Where(c => parentProperty.GetValue(c).ToString() == parentValue);
            var nowLevel = parentLevel + 1;
            foreach (var childModel in childModelList)
            {
                string value = valueProperty.GetValue(childModel).ToString();
                string text = string.Format("{0}∟{1}", new string('　', parentLevel), textProperty.GetValue(childModel).ToString());
                allListItems.Add(GetNewItem(htmlHelper,text, value, selectedValues.Contains(value), nowLevel));
                CreateChildOption(htmlHelper,modelList, valueProperty, textProperty, parentProperty, value, selectedValues, nowLevel, allListItems);
            }
        }

        #endregion
    }
}
