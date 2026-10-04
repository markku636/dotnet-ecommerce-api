using System;
using System.Collections.Generic;
using System.ComponentModel;
using EC226.MemberSite.Model.ViewModels.UI;

namespace EC226.Utility.Helper
{
    public class EnumHelper
    {
        /// <summary>
        /// 將列舉轉成List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<EnumEntity> EnumToList<T>()
        {
            List<EnumEntity> list = new List<EnumEntity>();

            foreach (var e in Enum.GetValues(typeof(T)))
            {
                EnumEntity m = new EnumEntity();
                object[] objArr = e.GetType().GetField(e.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), true);
                if (objArr != null && objArr.Length > 0)
                {
                    DescriptionAttribute da = objArr[0] as DescriptionAttribute;
                    m.description = da.Description;
                }
                m.value = Convert.ToInt32(e);
                m.title = e.ToString();
                list.Add(m);
            }
            return list;
        }

        /// <summary>
        /// 根據列舉值來取得單個列舉實體
        /// </summary>
        /// <typeparam name="T">列舉</typeparam>
        /// <param name="value">value</param>
        /// <returns></returns>
        public static EnumEntity GetEnumberEntity<T>(int value)
        {
            foreach (var e in Enum.GetValues(typeof(T)))
            {
                EnumEntity m = new EnumEntity();
                object[] objArr = e.GetType().GetField(e.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), true);
                if (objArr != null && objArr.Length > 0)
                {
                    DescriptionAttribute da = objArr[0] as DescriptionAttribute;
                    m.description = da.Description;
                }
                m.value = Convert.ToInt32(e);
                m.title = e.ToString();
                if (value == m.value)
                {
                    return m;
                }
            }
            return null;
        }


        /// <summary>
        /// 根據列舉值value來取得單個列舉實體的文字描述內容
        /// </summary>
        /// <typeparam name="T">列舉</typeparam>
        /// <param name="value">value</param>
        /// <returns></returns>
        public static string GetEnumDescriptionByValue<T>(int value)
        {
            foreach (var e in Enum.GetValues(typeof(T)))
            {
                EnumEntity m = new EnumEntity();
                object[] objArr = e.GetType().GetField(e.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), true);
                if (objArr != null && objArr.Length > 0)
                {
                    DescriptionAttribute da = objArr[0] as DescriptionAttribute;
                    m.description = da.Description;
                }
                m.value = Convert.ToInt32(e);
                m.title = e.ToString();
                if (value == m.value)
                {
                    return m.description;
                }
            }
            return "";
        }

        /// <summary>
        /// 根據列舉key來取得單個列舉實體的文字描述內容
        /// </summary>
        /// <typeparam name="T">列舉</typeparam>
        /// <param name="key">value</param>
        /// <returns></returns>
        public static string GetEnumDescriptionByKey<T>(string key)
        {
            foreach (var e in Enum.GetValues(typeof(T)))
            {
                EnumEntity m = new EnumEntity();
                object[] objArr = e.GetType().GetField(e.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), true);
                if (objArr != null && objArr.Length > 0)
                {
                    DescriptionAttribute da = objArr[0] as DescriptionAttribute;
                    m.description = da.Description;
                }
                m.value = Convert.ToInt32(e);
                m.title = e.ToString();
                if (key == m.title)
                {
                    return m.description;
                }
            }
            return "";
        }

    }




}
