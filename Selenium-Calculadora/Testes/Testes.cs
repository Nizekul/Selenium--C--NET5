using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Selenium_Calculadora.Xml;

namespace Selenium_Calculadora.Testes
{
    [TestFixture]
    public class Testes
    {
        private IWebDriver _driver;
        private XMLHelper _XMLHelper;
        private IWebElement _input;
        private IWebElement _btnResult;
        private IWebElement _divResultado;

        [SetUp]
        public void Setup()
        {
            _driver = new ChromeDriver();
            _driver.Navigate().GoToUrl("https://www.calculadoraonline.com.br/basica");

            _XMLHelper = new XMLHelper();
            _input = _driver.FindElement(By.Id("TIExp"));
            _btnResult = _driver.FindElement(By.Id("b27"));
            _divResultado = _driver.FindElement(By.Id("TIExp"));
        }

        [TestCase("Soma")]
        public void TestarOperacao(string operacao)
        {
            IWebElement input = _driver.FindElement(By.Id("TIExp"));

            var procedimentos = _XMLHelper.LerProcedimentos();

            foreach (var item in procedimentos.Where(p => p.Nome == operacao))
            {
                foreach (var caso in item.Casos)
                    TestarCaso(operacao, caso.Entrada1, caso.Entrada2, caso.ResultadoEsperado);                
            }
        }

        private void TestarCaso(string operacao, string entrada1, string entrada2, string resultadoEsperado)
        {
            _input.Clear();
            _input.SendKeys(CriarEntrada(operacao, entrada1, entrada2));
            _btnResult.Click();

            var resultado = _divResultado.GetAttribute("value");
            Console.WriteLine($"Resultado da operação {operacao}: {resultado}");
            Assert.That(resultado, 
                        Is.EqualTo(resultadoEsperado.ToString()), 
                        $"Erro ao realizar a operação {operacao} com a entrada {entrada1} + {entrada2}. Resultado esperado: {resultadoEsperado}, Resultado obtido: {resultado}");
        }

        private static string CriarEntrada(string operacao, string entrada1, string entrada2)
        {
            return operacao switch
            {
                "Soma" => $"{entrada1}+{entrada2}",
                "Multiplicacao" => $"{entrada1}*{entrada2}",
                "Divisao" => $"{entrada1}/{entrada2}",
                "Potenciacao" => $"{entrada1}^{entrada2}",
                "Porcentagem" => $"{entrada1}%{entrada2}",
                _ => throw new ArgumentException("Operação desconhecida", nameof(operacao))
            };
        }

        [Test]
        public void TesteMultiplicacao()
        {
            IWebElement input = _driver.FindElement(By.Id("TIExp"));
            var procedimentos = _XMLHelper.LerProcedimentos();

            foreach (var procedimento in procedimentos)
            {
                if (procedimento.Nome == "Multiplicacao")
                {
                    foreach (var caso in procedimento.Casos)
                    {
                        var entrada1 = caso.Entrada1;
                        var entrada2 = caso.Entrada2;
                        var resultadoEsperado = caso.ResultadoEsperado;

                        input.SendKeys($"{entrada1}*{entrada2}");

                        IWebElement btnResult = _driver.FindElement(By.Id("b27"));
                        btnResult.Click();

                        IWebElement divResultado = _driver.FindElement(By.Id("TIExp"));
                        string resultadoObtido = divResultado.GetAttribute("value");

                        Assert.That(resultadoObtido, Is.EqualTo(resultadoEsperado.ToString()), $"Erro ao multiplicar {entrada1} + {entrada2}. Resultado esperado: {resultadoEsperado}, Resultado obtido: {resultadoObtido}");
                    }
                }
            }
        }

        [TearDown]
        public void TearDown()
        {
            if (_driver != null)
            {
                _driver.Quit();
            }
        }
    }
}
