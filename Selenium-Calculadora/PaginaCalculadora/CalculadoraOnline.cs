using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace SeleniumCalculadora.PaginaCalculadora
{
    public class CalculadoraOnline
    {
        private readonly IConfiguration _configuration;
        private IWebDriver _driver;

        public CalculadoraOnline()
        {
            //_configuration = configuration;

            var chromeOptions = new ChromeOptions();
            chromeOptions.AddArgument("--headless");

            _driver = new ChromeDriver(
                "Drivers/ChromeDriver.exe",
                chromeOptions);
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
            var vegetable = _driver.FindElement(By.ClassName("bt bt1 cnum"));

            var rowsCotacoes = _driver
                .FindElement(By.Id("b1")
                );

        }

        public void Fechar()
        {
            _driver.Quit();
            _driver = null;
        }
    }
}