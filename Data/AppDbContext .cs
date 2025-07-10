using ConsultaPopularApi.Models;
using Microsoft.EntityFrameworkCore;

namespace ConsultaPopularApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Clinica> Clinicas { get; set; }
        public DbSet<Consulta> Consultas { get; set; }
        public DbSet<Paciente> Pacientes { get; set; }

        public DbSet<ClinicaEspecialidade> ClinicaEspecialidades { get; set; }
        public DbSet<Especialidade> Especialidades { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Clinica → Endereco
            modelBuilder.Entity<Clinica>()
                .OwnsOne(c => c.Endereco);

            // Clinica → ClinicaEspecialidades
            modelBuilder.Entity<Clinica>()
                .HasMany(c => c.ClinicaEspecialidades)
                .WithOne(ce => ce.Clinica)
                .HasForeignKey(ce => ce.ClinicaId)
                .OnDelete(DeleteBehavior.Cascade);

            // ClinicaEspecialidade → Consulta
            modelBuilder.Entity<ClinicaEspecialidade>()
                .HasMany(ce => ce.Consultas)
                .WithOne(c => c.ClinicaEspecialidade)
                .HasForeignKey(c => c.ClinicaEspecialidadeId)
                .OnDelete(DeleteBehavior.Cascade);

            // Especialidade → ClinicaEspecialidade
            modelBuilder.Entity<Especialidade>() 
                .HasMany(e => e.ClinicaEspecialidades)
                .WithOne(ce => ce.Especialidade)
                .HasForeignKey(ce => ce.EspecialidadeId)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }
    }
}
