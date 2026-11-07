using HospedajeAPI.Models;

namespace HospedajeAPI.Services
{
    public class MemoryDatabase
    {
        // Colecciones globales en memoria 
        public static List<Cliente> Clientes { get; set; } = new List<Cliente>();
        public static List<Empleado> Empleados { get; set; } = new List<Empleado>();
        public static List<Habitacion> Habitaciones { get; set; } = new List<Habitacion>();
        public static List<Reservacion> Reservaciones { get; set; } = new List<Reservacion>();

        // Bloque estático para precargar datos iniciales 
        static MemoryDatabase()
        {
            PrecargarDatos();
        }

        private static void PrecargarDatos()
        {
            Habitaciones.Add(new Habitacion
            {
                HabitacionId = "001",
                TipoHabitacion = "Start Vista al Mar",
                TarifaPorNoche = 100,
                TieneTvSatelital = false,
                PendientesMantenimiento = "Ninguno"
            });

            Clientes.Add(new Cliente
            {
                ClienteId = "1-0000-0000",
                NombreCliente = "Pedro",
                PrimerApellidoCliente = "López",
                SegundoApellidoCliente = "Osorio",
                FechaNacimientoCliente = new DateTime(1980, 6, 20)
            });

            Empleados.Add(new Empleado
            {
                EmpleadoId = "1-2222-2222",
                NombreEmpleado = "Marta",
                PrimerApellidoEmpleado = "Salazar",
                SegundoApellidoEmpleado = "Rojas",
                FechaNacimientoEmpleado = new DateTime(1990, 5, 20),
                Categoria = "Limpieza",
                SalarioMensual = 350000,
                FechaIngreso = new DateTime(2005, 3, 12),
                Provincia = "San José",
                Canton = "Escazú",
                Distrito = "San Antonio",
                DireccionExacta = "-",
            });

            Reservaciones.Add(new Reservacion
            {
                ReservacionId = "001",
                ClienteId = "0-0000-0000",
                HabitacionId = "101",
                FechaIngreso = DateTime.Now,
                FechaSalida = DateTime.Now,
                CantidadPersonas = 0,
                Estado = "Reservada"
            });
        }
    }
}