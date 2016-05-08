using Abp.Events.Bus.Entities;

namespace EventCloud.Events
{
    class EventDateChangeEvent : EntityEventData<Event>
    {
        public EventDateChangeEvent(Event entity)
            : base(entity)
        {

        }
    }
}
