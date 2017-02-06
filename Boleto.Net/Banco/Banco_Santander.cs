using System;
using System.Web.UI;
using BoletoNet;
using System.Globalization;

[assembly: WebResource("BoletoNet.Imagens.033.jpg", "image/jpg")]

namespace BoletoNet
{
    internal class Banco_Santander : AbstractBanco, IBanco
    {
        internal Banco_Santander()
        {
            this.Codigo = 033;
            this.Digito = "7";
            this.Nome = "Santander";
        }

        internal Banco_Santander(int Codigo)
        {

            this.Codigo = ((Codigo != 353) && (Codigo != 8)) ? 033 : Codigo;
            this.Digito = "0";
            this.Nome = "Banco_Santander";
        }

        #region Formatações e Validações do Boleto
        public override void FormataCodigoBarra(Boleto boleto)
        {
            string codigoBanco = Utils.FormatCode(this.Codigo.ToString(), 3);//3
            string codigoMoeda = boleto.Moeda.ToString();//1
            string fatorVencimento = FatorVencimento(boleto).ToString(); //4

            var valorNominal = "";

            if (boleto.SegundaVia)
                valorNominal = Utils.FormatCode(boleto.ValorCobrado.ToString("f").Replace(",", "").Replace(".", ""), 10);//10
            else
                valorNominal = Utils.FormatCode(boleto.ValorBoleto.ToString("f").Replace(",", "").Replace(".", ""), 10);//10

            string fixo = "9";//1
            string codigoCedente = Utils.FormatCode(boleto.Cedente.Codigo.ToString(), 7);//7
            string nossoNumero = Utils.FormatCode(boleto.NossoNumero, 12) + Mod11Santander(Utils.FormatCode(boleto.NossoNumero, 12));//13
            string IOS = boleto.PercentualIOS.ToString();//1
            string tipoCarteira = boleto.Carteira;//3;
            boleto.CodigoBarra.Codigo = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}",
                codigoBanco, codigoMoeda, fatorVencimento, valorNominal, fixo, codigoCedente, nossoNumero, IOS, tipoCarteira);

            string calculoDV = Mod10Mod11Santander(boleto.CodigoBarra.Codigo, 9).ToString();

            boleto.CodigoBarra.Codigo = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}",
                codigoBanco, codigoMoeda, calculoDV, fatorVencimento, valorNominal, fixo, codigoCedente, nossoNumero, IOS, tipoCarteira);
        }

        public override void FormataLinhaDigitavel(Boleto boleto)
        {
            string nossoNumero = Utils.FormatCode(boleto.NossoNumero, 12) + Mod11Santander(Utils.FormatCode(boleto.NossoNumero, 12));//13
            string codigoCedente = Utils.FormatCode(boleto.Cedente.Codigo.ToString(), 7);
            string fatorVencimento = FatorVencimento(boleto).ToString();
            string IOS = boleto.PercentualIOS.ToString();//1

            #region Grupo1

            string codigoBanco = Utils.FormatCode(this.Codigo.ToString(), 3);//3
            string codigoModeda = boleto.Moeda.ToString();//1
            string fixo = "9";//1
            string codigoCedente1 = codigoCedente.Substring(0, 4);//4
            string calculoDV1 = Mod10(string.Format("{0}{1}{2}{3}", codigoBanco, codigoModeda, fixo, codigoCedente1)).ToString();//1
            string grupo1 = string.Format("{0}{1}{2}.{3}{4}", codigoBanco, codigoModeda, fixo, codigoCedente1, calculoDV1);

            #endregion

            #region Grupo2

            string codigoCedente2 = codigoCedente.Substring(4, 3);//3
            string nossoNumero1 = nossoNumero.Substring(0, 7);//7
            string calculoDV2 = Mod10(string.Format("{0}{1}", codigoCedente2, nossoNumero1)).ToString();
            string grupo2 = string.Format("{0}{1}{2}", codigoCedente2, nossoNumero1, calculoDV2);
            grupo2 = " " + grupo2.Substring(0, 5) + "." + grupo2.Substring(5, 6);

            #endregion

            #region Grupo3

            string nossoNumero2 = nossoNumero.Substring(7, 6); //6

            string tipoCarteira = boleto.Carteira;//3
            string calculoDV3 = Mod10(string.Format("{0}{1}{2}", nossoNumero2, IOS, tipoCarteira)).ToString();//1
            string grupo3 = string.Format("{0}{1}{2}{3}", nossoNumero2, IOS, tipoCarteira, calculoDV3);
            grupo3 = " " + grupo3.Substring(0, 5) + "." + grupo3.Substring(5, 6) + " ";

            #endregion

            #region Grupo4
            var DVvalorNominal = "";
            //4
            if (boleto.SegundaVia)
                DVvalorNominal = Utils.FormatCode(boleto.ValorCobrado.ToString("f").Replace(",", "").Replace(".", ""), 10);//10
            else
                DVvalorNominal = Utils.FormatCode(boleto.ValorBoleto.ToString("f").Replace(",", "").Replace(".", ""), 10);//10

            string DVcodigoBanco = Utils.FormatCode(this.Codigo.ToString(), 3);//3
            string DVcodigoMoeda = boleto.Moeda.ToString();//1
            string DVfixo = "9";//1
            string DVcodigoCedente = Utils.FormatCode(boleto.Cedente.Codigo.ToString(), 7).ToString();//7
            string DVnossoNumero = Utils.FormatCode(boleto.NossoNumero, 12) + Mod11Santander(Utils.FormatCode(boleto.NossoNumero, 12));
            string DVtipoCarteira = boleto.Carteira;//3;

            string calculoDVcodigo = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}",
                DVcodigoBanco, DVcodigoMoeda, fatorVencimento, DVvalorNominal, DVfixo, DVcodigoCedente, DVnossoNumero, IOS, DVtipoCarteira);

            string grupo4 = Mod10Mod11Santander(calculoDVcodigo, 9).ToString() + " ";

            #endregion

            #region Grupo5
            var valorNominal = "";
            //4
            if (boleto.SegundaVia)
                valorNominal = Utils.FormatCode(boleto.ValorCobrado.ToString("f").Replace(",", "").Replace(".", ""), 10);//10
            else
                valorNominal = Utils.FormatCode(boleto.ValorBoleto.ToString("f").Replace(",", "").Replace(".", ""), 10);//10

            string grupo5 = string.Format("{0}{1}", fatorVencimento, valorNominal);
            //grupo5 = grupo5.Substring(0, 4) + " " + grupo5.Substring(4, 1)+" "+grupo5.Substring(5,9);

            #endregion

            boleto.CodigoBarra.LinhaDigitavel = string.Format("{0}{1}{2}{3}{4}", grupo1, grupo2, grupo3, grupo4, grupo5);

            //Usado somente no Santander
            boleto.Cedente.ContaBancaria.Conta = boleto.Cedente.Codigo.ToString();

        }

        public override void FormataNossoNumero(Boleto boleto)
        {
            if (boleto.Carteira == "101")
            {
                if (boleto.NossoNumero.Length > 7)
                    boleto.NossoNumero = string.Format("{0}-{1}", boleto.NossoNumero.Substring(5, 7), Mod11Santander(boleto.NossoNumero));
                else
                    boleto.NossoNumero = string.Format("{0}-{1}", boleto.NossoNumero, Mod11Santander(boleto.NossoNumero));
            }
            else
                boleto.NossoNumero = string.Format("{0}-{1}", boleto.NossoNumero, Mod11Santander(boleto.NossoNumero));
        }

        public override void FormataNumeroDocumento(Boleto boleto)
        {
            throw new NotImplementedException("Função não implementada.");
        }

        public override void ValidaBoleto(Boleto boleto)
        {
            //throw new NotImplementedException("Função não implementada.");
            if (!((boleto.Carteira == "102") || (boleto.Carteira == "101") || (boleto.Carteira == "201")))
                throw new NotImplementedException("Santander - Carteira não implementada.");

            //Banco 353  - Utilizar somente 08 posições do Nosso Numero (07 posições + DV), zerando os 05 primeiros dígitos
            if (this.Codigo == 353)
            {
                if (boleto.NossoNumero.Length != 7)
                    throw new NotImplementedException("Santander - Nosso Número deve ter 7 posições para o banco 353.");
            }

            //Banco 008  - Utilizar somente 09 posições do Nosso Numero (08 posições + DV), zerando os 04 primeiros dígitos
            if (this.Codigo == 8)
            {
                if (boleto.NossoNumero.Length != 8)
                    throw new NotImplementedException("Santander - Nosso Número deve ter 7 posições para o banco 008.");
            }

            if (this.Codigo == 33)
            {

                if (boleto.NossoNumero.Length == 7 && (boleto.Carteira.Equals("101") || boleto.Carteira.Equals("102") || boleto.Carteira.Equals("201")))
                    boleto.NossoNumero = Utils.FormatCode(boleto.NossoNumero, "0", 12, true);

                if (boleto.NossoNumero.Length != 12)
                    throw new NotSupportedException("Santander - Nosso Número deve ter 12 posições para o banco 033.");

            }

            if (boleto.Cedente.Codigo.ToString().Length > 7)
                throw new NotImplementedException("Santander - Código cedente deve ter 7 posições.");

            boleto.LocalPagamento += "após o vencimento, pagar somente no banco Santander";

            if (EspecieDocumento.ValidaSigla(boleto.EspecieDocumento) == "")
                boleto.EspecieDocumento = new EspecieDocumento_Santander400(((int)EnumEspecieDocumento_Santander400.DuplicataMercantil).ToString());

            if (boleto.PercentualIOS > 10 & (this.Codigo == 8 || this.Codigo == 33 || this.Codigo == 353))
                throw new Exception("Santander - O percentual do IOS é limitado a 9% para o Banco Santander");

            boleto.FormataCampos();
        }

        private static int Mod11Santander(string seq)
        {
            /* Variáveis
             * -------------
             * d - Dígito
             * s - Soma
             * p - Peso
             * b - Base
             * r - Resto
             */

            int d, r, s = 0, p = 2, b = 9;
            string n;

            for (int i = seq.Length; i > 0; i--)
            {
                n = Microsoft.VisualBasic.Strings.Mid(seq, i, 1);

                s = s + (Convert.ToInt32(n) * p);

                if (p < b)
                    p = p + 1;
                else
                    p = 2;
            }

            r = (s % 11);

            if (r == 0 || r == 1)
                d = 0;
            else if (r == 10)
                d = 1;
            else
                d = 11 - r;

            return d;
        }

        private static int Mod10Mod11Santander(string seq, int lim)
        {
            int ndig = 0;
            int nresto = 0;
            int total = 0;
            int multiplicador = 2;

            char[] posicaoSeq = seq.ToCharArray();
            Array.Reverse(posicaoSeq);
            string sequencia = new string(posicaoSeq);

            while (sequencia.Length > 0)
            {
                int valorPosicao = Convert.ToInt32(sequencia.Substring(0, 1));
                total += valorPosicao * multiplicador;
                multiplicador++;

                if (multiplicador == 10)
                {
                    multiplicador = 2;
                }

                sequencia = sequencia.Remove(0, 1);
            }

            nresto = (total * 10) % 11; //nresto = (((total * 10) / 11) % 10); Jefhtavares em 19/03/14


            if (nresto == 0 || nresto == 1 || nresto == 10)
                ndig = 1;
            else
                ndig = nresto;

            return ndig;
        }

        protected static int Mult10Mod11Santander(string seq, int lim, int flag)
        {
            int mult = 0;
            int total = 0;
            int pos = 1;
            int ndig = 0;
            int nresto = 0;
            string num = string.Empty;

            mult = 1 + (seq.Length % (lim - 1));

            if (mult == 1)
                mult = lim;

            while (pos <= seq.Length)
            {
                num = Microsoft.VisualBasic.Strings.Mid(seq, pos, 1);
                total += Convert.ToInt32(num) * mult;

                mult -= 1;
                if (mult == 1)
                    mult = lim;

                pos += 1;
            }

            nresto = ((total * 10) % 11);

            if (flag == 1)
                return nresto;
            else
            {
                if (nresto == 0 || nresto == 1)
                    ndig = 0;
                else if (nresto == 10)
                    ndig = 1;
                else
                    ndig = (11 - nresto);

                return ndig;
            }
        }

        public string Ocorrencia(string codigo)
        {
            switch (codigo)
            {
                case "01":
                    return "01-Título não existe";
                case "02":
                    return "02-Entrada Confirmada";
                case "03":
                    return "03-Entrada Rejeitada";
                case "06":
                    return "06-Liquidação";
                case "07":
                    return "07-Liquidação por conta";
                case "08":
                    return "08-Liquidação por saldo";
                case "09":
                    return "09-Baixa Automatica";
                case "10":
                    return "10-Baixa conf. instrução ou protesto";
                case "11":
                    return "11-Em Ser";
                case "12":
                    return "12-Abatimento Concedido";
                case "13":
                    return "13-Abatimento Cancelado";
                case "14":
                    return "14-Prorrogação de Vencimento";
                case "15":
                    return "15-Enviado para Cartório";
                case "16":
                    return "16-Título já baixado/liquidado";
                case "17":
                    return "17-Liquidado em cartório";
                case "21":
                    return "21-Entrada em cartório";
                case "22":
                    return "22-Retirado de cartório";
                case "24":
                    return "24-Custas de cartório";
                case "25":
                    return "25-Protestar Título";
                case "26":
                    return "26-Sustar protesto";
                default:
                    return "";
            }
        }

        #endregion

        #region Métodos de geração do arquivo remessa CNAB400

        #region HEADER

        public override string GerarHeaderRemessa(Cedente cedente, TipoArquivo tipoArquivo, int numeroArquivoRemessa)
        {
            return GerarHeaderRemessa("0", cedente, tipoArquivo, numeroArquivoRemessa);
        }
        public override string GerarHeaderRemessa(string numeroConvenio, Cedente cedente, TipoArquivo tipoArquivo, int numeroArquivoRemessa)
        {
            try
            {
                string _header = " ";

                base.GerarHeaderRemessa("0", cedente, tipoArquivo, numeroArquivoRemessa);

                switch (tipoArquivo)
                {

                    case TipoArquivo.CNAB240:
                        _header = GerarHeaderRemessaCNAB240(cedente);
                        break;
                    case TipoArquivo.CNAB400:
                        _header = GerarHeaderRemessaCNAB400(0, cedente);
                        break;
                    case TipoArquivo.Outro:
                        throw new Exception("Tipo de arquivo inexistente.");
                }

                return _header;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a geração do HEADER do arquivo de REMESSA.", ex);
            }
        }

        public string GerarHeaderRemessaCNAB400(int numeroConvenio, Cedente cedente)
        {
            try
            {
                var contaCorrente = "";
                if (cedente.ContaBancaria.Conta.Length == 9 || (!String.IsNullOrEmpty(cedente.ContaBancaria.DigitoConta) && cedente.ContaBancaria.Conta.Length == 8))
                    contaCorrente = Utils.FitStringLength(cedente.ContaBancaria.Conta.Substring(0, 7), 8, 8, '0', 0, true, true, true);
                else
                    contaCorrente = Utils.FitStringLength(cedente.ContaBancaria.Conta, 8, 8, '0', 0, true, true, true);
                //codigo de transmissão para arquivo com layout CNAB400
                var codigoTransmissao = Utils.FitStringLength(cedente.ContaBancaria.Agencia, 4, 4, '0', 0, true, true, true) +
                                        Utils.FitStringLength(cedente.Codigo.ToString(), 8, 8, '0', 0, true, true, true) +
                                        contaCorrente;

                string _header;

                _header = "01REMESSA01COBRANCA       ";
                _header += codigoTransmissao;
                _header += Utils.FitStringLength(cedente.Nome, 30, 30, ' ', 0, true, true, false).ToUpper();
                _header += "033";
                _header += "SANTANDER      ";                   //Nome do banco X(15)
                _header += DateTime.Now.ToString("ddMMyy");     //data da gravação
                _header += "0000000000000000";                  //zeros 9(16)
                _header += Utils.FormatCode("", " ", 275);      //brancos X(275)
                _header += "000";                               //zeros 9(03)
                _header += "000001";

                _header = Utils.SubstituiCaracteresEspeciais(_header);

                return _header;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar HEADER do arquivo de remessa do CNAB400.", ex);
            }
        }

        #endregion

        #region HEADER LOTE
        public override string GerarHeaderLoteRemessa(string numeroConvenio, Cedente cedente, int numeroArquivoRemessa, TipoArquivo tipoArquivo)
        {
            try
            {
                string header = " ";

                switch (tipoArquivo)
                {

                    case TipoArquivo.CNAB240:
                        header = GerarHeaderLoteRemessaCNAB240(cedente, numeroArquivoRemessa);
                        break;
                    case TipoArquivo.CNAB400:
                        header = GerarHeaderLoteRemessaCNAB400(0, cedente, numeroArquivoRemessa);
                        break;
                    case TipoArquivo.Outro:
                        throw new Exception("Tipo de arquivo inexistente.");
                }

                return header;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a geração do HEADER DO LOTE do arquivo de REMESSA.", ex);
            }
        }
        private string GerarHeaderLoteRemessaCNAB400(int p, Cedente cedente, int numeroArquivoRemessa)
        {
            throw new NotImplementedException();
        }

        #endregion HEADER LOTE

        #region DETALHE

        public override string GerarDetalheRemessa(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
        {
            try
            {
                string _detalhe = " ";

                base.GerarDetalheRemessa(boleto, numeroRegistro, tipoArquivo);

                switch (tipoArquivo)
                {
                    case TipoArquivo.CNAB240:
                        _detalhe = GerarDetalheRemessaCNAB240(boleto, numeroRegistro, tipoArquivo);
                        break;
                    case TipoArquivo.CNAB400:
                        _detalhe = GerarDetalheRemessaCNAB400(boleto, numeroRegistro, tipoArquivo);
                        break;
                    case TipoArquivo.Outro:
                        throw new Exception("Tipo de arquivo inexistente.");
                }

                return _detalhe;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a geração do DETALHE arquivo de REMESSA.", ex);
            }
        }

        public string GerarDetalheRemessaCNAB400(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
        {
            try
            {
                var contaCorrente = "";
                if (boleto.Cedente.ContaBancaria.Conta.Length == 9 || (!String.IsNullOrEmpty(boleto.Cedente.ContaBancaria.DigitoConta) && boleto.Cedente.ContaBancaria.Conta.Length == 8))
                    contaCorrente = Utils.FitStringLength(boleto.Cedente.ContaBancaria.Conta.Substring(0, 7), 8, 8, '0', 0, true, true, true);
                else
                    contaCorrente = Utils.FitStringLength(boleto.Cedente.ContaBancaria.Conta, 8, 8, '0', 0, true, true, true);
                //Código da transmissão fornecido pelo banco 9(20), mas normalmente é formado por:
                //018-021 = Código da agencia 9(04)
                //022-029 = Código do cedente 9(08)
                //030-037 = Conta cobrança/corrente 9(08)
                var codigoTransmissao = Utils.FitStringLength(boleto.Cedente.ContaBancaria.Agencia, 4, 4, '0', 0, true, true, true) +
                                        Utils.FitStringLength(boleto.Cedente.Codigo.ToString(), 8, 8, '0', 0, true, true, true) +
                                        contaCorrente;

                base.GerarDetalheRemessa(boleto, numeroRegistro, tipoArquivo);
                this.FormataNossoNumero(boleto);

                var dvAgencia = boleto.Cedente.ContaBancaria.DigitoAgencia == null ? "0" : boleto.Cedente.ContaBancaria.DigitoAgencia;

                string _detalhe;

                _detalhe = "1";
                #region Tipo Inscrição Cedente
                var tipoInscricaoCedente = "";
                if (boleto.Cedente.CPFCNPJ.Length <= 11)
                    tipoInscricaoCedente += "01";
                else
                    tipoInscricaoCedente += "02";
                #endregion
                _detalhe += tipoInscricaoCedente;
                _detalhe += Utils.FitStringLength(boleto.Cedente.CPFCNPJ, 14, 14, '0', 0, true, true, true);
                _detalhe += codigoTransmissao;
                _detalhe += Utils.FormatCode("", " ", 25);
                _detalhe += Utils.Right(boleto.NossoNumero.Replace("-", ""), 8, '0', true);
                _detalhe += "000000";
                _detalhe += " ";
                #region Multa
                var codMulta = "";
                if (boleto.PercMulta == 0)
                    codMulta = "0";
                else
                    codMulta = "4";
                #endregion
                _detalhe += codMulta;
                _detalhe += Utils.FitStringLength(boleto.PercMulta.ToString("0.00").Replace(",", ""), 4, 4, '0', 0, true, true, true);
                _detalhe += "00";
                _detalhe += "0000000000000";
                _detalhe += "    ";
                _detalhe += "000000";
                #region Carteira
                string carteira;
                switch (boleto.Carteira)
                {
                    case "101": //Carteira 101 (Rápida com registro - impressão pelo beneficiário)
                        carteira = "5";
                        break;
                    case "201": //Carteira 201 (Eletrônica com registro - impressão pelo banco)
                        carteira = "1";
                        break;
                    default:
                        throw new Exception("Carteira não implementada para emissão de remessa");
                }
                #endregion
                _detalhe += carteira;
                _detalhe += boleto.CodigoOcorrencia;
                _detalhe += Utils.FitStringLength(boleto.NumeroDocumento.PadLeft(10, '0'), 10, 10, ' ', 0, true, true, false);
                _detalhe += boleto.DataVencimento.ToString("ddMMyy");
                _detalhe += Utils.FitStringLength(boleto.ValorBoleto.ToString("0.00").Replace(",", ""), 13, 13, '0', 0, true, true, true);
                _detalhe += "033";
                //Se a Código da carteira = 5 (Rápida COM registro), informar, senão = 00000 (zeros 9(05))
                if (carteira.Equals(5))
                {
                    _detalhe += Utils.FitStringLength(boleto.Cedente.ContaBancaria.Agencia, 4, 4, '0', 0, true, true, true);
                    _detalhe += Utils.FitStringLength(dvAgencia, 1, 1, '0', 0, true, true, true);
                }
                else
                {
                    _detalhe += "00000";
                }
                _detalhe += Utils.FitStringLength(boleto.EspecieDocumento.Codigo.ToString(), 2, 2, '0', 0, true, true, true);
                _detalhe += "N";
                _detalhe += boleto.DataProcessamento.ToString("ddMMyy");
                #region Instruções Arquivo Remessa
                if (boleto.Cedente.Instrucoes.Count > 0)
                {
                    if (boleto.Cedente.Instrucoes[0] != null)
                    {
                        if (boleto.Cedente.Instrucoes[0].Codigo == (int)EnumInstrucoes_Santander.BaixarApos30Dias)
                        {
                            _detalhe += Utils.FormatCode(boleto.Cedente.Instrucoes[0].Codigo.ToString(), "0", 2, true) + "00";

                            if (boleto.Cedente.Instrucoes[1] != null)
                            {
                                if (boleto.Cedente.Instrucoes[1].Codigo == (int)EnumInstrucoes_Santander.BaixarApos30Dias)
                                {
                                    _detalhe += Utils.FormatCode(boleto.Cedente.Instrucoes[0].Codigo.ToString(), "0", 2, true) + "00";
                                }
                            }
                        }
                        else
                            _detalhe += "0000";
                    }
                }
                else
                    _detalhe += "0000";
                #endregion
                #region Juros
                if (boleto.PercJurosMora > 0)
                    boleto.JurosMora = Math.Round(boleto.ValorBoleto * (boleto.PercJurosMora / 100), 2);
                _detalhe += Utils.FitStringLength(boleto.JurosMora.ToString("0.00").Replace(",", ""), 13, 13, '0', 0, true, true, true);
                #endregion
                #region Desconto
                if (boleto.ValorDesconto > 0)
                {
                    _detalhe += boleto.DataDesconto.ToString("ddMMyy");
                    _detalhe += Utils.FitStringLength(boleto.ValorDesconto.ToString("0.00").Replace(",", ""), 13, 13, '0', 0, true, true, true);
                }
                else
                {
                    _detalhe += Utils.FitStringLength("", 6, 6, '0', 0, true, true, true);
                    _detalhe += Utils.FitStringLength("", 13, 13, '0', 0, true, true, true);
                }
                #endregion
                _detalhe += "0000000000000"; // Valor do IOF a ser recolhido pelo banco para Nota de Seguro 9(13)V9(02) = significa 13 posições, sendo 2 decimais
                _detalhe += "0000000000000"; // Valor do Abatimento 9(13)V9(02) = significa 13 posições, sendo 2 decimais
                #region Tipo Inscricao Pagador
                if (boleto.Sacado.CPFCNPJ.Length <= 11)
                    _detalhe += "01";  // CPF
                else
                    _detalhe += "02"; // CNPJ
                #endregion
                _detalhe += Utils.FitStringLength(boleto.Sacado.CPFCNPJ, 14, 14, '0', 0, true, true, true).ToUpper();
                _detalhe += Utils.FitStringLength(boleto.Sacado.Nome.TrimStart(' '), 40, 40, ' ', 0, true, true, false).ToUpper();
                _detalhe += Utils.FitStringLength(boleto.Sacado.Endereco.End.TrimStart(' '), 40, 40, ' ', 0, true, true, false).ToUpper();
                _detalhe += Utils.FitStringLength(boleto.Sacado.Endereco.Bairro.TrimStart(' '), 12, 12, ' ', 0, true, true, false).ToUpper();
                _detalhe += Utils.FitStringLength(boleto.Sacado.Endereco.CEP, 8, 8, ' ', 0, true, true, true);
                _detalhe += Utils.FitStringLength(boleto.Sacado.Endereco.Cidade.TrimStart(' '), 15, 15, ' ', 0, true, true, false).ToUpper();
                _detalhe += Utils.FitStringLength(boleto.Sacado.Endereco.UF, 2, 2, ' ', 0, true, true, false).ToUpper();
                _detalhe += Utils.FormatCode("", " ", 30);
                _detalhe += " ";
                _detalhe += "I"; // Identificador de complemento de conta cobrança
                _detalhe += boleto.Cedente.ContaBancaria.Conta.Substring(boleto.Cedente.ContaBancaria.Conta.Length - 1, 1) + boleto.Cedente.ContaBancaria.DigitoConta; //Complemento da conta, posições 384-385, com a última posição da conta e o dígito da conta
                _detalhe += "      ";
                string diasProtesto = "00";
                if (boleto.Instrucoes.Count > 0)
                {
                    if (boleto.Instrucoes[0].Codigo == (int)EnumInstrucoes_Santander.Protestar)
                        diasProtesto = Utils.FitStringLength(boleto.Instrucoes[0].QuantidadeDias.ToString(), 2, 2, '0', 0, true, true, true);
                    else if (boleto.Instrucoes.Count > 1)
                        if (boleto.Instrucoes[1].Codigo == (int)EnumInstrucoes_Santander.Protestar)
                            diasProtesto = Utils.FitStringLength(boleto.Instrucoes[1].QuantidadeDias.ToString(), 2, 2, '0', 0, true, true, true);
                        else
                            diasProtesto = "00"; //Nº de dias para protesto 9(02)
                }
                _detalhe += diasProtesto;
                _detalhe += " "; //brancos X(01)
                _detalhe += Utils.FitStringLength(numeroRegistro.ToString(), 6, 6, '0', 0, true, true, true);

                _detalhe = Utils.SubstituiCaracteresEspeciais(_detalhe);

                return _detalhe;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar DETALHE do arquivo CNAB400.", ex);
            }
        }

        #endregion DETALHE

        #region TRAILER

        public override string GerarTrailerRemessa(int numeroRegistro, TipoArquivo tipoArquivo, Cedente cedente, decimal vltitulostotal)
        {
            try
            {
                string _trailer = " ";

                base.GerarTrailerRemessa(numeroRegistro, tipoArquivo, cedente, vltitulostotal);

                switch (tipoArquivo)
                {
                    case TipoArquivo.CNAB240:
                        _trailer = GerarTrailerRemessa240();
                        break;
                    case TipoArquivo.CNAB400:
                        _trailer = GerarTrailerRemessa400(numeroRegistro, vltitulostotal);
                        break;
                    case TipoArquivo.Outro:
                        throw new Exception("Tipo de arquivo inexistente.");
                }

                return _trailer;

            }
            catch (Exception ex)
            {
                throw new Exception("", ex);
            }
        }

        public string GerarTrailerRemessa400(int numeroRegistro, decimal vltitulostotal)
        {
            try
            {
                string complemento = new string('0', 374);
                string _trailer;

                _trailer = "9";
                _trailer += Utils.FitStringLength(numeroRegistro.ToString(), 6, 6, '0', 0, true, true, true); //Quantidade total de linhas no arquivo
                _trailer += Utils.FitStringLength(vltitulostotal.ToString("0.00").Replace(",", ""), 13, 13, '0', 0, true, true, true);
                _trailer += complemento;
                _trailer += Utils.FitStringLength(numeroRegistro.ToString(), 6, 6, '0', 0, true, true, true); // Número sequencial do registro no arquivo.

                _trailer = Utils.SubstituiCaracteresEspeciais(_trailer);

                return _trailer;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a geração do registro TRAILER do arquivo de REMESSA.", ex);
            }
        }

        #endregion

        #endregion

        #region Métodos de geração do arquivo de remessa CNAB240

        #region HEADER

        public string GerarHeaderRemessaCNAB240(Cedente cedente)
        {
            try
            {
                //codigo de transmissão
                //normalmente formado pela agencia, quatro zeros e codigo do cedente para layout 240
                var codigoTransmissao = cedente.ContaBancaria.Agencia + "0000" + cedente.Codigo;
                var numeroSequencia = cedente.NumeroSequencial + 1;

                string header = Utils.FormatCode(Codigo.ToString(), "0", 3, true);                  //Codigo do banco                           001-003
                header += "0000";                                                                   //Lote de serviço                           004-007
                header += "0";                                                                      //Tipo de registro                          008-008
                header += Utils.FormatCode("", " ", 8);                                             //Resevado (uso Banco)                      009-016
                header += (cedente.CPFCNPJ.Length == 11 ? "1" : "2");                               //Tipo inscrição empresa                    017-017
                header += Utils.FormatCode(cedente.CPFCNPJ, "0", 15, true);                         //Nº inscrição empresa                      018-032
                header += Utils.FormatCode(codigoTransmissao, "0", 15);                             //Codigo transmissão adquirido pelo banco   033-047 
                header += Utils.FormatCode("", " ", 25);                                            //Resevado (uso Banco)                      048-072
                header += Utils.FitStringLength(cedente.Nome, 30, 30, ' ', 0, true, true, false);   //Nome do cedente                           073-102
                header += Utils.FormatCode("BANCO SANTANDER", " ", 30);                             //Nome do Banco                             103-132
                header += Utils.FormatCode("", " ", 10);                                            //Resevado (uso Banco)                      133-142
                header += "1";                                                                      //Código remessa = 1                        143-143
                header += DateTime.Now.ToString("ddMMyyyy");                                        //Data geração do arquivo DDMMAAAA          144-151
                header += Utils.FormatCode("", " ", 6);                                             //Resevado (uso Banco)                      152-157
                header += Utils.FormatCode(numeroSequencia.ToString(), "0", 6);                     //Nº sequencial do arquivo                  158-163
                header += "040";                                                                    //Nº da versão do layout                    164-166
                header += Utils.FormatCode("", " ", 74);                                            //Resevado (uso Banco)                      167-240
                return header;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar HEADER do arquivo de remessa do CNAB240.", ex);
            }
        }
        private string GerarHeaderLoteRemessaCNAB240(Cedente cedente, int numeroArquivoRemessa)
        {
            try
            {
                //codigo de transmissão
                //normalmente formado pela agencia, quatro zeros e codigo do cedente para layout 240
                var codigoTransmissao = cedente.ContaBancaria.Agencia + "0000" + cedente.Codigo;

                string header = Utils.FormatCode(Codigo.ToString(), "0", 3, true);                  //Codigo do Banco                               001-003
                header += "0001";                                                                   //Numero lote Remessa                           004-007
                header += "1";                                                                      //Tipo de registro                              008-008
                header += "R";                                                                      //Tipo de operacao (R - Remessa)                009-009
                header += "01";                                                                     //Tipo de serviço - 01 (Cobrança)               010-011
                header += "  ";                                                                     //Resevado (uso Banco)                          012-013
                header += "030";                                                                    //Nº versão layout do lote                      014-016
                header += " ";                                                                      //Resevado (uso Banco)                          017-017
                header += (cedente.CPFCNPJ.Length == 11 ? "1" : "2");                               //Tipo inscrição empresa 1 = CPF, 2 = CNPJ      018-018
                header += Utils.FormatCode(cedente.CPFCNPJ, "0", 15, true);                         //Nº inscrição empresa                          019-032
                header += Utils.FormatCode("", " ", 20);                                            //Resevado (uso Banco)                          034-053
                header += Utils.FormatCode(codigoTransmissao, "0", 15);                             //Código de transmissão adquirido pelo banco    054-068     
                header += Utils.FormatCode("", " ", 5);                                             //Resevado (uso Banco)                          069-073
                header += Utils.FitStringLength(cedente.Nome, 30, 30, ' ', 0, true, true, false);   //Nome do cedente                               074-103
                header += Utils.FormatCode("", " ", 40);                                            //Mensagem 1                                    104-143
                header += Utils.FormatCode("", " ", 40);                                            //Mensagem 2                                    144-183
                header += Utils.FormatCode(numeroArquivoRemessa.ToString(), "0", 8);                //Número remessa                                184-191
                header += DateTime.Now.ToString("ddMMyyyy");                                        //Data de gravação da remessa  DDMMAAAA         192-199   
                header += Utils.FormatCode("", " ", 41);                                            //Resevado (uso Banco)                          200-240
                return header;
            }
            catch (Exception e)
            {
                throw new Exception("Erro ao gerar HEADER DO LOTE do arquivo de remessa.", e);
            }
        }

        #endregion

        #region DETALHE
        public string GerarDetalheRemessaCNAB240(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
        {
            throw new NotImplementedException("Função não implementada.");
        }

        public override string GerarDetalheSegmentoPRemessa(Boleto boleto, int numeroRegistro, string codigoTransmissao)
        {
            try
            {
                var dvAgencia = boleto.Cedente.ContaBancaria.DigitoAgencia == null ? "0" : boleto.Cedente.ContaBancaria.DigitoAgencia;
                this.FormataNossoNumero(boleto);

                string _segmentoP;

                _segmentoP = Utils.FormatCode(Codigo.ToString(), "0", 3, true);
                _segmentoP += "00013";
                _segmentoP += Utils.FitStringLength(numeroRegistro.ToString(), 5, 5, '0', 0, true, true, true);
                _segmentoP += "P ";
                _segmentoP += Utils.FitStringLength(boleto.CodigoOcorrencia, 2, 2, '0', 0, true, true, true);
                _segmentoP += Utils.FitStringLength(boleto.Cedente.ContaBancaria.Agencia, 4, 4, '0', 0, true, true, true);
                _segmentoP += Utils.FitStringLength(dvAgencia, 1, 1, '0', 0, true, true, true);
                _segmentoP += Utils.FitStringLength(boleto.Cedente.ContaBancaria.Conta, 9, 9, '0', 0, true, true, true);        //Conta do cedente
                _segmentoP += Utils.FitStringLength(boleto.Cedente.ContaBancaria.DigitoConta, 1, 1, '0', 0, true, true, true);  //Digito Conta do cedente
                _segmentoP += Utils.FitStringLength(boleto.Cedente.ContaBancaria.Conta, 9, 9, '0', 0, true, true, true);        //Conta de cobrança do cedente
                _segmentoP += Utils.FitStringLength(boleto.Cedente.ContaBancaria.DigitoConta, 1, 1, '0', 0, true, true, true);  //Digito Conta de cobrança do cedente
                _segmentoP += "  ";
                _segmentoP += Utils.FitStringLength(boleto.NossoNumero.Replace("-", ""), 13, 13, '0', 0, true, true, true);
                #region Carteira
                //Código da carteira
                //1 - Eletrônica COM registro
                //3 - Caucionada eletrônica
                //4 - Cobrança SEM registro
                //5 - Rápida COM registro
                //6 - Caucionada rápida
                //7 - Descontada eletrônica
                string carteira;
                switch (boleto.Carteira)
                {
                    case "101": //Carteira 101 (Rápida com registro - impressão pelo beneficiário)
                        carteira = "5";
                        break;
                    case "201": //Carteira 201 (Eletrônica com registro - impressão pelo banco)
                        carteira = "1";
                        break;
                    default:
                        throw new Exception("Carteira não implementada para emissão de remessa");
                }
                #endregion
                _segmentoP += carteira;
                _segmentoP += "1"; //Cobrança registrada
                _segmentoP += "1"; //Tipo de documento 1=Tradicional 2=Escritural
                _segmentoP += "  ";
                _segmentoP += Utils.FitStringLength(boleto.NumeroDocumento, 15, 15, ' ', 0, true, true, false);
                _segmentoP += Utils.FitStringLength(boleto.DataVencimento.ToString("ddMMyyyy"), 8, 8, ' ', 0, true, true, false);
                _segmentoP += Utils.FitStringLength(boleto.ValorBoleto.ToString("0.00").Replace(",", ""), 15, 15, '0', 0, true, true, true);
                _segmentoP += "00000 ";
                _segmentoP += Utils.FitStringLength(boleto.EspecieDocumento.Codigo.ToString(), 2, 2, '0', 0, true, true, true);
                _segmentoP += "N";
                _segmentoP += Utils.FitStringLength(boleto.DataDocumento.ToString("ddMMyyyy"), 8, 8, ' ', 0, true, true, false);
                #region Juros e Desconto
                if (boleto.PercJurosMora > Convert.ToDecimal(0.00))
                {
                    _segmentoP += "1";
                    _segmentoP += Utils.FitStringLength(boleto.DataVencimento.ToString("ddMMyyyy"), 8, 8, '0', 0, true, true, false);

                    var valorJuros = Math.Round(boleto.ValorBoleto * (boleto.PercJurosMora / 100), 2);
                    var valorJurosArredondado = valorJuros.ToString("0.00");
                    _segmentoP += Utils.FitStringLength(valorJurosArredondado.Replace(",", ""), 15, 15, '0', 0, true, true, true);
                }
                else if (boleto.JurosPermanente)
                {
                    _segmentoP += "1";
                    _segmentoP += Utils.FormatCode("", "0", 8);
                    _segmentoP += Utils.FormatCode("", "0", 15);
                }
                else
                {
                    _segmentoP += "3";
                    _segmentoP += Utils.FormatCode("", "0", 8);
                    _segmentoP += Utils.FormatCode("", "0", 15);
                }

                if (boleto.ValorDesconto > 0)
                {
                    _segmentoP += "1";
                    _segmentoP += Utils.FitStringLength(boleto.DataVencimento.ToString("ddMMyyyy"), 8, 8, '0', 0, true, true, false);
                    _segmentoP += Utils.FitStringLength(boleto.ValorDesconto.ToString("0.00").Replace(",", ""), 15, 15, '0', 0, true, true, true);
                }
                else
                {
                    _segmentoP += "0";
                    _segmentoP += Utils.FormatCode("", "0", 8);
                    _segmentoP += Utils.FormatCode("", "0", 15);
                }
                #endregion
                _segmentoP += Utils.FormatCode(boleto.IOF.ToString().Replace(",", "").Replace(".", ""), "0", 15); // Valor do IOF a ser Recolhido 
                _segmentoP += Utils.FormatCode(boleto.Abatimento.ToString().Replace(",", "").Replace(".", ""), "0", 15); // Valor do Abatimento 
                _segmentoP += Utils.FitStringLength(boleto.NumeroDocumento, 25, 25, ' ', 0, true, true, false);

                string codigo_protesto = "3";
                string dias_protesto = "00";

                foreach (Instrucao instrucao in boleto.Instrucoes)
                {
                    switch ((EnumInstrucoes_Santander)instrucao.Codigo)
                    {
                        case EnumInstrucoes_Santander.Protestar:
                            codigo_protesto = "1";
                            dias_protesto = Utils.FitStringLength(instrucao.QuantidadeDias.ToString(), 2, 2, '0', 0, true, true, true);
                            break;
                        case EnumInstrucoes_Santander.NaoProtestar:
                            codigo_protesto = "3";
                            dias_protesto = "00";
                            break;
                        default:
                            break;
                    }
                }

                _segmentoP += codigo_protesto;
                _segmentoP += dias_protesto;

                if (boleto.BaixarAposXDias)
                    _segmentoP += "1";
                else
                    _segmentoP += "3";

                _segmentoP += "00000           ";

                _segmentoP = Utils.SubstituiCaracteresEspeciais(_segmentoP);

                return _segmentoP;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a geração do SEGMENTO P DO DETALHE do arquivo de REMESSA.", ex);
            }
        }

        public override string GerarDetalheSegmentoQRemessa(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
        {
            try
            {
                string _segmentoQ;

                _segmentoQ = Utils.FormatCode(Codigo.ToString(), "0", 3, true);
                _segmentoQ += "00013";
                _segmentoQ += Utils.FitStringLength(numeroRegistro.ToString(), 5, 5, '0', 0, true, true, true);
                _segmentoQ += "Q ";
                _segmentoQ += Utils.FitStringLength(boleto.CodigoOcorrencia, 2, 2, '0', 0, true, true, true);

                if (boleto.Sacado.CPFCNPJ.Length <= 11)
                    _segmentoQ += "1";
                else
                    _segmentoQ += "2";

                _segmentoQ += Utils.FitStringLength(boleto.Sacado.CPFCNPJ, 15, 15, '0', 0, true, true, true);
                _segmentoQ += Utils.FitStringLength(boleto.Sacado.Nome.TrimStart(' '), 40, 40, ' ', 0, true, true, false).ToUpper();
                _segmentoQ += Utils.FitStringLength(boleto.Sacado.Endereco.End.TrimStart(' '), 40, 40, ' ', 0, true, true, false).ToUpper();
                _segmentoQ += Utils.FitStringLength(boleto.Sacado.Endereco.Bairro.TrimStart(' '), 15, 15, ' ', 0, true, true, false).ToUpper();
                _segmentoQ += Utils.FitStringLength(boleto.Sacado.Endereco.CEP, 8, 8, ' ', 0, true, true, false).ToUpper(); ;
                _segmentoQ += Utils.FitStringLength(boleto.Sacado.Endereco.Cidade.TrimStart(' '), 15, 15, ' ', 0, true, true, false).ToUpper();
                _segmentoQ += Utils.FitStringLength(boleto.Sacado.Endereco.UF, 2, 2, ' ', 0, true, true, false).ToUpper();
                _segmentoQ += Utils.FitStringLength("", 16, 16, '0', 0, true, true, true);
                _segmentoQ += Utils.FitStringLength("", 40, 40, ' ', 0, true, true, true);
                _segmentoQ += "000000000000";
                _segmentoQ += Utils.FitStringLength("", 19, 19, ' ', 0, true, true, true);

                _segmentoQ = Utils.SubstituiCaracteresEspeciais(_segmentoQ);

                return _segmentoQ;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a geração do SEGMENTO Q DO DETALHE do arquivo de REMESSA.", ex);
            }
        }

        public override string GerarDetalheSegmentoRRemessa(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
        {
            try
            {
                string _brancos151 = new string(' ', 151);

                string _segmentoR;

                _segmentoR = Utils.FormatCode(Codigo.ToString(), "0", 3, true);
                _segmentoR += "00013";
                _segmentoR += Utils.FitStringLength(numeroRegistro.ToString(), 5, 5, '0', 0, true, true, true);
                _segmentoR += "R ";
                _segmentoR += Utils.FitStringLength(boleto.CodigoOcorrencia, 2, 2, '0', 0, true, true, true);
                // Desconto 2
                _segmentoR += "000000000000000000000000"; //24 zeros
                // Reservado (uso do Banco)
                _segmentoR += "                        "; //24 brancos
                #region Multa
                if (boleto.ValorMulta > 0)
                {
                    _segmentoR += "1";                                                                      // Código da Multa '1' = Valor Fixo,'2' = Percentual,'0' = Sem Multa 
                    _segmentoR += boleto.DataVencimento.ToString("ddMMyyyy");                               // Data da Multa 
                    _segmentoR += Utils.FitStringLength(boleto.ValorMulta.ToString().Replace(",", "").Replace(".", ""), 15, 15, '0', 0, true, true, true); // Valor/Percentual a Ser Aplicado
                }
                else if (boleto.PercMulta > 0)
                {
                    _segmentoR += "2";                                                                      // Código da Multa '1' = Valor Fixo,'2' = Percentual,'0' = Sem Multa 
                    _segmentoR += boleto.DataVencimento.ToString("ddMMyyyy");                               // Data da Multa 
                    _segmentoR += Utils.FitStringLength(boleto.PercMulta.ToString().Replace(",", "").Replace(".", ""), 15, 15, '0', 0, true, true, true); // Valor/Percentual a Ser Aplicado
                }
                else
                {
                    _segmentoR += "0";                                                                      // Código da Multa '1' = Valor Fixo,'2' = Percentual,'0' = Sem Multa 
                    _segmentoR += Utils.FormatCode("", "0", 8);                                             // Data da Multa 
                    _segmentoR += Utils.FormatCode("", "0", 15);                                            // Valor/Percentual a Ser Aplicado
                }
                #endregion
                _segmentoR += _brancos151;

                _segmentoR = Utils.SubstituiCaracteresEspeciais(_segmentoR);

                return _segmentoR;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a geração do SEGMENTO R DO DETALHE do arquivo de REMESSA.", ex);
            }
        }

        public override string GerarDetalheSegmentoSRemessa(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
        {
            try
            {
                string _segmentoS;

                _segmentoS = Utils.FormatCode(Codigo.ToString(), "0", 3, true);
                _segmentoS += "00013";
                _segmentoS += Utils.FormatCode(numeroRegistro.ToString(), "0", 5, true);
                _segmentoS += "S ";
                _segmentoS += Utils.FitStringLength(boleto.CodigoOcorrencia, 2, 2, '0', 0, true, true, true);
                _segmentoS += "2";

                if (boleto.Instrucoes.Count > 0)
                {
                    var instrucoes = boleto.Instrucoes[0].Descricao.Replace("<br/>", " ");
                    _segmentoS += Utils.FitStringLength(instrucoes, 200, 200, ' ', 0, true, true, false);
                }
                else
                    _segmentoS += Utils.FitStringLength("", 200, 200, ' ', 0, true, true, false);

                _segmentoS += Utils.FitStringLength("", 22, 22, ' ', 0, true, true, false);

                _segmentoS = Utils.SubstituiCaracteresEspeciais(_segmentoS);

                return _segmentoS;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a geração do SEGMENTO S DO DETALHE do arquivo de REMESSA.", ex);
            }
        }
        #endregion

        #region TRAILER
        public string GerarTrailerRemessa240()
        {
            throw new NotImplementedException("Função não implementada.");
        }

        public override string GerarTrailerLoteRemessa(int numeroRegistro)
        {
            try
            {
                string trailer = Utils.FormatCode(Codigo.ToString(), "0", 3, true);
                trailer += Utils.FormatCode("0001", "0", 4, true);
                trailer += "5";
                trailer += Utils.FormatCode("", " ", 9);
                trailer += Utils.FitStringLength(numeroRegistro.ToString(), 6, 6, '0', 0, true, true, true);
                trailer += Utils.FormatCode("", " ", 217);
                trailer = Utils.SubstituiCaracteresEspeciais(trailer);

                return trailer;
            }
            catch (Exception e)
            {
                throw new Exception("Erro durante a geração do registro TRAILER do LOTE de REMESSA.", e);
            }
        }

        public override string GerarTrailerArquivoRemessa(int numeroRegistro)
        {
            try
            {
                string trailer = Utils.FormatCode(Codigo.ToString(), "0", 3, true);
                trailer += "9999";
                trailer += "9";
                trailer += Utils.FormatCode("", " ", 9);
                trailer += "000001";
                trailer += Utils.FormatCode(numeroRegistro.ToString(), "0", 6, true);
                trailer += Utils.FormatCode("", " ", 211);
                trailer = Utils.SubstituiCaracteresEspeciais(trailer);

                return trailer;
            }
            catch (Exception e)
            {
                throw new Exception("Erro durante a geração do registro TRAILER do ARQUIVO de REMESSA.", e);
            }
        }

        #endregion

        #endregion

        #region Método de processamento do arquivo retorno CNAB400

        public override HeaderRetorno LerHeaderRetornoCNAB400(string registro)
        {
            try
            {
                HeaderRetorno header = new HeaderRetorno();
                header.RegistroHeader = registro;
                header.Agencia = Convert.ToInt32(registro.Substring(26, 4));
                header.ContaCorrente = Convert.ToInt32(registro.Substring(30, 8));

                return header;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao ler header do arquivo de RETORNO / CNAB 400.", ex);
            }
        }

        public override DetalheRetorno LerDetalheRetornoCNAB400(string registro)
        {
            try
            {
                DetalheRetorno detalhe = new DetalheRetorno(registro);

                //Tipo de Inscrição Empresa
                detalhe.CodigoInscricao = Utils.ToInt32(registro.Substring(1, 2));
                //Nº Inscrição da Empresa
                detalhe.NumeroInscricao = registro.Substring(3, 14);

                //Identificação da Empresa Cedente no Banco
                detalhe.Agencia = Utils.ToInt32(registro.Substring(17, 4));
                detalhe.Conta = Utils.ToInt32(registro.Substring(21, 8));

                //Nº Controle do Participante
                detalhe.NumeroControle = registro.Substring(37, 25);
                //Identificação do Título no Banco
                detalhe.NossoNumeroComDV = registro.Substring(62, 8);
                detalhe.NossoNumero = registro.Substring(62, 7);
                detalhe.DACNossoNumero = registro.Substring(69, 1);
                //Identificação de Ocorrência
                detalhe.CodigoOcorrencia = Utils.ToInt32(registro.Substring(108, 2));

                switch (detalhe.CodigoOcorrencia)
                {
                    case (int)ECodigoOcorrenciaSantander400.Entrada_Confirmada:
                        detalhe.Aceito = true;
                        break;
                    case (int)ECodigoOcorrenciaSantander400.Liquidação:
                    case (int)ECodigoOcorrenciaSantander400.Liquidação_Por_Saldo:
                    case (int)ECodigoOcorrenciaSantander400.Liquidação_Em_Cartório:
                    case (int)ECodigoOcorrenciaSantander400.Liquidação_Parcial:
                        detalhe.Baixado = true;
                        detalhe.Aceito = false;
                        break;
                    case (int)ECodigoOcorrenciaSantander400.Baixa:
                        detalhe.Cancelado = true;
                        detalhe.Aceito = false;
                        break;
                    case (int)ECodigoOcorrenciaSantander400.Rejeição:
                        detalhe.Aceito = false;
                        break;
                }

                //Descrição da ocorrência
                detalhe.DescricaoOcorrencia = this.Ocorrencia(registro.Substring(108, 2));

                //Número do Documento
                detalhe.NumeroDocumento = registro.Substring(116, 10);
                //Identificação do Título no Banco
                detalhe.IdentificacaoTitulo = registro.Substring(126, 8);

                //Valor do Título
                decimal valorTitulo = Convert.ToInt64(registro.Substring(152, 13));
                detalhe.ValorTitulo = valorTitulo / 100;
                //Banco Cobrador
                detalhe.CodigoBanco = Utils.ToInt32(registro.Substring(165, 3));
                //Agência Cobradora
                detalhe.AgenciaCobradora = Utils.ToInt32(registro.Substring(168, 5));
                //Espécie do Título
                detalhe.Especie = Utils.ToInt32(registro.Substring(173, 2));
                // IOF
                decimal iof = Convert.ToUInt64(registro.Substring(214, 13));
                detalhe.IOF = iof / 100;
                //Abatimento Concedido sobre o Título (Valor Abatimento Concedido)
                decimal valorAbatimento = Convert.ToUInt64(registro.Substring(227, 13));
                detalhe.ValorAbatimento = valorAbatimento / 100;
                //Desconto Concedido (Valor Desconto Concedido)
                decimal valorDesconto = Convert.ToUInt64(registro.Substring(240, 13));
                detalhe.Descontos = valorDesconto / 100;
                //Valor Pago
                decimal valorPago = Convert.ToUInt64(registro.Substring(253, 13));
                detalhe.ValorPago = valorPago / 100;
                //Juros Mora
                decimal jurosMora = Convert.ToUInt64(registro.Substring(266, 13));
                detalhe.JurosMora = jurosMora / 100;
                //Outros Créditos
                decimal outrosCreditos = Convert.ToUInt64(registro.Substring(279, 13));
                detalhe.OutrosCreditos = outrosCreditos / 100;
                //Tarifa de Cobrança
                decimal tarifaCobrança = Convert.ToUInt64(registro.Substring(175, 13));
                detalhe.TarifaCobranca = tarifaCobrança / 100;
                //Data Ocorrência no Banco
                int dataOcorrencia = Utils.ToInt32(registro.Substring(110, 6));
                detalhe.DataOcorrencia = Utils.ToDateTime(dataOcorrencia.ToString("##-##-##"));
                //Data Vencimento do Título
                int dataVencimento = Utils.ToInt32(registro.Substring(146, 6));
                detalhe.DataVencimento = Utils.ToDateTime(dataVencimento.ToString("##-##-##"));
                // Data do Crédito
                int dataCredito = Utils.ToInt32(registro.Substring(295, 6));
                detalhe.DataCredito = Utils.ToDateTime(dataCredito.ToString("##-##-##"));
                //Nome do Sacado
                detalhe.NomeSacado = registro.Substring(301, 36);

                detalhe.NumeroSequencial = Utils.ToInt32(registro.Substring(394, 6));

                return detalhe;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao ler detalhe do arquivo de RETORNO / CNAB 400.", ex);
            }
        }

        #endregion

        #region Método de processamento do arquivo retorno CNAB240
        public override HeaderRetorno LerHeaderRetornoCNAB240(string registro)
        {
            try
            {
                HeaderRetorno header = new HeaderRetorno();
                header.RegistroHeader = registro;
                header.Agencia = Convert.ToInt32(registro.Substring(32, 4));
                header.DigitoAgencia = Convert.ToInt32(registro.Substring(36, 1));
                header.ContaCorrente = Convert.ToInt32(registro.Substring(37, 9));
                header.DigitoContaCorrente = Convert.ToInt32(registro.Substring(46, 1));
                header.CodigoCedente = Convert.ToInt32(registro.Substring(52, 9));

                return header;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao ler header do arquivo de RETORNO / CNAB 240.", ex);
            }
        }

        public override DetalheSegmentoTRetornoCNAB240 LerDetalheSegmentoTRetornoCNAB240(string registro)
        {
            try
            {
                var segmentoT = new DetalheSegmentoTRetornoCNAB240(registro);

                if (registro.Substring(13, 1) != "T")
                    throw new Exception("Registro inválido. O detalhe não possuí as características do segmento T.");

                segmentoT.CodigoBanco = Convert.ToInt32(registro.Substring(0, 3));
                segmentoT.idCodigoMovimento = Convert.ToInt32(registro.Substring(15, 2));
                segmentoT.Agencia = Convert.ToInt32(registro.Substring(17, 4));
                segmentoT.DigitoAgencia = registro.Substring(21, 1);
                segmentoT.Conta = Convert.ToInt32(registro.Substring(22, 9));
                segmentoT.DigitoConta = registro.Substring(31, 1);

                segmentoT.NossoNumero = registro.Substring(40, 12);
                segmentoT.CodigoCarteira = Convert.ToInt32(registro.Substring(53, 1));
                segmentoT.NumeroDocumento = registro.Substring(54, 15);
                int dataVencimento = Convert.ToInt32(registro.Substring(69, 8));
                segmentoT.DataVencimento = Convert.ToDateTime(dataVencimento.ToString("##-##-####"));
                decimal valorTitulo = Convert.ToDecimal(registro.Substring(77, 15));
                segmentoT.ValorTitulo = valorTitulo / 100;
                segmentoT.IdentificacaoTituloEmpresa = registro.Substring(100, 25);
                segmentoT.TipoInscricao = Convert.ToInt32(registro.Substring(127, 1));
                segmentoT.NumeroInscricao = registro.Substring(128, 15);
                segmentoT.NomeSacado = registro.Substring(143, 40);
                segmentoT.ValorTarifas = Convert.ToDecimal(registro.Substring(193, 15)) / 100;
                segmentoT.IdRejeicao1 = registro.Substring(208, 2);
                segmentoT.IdRejeicao2 = registro.Substring(210, 2);
                segmentoT.IdRejeicao3 = registro.Substring(212, 2);
                segmentoT.IdRejeicao4 = registro.Substring(214, 2);
                segmentoT.IdRejeicao5 = registro.Substring(216, 2);
                segmentoT.UsoFebraban = registro.Substring(218, 22);
                segmentoT.CodigoMovimento = new CodigoMovimento(segmentoT.CodigoBanco, segmentoT.idCodigoMovimento);

                if (segmentoT.IdRejeicao1.Contains("A"))
                {
                    segmentoT.CodigoRejeicao1 = new CodigoRejeicao(segmentoT.CodigoBanco, 0);
                    segmentoT.CodigoRejeicao1.Descricao = "Sacado DDA";
                }
                else
                {
                    segmentoT.CodigoRejeicao1 = new CodigoRejeicao(segmentoT.CodigoBanco, Convert.ToInt32(segmentoT.IdRejeicao1));
                }

                if (segmentoT.IdRejeicao2.Contains("A"))
                {
                    segmentoT.CodigoRejeicao2 = new CodigoRejeicao(segmentoT.CodigoBanco, 0);
                    segmentoT.CodigoRejeicao2.Descricao = "Sacado DDA";
                }
                else
                {
                    segmentoT.CodigoRejeicao2 = new CodigoRejeicao(segmentoT.CodigoBanco, Convert.ToInt32(segmentoT.IdRejeicao2));
                }

                if (segmentoT.IdRejeicao3.Contains("A"))
                {
                    segmentoT.CodigoRejeicao3 = new CodigoRejeicao(segmentoT.CodigoBanco, 0);
                    segmentoT.CodigoRejeicao3.Descricao = "Sacado DDA";
                }
                else
                {
                    segmentoT.CodigoRejeicao3 = new CodigoRejeicao(segmentoT.CodigoBanco, Convert.ToInt32(segmentoT.IdRejeicao3));
                }

                if (segmentoT.IdRejeicao4.Contains("A"))
                {
                    segmentoT.CodigoRejeicao4 = new CodigoRejeicao(segmentoT.CodigoBanco, 0);
                    segmentoT.CodigoRejeicao4.Descricao = "Sacado DDA";
                }
                else
                {
                    segmentoT.CodigoRejeicao4 = new CodigoRejeicao(segmentoT.CodigoBanco, Convert.ToInt32(segmentoT.IdRejeicao4));
                }

                if (segmentoT.IdRejeicao5.Contains("A"))
                {
                    segmentoT.CodigoRejeicao5 = new CodigoRejeicao(segmentoT.CodigoBanco, 0);
                    segmentoT.CodigoRejeicao5.Descricao = "Sacado DDA";
                }
                else
                {
                    segmentoT.CodigoRejeicao5 = new CodigoRejeicao(segmentoT.CodigoBanco, Convert.ToInt32(segmentoT.IdRejeicao5));
                }

                switch (Convert.ToInt32(segmentoT.idCodigoMovimento))
                {
                    case (int)EnumCodigoMovimento_Santander.Entrada_confirmada:
                        segmentoT.Aceito = true;
                        break;
                    case (int)EnumCodigoMovimento_Santander.Liquidação:
                    case (int)EnumCodigoMovimento_Santander.Liquidação_após_baixa_ou_liquidação_título_não_registrado:
                        segmentoT.Baixado = true;
                        segmentoT.Aceito = false;
                        break;
                    case (int)EnumCodigoMovimento_Santander.Baixa:
                        segmentoT.Cancelado = true;
                        segmentoT.Aceito = false;
                        break;
                    case (int)EnumCodigoMovimento_Santander.Entrada_rejeitada:
                        segmentoT.Aceito = false;
                        break;
                }

                return segmentoT;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao processar arquivo de RETORNO - SEGMENTO T.", ex);
            }


        }

        public override DetalheSegmentoURetornoCNAB240 LerDetalheSegmentoURetornoCNAB240(string registro)
        {
            string _Controle_numBanco = registro.Substring(0, 3); //01
            string _Controle_lote = registro.Substring(3, 7); //02
            string _Controle_regis = registro.Substring(7, 1); //03
            string _Servico_numRegis = registro.Substring(8, 5); //04
            string _Servico_segmento = registro.Substring(13, 1); //05
            string _cnab06 = registro.Substring(14, 1); //06
            string _Servico_codMov = registro.Substring(15, 2); //07
            string _dadosTituloAcrescimo = registro.Substring(17, 15); //08
            string _dadosTituloValorDesconto = registro.Substring(32, 15); //09
            string _dadosTituloValorAbatimento = registro.Substring(47, 15); //10
            string _dadosTituloValorIof = registro.Substring(62, 15); //11
            string _dadosTituloValorPago = registro.Substring(76, 15); //12
            string _dadosTituloValorCreditoBruto = registro.Substring(92, 15); //13
            string _outDespesas = registro.Substring(107, 15); //14
            string _outPerCredEntRecb = registro.Substring(122, 5); //15
            string _outBanco = registro.Substring(127, 10); //16
            string _outDataOcorrencia = registro.Substring(137, 8); //17
            string _outDataCredito = registro.Substring(145, 8); //18
            string _cnab19 = registro.Substring(153, 87); //19

            try
            {

                if (!registro.Substring(13, 1).Equals(@"U"))
                {
                    throw new Exception("Registro inválida. O detalhe não possuí as características do segmento U.");
                }

                var segmentoU = new DetalheSegmentoURetornoCNAB240(registro);
                segmentoU.Servico_Codigo_Movimento_Retorno = Convert.ToDecimal(registro.Substring(15, 2)); //07.3U|Serviço|Cód. Mov.|Código de Movimento Retorno
                segmentoU.JurosMultaEncargos = Convert.ToDecimal(registro.Substring(17, 15)) / 100;
                segmentoU.ValorDescontoConcedido = Convert.ToDecimal(registro.Substring(32, 15)) / 100;
                segmentoU.ValorAbatimentoConcedido = Convert.ToDecimal(registro.Substring(47, 15)) / 100;
                segmentoU.ValorIOFRecolhido = Convert.ToDecimal(registro.Substring(62, 15)) / 100;
                segmentoU.ValorOcorrenciaSacado = segmentoU.ValorPagoPeloSacado = Convert.ToDecimal(registro.Substring(77, 15)) / 100;
                segmentoU.ValorLiquidoASerCreditado = Convert.ToDecimal(registro.Substring(92, 15)) / 100;
                segmentoU.ValorOutrasDespesas = Convert.ToDecimal(registro.Substring(107, 15)) / 100;
                segmentoU.ValorOutrosCreditos = Convert.ToDecimal(registro.Substring(122, 15)) / 100;
                segmentoU.DataOcorrencia = segmentoU.DataOcorrencia = DateTime.ParseExact(registro.Substring(137, 8), "ddMMyyyy", CultureInfo.InvariantCulture);
                segmentoU.DataCredito = registro.Substring(145, 8).ToString() == "00000000" ? segmentoU.DataOcorrencia : DateTime.ParseExact(registro.Substring(145, 8), "ddMMyyyy", CultureInfo.InvariantCulture);

                return segmentoU;
            }
            catch (Exception ex)
            {
                //TrataErros.Tratar(ex);
                throw new Exception("Erro ao processar arquivo de RETORNO - SEGMENTO U.", ex);
            }
        }

        #endregion

        public override bool ValidarRemessa(TipoArquivo tipoArquivo, string numeroConvenio, IBanco banco, Cedente cedente, Boletos boletos, int numeroArquivoRemessa, out string mensagem)
        {
            bool vRetorno = true;
            string vMsg = string.Empty;
            ////IMPLEMENTACAO PENDENTE...
            mensagem = vMsg;
            return vRetorno;
        }

    }
}
