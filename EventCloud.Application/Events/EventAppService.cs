using System;
using System.Data.Entity;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using EventCloud.Events.Dtos;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using System.Linq;
using System.Collections.Generic;
using Abp.AutoMapper;

namespace EventCloud.Events
{

    [AbpAuthorize]
    public class EventAppService : EventCloudAppServiceBase, IEventAppService
    {
        private readonly IEventManager _eventManager;
        private readonly IRepository<Event, Guid> _eventRepository;

        public EventAppService(IEventManager eventManager, IRepository<Event, Guid> eventRepository)
        {
            _eventManager = eventManager;
            _eventRepository = eventRepository;
        }

        public async Task<ListResultOutput<EventListDto>> GetList(GetEventListInput input)
        {
            var events = await _eventRepository
                .GetAll()
                .Include(e => e.Registrations)
                .WhereIf(!input.IncludeCanceledEvents, e => !e.IsCancelled)
                .OrderByDescending(e => e.CreationTime)
                .ToListAsync();

            return new ListResultOutput<EventListDto>(events.MapTo<List<EventListDto>>());
        }

        public Task Cancel(EntityRequestInput<Guid> input)
        {
            throw new NotImplementedException();
        }

        public Task CancelRegistration(EntityRequestInput<Guid> input)
        {
            throw new NotImplementedException();
        }

        public Task Create(CreateEventInput input)
        {
            throw new NotImplementedException();
        }

        public Task<EventDetailOutput> GetDetail(EntityRequestInput<Guid> input)
        {
            throw new NotImplementedException();
        }

        
        public Task<EventRegisterOutput> Register(EntityRequestInput<Guid> input)
        {
            throw new NotImplementedException();
        }
    }
}
