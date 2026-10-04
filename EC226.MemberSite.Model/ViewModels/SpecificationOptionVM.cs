
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace EC226.MemberSite.Model.ViewModels
{
    public class SpecificationOptionVM 
    {
        public int Id { get; set; }

        [JsonProperty("text")]
        public string Name { get; set; }

        [JsonProperty("specId")]
        public int SpecificationId { get; set; }
    }
}
