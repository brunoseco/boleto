using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Common.Boleto
{
    public class BoletoCobrancaHtml : BoletoCobranca
    {

        public BoletoCobrancaHtml()
        {

        }

        [SecurityCritical()]
        public string GerarBoletoHtml(BoletoCobranca cobranca, bool segundaVia)
        {
            return MontarHTMLBoleto(cobranca, segundaVia);
        }

        [SecurityCritical()]
        public string GerarBoleto(IEnumerable<BoletoCobranca> cobrancas, bool segundaVia)
        {
            var contagem = cobrancas.Count();
            var count = 1;
            var htmlBoleto = "";
            foreach (var ordemDeCobranca in cobrancas.OrderBy(_ => _.Boleto.Carne))
            {
                htmlBoleto += GerarBoletoHtml(ordemDeCobranca, segundaVia);
                var ehCarne = ordemDeCobranca.Boleto.Carne;

                if (!ehCarne)
                {
                    if (contagem > 1)
                        htmlBoleto += "<html><body><div style='page-break-after: always; margin-top: 1px;'></div></body></html>";
                }
                else
                {
                    if ((count % 3) == 0)
                        htmlBoleto += "<html><body><div style='page-break-after: always; margin-top: 1px;'></div></body></html>";
                }

                count++;
            }

            return htmlBoleto.ToString();
        }

        private string MontarHTMLBoleto(BoletoCobranca cobranca, bool segundaVia)
        {
            var conteudo = String.Empty;
            var instrucoes = String.Empty;

            var boleto = new StringBuilder();
            var bb = this.MontarBoleto(cobranca, segundaVia);

            MontarHtmlHeader(boleto, cobranca);
            boleto.Append(bb.MontaHtmlExporta(cobranca.Boleto.PathPastaImagens));
            MontarHtmlFooter(boleto);

            return boleto.ToString();
        }

        private void MontarHtmlHeader(StringBuilder html, BoletoCobranca cobranca)
        {
            html.Append("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">\n");
            html.Append("<html xmlns=\"http://www.w3.org/1999/xhtml\">\n");
            html.Append("<head>");
            html.Append("    <title>Boleto.Net</title>\n");
            {
                html.Append("<style>\n");

                if (File.Exists(cobranca.Boleto.PathCss))
                {
                    var estilo = File.ReadAllText(cobranca.Boleto.PathCss);
                    html.Append(estilo);
                }

                html.Append("</style>\n");
            }
        }
        
        private void MontarHtmlFooter(StringBuilder saida)
        {
            saida.Append("</body>\n");
            saida.Append("</html>\n");
        }


    }
}
