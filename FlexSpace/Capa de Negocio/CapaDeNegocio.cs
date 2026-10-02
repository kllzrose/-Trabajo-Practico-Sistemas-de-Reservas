using System;
using System.Collections.Generic;
using CapaDeDatos;
namespace CapaDeNegocio
{

    public class ClienteSancionadoException : Exception
    {
        public ClienteSancionadoException(string message) : base(message) { }
    }

  
    public class ReservaService
    {
       
        public decimal CalcularTarifaTotal(Cliente cliente, Puesto puesto, DateTime fechaInicio, DateTime fechaFin)
        {
            if (cliente.SancionesActivas >= 3)
            {
                throw new ClienteSancionadoException($"El cliente {cliente.Nombre} tiene 3 o más sanciones y está bloqueado.");
            }

            double horasReservadas = (fechaFin - fechaInicio).TotalHours;
            if (horasReservadas <= 0) throw new ArgumentException("La fecha de fin debe ser posterior a la de inicio.");

            decimal subtotalBase = (decimal)horasReservadas * puesto.TarifaBasePorHora;
            decimal costoActual = subtotalBase;

 
            if (cliente.SancionesActivas > 0)
            {
                return Math.Round(subtotalBase * 1.20m, 2);
            }

           
            if (ContieneFinDeSemana(fechaInicio, fechaFin))
            {
                costoActual += subtotalBase * 0.15m;
            }

          
            if (horasReservadas >= 5)
            {
                costoActual -= costoActual * 0.10m;
            }

         
            if (cliente.Tipo == TipoCliente.VIP)
            {
                costoActual -= costoActual * 0.05m;
            }

            return Math.Round(costoActual, 2);
        }


        public bool ValidarSolapamiento(List<Reserva> reservasExistentes, int puestoId, DateTime nuevaInicio, DateTime nuevaFin)
        {
            foreach (var reserva in reservasExistentes)
            {
                if (reserva.PuestoId == puestoId && reserva.Estado == EstadoReserva.Confirmada)
                {
                    if (nuevaInicio < reserva.FechaFin && nuevaFin > reserva.FechaInicio)
                    {
                        return false; 
                    }
                }
            }
            return true; 
        }

        private bool ContieneFinDeSemana(DateTime inicio, DateTime fin)
        {
            for (DateTime fecha = inicio.Date; fecha <= fin.Date; fecha = fecha.AddDays(1))
            {
                if (fecha.DayOfWeek == DayOfWeek.Saturday || fecha.DayOfWeek == DayOfWeek.Sunday) return true;
            }
            return false;
        }
    }
}