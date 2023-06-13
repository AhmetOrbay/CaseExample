using System.ComponentModel.DataAnnotations.Schema;

namespace HotelLibrary.Models
{
    public class HotelFeature :BaseEntity
    {
        [ForeignKey("Hotel")]
        public long HotelId { get; set; }
        public Hotel Hotels { get; set; }
        public bool IsDelete { get; set; } = false;
    }
}
