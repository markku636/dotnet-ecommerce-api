using System;
using System.Collections.Generic;
using System.Text;

/* TODO!! 這不要用，之後要移除 */
namespace EC226.MemberSite.Model.CodeGenModels
{
    public class ApiResult<T>
    {
        public T Data { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public bool IsKeyValuePair { get; set; }
        public string Url { get; set; }
    }
}
