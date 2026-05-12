using System.ComponentModel.DataAnnotations;

namespace VetCrm.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Informe o login")]
        [Display(Name = "Login")]
        public string Login { get; set; } = string.Empty;

        [Required(ErrorMessage = "Informe a senha")]
        [DataType(DataType.Password)]
        [Display(Name = "Senha")]
        public string Senha { get; set; } = string.Empty;
    }
}
