using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Selenium_Calculadora.Entidades;
using Selenium_Calculadora.Xml;

namespace Selenium_Calculadora.Testes
{
    [TestFixture]
    public class CalculadoraTests
    {

        private IWebDriver _driver;
        private readonly XMLHelper _XMLHelper = new();

        [SetUp]
        public void Setup()
        {
            var chromeOptions = new ChromeOptions();
            chromeOptions.AddArgument("--start-maximized");

            _driver = new ChromeDriver("Drivers/ChromeDriver.exe", chromeOptions);

            _driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(60);
            _driver.Navigate().GoToUrl("https://www.calculadoraonline.com.br/basica");
        }

        [TestCase("Soma")]
        [TestCase("Multiplicação")]
        [TestCase("Exponencial")]
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
            IWebElement _btnCalcular = _driver.FindElement(By.Id("b27"));
            IWebElement _divResultado = _driver.FindElement(By.Id("TIExp"));

            _input.Clear();

            var operador = GetOperador(operacao);
            var expressao = string.Join(operador, caso.Entradas);

            _input.SendKeys(expressao);
            Thread.Sleep(1500);

            _btnCalcular.Click();
            Thread.Sleep(2000);

            var resultado = _divResultado.GetAttribute("value");

            Assert.That(resultado, Is.EqualTo(caso.ResultadoEsperado),
                          $"Erro ao executar a operação de {operacao.ToLower()} {string.Join($" {operador} ", caso.Entradas)}. Resultado esperado: {caso.ResultadoEsperado}, Resultado obtido: {resultado}");
        }

        [TestCase]
        public void TestarJurosCompostos()
        {
            IWebElement _btnJurosCompostos = _driver.FindElement(By.Id("b57"));
            IWebElement _divResultado = _driver.FindElement(By.Id("TIExp"));

            var casos = _XMLHelper.LerProcedimentos()
                .FirstOrDefault(p => p.Nome == "JurosCompostos")
                .Casos;

            foreach (var caso in casos)
            {
                _btnJurosCompostos.Click();
                Thread.Sleep(2000);

                IWebElement _inputValorPresente = _driver.FindElement(By.Id("cx57_0"));
                IWebElement _inputTaxaJuros = _driver.FindElement(By.Id("cx57_1"));
                IWebElement _inputPeriodos = _driver.FindElement(By.Id("cx57_2"));
                IWebElement _btnCalcular = _driver.FindElement(By.ClassName("uk-button"));

                _inputValorPresente.Clear();
                _inputTaxaJuros.Clear();
                _inputPeriodos.Clear();

                _inputValorPresente.SendKeys(caso.Entradas[0]);
                _inputTaxaJuros.SendKeys(caso.Entradas[1]);
                _inputPeriodos.SendKeys(caso.Entradas[2]);
                Thread.Sleep(1500);

                _btnCalcular.Click();
                Thread.Sleep(2000);

                var resultado = _divResultado.GetAttribute("value");

                Assert.That(resultado, Is.EqualTo(caso.ResultadoEsperado),
                          $"Erro ao calcular os juros compostos. Resultado esperado: {caso.ResultadoEsperado}, Resultado obtido: {resultado}");
            }

        }

        private static string GetOperador(string operacao)
        {
            return operacao switch
            {
                "Soma" => "+",
                "Multiplicacao" => "*",
                "Divisao" => "/",
                "Exponencial" => "^",
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
