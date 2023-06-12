using HotelLibrary.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelLibrary.Models
{
    public class Hotel : BaseEntity
    {
        [ForeignKey("Address")]
        public long AddressId { get; set; }
        public Address Address { get; set; }
        public decimal HotelPoint { get; set; }
        public HotelStatus HotelStatus { get; set; }
        public bool IsDelete { get; set; }
        public decimal OneDayPrice { get; set; }
        public RoomType RoomType { get; set;}
        public ICollection<HotelImages> HotelImages { get; set; }

        public ICollection<HotelFeature> HotelFeatures { get; set; }
        public ICollection<HotelContact> HotelContacts { get; set; }
        
        [MaxLength(50)]
        public string? WebSite { get; set; }
        [MaxLength(300)]
        public string OtelInformation { get; set; }
        public DateTime HotelCreatedDate { get; set; }
        public ICollection<HotelManager> HotelManagers { get; set; }
    }
}
