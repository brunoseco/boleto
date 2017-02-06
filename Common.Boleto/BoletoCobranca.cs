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
    public class BoletoCobranca
    {

        public Boleto Boleto { get; set; }
        public Cedente Cedente { get; set; }
        public Sacado Sacado { get; set; }

        public BoletoCobranca()
        {
            this.Boleto = new Boleto();
            this.Cedente = new Cedente();
            this.Sacado = new Sacado();
        }
        
        protected BoletoNet.BoletoBancario MontarBoleto(BoletoCobranca cobranca, bool segundaVia)
        {
            var codigoBanco = Convert.ToInt32(cobranca.Boleto.CodigoBanco);
            var boleto = new BoletoNet.Boleto();
            var bb = new BoletoNet.BoletoBancario();
            var boletoCedente = new BoletoNet.Cedente();

            this.ConfiguraBoletoCedente(cobranca.Cedente, boletoCedente);

            var nossoNumero = this.ConfiguraNossoNumero(cobranca, codigoBanco);

            this.ConfiguraBoleto(cobranca, segundaVia, boletoCedente, nossoNumero, boleto);

            this.ConfiguraEspecieDocumentoBoleto(codigoBanco, boleto);

            this.ConfiguraNumeroParcela(codigoBanco, boleto);

            this.ConfiguraPercentualMultaJurosMora(cobranca, boleto);

            this.ConfiguraInstrucoesBoleto(cobranca, segundaVia, codigoBanco, boleto);

            this.ConfiguraBoletoBancario(cobranca, codigoBanco, boleto, bb);

            return bb;
        }

        private void ConfiguraBoletoCedente(Cedente cedente, BoletoNet.Cedente boletoCedente)
        {
            boletoCedente.CPFCNPJ = Convert.ToString(cedente.CpfCnpj);
            boletoCedente.Nome = Convert.ToString(cedente.Nome);
            boletoCedente.Codigo = cedente.CodigoConvenio.ToString();
            boletoCedente.Convenio = cedente.CodigoConvenio;

            boletoCedente.ContaBancaria.Agencia = Convert.ToString(cedente.Agencia);
            boletoCedente.ContaBancaria.DigitoAgencia = Convert.ToString(cedente.DigitoAgencia);
            boletoCedente.ContaBancaria.Conta = Convert.ToString(cedente.ContaCorrente);
            boletoCedente.ContaBancaria.DigitoConta = Convert.ToString(cedente.DigitoConta);
            boletoCedente.ContaBancaria.OperacaoConta = "003";

            boletoCedente.Endereco = new BoletoNet.Endereco();
            boletoCedente.Endereco.UF = Convert.ToString(cedente.UF);
            boletoCedente.Endereco.CEP = Convert.ToString(cedente.CEP);
            boletoCedente.Endereco.End = Convert.ToString(cedente.Endereco) + " - " + Convert.ToString(cedente.Cidade);
        }

        private void ConfiguraBoletoBancario(BoletoCobranca cobranca, int codigoBanco, BoletoNet.Boleto boleto, BoletoNet.BoletoBancario bb)
        {
            bb.CodigoBanco = Convert.ToInt16(codigoBanco);
            bb.Boleto = boleto;
            bb.FormatoCarne = cobranca.Boleto.Carne;
            bb.OcultarInstrucoes = true;
            bb.Boleto.Valida();
            bb.MostrarComprovanteEntrega = false;
        }

        private string ConfiguraNossoNumero(BoletoCobranca cobranca, int codigoBanco)
        {
            var nossoNumero = string.Empty;

            if (codigoBanco.Equals((int)EBanco.BancoDoBrasil))
            {
                if (!cobranca.Boleto.CodigoCarteira.Equals("16") && !cobranca.Boleto.CodigoCarteira.Equals("18"))
                {
                    nossoNumero = cobranca.Boleto.NossoNumero.ToString().PadLeft(10, '0');
                }
                else
                {
                    if (cobranca.Boleto.NossoNumero.ToString().Length < 5)
                        nossoNumero = cobranca.Boleto.NossoNumero.ToString().PadLeft(5, '0');
                    else
                        nossoNumero = cobranca.Boleto.NossoNumero.ToString().Substring(0, 5);
                }
            }
            else if (codigoBanco.Equals((int)EBanco.Bradesco))
            {
                nossoNumero = cobranca.Boleto.NossoNumero.ToString().PadLeft(7, '0');
            }
            else if (codigoBanco.Equals((int)EBanco.Caixa))
            {
                if ((cobranca.Boleto.CodigoCarteira.Equals("SR") || cobranca.Boleto.CodigoCarteira.Equals("RG")) && cobranca.Cedente.CodigoConvenio.ToString().Count().Equals(6))
                    nossoNumero = cobranca.Boleto.NossoNumero.ToString().PadLeft(15, '0');
                else
                    nossoNumero = cobranca.Boleto.NossoNumero.ToString().PadLeft(8, '0');
            }
            else
            {
                nossoNumero = cobranca.Boleto.NossoNumero.ToString().PadLeft(7, '0');
            }

            return nossoNumero;
        }

        private void ConfiguraBoleto(BoletoCobranca cobranca, bool segundaVia, BoletoNet.Cedente boletoCedente, string nossoNumero, BoletoNet.Boleto boleto)
        {
            boleto.DataVencimento = cobranca.Boleto.DataVencimento;
            boleto.ValorBoleto = cobranca.Boleto.Valor;
            boleto.Carteira = cobranca.Boleto.CodigoCarteira;
            boleto.NossoNumero = nossoNumero;
            boleto.Cedente = boletoCedente;
            boleto.Cedente = boletoCedente;
            boleto.NumeroDocumento = cobranca.Boleto.NossoNumero;
            boleto.Sacado = new BoletoNet.Sacado(cobranca.Sacado.CpfCnpj, cobranca.Sacado.Nome);
            boleto.Sacado.Endereco.End = cobranca.Sacado.Endereco + " " + cobranca.Sacado.Complemento + "<br />" + cobranca.Sacado.Bairro + " - " + cobranca.Sacado.Cidade + " - " + cobranca.Sacado.UF + " - " + cobranca.Sacado.CEP.Replace("-", "");
            boleto.Sacado.Email = cobranca.Sacado.Email;
            boleto.TipoModalidade = "01";
            boleto.BancoGeraBoleto = false;
            boleto.SegundaVia = segundaVia;
            boleto.Banco = new BoletoNet.Banco(Convert.ToInt32(cobranca.Boleto.CodigoBanco));
        }

        private void ConfiguraInstrucoesBoleto(BoletoCobranca cobranca, bool segundaVia, int codigoBanco, BoletoNet.Boleto boleto)
        {
            var instrucoes = new BoletoNet.Instrucao(Convert.ToInt32(codigoBanco));

            if (!segundaVia)
                this.ConfiguraBoletoPrimeiraVia(cobranca, boleto, instrucoes);
            else
                this.ConfiguraBoletoSegundaVia(cobranca, boleto, instrucoes);
        }

        private void ConfiguraPercentualMultaJurosMora(BoletoCobranca cobranca, BoletoNet.Boleto boleto)
        {
            boleto.PercMulta = cobranca.Boleto.PorcentagemMultaPorAtraso;
            boleto.PercJurosMora = cobranca.Boleto.PorcentagemJurosMoraDia;
        }

        private void ConfiguraEspecieDocumentoBoleto(int codigoBanco, BoletoNet.Boleto boleto)
        {
            var cod = "2";

            if (codigoBanco.Equals((int)EBanco.Bradesco))
                cod = "1";

            else if (codigoBanco.Equals((int)EBanco.Itau))
                cod = "1";

            else if (codigoBanco.Equals((int)EBanco.Sicoob))
                cod = "1";

            else if (codigoBanco.Equals((int)EBanco.Caixa))
                cod = "1";

            else if (codigoBanco.Equals((int)EBanco.Santander))
                cod = "1";

            boleto.EspecieDocumento = new BoletoNet.EspecieDocumento(Convert.ToInt32(codigoBanco), cod);
        }

        private void ConfiguraNumeroParcela(int codigoBanco, BoletoNet.Boleto boleto)
        {
            if (codigoBanco.Equals((int)EBanco.Sicoob))
                boleto.NumeroParcela = 1;
        }

        private void ConfiguraBoletoSegundaVia(BoletoCobranca cobranca, BoletoNet.Boleto boleto, BoletoNet.Instrucao instrucoes)
        {
            boleto.ValorMulta = cobranca.Boleto.ValorMulta;
            boleto.OutrosAcrescimos = cobranca.Boleto.ValorJuros;
            boleto.ValorCobrado = cobranca.Boleto.ValorSegundaVia;

            if (cobranca.Boleto.Instrucoes.Count() > 0)
            {
                if (cobranca.Boleto.Instrucoes.FirstOrDefault().Equals(MensagemPadrao.SEGUNDA_VIA))
                    instrucoes.Descricao += cobranca.Boleto.Instrucoes.FirstOrDefault();
            }

            boleto.Cedente.Instrucoes.Add(instrucoes);
        }

        private void ConfiguraBoletoPrimeiraVia(BoletoCobranca cobranca, BoletoNet.Boleto boleto, BoletoNet.Instrucao instrucoes)
        {
            boleto.DataDocumento = DateTime.Today;

            foreach (var item in cobranca.Boleto.Instrucoes)
            {
                var validItem = item;

                if (cobranca.Boleto.TemDesconto)
                    if (item.Equals(MensagemPadrao.DESCONTO_PONTUALIDADE))
                        validItem = string.Format(item, cobranca.Boleto.ValorDesconto);

                if (item.Equals(MensagemPadrao.MULTA_APOS_VENCTO))
                    validItem = string.Format(item, Math.Round(boleto.ValorBoleto * (boleto.PercMulta / 100M), 2), boleto.PercMulta);
                else if (item.Equals(MensagemPadrao.JUROS_MORA_DIA))
                    validItem = string.Format(item, Math.Round(boleto.ValorBoleto * (boleto.PercJurosMora / 100M), 2), boleto.PercJurosMora);
                else if (item.Equals(MensagemPadrao.BANCO_PGTO_APOS_VENCTO))
                    validItem = string.Format(item, boleto.Banco.Nome.ToUpper());

                instrucoes.Descricao += validItem + "<br />";
            }

            boleto.Cedente.Instrucoes.Add(instrucoes);
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
