using System;
using System.Collections.Generic;
using System.Text;
using EC226.Pagination;
using EC226.MemberSite.Model.ViewModels;
using EC226.MemberSite.Model.ViewModels.UI;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using EC226.MemberSite.IServices;
using EC226.Pagination;
using EC226.MemberSite.Model.ViewModels.UI;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace EC226.MemberSite.IServices
{
    public interface IAccountService
    {
        Result<UserInfo> Login(LoginViewModel loginViewModel);

        Result<PageModel<MemberListVM>> PagedListResult(PageModel model);

        PageModel<MemberListVM> PagedList(PageModel model);

        Result<AccountVM> GetById(int id);

        Result<ModelStateDictionary> CheckData(AccountVM vm);
        Result Create(AccountVM vm);

        Result Update(AccountVM vm);

        void UpdateUserRoleRelation(int currentUserId, List<int> selectedRoleIds = null);

        Result Delete(int id);
    }
}
