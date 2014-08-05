using System;
using UsedParts.Common;

namespace UsedParts.Services
{
    public class ServiceException : GenericException
    {
        public ServiceException()
        {
        }

        public ServiceException(string message)
            : base(message)
        {
        }

        public ServiceException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
