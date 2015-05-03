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
        }

        public bool WasSuccessful 
        { 
            get { return !Exceptions.Any(); } 
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

        public void AddException(Exception exception)
        {
            Exceptions.Add(exception);
        }
        
        public void AddResult(AutomationResult result)
        {
            Exceptions.AddRange(result.Exceptions);
        }

        public void ClearResult()
        {
            Exceptions.Clear();
        }

    }
}
