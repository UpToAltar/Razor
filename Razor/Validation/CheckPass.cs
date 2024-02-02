using System.ComponentModel.DataAnnotations;

namespace Razor.Validation;

public class CheckPass : ValidationAttribute
{
    public CheckPass() => ErrorMessage = "Passswords must contain at least one uppercase, one lowercase, one number ";

    public override bool IsValid(object? value)
    {
        if (value != null)
        {
            string s = value.ToString();
            int up = 0;
            int low = 0;
            int num = 0;
            foreach (var c in s)
            {
                if(c >= 'A' && c <= 'Z')
                {
                    up++;
                }
                else if(c >= 'a' && c <= 'z')
                {
                    low++;
                }
                else if(c >= '0' && c <= '9')
                {
                    num++;
                }
                
                
            }
            if(up >= 1 && low >= 1 && num >= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }
}