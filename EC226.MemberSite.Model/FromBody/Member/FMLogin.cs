using System.ComponentModel.DataAnnotations;

namespace EC226.MemberSite.Model.FromBody.Member
{

    public class FMLogin
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string TurnstileToken { get; set; }
    }

    public class FMGuestLogin
    {
        [Required]
        public string TurnstileToken { get; set; }

    }
}
