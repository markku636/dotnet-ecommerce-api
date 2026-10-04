using SqlSugar;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EC226.MemberSite.Model.Entities
{
    /// <summary>
    /// ArticleCategoryRelations
    /// </summary>
    [SugarTable("ArticleCategoryRelations")]
    public partial class ArticleCategoryRelations
    {
        /// <summary>
        /// 建構函式
        /// </summary>
        public ArticleCategoryRelations()
        {
        }
		        /// <summary>
        /// ArticleId
        /// </summary>
        [Display(Name = "ArticleId")]
        [Required(ErrorMessage = "請輸入{0}")]
        public System.Int32 ArticleId  { get; set; }
        
		        /// <summary>
        /// CategoryId
        /// </summary>
        [Display(Name = "CategoryId")]
        [Required(ErrorMessage = "請輸入{0}")]
        public System.Int32 CategoryId  { get; set; }
        
		
    }
}
