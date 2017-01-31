using System.Collections.Generic;
using AutoMapper;

namespace Model.Helpers
{
    public static class MapperHelper
    {
        public static void InitializeMaps()
        {
            Mapper.Initialize(x =>
            {
                /* examples
                    x.CreateMap<Ticket, TicketVo>();
                    x.CreateMap<TicketVo, Ticket>();
                
                    x.CreateMap<EventVo, Event>()
                        .ForMember(dest => dest.BuildingId, opts => opts.MapFrom(src => src.Building.Id));
                */
            });
        }
    }
}
