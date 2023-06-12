using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelLibrary.Models
{
    public class District : BaseEntity
    {

        [ForeignKey("City")]
        public int CityId { get; set; }
        public virtual City Country { get; set; }
    }
}
