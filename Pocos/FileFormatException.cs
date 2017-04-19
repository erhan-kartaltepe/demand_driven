using System;

namespace DemandDriven.Controllers
{
    /// <summary>
    /// Exception class for invalid file formats
    /// </summary>
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