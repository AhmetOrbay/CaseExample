using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelLibrary.Models
{
    public class Address
    {
        [Key]
        public long Id { get; set; }
        [MaxLength(100)]
        public string AddressDetailField { get; set; }
        [ForeignKey("District")]
        public long DistrictId { get; set; }
        public District District { get; set; }
        public string GoogleLocation { get; set; }
    }
}
