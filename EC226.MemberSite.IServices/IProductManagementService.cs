using EC226.MemberSite.Model.ViewModels;
using EC226.MemberSite.Model.ViewModels.UI;
using EC226.Pagination;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EC226.MemberSite.IServices
{
   public interface IProductManagementService
   {
      Task<Result<ProductDetailVM>> GetWebPortalProductDetailResultAsync(int spuId);
      Task<Result<PageModel<ProductVM>>> GetWebPortalProductListResultAsync(PageModel model);
      Task<PageModel<ProductVM>> GetWebPortalProductListAsync(PageModel model);

      Task<Result<PageModel<StandardProductUnitVM>>> PagedListResultAsync(PageModel model);

      Task<Result<StandardProductUnitVM>> GetSpuResultByIdAsync(int id);

      Task<StandardProductUnitVM> GetSpuByIdAsync(int id);

      Task<Result<List<SpecificationVM>>> GetSpecificationResultsByCategoryIdAsync(int? categoryId);

      Task<List<SpecificationVM>> GetSpecificationByCategoryIdAsync(int? categoryId);

      Task<Result<List<StockKeepingUnitVM>>> GetSkuTreeResultBySpuIdAsync(int spuId);

      Task<List<StockKeepingUnitVM>> GetSkuTreeBySpuIdAsync(int spuId);

      Task<Result<List<SpecificationOptionVM>>> GetSpecificationOptionResultByIdAsync(int specId);

      Task<List<SpecificationOptionVM>> GetSpecificationOptionByIdAsync(int specId);

      Task<Result<SpecificationVM>> CreateSpecificationAsync(int spuId, string name);

      Task<Result<List<StockKeepingUnitSpecificationVM>>> CreateSpecificationOptionAsync(CreateSpecOptionVM createSpecOptionVM);

      Task<Result> DeleteSkuAsync(int skuId);

      Task<Result> DeleteSkuOptionAsync(int skuId, int specId, int specOptionId);

      Task<Result<StandardProductUnitVM>> CreateSpuAsync(StandardProductUnitVM vm);
      Task<Result> UpdateSpuAsync(StandardProductUnitVM vm);

      Task<Result<List<StockKeepingUnitStockVM>>> GetStockByStandardProductUnitIdAsync(int spuId);

      Task<Result> UpdateStockAsync(List<StockKeepingUnitStockVM> updateDatas);

      Task<Result> UpdateSpuImagesAsync(UpdateSpuImagesVM vm);
   }
}
