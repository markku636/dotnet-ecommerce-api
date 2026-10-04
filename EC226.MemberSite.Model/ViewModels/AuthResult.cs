using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EC226.MemberSite.Model.ViewModels
{
    public class AuthResult
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public bool Success { get; set; }
        public List<string> Errors { get; set; }
    }

    public class TokenRequest
    {
        /// <summary>
        /// 原 Token
        /// </summary>
        [Required]
        public string Token { get; set; }
        /// <summary>
        /// Refresh Token
        /// </summary>
        [Required]
        public string RefreshToken { get; set; }
    }

}
