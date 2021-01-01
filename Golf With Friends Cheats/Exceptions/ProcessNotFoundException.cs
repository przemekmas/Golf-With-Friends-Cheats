using System;

namespace Golf_With_Friends_Cheats.Exceptions
{
    public class ProcessNotFoundException : Exception
    {
        public ProcessNotFoundException(string message) : base(message)
        {
        }
    }
}
