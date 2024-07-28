using SmartManager.Models;
using System.ComponentModel.DataAnnotations;

namespace SmartManager
{
    public class GenderValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var client = (Client)validationContext.ObjectInstance;
            if (client.PersonType == "Física" && string.IsNullOrEmpty(client.Gender))
            {
                return new ValidationResult("O campo Gênero é obrigatório para pessoa física.");
            }
            return ValidationResult.Success;
        }
    }
}
