using AutoMapper;

namespace Company.Customers.Application.AutoMappers
{
    public class DtoToResponseMappingProfile : Profile
    {
        public DtoToResponseMappingProfile()
        {
            CreateMap<SongQueryDto, SongGetByLocationViewModel>();
        }
    }
}
