using System;
using System.Collections.Generic;
using System.Linq;

namespace GroundRunning.Common
{
    public class AutomationResult
    {
        public AutomationResult()
        {
            Exceptions = new List<Exception>();
            Warnings = new List<Exception>();
        }

        public bool WasSuccessful 
        { 
            get { return !Exceptions.Any(); } 
        }

        public bool HadWarnings
        {
            get { return !Warnings.Any(); }
        }

        public List<Exception> Exceptions { get; private set; }

        public IEnumerable<string> ErrorMessages 
        {
            get
            {
                if(!WasSuccessful)
                {
                    return Exceptions.Select(e => e.Message);
                }
                return null;
            } 
        }

        public List<Exception> Warnings { get; private set; }

        public IEnumerable<string> WarningMessages
        {
            get 
            {
                if(HadWarnings)
                {
                    return Warnings.Select(w => w.Message);
                }
                return null;
            }
        }

        public void AddResult(AutomationResult result)
        {
            Exceptions.AddRange(result.Exceptions);
        }
        
        public void AddException(Exception exception)
        {
            Exceptions.Add(exception);
        }

        /// <summary>
        /// Adds a new exception of type <typeparamref name="AutomationException"/>
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="exception">Inner Exception</param>
        public void AddException(string message, Exception exception)
        {
            Exceptions.Add(new AutomationException(message, exception));
        }
        
        public void AddWarning(Exception warning)
        {
            Warnings.Add(warning);
        }

        /// <summary>
        /// Adds a new warning of type <typeparamref name="AutomationException"/>
        /// </summary>
        /// <param name="message">Warning message</param>
        /// <param name="exception">The Exception</param>
        public void AddWarning(string message, Exception exception)
        {
            Warnings.Add(new AutomationException(message, exception));
        }

        public void ClearResults()
        {
            Exceptions.Clear();
            Warnings.Clear();
        }

    }
}
