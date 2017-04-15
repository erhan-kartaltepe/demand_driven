using System;

namespace DemandDriven.Controllers
{
    internal class FileFormatException : Exception
    {
        public FileFormatException()
        {
        }

        public FileFormatException(string message) : base(message)
        {
        }

        public FileFormatException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}