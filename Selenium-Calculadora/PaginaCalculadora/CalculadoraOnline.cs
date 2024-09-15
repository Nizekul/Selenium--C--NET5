using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace SeleniumCalculadora.PaginaCalculadora
{
    public class CalculadoraOnline
    {
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

        public void ObterValores()
        {
            IWebElement input = _driver.FindElement(By.Id("TIExp"));

            input.SendKeys("3+5");

            IWebElement btnResult = _driver.FindElement(By.Id("b27"));
            btnResult.Click();

            IWebElement divResultado = _driver.FindElement(By.Id("TIExp"));
            string resultado = divResultado.GetAttribute("value");

            Console.WriteLine("Resultado da operação: " + resultado);
        }

        public void Fechar()
        {
            _driver.Quit();
            _driver = null;
        }
    }
}