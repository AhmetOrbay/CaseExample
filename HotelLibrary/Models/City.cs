using System.ComponentModel.DataAnnotations.Schema;

namespace HotelLibrary.Models
{
    public class City : BaseEntity
    {

        [ForeignKey("Country")]
        public int CountryId { get; set; }
        public virtual Country Country { get; set; }
    }
}
