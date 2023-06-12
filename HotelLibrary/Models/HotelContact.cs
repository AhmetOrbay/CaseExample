using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelLibrary.Models
{
    public class HotelContact
    {
        [Key]
        public long Id { get; set; }
        [RegularExpression(@"^\+?[0-9]{1,3}-?[0-9]{3}-?[0-9]{3}-?[0-9]{4}$", ErrorMessage = "Invalid phone number")]
        public string TelephoneNumber { get; set; }
        public string HotelEmail { get; set; }
        public DateTime COntactCreatedDate { get; set; }
        [ForeignKey("Hotel")]
        public long HotelId { get; set; }
        public Hotel Hotel { get; set; }
    }
}
