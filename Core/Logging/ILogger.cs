using System;
using SystemException=System.Exception;

namespace AeroGear.Mobile.Core.Logging
{
    /// <summary>
    /// Common interface for logging.
    /// </summary>
    public interface ILogger
    {
        void Info(String tag, String message);

        void Info(String message);

        void Info(String tag, String message, SystemException e);

        void Info(String message, SystemException e);

        void Warning(String tag, String message);

        void Warning(String message);

        void Warning(String tag, String message, SystemException e);

        void Warning(String message, SystemException e);

        void Debug(String tag, String message);

        void Debug(String message);

        void Debug(String tag, String message, SystemException e);

        void Debug(String message, SystemException e);

        void Error(String tag, String message);

        void Error(String message);

        void Error(String tag, String message, SystemException e);

        void Error(String message, SystemException e);
    }
}
