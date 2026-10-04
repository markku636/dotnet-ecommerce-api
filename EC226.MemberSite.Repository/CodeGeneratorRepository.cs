using System.Collections.Generic;
using System.Linq;
using EC226.CodeGenerator;
using EC226.MemberSite.IRepository;
using EC226.MemberSite.IRepository.UnitOfWork;
using EC226.MemberSite.Model.ViewModels.Basics;
using EC226.MemberSite.Repository;
using SqlSugar;

namespace EC226.Repository.CodeGenerator
{
    public class CodeGeneratorRepository : BaseRepository<object>, ICodeGeneratorRepository
    {
        public CodeGeneratorRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        /// <summary>
        ///     獲取所有的表
        /// </summary>
        /// <returns></returns>
        public List<DbTableInfo> GetDbTables()
        {
            var tables = DbClient.DbMaintenance.GetTableInfoList(false).OrderBy(p => p.Name).ToList();
            var views = DbClient.DbMaintenance.GetViewInfoList(false).OrderBy(p => p.Name).ToList();
            if (!views.Any()) return tables;
            var newList = tables.Union(views).ToList();
            return newList;
        }

        /// <summary>
        ///     獲取表下面所有的字段
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public List<DbColumnInfo> GetDbTablesColumns(string tableName)
        {
            var columns = DbClient.DbMaintenance.GetColumnInfosByTableName(tableName, false);
            return columns;
        }


        /// <summary>
        ///     自動資料庫代碼
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="fileType"></param>
        /// <returns></returns>
        public byte[] CodeGen(string tableName, string fileType)
        {
            var tables = DbClient.DbMaintenance.GetTableInfoList(false);
            var views = DbClient.DbMaintenance.GetViewInfoList(false);
            var tb = tables.Find(p => p.Name == tableName) ?? views.Find(p => p.Name == tableName);
            if (tb == null)
            {
                return null;
            }

            var columns = DbClient.DbMaintenance.GetColumnInfosByTableName(tb.Name, false);
            return CodeGeneratorHelper.CodeGenerator(tb.Name, tb.Description, columns, fileType);
        }


        /// <summary>
        ///  自動資料庫類型的所有資料庫代碼
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="fileType"></param>
        /// <returns></returns>
        public byte[] CodeGenByAll(string fileType)
        {
            var tables = DbClient.DbMaintenance.GetTableInfoList(false);
            var views = DbClient.DbMaintenance.GetViewInfoList(false);
            var newList = tables;

            List<string> bypassTableList = new List<string>() { "ExportIndex","SerialKey", "dtproperties", "sysdiagrams", "dtproperties" };



            if (views.Any()) newList = tables.Where(x => !bypassTableList.Contains(x.Name)).Union(views).ToList();

            var allDb = new List<DbTableInfoAndColumns>();
            newList.ForEach(p =>
            {
                var model = new DbTableInfoAndColumns();
                model.Name = p.Name;
                model.DbObjectType = p.DbObjectType;
                model.Description = p.Description;
                model.columns = DbClient.DbMaintenance.GetColumnInfosByTableName(p.Name, false);
                allDb.Add(model);
            });

            if (!allDb.Any())
                return null;
            return CodeGeneratorHelper.CodeGeneratorAll(allDb, fileType);
        }
    }

}