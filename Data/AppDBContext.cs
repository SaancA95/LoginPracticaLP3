
using Microsoft.EntityFrameworkCore;
using PracticasLP3.Models;


namespace PracticasLP3.Data;
public class AppDBContext : DbContext
{
    public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
    {

    }
    public DbSet<Usuario> Usuarios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);



        modelBuilder.Entity<Usuario>(tb =>
        {
            tb.HasKey(col => col.Id);
            tb.Property(col => col.Id)
            .UseIdentityColumn()
            .ValueGeneratedOnAdd();

            tb.Property(col => col.Nombre).HasMaxLength(50);
            tb.Property(col => col.Correo).HasMaxLength(50);
            tb.Property(col => col.Clave).HasMaxLength(50);

        });

        modelBuilder.Entity<Usuario>().ToTable("Usuario");







    }


}










