using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EC226.Utility.Extensions
{
    public class ErrorExtensions
    {
        public class ReCaptchaException: Exception
        {
            public ReCaptchaException(string message) : base(message) { }
        }
        public class ThirdPartyException: Exception
        {
            public ThirdPartyException(string message) : base(message) { }
        }

        public class TurnstileException: Exception
        {
            public TurnstileException(string message) : base(message) { }
        }

    }
}
