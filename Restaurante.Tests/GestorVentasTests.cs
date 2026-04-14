using Restaurante;

namespace Restaurante.Tests
{
    public class GestorVentasTests
    {
        private Mock<ICalculadoraCuenta> _calculadoraMock;
        private GestorVentas _gestorVentas;

        [SetUp]
        public void Setup()
        {
            // Crear el mock
            _calculadoraMock = new Mock<ICalculadoraCuenta>();
            
            // Configuramos qué debe responder el mock cuando lo llamen
            _calculadoraMock.Setup(calc => calc.CalcularCostoPlatillos(It.IsAny<int>(), It.IsAny<double>()))
                .Returns<int, double>((cant, precio) => cant * precio);
                
            _calculadoraMock.Setup(calc => calc.CalcularIGV(It.IsAny<double>()))
                .Returns<double>(subtotal => subtotal * 0.18);
                
            _calculadoraMock.Setup(calc => calc.CalcularPropina(It.IsAny<double>(), It.IsAny<double>()))
                .Returns<double>((total, porc) => total * (porc / 100));

            _calculadoraMock.Setup(calc => calc.AplicarDescuento(It.IsAny<double>(), It.IsAny<double>()))
                .Returns<double>((total, desc) => total - desc);

            // Inicializamos el gestor inyectando el mock
            _gestorVentas = new GestorVentas(_calculadoraMock.Object);
        }

        [Test]
        public void CalcularTotalMesa_DadoConsumo_SumaCostoIGVYPropina()
        {
            // Arrange
            int cantidad = 2; // 2 platos
            double precio = 50.0; // 50 cada plato -> Subtotal = 100
            double porcentajePropina = 10.0; // 10% propina -> 10
            // IGV esperado -> 18

            // Act
            double resultado = _gestorVentas.CalcularTotalMesa(cantidad, precio, porcentajePropina);

            // Assert
            // 100 (Subtotal) + 18 (IGV) + 10 (Propina) = 128
            Assert.That(resultado, Is.EqualTo(128.0), "El total de la mesa debería ser 128.");
            
            // Verificamos que los métodos del mock realmente fueron llamados
            _calculadoraMock.Verify(calc => calc.CalcularCostoPlatillos(cantidad, precio), Times.Once);
            _calculadoraMock.Verify(calc => calc.CalcularIGV(100.0), Times.Once);
            _calculadoraMock.Verify(calc => calc.CalcularPropina(100.0, porcentajePropina), Times.Once);
        }

        [Test]
        public void CobrarConCupon_DadoTotalYCupon_RetornaMontoReducido()
        {
            // Arrange
            double totalMesa = 128.0;
            double cupon = 28.0;

            // Act
            double resultado = _gestorVentas.CobrarConCupon(totalMesa, cupon);

            // Assert
            Assert.That(resultado, Is.EqualTo(100.0), "Si la cuenta es 128 y el cupón es 28, debe pagar 100.");
            
            // Verificar llamada al método
            _calculadoraMock.Verify(calc => calc.AplicarDescuento(totalMesa, cupon), Times.Once);
        }
    }
}