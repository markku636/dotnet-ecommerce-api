using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EC226.Utility.JWT
{
    public class JWTHeader
    {
        public string ALG { get; set; }
        public string Type { get; set; }
        public  string IV { get; set; }
    }
}
