
namespace HotelLibrary.Dtos
{
    public class AddressDto
    {
        public long Id { get; set; }
        public string AddressDetailField { get; set; }
        public int DistrictId { get; set; }
        public DistrictDto District { get; set; }
        public string GoogleLocation { get; set; }
    }
}
