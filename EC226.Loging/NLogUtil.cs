using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using EC226.Configuration;
using Newtonsoft.Json;
using NLog;
using NLog.Config;
using SqlSugar;

namespace EC226.Loging
{
    public static class NLogUtil
    {
        private static readonly Logger FileLogger = LogManager.GetLogger("logfile");
        private static readonly Logger SEQLogger = LogManager.GetLogger("seq");

        public static void WriteSEQLog(string message, LogLevel logLevel,  Exception exception = null, dynamic extraData = null)
        {
            LogEventInfo theEvent = GetLogEventInfo(message, logLevel,  exception, extraData);

            
            SEQLogger.Log(theEvent);
        }

        public static void WriteFileLog(string message, LogLevel logLevel,  Exception exception = null, dynamic extraData = null)
        {
            LogEventInfo theEvent = GetLogEventInfo(message, logLevel,  exception, extraData);

            FileLogger.Log(theEvent);
        }

        private static LogEventInfo GetLogEventInfo(string message, LogLevel logLevel, Exception exception = null, dynamic extraData = null)
        {
            LogEventInfo theEvent = new LogEventInfo(logLevel, FileLogger.Name, message);
            
            if (exception != null)
            {
                theEvent.Exception = exception;
            }
            
            if (extraData != null)
            {
                string optionJson = JsonConvert.SerializeObject(extraData);

                theEvent.Properties["ExtraData"] = optionJson;
            }

            return theEvent;
        }

        /// <summary>
        /// 同時寫入到日誌到資料庫和檔案
        /// </summary>
        /// <param name="logLevel">日誌等級</param>
        /// <param name="logType">日誌類型</param>
        /// <param name="logTitle">標題（255字元）</param>
        /// <param name="message">資訊</param>
        /// <param name="exception">異常</param>
        public static void WriteAll(string message, LogLevel logLevel,  Exception exception = null)
        {
            //先存檔案
            WriteFileLog(message, logLevel,  exception);

            // 寫入 seq 
            WriteSEQLog(message, logLevel, exception);
        }
    }
}
