
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using EC226.Auth.OverWrite;
using EC226.Loging;
using EC226.MemberSite.IRepository;
using EC226.MemberSite.IRepository.UnitOfWork;
using EC226.MemberSite.IServices;
using EC226.MemberSite.Model.Entities;
using EC226.MemberSite.Model.Enums;
using EC226.MemberSite.Model.ViewModels;
using EC226.MemberSite.Model.ViewModels.UI;
using EC226.Pagination;
using EC226.Payment;
using EC226.Utility.Extensions;
using EC226.Utility.Helper;
using EC226.Utility.JWT;
using EC226.Utility.Library.Extensions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;


namespace EC226.MemberSite.Services
{

   public class MemberService: BaseServices<Member>, IMemberService
   {
      private readonly IMemberRepository _dal;

      private readonly IUnitOfWork _unitOfWork;
      public MemberService(IUnitOfWork unitOfWork, IMemberRepository dal)
      {
         this._dal = dal;
         base.BaseDal = dal;
         _unitOfWork = unitOfWork;
      }

      const string Salt = "Q_)x]H7Gxxs";

      private static Dictionary<string, Member> refreshTokens = new Dictionary<string, Member>();

      private async Task<(bool isExist, Member member)> CheckMemberExistAsync(string email)
      {
         var member = await this._unitOfWork.GetDbClient().Queryable<Member>().Where(x => x.Email == email.ToLower()).FirstAsync();
         return (member != null, member);
      }

      public async Task<Result<LoginTokenVM>> RegisterAsync(RegisterMemberVM registerVM)
      {
         var result = new Result<LoginTokenVM>();

         try
         {
            if ((await CheckMemberExistAsync(registerVM.Email)).isExist)
            {
               result.Fail("Email 重覆", ApiReturnCode.Email_Exist);
               return result;
            }

            Member member = new Member();
            AutoMapperInstance.ConvertModel(registerVM, member);

            member.Hash = Guid.NewGuid().ToString("N");
            member.Password = (registerVM.Password + Salt).HMACSHA1(member.Hash);
            member.Enable = true;
            member.DisplayName = registerVM.Name;
            member.HasChangePassword = true;

            await this._unitOfWork.GetDbClient().Insertable<Member>(member).ExecuteCommandAsync();            

            MemberPayLoadData userPayLoad = new MemberPayLoadData
            {
               Channel = Channel.Desktop,
               IP = IpHelper.GetIp(),
               Email = registerVM.Email.ToLower(),
               MemberId = member.Id,
               CreateDateTime = DateTime.Now.ToString()
            };

            var tokenManger = new JWTTokenManger<MemberPayLoadData>();
            var token = tokenManger.Generator(userPayLoad);

            result.Data.AccessToken = token.AccessToken;
            result.Data.RefreshToken = token.RefreshToken;
            result.Data.Expired = token.Expired;

            refreshTokens.Add(token.RefreshToken, member);

            result.Success();
         }
         catch (Exception ex)
         {
            NLogUtil.WriteSEQLog($"Error: {ex.Message}, StackTrace: {ex.StackTrace}", NLog.LogLevel.Error);
         }

         return result;
      }

      public async Task<Result<LoginTokenVM>> LoginAsync(LoginVM loign)
      {
         var result = new Result<LoginTokenVM>();

         try
         {
            if (string.IsNullOrWhiteSpace(loign.Email) || string.IsNullOrWhiteSpace(loign.Password))
            {
               result.Fail("不得為空", ApiReturnCode.Invild_Parameter);
               return result;
            }

            var member = await this._unitOfWork.GetDbClient().Queryable<Member>().Where(x => x.Email == loign.Email.ToLower()).FirstAsync();

            if (member != null)
            {
               var isLogin = member.Password == (loign.Password + Salt).HMACSHA1(member.Hash);

               if (!isLogin)
               {
                  result.Fail("帳號密密碼錯誤", ApiReturnCode.Username_Or_Password_Is_Incorrect);
                  return result;
               }

               if (!member.Enable)
               {
                  result.Fail("帳號被禁用", ApiReturnCode.Invild_Parameter);
                  return result;
               }

               MemberPayLoadData userPayLoad = new MemberPayLoadData
               {
                  Channel = Channel.Desktop,
                  IP = IpHelper.GetIp(),
                  Email = loign.Email.ToLower(),
                  MemberId = member.Id,
                  CreateDateTime = DateTime.Now.ToString()
               };

               var tokenManger = new JWTTokenManger<MemberPayLoadData>();
               var token = tokenManger.Generator(userPayLoad);

               result.Data.AccessToken = token.AccessToken;
               result.Data.RefreshToken = token.RefreshToken;
               result.Data.Expired = token.Expired;

               result.Success();
            }
         }
         catch (Exception ex)
         {
            NLogUtil.WriteSEQLog($"Error: {ex.Message}, StackTrace: {ex.StackTrace}", NLog.LogLevel.Error);
         }

         return result;
      }

      public async Task<Result<MemberProfileVM>> GetMemberProfileAsync(int memberId)
      {
         var result = new Result<MemberProfileVM>();

         try
         {
            var member = await this._unitOfWork.GetDbClient().Queryable<Member>().Where(x => x.Id == memberId && x.Enable).Includes(x => x.RecipienAddresses).FirstAsync();

            if (member != null)
            {
               var address = member.RecipienAddresses.FirstOrDefault(x => x.IsDefault);

               result.Data.Address = AutoMapperInstance.ConvertModel(address, result.Data.Address);
               result.Data.MemberInfo = AutoMapperInstance.ConvertModel(member, result.Data.MemberInfo);

               if (result.Data.Address == null)
               {
                  result.Data.Address = new RecipienAddressVM()
                  {
                     RecipientName = member.DisplayName,
                     Phone = member.Phone,
                     ZipCode = "100",
                     Country = "臺北市",
                     IsNeverSetUp = true,
                  };
               }

               result.Success();
            }
         }
         catch (Exception ex)
         {
            NLogUtil.WriteSEQLog($"Error: {ex.Message}, StackTrace: {ex.StackTrace}", NLog.LogLevel.Error);
         }

         return result;
      }

      public async Task<Result<MemberInfoVM>> GetMemberInfoAsync(int memberId)
      {
         var result = new Result<MemberInfoVM>();

         try
         {
            var member = await this._unitOfWork.GetDbClient().Queryable<Member>().Where(x => x.Id == memberId && x.Enable).FirstAsync();

            if (member != null)
            {
               result.Data.DisplayName = member.DisplayName;
               result.Data.Email = member.Email;
               result.Data.Phone = member.Phone;

               result.Success();
            }
         }
         catch (Exception ex)
         {
            NLogUtil.WriteSEQLog($"Error: {ex.Message}, StackTrace: {ex.StackTrace}", NLog.LogLevel.Error);
         }

         return result;
      }

      public async Task<Result<LoginTokenVM>> SocialNetworkLoginAsync(SocialNetworkLogin sLogin)
      {
         var result = new Result<LoginTokenVM>();

         try
         {
            if (string.IsNullOrWhiteSpace(sLogin.Email) || string.IsNullOrWhiteSpace(sLogin.ProviderId) || string.IsNullOrWhiteSpace(sLogin.ProviderMemberId))
            {
               result.Fail("不得為空", ApiReturnCode.Invild_Parameter);
               return result;
            }

            var memberAuthenticationProvider = await this._unitOfWork.GetDbClient().Queryable<MemberAuthenticationProvider>().Where(x => x.ProviderMemberId == sLogin.ProviderMemberId && x.Email == sLogin.Email && x.ProviderId == sLogin.ProviderId).FirstAsync();
            var memberExist = await CheckMemberExistAsync(sLogin.Email);

            Member member = memberExist.member;

            if (memberAuthenticationProvider != null && memberExist.isExist)
            {
               goto GetLoginInfo;
            }

            if (memberAuthenticationProvider == null && !memberExist.isExist)
            {
               this._unitOfWork.BeginTran();
               member = new Member
               {
                  Hash = Guid.NewGuid().ToString("N"),
                  Password = Guid.NewGuid().ToString("N"),
                  HasChangePassword = false,
                  Enable = true,
                  DisplayName = sLogin.DisplayName,
                  Email = sLogin.Email
               };

               await this._unitOfWork.GetDbClient().Insertable<Member>(member).ExecuteCommandAsync();

               var newMemberAuth = new MemberAuthenticationProvider
               {
                  FirebaseMemberId = sLogin.FirebaseMemberId,
                  ProviderMemberId = sLogin.ProviderMemberId,
                  ProviderId = sLogin.ProviderId,
                  MemberId = member.Id,
                  DisplayName = sLogin.DisplayName,
                  Email = sLogin.Email,
                  Enable = true
               };

               await this._unitOfWork.GetDbClient().Insertable<MemberAuthenticationProvider>(newMemberAuth).ExecuteCommandAsync();

               this._unitOfWork.CommitTran();
            }
            else if (memberExist.isExist && memberAuthenticationProvider == null)
            {
               this._unitOfWork.BeginTran();
               var newMemberAuth = new MemberAuthenticationProvider
               {
                  FirebaseMemberId = sLogin.FirebaseMemberId,
                  ProviderMemberId = sLogin.ProviderMemberId,
                  ProviderId = sLogin.ProviderId,
                  MemberId = member.Id,
                  DisplayName = sLogin.DisplayName,
                  Email = sLogin.Email,
                  Enable = true
               };

               await this._unitOfWork.GetDbClient().Insertable<MemberAuthenticationProvider>(newMemberAuth).ExecuteCommandAsync();

               this._unitOfWork.CommitTran();
            }
            else
            {
               Guard.Against<Exception>(true, $"異常登入:{JsonConvert.SerializeObject(sLogin)}");
            }

         GetLoginInfo:
            MemberPayLoadData userPayLoad = new MemberPayLoadData
            {
               Channel = Channel.Desktop,
               IP = IpHelper.GetIp(),
               Email = sLogin.Email.ToLower(),
               MemberId = member.Id,
               CreateDateTime = DateTime.Now.ToString()
            };

            var tokenManger = new JWTTokenManger<MemberPayLoadData>();
            var token = tokenManger.Generator(userPayLoad);
            result.Data.AccessToken = token.AccessToken;
            result.Data.RefreshToken = token.RefreshToken;
            result.Data.Expired = token.Expired;

            refreshTokens.Add(token.RefreshToken, member);
            result.Success();
         }
         catch (Exception ex)
         {

            NLogUtil.WriteSEQLog($"異常登入 Error: {ex.Message}, StackTrace: {ex.StackTrace},{JsonConvert.SerializeObject(sLogin)}", NLog.LogLevel.Error);
            this._unitOfWork.RollbackTran();
         }

         return result;
      }

      public async Task<Result> UpdateMemberInfoAsync(int memberId, UpdateProfilerVM memberInfoVM)
      {
         var result = new Result();

         try
         {
            var member = await this._unitOfWork.GetDbClient().Queryable<Member>().Where(x => x.Id == memberId && x.Enable).FirstAsync();

            if (member != null)
            {
               if (!string.IsNullOrWhiteSpace(memberInfoVM.DisplayName))
               {
                  member.DisplayName = memberInfoVM.DisplayName;
               }

               if (!string.IsNullOrWhiteSpace(memberInfoVM.Phone))
               {
                  member.Phone = memberInfoVM.Phone;
               }

               if (!string.IsNullOrWhiteSpace(memberInfoVM.OldPassword) && !string.IsNullOrWhiteSpace(memberInfoVM.NewPassword) && member.HasChangePassword)
               {
                  if (member.Password == (memberInfoVM.OldPassword + Salt).HMACSHA1(member.Hash))
                  {
                     member.Password = (memberInfoVM.NewPassword + Salt).HMACSHA1(member.Hash);
                  }
                  else
                  {
                     Guard.Against<Exception>(true, "新舊密碼不同");
                  }
               }
               else if (!string.IsNullOrWhiteSpace(memberInfoVM.NewPassword) && !member.HasChangePassword)
               {
                  member.Password = (memberInfoVM.NewPassword + Salt).HMACSHA1(member.Hash);
                  member.HasChangePassword = true;
               }

               member.ModifyTime = DateTime.UtcNow;

               
            }

            result.Success();
         }
         catch (Exception ex)
         {
            NLogUtil.WriteSEQLog($"Error: {ex.Message}, StackTrace: {ex.StackTrace}", NLog.LogLevel.Error);
         }

         return result;
      }

      public async Task<Result> UpdateDefaultAddressAsync(int memberId, RecipienAddressVM addressVM)
      {
         var result = new Result();

         try
         {
            var address = await this._unitOfWork.GetDbClient().Queryable<RecipienAddress>().Where(x => x.MemberId == memberId && x.IsDefault).FirstAsync();

            if (address != null)
            {
               address.RecipientName = addressVM.RecipientName;
               address.Phone = addressVM.Phone;
               address.Country = addressVM.Country;
               address.ZipCode = addressVM.ZipCode;
               address.Address = addressVM.Address;
               address.ModifyTime = DateTime.UtcNow;
               address.IsDefault = true;
            }
            else
            {
               var newAddress = new RecipienAddress
               {
                  MemberId = memberId,
                  RecipientName = addressVM.RecipientName,
                  Phone = addressVM.Phone,
                  Country = addressVM.Country,
                  ZipCode = addressVM.ZipCode,
                  Address = addressVM.Address,
                  CreateTime = DateTime.UtcNow,
                  IsDefault = true
               };

               await this._unitOfWork.GetDbClient().Insertable<RecipienAddress>(newAddress).ExecuteCommandAsync();
            }

            result.Success();
         }
         catch (Exception ex)
         {
            NLogUtil.WriteSEQLog($"Error: {ex.Message}, StackTrace: {ex.StackTrace}", NLog.LogLevel.Error);
         }

         return result;
      }

   }
}
