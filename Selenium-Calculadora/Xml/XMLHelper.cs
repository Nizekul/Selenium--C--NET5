using System.Linq;
using System.Xml.Linq;
using Selenium_Calculadora.Entidades;

namespace Selenium_Calculadora.Xml
{
    public class XMLHelper
    {
        private static readonly string _CaminhoXml = "Xml\\CasosDeTeste.xml";
        private static XElement DocumentoXml
        {
            get
            {
                return XElement.Load(_CaminhoXml);
            }
        }

        public IEnumerable<Procedimento> LerProcedimentos()
        {
            return DocumentoXml.Descendants("procedimento").Select(procedimento => new Procedimento
            {
                Nome = procedimento.Attribute("nome")?.Value ?? "",
                Casos = procedimento.Elements("caso").Select(caso => new Caso
                {
                    Entradas = caso.Element("entradas")?.Elements("entrada")
                            .Select(e => e.Value.ToString())
                            .ToList() ?? new List<string>(),
                    ResultadoEsperado = caso.Element("resultado")?.Value
                }).ToList()
            });
        }
    }
}
