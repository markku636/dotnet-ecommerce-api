
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EC226.Loging;
using EC226.MemberSite.IRepository;
using EC226.MemberSite.IRepository.UnitOfWork;
using EC226.MemberSite.IServices;
using EC226.MemberSite.Model.Entities;
using EC226.MemberSite.Model.ViewModels;
using EC226.MemberSite.Model.ViewModels.UI;
using EC226.Pagination;
using EC226.Utility.Helper;
using EC226.Utility.Library.Extensions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Stripe;


namespace EC226.MemberSite.Services
{
   public class ProductManagementService: IProductManagementService
   {
      private readonly IUnitOfWork _unitOfWork;
      public ProductManagementService(IUnitOfWork unitOfWork)
      {
         _unitOfWork = unitOfWork;
      }

      public async Task<PageModel<StandardProductUnitVM>> PagedListAsync(PageModel model)
      {
         var accountViewVms = new List<StandardProductUnitVM>();


         var query = this._unitOfWork.GetDbClient().Queryable<StandardProductUnit>()
                    .LeftJoin<Category>((s, c) => s.CategoryId == c.Id)//多个条件用&&
                    .Where(x => x.Enable)
                    .OrderBy(x => x.Id)
                    .WhereAndOrder(model)
                    .OrderBy(model.Order);

         var count = await query.CountAsync();
         var spuList = await query
             .Skip((model.PageIndex - 1) * model.PageSize)
             .Take(model.PageSize)
             .ToListAsync();

         AutoMapperInstance.ConvertModel(spuList, accountViewVms);

         return model.Record(accountViewVms, count)
             .SetPrimaryKey(x => x.Id)
             .ColumFor(x => x.Id)
             .ColumFor(x => x.CategoryName)
             .ColumFor(x => x.Name)
             .ColumFor(x => x.Ingredients)
             .ColumFor(x => x.Sort);
      }

      public async Task<Result<PageModel<ProductVM>>> GetWebPortalProductListResultAsync(PageModel model)
      {
         var result = new Result<PageModel<ProductVM>>();
         try
         {
            result.Data = await GetWebPortalProductListAsync(model);
            result.Success();
         }
         catch (Exception ex)
         {
            NLogUtil.WriteSEQLog($"Error: {ex.Message}, StackTrace: {ex.StackTrace}", NLog.LogLevel.Error);
         }

         return result;
      }

      public async Task<Result<ProductDetailVM>> GetWebPortalProductDetailResultAsync(int spuId)
      {
         var result = new Result<ProductDetailVM>();

         try
         {
            var spu = await this._unitOfWork.GetDbClient().Queryable<StandardProductUnit>().Includes(x => x.StandardProductUnitImages).FirstAsync(x => x.Id == spuId && x.Enable);



            if (spu == null) return result;

            var productDetailVM = new ProductDetailVM
            {
               StandardProductUnitId = spu.Id,
               Name = spu.Name,
               Ingredients = spu.Ingredients,
               Description = spu.Description,
               CategoryId = spu.CategoryId,
               Sort = spu.Sort,
               CoverUrl = spu.CoverUrl,
               Enable = spu.Enable,
               ProductImages = spu.StandardProductUnitImages.Select(x => x.ImagePath).ToList(),
               StockDetails = new List<StockDetailVM>(),
               Specifications = new List<SpecificationWebVM>()
            };

            var skuStockDetails = await this._unitOfWork.GetDbClient().Queryable<StockKeepingUnitStock>().Where(x => x.StandardProductUnitId == spuId).ToListAsync();



            var currentSkuDetails = skuStockDetails
                .Where(x => x.StandardProductUnitId == spu.Id)
                .OrderBy(x => x.CreateDate)
                .ToList();

            foreach (var skuDetail in currentSkuDetails)
            {
               productDetailVM.StockDetails.Add(new StockDetailVM
               {
                  Id = skuDetail.Id,
                  StandardProductUnitId = skuDetail.StandardProductUnitId,
                  Price = skuDetail.Price,
                  Stock = skuDetail.Stock,
                  Cost = skuDetail.Cost,
                  Skus = skuDetail.SKUs
               });
            }

            var relations = await this._unitOfWork.GetDbClient().Queryable<SpecificationOptionRelation>()
                .Where(x => x.StandardProductUnitId == spuId)
                .ToListAsync();

            var specificationIds = relations.Select(x => x.SpecificationId).ToList();
            var specificationOptionIds = relations.Select(x => x.SpecificationOptionId).ToList();

            var spec = await this._unitOfWork.GetDbClient().Queryable<Specification>()
                .Where(x => specificationIds.Contains(x.Id) &&
                          x.SpecificationOptions.Any(y => specificationOptionIds.Contains(y.Id) && y.SpecificationId == x.Id))
                .Includes(x => x.SpecificationOptions)
                .OrderBy(x => x.Sort)
                .ToListAsync();

            foreach (var item in spec)
            {
               var specVM = new SpecificationWebVM
               {
                  Id = item.Id,
                  Name = item.Name,
                  Sort = item.Sort,
                  Options = item.SpecificationOptions.Select(option => new SpecificationOptionWebVM
                  {
                     Id = option.Id,
                     Name = option.Name
                  }).ToList()
               };

               productDetailVM.Specifications.Add(specVM);
            }

            result.Data = productDetailVM;
            result.Success();
         }
         catch (Exception ex)
         {
            NLogUtil.WriteSEQLog($"Error: {ex.Message}, StackTrace: {ex.StackTrace}", NLog.LogLevel.Error);
         }

         return result;
      }

      public async Task<PageModel<ProductVM>> GetWebPortalProductListAsync(PageModel model)
      {
         var products = new List<ProductVM>();

         var query = this._unitOfWork.GetDbClient().Queryable<StandardProductUnit>()
             .Where(x => x.Enable)
             .WhereAndOrder(model)
             .OrderBy(model.Order);

         var count = await query.CountAsync();
         var spus = await query
             .Skip((model.PageIndex - 1) * model.PageSize)
             .Take(model.PageSize)
             .ToListAsync();

         var spuIds = spus.Select(x => x.Id).ToList();

         var skuStockDetails = await this._unitOfWork.GetDbClient().Queryable<StockKeepingUnitStock>()
             .Where(x => spuIds.Contains(x.StandardProductUnitId))
             .ToListAsync();

         foreach (var item in spus)
         {
            var newProduct = new ProductVM
            {
               StandardProductUnitId = item.Id,
               Name = item.Name,
               Ingredients = item.Ingredients,
               Description = item.Description,
               CategoryId = item.CategoryId,
               Sort = item.Sort,
               CoverUrl = item.CoverUrl,
               StockDetails = skuStockDetails
                    .Where(x => x.StandardProductUnitId == item.Id)
                    .OrderBy(x => x.CreateDate)
                    .Select(skuDetail => new StockDetailVM
                    {
                       Id = skuDetail.Id,
                       StandardProductUnitId = skuDetail.StandardProductUnitId,
                       Price = skuDetail.Price,
                       Stock = skuDetail.Stock,
                       Cost = skuDetail.Cost,
                       Skus = skuDetail.SKUs
                    })
                    .ToList()
            };

            products.Add(newProduct);
         }

         return model.Record(products, count);
      }

      public async Task<Result<PageModel<StandardProductUnitVM>>> PagedListResultAsync(PageModel model)
      {
         var result = new Result<PageModel<StandardProductUnitVM>>();

         try
         {
            result.Data = await PagedListAsync(model);
            result.Success();
         }
         catch (Exception ex)
         {
            NLogUtil.WriteSEQLog($"Error: {ex.Message}, StackTrace: {ex.StackTrace}", NLog.LogLevel.Error);
         }

         return result;
      }

      public async Task<Result<SpecificationVM>> CreateSpecificationAsync(int spuId, string name)
      {
         var result = new Result<SpecificationVM>();
         var specificationVM = new SpecificationVM();

         try
         {
            var existSpec = await this._unitOfWork.GetDbClient().Queryable<Specification>()
                .FirstAsync(x => x.Name == name);


            var existSpu = await this._unitOfWork.GetDbClient().Queryable<StandardProductUnit>()
                .FirstAsync(x => x.Id == spuId);


            if (existSpec == null && existSpu != null)
            {
               this._unitOfWork.BeginTran();

               var specification = new Specification
               {
                  CategoryId = existSpu.CategoryId,
                  Name = name
               };
               await this._unitOfWork.GetDbClient().Updateable<Specification>(specification).ExecuteCommandAsync();


               this._unitOfWork.CommitTran();
               specificationVM = AutoMapperInstance.ConvertModel(specification, specificationVM);
               result.Data = specificationVM;
               result.Success();
            }
         }
         catch (Exception ex)
         {
            this._unitOfWork.RollbackTran();
            NLogUtil.WriteSEQLog($"Error: {ex.Message}, StackTrace: {ex.StackTrace}", NLog.LogLevel.Error);
         }

         return result;
      }


      public async Task<Result<List<StockKeepingUnitSpecificationVM>>> CreateSpecificationOptionAsync(CreateSpecOptionVM createSpecOptionVM)
      {
         var result = new Result<List<StockKeepingUnitSpecificationVM>>();

         try
         {
            var existSpu = await this._unitOfWork.GetDbClient().Queryable<StandardProductUnit>()
                .Where(x => x.Id == createSpecOptionVM.SpuId)
                .Includes(x => x.StockKeepingUnits)
                .FirstAsync();

            var existSpec = await this._unitOfWork.GetDbClient().Queryable<Specification>()
                .Where(x => x.Id == createSpecOptionVM.SpecId)
                .FirstAsync();

            if (existSpu != null && existSpec != null)
            {
               var skuVMs = new List<StockKeepingUnitSpecificationVM>();
               this._unitOfWork.BeginTran();

               foreach (var item in createSpecOptionVM.Options)
               {
                  SpecificationOption specificationOption;

                  if (createSpecOptionVM.IsCreate)
                  {
                     specificationOption = new SpecificationOption
                     {
                        Name = item,
                        SpecificationId = createSpecOptionVM.SpecId
                     };
                     await this._unitOfWork.GetDbClient().Updateable<SpecificationOption>(specificationOption).ExecuteCommandAsync();
                  }
                  else
                  {
                     var specOptionId = Convert.ToInt32(item);
                     specificationOption = await this._unitOfWork.GetDbClient().Queryable<SpecificationOption>()
                         .Where(x => x.Id == specOptionId)
                         .FirstAsync();

                     Guard.Against<Exception>(specificationOption == null, $"SpecificationOption id ({specOptionId}) is not found!");
                  }

                  var relation = await this._unitOfWork.GetDbClient().Queryable<SpecificationOptionRelation>()
                      .Where(x => x.StandardProductUnitId == existSpu.Id &&
                                x.SpecificationId == createSpecOptionVM.SpecId &&
                                x.SpecificationOptionId == specificationOption.Id)
                      .FirstAsync();

                  if (relation == null)
                  {
                     var newRelation = new SpecificationOptionRelation
                     {
                        StandardProductUnitId = existSpu.Id,
                        SpecificationId = createSpecOptionVM.SpecId,
                        SpecificationOptionId = specificationOption.Id,
                        StockKeepingUnitId = createSpecOptionVM.SkuId
                     };
                     await this._unitOfWork.GetDbClient().Updateable<SpecificationOptionRelation>(newRelation).ExecuteCommandAsync();


                     skuVMs.Add(new StockKeepingUnitSpecificationVM
                     {
                        StandardProductUnitId = existSpu.Id,
                        SpecificationId = existSpec.Id,
                        StockKeepingUnitId = createSpecOptionVM.SkuId,
                        SpecificationOptionId = specificationOption.Id,
                        SpecificationOptionName = specificationOption.Name,
                        ShowImage = false
                     });
                  }
               }

               this._unitOfWork.BeginTran();
               result.Data = skuVMs;
               result.Success();
            }
         }
         catch (Exception ex)
         {
            this._unitOfWork.RollbackTran();
            NLogUtil.WriteSEQLog($"Error: {ex.Message}, StackTrace: {ex.StackTrace}", NLog.LogLevel.Error);
         }

         return result;
      }

      public async Task<List<SpecificationVM>> GetSpecificationByCategoryIdAsync(int? categoryId)
      {
         var query = this._unitOfWork.GetDbClient().Queryable<Specification>().OrderBy(y => y.CategoryId);

         if (categoryId != null)
         {
            query = query.Where(x => x.CategoryId == categoryId);
         }

         var dbData = await query.ToListAsync();
         var newData = AutoMapperInstance.ConvertModel<List<Specification>, List<SpecificationVM>>(dbData);
         return newData;
      }


      public async Task<Result<List<SpecificationVM>>> GetSpecificationResultsByCategoryIdAsync(int? categoryId)
      {
         var result = new Result<List<SpecificationVM>>();

         try
         {
            result.Data = await GetSpecificationByCategoryIdAsync(categoryId);
            result.Success();
         }
         catch (Exception ex)
         {
            NLogUtil.WriteSEQLog($"Error: {ex.Message}, StackTrace: {ex.StackTrace}", NLog.LogLevel.Error);
         }

         return result;
      }

      public async Task<List<StockKeepingUnitVM>> GetSkuTreeBySpuIdAsync(int spuId)
      {

         // todo
         //var spu = await this._unitOfWork.GetDbClient().Queryable<StandardProductUnit>()
         //    .Where(x => x.Id == spuId).OrderBy(z => z.Id)
         //    .Includes(x => x.Category)
         //    .Includes(x => x.StockKeepingUnits)
         //    .FirstAsync();

         //if (spu == null) return new List<StockKeepingUnitVM>();

         //var skuIds = spu.StockKeepingUnits.Select(x => x.Id).ToList();

         //var skuSpecificationList = await this._unitOfWork.GetDbClient().Queryable<ViewSkuSpecificationList>()
         //    .Where(x => skuIds.Contains(x.StockKeepingUnitId))
         //    .OrderBy(z => z.CreateDate)
         //    .ToListAsync();

         //var newSKUs = new List<StockKeepingUnitVM>();

         //foreach (var item in spu.StockKeepingUnits)
         //{
         //   var spec = skuSpecificationList.FirstOrDefault(x => x.StockKeepingUnitId == item.Id);

         //   if (spec != null)
         //   {
         //      var newData = AutoMapperInstance.ConvertModel<StockKeepingUnit, StockKeepingUnitVM>(item);
         //      newData.SpecificationId = spec.SpecificationId;

         //      newData.SkuSpecificationList = skuSpecificationList
         //          .Where(x => x.StockKeepingUnitId == item.Id)
         //          .Select(y => new StockKeepingUnitSpecificationVM
         //          {
         //             SpecificationId = y.SpecificationId,
         //             StockKeepingUnitId = y.StockKeepingUnitId,
         //             SpecificationOptionId = y.SpecificationOptionId,
         //             SpecificationOptionName = y.SpecificationOptionName,
         //             ShowImage = y.ShowImage
         //          }).ToList();

         //      newSKUs.Add(newData);
         //   }
         //}

         //return newSKUs;

         return null;
      }

      public async Task<Result<List<StockKeepingUnitVM>>> GetSkuTreeResultBySpuIdAsync(int spuId)
      {
         var result = new Result<List<StockKeepingUnitVM>>();

         try
         {
            result.Data = await GetSkuTreeBySpuIdAsync(spuId);
            result.Success();
         }
         catch (Exception ex)
         {
            NLogUtil.WriteSEQLog($"Error: {ex.Message}, StackTrace: {ex.StackTrace}", NLog.LogLevel.Error);
         }

         return result;
      }


      public async Task<StandardProductUnitVM> GetSpuByIdAsync(int spuid)
      {
         var spu = await this._unitOfWork.GetDbClient().Queryable<StandardProductUnit>()
             .Where(x => x.Id == spuid).OrderBy(z => z.Id)
             .Includes(x => x.Category)
             .Includes(x => x.StandardProductUnitImages)
             .Includes(x => x.StandardProductUnitCarousels) // todo  好像重覆了
             .Includes(x => x.StockKeepingUnits)
             .FirstAsync();

         if (spu == null) return null;

         var newData = AutoMapperInstance.ConvertModel<StandardProductUnit, StandardProductUnitVM>(spu);
         newData.Images = spu.StandardProductUnitImages.Select(x => x.ImagePath).ToList();

         return newData;
      }

      public async Task<Result<StandardProductUnitVM>> GetSpuResultByIdAsync(int id)
      {
         var result = new Result<StandardProductUnitVM>();

         try
         {
            result.Data = await GetSpuByIdAsync(id);
            result.Success();
         }
         catch (Exception ex)
         {
            NLogUtil.WriteSEQLog($"Error: {ex.Message}, StackTrace: {ex.StackTrace}", NLog.LogLevel.Error);
         }

         return result;
      }

      public async Task<List<SpecificationOptionVM>> GetSpecificationOptionByIdAsync(int specId)
      {
         var spec = await this._unitOfWork.GetDbClient().Queryable<SpecificationOption>()
             .Where(x => x.SpecificationId == specId).OrderBy(z => z.Id)
             .ToListAsync();

         var newData = AutoMapperInstance.ConvertModel<List<SpecificationOption>, List<SpecificationOptionVM>>(spec);
         return newData;
      }

      public async Task<Result<List<SpecificationOptionVM>>> GetSpecificationOptionResultByIdAsync(int specId)
      {
         var result = new Result<List<SpecificationOptionVM>>();

         try
         {
            result.Data = await GetSpecificationOptionByIdAsync(specId);
            result.Success();
         }
         catch (Exception ex)
         {
            NLogUtil.WriteSEQLog($"Error: {ex.Message}, StackTrace: {ex.StackTrace}", NLog.LogLevel.Error);
         }

         return result;
      }

      public async Task<Result> DeleteSkuAsync(int skuId)
      {
         var result = new Result();

         try
         {
            // 使用异步方法获取数据
            var existSku = await this._unitOfWork.GetDbClient().Queryable<StockKeepingUnit>()
                .Where(x => x.Id == skuId)
                .Includes(x => x.SpecificationOptionRelations)
                .FirstAsync();

            if (existSku != null)
            {
               foreach (var item in existSku.SpecificationOptionRelations)
               {
                  // 异步删除每个SpecificationOptionRelation
                  this._unitOfWork.GetDbClient().Deleteable(item).ExecuteCommand();
               }

               // 异步删除StockKeepingUnit
               this._unitOfWork.GetDbClient().Deleteable(existSku).ExecuteCommand();


            }

            result.Success();
         }
         catch (Exception ex)
         {
            NLogUtil.WriteSEQLog($"Error: {ex.Message}, StackTrace: {ex.StackTrace}", NLog.LogLevel.Error);
            result.Fail("An error occurred while deleting the SKU."); // 返回错误信息
         }

         return result;
      }


      public async Task<Result> DeleteSkuOptionAsync(int skuId, int specId, int specOptionId)
      {
         var result = new Result();

         try
         {
            // 异步获取数据
            var existSku = await this._unitOfWork.GetDbClient().Queryable<StockKeepingUnit>()
                .Where(x => x.Id == skuId &&
                          x.SpecificationOptionRelations.Any(y =>
                              y.SpecificationOptionId == specOptionId &&
                              y.SpecificationId == specId))
                .Includes(z => z.SpecificationOptionRelations)
                .FirstAsync();

            if (existSku != null)
            {
               var removeList = existSku.SpecificationOptionRelations
                   .Where(y => y.SpecificationOptionId == specOptionId && y.SpecificationId == specId)
                   .ToList();

               foreach (var item in removeList)
               {
                  // 异步删除每个 SpecificationOptionRelation
                  this._unitOfWork.GetDbClient().Deleteable(item).ExecuteCommand();
               }



            }

            result.Success();
         }
         catch (Exception ex)
         {
            NLogUtil.WriteSEQLog($"Error: {ex.Message}, StackTrace: {ex.StackTrace}", NLog.LogLevel.Error);
            result.Fail("An error occurred while deleting the SKU option."); // 返回错误信息
         }

         return result;
      }


      public async Task<Result<StandardProductUnitVM>> CreateSpuAsync(StandardProductUnitVM vm)
      {
         var result = new Result<StandardProductUnitVM>();

         try
         {
            var spu = new StandardProductUnit
            {
               Name = vm.Name,
               Ingredients = vm.Ingredients,
               Description = vm.Description,
               CategoryId = vm.CategoryId,
               Sort = vm.Sort,
               IsHot = vm.IsHot,
               Enable = vm.Enable,
               CoverUrl = vm.CoverUrl
            };

            await this._unitOfWork.GetDbClient().Insertable<StandardProductUnit>(spu).ExecuteCommandAsync();

            result.Data = AutoMapperInstance.ConvertModel(spu, result.Data);
            result.Success();
         }
         catch (Exception ex)
         {
            NLogUtil.WriteSEQLog($"Error: {ex.Message}, StackTrace: {ex.StackTrace}", NLog.LogLevel.Error);
         }

         return result;
      }


      public async Task<Result> UpdateSpuAsync(StandardProductUnitVM vm)
      {
         var result = new Result();

         try
         {
            var spu = await this._unitOfWork.GetDbClient().Queryable<StandardProductUnit>()
                .Where(x => x.Id == vm.Id)
                .FirstAsync();

            if (spu != null)
            {
               spu.Name = vm.Name;
               spu.Ingredients = vm.Ingredients;
               spu.Description = vm.Description;
               spu.CategoryId = vm.CategoryId;
               spu.Sort = vm.Sort;
               spu.IsHot = vm.IsHot;
               spu.Enable = vm.Enable;
               spu.CoverUrl = vm.CoverUrl;

               result.Success();
            }
         }
         catch (Exception ex)
         {
            NLogUtil.WriteSEQLog($"Error: {ex.Message}, StackTrace: {ex.StackTrace}", NLog.LogLevel.Error);
         }

         return result;
      }

      public async Task<Result<List<StockKeepingUnitStockVM>>> GetStockByStandardProductUnitIdAsync(int spuId)
      {
         var result = new Result<List<StockKeepingUnitStockVM>>();

         try
         {
            // 非同步查詢 StandardProductUnit，包含其相關的 StockKeepingUnitStocks
            var existSpu = await this._unitOfWork.GetDbClient().Queryable<StandardProductUnit>()
                .Where(x => x.Id == spuId)
                .Includes(x => x.StockKeepingUnitStocks)
                .FirstAsync();

            if (existSpu != null)
            {
               var stocks = existSpu.StockKeepingUnitStocks;

               // 非同步填充資料到結果
               result.Data = stocks.Select(item => new StockKeepingUnitStockVM
               {
                  Id = item.Id,
                  Skus = string.IsNullOrEmpty(item.Memo)
                       ? new List<SkuStockItem>()
                       : JsonConvert.DeserializeObject<List<SkuStockItem>>(item.Memo),
                  StandardProductUnitId = item.StandardProductUnitId,
                  Price = item.Price,
                  Cost = item.Cost,
                  Stock = item.Stock,
                  CreateDate = item.CreateDate
               }).ToList();
            }

            result.Success();
         }
         catch (Exception ex)
         {
            NLogUtil.WriteSEQLog($"Error: {ex.Message}, StackTrace: {ex.StackTrace}", NLog.LogLevel.Error);
         }

         return result;
      }

      public async Task<Result> UpdateStockAsync(List<StockKeepingUnitStockVM> updateDatas)
      {
         var result = new Result();

         try
         {
            var spuId = updateDatas.FirstOrDefault()?.StandardProductUnitId;

            if (spuId == null)
            {
               throw new ArgumentException("The StandardProductUnitId cannot be null.");
            }

            var existSpu = await this._unitOfWork.GetDbClient().Queryable<StandardProductUnit>()
                .Where(x => x.Id == spuId)
                .Includes(x => x.StockKeepingUnitStocks)
                .FirstAsync();

            if (existSpu != null)
            {
               var stocks = existSpu.StockKeepingUnitStocks;

               // 開始資料庫交易
               this._unitOfWork.BeginTran();

               foreach (var item in updateDatas)
               {
                  // 生成 SKU 字串
                  var skus = item.Skus.Select(w => $"{w.SpecificationId}:{w.SpecificationOptionId}")
                                      .Aggregate((current, next) => $"{current},{next}");
                  var memo = JsonConvert.SerializeObject(item.Skus);

                  var existSkuStock = stocks.FirstOrDefault(x => x.StandardProductUnitId == spuId && x.SKUs == skus);

                  if (existSkuStock == null)
                  {
                     // 插入新的庫存記錄
                     var stockKeepingUnitStock = new StockKeepingUnitStock
                     {
                        SKUs = skus,
                        StandardProductUnitId = spuId.Value,
                        Price = item.Price,
                        Cost = item.Cost,
                        Stock = item.Stock,
                        Memo = memo
                     };

                     await this._unitOfWork.GetDbClient().Updateable(stockKeepingUnitStock).ExecuteCommandAsync();
                  }
                  else
                  {
                     // 更新現有庫存記錄
                     existSkuStock.Price = item.Price;
                     existSkuStock.Cost = item.Cost;
                     existSkuStock.Stock = item.Stock;
                     existSkuStock.Memo = memo;
                  }
               }

               // 提交交易
               this._unitOfWork.CommitTran();
               result.Success();
            }
         }
         catch (Exception ex)
         {
            // 回滾交易
            this._unitOfWork.RollbackTran();
            NLogUtil.WriteSEQLog($"Error: {ex.Message}, StackTrace: {ex.StackTrace}", NLog.LogLevel.Error);
         }

         return result;
      }

      public async Task<Result> UpdateSpuImagesAsync(UpdateSpuImagesVM vm)
      {
         var result = new Result();

         try
         {
            // 非同步查詢 SPU
            var spu = await this._unitOfWork.GetDbClient().Queryable<StandardProductUnit>()
                .Where(x => x.Id == vm.SpuId)
                .FirstAsync();

            // 確保 SPU 存在
            Guard.Against<Exception>(spu == null, "找不到 SPU");

            // 非同步獲取舊的圖片
            var oldImages = await this._unitOfWork.GetDbClient().Queryable<StandardProductUnitImage>()
                .Where(x => x.StandardProductUnitId == vm.SpuId)
                .ToListAsync();

            // 刪除舊圖片
            foreach (var oldImg in oldImages)
            {
               await this._unitOfWork.GetDbClient().Deleteable<StandardProductUnitImage>(x => x.Id == oldImg.Id).ExecuteCommandAsync();
            }

            // 插入新圖片
            foreach (var imgPath in vm.SpuImages)
            {
               var newSpuImg = new StandardProductUnitImage
               {
                  StandardProductUnitId = vm.SpuId,
                  ImagePath = imgPath
               };
               await this._unitOfWork.GetDbClient().Updateable<StandardProductUnitImage>(newSpuImg).ExecuteCommandAsync();
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
