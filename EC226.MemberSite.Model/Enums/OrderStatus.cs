using EC226.MemberSite.Model.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace EC226.MemberSite.Model.Enums
{
    public enum OrderStatus
    {
        
        待付款 = 0,

        [State((int)OrderStatus.待付款)]
        己付款 =1,

        [State((int)OrderStatus.己付款)]
        製作中 = 10,

        [State((int)OrderStatus.製作中)]
        待出貨 = 30,       
                
        [State((int)OrderStatus.待出貨)]
        已出貨 = 40,       

        [State((int)OrderStatus.已出貨)]
        已到貨 = 50,
    }

}
