using System;
using System.Collections.Generic;


namespace CapaDeDatos
{
    public class RepositorioFlexSpace
    {
        public enum TipoCliente { Estandar, VIP }
        public enum TipoPuesto { EscritorioIndividual, SalaReuniones, CabinaPrivada }
        public enum EstadoReserva { Confirmada, Cancelada, Finalizada }

        public class Cliente
        {
            public int Id { get; set; }
            public string Nombre { get; set; }
            public string Email { get; set; }
            public TipoCliente Tipo { get; set; }
            public int SancionesActivas { get; set; }
        }

        public class Puesto
        {
            public int Id { get; set; }
            public string Codigo { get; set; }
            public TipoPuesto Tipo { get; set; }
            public decimal TarifaBasePorHora { get; set; }
        }

        public class Reserva
        {
            public int Id { get; set; }
            public int ClienteId { get; set; }
            public int PuestoId { get; set; }
            public DateTime FechaInicio { get; set; }
            public DateTime FechaFin { get; set; }
            public EstadoReserva Estado { get; set; }
            public decimal CostoTotal { get; set; }
        }
       
        private static List<Cliente> _clientes = new List<Cliente>
        {
            new Cliente { Id = 1, Nombre = "Ana Pérez", Email = "ana@mail.com", Tipo = TipoCliente.VIP, SancionesActivas = 0 },
            new Cliente { Id = 2, Nombre = "Carlos Gómez", Email = "carlos@mail.com", Tipo = TipoCliente.Estandar, SancionesActivas = 3 }
        };

        private static List<Puesto> _puestos = new List<Puesto>
        {
            new Puesto { Id = 1, Codigo = "P01", Tipo = TipoPuesto.EscritorioIndividual, TarifaBasePorHora = 1000m },
            new Puesto { Id = 2, Codigo = "P02", Tipo = TipoPuesto.SalaReuniones, TarifaBasePorHora = 2500m }
        };

        private static List<Reserva> _reservas = new List<Reserva>();

       
        public Cliente ObtenerClientePorId(int id)
        {
            return _clientes.Find(c => c.Id == id);
        }

        
        public Puesto ObtenerPuestoPorId(int id)
        {
            return _puestos.Find(p => p.Id == id);
        }

      


        public List<Reserva> ObtenerReservasPorPuesto(string codigoPuesto)
        {
            var puesto = _puestos.Find(p => p.Codigo == codigoPuesto);
            if (puesto == null) return new List<Reserva>();

            return _reservas.FindAll(r => r.PuestoId == puesto.Id && r.Estado == EstadoReserva.Confirmada);
        }

      
        public void GuardarReserva(Reserva reserva)
        {
            reserva.Id = _reservas.Count + 1;
            _reservas.Add(reserva);
        }

        
        public void ActualizarSanciones(int clienteId, int nuevasSanciones)
        {
            var cliente = ObtenerClientePorId(clienteId);
            if (cliente != null)
            {
                cliente.SancionesActivas = nuevasSanciones;
            }
        }
    }
}