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
            // 1. Instanciamos la clase REAL (¡Cero Mocks aquí!)
            _calculadoraReal = new CalculadoraCuenta();
            
            // 2. Inyectamos la dependencia real al gestor
            _gestorVentas = new GestorVentas(_calculadoraReal);
        }

        [Test]
        public void CalcularTotalMesa_UsandoCalculadoraReal_RetornaTotalCorrecto()
        {
            // Arrange
            int cantidadPlatos = 2;
            double precioPlato = 50.0;     // El subtotal será 100.0
            double porcentajePropina = 10; // La propina será 10.0
                                           // El IGV real calculado será 18.0

            // Act
            // Esta llamada viajará desde el Gestor hacia la Calculadora real y regresará
            double resultado = _gestorVentas.CalcularTotalMesa(cantidadPlatos, precioPlato, porcentajePropina);

            // Assert
            // Esperamos 100 + 18 + 10 = 128.0
            Assert.That(resultado, Is.EqualTo(128.0), "La integración entre el Gestor y la Calculadora falló al sumar el total.");
        }

        [Test]
        public void CobrarConCupon_UsandoCalculadoraReal_DescuentaCorrectamente()
        {
            // Arrange
            double totalMesa = 128.0;
            double valorCupon = 28.0;

            // Act
            double resultado = _gestorVentas.CobrarConCupon(totalMesa, valorCupon);

            // Assert
            Assert.That(resultado, Is.EqualTo(100.0), "La integración para aplicar descuentos falló.");
        }
    }
}