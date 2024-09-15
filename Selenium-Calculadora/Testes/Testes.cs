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

        [SetUp]
        public void Setup()
        {
            _driver = new ChromeDriver();
            _driver.Navigate().GoToUrl("https://www.calculadoraonline.com.br/basica"); 

            _XMLHelper = new XMLHelper();
        }

        [Test]
        public void TesteProcedimentos()
        {
            IWebElement input = _driver.FindElement(By.Id("TIExp"));

            var procedimentos = _XMLHelper.LerProcedimentos();

            foreach (var procedimento in procedimentos)
            {
                foreach (var caso in procedimento.Casos)
                {
                    var entrada1 = caso.Entrada1;
                    var entrada2 = caso.Entrada2;

                    var entrada = CriarEntrada(procedimento.Nome, entrada1, entrada2);

                    input.SendKeys($"{entrada}");                   

                    IWebElement btnResult = _driver.FindElement(By.Id("b27"));
                    btnResult.Click();

                    IWebElement divResultado = _driver.FindElement(By.Id("TIExp"));
                    string resultado = divResultado.GetAttribute("value");

                    Console.WriteLine("Resultado da operação: " + resultado);
                }
            }
        }

        private string CriarEntrada(string operacao, string entrada1, string entrada2)
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
