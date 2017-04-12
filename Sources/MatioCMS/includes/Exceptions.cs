using System;

namespace MatioCMS
{
    /// <summary>
    /// Base exception type in Matio CMS
    /// </summary>
    public class ApplicationException : Exception
    {
        public ApplicationException() : base() { }
        public ApplicationException(string message) : base(message) { }
        public ApplicationException(string message, Exception innerException) : base(message, innerException) { }
    }

    public class ConfigFileNotFoundException : ApplicationException
    {
        public ConfigFileNotFoundException() : base() { }
        public ConfigFileNotFoundException(string message) : base(message) { }
        public ConfigFileNotFoundException(string message, Exception innerException) : base(message, innerException) { }
    }

    public class AdminException : ApplicationException
    {
        public AdminException() : base() { }
        public AdminException(string message) : base(message) { }
        public AdminException(string message, Exception innerException) : base(message, innerException) { }
    }

    public class AdminLoginException : AdminException
    {
        public AdminLoginException() : base() { }
        public AdminLoginException(string message) : base(message) { }
        public AdminLoginException(string message, Exception innerException) : base(message, innerException) { }
    }
}
