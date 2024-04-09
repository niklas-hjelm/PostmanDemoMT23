using api.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace api.DataAccess;
public class PeopleContext : DbContext
{
    public DbSet<Person> People { get; set; }

    public PeopleContext(DbContextOptions<PeopleContext> options)
        : base(options)
    {
    }
}