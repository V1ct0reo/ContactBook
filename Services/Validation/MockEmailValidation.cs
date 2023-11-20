using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactBook.Services.Validation
{
    internal class MockEmailValidation : IValidationService
    {
        public ValidationResult Validate(string email)
        {
            if (email == null) return new ValidationResult() { IsValid = false };
            var result = new ValidationResult() { IsValid = email.Contains("@") };
            return result;
        }
    }
}
