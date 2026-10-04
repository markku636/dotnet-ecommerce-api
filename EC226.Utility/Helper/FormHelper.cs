using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using EC226.Configuration;
using NPOI.OpenXmlFormats.Dml;

namespace EC226.Utility.Helper
{
    public static class FormHelper
    {

        /// <summary>
        /// 驗證欄位型別及提交的值是否對應
        /// </summary>
        /// <param name="typeName"></param>
        /// <param name="thisValue"></param>
        /// <returns></returns>
        public static bool ValidateField(string typeName, object thisValue)
        {
            var bl = false;
            var valueType = thisValue.GetType();
            if (typeName == GlobalEnumVars.FormValidationTypes.字串.ToString())
            {
                return valueType == typeof(string);
            }
            else if (typeName == GlobalEnumVars.FormValidationTypes.數字.ToString())
            {
                return thisValue != DBNull.Value && double.TryParse(thisValue.ToString(), out _);
            }
            else if (typeName == GlobalEnumVars.FormValidationTypes.整數.ToString())
            {
                return thisValue != DBNull.Value && int.TryParse(thisValue.ToString(), out _);
            }
            else if (typeName == GlobalEnumVars.FormValidationTypes.價格.ToString())
            {
                return thisValue != DBNull.Value && double.TryParse(thisValue.ToString(), out _);
            }
            else if (typeName == GlobalEnumVars.FormValidationTypes.郵箱.ToString())
            {
                if (valueType == typeof(string) && !string.IsNullOrEmpty(thisValue.ToString()))
                {
                    return Regex.IsMatch(thisValue.ToString() ?? string.Empty, @"^[\w\.]+([-]\w+)*@[A-Za-z0-9-_]+[\.][A-Za-z0-9-_]");
                }
                else
                {
                    return false;
                }
            }
            else if (typeName == GlobalEnumVars.FormValidationTypes.手機號.ToString())
            {
                if (valueType == typeof(string) && !string.IsNullOrEmpty(thisValue.ToString()))
                {
                    return CommonHelper.IsMobile(thisValue.ToString());
                }
                else
                {
                    return false;
                }
            }
            else if (typeName == GlobalEnumVars.FormValidationTypes.多數據.ToString())
            {
                return valueType == typeof(Array);
            }
            else
            {
                bl = false;
            }
            return bl;
        }


    }
}
