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

            foreach (var item in procedimentos)
            {
                foreach (var item2 in item.Casos)
                {
                    var entrada1 = item2.Entrada1;
                    var entrada2 = item2.Entrada2;

                    switch (item.Nome)
                    {
                        case "Soma":
                            input.SendKeys($"{entrada1}+{entrada2}");
                            break;
                        case "Divisao":
                            input.SendKeys($"{entrada1}/{entrada2}");
                            break;
                        case "Potenciacao":
                            input.SendKeys($"{entrada1}^{entrada2}");
                            break;
                        case "Porcentagem":
                            input.SendKeys($"{entrada1}%{entrada2}");
                            break;
                    }

                    IWebElement btnResult = _driver.FindElement(By.Id("b27"));
                    btnResult.Click();

                    IWebElement divResultado = _driver.FindElement(By.Id("TIExp"));
                    string resultado = divResultado.GetAttribute("value");

                    Console.WriteLine("Resultado da operação: " + resultado);
                }
            }
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
