
using System.ComponentModel.DataAnnotations;

namespace EC226.MemberSite.Model.ViewModels.Basics
{
    /// <summary>
    ///     全域性配置字典值內容
    /// </summary>
    public class DictionaryKeyValues
    {
        /// <summary>
        ///     key
        /// </summary>
        [Required(ErrorMessage = "key不能為空")]
        public string sKey { get; set; }

        /// <summary>
        ///     值
        /// </summary>
        public string sValue { get; set; }
    }


    /// <summary>
    ///     常用複用無條件匹配
    /// </summary>
    public class CommonKeyValues
    {
        /// <summary>
        ///     key
        /// </summary>
        [Required(ErrorMessage = "key不能為空")]
        public string sKey { get; set; }

        /// <summary>
        ///     值
        /// </summary>
        public string sValue { get; set; }


        /// <summary>
        ///     說明備註
        /// </summary>
        public string sDescription { get; set; }
    }
}
