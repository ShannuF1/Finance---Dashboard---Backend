public class FinanceDbContext : DbContext
{
    public FinanceDbContext(DbContextOptions<FinanceDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<FinancialRecord> FinancialRecords { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Ensure data integrity (Requirement 6)
        modelBuilder.Entity<FinancialRecord>().Property(r => r.Amount).HasPrecision(18, 2);
    }
}
