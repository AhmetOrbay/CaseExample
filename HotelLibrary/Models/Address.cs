using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelLibrary.Models
{
    public class Address
    {
        public long Id { get; set; }
        [MaxLength(100)]
        public string AddressDetailField { get; set; }
        [ForeignKey("District")]
        public int DistrictId { get; set; }
        public District District { get; set; }
        public string GoogleLocation { get; set; }
    }
}
