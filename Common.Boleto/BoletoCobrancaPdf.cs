using Common.Boleto.Enum;
using Common.Document;
using SelectPdf;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Common.Boleto
{
    public class BoletoCobrancaPdf : BoletoCobranca
    {

        [SecurityCritical()]
        public byte[] GerarBoleto(List<BoletoCobranca> cobrancas, bool segundaVia)
        {
            var component = new PdfComponentSelect();
            PdfDocument doc = component.GetInstance();

            doc.DocumentInformation.Title = "Boleto(s) " + DateTime.Now.TimeOfDay;
            doc.DocumentInformation.CreationDate = DateTime.Now;

            PdfRenderingResult pdf;

            PdfPage page = doc.AddPage(PdfCustomPageSize.A4, new PdfMargins(5, 0, 5, 0), PdfPageOrientation.Portrait);

            var addNovaPagina = false;

            var cobrancasComum = cobrancas.Where(_ => !_.Boleto.Carne).OrderBy(_ => _.Boleto.DataVencimento);
            foreach (var item in cobrancasComum)
            {
                if (addNovaPagina)
                {
                    page = doc.AddPage(PdfCustomPageSize.A4, new PdfMargins(5, 0, 5, 0), PdfPageOrientation.Portrait);
                    addNovaPagina = false;
                }

                var boletoBancario = this.MontarBoleto(item, segundaVia);
                pdf = this.MakePages(doc, boletoBancario, item.Boleto.pathArquivosBoletoNet, page, item);

                addNovaPagina = true;
            }

            var count = 1;
            var cobrancasCarne = cobrancas.Where(_ => _.Boleto.Carne).OrderBy(_ => _.Boleto.DataVencimento);
            foreach (var item in cobrancasCarne)
            {
                var boletoBancario = this.MontarBoleto(item, segundaVia);

                if (addNovaPagina)
                {
                    page = doc.AddPage(PdfCustomPageSize.A4, new PdfMargins(5, 0, 5, 0), PdfPageOrientation.Portrait);
                    addNovaPagina = false;
                }

                if (count == 1)
                    pdf = this.MakePagesCarne(doc, boletoBancario, item.Boleto.pathArquivosBoletoNet, page, count, 0, 0);
                else if (count == 2)
                    pdf = this.MakePagesCarne(doc, boletoBancario, item.Boleto.pathArquivosBoletoNet, page, count, 0, 280);
                else
                {
                    pdf = this.MakePagesCarne(doc, boletoBancario, item.Boleto.pathArquivosBoletoNet, page, count, 0, 560);
                    count = 0;
                    addNovaPagina = true;
                }

                count++;
            }

            var bytes = doc.Save();

            doc.Close();

            return bytes;
        }

        #region Carne

        private PdfRenderingResult MakePagesCarne(PdfDocument doc, BoletoNet.BoletoBancario bb, string pathArquivosBoletoNet, PdfPage page, int count, int x, int y)
        {
            PdfRenderingResult pdf;
            PdfFont font = doc.AddFont(PdfStandardFont.HelveticaBold); font.Size = 08;
            Color color = Color.Black;

            pdf = this.BackgroundCarne(bb, page, pathArquivosBoletoNet, x, y);

            pdf = this.MontaCarneEsquerda(bb, pathArquivosBoletoNet, page, pdf, font, color, x, y);
            pdf = this.MontaCarneDireita(bb, pathArquivosBoletoNet, page, pdf, font, color, x, y);

            return pdf;
        }

        private PdfRenderingResult MontaCarneDireita(BoletoNet.BoletoBancario bb, string pathArquivosBoletoNet, PdfPage page, PdfRenderingResult pdf, PdfFont font, Color color, int x, int y)
        {
            pdf = this.HeaderCarneDireita(bb, pathArquivosBoletoNet, page, ref pdf, font, ref color, ref x, y);
            pdf = this.Linha1CarneDireita(bb, page, ref pdf, font, ref color, x, y);
            pdf = this.Linha2CarneDireita(bb, page, ref pdf, font, ref color, x, y);
            pdf = this.Linha3CarneDireita(bb, page, ref pdf, font, ref color, x, y);
            pdf = this.Linha4CarneDireita(bb, page, ref pdf, font, ref color, x, y);
            pdf = this.Linha5CarneDireita(bb, page, ref pdf, font, ref color, x, y);
            pdf = this.Linha6CarneDireita(bb, page, ref pdf, font, ref color, x, y);
            pdf = this.FooterCarneDireita(bb, page, ref pdf, font, ref color, x, y);

            return pdf;
        }

        private PdfRenderingResult FooterCarneDireita(BoletoNet.BoletoBancario bb, PdfPage page, ref PdfRenderingResult pdf, PdfFont font, ref Color color, int x, int y)
        {
            PdfTextElement compensacao = new PdfTextElement(x + 390, y + 252, 140, "Ficha de Compensação", font, color);
            compensacao.HorizontalAlign = PdfTextHorizontalAlign.Right;
            pdf = page.Add(compensacao);

            var codigobarras = new BoletoNet.BoletoBancario().GeraImagemCodigoBarras(bb.Boleto);
            PdfImageElement imgcodbarras = new PdfImageElement(x + 115, y + 233, codigobarras);
            pdf = page.Add(imgcodbarras);

            return pdf;
        }

        private PdfRenderingResult Linha6CarneDireita(BoletoNet.BoletoBancario bb, PdfPage page, ref PdfRenderingResult pdf, PdfFont font, ref Color color, int x, int y)
        {
            var index2 = y + 199;
            var liststr2 = bb.ExibeSacado.Split(new string[] { "<br />" }, StringSplitOptions.None);
            foreach (var item in liststr2)
            {
                PdfTextElement pagador = new PdfTextElement(x + 112, index2, item, font, color);
                pdf = page.Add(pagador);
                index2 += 08;
            }

            var liststr3 = bb.ExibeInfoSacado.Split(new string[] { "<br />" }, StringSplitOptions.None);
            foreach (var item in liststr3)
            {
                PdfTextElement pagador = new PdfTextElement(x + 112, index2, item, font, color);
                pdf = page.Add(pagador);
                index2 += 08;
            }

            return pdf;
        }

        private PdfRenderingResult Linha5CarneDireita(BoletoNet.BoletoBancario bb, PdfPage page, ref PdfRenderingResult pdf, PdfFont font, ref Color color, int x, int y)
        {
            PdfTextElement desconto = new PdfTextElement(x + 430, y + 108, 140, bb.ExibeDescontos, font, color);
            desconto.HorizontalAlign = PdfTextHorizontalAlign.Right;
            pdf = page.Add(desconto);

            PdfTextElement deducoes = new PdfTextElement(x + 430, y + 126, 140, bb.ExibeOutrasDeducoes, font, color);
            deducoes.HorizontalAlign = PdfTextHorizontalAlign.Right;
            pdf = page.Add(deducoes);

            PdfTextElement moramulta = new PdfTextElement(x + 430, y + 144, 140, bb.ExibeMoraMulta, font, color);
            moramulta.HorizontalAlign = PdfTextHorizontalAlign.Right;
            pdf = page.Add(moramulta);

            PdfTextElement acrescimos = new PdfTextElement(x + 430, y + 162, 140, bb.ExibeOutrosAcrescimos, font, color);
            acrescimos.HorizontalAlign = PdfTextHorizontalAlign.Right;
            pdf = page.Add(acrescimos);

            PdfTextElement cobrado = new PdfTextElement(x + 430, y + 181, 140, bb.ExibeValorCobrado, font, color);
            cobrado.HorizontalAlign = PdfTextHorizontalAlign.Right;
            pdf = page.Add(cobrado);

            var index1 = y + 108;
            var liststr1 = bb.ExibeInstrucoesCedente.Split(new string[] { "<br />" }, StringSplitOptions.None);
            foreach (var item in liststr1)
            {
                PdfTextElement instrucao = new PdfTextElement(x + 112, index1, 332, 40, item, font, color);
                pdf = page.Add(instrucao);
                index1 += 10;
            }

            return pdf;
        }

        private PdfRenderingResult Linha4CarneDireita(BoletoNet.BoletoBancario bb, PdfPage page, ref PdfRenderingResult pdf, PdfFont font, ref Color color, int x, int y)
        {
            PdfTextElement carteira = new PdfTextElement(x + 194, y + 89, bb.ExibeDescricaoCarteira, font, color);
            pdf = page.Add(carteira);

            PdfTextElement especie = new PdfTextElement(x + 257, y + 89, bb.Boleto.Especie, font, color);
            pdf = page.Add(especie);

            PdfTextElement quantidade = new PdfTextElement(x + 300, y + 89, bb.ExibeQuantidade, font, color);
            pdf = page.Add(quantidade);

            PdfTextElement valorcobrado = new PdfTextElement(x + 356, y + 89, bb.ExibeValorCobrado, font, color);
            pdf = page.Add(valorcobrado);

            PdfTextElement valordocumento = new PdfTextElement(x + 430, y + 89, 140, bb.ExibeValorDocumento, font, color);
            valordocumento.HorizontalAlign = PdfTextHorizontalAlign.Right;
            pdf = page.Add(valordocumento);

            return pdf;
        }

        private PdfRenderingResult Linha3CarneDireita(BoletoNet.BoletoBancario bb, PdfPage page, ref PdfRenderingResult pdf, PdfFont font, ref Color color, int x, int y)
        {
            PdfTextElement datadocumento = new PdfTextElement(x + 112, y + 73, bb.ExibeDataDocumento, font, color);
            pdf = page.Add(datadocumento);

            PdfTextElement numerodocumento = new PdfTextElement(x + 196, y + 73, bb.Boleto.NumeroDocumento, font, color);
            pdf = page.Add(numerodocumento);

            PdfTextElement especiedoc = new PdfTextElement(x + 305, y + 73, bb.ExibeEspecieDocumento, font, color);
            pdf = page.Add(especiedoc);

            PdfTextElement aceite = new PdfTextElement(x + 352, y + 73, bb.Boleto.Aceite, font, color);
            pdf = page.Add(aceite);

            PdfTextElement dataprocessamento = new PdfTextElement(x + 381, y + 73, bb.ExibeDataProcessamento, font, color);
            pdf = page.Add(dataprocessamento);

            PdfTextElement nossonumero = new PdfTextElement(x + 430, y + 73, 140, bb.ExibeNossoNumero, font, color);
            nossonumero.HorizontalAlign = PdfTextHorizontalAlign.Right;
            pdf = page.Add(nossonumero);

            return pdf;
        }

        private PdfRenderingResult Linha2CarneDireita(BoletoNet.BoletoBancario bb, PdfPage page, ref PdfRenderingResult pdf, PdfFont font, ref Color color, int x, int y)
        {
            PdfTextElement agenciacodigo = new PdfTextElement(x + 430, y + 57, 140, bb.ExibeAgenciaCodigoCedente, font, color);
            agenciacodigo.HorizontalAlign = PdfTextHorizontalAlign.Right;
            pdf = page.Add(agenciacodigo);

            var index = y + 49;
            var liststr = bb.ExibeDadosCedente.Split(new string[] { "<br />" }, StringSplitOptions.None);
            foreach (var item in liststr)
            {
                PdfTextElement beneficiario = new PdfTextElement(x + 112, index, item, font, color);
                pdf = page.Add(beneficiario);
                index += 08;
            }

            return pdf;
        }

        private PdfRenderingResult Linha1CarneDireita(BoletoNet.BoletoBancario bb, PdfPage page, ref PdfRenderingResult pdf, PdfFont font, ref Color color, int x, int y)
        {
            font.Size = 08;

            PdfTextElement localpagamento = new PdfTextElement(x + 112, y + 34, bb.Boleto.LocalPagamento, font, color);
            pdf = page.Add(localpagamento);

            PdfTextElement vencimento = new PdfTextElement(x + 430, y + 34, 140, bb.ExibeDataVencimento, font, color);
            vencimento.HorizontalAlign = PdfTextHorizontalAlign.Right;
            pdf = page.Add(vencimento);

            return pdf;
        }

        private PdfRenderingResult HeaderCarneDireita(BoletoNet.BoletoBancario bb, string pathArquivosBoletoNet, PdfPage page, ref PdfRenderingResult pdf, PdfFont font, ref Color color, ref int x, int y)
        {
            var _codBanco = this.FormatCode(bb.Banco.Codigo.ToString(), "0", 3, true);
            var imgBanco = pathArquivosBoletoNet + _codBanco + ".jpg";

            PdfImageElement logo = new PdfImageElement(x + 112, y, 95, 25, imgBanco);
            pdf = page.Add(logo);

            x = x + 5;

            font.Size = 12;
            PdfTextElement codbanco = new PdfTextElement(x + 200, y + 09, 50, bb.ExibeCodigoBanco + "-" + bb.ExibeDigitoBanco, font, color);
            codbanco.HorizontalAlign = PdfTextHorizontalAlign.Center;
            pdf = page.Add(codbanco);

            font.Size = 11;
            PdfTextElement codboleto = new PdfTextElement(x + 170, y + 11, 400, bb.Boleto.CodigoBarra.LinhaDigitavel, font, color);
            codboleto.HorizontalAlign = PdfTextHorizontalAlign.Right;
            pdf = page.Add(codboleto);

            return pdf;
        }

        private PdfRenderingResult MontaCarneEsquerda(BoletoNet.BoletoBancario bb, string pathArquivosBoletoNet, PdfPage page, PdfRenderingResult pdf, PdfFont font, Color color, int x, int y)
        {
            var _codBanco = this.FormatCode(bb.Banco.Codigo.ToString(), "0", 3, true);
            var imgBanco = pathArquivosBoletoNet + _codBanco + ".jpg";

            PdfImageElement logo = new PdfImageElement(x + 1, y + 15, 106, 25, imgBanco);
            pdf = page.Add(logo);

            x = x + 5;

            font.Size = 07;
            PdfTextElement cedente = new PdfTextElement(x, y + 80, 100, bb.ExibeNomeCedente.Replace("<br />", "\n"), font, color);
            pdf = page.Add(cedente);

            font.Size = 08;
            PdfTextElement telefone = new PdfTextElement(x, y + 113, 100, "", font, color);
            pdf = page.Add(telefone);

            PdfTextElement vencimento = new PdfTextElement(x, y + 131, 100, bb.ExibeDataVencimento, font, color);
            pdf = page.Add(vencimento);

            PdfTextElement agencia = new PdfTextElement(x, y + 150, 150, bb.ExibeAgenciaCodigoCedente, font, color);
            pdf = page.Add(agencia);

            PdfTextElement numerodocumento = new PdfTextElement(x, y + 169, 100, bb.Boleto.NumeroDocumento, font, color);
            pdf = page.Add(numerodocumento);

            PdfTextElement valordocumento = new PdfTextElement(x, y + 187, 100, bb.ExibeValorDocumento, font, color);
            pdf = page.Add(valordocumento);

            PdfTextElement valorcobrado = new PdfTextElement(x, y + 205, 100, bb.ExibeValorCobrado, font, color);
            pdf = page.Add(valorcobrado);

            PdfTextElement sacado = new PdfTextElement(x, y + 223, 100, bb.ExibeSacado, font, color);
            pdf = page.Add(sacado);

            return pdf;
        }

        private PdfRenderingResult BackgroundCarne(BoletoNet.BoletoBancario bb, PdfPage page, string pathArquivosBoletoNet, int x, int y)
        {
            PdfRenderingResult pdf;

            var imgBoleto = pathArquivosBoletoNet + "layouts//";

            if (bb.Boleto.Banco.Codigo == (int)EBanco.Bradesco)
                imgBoleto += "carne-bradesco.png";

            else if (bb.Boleto.Banco.Codigo == (int)EBanco.HSBC)
                imgBoleto += "carne-hsbc.png";

            else if (bb.Boleto.Banco.Codigo == (int)EBanco.Caixa)
                imgBoleto += "carne-caixa.png";

            else if (bb.Boleto.Banco.Codigo == (int)EBanco.Itau)
                imgBoleto += "carne-itau.png";

            else if (bb.Boleto.Banco.Codigo == (int)EBanco.Santander)
                imgBoleto += "carne-santander.png";

            else if (bb.Boleto.Banco.Codigo == (int)EBanco.BancoDoBrasil && (bb.Boleto.Carteira.Equals("17-019") || bb.Boleto.Carteira.Equals("18-019")))
                imgBoleto += "carne-bb.png";

            else
                imgBoleto += "carne-padrao.png";

            PdfImageElement fundoBoleto = new PdfImageElement(x, y, imgBoleto);
            pdf = page.Add(fundoBoleto);

            return pdf;
        }
        #endregion

        #region Caixa
        private PdfRenderingResult MontaCaixaFichaCompensacao(BoletoNet.BoletoBancario bb, PdfRenderingResult pdf, PdfPage page, PdfFont font, Color color, string pathArquivosBoletoNet)
        {
            pdf = this.HeaderComumFichaCompensacao(bb, pdf, page, font, color, pathArquivosBoletoNet);
            pdf = this.Linha1ComumFichaCompensacao(bb, pdf, page, font, color);
            pdf = this.Linha2ComumFichaCompensacao(bb, pdf, page, font, color);
            pdf = this.Linha3ComumFichaCompensacao(bb, pdf, page, font, color);
            pdf = this.Linha4ComumFichaCompensacao(bb, pdf, page, font, color);
            pdf = this.Linha5ComumFichaCompensacao(bb, pdf, page, font, color);
            pdf = this.Linha6ComumFichaCompensacao(bb, pdf, page, font, color);
            pdf = this.FooterComumFichaCompensacao(bb, pdf, page, font, color);

            return pdf;
        }

        private PdfRenderingResult MontaCaixaReciboPagador(BoletoNet.BoletoBancario bb, PdfRenderingResult pdf, PdfPage page, PdfFont font, Color color, string pathArquivosBoletoNet)
        {
            pdf = this.HeaderCaixaReciboPagador(bb, pdf, page, font, color, pathArquivosBoletoNet);
            pdf = this.Linha1CaixaReciboPagador(bb, pdf, page, font, color);
            pdf = this.Linha2CaixaReciboPagador(bb, pdf, page, font, color);
            pdf = this.Linha3CaixaReciboPagador(bb, pdf, page, font, color);
            pdf = this.Linha4CaixaReciboPagador(bb, pdf, page, font, color);
            pdf = this.Linha5CaixaReciboPagador(bb, pdf, page, font, color);
            pdf = this.Linha6CaixaReciboPagador(bb, pdf, page, font, color);

            return pdf;
        }

        private PdfRenderingResult Linha6CaixaReciboPagador(BoletoNet.BoletoBancario bb, PdfRenderingResult pdf, PdfPage page, PdfFont font, Color color)
        {
            var y = 184 + 210;
            var x = 45;

            font.Size = 08;

            var liststr1 = bb.ExibeInstrucoesSacado.Split(new string[] { "<br />" }, StringSplitOptions.None);
            foreach (var item in liststr1)
            {
                PdfTextElement instrucao = new PdfTextElement(x, y, item, font, color);
                pdf = page.Add(instrucao);
                y += 09;
            }

            return pdf;
        }

        private PdfRenderingResult Linha5CaixaReciboPagador(BoletoNet.BoletoBancario bb, PdfRenderingResult pdf, PdfPage page, PdfFont font, Color color)
        {
            var y = 165 + 210;
            var x = 05;

            PdfTextElement carteira = new PdfTextElement(x, y, bb.ExibeDescricaoCarteira, font, color);
            pdf = page.Add(carteira);

            PdfTextElement especie = new PdfTextElement(x + 100, y, bb.ExibeEspecieDocumento, font, color);
            pdf = page.Add(especie);

            PdfTextElement vencimento = new PdfTextElement(x + 200, y, bb.ExibeDataVencimento, font, color);
            pdf = page.Add(vencimento);

            PdfTextElement valordocumento = new PdfTextElement(x + 299, y, bb.ExibeValorDocumento, font, color);
            pdf = page.Add(valordocumento);

            PdfTextElement valorcobrado = new PdfTextElement(x + 409, y, 140, bb.ExibeValorCobrado, font, color);
            valorcobrado.HorizontalAlign = PdfTextHorizontalAlign.Right;
            pdf = page.Add(valorcobrado);

            return pdf;
        }

        private PdfRenderingResult Linha4CaixaReciboPagador(BoletoNet.BoletoBancario bb, PdfRenderingResult pdf, PdfPage page, PdfFont font, Color color)
        {
            var y = 129 + 210;
            var x = 05;

            this.MontaSacado(bb, ref pdf, page, font, ref color, ref x, y);

            return pdf;
        }

        private PdfRenderingResult Linha3CaixaReciboPagador(BoletoNet.BoletoBancario bb, PdfRenderingResult pdf, PdfPage page, PdfFont font, Color color)
        {
            var y = 109 + 210;
            var x = 05;

            PdfTextElement datadocumento = new PdfTextElement(x, y, bb.ExibeDataDocumento, font, color);
            pdf = page.Add(datadocumento);

            PdfTextElement numerodocumento = new PdfTextElement(x + 100, y, bb.Boleto.NumeroDocumento, font, color);
            pdf = page.Add(numerodocumento);

            PdfTextElement aceite = new PdfTextElement(x + 200, y, bb.Boleto.Aceite, font, color);
            pdf = page.Add(aceite);

            PdfTextElement dataprocessamento = new PdfTextElement(x + 299, y, bb.ExibeDataProcessamento, font, color);
            pdf = page.Add(dataprocessamento);

            PdfTextElement nossonumero = new PdfTextElement(x + 409, y, 140, bb.ExibeNossoNumero, font, color);
            nossonumero.HorizontalAlign = PdfTextHorizontalAlign.Right;
            pdf = page.Add(nossonumero);

            return pdf;
        }

        private PdfRenderingResult Linha2CaixaReciboPagador(BoletoNet.BoletoBancario bb, PdfRenderingResult pdf, PdfPage page, PdfFont font, Color color)
        {
            var y = 89 + 210;
            var x = 05;

            PdfTextElement endereco = new PdfTextElement(x, y, bb.Cedente.Endereco.End, font, color);
            pdf = page.Add(endereco);

            PdfTextElement uf = new PdfTextElement(x + 323, y, bb.Cedente.Endereco.UF, font, color);
            pdf = page.Add(uf);

            PdfTextElement cep = new PdfTextElement(x + 409, y, 140, bb.Cedente.Endereco.CEP, font, color);
            cep.HorizontalAlign = PdfTextHorizontalAlign.Right;
            pdf = page.Add(cep);

            return pdf;
        }

        private PdfRenderingResult Linha1CaixaReciboPagador(BoletoNet.BoletoBancario bb, PdfRenderingResult pdf, PdfPage page, PdfFont font, Color color)
        {
            var y = 68 + 210;
            var x = 05;

            PdfTextElement nome = new PdfTextElement(x, y, bb.ExibeNomeCedente, font, color);
            pdf = page.Add(nome);

            PdfTextElement cpfcnpj = new PdfTextElement(x + 295, y, bb.Cedente.CPFCNPJ, font, color);
            pdf = page.Add(cpfcnpj);

            PdfTextElement agencia = new PdfTextElement(x + 409, y, 140, bb.ExibeAgenciaCodigoCedente, font, color);
            agencia.HorizontalAlign = PdfTextHorizontalAlign.Right;
            pdf = page.Add(agencia);

            return pdf;
        }

        private PdfRenderingResult HeaderCaixaReciboPagador(BoletoNet.BoletoBancario bb, PdfRenderingResult pdf, PdfPage page, PdfFont font, Color color, string pathArquivosBoletoNet)
        {
            var _codBanco = this.FormatCode(bb.Banco.Codigo.ToString(), "0", 3, true);
            var imgBanco = pathArquivosBoletoNet + _codBanco + ".jpg";

            var y = 28 + 210;

            PdfImageElement logo = new PdfImageElement(0, y, 112, 30, imgBanco);
            pdf = page.Add(logo);

            font.Size = 15;
            PdfTextElement codbanco = new PdfTextElement(114, y + 11, 50, bb.ExibeCodigoBanco + "-" + bb.ExibeDigitoBanco, font, color);
            codbanco.HorizontalAlign = PdfTextHorizontalAlign.Center;
            pdf = page.Add(codbanco);

            font.Size = 13;
            PdfTextElement codboleto = new PdfTextElement(154, y + 13, 400, bb.Boleto.CodigoBarra.LinhaDigitavel, font, color);
            codboleto.HorizontalAlign = PdfTextHorizontalAlign.Right;
            pdf = page.Add(codboleto);

            font.Size = 09;

            return pdf;
        }

        #endregion

        #region Comum
        private PdfRenderingResult MontaComumFichaCompensacao(BoletoNet.BoletoBancario bb, PdfRenderingResult pdf, PdfPage page, PdfFont font, Color color, string pathArquivosBoletoNet)
        {
            pdf = this.HeaderComumFichaCompensacao(bb, pdf, page, font, color, pathArquivosBoletoNet);
            pdf = this.Linha1ComumFichaCompensacao(bb, pdf, page, font, color);
            pdf = this.Linha2ComumFichaCompensacao(bb, pdf, page, font, color);
            pdf = this.Linha3ComumFichaCompensacao(bb, pdf, page, font, color);
            pdf = this.Linha4ComumFichaCompensacao(bb, pdf, page, font, color);
            pdf = this.Linha5ComumFichaCompensacao(bb, pdf, page, font, color);
            pdf = this.Linha6ComumFichaCompensacao(bb, pdf, page, font, color);
            pdf = this.FooterComumFichaCompensacao(bb, pdf, page, font, color);

            return pdf;
        }

        private PdfRenderingResult MontaComumReciboPagador(BoletoNet.BoletoBancario bb, PdfRenderingResult pdf, PdfPage page, PdfFont font, Color color, string pathArquivosBoletoNet)
        {
            pdf = this.HeaderComumReciboPagador(bb, pdf, page, font, color, pathArquivosBoletoNet);
            pdf = this.Linha1ComumReciboPagador(bb, pdf, page, font, color);
            pdf = this.Linha2ComumReciboPagador(bb, pdf, page, font, color);
            pdf = this.Linha3ComumReciboPagador(bb, pdf, page, font, color);
            pdf = this.Linha4ComumReciboPagador(bb, pdf, page, font, color);

            return pdf;
        }

        private PdfRenderingResult FooterComumFichaCompensacao(BoletoNet.BoletoBancario bb, PdfRenderingResult pdf, PdfPage page, PdfFont font, Color color)
        {
            PdfTextElement valordocumento = new PdfTextElement(355, 590 + 210, 140, "Ficha de Compensação", font, color);
            valordocumento.HorizontalAlign = PdfTextHorizontalAlign.Right;
            pdf = page.Add(valordocumento);

            var codigobarras = new BoletoNet.BoletoBancario().GeraImagemCodigoBarras(bb.Boleto);

            PdfImageElement imgcodbarras = new PdfImageElement(10, 570 + 210, codigobarras);
            pdf = page.Add(imgcodbarras);

            return pdf;
        }

        private PdfRenderingResult Linha6ComumFichaCompensacao(BoletoNet.BoletoBancario bb, PdfRenderingResult pdf, PdfPage page, PdfFont font, Color color)
        {
            var y = 525 + 210;
            var x = 05;

            this.MontaSacado(bb, ref pdf, page, font, ref color, ref x, y);

            font.Size = 10;
            PdfTextElement codbaixa = new PdfTextElement(410, 530 + 210, 140, "", font, color);
            pdf = page.Add(codbaixa);

            return pdf;
        }

        private PdfRenderingResult Linha5ComumFichaCompensacao(BoletoNet.BoletoBancario bb, PdfRenderingResult pdf, PdfPage page, PdfFont font, Color color)
        {
            var y = 414 + 210;
            var x = 05;

            var liststr = bb.ExibeInstrucoesCedente.Split(new string[] { "<br />" }, StringSplitOptions.None);
            foreach (var item in liststr)
            {
                PdfTextElement instrucao = new PdfTextElement(x, y, item, font, color);
                pdf = page.Add(instrucao);
                y += 10;
            }

            x = 413;
            y = 416 + 210;

            PdfTextElement desconto = new PdfTextElement(x, y, 140, bb.ExibeDescontos, font, color);
            desconto.HorizontalAlign = PdfTextHorizontalAlign.Right;
            pdf = page.Add(desconto);

            PdfTextElement deducoes = new PdfTextElement(x, y + 22, 140, bb.ExibeOutrasDeducoes, font, color);
            deducoes.HorizontalAlign = PdfTextHorizontalAlign.Right;
            pdf = page.Add(deducoes);

            PdfTextElement moramulta = new PdfTextElement(x, y + 44, 140, bb.ExibeMoraMulta, font, color);
            moramulta.HorizontalAlign = PdfTextHorizontalAlign.Right;
            pdf = page.Add(moramulta);

            PdfTextElement acrescimos = new PdfTextElement(x, y + 65, 140, bb.ExibeOutrosAcrescimos, font, color);
            acrescimos.HorizontalAlign = PdfTextHorizontalAlign.Right;
            pdf = page.Add(acrescimos);

            PdfTextElement valorocobrado = new PdfTextElement(x, y + 87, 140, bb.ExibeValorCobrado, font, color);
            valorocobrado.HorizontalAlign = PdfTextHorizontalAlign.Right;
            pdf = page.Add(valorocobrado);

            return pdf;
        }

        private PdfRenderingResult Linha4ComumFichaCompensacao(BoletoNet.BoletoBancario bb, PdfRenderingResult pdf, PdfPage page, PdfFont font, Color color)
        {
            var y = 395 + 210;
            var x = 05;

            PdfTextElement usodobanco = new PdfTextElement(x, y, "", font, color);
            pdf = page.Add(usodobanco);

            PdfTextElement carteira = new PdfTextElement(x + 96, y, bb.ExibeDescricaoCarteira, font, color);
            pdf = page.Add(carteira);

            PdfTextElement especie = new PdfTextElement(x + 170, y, bb.Boleto.Especie, font, color);
            pdf = page.Add(especie);

            PdfTextElement quantidade = new PdfTextElement(x + 220, y, bb.ExibeQuantidade, font, color);
            pdf = page.Add(quantidade);

            PdfTextElement valor = new PdfTextElement(x + 320, y, 70, bb.Boleto.ValorMoeda, font, color);
            valor.HorizontalAlign = PdfTextHorizontalAlign.Right;
            pdf = page.Add(valor);

            PdfTextElement valordocumento = new PdfTextElement(x + 410, y, 140, bb.ExibeValorDocumento, font, color);
            valordocumento.HorizontalAlign = PdfTextHorizontalAlign.Right;
            pdf = page.Add(valordocumento);

            return pdf;
        }

        private PdfRenderingResult Linha3ComumFichaCompensacao(BoletoNet.BoletoBancario bb, PdfRenderingResult pdf, PdfPage page, PdfFont font, Color color)
        {
            var y = 377 + 210;
            var x = 05;

            PdfTextElement datadocumento = new PdfTextElement(x, y, bb.ExibeDataDocumento, font, color);
            pdf = page.Add(datadocumento);

            PdfTextElement numerodocumento = new PdfTextElement(x + 98, y, bb.Boleto.NumeroDocumento, font, color);
            pdf = page.Add(numerodocumento);

            PdfTextElement especiedoc = new PdfTextElement(x + 224, y, bb.ExibeEspecieDocumento, font, color);
            pdf = page.Add(especiedoc);

            PdfTextElement aceite = new PdfTextElement(x + 280, y, bb.Boleto.Aceite, font, color);
            pdf = page.Add(aceite);

            PdfTextElement dataprocessamento = new PdfTextElement(x + 316, y, bb.ExibeDataProcessamento, font, color);
            pdf = page.Add(dataprocessamento);

            PdfTextElement nossonumero = new PdfTextElement(x + 410, y, 140, bb.ExibeNossoNumero, font, color);
            nossonumero.HorizontalAlign = PdfTextHorizontalAlign.Right;
            pdf = page.Add(nossonumero);

            return pdf;
        }

        private PdfRenderingResult Linha2ComumFichaCompensacao(BoletoNet.BoletoBancario bb, PdfRenderingResult pdf, PdfPage page, PdfFont font, Color color)
        {
            var y = 349 + 210;
            var x = 05;

            PdfTextElement agenciacodigo = new PdfTextElement(x + 410, y + 6, 140, bb.ExibeAgenciaCodigoCedente, font, color);
            agenciacodigo.HorizontalAlign = PdfTextHorizontalAlign.Right;
            pdf = page.Add(agenciacodigo);

            var liststr1 = bb.ExibeDadosCedente.Split(new string[] { "<br />" }, StringSplitOptions.None);
            foreach (var item in liststr1)
            {
                PdfTextElement beneficiario = new PdfTextElement(x, y, item, font, color);
                pdf = page.Add(beneficiario);
                y += 08;
            }

            return pdf;
        }

        private PdfRenderingResult Linha1ComumFichaCompensacao(BoletoNet.BoletoBancario bb, PdfRenderingResult pdf, PdfPage page, PdfFont font, Color color)
        {
            var y = 330 + 210;
            var x = 05;

            PdfTextElement localpagamento = new PdfTextElement(x, y, bb.Boleto.LocalPagamento, font, color);
            pdf = page.Add(localpagamento);

            PdfTextElement vencimento = new PdfTextElement(x + 410, y, 140, bb.ExibeDataVencimento, font, color);
            vencimento.HorizontalAlign = PdfTextHorizontalAlign.Right;
            pdf = page.Add(vencimento);

            return pdf;
        }

        private PdfRenderingResult HeaderComumFichaCompensacao(BoletoNet.BoletoBancario bb, PdfRenderingResult pdf, PdfPage page, PdfFont font, Color color, string pathArquivosBoletoNet)
        {
            var _codBanco = this.FormatCode(bb.Banco.Codigo.ToString(), "0", 3, true);
            var imgBanco = pathArquivosBoletoNet + _codBanco + ".jpg";

            var y = 291 + 210;

            PdfImageElement logo = new PdfImageElement(0, y, 112, 30, imgBanco);
            pdf = page.Add(logo);

            font.Size = 15;
            PdfTextElement codbanco = new PdfTextElement(114, y + 11, 50, bb.ExibeCodigoBanco + "-" + bb.ExibeDigitoBanco, font, color);
            codbanco.HorizontalAlign = PdfTextHorizontalAlign.Center;
            pdf = page.Add(codbanco);

            font.Size = 13;
            PdfTextElement codboleto = new PdfTextElement(154, y + 13, 400, bb.Boleto.CodigoBarra.LinhaDigitavel, font, color);
            codboleto.HorizontalAlign = PdfTextHorizontalAlign.Right;
            pdf = page.Add(codboleto);

            font.Size = 09;

            return pdf;
        }


        private PdfRenderingResult Linha4ComumReciboPagador(BoletoNet.BoletoBancario bb, PdfRenderingResult pdf, PdfPage page, PdfFont font, Color color)
        {
            var y = 127 + 210;
            var x = 05;

            this.MontaSacado(bb, ref pdf, page, font, ref color, ref x, y);

            return pdf;
        }

        private PdfRenderingResult Linha3ComumReciboPagador(BoletoNet.BoletoBancario bb, PdfRenderingResult pdf, PdfPage page, PdfFont font, Color color)
        {
            var y = 106 + 210;
            var x = 05;

            PdfTextElement descontos = new PdfTextElement(x + 22, y, 70, bb.ExibeDescontos, font, color);
            descontos.HorizontalAlign = PdfTextHorizontalAlign.Right;
            pdf = page.Add(descontos);

            PdfTextElement deducoes = new PdfTextElement(x + 120, y, 70, bb.ExibeOutrasDeducoes, font, color);
            deducoes.HorizontalAlign = PdfTextHorizontalAlign.Right;
            pdf = page.Add(deducoes);

            PdfTextElement moramulta = new PdfTextElement(x + 220, y, 70, bb.ExibeMoraMulta, font, color);
            moramulta.HorizontalAlign = PdfTextHorizontalAlign.Right;
            pdf = page.Add(moramulta);

            PdfTextElement acrescimos = new PdfTextElement(x + 321, y, 70, bb.ExibeOutrosAcrescimos, font, color);
            acrescimos.HorizontalAlign = PdfTextHorizontalAlign.Right;
            pdf = page.Add(acrescimos);

            PdfTextElement valorcobrado = new PdfTextElement(x + 410, y, 140, 70, bb.ExibeValorCobrado, font, color);
            valorcobrado.HorizontalAlign = PdfTextHorizontalAlign.Right;
            pdf = page.Add(valorcobrado);

            return pdf;
        }

        private PdfRenderingResult Linha2ComumReciboPagador(BoletoNet.BoletoBancario bb, PdfRenderingResult pdf, PdfPage page, PdfFont font, Color color)
        {
            var y = 87 + 210;
            var x = 05;

            PdfTextElement numerodocumento = new PdfTextElement(x, y, bb.Boleto.NumeroDocumento, font, color);
            pdf = page.Add(numerodocumento);

            PdfTextElement cpfcnpj = new PdfTextElement(x + 165, y, bb.Cedente.CPFCNPJ, font, color);
            pdf = page.Add(cpfcnpj);

            PdfTextElement vencimento = new PdfTextElement(x + 280, y, bb.ExibeDataVencimento, font, color);
            pdf = page.Add(vencimento);

            PdfTextElement valordocumento = new PdfTextElement(x + 410, y, 140, bb.ExibeValorDocumento, font, color);
            valordocumento.HorizontalAlign = PdfTextHorizontalAlign.Right;
            pdf = page.Add(valordocumento);

            return pdf;
        }

        private PdfRenderingResult Linha1ComumReciboPagador(BoletoNet.BoletoBancario bb, PdfRenderingResult pdf, PdfPage page, PdfFont font, Color color)
        {
            var y = 68 + 210;
            var x = 05;

            PdfTextElement nome = new PdfTextElement(x, y, bb.ExibeNomeCedente, font, color);
            pdf = page.Add(nome);

            PdfTextElement agencia = new PdfTextElement(x + 260, y, bb.ExibeAgenciaCodigoCedente, font, color);
            pdf = page.Add(agencia);

            PdfTextElement nossonumero = new PdfTextElement(x + 410, y, 140, bb.ExibeNossoNumero, font, color);
            nossonumero.HorizontalAlign = PdfTextHorizontalAlign.Right;
            pdf = page.Add(nossonumero);

            return pdf;
        }

        private PdfRenderingResult HeaderComumReciboPagador(BoletoNet.BoletoBancario bb, PdfRenderingResult pdf, PdfPage page, PdfFont font, Color color, string pathArquivosBoletoNet)
        {
            var _codBanco = this.FormatCode(bb.Banco.Codigo.ToString(), "0", 3, true);
            var imgBanco = pathArquivosBoletoNet + _codBanco + ".jpg";

            var y = 28 + 210;

            PdfImageElement logo = new PdfImageElement(0, y, 112, 30, imgBanco);
            pdf = page.Add(logo);

            font.Size = 15;
            PdfTextElement codbanco = new PdfTextElement(114, y + 11, 50, bb.ExibeCodigoBanco + "-" + bb.ExibeDigitoBanco, font, color);
            codbanco.HorizontalAlign = PdfTextHorizontalAlign.Center;
            pdf = page.Add(codbanco);

            font.Size = 13;
            PdfTextElement codboleto = new PdfTextElement(154, y + 13, 400, bb.Boleto.CodigoBarra.LinhaDigitavel, font, color);
            codboleto.HorizontalAlign = PdfTextHorizontalAlign.Right;
            pdf = page.Add(codboleto);

            font.Size = 09;

            return pdf;
        }

        private PdfRenderingResult BackgroundComum(BoletoNet.BoletoBancario bb, PdfPage page, string pathArquivosBoletoNet)
        {
            PdfRenderingResult pdf;

            var imgBoleto = pathArquivosBoletoNet + "layouts//";

            if (bb.Boleto.Banco.Codigo == (int)EBanco.Bradesco)
                imgBoleto += "comum-bradesco.png";

            else if (bb.Boleto.Banco.Codigo == (int)EBanco.HSBC)
                imgBoleto += "comum-hsbc.png";

            else if (bb.Boleto.Banco.Codigo == (int)EBanco.Caixa)
                imgBoleto += "comum-caixa.png";

            else if (bb.Boleto.Banco.Codigo == (int)EBanco.Itau)
                imgBoleto += "comum-itau.png";

            else if (bb.Boleto.Banco.Codigo == (int)EBanco.Santander)
                imgBoleto += "comum-santander.png";

            else if (bb.Boleto.Banco.Codigo == (int)EBanco.BancoDoBrasil)
                imgBoleto += "comum-bb.png";

            else
                imgBoleto += "comum-padrao.png";

            PdfImageElement fundoBoleto = new PdfImageElement(0, 210, imgBoleto);
            pdf = page.Add(fundoBoleto);
            return pdf;
        }
        #endregion

        #region Banco do Brasil
        private PdfRenderingResult MontaBancoDoBrasilFichaCompensacao(BoletoNet.BoletoBancario bb, PdfRenderingResult pdf, PdfPage page, PdfFont font, Color color, string pathArquivosBoletoNet)
        {
            pdf = this.HeaderComumFichaCompensacao(bb, pdf, page, font, color, pathArquivosBoletoNet);
            pdf = this.Linha1ComumFichaCompensacao(bb, pdf, page, font, color);
            pdf = this.Linha2ComumFichaCompensacao(bb, pdf, page, font, color);
            pdf = this.Linha3ComumFichaCompensacao(bb, pdf, page, font, color);
            pdf = this.Linha4ComumFichaCompensacao(bb, pdf, page, font, color);
            pdf = this.Linha5ComumFichaCompensacao(bb, pdf, page, font, color);
            pdf = this.Linha6ComumFichaCompensacao(bb, pdf, page, font, color);
            pdf = this.FooterComumFichaCompensacao(bb, pdf, page, font, color);

            return pdf;
        }

        private PdfRenderingResult MontaBancoDoBrasilReciboPagador(BoletoNet.BoletoBancario bb, PdfRenderingResult pdf, PdfPage page, PdfFont font, Color color, string pathArquivosBoletoNet)
        {
            pdf = this.HeaderBancoDoBrasilReciboPagador(bb, pdf, page, font, color, pathArquivosBoletoNet);
            pdf = this.Linha1BancoDoBrasilReciboPagador(bb, pdf, page, font, color);
            pdf = this.Linha2BancoDoBrasilReciboPagador(bb, pdf, page, font, color);
            pdf = this.Linha3BancoDoBrasilReciboPagador(bb, pdf, page, font, color);
            pdf = this.Linha4BancoDoBrasilReciboPagador(bb, pdf, page, font, color);

            return pdf;
        }

        private PdfRenderingResult Linha4BancoDoBrasilReciboPagador(BoletoNet.BoletoBancario bb, PdfRenderingResult pdf, PdfPage page, PdfFont font, Color color)
        {
            var y = 165 + 210;
            var x = 05;

            this.MontaSacado(bb, ref pdf, page, font, ref color, ref x, y);

            return pdf;
        }

        private PdfRenderingResult Linha3BancoDoBrasilReciboPagador(BoletoNet.BoletoBancario bb, PdfRenderingResult pdf, PdfPage page, PdfFont font, Color color)
        {
            var y = 144 + 210;
            var x = 05;

            PdfTextElement descontos = new PdfTextElement(x + 22, y, 70, bb.ExibeDescontos, font, color);
            descontos.HorizontalAlign = PdfTextHorizontalAlign.Right;
            pdf = page.Add(descontos);

            PdfTextElement deducoes = new PdfTextElement(x + 120, y, 70, bb.ExibeOutrasDeducoes, font, color);
            deducoes.HorizontalAlign = PdfTextHorizontalAlign.Right;
            pdf = page.Add(deducoes);

            PdfTextElement moramulta = new PdfTextElement(x + 220, y, 70, bb.ExibeMoraMulta, font, color);
            moramulta.HorizontalAlign = PdfTextHorizontalAlign.Right;
            pdf = page.Add(moramulta);

            PdfTextElement acrescimos = new PdfTextElement(x + 321, y, 70, bb.ExibeOutrosAcrescimos, font, color);
            acrescimos.HorizontalAlign = PdfTextHorizontalAlign.Right;
            pdf = page.Add(acrescimos);

            PdfTextElement valorcobrado = new PdfTextElement(x + 410, y, 140, 70, bb.ExibeValorCobrado, font, color);
            valorcobrado.HorizontalAlign = PdfTextHorizontalAlign.Right;
            pdf = page.Add(valorcobrado);

            return pdf;
        }

        private PdfRenderingResult Linha2BancoDoBrasilReciboPagador(BoletoNet.BoletoBancario bb, PdfRenderingResult pdf, PdfPage page, PdfFont font, Color color)
        {
            var y = 125 + 210;
            var x = 05;

            PdfTextElement numerodocumento = new PdfTextElement(x, y, bb.Boleto.NumeroDocumento, font, color);
            pdf = page.Add(numerodocumento);

            PdfTextElement cpfcnpj = new PdfTextElement(x + 165, y, bb.Cedente.CPFCNPJ, font, color);
            pdf = page.Add(cpfcnpj);

            PdfTextElement vencimento = new PdfTextElement(x + 280, y, bb.ExibeDataVencimento, font, color);
            pdf = page.Add(vencimento);

            PdfTextElement valordocumento = new PdfTextElement(x + 410, y, 140, bb.ExibeValorDocumento, font, color);
            valordocumento.HorizontalAlign = PdfTextHorizontalAlign.Right;
            pdf = page.Add(valordocumento);

            return pdf;
        }

        private PdfRenderingResult Linha1BancoDoBrasilReciboPagador(BoletoNet.BoletoBancario bb, PdfRenderingResult pdf, PdfPage page, PdfFont font, Color color)
        {
            var y = 68 + 210;
            var x = 05;

            PdfTextElement nome = new PdfTextElement(x, y, 250, bb.ExibeNomeCedente.Replace("<br />", "\n"), font, color);
            pdf = page.Add(nome);

            PdfTextElement agencia = new PdfTextElement(x + 260, y, bb.ExibeAgenciaCodigoCedente, font, color);
            pdf = page.Add(agencia);

            PdfTextElement nossonumero = new PdfTextElement(x + 410, y, 140, bb.ExibeNossoNumero, font, color);
            nossonumero.HorizontalAlign = PdfTextHorizontalAlign.Right;
            pdf = page.Add(nossonumero);

            return pdf;
        }

        private PdfRenderingResult HeaderBancoDoBrasilReciboPagador(BoletoNet.BoletoBancario bb, PdfRenderingResult pdf, PdfPage page, PdfFont font, Color color, string pathArquivosBoletoNet)
        {
            var _codBanco = this.FormatCode(bb.Banco.Codigo.ToString(), "0", 3, true);
            var imgBanco = pathArquivosBoletoNet + _codBanco + ".jpg";

            var y = 28 + 210;

            PdfImageElement logo = new PdfImageElement(0, y, 112, 30, imgBanco);
            pdf = page.Add(logo);

            font.Size = 15;
            PdfTextElement codbanco = new PdfTextElement(114, y + 11, 50, bb.ExibeCodigoBanco + "-" + bb.ExibeDigitoBanco, font, color);
            codbanco.HorizontalAlign = PdfTextHorizontalAlign.Center;
            pdf = page.Add(codbanco);

            font.Size = 13;
            PdfTextElement codboleto = new PdfTextElement(154, y + 13, 400, bb.Boleto.CodigoBarra.LinhaDigitavel, font, color);
            codboleto.HorizontalAlign = PdfTextHorizontalAlign.Right;
            pdf = page.Add(codboleto);

            font.Size = 09;

            return pdf;
        }

        #endregion

        private void MontaSacado(BoletoNet.BoletoBancario bb, ref PdfRenderingResult pdf, PdfPage page, PdfFont font, ref Color color, ref int x, int y)
        {
            font.Size = 08;

            var liststr1 = bb.ExibeSacado.Split(new string[] { "<br />" }, StringSplitOptions.None);
            foreach (var item in liststr1)
            {
                PdfTextElement pagador = new PdfTextElement(x, y, item, font, color);
                pdf = page.Add(pagador);
                y += 08;
            }

            var liststr2 = bb.ExibeInfoSacado.Split(new string[] { "<br />" }, StringSplitOptions.None);
            foreach (var item in liststr2)
            {
                PdfTextElement pagador = new PdfTextElement(x, y, item, font, color);
                pdf = page.Add(pagador);
                y += 08;
            }
        }

        private PdfRenderingResult MakePages(PdfDocument doc, BoletoNet.BoletoBancario bb, string pathArquivosBoletoNet, PdfPage page, BoletoCobranca boletoCobranca)
        {
            PdfRenderingResult pdf;
            PdfFont font = doc.AddFont(PdfStandardFont.HelveticaBold); font.Size = 09;
            Color color = Color.Black;

            if (boletoCobranca.Sacado.Alunos != null && boletoCobranca.Sacado.Alunos.Count() > 0)
                pdf = this.MontaTabelaInformacao(bb, page, boletoCobranca, font, color);

            pdf = this.BackgroundComum(bb, page, pathArquivosBoletoNet);

            if (bb.Boleto.Banco.Codigo == (int)EBanco.Caixa)
            {
                pdf = this.MontaCaixaReciboPagador(bb, pdf, page, font, color, pathArquivosBoletoNet);
                pdf = this.MontaCaixaFichaCompensacao(bb, pdf, page, font, color, pathArquivosBoletoNet);
            }
            else if (bb.Boleto.Banco.Codigo == (int)EBanco.BancoDoBrasil)
            {
                pdf = this.MontaBancoDoBrasilReciboPagador(bb, pdf, page, font, color, pathArquivosBoletoNet);
                pdf = this.MontaBancoDoBrasilFichaCompensacao(bb, pdf, page, font, color, pathArquivosBoletoNet);
            }
            else
            {
                pdf = this.MontaComumReciboPagador(bb, pdf, page, font, color, pathArquivosBoletoNet);
                pdf = this.MontaComumFichaCompensacao(bb, pdf, page, font, color, pathArquivosBoletoNet);
            }

            return pdf;
        }

        private PdfRenderingResult MontaTabelaInformacao(BoletoNet.BoletoBancario bb, PdfPage page, BoletoCobranca boletoCobranca, PdfFont font, Color color)
        {
            PdfRenderingResult pdf;

            var x = 0;
            var y = 30;

            pdf = this.MontagemEstruturaTabela(page, font, ref color);

            foreach (var aluno in boletoCobranca.Sacado.Alunos)
            {
                pdf = this.MontaDadosDoAluno(aluno, page, font, color, x, y);
                y += 15;
            }

            return pdf;
        }

        private PdfRenderingResult MontaDadosDoAluno(AlunoBoleto aluno, PdfPage page, PdfFont font, Color color, int x, int y)
        {
            PdfRenderingResult pdf;

            var tabela = new PdfRectangleElement(x, y, 585, 15);
            var linhaPadrao = new PdfLineStyle();
            linhaPadrao.LineCapStyle = PdfLineCapStyle.Default;
            linhaPadrao.LineWidth = 1;
            tabela.LineStyle = linhaPadrao;
            pdf = page.Add(tabela);

            y += 3;

            PdfTextElement nomeAluno = new PdfTextElement(x + 2, y, 253, aluno.Nome, font, color);
            nomeAluno.HorizontalAlign = PdfTextHorizontalAlign.Left;
            pdf = page.Add(nomeAluno);

            PdfTextElement idiomaAluno = new PdfTextElement(x + 207, y, 78, aluno.Idioma, font, color);
            idiomaAluno.HorizontalAlign = PdfTextHorizontalAlign.Left;
            pdf = page.Add(idiomaAluno);

            PdfTextElement turmaAluno = new PdfTextElement(x + 287, y, 78, aluno.Turma, font, color);
            turmaAluno.HorizontalAlign = PdfTextHorizontalAlign.Left;
            pdf = page.Add(turmaAluno);

            PdfTextElement categoriaFinanceiroAluno = new PdfTextElement(x + 367, y, 138, aluno.CategoriaFinanceiro, font, color);
            categoriaFinanceiroAluno.HorizontalAlign = PdfTextHorizontalAlign.Left;
            pdf = page.Add(categoriaFinanceiroAluno);

            PdfTextElement numeroParcelaoAluno = new PdfTextElement(x + 507, y, 78, aluno.NumeroParcela, font, color);
            numeroParcelaoAluno.HorizontalAlign = PdfTextHorizontalAlign.Center;
            pdf = page.Add(numeroParcelaoAluno);

            return pdf;
        }

        private PdfRenderingResult MontagemEstruturaTabela(PdfPage page, PdfFont font, ref Color color)
        {
            PdfRenderingResult pdf;
            var tabela = new PdfRectangleElement(0, 0, 585, 200);
            var linhaPadrao = new PdfLineStyle();
            linhaPadrao.LineCapStyle = PdfLineCapStyle.Default;
            linhaPadrao.LineWidth = 1;
            tabela.LineStyle = linhaPadrao;
            pdf = page.Add(tabela);

            tabela = new PdfRectangleElement(0, 0, 585, 30);
            tabela.BackColor = Color.LightGray;
            linhaPadrao = new PdfLineStyle();
            pdf = page.Add(tabela);

            tabela = new PdfRectangleElement(0, 0, 585, 30);
            linhaPadrao = new PdfLineStyle();
            linhaPadrao.LineCapStyle = PdfLineCapStyle.Default;
            linhaPadrao.LineWidth = 1;
            tabela.LineStyle = linhaPadrao;
            pdf = page.Add(tabela);

            PdfTextElement tituloNomeAluno = new PdfTextElement(50, 10, 100, "Nome do Aluno", font, color);
            tituloNomeAluno.HorizontalAlign = PdfTextHorizontalAlign.Center;
            pdf = page.Add(tituloNomeAluno);

            //tabela = new PdfRectangleElement(0, 0, 205, 200);
            tabela = new PdfRectangleElement(0, 0, 365, 200);
            linhaPadrao = new PdfLineStyle();
            linhaPadrao.LineCapStyle = PdfLineCapStyle.Default;
            linhaPadrao.LineWidth = 1;
            tabela.LineStyle = linhaPadrao;
            pdf = page.Add(tabela);

            //PdfTextElement tituloIdioma = new PdfTextElement(195, 10, 100, "Idioma", font, color);
            //tituloIdioma.HorizontalAlign = PdfTextHorizontalAlign.Center;
            //pdf = page.Add(tituloIdioma);

            //tabela = new PdfRectangleElement(205, 0, 80, 200);
            //linhaPadrao = new PdfLineStyle();
            //linhaPadrao.LineCapStyle = PdfLineCapStyle.Default;
            //linhaPadrao.LineWidth = 1;
            //tabela.LineStyle = linhaPadrao;
            //pdf = page.Add(tabela);

            //PdfTextElement tituloTurma = new PdfTextElement(275, 10, 100, "Turma", font, color);
            //tituloTurma.HorizontalAlign = PdfTextHorizontalAlign.Center;
            //pdf = page.Add(tituloTurma);

            //tabela = new PdfRectangleElement(285, 0, 80, 200);
            //linhaPadrao = new PdfLineStyle();
            //linhaPadrao.LineCapStyle = PdfLineCapStyle.Default;
            //linhaPadrao.LineWidth = 1;
            //tabela.LineStyle = linhaPadrao;
            //pdf = page.Add(tabela);

            PdfTextElement tituloCategoriaFinanceiro = new PdfTextElement(385, 10, 100, "Categoria", font, color);
            tituloCategoriaFinanceiro.HorizontalAlign = PdfTextHorizontalAlign.Center;
            pdf = page.Add(tituloCategoriaFinanceiro);

            tabela = new PdfRectangleElement(365, 0, 140, 200);
            linhaPadrao = new PdfLineStyle();
            linhaPadrao.LineCapStyle = PdfLineCapStyle.Default;
            linhaPadrao.LineWidth = 1;
            tabela.LineStyle = linhaPadrao;
            pdf = page.Add(tabela);

            PdfTextElement tituloParcela = new PdfTextElement(495, 10, 100, "Parcela", font, color);
            tituloParcela.HorizontalAlign = PdfTextHorizontalAlign.Center;
            pdf = page.Add(tituloParcela);

            tabela = new PdfRectangleElement(505, 0, 80, 200);
            linhaPadrao = new PdfLineStyle();
            linhaPadrao.LineCapStyle = PdfLineCapStyle.Default;
            linhaPadrao.LineWidth = 1;
            tabela.LineStyle = linhaPadrao;
            pdf = page.Add(tabela);

            return pdf;
        }

        private string FormatCode(string text, string with, int length, bool left)
        {
            length -= text.Length;
            if (left)
            {
                for (int i = 0; i < length; ++i)
                {
                    text = with + text;
                }
            }
            else
            {
                for (int i = 0; i < length; ++i)
                {
                    text += with;
                }
            }
            return text;
        }
    }
}