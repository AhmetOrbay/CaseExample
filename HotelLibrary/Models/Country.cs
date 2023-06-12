using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelLibrary.Models
{
    public class Country : BaseEntity
    {
        public string IsoCode { get; set; }
        public virtual ICollection<City> Cities  { get; set; }
    }
}
