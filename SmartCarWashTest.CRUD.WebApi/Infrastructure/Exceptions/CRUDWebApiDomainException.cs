using System;

namespace SmartCarWashTest.CRUD.WebApi.Infrastructure.Exceptions
{
    public class CrudWebApiDomainException : Exception
    {
        public CrudWebApiDomainException()
        {
        }

        public CrudWebApiDomainException(string message)
            : base(message)
        {
        }

        public CrudWebApiDomainException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}