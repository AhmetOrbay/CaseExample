
namespace HotelLibrary.Dtos
{
    public class DistrictDto : BaseEntityDto
    {

        public long CityId { get; set; }
        public CityDto? City { get; set; }
    }
}
