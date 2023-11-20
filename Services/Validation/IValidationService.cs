using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactBook.Services.Validation
{
    public interface IValidationService
    {
        ValidationResult Validate(string text);
    }
}
