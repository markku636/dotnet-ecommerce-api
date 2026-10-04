
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using EC226.Loging;
using EC226.MemberSite.IRepository;
using EC226.MemberSite.IRepository.UnitOfWork;
using EC226.MemberSite.IServices;
using EC226.MemberSite.Model.CodeGenModels;
using EC226.MemberSite.Model.Entities;
using EC226.MemberSite.Model.Enums;
using EC226.MemberSite.Model.ViewModels;
using EC226.MemberSite.Model.ViewModels.UI;
using EC226.Pagination;
using EC226.Payment;
using EC226.Repository.UnitOfWork;
using EC226.Utility.Helper;
using EC226.Utility.Library.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NPOI.SS.Formula.Functions;
using SqlSugar;


namespace EC226.MemberSite.Services
{
   public class Collections: ICollectionService
   {
      private readonly IComponentContext _context;

      private readonly IUnitOfWork _unitOfWork;
      public Collections(IUnitOfWork unitOfWork, IComponentContext context)
      {
         _unitOfWork = unitOfWork;
         this._context = context;
      }

      public Result<CollectionSchemaViewModel> GetCollectionSchema(string functionKey)
      {
         Result<CollectionSchemaViewModel> result = new Result<CollectionSchemaViewModel>();
         try
         {
            List<int> tableIds = new List<int>();

            var schema = _unitOfWork.GetDbClient().Queryable<CollectionSchema>().Where(x => x.FunctionKey == functionKey)
               .Includes(x => x.CollectionForeignDataSources)
               .First();

            if (schema != null)
            {
               result.Data.CollectionSchema = schema;
               tableIds.Add(schema.MainTableId);

               tableIds.AddRange(schema.CollectionForeignDataSources.Select(x => x.TableId).ToList());

               
               var tables = _unitOfWork.GetDbClient().Queryable<CodeTable>().Where(x => tableIds.Contains(x.Id)).ToList();
               var columns = _unitOfWork.GetDbClient().Queryable<CodeColumns>().Where(x => tableIds.Contains(x.CodeTableId)).ToList();


               result.Data.Tables = tables;
               result.Data.CodeColumns = columns;
            }
            result.Success();
         }
         catch (Exception ex)
         {
         }

         return result;
      }

      public Result SaveData(SaveDataRequest request)
      {
         var result = new Result();

         if (request == null || string.IsNullOrEmpty(request.FunctionName) || request.DataSource == null)
         {
            return result;
         }

         try
         {
            var schema = _unitOfWork.GetDbClient().Queryable<CollectionSchema>().Where(x => x.FunctionKey == request.FunctionKey).First();


            if (schema == null)
            {
               _unitOfWork.BeginTran();
               // 儲存主表資料
               var mainData = new CollectionSchema
               {
                  FunctionName = request.FunctionName,
                  FunctionKey = request.FunctionKey,
                  Sort = int.TryParse(request.Sort, out var sort) ? sort : 0,
                  DatabaseId = request.Database,
                  MainTableId = request.MainTable,
                  FormSchema = string.Empty,
                  CreatedBy = "System",
                  ModifiedBy = "System" // 初次創建時，修改者與創建者相同
               };

               var mainId = _unitOfWork.GetDbClient().Insertable(mainData).ExecuteReturnIdentity();

               // 儲存外鍵表資料
               foreach (var data in request.DataSource)
               {
                  var foreignKeyData = new CollectionForeignDataSource
                  {
                     TableId = mainId, // 使用插入主表的自增主鍵
                     ForeignKeyId = data.ForeignKeyId,
                     RelatedKeyId = data.RelatedKeyId,
                     RelatedType = data.RelatedType,
                     SchemaId = mainId
                  };

                  _unitOfWork.GetDbClient().Insertable(foreignKeyData).ExecuteCommand();
               }
               _unitOfWork.CommitTran();
               result.Success();
               result.Message = "Data saved successfully.";
            }
         }
         catch (Exception ex)
         {
            _unitOfWork.RollbackTran();
            result.Message = $"Error saving data: {ex.Message}";
         }

         return result;
      }





   }
}
