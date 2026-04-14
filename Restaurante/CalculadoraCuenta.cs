using System;

namespace Restaurante
{
    // Interfaz para la calculadora de la cuenta del restaurante
    public interface ICalculadoraCuenta
    {
        double CalcularCostoPlatillos(int cantidad, double precioUnitario);
        double CalcularIGV(double subtotal);
        double CalcularPropina(double total, double porcentaje);
        double AplicarDescuento(double total, double descuento);
    }

    // Clase básica para calcular partes de la cuenta
    public class CalculadoraCuenta : ICalculadoraCuenta
    {
        // 1. Método para multiplicar cantidad de platos por su precio
        public double CalcularCostoPlatillos(int cantidad, double precioUnitario)
        {
            return cantidad * precioUnitario;
        }

        // 2. Método para calcular el 18% de impuestos (IGV)
        public double CalcularIGV(double subtotal)
        {
            return subtotal * 0.18;
        }

        // 3. Método para calcular la propina basada en un porcentaje
        public double CalcularPropina(double total, double porcentaje)
        {
            return total * (porcentaje / 100);
        }

        // 4. Método para descontar un monto fijo al total
        public double AplicarDescuento(double total, double descuento)
        {
            if (descuento > total)
            {
                throw new ArgumentException("El descuento no puede ser mayor al total de la cuenta.");
            }
            if (descuento < 0)
            {
                throw new ArgumentException("El descuento no puede ser negativo.");
            }
            return total - descuento;
        }
    }
}