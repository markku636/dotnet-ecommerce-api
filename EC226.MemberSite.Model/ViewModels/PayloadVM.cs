

namespace EC226.MemberSite.Model.ViewModels
{
    public class PayloadVM
    {
        //使用者資訊
        public AccountVM Account { get; set; }
        //過期時間
        public int Expired { get; set; }
    }
}
