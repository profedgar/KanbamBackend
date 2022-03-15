using System.ComponentModel.DataAnnotations;

namespace backend.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "O Login é obrigatório")]
        [EmailAddress(ErrorMessage = "Login inválido")]
        public string Login { get; set; }

        [Required(ErrorMessage = "A senha é obrigatória")]
        [DataType(DataType.Password)]
        public string Senha { get; set; }

    }
}
