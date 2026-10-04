using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EC226.Configuration;
using EC226.MemberSite.IServices;
using EC226.Loging;
using EC226.MemberSite.Model.ViewModels.UI;
using EC226.Utility.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NPOI.HSSF.UserModel;

namespace EC226.MemberSite.Api.Controllers
{
    /// <summary>
    /// 代碼資料庫器
    /// </summary>
    [Description("代碼資料庫器")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    //[Authorize(Permissions.Name)]
    public class CodeGeneratorController : ControllerBase
    {
        private readonly ICodeGeneratorServices _codeGeneratorServices;
        private readonly IWebHostEnvironment _webHostEnvironment;

        private readonly IMapper _mapper;

        /// <summary>
        /// 構造函數
        /// </summary>
        public CodeGeneratorController(ICodeGeneratorServices codeGeneratorServices, IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            _codeGeneratorServices = codeGeneratorServices;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
        }


        /// <summary>
        /// 取得資料表列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("取得資料表列表")]
        public AdminUiCallBack GetTables()
        {
            var tables = _codeGeneratorServices.GetDbTables();

            var fristDb = new SqlSugar.DbTableInfo { Name = "AllTable", Description = "所有數據表含視圖" };

            tables.Insert(0, fristDb);

            var jm = new AdminUiCallBack
            {
                code = 0,
                data = _mapper
                      .Map<List<SqlSugar.DbTableInfo>, List<Model.ViewModels.Basics.DbTableInfoTree>>(tables)
            };
            return jm;
        }

        /// <summary>
        /// 執行程式產生器
        /// </summary>
        /// <param name="tableName">AllTable</param>
        /// <param name="fileType">AllFiles、EntityFiles、ServicesFiles、InterFaceFiles</param>
        /// <returns></returns>
        [HttpPost]
        [Description("執行程式產生器")]
        public ActionResult CodeGen([FromQuery] string tableName = "Mem_Main", [FromQuery] string fileType = "EntityFiles")
        {
            var jm = new AdminUiCallBack();
            if (string.IsNullOrEmpty(tableName))
            {
                jm.msg = "請選擇資料庫";
                return new JsonResult(jm);
            }
            if (string.IsNullOrEmpty(fileType))
            {
                jm.msg = "請選擇要資料庫的文件類型";
                return new JsonResult(jm);
            }

            if (tableName == "AllTable")
            {
                var data = _codeGeneratorServices.CodeGenByAll(fileType);
                if (data != null)
                {
                    return File(data, System.Net.Mime.MediaTypeNames.Application.Zip, tableName + "-" + fileType + ".zip");
                }
                else
                {
                    jm.msg = tableName + "取得資料庫字段失敗";
                    return new JsonResult(jm);
                }
            }
            else
            {
                var data = _codeGeneratorServices.CodeGen(tableName, fileType);
                if (data != null)
                {
                    return File(data, System.Net.Mime.MediaTypeNames.Application.Zip, tableName + "-" + fileType + ".zip");
                }
                else
                {
                    jm.msg = tableName + "取得資料庫字段失敗";
                    return new JsonResult(jm);
                }
            }

        }


        #region 取得所有表和表名並資料庫excel

        /// <summary>
        /// 取得所有表和表名並資料庫excel
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("取得所有表和表名並資料庫excel")]
        public AdminUiCallBack GetDataBaseTablesToExcel()
        {
            var jm = new AdminUiCallBack();

            //創建Excel文件的對象
            var book = new HSSFWorkbook();
            //添加一個sheet
            var mySheet = book.CreateSheet("Sheet1");
            //取得list數據
            var listmodel = _codeGeneratorServices.GetDbTables();

            var headerStyle = ExcelHelper.GetHeaderStyle(book);
            var commonCellStyle = ExcelHelper.GetCommonStyle(book);


            //給sheet1添加第一行的頭部標題
            var headerRow = mySheet.CreateRow(0);

            var cell0 = headerRow.CreateCell(0); cell0.SetCellValue("序列"); cell0.CellStyle = headerStyle;
            mySheet.SetColumnWidth(0, 10 * 256);

            var cell1 = headerRow.CreateCell(1); cell1.SetCellValue("表名"); cell1.CellStyle = headerStyle;
            mySheet.SetColumnWidth(1, 50 * 256);

            var cell2 = headerRow.CreateCell(2); cell2.SetCellValue("描述"); cell2.CellStyle = headerStyle;
            mySheet.SetColumnWidth(2, 50 * 256);

            //將數據逐步寫入sheet1各個行
            for (var i = 0; i < listmodel.Count; i++)
            {
                var rowtemp = mySheet.CreateRow(i + 1);

                var rowtemp01 = rowtemp.CreateCell(0);
                rowtemp01.SetCellValue(i);
                rowtemp01.CellStyle = commonCellStyle;

                var rowtemp02 = rowtemp.CreateCell(1);
                rowtemp02.SetCellValue(listmodel[i].Name);
                rowtemp02.CellStyle = commonCellStyle;

                var rowtemp03 = rowtemp.CreateCell(2);
                rowtemp03.SetCellValue(listmodel[i].Description);
                rowtemp03.CellStyle = commonCellStyle;
            }
            // 資料庫excel
            string webRootPath = _webHostEnvironment.WebRootPath;
            string tpath = "/files/" + DateTime.Now.ToString("yyyy-MM-dd") + "/";
            string fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "-資料庫表資料庫.xls";
            string filePath = webRootPath + tpath;
            DirectoryInfo di = new DirectoryInfo(filePath);
            if (!di.Exists)
            {
                di.Create();
            }
            FileStream fileHssf = new FileStream(filePath + fileName, FileMode.Create);
            book.Write(fileHssf);
            fileHssf.Close();

            jm.code = 0;
            jm.msg = GlobalConstVars.ExcelExportSuccess;
            jm.data = tpath + fileName;

            return jm;
        }

        #endregion


    }
}