using System;
namespace AeroGear.Mobile.Core.Utils
{
    public class SanityCheck
    {
        private SanityCheck()
        {
        }

        public static T nonNull<T>(T value, string paramName) {
            if (value == null)
            {
                throw new ArgumentNullException(String.Format(paramName));
            }
            return value;
        }

        public static string nonEmpty(string value, string paramName)
        {
            return nonEmpty(value, true, "'{0}' can't be empty or null", paramName);
        }

        public static string nonEmpty(string value, String paramName, bool trim)
        {
            return nonEmpty(value, trim, "'{0}' can't be empty or null", paramName);
        }

        public static string nonEmpty(string value, string customMessage, params Object[] messageParams)
        {
            return nonEmpty(value, true, customMessage, messageParams);
        }

        public static string nonEmpty(string value, bool trim,
                                      string customMessage, params Object[] messageParams)
        {
            nonNull(value, customMessage);

            if (value.Length == 0 || (trim && value.Trim().Length == 0))
            {
                throw new ArgumentException(String.Format(customMessage, messageParams));
            }
            return value;
        }
    }
}
