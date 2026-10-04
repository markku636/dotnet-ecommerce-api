using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace EC226.MemberSite.Model.ViewModels
{
    public class SpecificationVM
    {
        public int Id { get; set; }

        [JsonProperty("text")]
        public string Name { get; set; }
        public int Sort { get; set; }
        public int? CategoryId { get; set; }

        public SpecificationOptionVM options { get; set; }
    }
}
