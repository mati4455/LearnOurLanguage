using System.Collections.Generic;
using AutoMapper;
using Model.Models.Account;
using Model.Models.Database;

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
                x.CreateMap<User, AppUserVo>()
                    .ForMember(dest => dest.AccessLevel, opts => opts.MapFrom(src => src.Role.AccessLevel))
                    .ForMember(dest => dest.RoleName, opts => opts.MapFrom(src => src.Role.Name));
            });
        }
    }
}
