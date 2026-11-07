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
                HabitacionId = "101",
                TipoHabitacion = "Start Vista al Mar",
                TarifaPorNoche = 150, // O 'PrecioNoche' según cómo se llame en tu modelo Habitación
                TieneTvSatelital = true,
                PendientesMantenimiento = "Ninguno"
            });

            Clientes.Add(new Cliente
            {
                TipoIdentificacion = "Cédula",
                ClienteId = "1-1111-1111",
                NombreCliente = "Juan",
                PrimerApellidoCliente = "Pérez",
                SegundoApellidoCliente = "Rojas",
                FechaNacimientoCliente = new DateTime(1990, 5, 20)
            });

            Empleados.Add(new Empleado
            {
                TipoIdentificacion = "Cédula",
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
                ReservacionId = "RES-0001", 
                ClienteId = "1-1111-1111",  
                HabitacionId = "101",   
                FechaRegistro = DateTime.Now.AddDays(-1),
                FechaInicio = DateTime.Now.AddDays(1), 
                FechaSalida = DateTime.Now.AddDays(4), 
                CantidadPersonas = 2,
                SolicitudesEspeciales = "Ninguna.",
                PorcentajeDescuento = 10, 
                TarifaReservacion = 150, 
                Estado = "Reservada"
            });
        }
    }
}