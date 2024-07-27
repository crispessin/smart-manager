using SmartManager.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace SmartManager
{
    public class DocumentValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is string documentNumber)
            {                
                string cleanedDocumentNumber = Regex.Replace(documentNumber, @"[^\d]", "");

                var client = (Client)validationContext.ObjectInstance;
                if (client.PersonType == "Física" && cleanedDocumentNumber.Length != 11)
                {
                    return new ValidationResult("CPF deve ter 11 dígitos.");
                }

                if (client.PersonType == "Jurídica" && cleanedDocumentNumber.Length != 14)
                {
                    return new ValidationResult("CNPJ deve ter 14 dígitos.");
                }

                return ValidationResult.Success;
            }
            return new ValidationResult("CPF/CNPJ é obrigatório.");
        }
    }
}
