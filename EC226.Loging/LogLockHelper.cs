using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using EC226.MemberSite.Model.ViewModels.LogInfo;
using EC226.Utility.Extensions;
using Newtonsoft.Json;

namespace EC226.Loging
{
    /// <summary>
    /// 日誌鎖
    /// </summary>
    public class LogLockHelper
    {

        static ReaderWriterLockSlim LogWriteLock = new ReaderWriterLockSlim();
        static int WritedCount = 0;
        static int FailedCount = 0;
        static string _contentRoot = string.Empty;

        public LogLockHelper(string contentPath)
        {
            _contentRoot = contentPath;
        }

        public static void OutSql2Log(string rootPath, string filename, string[] dataParas, bool IsHeader = true)
        {
            try
            {
                //設定讀寫鎖為寫入模式獨佔資源，其他寫入請求需要等待本次寫入結束之後才能繼續寫入
                //注意：長時間持有讀執行緒鎖或寫執行緒鎖會使其他執行緒發生飢餓 (starve)。 爲了得到最好的效能，需要考慮重新構造應用程式以將寫訪問的持續時間減少到最小。
                //      從效能方面考慮，請求進入寫入模式應該緊跟檔案操作之前，在此處進入寫入模式僅是爲了降低程式碼複雜度
                //      因進入與退出寫入模式應在同一個try finally語句塊內，所以在請求進入寫入模式之前不能觸發異常，否則釋放次數大於請求次數將會觸發異常
                LogWriteLock.EnterWriteLock();

                var path = Path.Combine(_contentRoot, "App_Data/" + rootPath);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string logFilePath = Path.Combine(path, $@"{filename}.log");

                var now = DateTime.Now;
                string logContent = String.Join("\r\n", dataParas);
                if (IsHeader)
                {
                    logContent = (
                       "--------------------------------\r\n" +
                       DateTime.Now + "|\r\n" +
                       String.Join("\r\n", dataParas) + "\r\n"
                       );
                }

                File.AppendAllText(logFilePath, logContent);
                WritedCount++;
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
                FailedCount++;
            }
            finally
            {
                //退出寫入模式，釋放資源佔用
                //注意：一次請求對應一次釋放
                //      若釋放次數大於請求次數將會觸發異常[寫入鎖定未經保持即被釋放]
                //      若請求處理完成後未釋放將會觸發異常[此模式不下允許以遞迴方式取得寫入鎖定]
                LogWriteLock.ExitWriteLock();
            }
        }

        public static string ReadLog(string Path, Encoding encode)
        {
            string s = "";
            try
            {
                LogWriteLock.EnterReadLock();

                if (!System.IO.File.Exists(Path))
                {
                    s = null;
                }
                else
                {
                    StreamReader f2 = new StreamReader(Path, encode);
                    s = f2.ReadToEnd();
                    f2.Close();
                    f2.Dispose();
                }
            }
            catch (Exception)
            {
                FailedCount++;
            }
            finally
            {
                LogWriteLock.ExitReadLock();
            }
            return s;
        }


        public static List<LogInfo> GetLogData()
        {
            List<LogInfo> aopLogs = new List<LogInfo>();
            List<LogInfo> excLogs = new List<LogInfo>();
            List<LogInfo> sqlLogs = new List<LogInfo>();
            List<LogInfo> reqresLogs = new List<LogInfo>();

            try
            {
                var aoplogContent = ReadLog(Path.Combine(_contentRoot, "Logs", "AOPLog.log"), Encoding.UTF8);

                if (!string.IsNullOrEmpty(aoplogContent))
                {
                    aopLogs = aoplogContent.Split("--------------------------------")
                 .Where(d => !string.IsNullOrEmpty(d) && d != "\n" && d != "\r\n")
                 .Select(d => new LogInfo
                 {
                     Datetime = d.Split("|")[0].ObjectToDate(),
                     Content = d.Split("|")[1]?.Replace("\r\n", "<br>"),
                     LogColor = "AOP",
                 }).ToList();
                }
            }
            catch (Exception) { }

            try
            {
                var exclogContent = ReadLog(Path.Combine(_contentRoot, "Logs", $"GlobalExcepLogs_{DateTime.Now.ToString("yyyMMdd")}.log"), Encoding.UTF8);

                if (!string.IsNullOrEmpty(exclogContent))
                {
                    excLogs = exclogContent.Split("--------------------------------")
                                 .Where(d => !string.IsNullOrEmpty(d) && d != "\n" && d != "\r\n")
                                 .Select(d => new LogInfo
                                 {
                                     Datetime = (d.Split("|")[0]).Split(',')[0].ObjectToDate(),
                                     Content = d.Split("|")[1]?.Replace("\r\n", "<br>"),
                                     LogColor = "EXC",
                                     Import = 9,
                                 }).ToList();
                }
            }
            catch (Exception) { }

            try
            {
                var sqllogContent = ReadLog(Path.Combine(_contentRoot, "Logs", "SqlLog.log"), Encoding.UTF8);

                if (!string.IsNullOrEmpty(sqllogContent))
                {
                    sqlLogs = sqllogContent.Split("--------------------------------")
                                  .Where(d => !string.IsNullOrEmpty(d) && d != "\n" && d != "\r\n")
                                  .Select(d => new LogInfo
                                  {
                                      Datetime = d.Split("|")[0].ObjectToDate(),
                                      Content = d.Split("|")[1]?.Replace("\r\n", "<br>"),
                                      LogColor = "SQL",
                                  }).ToList();
                }
            }
            catch (Exception) { }

            try
            {
                var Logs = JsonConvert.DeserializeObject<List<RequestInfo>>("[" + ReadLog(Path.Combine(_contentRoot, "Logs", "RequestIpInfoLog.log"), Encoding.UTF8) + "]");

                Logs = Logs.Where(d => d.Datetime.ObjectToDate() >= DateTime.Today).ToList();

                reqresLogs = Logs.Select(d => new LogInfo
                {
                    Datetime = d.Datetime.ObjectToDate(),
                    Content = $"IP:{d.Ip}<br>{d.Url}",
                    LogColor = "ReqRes",
                }).ToList();
            }
            catch (Exception)
            {
            }

            if (excLogs != null)
            {
                aopLogs.AddRange(excLogs);
            }
            if (sqlLogs != null)
            {
                aopLogs.AddRange(sqlLogs);
            }
            if (reqresLogs != null)
            {
                aopLogs.AddRange(reqresLogs);
            }
            aopLogs = aopLogs.OrderByDescending(d => d.Import).ThenByDescending(d => d.Datetime).Take(100).ToList();

            return aopLogs;
        }


        public static RequestApiWeekView RequestApiinfoByWeek()
        {
            List<RequestInfo> Logs = new List<RequestInfo>();
            List<ApiWeek> apiWeeks = new List<ApiWeek>();
            string apiWeeksJson = string.Empty;
            List<string> columns = new List<string>();
            columns.Add("日期");


            try
            {
                Logs = JsonConvert.DeserializeObject<List<RequestInfo>>("[" + ReadLog(Path.Combine(_contentRoot, "Logs", "RequestIpInfoLog.log"), Encoding.UTF8) + "]");

                var ddd = Logs.Where(d => d.Week == "週日").ToList();

                apiWeeks = (from n in Logs
                            group n by new { n.Week, n.Url } into g
                            select new ApiWeek
                            {
                                week = g.Key.Week,
                                url = g.Key.Url,
                                count = g.Count(),
                            }).ToList();

                //apiWeeks = apiWeeks.OrderByDescending(d => d.count).Take(8).ToList();

            }
            catch (Exception)
            {
            }

            StringBuilder jsonBuilder = new StringBuilder();
            jsonBuilder.Append("[");

            var weeks = apiWeeks.GroupBy(x => new { x.week }).Select(s => s.First()).ToList();
            foreach (var week in weeks)
            {
                var apiweeksCurrentWeek = apiWeeks.Where(d => d.week == week.week).OrderByDescending(d => d.count).Take(8).ToList();
                jsonBuilder.Append("{");

                jsonBuilder.Append("\"");
                jsonBuilder.Append("日期");
                jsonBuilder.Append("\":\"");
                jsonBuilder.Append(week.week);
                jsonBuilder.Append("\",");

                foreach (var item in apiweeksCurrentWeek)
                {
                    jsonBuilder.Append("\"");
                    jsonBuilder.Append(item.url);
                    jsonBuilder.Append("\":\"");
                    jsonBuilder.Append(item.count);
                    jsonBuilder.Append("\",");
                }
                jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
                jsonBuilder.Append("},");
            }

            jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
            jsonBuilder.Append("]");

            columns.AddRange(apiWeeks.OrderByDescending(d => d.count).Take(8).Select(d => d.url).ToList());

            return new RequestApiWeekView()
            {
                columns = columns,
                rows = jsonBuilder.ToString(),
            };
        }

        public static AccessApiDateView AccessApiByDate()
        {
            List<RequestInfo> Logs = new List<RequestInfo>();
            List<ApiDate> apiDates = new List<ApiDate>();
            try
            {
                Logs = JsonConvert.DeserializeObject<List<RequestInfo>>("[" + ReadLog(Path.Combine(_contentRoot, "Logs", "RequestIpInfoLog.log"), Encoding.UTF8) + "]");

                apiDates = (from n in Logs
                            group n by new { n.Date } into g
                            select new ApiDate
                            {
                                date = g.Key.Date,
                                count = g.Count(),
                            }).ToList();

                apiDates = apiDates.OrderByDescending(d => d.date).Take(7).ToList();

            }
            catch (Exception)
            {
            }

            return new AccessApiDateView()
            {
                columns = new string[] { "date", "count" },
                rows = apiDates.OrderBy(d => d.date).ToList(),
            };
        }

        public static AccessApiDateView AccessApiByHour()
        {
            List<RequestInfo> Logs = new List<RequestInfo>();
            List<ApiDate> apiDates = new List<ApiDate>();
            try
            {
                Logs = JsonConvert.DeserializeObject<List<RequestInfo>>("[" + ReadLog(Path.Combine(_contentRoot, "Logs", "RequestIpInfoLog.log"), Encoding.UTF8) + "]");

                apiDates = (from n in Logs
                            where n.Datetime.ObjectToDate() >= DateTime.Today
                            group n by new { hour = n.Datetime.ObjectToDate().Hour } into g
                            select new ApiDate
                            {
                                date = g.Key.hour.ToString("00"),
                                count = g.Count(),
                            }).ToList();

                apiDates = apiDates.OrderBy(d => d.date).Take(24).ToList();

            }
            catch (Exception)
            {
            }

            return new AccessApiDateView()
            {
                columns = new string[] { "date", "count" },
                rows = apiDates,
            };
        }
    }

}
