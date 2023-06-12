using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelLibrary.Models
{
    public class HotelImages
    {
        [Key]
        public long Id { get; set; }
        [ForeignKey("Hotel")]
        public long Hotel { get; set; }
        public Hotel Hotels { get; set; }
    }
}
