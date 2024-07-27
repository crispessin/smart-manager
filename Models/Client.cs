using System.ComponentModel.DataAnnotations;

namespace SmartManager.Models
{
    public class Client
    {
        public const string Isento = "Isento";

        public int Id { get; set; }

        [Required(ErrorMessage = "Nome do Cliente/Razão Social é obrigatório")]
        [StringLength(150, ErrorMessage = "Nome do Cliente/Razão Social pode ter no máximo 150 caracteres")]
        public string Name { get; set; }

        [Required(ErrorMessage = "E-mail é obrigatório")]
        [StringLength(150, ErrorMessage = "E-mail pode ter no máximo 150 caracteres")]
        [EmailAddress(ErrorMessage = "E-mail inválido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Telefone é obrigatório")]
        [RegularExpression(@"^\d{10,11}$", ErrorMessage = "Telefone deve ter 10 ou 11 caracteres numéricos")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Tipo de Pessoa é obrigatório")]
        public string PersonType { get; set; }

        [Required(ErrorMessage = "CPF/CNPJ é obrigatório")]        
        [DocumentValidation]
        public string DocumentNumber { get; set; }

        [Required(ErrorMessage = "Inscrição Estadual é obrigatória")]
        [StringLength(12, ErrorMessage = "Inscrição Estadual pode ter no máximo 12 caracteres")]
        public string InscricaoEstadual { get; set; }

        public bool IsBlocked { get; set; }

        [GenderValidation]
        public string? Gender { get; set; }

        [BirthDateValidation]
        public DateTime? BirthDate { get; set; }

        [StringLength(15, MinimumLength = 8, ErrorMessage = "Senha deve ter entre 8 e 15 caracteres")]
        public string? Password { get; set; }
        
        [Compare("Password", ErrorMessage = "As senhas não conferem")]
        public string? ConfirmPassword { get; set; }

        [DataType(DataType.Date)]
        public DateTime RegistrationDate { get; set; } = DateTime.Now;
    }
}