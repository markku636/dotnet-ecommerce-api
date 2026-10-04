
using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EC226.Utility.Helper
{
    /// <summary>
    /// Json檔案讀寫
    /// 引用Newtonsoft.Json
    /// </summary>
    public class JsonFileHelper
    {
        //注意：section為根節點
        private readonly string _path;
        private IConfiguration Configuration { get; set; }
        public JsonFileHelper(string jsonName)
        {
            _path = !jsonName.EndsWith(".json") ? $"{jsonName}.json" : jsonName;
            //ReloadOnChange = true 當*.json檔案被修改時重新載入            
            Configuration = new ConfigurationBuilder()
            .Add(new JsonConfigurationSource { Path = _path, ReloadOnChange = true, Optional = true })
            .Build();
        }

        /// <summary>
        /// 讀取Json返回實體對像
        /// </summary>
        /// <returns></returns>
        public T Read<T>() => Read<T>("");

        /// <summary>
        /// 根據節點讀取Json返回實體對像
        /// </summary>
        /// <returns></returns>
        public T Read<T>(string section)
        {
            using var file = new StreamReader(_path);
            using var reader = new JsonTextReader(file);
            var jObj = (JObject)JToken.ReadFrom(reader);
            if (!string.IsNullOrWhiteSpace(section))
            {
                var secJt = jObj[section];
                if (secJt != null)
                {
                    return JsonConvert.DeserializeObject<T>(secJt.ToString());
                }
            }
            else
            {
                return JsonConvert.DeserializeObject<T>(jObj.ToString());
            }

            return default;
        }

        /// <summary>
        /// 讀取Json返回集合
        /// </summary>
        /// <returns></returns>
        public List<T> ReadList<T>() => ReadList<T>("");

        /// <summary>
        /// 根據節點讀取Json返回集合
        /// </summary>
        /// <returns></returns>
        public List<T> ReadList<T>(string section)
        {
            using var file = new StreamReader(_path);
            using var reader = new JsonTextReader(file);
            var jObj = (JObject)JToken.ReadFrom(reader);
            if (!string.IsNullOrWhiteSpace(section))
            {
                var secJt = jObj[section];
                if (secJt != null)
                {
                    return JsonConvert.DeserializeObject<List<T>>(secJt.ToString());
                }
            }
            else
            {
                return JsonConvert.DeserializeObject<List<T>>(jObj.ToString());
            }

            return default;
        }

        /// <summary>
        /// 寫入檔案
        /// </summary>
        /// <typeparam name="T">自定義對像</typeparam>
        /// <param name="t"></param>
        public void Write<T>(T t) => Write("", t);

        /// <summary>
        /// 寫入指定section檔案
        /// </summary>
        /// <typeparam name="T">自定義對像</typeparam>
        /// <param name="t"></param>
        public void Write<T>(string section, T t)
        {
            JObject jObj;
            using (StreamReader file = new StreamReader(_path))
            using (JsonTextReader reader = new JsonTextReader(file))
            {
                jObj = (JObject)JToken.ReadFrom(reader);
                var json = JsonConvert.SerializeObject(t);
                if (string.IsNullOrWhiteSpace(section))
                    jObj = JObject.Parse(json);
                else
                    jObj[section] = JObject.Parse(json);
            }

            using var writer = new StreamWriter(_path);
            using var jsonWriter = new JsonTextWriter(writer);
            jObj.WriteTo(jsonWriter);
        }

        /// <summary>
        /// 刪除指定section節點
        /// </summary>
        /// <param name="section"></param>
        /// <exception cref="Exception"></exception>
        public void Remove(string section)
        {
            JObject jObj;
            using (StreamReader file = new StreamReader(_path))
            using (JsonTextReader reader = new JsonTextReader(file))
            {
                jObj = (JObject)JToken.ReadFrom(reader);
                jObj.Remove(section);
            }

            using (var writer = new StreamWriter(_path))
            using (var jsonWriter = new JsonTextWriter(writer))
            {
                jObj.WriteTo(jsonWriter);
            }
        }
    }
}
