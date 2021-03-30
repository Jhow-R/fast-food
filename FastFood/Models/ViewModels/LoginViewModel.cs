using System;
using System.ComponentModel.DataAnnotations;

namespace FastFood.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Usuário")]
        public String Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public String Password { get; set; }

        public String ReturnUrl { get; set; }
    }
}
