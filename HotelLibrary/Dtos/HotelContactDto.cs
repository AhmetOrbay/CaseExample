

namespace HotelLibrary.Models
{
    public class HotelContactDto
    {
        public long Id { get; set; }
        public string TelephoneNumber { get; set; }
        public string HotelEmail { get; set; }
        public DateTime COntactCreatedDate { get; set; }
        public long HotelId { get; set; }
        public Hotel Hotel { get; set; }
    }
}
