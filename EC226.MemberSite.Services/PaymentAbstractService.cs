using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Configuration;
using EC226.Core.Config;
using Microsoft.Extensions.Logging;

namespace EC226.Services
{
    public abstract class PaymentAbstractService
    {               

        public static string GetHostUrl()
        {
            var configuration = (IConfiguration)HttpContext.Current.RequestServices.GetService(typeof(IConfiguration));
            var config = configuration.GetSection("HostUrl");
            return config.Value;
        }

    }
}
