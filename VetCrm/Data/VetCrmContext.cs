using Microsoft.EntityFrameworkCore;
using VetCrm.Models;

namespace VetCrm.Data
{
    public class VetCrmContext : DbContext
    {
        public VetCrmContext(DbContextOptions<VetCrmContext> options) : base(options) { }

        public DbSet<Proprietario> Proprietarios { get; set; }
        public DbSet<Paciente> Pacientes { get; set; }
        public DbSet<Especie> Especies { get; set; }
        public DbSet<Raca> Racas { get; set; }
        public DbSet<Veterinario> Veterinarios { get; set; }
        public DbSet<Consulta> Consultas { get; set; }
        public DbSet<Prontuario> Prontuarios { get; set; }
        public DbSet<Vacina> Vacinas { get; set; }
        public DbSet<PacienteVacina> PacienteVacinas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Paciente>()
                .HasOne(p => p.Raca)
                .WithMany()
                .HasForeignKey(p => p.RacaId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Paciente>()
                .HasOne(p => p.Especie)
                .WithMany()
                .HasForeignKey(p => p.EspecieId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Prontuario>()
                .HasOne(p => p.Paciente)
                .WithMany()
                .HasForeignKey(p => p.PacienteId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Prontuario>()
                .HasOne(p => p.Consulta)
                .WithOne(c => c.Prontuario)
                .HasForeignKey<Prontuario>(p => p.ConsultaId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Consulta>()
                .HasOne(c => c.Paciente)
                .WithMany(p => p.Consultas)
                .HasForeignKey(c => c.PacienteId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PacienteVacina>()
                .HasOne(pv => pv.Paciente)
                .WithMany(p => p.PacienteVacinas)
                .HasForeignKey(pv => pv.PacienteId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}