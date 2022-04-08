namespace UserMgmt.Helpers;
using Microsoft.EntityFrameworkCore;

public class DataContext : DbContext
{
    public DbSet<User> Users { get; set; }

    public DataContext(DbContextOptions<DataContext> options)
            : base(options)
    {
    }

    //protected override void OnConfiguring(DbContextOptionsBuilder options)
    //{
    //    // in memory database used for simplicity, change to a real db for production applications
    //    options.user("UserDB");
    //}
}