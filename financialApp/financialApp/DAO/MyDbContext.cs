using financialApp.DAO.Models;
using Microsoft.EntityFrameworkCore;

namespace financialApp.DAO;

public class MyDbContext : DbContext
{
    public DbSet<Category> Categories { get; set; }
    public DbSet<Entry> Entries { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Wallet> Wallets { get; set; }

#pragma warning disable CS8618
    public MyDbContext(DbContextOptions options) : base(options)
#pragma warning restore CS8618
    {

    }
}
