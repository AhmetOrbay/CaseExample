
namespace HotelLibrary.Dtos
{
    public class CityDto : BaseEntityDto
    {

        public long CountryId { get; set; }
        public CountryDto? Country { get; set; }
    }
}
