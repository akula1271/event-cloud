using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Abp.Timing;
using Abp.UI;
using EventCloud.Domain.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventCloud.Events
{
    [Table("AppEvents")]
    public class Event : FullAuditedEntity<Guid>, IMustHaveTenant
    {
        public const int MaxTitleLength = 128;
        public const int MaxDescriptionLength = 2048;

        public virtual int TenantId { get; set; }

        [Required]
        [StringLength(MaxTitleLength)]
        public virtual string Title { get; protected set; }

        [StringLength(MaxDescriptionLength)]
        public virtual string Description { get; protected set; }

        public virtual DateTime Date { get; protected set; }
        
        public virtual bool IsCancelled { get; protected set; }

        [Range(0, int.MaxValue)]
        public virtual int MaxRegistrationCount { get; protected set; }

        [ForeignKey("EventId")]
        public virtual ICollection<EventRegistration> Registrations { get; protected set; }

        public bool IsInPast()
        {
            return Date < Clock.Now;
        }

        protected Event() { }

        public static Event create(int tenantId, string title, DateTime date, string description, int maxRegistrationCount = 0)
        {
            var @event = new Event
            {
                Id = Guid.NewGuid(),
                TenantId = tenantId,
                Title = title,
                Description = description,
                MaxRegistrationCount = maxRegistrationCount
            };

            @event.SetDate(date);

            @event.Registrations = new Collection<EventRegistration>();

            return @event;
        }

        public bool isAllowedCancellationTimeEnded()
        {
            return Date.Subtract(Clock.Now).TotalHours <= 2.0; 
        }

        private void SetDate(DateTime date)
        {
            AssertNotCancelled();

            if (date < Clock.Now)
            {
                throw new UserFriendlyException("Can not set an event's date in the past");
            }
            if (date <= Clock.Now.AddHours(3)){
                throw new UserFriendlyException("Should set an event's date 3 hours before atleast.");
            }

            Date = date;
            DomainEvents.EventBus.Trigger(new EventDateChangeEvent(this));
        }


        internal void Cancel()
        {
            AssertNotInPast();
            IsCancelled = true;

        }

        private void AssertNotInPast()
        {
            if (IsInPast()) {
                throw new UserFriendlyException("This event was in the past");
            }

        }

        private void AssertNotCancelled()
        {
            if(IsCancelled) {
                throw new UserFriendlyException("This event was cancelled");
            }
        }
    }
}
