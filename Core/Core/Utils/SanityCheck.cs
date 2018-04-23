using System;
namespace AeroGear.Mobile.Core.Utils
{
    public class SanityCheck
    {
        private SanityCheck()
        {
        }

        /// <summary>
        /// Checks that the passed in value is not null. If it is null, a ArgumentNullException is
        /// thrown with a message specifying that the parameter can't be null.
        /// </summary>
        /// <returns>The received value.</returns>
        /// <param name="value">String to be checked.</param>
        /// <param name="paramName">Parameter name to be used in the error message.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static T NonNull<T>(T value, string paramName) {
            if (value == null)
            {
                throw new ArgumentNullException(String.Format(paramName));
            }
            return value;
        }

        /// <summary>
        /// Checks if the passed in value is an empty string. The string gets trimmed. If it is empty, an
        /// ArgumentException gets thrown with an appropriate error message.
        /// </summary>
        /// <returns>The received value.</returns>
        /// <param name="value">String to be checked.</param>
        /// <param name="paramName">Parameter name to be used in the error message.</param>
        public static string NonEmpty(string value, string paramName)
        {
            return NonEmpty(value, true, "'{0}' can't be empty or null", paramName);
        }

        /// <summary>
        /// Checks if the passed in value is an empty string. if trim is true, the string
        /// gets trimmed.If it is empty, an ArgumentException gets thrown with an appropriate
        /// error message.
        /// </summary>
        /// <returns>The received value.</returns>
        /// <param name="value">String to be checked.</param>
        /// <param name="paramName">Parameter name to be used in the error message.</param>
        /// <param name="trim">whether the string must be trimmed or not.</param>
        public static string NonEmpty(string value, String paramName, bool trim)
        {
            return NonEmpty(value, trim, "'{0}' can't be empty or null", paramName);
        }

        /// <summary>
        /// Checks if the passed in value is an empty String. If it is empty an
        /// ArgumentException is thrown with the passed in custom message.The params are
        /// applied to the customMessage according to String.Format
        /// </summary>
        /// <returns>The received value.</returns>
        /// <param name="value">String to be checked.</param>
        /// <param name="customMessage">Custom message to be put into the exception.</param>
        /// <param name="messageParams">Parameters to be applied to the custom message.</param>
        public static string NonEmpty(string value, string customMessage, params Object[] messageParams)
        {
            return NonEmpty(value, true, customMessage, messageParams);
        }

        /// <summary>
        /// Checks if the passed in value is an empty String. If it is empty an
        /// ArgumentException is thrown with the passed in custom message. The params are
        /// applied to the customMessage according to String.Format
        /// </summary>
        /// <returns>The received value.</returns>
        /// <param name="value">String to be checked.</param>
        /// <param name="trim">Whether the string must be trimmed or not.</param>
        /// <param name="customMessage">Custom message to be put into the exception.</param>
        /// <param name="messageParams">Parameters to be applied to the custom message.</param>
        public static string NonEmpty(string value, bool trim,
                                      string customMessage, params Object[] messageParams)
        {
            NonNull(value, String.Format(customMessage, messageParams));

            if (value.Length == 0 || (trim && value.Trim().Length == 0))
            {
                throw new ArgumentException(String.Format(customMessage, messageParams));
            }
            return value;
        }
    }
}
