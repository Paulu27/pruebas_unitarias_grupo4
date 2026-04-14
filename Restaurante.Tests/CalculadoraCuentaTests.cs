using Restaurante;
using System;

namespace Restaurante.Tests
{
    public class CalculadoraCuentaTests
    {
        private CalculadoraCuenta _calculadora;

        [SetUp]
        public void Setup()
        {
            _calculadora = new CalculadoraCuenta();
        }

        [Test]
        public void CalcularCostoPlatillos_DadoCantidadYPrecio_RetornaCostoCorrecto()
        {
            // Arrange
            int cantidad = 3;
            double precio = 25.0;

            // Act
            double resultado = _calculadora.CalcularCostoPlatillos(cantidad, precio);

            // Assert
            Assert.That(resultado, Is.EqualTo(75.0), "3 platos a 25.0 cada uno debe ser 75.0");
        }

        [Test]
        public void CalcularIGV_DadoSubtotal_RetornaDieciochoPorCiento()
        {
            // Arrange
            double subtotal = 100.0;

            // Act
            double resultado = _calculadora.CalcularIGV(subtotal);

            // Assert
            Assert.That(resultado, Is.EqualTo(18.0), "El 18% de 100 debe ser 18.");
        }

        [Test]
        public void AplicarDescuento_MontoValido_RetornaTotalConDescuento()
        {
            // Arrange
            double total = 50.0;
            double descuento = 10.0;

            // Act
            double resultado = _calculadora.AplicarDescuento(total, descuento);

            // Assert
            Assert.That(resultado, Is.EqualTo(40.0), "Si al total de 50 se le descuenta 10, debe quedar 40.");
        }

        [Test]
        public void AplicarDescuento_DescuentoMayorAlTotal_LanzaExcepcion()
        {
            // Arrange
            double total = 50.0;
            double descuento = 60.0;

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _calculadora.AplicarDescuento(total, descuento), 
                "Un descuento mayor al total debe lanzar ArgumentException.");
        }
    }
}