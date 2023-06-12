using HotelLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelLibrary.Repositories
{
    public class HotelDbContext : DbContext
    {
        public HotelDbContext(DbContextOptions<HotelDbContext> options) : base(options)
        {
        }

        public virtual DbSet<Address> Addresses{ get; set; }
        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<District> Districts { get; set; }
        public virtual DbSet<Hotel> Hotels { get; set; }
        public virtual DbSet<HotelFeature> HotelFeatures { get; set; }
        public virtual DbSet<HotelImages> HotelImages{ get; set; }
        public virtual DbSet<HotelManager> HotelManagers { get; set; }
        public virtual DbSet<HotelContact> HotelContacts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Hotel>().HasQueryFilter(h => !h.IsDelete);

            base.OnModelCreating(modelBuilder);

        }
    }
}
