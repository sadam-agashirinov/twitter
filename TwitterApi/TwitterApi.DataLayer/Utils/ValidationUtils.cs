using System.Linq;

namespace TwitterApi.DataLayer.Utils
{
    public static class ValidationUtils
    {
        public static bool IsValidString(string str)
        {
            return !string.IsNullOrEmpty(str) && !string.IsNullOrEmpty(str.Trim());
        }

        public static bool IsValidStrings(params string[] strings)
        {
            return strings.All(IsValidString);
        }
    }
}