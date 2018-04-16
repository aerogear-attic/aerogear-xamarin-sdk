using System;
using SystemException=System.Exception;

namespace AeroGear.Mobile.Core.Logging
{
    /// <summary>
    /// Common interface for logging.
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Send an INFO log message
        /// </summary>
        /// <param name="tag">Tag.</param>
        /// <param name="message">Message.</param>
        void Info(String tag, String message);

        /// <summary>
        /// Send an INFO log message
        /// </summary>
        /// <param name="message">Message.</param>
        void Info(String message);

        /// <summary>
        /// Send an INFO log message and log the exception.
        /// </summary>
        /// <param name="tag">Tag.</param>
        /// <param name="message">Message.</param>
        /// <param name="e">Exception.</param>
        void Info(String tag, String message, SystemException e);

        /// <summary>
        /// Send an INFO log message and log the exception.
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="e">Exception</param>
        void Info(String message, SystemException e);

        /// <summary>
        /// Send a WARN log message
        /// </summary>
        /// <param name="tag">Tag.</param>
        /// <param name="message">Message.</param>
        void Warning(String tag, String message);

        /// <summary>
        /// Send a WARN log message
        /// </summary>
        /// <param name="message">Message.</param>
        void Warning(String message);

        /// <summary>
        /// Send a WARN log message and log the exception.
        /// </summary>
        /// <param name="tag">Tag.</param>
        /// <param name="message">Message.</param>
        /// <param name="e">Exception.</param>
        void Warning(String tag, String message, SystemException e);

        /// <summary>
        /// Send a WARN log message and log the exception.
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="e">Exception</param>
        void Warning(String message, SystemException e);

        /// <summary>
        /// Send a DEBUG log message
        /// </summary>
        /// <param name="tag">Tag.</param>
        /// <param name="message">Message.</param>
        void Debug(String tag, String message);

        /// <summary>
        /// Send a DEBUG log message
        /// </summary>
        /// <param name="message">Message.</param>
        void Debug(String message);

        /// <summary>
        /// Send a DEBUG log message and log the exception.
        /// </summary>
        /// <param name="tag">Tag.</param>
        /// <param name="message">Message.</param>
        /// <param name="e">Exception.</param>
        void Debug(String tag, String message, SystemException e);


        /// <summary>
        /// Send a DEBUG log message and log the exception.
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="e">Exception</param>
        void Debug(String message, SystemException e);

        /// <summary>
        /// Send an ERROR log message
        /// </summary>
        /// <param name="tag">Tag.</param>
        /// <param name="message">Message.</param>
        void Error(String tag, String message);

        /// <summary>
        /// Send an ERROR log message
        /// </summary>
        /// <param name="message">Message.</param>
        void Error(String message);

        /// <summary>
        /// Send an ERROR log message and log the exception.
        /// </summary>
        /// <param name="tag">Tag.</param>
        /// <param name="message">Message.</param>
        /// <param name="e">Exception.</param>
        void Error(String tag, String message, SystemException e);

        /// <summary>
        /// Send an ERROR log message and log the exception.
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="e">Exception</param>
        void Error(String message, SystemException e);
    }
}
