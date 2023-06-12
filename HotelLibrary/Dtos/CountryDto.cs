
namespace HotelLibrary.Dtos
{
    public class CountryDto : BaseEntityDto
    {
        public string IsoCode { get; set; }
        public virtual ICollection<CityDto> Cities  { get; set; }
    }
}
