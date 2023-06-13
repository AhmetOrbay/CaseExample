

namespace HotelLibrary.Dtos
{
    public class HotelManagerDto : BaseEntityDto
    {
        public string SurName { get; set; }
        public string TelephoneNumber { get; set; }
        public string Email { get; set; }
        public long HotelId { get; set; }
        public  HotelDto? Hotels { get; set; }
        public DateTime ManagerCreatedDate { get; set; }
    }
}
