using System.Threading.Tasks;

namespace Core.CQRS
{
    public interface ICommandBus
    {
        Task Run(Command command);
    }
}