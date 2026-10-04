using SqlSugar;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EC226.MemberSite.Model.Entities
{
    /// <summary>
    /// ArticleCategoryRelation
    /// </summary>
    [SugarTable("ArticleCategoryRelation")]
    public partial class ArticleCategoryRelation
    {
        /// <summary>
        /// 建構函式
        /// </summary>
        public ArticleCategoryRelation()
        {
        }
		        /// <summary>
        /// ArticleId
        /// </summary>
        [Display(Name = "ArticleId")]
        [SugarColumn(IsPrimaryKey = true)]
[Required(ErrorMessage = "請輸入{0}")]
        public System.Int32 ArticleId  { get; set; }
        
		        /// <summary>
        /// CategoryId
        /// </summary>
        [Display(Name = "CategoryId")]
        [SugarColumn(IsPrimaryKey = true)]
[Required(ErrorMessage = "請輸入{0}")]
        public System.Int32 CategoryId  { get; set; }
        
		
    }
}
