
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace EC226.MemberSite.Model.ViewModels
{
    public class StandardProductUnitVM
    {
        
        public int Id { get; set; }
        
        [DisplayName("產品名稱")]
        public string Name { get; set; }

        [DisplayName("成份")]
        public string Ingredients { get; set; }

        [DisplayName("描述")]
        public string Description { get; set; }
        public int CategoryId { get; set; }

        [DisplayName("排序")]
        public int Sort { get; set; }
        public bool Enable { get; set; }
        public string CoverUrl { get; set; }

        [DisplayName("分類")]
        public string  CategoryName { get; set; }

        [DisplayName("熱門商品")]
        public bool IsHot { get; set; }

        [DisplayName("圖片")]
        public List<string> Images { get; set; }
    }
   
}
