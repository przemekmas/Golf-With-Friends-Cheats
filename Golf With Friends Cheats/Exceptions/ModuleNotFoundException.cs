using System;

namespace Golf_With_Friends_Cheats.Exceptions
{
    public class ModuleNotFoundException : Exception
    {
        public ModuleNotFoundException(string message) : base(message)
        {
        }
    }
}
