using System;
using System.Collections.Generic;
using System.Text;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;

namespace EC226.Utility.Helper
{
    public static class ExcelHelper
    {
        /// <summary>
        /// 取得頭部樣式
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public static ICellStyle GetHeaderStyle(HSSFWorkbook hs)
        {
            //樣式
            ICellStyle headerStyle = hs.CreateCellStyle();//建立樣式
            headerStyle.VerticalAlignment = VerticalAlignment.Center;//垂直居中 方法1 
            //style.Alignment = HorizontalAlignment.CenterSelection;//設定居中 方法2
            headerStyle.Alignment = HorizontalAlignment.Center;//設定居中 方法3 
            HSSFFont headerfont = (HSSFFont)hs.CreateFont();//建立字型
            headerfont.Color = HSSFColor.Black.Index;//給字型設定顏色
            headerfont.FontName = "細明體";
            headerfont.IsBold = true;
            headerfont.FontHeight = 11;
            headerfont.FontHeightInPoints = 11;
            headerStyle.SetFont(headerfont);

            headerStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;    //下邊框線
            headerStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;      //左邊框線
            headerStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;     //右邊框線
            headerStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;       //上邊框線
            return headerStyle;
        }
        /// <summary>
        /// 取得通用列樣式
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public static ICellStyle GetCommonStyle(HSSFWorkbook hs)
        {
            //樣式
            ICellStyle commonCellStyle = hs.CreateCellStyle();//建立樣式
            commonCellStyle.VerticalAlignment = VerticalAlignment.Center;//垂直居中
            //commonCellStyle.Alignment = HorizontalAlignment.CenterSelection;//設定居中
            commonCellStyle.Alignment = HorizontalAlignment.Center;//設定居中

            HSSFFont commonFont = (HSSFFont)hs.CreateFont();//建立字型
            commonFont.FontName = "細明體";
            commonFont.FontHeight = 10;
            commonFont.FontHeightInPoints = 10;
            commonCellStyle.SetFont(commonFont);

            commonCellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;    //下邊框線
            commonCellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;      //左邊框線
            commonCellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;     //右邊框線
            commonCellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;       //上邊框線
            return commonCellStyle;
        }

    }
}
