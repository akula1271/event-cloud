using System.Threading.Tasks;
using EventCloud.Users;
using Abp.Domain.Repositories;
using System;
using Abp.Timing;
using EventCloud.Configuration;
using Abp.UI;

namespace EventCloud.Events
{
    class EventRegistrationPolicy : EventCloudServiceBase, IEventRegistrationPolicy
    {
        private readonly IRepository<EventRegistration> _eventRegistrationRepository;

        public EventRegistrationPolicy (IRepository<EventRegistration> eventRegistrationReposiory)
        {
            _eventRegistrationRepository = eventRegistrationReposiory;
        }

        public async Task CheckRegistrationAttemptASync(Event @event, User user)
        {
            if(@event ==null) { throw new ArgumentNullException("event"); }
            if(user == null) { throw new ArgumentNullException("user"); }

            CheckEventDate(@event);
            await CheckEventRegistrationFrequencyAsync(user);
        }

        private static void CheckEventDate(Event @event)
        {
            if (@event.IsInPast())
            {
                throw new UserFriendlyException("Can not register event in the past!"); //TODO: Localize
            }
        }

        private async Task CheckEventRegistrationFrequencyAsync(User user)
        {
            var oneMonthAgo = Clock.Now.AddDays(-30);
            var maxAllowedEventRegistrationCountInLast30DaysPerUser = await SettingManager.GetSettingValueAsync(EventCloudSettingNames.MaxAllowedEventRegistrationCountInLast30DaysPerUser);
            if(Int32.Parse(maxAllowedEventRegistrationCountInLast30DaysPerUser) > 0)
            {
                var registrationCountInLast30Days = await _eventRegistrationRepository.CountAsync(r => r.UserId == user.Id && r.CreationTime >= oneMonthAgo);
                if (registrationCountInLast30Days > Int32.Parse(maxAllowedEventRegistrationCountInLast30DaysPerUser))
                {
                    throw
                        new UserFriendlyException(string.Format("Can not register more than {0} events in one month", Int32.Parse(maxAllowedEventRegistrationCountInLast30DaysPerUser)));
                }
            }
        }
    }
}
