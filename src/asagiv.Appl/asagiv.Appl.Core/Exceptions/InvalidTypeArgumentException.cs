using System;
using System.Runtime.Serialization;

namespace asagiv.Appl.Core.Exceptions
{
    public class InvalidTypeArgumentException : Exception
    {
        #region Properties
        public Type ExpectedType { get; }
        public Type ActualType { get; }
        #endregion

        #region Constructors
        public InvalidTypeArgumentException() { }
        public InvalidTypeArgumentException(string message)
            : base(message) { }
        public InvalidTypeArgumentException(string message, Exception innerException)
            : base(message, innerException) { }
        public InvalidTypeArgumentException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
        public InvalidTypeArgumentException(Type expectedType, Type actualType)
            : base($"Invalid type argument: Expected type: {expectedType.Name}, Actual Type: {actualType.Name}")
        {
            ExpectedType = expectedType;
            ActualType = actualType;
        }
        #endregion
    }
}
