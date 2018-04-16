using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AeroGear.Mobile.Core.Logging
{
    /// <summary>
    /// Logger that logs nothing.
    /// </summary>
    sealed class NullLogger : ILogger
    {
        public void Debug(string tag, string message)
        {
        
        }

        public void Debug(string message)
        {
        
        }

        public void Debug(string tag, string message, System.Exception e)
        {
        
        }

        public void Debug(string message, System.Exception e)
        {
         
        }

        public void Error(string tag, string message)
        {
         
        }

        public void Error(string message)
        {
         
        }

        public void Error(string tag, string message, System.Exception e)
        {
         
        }

        public void Error(string message, System.Exception e)
        {
         
        }

        public void Info(string tag, string message)
        {
         
        }

        public void Info(string message)
        {
         
        }

        public void Info(string tag, string message, System.Exception e)
        {
         
        }

        public void Info(string message, System.Exception e)
        {
         
        }

        public void Warning(string tag, string message)
        {
         
        }

        public void Warning(string message)
        {
         
        }

        public void Warning(string tag, string message, System.Exception e)
        {
         
        }

        public void Warning(string message, System.Exception e)
        {
         
        }
    }
}
