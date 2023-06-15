using System.ComponentModel.DataAnnotations.Schema;

namespace HotelLibrary.Models
{
    public class City : BaseEntity
    {

        [ForeignKey("Country")]
        public long CountryId { get; set; }
        public virtual Country Country { get; set; }
        public ICollection<District> Districts { get; set; }
    }
}
