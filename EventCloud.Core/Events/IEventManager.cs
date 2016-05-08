using Abp.Domain.Services;
using EventCloud.Users;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventCloud.Events
{
    public interface IEventManager :IDomainService
    {
        Task<Event> GetAsync(Guid id);

        Task CreateAsync(Event @event);

        void Cancel(Event @event);

        Task<EventManager> RegisterAsync(Event @event, User user);

        Task<IReadOnlyList<User>> GetRegisteredUsersAsync(Event @event); 
    }
}