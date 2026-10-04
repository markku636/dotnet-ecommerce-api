using System.IO;
using AutoMapper;
using EC226.MemberSite.Model.ViewModels.UI;
using Newtonsoft.Json;

namespace EC226.Mapping
{
    /// <summary>
    /// AutoMapper的全域性實體對映配置靜態類
    /// </summary>
    public class AutoMapperConfiguration : Profile, AutoMapperIProfile
    {
        public AutoMapperConfiguration()
        {
            //CreateMap<Manager, ManagerDTO>().ReverseMap();

            CreateMap<SqlSugar.DbTableInfo, EC226.MemberSite.Model.ViewModels.Basics.DbTableInfoTree>()
                .AfterMap((from, to, context) =>
                {
                    to.Label = from.Name + "[" + from.Description + "]";
                });

            ////商品分類轉前端json
            //CreateMap<EC226GoodsCategory, DTreeList>()
            //    .AfterMap((from, to, context) =>
            //    {
            //        to.id = from.id.ToString();
            //        to.title = from.name;
            //        to.checkArr = "0";
            //        to.parentId = from.parentId.ToString();
            //    });

            //#region 小程式互動相關=======================================================================================
            ////小程式首頁取得頁面佈局資訊數據轉換
            //CreateMap<EC226PagesItems, PagesItemsDto>()
            //    .AfterMap((from, to, context) =>
            //    {
            //        to.parameters = new JsonSerializer().Deserialize(new JsonTextReader(new StringReader(from.parameters)));
            //    });
            //#endregion

        }
    }
}
