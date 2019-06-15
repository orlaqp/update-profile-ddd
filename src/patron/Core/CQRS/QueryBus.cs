using System;

namespace Core.CQRS
{
    public class QueryBus
    {
        private readonly IServiceProvider services;
        public QueryBus(IServiceProvider services)
        {
            this.services = services;
        }
    }
}