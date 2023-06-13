

using HotelLibrary.Dtos;
using HotelLibrary.Models;
using HotelLibrary.Models.Enums;

namespace HotelLibrary.Dtos
{
    public class HotelDto : BaseEntityDto
    {
        public long AddressId { get; set; }
        public AddressDto? Address { get; set; }
        public decimal HotelPoint { get; set; }
        public HotelStatus HotelStatus { get; set; }
        public bool IsDelete { get; set; }
        public decimal OneDayPrice { get; set; }
        public RoomType RoomType { get; set;}
        public List<HotelImagesDto>? HotelImages { get; set; }

        public List<HotelFeatureDto>? HotelFeatures { get; set; }
        public List<HotelContactDto>? HotelContacts { get; set; }
        public string? WebSite { get; set; }
        public string OtelInformation { get; set; }
        public DateTime HotelCreatedDate { get; set; }
        public List<HotelManagerDto>? HotelManagers { get; set; }
    }
}
