

namespace HotelLibrary.Models
{
    public class Country : BaseEntity
    {
        public string IsoCode { get; set; }
        public virtual ICollection<City> Cities  { get; set; }
    }
}
