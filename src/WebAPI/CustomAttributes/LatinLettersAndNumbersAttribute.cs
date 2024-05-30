using System.ComponentModel.DataAnnotations;
using WebAPI.Common;

namespace WebAPI.CustomAttributes;

public class LatinLettersAndNumbersAttribute : RegularExpressionAttribute
{
    public LatinLettersAndNumbersAttribute() 
        : base(RegularExpressions.LatinLettersAndNumbers)
    {
        ErrorMessage = "The field can only contain Latin letters and numbers.";
    }
}