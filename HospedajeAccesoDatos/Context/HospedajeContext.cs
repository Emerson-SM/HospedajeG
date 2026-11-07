using HospedajeAccesoDatos.Models;
using Microsoft.EntityFrameworkCore;

namespace HospedajeAccesoDatos.Context
{
    public class HospedajeContext : DbContext
    {
        public HospedajeContext(DbContextOptions<HospedajeContext> options)
            : base(options)
        {
        }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Empleado> Empleados { get; set; }
        public DbSet<Habitacion> Habitaciones { get; set; }
        public DbSet<Reservacion> Reservaciones { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Cliente>().ToTable("Cliente");
            modelBuilder.Entity<Empleado>().ToTable("Empleado");
            modelBuilder.Entity<Habitacion>().ToTable("Habitacion");
            modelBuilder.Entity<Reservacion>().ToTable("Reservacion");
        }
    }
}