using SmartManager.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace SmartManager
{
    public class InscricaoEstadualValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string cleanedInscricaoEstadual = Regex.Replace((string) value ?? "", @"[^\d]", "");
            var client = (Client)validationContext.ObjectInstance;

            if ((client.PersonType == "Jurídica" || client.InscricaoEstadualPF) && !client.InscricaoEstadualIsento)
            {

                if (string.IsNullOrWhiteSpace(cleanedInscricaoEstadual))
                {
                    return new ValidationResult("Inscrição Estadual é obrigatória.");
                }

                if (cleanedInscricaoEstadual.Length > 12)
                {
                    return new ValidationResult("Inscrição Estadual pode ter no máximo 12 caracteres.");
                }
            }

            return ValidationResult.Success;

        }
    }
}
