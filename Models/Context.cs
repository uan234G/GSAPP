using Microsoft.EntityFrameworkCore;

namespace GSAPP.Models
{
    public class MyContext : DbContext
    {
        // base() calls the parent class' constructor passing the "options" parameter along
        public MyContext(DbContextOptions options) : base(options) { }

        public DbSet<User> Users { get; set; }
<<<<<<< HEAD
        public DbSet<Request> Request { get; set; }
=======
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Request> Requests { get; set; }
>>>>>>> 1c6320479f7930dfbc60ce74e26ce661c35d3dd2

    }
}