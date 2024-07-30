using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartManager.Models
{
    public class Client
    {        
        public int Id { get; set; }

        [Required(ErrorMessage = "Nome do Cliente/Raz�o Social � obrigat�rio")]
        [StringLength(150, ErrorMessage = "Nome do Cliente/Raz�o Social pode ter no m�ximo 150 caracteres")]
        public string Name { get; set; }

        [Required(ErrorMessage = "E-mail � obrigat�rio")]
        [StringLength(150, ErrorMessage = "E-mail pode ter no m�ximo 150 caracteres")]
        [EmailAddress(ErrorMessage = "E-mail inv�lido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Telefone � obrigat�rio")]
        [RegularExpression(@"^\d{10,11}$", ErrorMessage = "Telefone deve ter 10 ou 11 caracteres num�ricos")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Tipo de Pessoa � obrigat�rio")]
        public string PersonType { get; set; }

        [Required(ErrorMessage = "CPF/CNPJ � obrigat�rio")]        
        [DocumentValidation]
        public string DocumentNumber { get; set; }
        
        [InscricaoEstadualValidation]
        public string? InscricaoEstadual { get; set; }

        public bool InscricaoEstadualPF { get; set; }

        public bool InscricaoEstadualIsento { get; set; }

        public bool IsBlocked { get; set; }

        [GenderValidation]
        public string? Gender { get; set; }

        [BirthDateValidation]
        public DateTime? BirthDate { get; set; }

        [StringLength(15, MinimumLength = 8, ErrorMessage = "Senha deve ter entre 8 e 15 caracteres")]
        public string? Password { get; set; }

        [NotMapped]
        [Compare("Password", ErrorMessage = "As senhas n�o conferem")]
        public string? ConfirmPassword { get; set; }

        [DataType(DataType.Date)]
        public DateTime RegistrationDate { get; set; } = DateTime.Now;
        
    }
}