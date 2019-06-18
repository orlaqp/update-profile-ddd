using System;

namespace Core.Exceptions
{
    public class BusinessException : Exception
    {
        public BusinessException() : base()
        {
            
        }

        public BusinessException(string code, string message) : base(message)
        {
            Code = code;
        }

        public string Code { get; protected set; }
    }
}