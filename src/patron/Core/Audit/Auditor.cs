using System;
using System.Threading.Tasks;
using Core.Domain;

namespace Core.Audit
{
    public class Auditor : IAuditor
    {
        public async Task Audit(IDomainEvent domainEvent)
        {
            Console.WriteLine("Saving event ...");
        }
    }
}