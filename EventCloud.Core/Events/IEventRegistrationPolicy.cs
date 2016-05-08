using Abp.Domain.Services;
using EventCloud.Users;
using System.Threading.Tasks;

namespace EventCloud.Events
{
    public interface IEventRegistrationPolicy : IDomainService
    {
        Task CheckRegistrationAttemptASync(Event @event, User user);
    }
}
