using System.Threading.Tasks;
using EC226.MemberSite.Model.ViewModels;
using EC226.MemberSite.Model.ViewModels.UI;

namespace EC226.MemberSite.IServices
{
   public interface IMemberService
   {
      Task<Result<LoginTokenVM>> RegisterAsync(RegisterMemberVM registerVM);
      Task<Result<LoginTokenVM>> SocialNetworkLoginAsync(SocialNetworkLogin sLogin);
      Task<Result<LoginTokenVM>> LoginAsync(LoginVM loign);

      Task<Result<MemberInfoVM>> GetMemberInfoAsync(int memberId);
      Task<Result<MemberProfileVM>> GetMemberProfileAsync(int memberId);

      Task<Result> UpdateMemberInfoAsync(int memberId, UpdateProfilerVM updateProfilerVM);

      Task<Result> UpdateDefaultAddressAsync(int memberId, RecipienAddressVM addressVM);
   }
}
