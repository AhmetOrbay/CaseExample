
namespace HotelLibrary.Dtos
{
    public class DistrictDto : BaseEntityDto
    {

        public int CityId { get; set; }
        public virtual CityDto Country { get; set; }
    }
}
