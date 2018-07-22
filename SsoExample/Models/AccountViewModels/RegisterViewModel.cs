using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace SsoExample.Models.AccountViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [Phone]
        [Display(Name = "手机号")]
        public string Phone { get; set; }
        [Required]
        [Remote("ValidateCode","Account", AdditionalFields = nameof(Phone),ErrorMessage = "验证码验证失败")]
        [Display(Name = "短信验证码")]
        public string Code { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "{0}的长度必须为{2}-{1}之间的数值.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "确认密码")]
        [Compare(nameof(Password), ErrorMessage = "两次输入的密码必须相同.")]
        public string ConfirmPassword { get; set; }
    }
}
