
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EC226.MemberSite.Model.ViewModels
{
    public class RecipienAddressVM
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public string RecipientName { get; set; }
        public string Phone { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }
        public string Address{ get; set; }
        public Boolean IsNeverSetUp { get; set; }
        
    }
}
