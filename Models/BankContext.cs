using Microsoft.EntityFrameworkCore;
using Bank.Models;

namespace Bank.Models
{
    public class BankContext : DbContext
    {
        public DbSet<User> User { get; set; }
        public DbSet<Transaction> Transaction { get; set;}

        // base() calls the parent class' constructor passing the "options" parameter along
        public BankContext(DbContextOptions<BankContext> options) : base(options) { }
    }
}