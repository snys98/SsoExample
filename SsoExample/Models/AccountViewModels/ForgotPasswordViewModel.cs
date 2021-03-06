﻿using System.ComponentModel.DataAnnotations;

namespace SsoExample.Models.AccountViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
