using AutoMapper;
using ContactBook.Infrastructure.Data.Query.Queries.Get;

public class DefaultProfile:Profile
{
       public DefaultProfile()
       {
           CreateMap<Query, Response>()
           .ForMember(dest => dest.Value, opt => opt.MapFrom(src=> src.Value));
       }
}
