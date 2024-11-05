using System.Runtime.CompilerServices;

namespace REST_API_UNIT.Utils
{
    public class InputValidationUtils
    {
        public static bool IsValid(object input)
        {
            if(input is string str)
            {
                return !string.IsNullOrWhiteSpace(str);
            }
            return false;
        }
    }
}
