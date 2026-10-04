using System.Collections.Generic;
using EC226.MemberSite.IRepository;
using EC226.MemberSite.IServices;
using SqlSugar;

namespace EC226.Services.CodeGenerator
{
	public class CodeGeneratorServices: ICodeGeneratorServices
	{
		ICodeGeneratorRepository _dal;

		public CodeGeneratorServices(ICodeGeneratorRepository dal) {
			this._dal = dal;
		}


		/// <summary>
		/// 取得所有的表
		/// </summary>
		/// <returns></returns>
		public List<DbTableInfo> GetDbTables() {
			return _dal.GetDbTables();
		}

		/// <summary>
		/// 取得表下面所有的字段
		/// </summary>
		/// <param name="tableName"></param>
		/// <returns></returns>
		public List<DbColumnInfo> GetDbTablesColumns(string tableName) {
			return _dal.GetDbTablesColumns(tableName);
		}

		/// <summary>
		/// 自動資料庫代碼
		/// </summary>
		/// <param name="tableName"></param>
		/// <param name="fileType"></param>
		/// <returns></returns>
		public byte[] CodeGen(string tableName, string fileType) {
			return _dal.CodeGen(tableName, fileType);
		}

		/// <summary>
		/// 自動資料庫類型的所有資料庫代碼
		/// </summary>
		/// <param name="tableName"></param>
		/// <param name="fileType"></param>
		/// <returns></returns>
		public byte[] CodeGenByAll(string fileType) {
			return _dal.CodeGenByAll(fileType);
		}
	}
}
