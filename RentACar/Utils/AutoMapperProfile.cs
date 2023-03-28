using AutoMapper;
using RentACar.Models.Db;
using RentACar.Models.Dto;

namespace RentACar.Utils;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        
        // DTO => Entity
        CreateMap<CustomerDTO, CustomerEntity>()
            .ForMember(dest => dest.RegistrationDate,
                opt => opt.MapFrom(src => DateTime.UtcNow));
        CreateMap<CarDTO, CarEntity>();
        CreateMap<ReservationDTO, ReservationEntity>();
        
        // Entity => DTO
        CreateMap<CustomerEntity, CustomerDTO>();
        CreateMap<CarEntity, CarDTO>();
        CreateMap<ReservationEntity, ReservationDTO>();
        
    }
}