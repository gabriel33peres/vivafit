using Microsoft.EntityFrameworkCore;
using VivaFit.API.Models;

namespace VivaFit.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Aluno> Alunos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Plano> Planos { get; set; }
        public DbSet<Matricula> Matriculas { get; set; }
        public DbSet<Professor> Professores { get; set; }
        public DbSet<Pagamento> Pagamentos { get; set; }
        public DbSet<Treino> Treinos { get; set; }
        public DbSet<Avaliacao> Avaliacoes { get; set; }
        public DbSet<Frequencia> Frequencias { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

        // E-mail de usuário deve ser único
        modelBuilder.Entity<Usuario>()
            .HasIndex(u => u.Email)
            .IsUnique();

        // Um aluno pode ter várias matrículas
        modelBuilder.Entity<Matricula>()
            .HasOne<Aluno>()
            .WithMany()
            .HasForeignKey(m => m.AlunoId)
            .OnDelete(DeleteBehavior.Restrict);

        // Um aluno pode ter várias frequências
        modelBuilder.Entity<Frequencia>()
            .HasOne<Aluno>()
            .WithMany()
            .HasForeignKey(f => f.AlunoId)
            .OnDelete(DeleteBehavior.Restrict);
        // Um plano pode ter várias matrículas
        modelBuilder.Entity<Matricula>()
            .HasOne<Plano>()
            .WithMany()
            .HasForeignKey(m => m.PlanoId)
            .OnDelete(DeleteBehavior.Restrict);
        // Uma matrícula pode ter vários pagamentos
        modelBuilder.Entity<Pagamento>()
            .HasOne<Matricula>()
            .WithMany()
            .HasForeignKey(p => p.MatriculaId)
            .OnDelete(DeleteBehavior.Restrict);
        // Um aluno pode ter vários treinos
        modelBuilder.Entity<Treino>()
            .HasOne<Aluno>()
            .WithMany()
            .HasForeignKey(t => t.AlunoId)
            .OnDelete(DeleteBehavior.Restrict);
        // Um professor pode ter vários treinos
        modelBuilder.Entity<Treino>()
            .HasOne<Professor>()
            .WithMany()
            .HasForeignKey(t => t.ProfessorId)
            .OnDelete(DeleteBehavior.Restrict);
        // Um professor pode realizar várias avaliações
        modelBuilder.Entity<Avaliacao>()
            .HasOne<Professor>()
            .WithMany()
            .HasForeignKey(a => a.ProfessorId)
            .OnDelete(DeleteBehavior.Restrict);

        }
    }
}