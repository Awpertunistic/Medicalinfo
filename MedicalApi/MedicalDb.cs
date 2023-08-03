using Microsoft.EntityFrameworkCore;

class MedicalDb : DbContext
{
    public MedicalDb(DbContextOptions<MedicalDb> options)
        : base(options) { }

    public DbSet<Medical> Medicals => Set<Medical>();
}