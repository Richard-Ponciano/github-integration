using System.ComponentModel.DataAnnotations;

namespace GithubIntegration.Domain.Model
{
    public class Usuario
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Compo Obrigatório")]
        [MinLength(3, ErrorMessage = "São necessários ao menos 3 caracteres")]
        [MaxLength(10, ErrorMessage = "São permitidos no máximo 10 caracteres")]
        public string User { get; set; }
    }
}