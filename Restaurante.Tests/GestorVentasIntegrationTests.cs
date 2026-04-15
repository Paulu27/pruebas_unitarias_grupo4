using NUnit.Framework;
using Restaurante;

namespace Restaurante.Tests
{
    public class GestorVentasIntegrationTests
    {
        private ICalculadoraCuenta _calculadoraReal;
        private GestorVentas _gestorVentas;

        [SetUp]
        public void Setup()
        {
            // Instanciamos las clases reales (Sin Mocks)
            _calculadoraReal = new CalculadoraCuenta();
            _gestorVentas = new GestorVentas(_calculadoraReal);
        }

        // ¡Aquí inyectamos los datos!
        // Parámetros: (cantidadPlatos, precioPlato, propina, totalEsperado)
        
        [TestCase(2, 50.0, 10.0, 128.0)]  // Caso 1: 100 de platos + 18 IGV + 10 propina = 128
        [TestCase(1, 20.0, 5.0, 24.6)]    // Caso 2: 20 de platos + 3.6 IGV + 1 propina = 24.6
        [TestCase(4, 35.0, 15.0, 186.2)]  // Caso 3: 140 de platos + 25.2 IGV + 21 propina = 186.2
        public void CalcularTotalMesa_VariosDatos_RetornaTotalCorrecto(int cant, double precio, double propina, double totalEsperado)
        {
            // Arrange (Los datos ya vienen en los parámetros del método por el TestCase)

            // Act
            double resultado = _gestorVentas.CalcularTotalMesa(cant, precio, propina);

            // Assert
            Assert.That(resultado, Is.EqualTo(totalEsperado), 
                $"El cálculo falló para {cant} platos de {precio} con {propina}% de propina.");
        }
    }
}