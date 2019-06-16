using System;

namespace Core.CQRS
{
    public abstract class QueryBus
    {
        private readonly IServiceProvider services;
        public QueryBus(IServiceProvider services)
        {
            this.services = services;
        }
    }
}