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
        private XMLHelper _XMLHelper = new XMLHelper();

        public Testes()
        {
            var chromeOptions = new ChromeOptions();
            chromeOptions.AddArgument("--headless");

            _driver = new ChromeDriver(
                "Drivers/ChromeDriver.exe",
                chromeOptions);
            Setup();
        }


        [SetUp]
        public void Setup()
        {
            _driver.Manage().Timeouts().PageLoad =
                TimeSpan.FromSeconds(60);
            _driver.Navigate().GoToUrl(
                "https://www.calculadoraonline.com.br/basica"
                );
        }

        //[Test] 
        //public void TesteProcedimentos()
        //{
        //    IWebElement input = _driver.FindElement(By.Id("TIExp"));

        //    var procedimentos = _XMLHelper.LerProcedimentos();

        //    foreach (var item in procedimentos)
        //    {
        //        foreach (var item2 in item.Casos)
        //        {
        //            var entradas = item2.Entradas;


        //            switch (item.Nome)
        //            {
        //                case "Soma":
        //                    var expressao = string.Join("+", entradas);
        //                    input.SendKeys(expressao);
        //                    break;
        //                case "Divisao":
        //                    expressao = string.Join("+", entradas);
        //                    input.SendKeys(expressao);
        //                    break;
        //                case "Potenciacao":
        //                    expressao = string.Join("+", entradas);
        //                    input.SendKeys(expressao);
        //                    break;
        //                case "Porcentagem":
        //                    expressao = string.Join("+", entradas);
        //                    input.SendKeys(expressao);
        //                    break;
        //            }

        //            IWebElement btnResult = _driver.FindElement(By.Id("b27"));
        //            btnResult.Click();

        //            IWebElement divResultado = _driver.FindElement(By.Id("TIExp"));
        //            string resultado = divResultado.GetAttribute("value");

        //            Console.WriteLine("Resultado da operação: " + resultado);
        //        }
        //    }
        //}

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
