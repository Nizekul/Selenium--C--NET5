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
                        case "Multiplicacao":
                            input.SendKeys($"{entrada1}*{entrada2}");
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
