

namespace HotelLibrary.Dtos
{
    public class HotelFeatureDto : BaseEntityDto
    {
        public long HotelId { get; set; }
        public HotelDto Hotels { get; set; }
    }
}
