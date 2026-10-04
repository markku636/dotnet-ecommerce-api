

using EC226.MemberSite.Model.Enums;

namespace EC226.Utility.JWT
{
    public class MemberPayLoadData
    {
        public int MemberId { get; set; }

        public Channel Channel { get; set; }
        public string MemberCode { get; set; }
        public string Email { get; set; }
        public string CreateDateTime { get; set; }
        public string IP { get; set; }
    }
}
