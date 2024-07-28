using SmartManager.Models;
using System.ComponentModel.DataAnnotations;

namespace SmartManager
{
    public class BirthDateValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var client = (Client)validationContext.ObjectInstance;
            if (client.PersonType == "Física" && client.BirthDate == null)
            {
                return new ValidationResult("O campo Data de Nascimento é obrigatório para pessoa física.");
            }
            return ValidationResult.Success;
        }
    }
}
