using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;

namespace EC226.MemberSite.Model.ViewModels
{
    public class StockKeepingUnitStockVM
    {
        public int Id { get; set; }        
        public int StandardProductUnitId { get; set; }
        public int Price { get; set; }
        public int Stock { get; set; }
        public int Cost { get; set; }
        public List<SkuStockItem> Skus { get; set; }
        public DateTime CreateDate { get; set; }
    }

    public class SkuStockItem
    {
        [JsonProperty("specId")]
        public string SpecificationId { get; set; }

        [JsonProperty("specName")]
        public string Name { get; set; }

        [JsonProperty("specOptionId")]
        public string SpecificationOptionId { get; set; }
        [JsonProperty("specOptionName")]
        public string SpecificationOptionName { get; set; }
    }


}
