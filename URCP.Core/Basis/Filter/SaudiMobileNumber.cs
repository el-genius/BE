using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace URCP.Core
{
    public class SaudiMobileNumber : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var mobilePattern = @"^(009665|9665|\+9665|05|5)([0-9]{8})$";

            if (value != null && !string.IsNullOrEmpty(value.ToString()) && !Regex.IsMatch(value.ToString(), mobilePattern))
                return new ValidationResult(this.FormatErrorMessage(validationContext.DisplayName));

            return ValidationResult.Success;
        }
    }
}
