
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace EC226.MemberSite.Model.ViewModels
{      

    public class StockKeepingUnitVM
    {
        [JsonProperty("skuId")]        
        public int Id { get; set; }
        
        public int StandardProductUnitId { get; set; }

        [JsonProperty("text")]
        public string Name { get; set; }
                
        public string MainPicture { get; set; }

        [JsonProperty("id")]
        public int SpecificationId { get; set; }


        [JsonProperty("leaf")]

        public List<StockKeepingUnitSpecificationVM> SkuSpecificationList { get; set; }
    }

    public class StockKeepingUnitSpecificationVM
    {
        [JsonProperty("spuId")]
        public int StandardProductUnitId { get; set; }

        [JsonProperty("skuId")]
        public int StockKeepingUnitId { get; set; }


        [JsonProperty("specId")]
        public int SpecificationId { get; set; }

        [JsonProperty("text")]       

        public string SpecificationOptionName { get; set; }

        [JsonProperty("id")]
        public int SpecificationOptionId { get; set; }

        [JsonProperty("is_show")]
        public bool ShowImage { get; set; }

    }

}
