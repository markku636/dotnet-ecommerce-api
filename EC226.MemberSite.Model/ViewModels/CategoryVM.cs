
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace EC226.MemberSite.Model.ViewModels
{
    public class CategoryVM
    {
        [DisplayName("Id")]
        public int Id { get; set; }
        [DisplayName("分類名稱")]
        public string Name { get; set; }        

        [DisplayName("父分類")]
        public int? ParentId { get; set; }

        [DisplayName("排序")]
        public int Sort { get; set; }

        [DisplayName("啟用")]
        public bool Enable { get; set; }

        [DisplayName("刪除")]
        public bool IsDelete { get; set; }
    }
}
