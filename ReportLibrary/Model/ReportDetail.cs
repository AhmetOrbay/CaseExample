using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportLibrary.Model
{
    public class ReportDetail
    {
        [Key]
        public long Id { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string District { get; set; }

        public string GoogleLocation { get; set; }
        public string AddressDetailField { get; set; }
        public int LocationHotelCount { get; set; }
        public int LocationTelephoneCount { get; set; }
        public long ReportId { get; set; }
    }
}
