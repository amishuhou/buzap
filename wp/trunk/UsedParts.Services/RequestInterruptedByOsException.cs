using System;
using UsedParts.Common;

namespace UsedParts.Services
{
    public class RequestInterruptedByOsException : GenericException
    {
        public RequestInterruptedByOsException()
        {
        }

        public RequestInterruptedByOsException(string message)
            : base(message)
        {
        }

        public RequestInterruptedByOsException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
