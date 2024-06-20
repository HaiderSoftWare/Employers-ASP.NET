using Microsoft.EntityFrameworkCore;

namespace employers.Model;



class EmployerDb : DbContext
{
    public EmployerDb(DbContextOptions<EmployerDb> options)
        : base(options) { }

    public DbSet<EmploerModel> Articles { get; set; }
}