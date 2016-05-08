using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Abp.Events.Bus;
using Abp.UI;
using EventCloud.Users;
using System.Data.Entity;

namespace EventCloud.Events
{
    public class EventManager : IEventManager
    {
        public IEventBus EventBus { get; set; }

        private readonly IEventRegistrationPolicy _registrationPolicy;
        private readonly IRepository<EventRegistration> _eventRegistrationRepository;
        private readonly IRepository<Event, Guid> _eventRepository;

        public EventManager(IEventRegistrationPolicy registrationPolicy, IRepository<EventRegistration> eventRegistrationRepository,
                            IRepository<Event, Guid> eventRepository)
        {
            _registrationPolicy = registrationPolicy;
            _eventRegistrationRepository = eventRegistrationRepository;
            _eventRepository = eventRepository;

            EventBus = NullEventBus.Instance;
        } 

        public void Cancel(Event @event)
        {
            throw new NotImplementedException();
        }

        public async Task CreateAsync(Event @event)
        {
            await _eventRepository.InsertAsync(@event);
        }

        public Task<Event> GetAsync(Guid id)
        {
            var @event = _eventRepository.GetAsync(id);
            if(@event == null)
            {
                throw new UserFriendlyException("Could not retrieve event, may be it's deleted");
            }
            return @event;
        }

        public async Task<IReadOnlyList<User>> GetRegisteredUsersAsync(Event @event)
        {
            return await _eventRegistrationRepository
           .GetAll()
           .Include(registration => registration.User)
           .Where(registration => registration.EventId == @event.Id)
           .Select(registration => registration.User)
           .ToListAsync();
        }

        public Task<EventManager> RegisterAsync(Event @event, User user)
        {
            throw new NotImplementedException();
        }
    }
}
