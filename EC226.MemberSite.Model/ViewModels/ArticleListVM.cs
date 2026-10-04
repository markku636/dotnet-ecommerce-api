
using System;
using System.ComponentModel;
using EC226.MemberSite.Model.Entities;

namespace EC226.MemberSite.Model.ViewModels
{
    public class ArticleListVM : Article
    {
  
        public int Id { get; set; }

        [DisplayName("標題")]

        public string Title { get; set; }
        [DisplayName("內容")]
        public string Content { get; set; }
        
        [DisplayName("建立時間")]
        public DateTime CreateTime { get; set; }

        [DisplayName("修改時間")]
        public DateTime? ModifyTime { get; set; }
        public bool IsDelete { get; set; }
        public string CoverUrl { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public bool Enable { get; set; }
        [DisplayName("點擊數")]
        public int Hit { get; set; }
    }
}


