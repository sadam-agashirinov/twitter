using System.Linq;

namespace TwitterApi.DataLayer.Utils
{
    public static class ValidationUtils
    {
        public static bool IsValidName(string name)
        {
            return !string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(name.Trim());
        }

        public static bool IsValidNames(params string[] names)
        {
            return names.All(IsValidName);
        }
    }
}