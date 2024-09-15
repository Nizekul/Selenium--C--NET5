
using Selenium_Calculadora.Xml;
using SeleniumCalculadora.PaginaCalculadora;

class Program
{
    static void Main(string[] args)
    {
        CalculadoraOnline teste = new();
        teste.ObterValores();

        XMLHelper xMLHelper = new();
        xMLHelper.LerProcedimentos();      
    }
}