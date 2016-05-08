using Abp.Events.Bus.Entities;

namespace EventCloud.Events
{
    class EventCancelledEvent : EntityEventData<Event>
    {
        public EventCancelledEvent(Event entity) : base(entity)
        {
        }
    }
}
