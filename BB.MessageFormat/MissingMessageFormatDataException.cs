using System;

namespace BB.MessageFormat
{
    /// <summary>
    /// A custom exception that occurs when you forget to pass expected data to a Message when attempting to get it's
    /// final string value.
    /// </summary>
    public class MissingMessageFormatDataException : Exception
    {
        public MissingMessageFormatDataException()
        {
        }

        public MissingMessageFormatDataException(string message)
            : base(message)
        {
        }

        public MissingMessageFormatDataException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public MissingMessageFormatDataException(string message, Exception innerException, params object[] args)
            : base(string.Format(message, args), innerException)
        {
        }
    }
}