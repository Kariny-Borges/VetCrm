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
        public DbSet<Especialidade> Especialidades { get; set; }
        public DbSet<Consulta> Consultas { get; set; }
        public DbSet<TipoConsulta> TiposConsulta { get; set; }
        public DbSet<Prontuario> Prontuarios { get; set; }
        public DbSet<Vacina> Vacinas { get; set; }
        public DbSet<PacienteVacina> PacienteVacinas { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Categoria> Categorias { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Proprietario>()
                .HasOne(p => p.Endereco)
                .WithMany()
                .HasForeignKey(p => p.EnderecoId)
                .IsRequired(false);

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

            modelBuilder.Entity<Consulta>()
                .HasOne(c => c.TipoConsulta)
                .WithMany(t => t.Consultas)
                .HasForeignKey(c => c.TipoConsultaId)
                .OnDelete(DeleteBehavior.Restrict);

            // Semeia os tipos de consulta com os MESMOS Ids do antigo enum (1/2/3),
            // pra que as consultas já existentes continuem apontando pro tipo certo.
            modelBuilder.Entity<TipoConsulta>().HasData(
                new TipoConsulta { Id = 1, Nome = "Agendada" },
                new TipoConsulta { Id = 2, Nome = "Emergência" },
                new TipoConsulta { Id = 3, Nome = "Retorno" }
            );

            modelBuilder.Entity<PacienteVacina>()
                .HasOne(pv => pv.Paciente)
                .WithMany(p => p.PacienteVacinas)
                .HasForeignKey(pv => pv.PacienteId)
                .OnDelete(DeleteBehavior.Restrict);

            // Veterinario → Especialidade
            modelBuilder.Entity<Veterinario>()
                .HasOne(v => v.Especialidade)
                .WithMany(e => e.Veterinarios)
                .HasForeignKey(v => v.EspecialidadeId)
                .OnDelete(DeleteBehavior.Restrict);

            // Usuario → Endereco
            modelBuilder.Entity<Usuario>()
                .HasOne(u => u.Endereco)
                .WithMany()
                .HasForeignKey(u => u.EnderecoId)
                .IsRequired(false);

            // Estabelecimento → Endereco
            modelBuilder.Entity<Estabelecimento>()
                .HasOne(e => e.Endereco)
                .WithMany()
                .HasForeignKey(e => e.EnderecoId)
                .IsRequired(false);

            // UsuarioEstabelecimento → Usuario
            modelBuilder.Entity<UsuarioEstabelecimento>()
                .HasOne(ue => ue.Usuario)
                .WithMany(u => u.UsuarioEstabelecimentos)
                .HasForeignKey(ue => ue.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            // UsuarioEstabelecimento → Estabelecimento
            modelBuilder.Entity<UsuarioEstabelecimento>()
                .HasOne(ue => ue.Estabelecimento)
                .WithMany(e => e.UsuarioEstabelecimentos)
                .HasForeignKey(ue => ue.EstabelecimentoId)
                .OnDelete(DeleteBehavior.Restrict);
        }
        public DbSet<Endereco> Enderecos { get; set; }
        public DbSet<VetCrm.Models.Contato> Contato { get; set; } = default!;
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Estabelecimento> Estabelecimentos { get; set; }
        public DbSet<UsuarioEstabelecimento> UsuarioEstabelecimentos { get; set; }
    }
}