using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroundRunning.Common
{
    public class AutomationException : Exception
    {
        private string p;
        private decimal requiredVersion;


        public AutomationException()
        {
        }

        public AutomationException(string message) : base(message)
        {
        }
        
        public AutomationException(string message, Exception exception) : base(message, exception)
        {
        }

        public AutomationException(string messageFormat, params object[] args) : base(string.Format(messageFormat, args))
        {
        }     
    }
}
