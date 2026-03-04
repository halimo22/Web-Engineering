using Microsoft.EntityFrameworkCore;
using WebApplication3.Models;
namespace WebApplication3.Database;


public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    public DbSet<Book> Books { get; set; } 
    


}