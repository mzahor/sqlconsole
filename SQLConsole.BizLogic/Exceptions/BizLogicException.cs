using System;

namespace SqlConsole.BizLogic.Exceptions
{
    public class BizLogicException : ApplicationException
    {
        public BizLogicException(string message)
            : base(message)
        {
        }

        public BizLogicException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
