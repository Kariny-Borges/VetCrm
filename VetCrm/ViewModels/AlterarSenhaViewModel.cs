using System.ComponentModel.DataAnnotations;

namespace VetCrm.ViewModels
{
    public class AlterarSenhaViewModel
    {
        [Required(ErrorMessage = "Informe a senha atual")]
        [DataType(DataType.Password)]
        [Display(Name = "Senha atual")]
        public string SenhaAtual { get; set; } = string.Empty;

        [Required(ErrorMessage = "Informe a nova senha")]
        [DataType(DataType.Password)]
        [MinLength(4, ErrorMessage = "A senha deve ter ao menos 4 caracteres")]
        [Display(Name = "Nova senha")]
        public string NovaSenha { get; set; } = string.Empty;

        [Required(ErrorMessage = "Confirme a nova senha")]
        [DataType(DataType.Password)]
        [Compare(nameof(NovaSenha), ErrorMessage = "As senhas não coincidem")]
        [Display(Name = "Confirmar nova senha")]
        public string ConfirmarSenha { get; set; } = string.Empty;
    }
}
