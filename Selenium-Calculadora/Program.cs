
using Selenium_Calculadora.Xml;
using SeleniumCalculadora.PaginaCalculadora;

class Program
{
    static void Main(string[] args)
    {
        CalculadoraOnline teste = new();
        teste.ObterValores();

        XMLHelper xMLHelper = new();
        var procedimentos = xMLHelper.LerProcedimentos();
        foreach (var procedimento in procedimentos)
        {
            Console.WriteLine($"Nome do Procedimento: {procedimento.Nome}");
            foreach (var caso in procedimento.Casos)
            {
                Console.WriteLine($"  Caso ID: {caso.Id}");
                Console.WriteLine($"    Entrada 1: {caso.Entrada1}");
                Console.WriteLine($"    Entrada 2: {caso.Entrada2}");
                Console.WriteLine($"    Resultado Esperado: {caso.ResultadoEsperado}");
            }
        }
    }
}