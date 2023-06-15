using AutoMapper;
using HotelLibrary.Dtos;
using HotelLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelLibrary.Extensions
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Address, AddressDto>()
                .ForMember(dest => dest.District, opt => opt.MapFrom(src => src.District))
                .ReverseMap();
            CreateMap<City, CityDto>()
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Country))
                .ReverseMap();
            CreateMap<Country, CountryDto>()
                .ForMember(dest => dest.Cities, opt => opt.MapFrom(src => src.Cities))
                .ReverseMap();
            CreateMap<District, DistrictDto>()
                 .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City))
                .ReverseMap();
            CreateMap<Hotel, HotelDto>()
                 .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
                 .ForMember(dest => dest.HotelContacts, opt => opt.MapFrom(src => src.HotelContacts))
                 .ForMember(dest => dest.HotelFeatures, opt => opt.MapFrom(src => src.HotelFeatures))
                 .ForMember(dest => dest.HotelImages, opt => opt.MapFrom(src => src.HotelImages))
                 .ForMember(dest => dest.HotelManagers, opt => opt.MapFrom(src => src.HotelManagers))
                .ReverseMap();

            CreateMap<HotelContact, HotelContactDto>()
                 .ForMember(dest => dest.Hotel, opt => opt.MapFrom(src => src.Hotel))
                .ReverseMap();

            CreateMap<HotelFeature, HotelFeatureDto>()
                 .ForMember(dest => dest.Hotels, opt => opt.MapFrom(src => src.Hotels))
                .ReverseMap();

            CreateMap<HotelImages, HotelImagesDto>()
                 .ForMember(dest => dest.Hotels, opt => opt.MapFrom(src => src.Hotels))
                .ReverseMap();

            CreateMap<HotelManager, HotelManagerDto>()
                 .ForMember(dest => dest.Hotels, opt => opt.MapFrom(src => src.Hotels))
                .ReverseMap();


        }
    }
}
