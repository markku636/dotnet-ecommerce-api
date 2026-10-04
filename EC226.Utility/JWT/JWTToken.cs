using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EC226.Utility.JWT
{
    public class JWTToken
    {
        public string AccessToken { get; set; }
        
        public string RefreshToken { get; set; }
        
        public int Expired { get; set; } // 幾秒後失效
    }

}
