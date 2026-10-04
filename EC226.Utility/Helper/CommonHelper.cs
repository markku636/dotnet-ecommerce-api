using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using EC226.Configuration;
using EC226.MemberSite.Model.ViewModels.Basics;

namespace EC226.Utility.Helper
{
    /// <summary>
    /// 通用幫助類
    /// </summary>
    public static class CommonHelper
    {

        #region 判斷字串是否為手機號碼
        /// <summary>
        /// 判斷字串是否為手機號碼
        /// </summary>
        /// <param name="mobilePhoneNumber"></param>
        /// <returns></returns>
        public static bool IsMobile(string mobilePhoneNumber)
        {
            if (mobilePhoneNumber.Length < 11)
            {
                return false;
            }

            //電信手機號碼正則
            string dianxin = @"^1[345789][01379]\d{8}$";
            Regex regexDx = new Regex(dianxin);
            //聯通手機號碼正則
            string liantong = @"^1[345678][01256]\d{8}$";
            Regex regexLt = new Regex(liantong);
            //移動手機號碼正則
            string yidong = @"^1[345789][0123456789]\d{8}$";
            Regex regexYd = new Regex(yidong);
            if (regexDx.IsMatch(mobilePhoneNumber) || regexLt.IsMatch(mobilePhoneNumber) || regexYd.IsMatch(mobilePhoneNumber))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region 檢測是否符合email格式

        /// <summary>
        ///     檢測是否符合email格式
        /// </summary>
        /// <param name="strEmail">要判斷的email字串</param>
        /// <returns>判斷結果</returns>
        public static bool IsValidEmail(string strEmail)
        {
            return Regex.IsMatch(strEmail, @"^[\w\.]+([-]\w+)*@[A-Za-z0-9-_]+[\.][A-Za-z0-9-_]");
        }

        public static bool IsValidDoEmail(string strEmail)
        {
            return Regex.IsMatch(strEmail,
                @"^@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
        }
        #endregion

        #region 檢測是否是正確的Url
        /// <summary>
        ///     檢測是否是正確的Url
        /// </summary>
        /// <param name="strUrl">要驗證的Url</param>
        /// <returns>判斷結果</returns>
        public static bool IsUrl(string strUrl)
        {
            return Regex.IsMatch(strUrl,
                @"^(http|https)\://([a-zA-Z0-9\.\-]+(\:[a-zA-Z0-9\.&%\$\-]+)*@)*((25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9])\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[0-9])|localhost|([a-zA-Z0-9\-]+\.)*[a-zA-Z0-9\-]+\.(com|edu|gov|int|mil|net|org|biz|arpa|info|name|pro|aero|coop|museum|[a-zA-Z]{1,10}))(\:[0-9]+)*(/($|[a-zA-Z0-9\.\,\?\'\\\+&%\$#\=~_\-]+))*$");
        }


        #endregion

        #region string 轉int陣列

        public static int[] StringToIntArray(string str)
        {
            try
            {
                if (string.IsNullOrEmpty(str)) return new int[0];
                if (str.EndsWith(","))
                {
                    str = str.Remove(str.Length - 1, 1);
                }
                var idstrarr = str.Split(',');
                var idintarr = new int[idstrarr.Length];

                for (int i = 0; i < idstrarr.Length; i++)
                {
                    idintarr[i] = Convert.ToInt32(idstrarr[i]);
                }
                return idintarr;
            }
            catch
            {
                return new int[0];
            }
        }
        #endregion

        #region String轉陣列
        public static string[] StringToStringArray(string str)
        {
            try
            {
                if (string.IsNullOrEmpty(str)) return new string[0];
                if (str.EndsWith(",")) str = str.Remove(str.Length - 1, 1);
                return str.Split(',');
            }
            catch
            {
                return new string[0];
            }
        }
        #endregion

        #region String陣列轉Int陣列
        public static int[] StringArrAyToIntArray(string[] str)
        {
            try
            {
                int[] iNums = Array.ConvertAll<string, int>(str, s => int.Parse(s));
                return iNums;
            }
            catch
            {
                return new int[0];
            }
        }
        #endregion

        #region string轉Guid陣列
        public static System.Guid[] StringToGuidArray(string str)
        {
            try
            {
                if (string.IsNullOrEmpty(str)) return new System.Guid[0];
                if (str.EndsWith(",")) str = str.Remove(str.Length - 1, 1);
                var strarr = str.Split(',');
                System.Guid[] guids = new System.Guid[strarr.Length];
                for (int index = 0; index < strarr.Length; index++)
                {
                    guids[index] = System.Guid.Parse(strarr[index]);
                }
                return guids;
            }
            catch
            {
                return new System.Guid[0];
            }
        }
        #endregion

        #region 取得32位md5加密
        /// <summary>
        /// 通過建立雜湊字串適用於任何 MD5 雜湊函式 （在任何平臺） 上建立 32 個字元的十六進制格式雜湊字串
        /// </summary>
        /// <param name="source"></param>
        /// <returns>32位md5加密字串</returns>
        public static string Md5For32(string source)
        {
            using (MD5 md5Hash = MD5.Create())
            {
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(source));
                StringBuilder sBuilder = new StringBuilder();
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }

                string hash = sBuilder.ToString();
                return hash.ToUpper();
            }
        }
        #endregion

        #region 取得16位md5加密
        /// <summary>
        /// 取得16位md5加密
        /// </summary>
        /// <param name="source"></param>
        /// <returns>16位md5加密字串</returns>
        public static string Md5For16(string source)
        {
            using (MD5 md5Hash = MD5.Create())
            {
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(source));
                //轉換成字串，並取9到25位
                string sBuilder = BitConverter.ToString(data, 4, 8);
                //BitConverter轉換出來的字串會在每個字元中間產生一個分隔符，需要去除掉
                sBuilder = sBuilder.Replace("-", "");
                return sBuilder.ToUpper();
            }
        }

        #endregion

        #region 返回目前的毫秒時間戳

        /// <summary>
        /// 返回目前的毫秒時間戳
        /// </summary>
        public static string Msectime()
        {
            long timeTicks = (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000;
            return timeTicks.ToString();
        }


        #endregion

        #region 取得多種數據編號
        /// <summary>
        /// 取得多種數據編號
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetSerialNumberType(int type)
        {
            var str = string.Empty;
            Random rand = new Random();
            switch (type)
            {
                case (int)GlobalEnumVars.SerialNumberType.訂單編號:         //訂單編號
                    str = type + Msectime() + rand.Next(0, 9);
                    break;
                case (int)GlobalEnumVars.SerialNumberType.支付單編號:         //支付單編號
                    str = type + Msectime() + rand.Next(0, 9);
                    break;
                case (int)GlobalEnumVars.SerialNumberType.商品編號:         //商品編號
                    str = 'G' + Msectime() + rand.Next(0, 5);
                    break;
                case (int)GlobalEnumVars.SerialNumberType.貨品編號:         //貨品編號
                    str = 'P' + Msectime() + rand.Next(0, 5);
                    break;
                case (int)GlobalEnumVars.SerialNumberType.售後單編號:         //售後單編號
                    str = type + Msectime() + rand.Next(0, 9);
                    break;
                case (int)GlobalEnumVars.SerialNumberType.退款單編號:         //退款單編號
                    str = type + Msectime() + rand.Next(0, 9);
                    break;
                case (int)GlobalEnumVars.SerialNumberType.退貨單編號:         //退貨單編號
                    str = type + Msectime() + rand.Next(0, 9);
                    break;
                case (int)GlobalEnumVars.SerialNumberType.發貨單編號:         //發貨單編號
                    str = type + Msectime() + rand.Next(0, 9);
                    break;
                case (int)GlobalEnumVars.SerialNumberType.服務訂單編號:         //服務訂單編號
                    str = type + Msectime() + rand.Next(0, 9);
                    break;
                case (int)GlobalEnumVars.SerialNumberType.提貨單號:         //提貨單號
                    //str = 'T' + type + msectime() + rand.Next(0, 5);
                    var charsStr = new[] { 'Q', 'W', 'E', 'R', 'T', 'Y', 'U', 'P', 'A', 'S', 'D', 'F', 'G', 'H', 'J', 'K', 'L', 'Z', 'X', 'C', 'V', 'B', 'N', 'M', '2', '3', '4', '5', '6', '7', '8', '9' };
                    var charsLen = charsStr.Length - 1;
                    //    shuffle($chars);
                    str = "";
                    for (int i = 0; i < 6; i++)
                    {
                        str += charsStr[rand.Next(0, charsLen)];
                    }
                    break;
                case (int)GlobalEnumVars.SerialNumberType.服務券兌換碼:         //服務券兌換碼
                    var charsStr2 = new[] { 'Q', 'W', 'E', 'R', 'T', 'Y', 'U', 'P', 'A', 'S', 'D', 'F', 'G', 'H', 'J', 'K', 'L', 'Z', 'X', 'C', 'V', 'B', 'N', 'M', '2', '3', '4', '5', '6', '7', '8', '9' };
                    var charsLen2 = charsStr2.Length - 1;
                    //    shuffle($chars);
                    str = "";
                    for (int i = 0; i < 6; i++)
                    {
                        str += charsStr2[rand.Next(0, charsLen2)];
                    }
                    break;
                default:
                    str = 'T' + Msectime() + rand.Next(0, 9);
                    break;
            }
            return str;
        }

        #endregion

        #region 剩餘多久時間文字描述
        /// <summary>
        /// 剩餘多久時間
        /// </summary>
        /// <param name="remainingTime"></param>
        /// <returns>文字描述</returns>
        public static string GetRemainingTime(DateTime remainingTime)
        {
            TimeSpan timeSpan = remainingTime - DateTime.Now;
            var day = timeSpan.Days;
            var hours = timeSpan.Hours;
            var minute = timeSpan.Minutes;
            var seconds = timeSpan.Seconds;
            if (day > 0)
            {
                return day + "天" + hours + "小時" + minute + "分" + seconds + "秒";
            }
            else
            {
                if (hours > 0)
                {
                    return hours + "小時" + minute + "分" + seconds + "秒";
                }
                else
                {
                    return minute + "分" + seconds + "秒";
                }
            }
        }

        #endregion

        #region 剩餘多久時間返回時間型別
        /// <summary>
        /// 剩餘多久時間
        /// </summary>
        /// <param name="remainingTime"></param>
        /// <returns>返回時間型別</returns>
        public static void GetBackTime(DateTime remainingTime, out int day, out int hours, out int minute, out int seconds)
        {
            TimeSpan timeSpan = remainingTime - DateTime.Now;
            day = timeSpan.Days;
            hours = timeSpan.Hours;
            minute = timeSpan.Minutes;
            seconds = timeSpan.Seconds;
        }

        #endregion

        #region 計算時間戳剩餘多久時間

        /// <summary>
        /// 計算時間戳剩餘多久時間
        /// </summary>
        /// <param name="postTime">提交時間(要是以前的時間)</param>
        /// <returns></returns>
        public static string TimeAgo(DateTime postTime)
        {
            //目前時間的時間戳
            var nowtimes = ConvertTicks(DateTime.Now);
            //提交的時間戳
            var posttimes = ConvertTicks(postTime);
            //相差時間戳
            var counttime = nowtimes - posttimes;

            //進行時間轉換
            if (counttime <= 60)
            {
                return "剛剛";
            }
            else if (counttime > 60 && counttime <= 120)
            {
                return "1分鐘前";
            }
            else if (counttime > 120 && counttime <= 180)
            {
                return "2分鐘前";
            }
            else if (counttime > 180 && counttime < 3600)
            {
                return Convert.ToInt32((counttime / 60)) + "分鐘前";
            }
            else if (counttime >= 3600 && counttime < 3600 * 24)
            {
                return Convert.ToInt32((counttime / 3600)) + "小時前";
            }
            else if (counttime >= 3600 * 24 && counttime < 3600 * 24 * 2)
            {
                return "昨天";
            }
            else if (counttime >= 3600 * 24 * 2 && counttime < 3600 * 24 * 3)
            {
                return "前天";
            }
            else if (counttime >= 3600 * 24 * 3 && counttime <= 3600 * 24 * 7)
            {
                return Convert.ToInt32((counttime / (3600 * 24))) + "天前";
            }
            else if (counttime >= 3600 * 24 * 7 && counttime <= 3600 * 24 * 30)
            {
                return Convert.ToInt32((counttime / (3600 * 24 * 7))) + "周前";
            }
            else if (counttime >= 3600 * 24 * 30 && counttime <= 3600 * 24 * 365)
            {
                return Convert.ToInt32((counttime / (3600 * 24 * 30))) + "個月前";
            }
            else if (counttime >= 3600 * 24 * 365)
            {
                return Convert.ToInt32((counttime / (3600 * 24 * 365))) + "年前";
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 時間轉換為秒的時間戳
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        private static long ConvertTicks(DateTime time)
        {
            long currentTicks = time.Ticks;
            DateTime dtFrom = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            long currentMillis = (currentTicks - dtFrom.Ticks) / 10000000;  //轉換為秒為Ticks/10000000，轉換為毫秒Ticks/10000
            return currentMillis;
        }

        #endregion

        #region 清除HTML中指定樣式
        /// <summary>
        /// 清除HTML中指定樣式
        /// </summary>
        /// <param name="content"></param>
        /// <param name="rule"></param>
        /// <returns></returns>
        public static string ClearHtml(string content, string[] rule)
        {
            if (!rule.Any())
            {
                return content;
            }

            foreach (var item in rule)
            {
                content = Regex.Replace(content, "/" + item + @"\s*=\s*\d+\s*/i", "");
                content = Regex.Replace(content, "/" + item + @"\s*=\s*.+?[""]/i", "");
                content = Regex.Replace(content, "/" + item + @"\s*:\s*\d+\s*px\s*;?/i", "");
            }
            return content;
        }
        #endregion

        #region list隨機排序方法
        /// <summary>
        /// list隨機排序方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ListT"></param>
        /// <returns></returns>
        public static List<T> RandomSortList<T>(List<T> ListT)
        {
            Random random = new Random();
            List<T> newList = new List<T>();
            foreach (T item in ListT)
            {
                newList.Insert(random.Next(newList.Count + 1), item);
            }
            return newList;
        }
        #endregion

        #region 從字典中取單個數據
        /// <summary>
        /// 從字典中取單個數據
        /// </summary>
        /// <param name="configs"></param>
        /// <param name="skey"></param>
        /// <returns></returns>
        public static string GetConfigDictionary(Dictionary<string, DictionaryKeyValues> configs, string skey)
        {
            configs.TryGetValue(skey, out var di);
            return di?.sValue;
        }

        #endregion

        #region 截前後字元(串)
        ///<summary>
        /// 截前後字元(串)
        ///</summary>
        ///<param name="val">原字串</param>
        ///<param name="str">要截掉的字串</param>
        ///<param name="all">是否貪婪</param>
        ///<returns></returns>
        public static string GetCaptureInterceptedText(string val, string str, bool all = false)
        {
            return Regex.Replace(val, @"(^(" + str + ")" + (all ? "*" : "") + "|(" + str + ")" + (all ? "*" : "") + "$)", "");
        }
        #endregion

        #region 取得現在是星期幾
        /// <summary>
        /// 取得現在是星期幾
        /// </summary>
        /// <returns></returns>
        public static string GetWeek()
        {
            string week = string.Empty;
            switch (DateTime.Now.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    week = "週一";
                    break;
                case DayOfWeek.Tuesday:
                    week = "週二";
                    break;
                case DayOfWeek.Wednesday:
                    week = "週三";
                    break;
                case DayOfWeek.Thursday:
                    week = "週四";
                    break;
                case DayOfWeek.Friday:
                    week = "週五";
                    break;
                case DayOfWeek.Saturday:
                    week = "週六";
                    break;
                case DayOfWeek.Sunday:
                    week = "週日";
                    break;
                default:
                    week = "N/A";
                    break;
            }
            return week;
        }

        #endregion

        #region UrlEncode (URL編碼)
        /// <summary>
        /// UrlEncode (URL編碼)
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string UrlEncode(string str)
        {
            StringBuilder sb = new StringBuilder();
            byte[] byStr = System.Text.Encoding.UTF8.GetBytes(str); //預設是System.Text.Encoding.Default.GetBytes(str)
            for (int i = 0; i < byStr.Length; i++)
            {
                sb.Append(@"%" + Convert.ToString(byStr[i], 16));
            }

            return (sb.ToString());
        }

        #endregion

        #region 取得10位時間戳
        /// <summary>
        /// 取得10位時間戳
        /// </summary>
        /// <returns></returns>
        public static long GetTimeStampByTotalSeconds()
        {
            TimeSpan ts = DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds);
        }
        #endregion

        #region 取得13位時間戳
        /// <summary>
        /// 取得13位時間戳
        /// </summary>
        /// <returns></returns>
        public static long GetTimeStampByTotalMilliseconds()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalMilliseconds);
        }
        #endregion


        /// <summary>
        ///  精確計算base64字串檔案大小（單位：B）
        ///  param base64String
        ///  return double 位元組大小
        /// </summary>
        /// <param name="base64String"></param>
        /// <returns></returns>
        public static double Base64FileSize(String base64String)
        {
            //檢測是否含有base64,檔案頭)
            if (base64String.LastIndexOf(",", StringComparison.Ordinal) > -1)
            {
                base64String = base64String[(base64String.LastIndexOf(",", StringComparison.Ordinal) + 1)..];
            }
            //取得base64字串長度(不含data:audio/wav;base64,檔案頭)
            var size0 = base64String.Length;
            if (size0 <= 10) return size0 - ((double)size0 / 8) * 2;
            //取得字串的尾巴的最後10個字元，用於判斷尾巴是否有等號，正常產生的base64檔案'等號'不會超過4個
            var tail = base64String[(size0 - 10)..];
            //找到等號，把等號也去掉,(等號其實是空的意思,不能算在檔案大小裡面)
            int equalIndex = tail.IndexOf("=", StringComparison.Ordinal);
            if (equalIndex > 0)
            {
                size0 = size0 - (10 - equalIndex);
            }
            //計算后得到的檔案流大小，單位為位元組
            return size0 - ((double)size0 / 8) * 2;
        }

        /// <summary>
        /// 判斷檔案大小
        /// </summary>
        /// <param name="base64"></param>
        /// <param name="size"></param>
        /// <param name="unit"></param>
        /// <returns></returns>
        public static bool CheckBase64Size(string base64, int size, string unit = "M")
        {
            // 上傳檔案的大小, 單位為位元組.
            var len = Base64FileSize(base64);
            // 準備接收換算後文件大小的容器
            double fileSize = unit.ToUpperInvariant() switch
            {
                "B" => len,
                "K" => (double)len / 1024,
                "M" => (double)len / 1048576,
                "G" => (double)len / 1073741824,
                _ => 0
            };
            // 如果上傳檔案大於限定的容量
            return !(fileSize > size);
        }


        /// <summary>
        /// 10位時間戳 轉化
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static long ConvertDateTimeToInt(System.DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0));
            long t = (time.Ticks - startTime.Ticks) / 10000000;   //除10000000調整為10位    
            return t;
        }

    }
}
