using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Repositories;
using Abp.UI;
using EventCloud.Users;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;

namespace EventCloud.Events
{
    [Table("AppEventRegistrations")]
    public class EventRegistration : CreationAuditedEntity, IMustHaveTenant
    {
        public int TenantId { get; set; }

        [ForeignKey("EventId")]
        public virtual Event Event { get; protected set; }
        public virtual Guid EventId { get; protected set; }

        [ForeignKey("UserId")]
        public virtual User User { get; protected set; }
        public virtual long UserId { get; protected set; }

        protected EventRegistration()
        {

        }

        public async static Task<EventRegistration> CreateAsync(Event @event, User user, IEventRegistrationPolicy registrationPolicy)
        {
            await registrationPolicy.CheckRegistrationAttemptASync(@event, user);

            return new EventRegistration
            {
                TenantId = @event.TenantId,
                EventId = @event.Id,
                Event = @event,
                UserId = user.Id,
                User = user
            };

        }

        public async Task CancelAsync(IRepository<EventRegistration> repository)
        {
            if(repository == null)
            {
                throw new ArgumentNullException("Repository");
            }

            if(Event.IsInPast())
            {
                throw new UserFriendlyException("Can not cancel event which is in the past!");
            }

            if (Event.isAllowedCancellationTimeEnded())
            {
                throw new UserFriendlyException("It's too late to cancel your registration!");
            }

            await repository.DeleteAsync(this);
        }
    }
}
