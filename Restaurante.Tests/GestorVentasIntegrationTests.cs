using NUnit.Framework;
using Restaurante;
using System.Collections.Generic;
using System.IO;

namespace Restaurante.Tests
{
    public class GestorVentasIntegrationTests
    {
        private ICalculadoraCuenta _calculadoraReal;
        private GestorVentas _gestorVentas;

        [SetUp]
        public void Setup()
        {
            // Instanciamos las clases reales (Integración)
            _calculadoraReal = new CalculadoraCuenta();
            _gestorVentas = new GestorVentas(_calculadoraReal);
        }

        // 1. FUNCIÓN QUE LEE EL TXT
        public static IEnumerable<TestCaseData> CargarDatosDesdeTxt()
        {
            // Buscamos el archivo en la carpeta donde se ejecuta la prueba
            string rutaArchivo = Path.Combine(TestContext.CurrentContext.TestDirectory, "datos_mesas.txt");
            
            // Leemos todas las líneas del txt
            string[] lineas = File.ReadAllLines(rutaArchivo);

            foreach (string linea in lineas)
            {
                // Separamos los datos por la coma
                string[] valores = linea.Split(',');

                // Convertimos el texto a números
                int cant = int.Parse(valores[0]);
                double precio = double.Parse(valores[1]);
                double propina = double.Parse(valores[2]);
                double totalEsperado = double.Parse(valores[3]);

                // Retornamos esta fila como un nuevo caso de prueba
                yield return new TestCaseData(cant, precio, propina, totalEsperado);
            }
        }

        // 2. CONECTAR LA PRUEBA A LA FUNCIÓN USANDO TestCaseSource
        [Test, TestCaseSource(nameof(CargarDatosDesdeTxt))]
        public void CalcularTotalMesa_LeyendoDatosTxt_RetornaTotalCorrecto(int cant, double precio, double propina, double totalEsperado)
        {
            // Act
            double resultado = _gestorVentas.CalcularTotalMesa(cant, precio, propina);

            // Assert
            Assert.That(resultado, Is.EqualTo(totalEsperado), 
                $"El cálculo falló para la fila del txt: {cant} platos de {precio}.");
        }

        [Test]
        public void CalcularTotalMesa_ClienteNoDejaPropina_RetornaSubtotalMasIGV()
        {
            // Arrange: Un caso de negocio válido. ¿Qué pasa si la propina es 0?
            int cantPlatos = 2;
            double precio = 50.0; // Subtotal = 100.0
            double propina = 0.0; // Sin propina
                                  // IGV 18% = 18.0
                                  // Total esperado = 118.0

            // Act
            double resultado = _gestorVentas.CalcularTotalMesa(cantPlatos, precio, propina);

            // Assert
            Assert.That(resultado, Is.EqualTo(118.0), "Si el cliente no deja propina, solo debe cobrarse el subtotal más el IGV.");
        }

        [Test]
        public void CobrarConCupon_IntentoDeCuponNegativo_LanzaExcepcion()
        {
            // Arrange: Un cajero intenta ingresar un descuento negativo (posible error de tipeo o fraude)
            double totalMesa = 150.0;
            double cuponNegativo = -20.0; 

            // Act & Assert
            // La Calculadora real tiene una regla contra descuentos negativos. 
            // Verificamos que el Gestor no se salte esta regla de seguridad.
            Assert.Throws<ArgumentException>(() => _gestorVentas.CobrarConCupon(totalMesa, cuponNegativo),
                "El sistema debe bloquear intentos de ingresar cupones con montos negativos.");
        }

        [Test]
        public void CalcularTotalMesa_EventoCorporativoMasivo_CalculaMontoGrandeCorrectamente()
        {
            // Arrange: Prueba de estrés/volumen con números más grandes
            int cantPlatos = 500;
            double precio = 120.5; // Subtotal = 60,250.0
            double propina = 15.0; // Propina 15% = 9,037.5
                                   // IGV 18% = 10,845.0
                                   // Total esperado = 80,132.5

            // Act
            double resultado = _gestorVentas.CalcularTotalMesa(cantPlatos, precio, propina);

            // Assert
            Assert.That(resultado, Is.EqualTo(80132.5), "El cálculo para eventos masivos debe mantener la precisión de los decimales.");
        }
    }
}