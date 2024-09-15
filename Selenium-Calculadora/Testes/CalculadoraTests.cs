using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Selenium_Calculadora.Entidades;
using Selenium_Calculadora.Xml;

namespace Selenium_Calculadora.Testes
{
    [TestFixture]
    public class CalculadoraTests
    {

        private IWebDriver _driver;
        private readonly XMLHelper _XMLHelper = new();

        public CalculadoraTests()
        {
            var chromeOptions = new ChromeOptions();
            chromeOptions.AddArgument("--start-maximized");

            _driver = new ChromeDriver("Drivers/ChromeDriver.exe", chromeOptions);
            Setup();
        }


        [SetUp]
        public void Setup()
        {
            _driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(60);
            _driver.Navigate().GoToUrl("https://www.calculadoraonline.com.br/basica");
        }

        [TestCase("Soma")]
        public void TestarOperacao(string nomeOperacao)
        {
            var procedimentos = _XMLHelper.LerProcedimentos();

            foreach (var item in procedimentos.Where(p => p.Nome == nomeOperacao))
            {
                foreach (var caso in item.Casos)
                {
                    TestarCaso(nomeOperacao, caso);
                }
            }
        }

        private void TestarCaso(string operacao, Caso caso)
        {
            IWebElement _input = _driver.FindElement(By.Id("TIExp"));
            IWebElement _btnResult = _driver.FindElement(By.Id("b27"));
            IWebElement _divResultado = _driver.FindElement(By.Id("TIExp"));

            _input.Clear();
          
            var operador = GetOperador(operacao);
            var expressao = string.Join(operador, caso.Entradas);

            _input.SendKeys(expressao);
            Thread.Sleep(1500);

            _btnResult.Click();
            Thread.Sleep(2000);

            var resultado = _divResultado.GetAttribute("value");

            Assert.That(resultado, Is.EqualTo(caso.ResultadoEsperado),
                          $"Erro ao {operacao.ToLower()} {string.Join($" {operador} ", caso.Entradas)}. Resultado esperado: {caso.ResultadoEsperado}, Resultado obtido: {resultado}");
        }

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
                        var entradas = caso.Entradas;
                        var resultadoEsperado = caso.ResultadoEsperado;

                        input.Clear();

                        var expressao = string.Join("*", entradas);
                        input.SendKeys(expressao);

                        IWebElement btnResult = _driver.FindElement(By.Id("b27"));
                        btnResult.Click();

                        IWebElement divResultado = _driver.FindElement(By.Id("TIExp"));
                        string resultadoObtido = divResultado.GetAttribute("value");

                        Assert.That(resultadoObtido, Is.EqualTo(resultadoEsperado.ToString()), $"Erro ao multiplicar {string.Join(" * ", entradas)}. Resultado esperado: {resultadoEsperado}, Resultado obtido: {resultadoObtido}");

                        input.Clear();
                    }
                }
            }
        }
        private static string GetOperador(string operacao)
        {
            return operacao switch
            {
                "Soma" => "+",
                "Multiplicacao" => "*",
                "Divisao" => "/",
                "Potenciacao" => "^",
                "Porcentagem" => "%",
                _ => throw new ArgumentException("Operação desconhecida", nameof(operacao))
            };
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
