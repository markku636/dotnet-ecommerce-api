using System;
using System.Collections.Generic;
using System.Text;


namespace EC226.MemberSite.Model.ViewModels
{
    // 前台用
    public class ProductVM
    {
        public int StandardProductUnitId { get; set; }
        public string Name { get; set; }
        public string Ingredients { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public int Sort { get; set; }
        public bool Enable { get; set; }
        public string CoverUrl { get; set; }
        public List<StockDetailVM> StockDetails { get; set; }
    }

    public class ProductDetailVM
    {
        public int StandardProductUnitId { get; set; }
        public string Name { get; set; }
        public string Ingredients { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public int Sort { get; set; }
        public bool Enable { get; set; }
        public string CoverUrl { get; set; }
        public List<StockDetailVM> StockDetails { get; set; }

        public List<SpecificationWebVM> Specifications { get; set; }

        public List<string> ProductImages { get; set; }

    }


    public class SpecificationWebVM
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Sort { get; set; }        

        public List<SpecificationOptionWebVM> Options { get; set; }
    }

    public class SpecificationOptionWebVM
    {
        public int Id { get; set; }

        
        public string Name { get; set; }        
        
    }

    public class SkuSpecItem
    {
        public int SpecificationId { get; set; }

        public int SpecificationOptionId { get; set; }
    }

    public class StockDetailVM
    {
        public int Id { get; set; }
        public int StandardProductUnitId { get; set; }
        public int Price { get; set; }
        public int Stock { get; set; }
        public int Cost { get; set; }
        public string Skus { get; set; }
        public DateTime CreateDate { get; set; }
    }

}
