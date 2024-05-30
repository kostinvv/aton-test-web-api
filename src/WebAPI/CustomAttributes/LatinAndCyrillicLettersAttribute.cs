using System.ComponentModel.DataAnnotations;
using WebAPI.Common;

namespace WebAPI.CustomAttributes;

public class LatinAndCyrillicLettersAttribute : RegularExpressionAttribute
{
    public LatinAndCyrillicLettersAttribute() 
        : base(RegularExpressions.LatinAndCyrillicLetters)
    {
        ErrorMessage = "The field can only contain Latin and Cyrillic letters.";
    }
}