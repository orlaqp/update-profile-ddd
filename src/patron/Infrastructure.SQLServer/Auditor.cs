using System;
using System.Threading.Tasks;
using Core.Audit;
using Core.Domain;

namespace Infrastructure.SQLServer
{
    public class Auditor : IAuditor
    {
        public async Task Audit(IDomainEvent domainEvent)
        {
            Console.WriteLine("Auditing event ...");
        }
    }
}