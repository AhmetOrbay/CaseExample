using System.ComponentModel.DataAnnotations.Schema;


namespace HotelLibrary.Models
{
    public class District : BaseEntity
    {

        [ForeignKey("City")]
        public int CityId { get; set; }
        public virtual City Country { get; set; }
    }
}
