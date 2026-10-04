using AutoMapper;
using EC226.MemberSite.Model.Entities;
using EC226.MemberSite.Model.ViewModels;
using System.Reflection;

namespace EC226.MemberSite.Services.AutoMapperProfile
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            CreateMap<RecipienAddress, RecipienAddressVM>();
            CreateMap<Article, ArticleVM>();
            CreateMap<Member, MemberInfoVM>();
            
            CreateMap<Article, ArticleListVM>();

            CreateMap<Member, AccountVM>();

            CreateMap<AccountVM, Member>();

            CreateMap<Member, MemberListVM>();

            CreateMap<Role, RoleVM>();

            CreateMap<RoleVM, Role>();

            CreateMap<Role, RoleViewVM>();

            CreateMap<Program, ProgramVM>();

            CreateMap<ProgramVM, Program>();

            CreateMap<Program, ProgramViewVM>();

            //CreateMap<StandardProductUnit, StandardProductUnitVM>()
            //        .ForMember(x => x.CategoryName, opt => opt.MapFrom(model => model.Category.Name));

            CreateMap<StockKeepingUnit, StockKeepingUnitVM>();

            CreateMap<Specification, SpecificationVM>();

            CreateMap<SpecificationOption, SpecificationOptionVM>();

            CreateMap<Category, CategoryVM>();

            CreateMap<Category, CategoryVM>();


            CreateMap<RegisterMemberVM, Member>();

            CreateMap<Member, MemberVM>();

            CreateMap<MemberAuthenticationProvider, MemberAuthenticationProviderVM>();


            CreateMap<Order, OrderVM>();

            CreateMap<OrderDetail, OrderDetailVM>();


            CreateMap<EC226.MemberSite.Model.Entities.Payment, PaymentVM>();

            CreateMap<PaymentOption, PaymentOptionVM>();

        }


    }
}
