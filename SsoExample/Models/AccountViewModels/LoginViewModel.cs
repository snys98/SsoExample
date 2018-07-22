using System.ComponentModel.DataAnnotations;

namespace SsoExample.Models.AccountViewModels
{
    public class LoginViewModel
    {

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string Password { get; set; }

        [Display(Name = "记住我?")]
        public bool RememberMe { get; set; }
        [Required]
        [Display(Name = "账号/手机号")]
        public string UsernameOrPhone { get; set; }

        public string ReturnUrl { get; set; }
    }
}
