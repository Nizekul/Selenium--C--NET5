using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Selenium_Calculadora.Xml;

namespace SeleniumCalculadora.PaginaCalculadora
{
    public class CalculadoraOnline
    {
        private XMLHelper _XMLHelper = new XMLHelper();
        private IWebDriver _driver;

        public CalculadoraOnline()
        {
            var chromeOptions = new ChromeOptions();
            chromeOptions.AddArgument("--headless");

            _driver = new ChromeDriver(
                "Drivers/ChromeDriver.exe",
                chromeOptions);
            CarregarPagina();
        }

        public void CarregarPagina()
        {
            _driver.Manage().Timeouts().PageLoad =
                TimeSpan.FromSeconds(60);
            _driver.Navigate().GoToUrl(
                "https://www.calculadoraonline.com.br/basica"
                );
        }

        public void SomaValores()
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
                            input.SendKeys($"{entrada1}+{entrada2}");
                            break;
                        case "Potenciacao":
                            input.SendKeys($"{entrada1}+{entrada2}");
                            break;
                        case "Porcentagem":
                            input.SendKeys($"{entrada1}+{entrada2}");
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


        public void Fechar()
        {
            _driver.Quit();
            _driver = null;
        }
    }
}