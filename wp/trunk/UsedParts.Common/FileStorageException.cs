using System;

namespace UsedParts.Common
{
    public class FileStorageException : GenericException
    {
        public FileStorageException(string message)
			: base(message)
		{
		}

        public FileStorageException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

    }
}
