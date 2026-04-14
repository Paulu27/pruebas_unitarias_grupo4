namespace Restaurante
{
    // Clase que utiliza la calculadora para operaciones de caja más complejas
    public class GestorVentas
    {
        private readonly ICalculadoraCuenta _calculadora;

        public GestorVentas(ICalculadoraCuenta calculadora)
        {
            _calculadora = calculadora;
        }

        // 5. Método avanzado: Calcula todo lo que debe pagar una mesa
        public double CalcularTotalMesa(int cantidadPlatos, double precioPlato, double porcentajePropina)
        {
            double subtotal = _calculadora.CalcularCostoPlatillos(cantidadPlatos, precioPlato);
            double igv = _calculadora.CalcularIGV(subtotal);
            
            // La propina la calculamos sobre el subtotal sin IGV (por ejemplo)
            double propina = _calculadora.CalcularPropina(subtotal, porcentajePropina);
            
            return subtotal + igv + propina;
        }

        // 6. Método avanzado: Cierra la cuenta aplicando un cupón de descuento
        public double CobrarConCupon(double totalMesa, double valorCupon)
        {
            return _calculadora.AplicarDescuento(totalMesa, valorCupon);
        }
    }
}