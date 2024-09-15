using System.Xml.Linq;

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
                Nome = procedimento.Attribute("nome")?.Value,
                Casos = procedimento.Elements("caso").Select(caso => new Caso
                {
                    Id = caso.Attribute("id")?.Value,
                    Entrada1 = caso.Element("entrada1")?.Value,
                    Entrada2 = caso.Element("entrada2")?.Value,
                    ResultadoEsperado = caso.Element("resultado")?.Value
                }).ToList()
            }).ToList();
        }


    }

}
