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

        public void ObterValores()
        {
            var listValores = _XMLHelper.LerProcedimentos();

            foreach (var procedimento in listValores)
            {
                foreach (var caso in procedimento.Casos)
                {
                    // Clicar nos botões baseados nas entradas do XML
                    foreach (var entrada in caso.Entradas)
                    {
                        IWebElement botao = null;

                        // Atribuir os IDs dos botões conforme as entradas
                        switch (entrada)
                        {
                            case 5:
                                botao = _driver.FindElement(By.Id("b10"));
                                break;
                            case 3:
                                botao = _driver.FindElement(By.Id("b19"));
                                break;
                                // Adicionar mais cases para outros botões conforme necessário
                        }

                        // Clicar no botão correspondente
                        botao.Click();
                    }

                    // Após clicar nos números, clicamos no botão de soma (ID do botão de '+' na calculadora)
                    IWebElement botaoSoma = _driver.FindElement(By.Id("b24")); // Exemplo de ID do botão "+"
                    botaoSoma.Click();

                    // Agora clicamos no botão de "=" para obter o resultado
                    IWebElement botaoIgual = _driver.FindElement(By.Id("b25")); // Exemplo de ID do botão "="
                    botaoIgual.Click();

                    // Pegar o valor exibido no resultado (exemplo usando ID do display)
                    IWebElement resultadoDisplay = _driver.FindElement(By.Id("display")); // Exemplo de ID do display
                    string resultadoObtido = resultadoDisplay.Text;

                    // Comparar o resultado obtido com o resultado esperado
                    if (resultadoObtido == caso.ResultadoEsperado.ToString())
                    {
                        Console.WriteLine($"Teste de {procedimento.Nome}: Sucesso!");
                    }
                    else
                    {
                        Console.WriteLine($"Teste de {procedimento.Nome}: Falhou! Esperado: {caso.ResultadoEsperado}, Obtido: {resultadoObtido}");
                    }

                    // Limpar o display antes de executar o próximo teste (caso haja um botão de limpar)
                    IWebElement botaoLimpar = _driver.FindElement(By.Id("b27")); // Exemplo de ID do botão "C" (Clear)
                    botaoLimpar.Click();
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