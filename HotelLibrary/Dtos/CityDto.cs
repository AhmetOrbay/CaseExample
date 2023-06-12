
namespace HotelLibrary.Dtos
{
    public class CityDto : BaseEntityDto
    {

        public int CountryId { get; set; }
        public virtual CountryDto Country { get; set; }
    }
}
