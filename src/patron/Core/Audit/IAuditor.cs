using System.Threading.Tasks;
using Core.Domain;

namespace Core.Audit
{
    public interface IAuditor
    {
        Task Audit(IDomainEvent domainEvent);
    }
}