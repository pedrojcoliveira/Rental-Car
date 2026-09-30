using Microsoft.EntityFrameworkCore;
using Rental_Car_Project.Models;

namespace Rental_Car_Project.Data
{
public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    // Tabelas da base de dados
    public DbSet<Vehicle> Vehicles => Set<Vehicle>();
    public DbSet<Client> Clients => Set<Client>();
    public DbSet<RentalContract> RentalContracts => Set<RentalContract>();


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<Vehicle>(e =>
        {
            // Certifica que não existem veículos com a mesma matrícula
            e.HasIndex(v => v.LicensePlate).IsUnique();
        });

        modelBuilder.Entity<Client>(e =>
        {
            // Certifica que não existem clientes com o mesmo email
            e.HasIndex(c => c.Email).IsUnique();
        });

        modelBuilder.Entity<RentalContract>(e =>
        {
            // A base de dados rejeita contratos de aluguer com datas inválidas (Data de Fim anterior à Data de Início)

            e.ToTable(t => t.HasCheckConstraint("CK_Rental_Dates", "[EndDate] > [StartDate]"));
        });
    }
}
}
