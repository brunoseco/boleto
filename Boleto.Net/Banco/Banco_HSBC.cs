using System;
using System.Web.UI;
using Microsoft.VisualBasic;
using System.Globalization;

[assembly: WebResource("BoletoNet.Imagens.399.jpg", "image/jpg")]
namespace BoletoNet
{
    /// <summary>
    /// Classe referente ao HSBC
    /// </summary>
    internal class Banco_HSBC : AbstractBanco, IBanco
    {
        #region Variáveis

        private string _dacNossoNumero = string.Empty;
        private int _dacBoleto = 0;

        #endregion

        #region Construtores

        internal Banco_HSBC()
        {
            try
            {
                this.Codigo = 399;
                this.Digito = "9";
                this.Nome = "HSBC";
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao instanciar objeto.", ex);
            }
        }
        #endregion

        #region Métodos de Instância

        public override void ValidaBoleto(Boleto boleto)
        {
            try
            {
                if (string.IsNullOrEmpty(boleto.Carteira))
                    throw new NotImplementedException("HSBC - Carteira não informada. Utilize a carteira 'CSB' ou 'CNR'");

                //Verifica as carteiras implementadas
                if (!boleto.Carteira.Equals("CSB") &
                    !boleto.Carteira.Equals("CNR") &
                    !boleto.Carteira.Equals("00"))

                    throw new NotImplementedException("HSBC - Carteira não implementada. Utilize a carteira 'CSB' ou 'CNR'.");

                //Verifica se o nosso número é válido
                if (Utils.ToString(boleto.NossoNumero) == string.Empty)
                    throw new NotImplementedException("HSBC - Nosso número inválido");

                //Verifica se o nosso número é válido
                if (Utils.ToInt64(boleto.NossoNumero) == 0)
                    throw new NotImplementedException("HSBC - Nosso número inválido");

                //Verifica se o tamanho para o NossoNumero são 10 dígitos (5 range + 5 numero sequencial)
                if (Convert.ToInt32(boleto.NossoNumero).ToString().Length > 10)
                    throw new NotImplementedException("HSBC - A quantidade de dígitos do nosso número para a carteira " + boleto.Carteira + ", são 10 números.");
                else if (Convert.ToInt32(boleto.NossoNumero).ToString().Length < 10)
                    boleto.NossoNumero = Utils.FormatCode(boleto.NossoNumero, 10);

                // Calcula o DAC do Nosso Número
                // Nosso Número = Range(5) + Numero Sequencial(5)
                _dacNossoNumero = Mod11(boleto.NossoNumero, 7).ToString();

                //Atribui o nome do banco ao local de pagamento
                boleto.LocalPagamento = "PAGAR PREFERENCIALMENTE EM AGÊNCIAS DO HSBC";

                //Verifica se data do processamento é valida
				//if (boleto.DataProcessamento.ToString("dd/MM/yyyy") == "01/01/0001")
				if (boleto.DataProcessamento == DateTime.MinValue) // diegomodolo (diego.ribeiro@nectarnet.com.br)
                    boleto.DataProcessamento = DateTime.Now;

                //Verifica se data do documento é valida
				//if (boleto.DataDocumento.ToString("dd/MM/yyyy") == "01/01/0001")
				if (boleto.DataDocumento == DateTime.MinValue) // diegomodolo (diego.ribeiro@nectarnet.com.br)
                    boleto.DataDocumento = DateTime.Now;

                FormataCodigoBarra(boleto);
                FormataLinhaDigitavel(boleto);
                FormataNossoNumero(boleto);
            }
            catch (Exception e)
            {
                throw new Exception("HSBC - Erro ao validar boletos.", e);
            }
        }

        # endregion

        # region Métodos de formatação do boleto

        public override void FormataCodigoBarra(Boleto boleto)
        {
            try
            {
                var valorBoleto = "";
                //4
                if (boleto.SegundaVia)
                    valorBoleto = Utils.FormatCode(boleto.ValorCobrado.ToString("f").Replace(",", "").Replace(".", ""), 10);//10
                else
                    valorBoleto = Utils.FormatCode(boleto.ValorBoleto.ToString("f").Replace(",", "").Replace(".", ""), 10);//10

                string numeroDocumento = Utils.FormatCode(boleto.NumeroDocumento.ToString(), 7);

                switch (boleto.Carteira.ToUpper())
                {
                    case "CSB": boleto.CodigoBarra.Codigo =
                            // Código de Barras
                            //banco & moeda & fator & valor & nossonumero & dac_nossonumero & agencia & conta & "00" & "1"
                        string.Format("{0}{1}{2}{3}{4}{5}{6}001", Codigo, boleto.Moeda,
                                      FatorVencimento(boleto), valorBoleto, boleto.NossoNumero + _dacNossoNumero,
                                      Utils.FormatCode(boleto.Cedente.ContaBancaria.Agencia, 4),
                                      Utils.FormatCode(boleto.Cedente.ContaBancaria.Conta, 7));
                        break;
                    case "CNR":
                        // Código de Barras
                        //banco & moeda & fator & valor & codCedente & nossonumero & diadoano & digito do ano & "2"
                        boleto.CodigoBarra.Codigo =
                        string.Format("{0}{1}{2}{3}{4}{5}{6}2",
                                        Codigo,
                                        boleto.Moeda,
                                        FatorVencimento(boleto),
                                        valorBoleto,
                                        Utils.FormatCode(boleto.Cedente.Codigo.ToString(), 7),
                                        Utils.FormatCode(boleto.NossoNumero.ToString(), 13),
                                        Utils.FormatCode(boleto.DataVencimento.DayOfYear.ToString() + boleto.DataVencimento.ToString("yy").Substring(1, 1), 4));
                         break;
                    case "00": boleto.CodigoBarra.Codigo =
                             // Código de Barras
                             //banco & moeda & fator & valor & nossonumero & dac_nossonumero & agencia & conta & "00" & "1"
                       string.Format("{0}{1}{2}{3}{4}{5}{6}001", Codigo, boleto.Moeda,
                                     FatorVencimento(boleto), valorBoleto, boleto.NossoNumero + _dacNossoNumero,
                                     Utils.FormatCode(boleto.Cedente.ContaBancaria.Agencia, 4),
                                     Utils.FormatCode(boleto.Cedente.ContaBancaria.Conta, 7));
                         break;

                }


                _dacBoleto = Mod11(boleto.CodigoBarra.Codigo, 9, 0);

                boleto.CodigoBarra.Codigo = Strings.Left(boleto.CodigoBarra.Codigo, 4) + _dacBoleto + Strings.Right(boleto.CodigoBarra.Codigo, 39);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao formatar código de barras.", ex);
            }
        }
        public override void FormataLinhaDigitavel(Boleto boleto)
        {
            //AAABC.CCCCX DDDDD.DEEEEY FFFFF.FF001Z W GGGGHHHHHHHHHH

            try
            {
                //string numeroDocumento = Utils.FormatCode(boleto.NumeroDocumento.ToString(), 13);
                string nossoNumero = Utils.FormatCode(boleto.NossoNumero.ToString(), 13);
                string codigoCedente = Utils.FormatCode(boleto.Cedente.Codigo.ToString(), 7);

                string C1 = string.Empty;
                string C2 = string.Empty;
                string C3 = string.Empty;
                string C5 = string.Empty;

                string AAA;
                string B;
                string CCCCC;
                string X;


                string DDDDDD; string DD;
                string EEEE; string EEEEEEEE;
                string Y;
                string FFFFFFF; string FFFFF;
                string GGGGG;
                string Z;
                switch (boleto.Carteira.ToUpper())
                {
                    case "CSB":
                        #region AAABC.CCCCX

                        AAA = Utils.FormatCode(Codigo.ToString(), 3);
                        B = boleto.Moeda.ToString();
                        CCCCC = boleto.NossoNumero.Substring(0, 5);
                        X = Mod10(AAA + B + CCCCC).ToString();

                        C1 = string.Format("{0}{1}{2}.", AAA, B, CCCCC.Substring(0, 1));
                        C1 += string.Format("{0}{1} ", CCCCC.Substring(1, 4), X);

                        #endregion AAABC.CCDDX

                        #region DDDDD.DEEEEY

                        DDDDDD = boleto.NossoNumero.Substring(5, 5) + _dacNossoNumero;
                        EEEE = Utils.FormatCode(boleto.Cedente.ContaBancaria.Agencia, 4);
                        Y = Mod10(DDDDDD + EEEE).ToString();

                        C2 = string.Format("{0}.", DDDDDD.Substring(0, 5));
                        C2 += string.Format("{0}{1}{2} ", DDDDDD.Substring(5, 1), EEEE, Y);

                        #endregion DDDDD.DEEEEY

                        #region FFFFF.FF001Z

                        FFFFFFF = Utils.FormatCode(boleto.Cedente.ContaBancaria.Conta, 7);
                        Z = Mod10(FFFFFFF + "001").ToString();

                        C3 = string.Format("{0}.", FFFFFFF.Substring(0, 5));
                        C3 += string.Format("{0}001{1}", FFFFFFF.Substring(5, 2), Z);

                        #endregion FFFFF.FF001Z
                        break;
                    case "CNR":
                        #region AAABC.CCCCX

                        AAA = Utils.FormatCode(Codigo.ToString(), 3);
                        B = boleto.Moeda.ToString();
                        CCCCC = codigoCedente.Substring(0, 5);
                        X = Mod10(AAA + B + CCCCC).ToString();

                        C1 = string.Format("{0}{1}{2}.", AAA, B, CCCCC.Substring(0, 1));
                        C1 += string.Format("{0}{1} ", CCCCC.Substring(1, 4), X);

                        #endregion AAABC.CCDDX

                        #region DDEEE.EEEEEY

                        DD = codigoCedente.Substring(5, 2);
                        EEEEEEEE = nossoNumero.Substring(0, 8);
                        Y = Mod10(DD + EEEEEEEE).ToString();

                        C2 = string.Format("{0}{1}.", DD, EEEEEEEE.Substring(0, 3));
                        C2 += string.Format("{0}{1} ", EEEEEEEE.Substring(3, 5), Y);

                        #endregion DDEEE.EEEEEY

                        #region FFFFF.GGGGGZ

                        FFFFF = nossoNumero.Substring(8, 5);
                        GGGGG = Utils.FormatCode(boleto.DataVencimento.DayOfYear.ToString() + boleto.DataVencimento.ToString("yy").Substring(1, 1), 4) + "2";

                        Z = Mod10(FFFFF + GGGGG).ToString();

                        C3 = string.Format("{0}.", FFFFF);
                        C3 += string.Format("{0}{1}", GGGGG, Z);

                        #endregion FFFFF.GGGGGZ
                        break;
                    case "00":
                        #region AAABC.CCCCX

                        AAA = Utils.FormatCode(Codigo.ToString(), 3);
                        B = boleto.Moeda.ToString();
                        CCCCC = boleto.NossoNumero.Substring(0, 5);
                        X = Mod10(AAA + B + CCCCC).ToString();

                        C1 = string.Format("{0}{1}{2}.", AAA, B, CCCCC.Substring(0, 1));
                        C1 += string.Format("{0}{1} ", CCCCC.Substring(1, 4), X);

                        #endregion AAABC.CCDDX

                        #region DDDDD.DEEEEY

                        DDDDDD = boleto.NossoNumero.Substring(5, 5) + _dacNossoNumero;
                        EEEE = Utils.FormatCode(boleto.Cedente.ContaBancaria.Agencia, 4);
                        Y = Mod10(DDDDDD + EEEE).ToString();

                        C2 = string.Format("{0}.", DDDDDD.Substring(0, 5));
                        C2 += string.Format("{0}{1}{2} ", DDDDDD.Substring(5, 1), EEEE, Y);

                        #endregion DDDDD.DEEEEY

                        #region FFFFF.FF001Z

                        FFFFFFF = Utils.FormatCode(boleto.Cedente.ContaBancaria.Conta, 7);
                        Z = Mod10(FFFFFFF + "001").ToString();

                        C3 = string.Format("{0}.", FFFFFFF.Substring(0, 5));
                        C3 += string.Format("{0}001{1}", FFFFFFF.Substring(5, 2), Z);

                        #endregion FFFFF.FF001Z
                        break;
                    default:
                        throw new NotImplementedException("Função não implementada.");
                }


                string W = string.Format(" {0} ", _dacBoleto);

                #region HHHHIIIIIIIIII

                string HHHH = FatorVencimento(boleto).ToString();

                var IIIIIIIIII = "";
                //4
                if (boleto.SegundaVia)
                    IIIIIIIIII = Utils.FormatCode(boleto.ValorCobrado.ToString("f").Replace(",", "").Replace(".", ""), 10);//10
                else
                    IIIIIIIIII = Utils.FormatCode(boleto.ValorBoleto.ToString("f").Replace(",", "").Replace(".", ""), 10);//10

                C5 = HHHH + IIIIIIIIII;

                #endregion HHHHHHHHHHHHHH

                boleto.CodigoBarra.LinhaDigitavel = C1 + C2 + C3 + W + C5;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao formatar linha digitável.", ex);
            }
        }
        public override void FormataNossoNumero(Boleto boleto)
        {
            try
            {
                switch (boleto.Carteira.ToUpper())
                {
                    case "CSB":
                        boleto.NossoNumero = string.Format("{0}{1}", boleto.NossoNumero, _dacNossoNumero);
                        break;

                    case "CNR": boleto.NossoNumero = string.Format("{0}{1}4{2}", boleto.NossoNumero, Mod11Base9(boleto.NossoNumero).ToString(), Mod11Base9((int.Parse(boleto.NossoNumero + Mod11Base9(boleto.NossoNumero).ToString() + "4") + int.Parse(boleto.Cedente.Codigo.ToString()) + int.Parse(boleto.DataVencimento.ToString("ddMMyy"))).ToString())); break;

                    case "00":
                        boleto.NossoNumero = string.Format("{0}{1}", boleto.NossoNumero, _dacNossoNumero);
                        break;
                    default:
                        throw new NotImplementedException("Carteira não implementada.  Use CSB, CNR ou 00");
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao formatar nosso número", ex);
            }
        }

        # endregion

        #region Métodos de geração do arquivo remessa CNAB400

        # region HEADER
        public override string GerarHeaderRemessa(string numeroConvenio, Cedente cedente, TipoArquivo tipoArquivo, int numeroArquivoRemessa)
        {
            try
            {
                string _header = " ";

                base.GerarHeaderRemessa(numeroConvenio, cedente, tipoArquivo, numeroArquivoRemessa);

                switch (tipoArquivo)
                {
                    case TipoArquivo.CNAB240:
                        _header = GerarHeaderRemessaCNAB240(cedente, numeroArquivoRemessa);
                        break;
                    case TipoArquivo.CNAB400:
                        _header = GerarHeaderRemessaCNAB400(cedente, numeroArquivoRemessa);
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

        public string GerarHeaderRemessaCNAB400(Cedente cedente, int numeroArquivoRemessa)
        {
            try
            {
                TRegistroEDI reg = new TRegistroEDI();
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0001, 001, 0, "0", '0'));                                   //001-001
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0002, 001, 0, "1", '0'));                                   //002-002
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0003, 007, 0, "REMESSA", ' '));                             //003-009
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0010, 002, 0, "01", '0'));                                  //010-011
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0012, 015, 0, "COBRANCA", ' '));                            //012-019
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0027, 001, 0, "0", '0'));                                   //027-027
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0028, 004, 0, cedente.ContaBancaria.Agencia, '0'));         //028-031
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0032, 002, 0, "55", '0'));                                  //032-033
                #region Nota Explicativa 1 Conta Corrente
                string sNumConta = cedente.ContaBancaria.Agencia + cedente.ContaBancaria.Conta + cedente.ContaBancaria.DigitoConta;
                #endregion
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliDireita______, 0034, 011, 0, sNumConta, '0'));                             //034-044
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0045, 002, 0, string.Empty, ' '));                          //045-046
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0047, 030, 0, cedente.Nome.ToUpper(), ' '));                //047-076
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0077, 003, 0, "399", '0'));                                 //101-105
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0080, 015, 0, "HSBC", ' '));                                //077-094
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediDataDDMMAA___________, 0095, 006, 0, DateTime.Now, ' '));                          //095-100
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0101, 005, 0, "01600", '0'));                               //101-105
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0106, 003, 0, "BPI", ' '));                                 //106-108
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0109, 002, 0, string.Empty, ' '));                          //109-110
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0111, 007, 0, "LANCV08", '0'));                             //111-117
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0118, 277, 0, string.Empty, ' '));                          //118-394
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0395, 006, 0, "000001", ' '));                              //395-400
                
                reg.CodificarLinha();
                
                string vLinha = reg.LinhaRegistro;
                string _header = Utils.SubstituiCaracteresEspeciais(vLinha);
                
                return _header;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar HEADER do arquivo de remessa do CNAB400.", ex);
            }
        }

        public string GerarHeaderRemessaCNAB400(int numeroConvenio, Cedente cedente, int numeroArquivoRemessa)
        {
            throw new NotImplementedException("Função não implementada.");
        }

        public override string GerarHeaderRemessa(string numeroConvenio, Cedente cedente, TipoArquivo tipoArquivo, int numeroArquivoRemessa, Boleto boletos)
        {
            throw new NotImplementedException("Função não implementada.");
        }

        # endregion

        # region DETALHE
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

        public string GerarDetalheRemessaCNAB240(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
        {
            throw new NotImplementedException("Função não implementada.");
        }

        public string GerarDetalheRemessaCNAB400(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
        {
            try
            {
                base.GerarDetalheRemessa(boleto, numeroRegistro, tipoArquivo);

                TRegistroEDI reg = new TRegistroEDI();
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0001, 001, 0, "1", '0'));                                       //001-001
                #region Regra Tipo de Inscrição Cedente
                string vCpfCnpjEmi = "00";
                if (boleto.Cedente.CPFCNPJ.Length.Equals(11)) vCpfCnpjEmi = "01"; //Cpf é sempre 11;
                else if (boleto.Cedente.CPFCNPJ.Length.Equals(14)) vCpfCnpjEmi = "02"; //Cnpj é sempre 14;
                #endregion
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0002, 002, 0, vCpfCnpjEmi, '0'));                               //002-003
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0004, 014, 0, boleto.Cedente.CPFCNPJ, '0'));                    //004-017
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0018, 001, 0, "0", '0'));                                       //018-018
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0019, 004, 0, boleto.Cedente.ContaBancaria.Agencia, '0'));      //019-022
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0023, 002, 0, "55", '0'));                                      //023-024
                #region Nota Explicativa 1 Conta Corrente
                string sNumConta = boleto.Cedente.ContaBancaria.Agencia + boleto.Cedente.ContaBancaria.Conta + boleto.Cedente.ContaBancaria.DigitoConta;
                #endregion
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0025, 011, 0, sNumConta, '0'));                                 //025-035
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0036, 002, 0, "", ' '));                                        //036-037
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0038, 025, 0, "", ' '));                                        //038-062
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0063, 011, 0, boleto.NossoNumero, '0'));                        //063-073
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediDataDDMMAA___________, 0074, 006, 0, boleto.DataDesconto, ' '));                       //074-079
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0080, 011, 2, boleto.ValorDesconto, '0'));                      //080-090
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediDataDDMMAA___________, 0091, 006, 0, boleto.DataOutrosDescontos, ' '));                //091-096
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0097, 011, 2, boleto.ValorDesconto, '0'));                      //097-107
                #region Nota Explicativa 5 Carteira
                var carteira = "";
                if (boleto.Cedente.Carteira.Equals("CSB"))
                    carteira = "1";
                else
                    carteira = boleto.Cedente.Carteira;
                #endregion
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0108, 001, 0, carteira, '0'));                                  //108-108
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0109, 002, 0, boleto.CodigoOcorrencia, ' '));                   //109-110
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0111, 010, 0, boleto.NumeroDocumento, ' '));                    //111-120
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediDataDDMMAA___________, 0121, 006, 0, boleto.DataVencimento, ' '));                     //121-126
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0127, 013, 2, boleto.ValorBoleto, '0'));                        //127-139
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0140, 003, 0, "399", '0'));                                     //140-142
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0143, 005, 0, "00000", '0'));                                   //143-147
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0148, 002, 0, boleto.EspecieDocumento.Codigo.ToString(), '0')); //148-149
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0150, 001, 0, boleto.Aceite, ' '));                             //150-150
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediDataDDMMAA___________, 0151, 006, 0, boleto.DataProcessamento, ' '));                  //151-156
                #region Instruções
                string vInstrucao1 = "0";
                string vInstrucao2 = "0";
                //string vInstrucao3 = "0";
                switch (boleto.Instrucoes.Count)
                {
                    case 1:
                        vInstrucao1 = boleto.Instrucoes[0].Codigo.ToString();
                        vInstrucao2 = "0";
                        //vInstrucao3 = "0";
                        break;
                    case 2:
                        vInstrucao1 = boleto.Instrucoes[0].Codigo.ToString();
                        vInstrucao2 = boleto.Instrucoes[1].Codigo.ToString();
                        //vInstrucao3 = "0";
                        break;
                    case 3:
                        vInstrucao1 = boleto.Instrucoes[0].Codigo.ToString();
                        vInstrucao2 = boleto.Instrucoes[1].Codigo.ToString();
                        //vInstrucao3 = boleto.Instrucoes[2].Codigo.ToString();
                        break;
                }
                #endregion
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0157, 002, 0, vInstrucao1, '0'));                               //157-158
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0159, 002, 0, vInstrucao2, '0'));                               //159-160
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0161, 013, 2, boleto.PercJurosMora, '0'));                      //161-173
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediDataDDMMAA___________, 0174, 006, 0, boleto.DataDesconto, '0'));                       //174-179
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0180, 013, 2, boleto.ValorDesconto, '0'));                      //180-192
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0193, 013, 2, boleto.IOF, '0'));                                //193-205
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0206, 013, 2, boleto.Abatimento, '0'));                         //206-218
                #region Regra Tipo de Inscrição Sacado
                string vCpfCnpjSac = "00";
                if (boleto.Sacado.CPFCNPJ.Length.Equals(11)) vCpfCnpjSac = "01"; //Cpf é sempre 11;
                else if (boleto.Sacado.CPFCNPJ.Length.Equals(14)) vCpfCnpjSac = "02"; //Cnpj é sempre 14;
                #endregion
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0219, 002, 0, vCpfCnpjSac, '0'));                               //219-220
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0221, 014, 0, boleto.Sacado.CPFCNPJ, '0'));                     //221-234
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0235, 040, 0, boleto.Sacado.Nome.ToUpper(), ' '));              //235-274
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0275, 040, 0, boleto.Sacado.Endereco.End.ToUpper(), ' '));      //275-314
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0315, 012, 0, boleto.Sacado.Endereco.Bairro.ToUpper(), ' '));   //315-326
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0327, 008, 0, boleto.Sacado.Endereco.CEP, '0'));                //327-334
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0335, 015, 0, boleto.Sacado.Endereco.Cidade.ToUpper(), ' '));   //335-349
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0350, 002, 0, boleto.Sacado.Endereco.UF.ToUpper(), ' '));       //350-351
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0352, 039, 0, "", ' '));                                        //352-390
                #region Tipo Modalidade Boleto
                var tipoModalidade = "";
                if (boleto.BancoGeraBoleto)
                    tipoModalidade = "S";
                #endregion
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0391, 001, 0, tipoModalidade, ' '));                            //391-391
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0392, 002, 0, "", ' '));                                        //392-393
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0394, 001, 0, boleto.Moeda, ' '));                              //394-394                
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0395, 006, 0, numeroRegistro, '0'));                            //395-400
                
                reg.CodificarLinha();
                
                string _detalhe = Utils.SubstituiCaracteresEspeciais(reg.LinhaRegistro);
                
                return _detalhe;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar DETALHE do arquivo CNAB400.", ex);
            }
        }

        # endregion DETALHE

        # region TRAILER

        /// <summary>
        /// TRAILER do arquivo CNAB
        /// Gera o TRAILER do arquivo remessa de acordo com o lay-out informado
        /// </summary>
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
                        _trailer = GerarTrailerRemessa400(numeroRegistro);
                        break;
                    case TipoArquivo.Outro:
                        throw new Exception("Tipo de arquivo inexistente.");
                }

                return _trailer;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a geração do TRAILER do arquivo de REMESSA.", ex);
            }
        }

        public string GerarTrailerRemessa240()
        {
            throw new NotImplementedException("Função não implementada.");
        }

        public string GerarTrailerRemessa400(int numeroRegistro)
        {
            try
            {
                TRegistroEDI reg = new TRegistroEDI();
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0001, 001, 0, "9", ' '));            //001-001
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0002, 393, 0, string.Empty, ' '));   //002-393
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0395, 006, 0, numeroRegistro, '0')); //395-400
                
                reg.CodificarLinha();
                
                string vLinha = reg.LinhaRegistro;
                string _trailer = Utils.SubstituiCaracteresEspeciais(vLinha);
                
                return _trailer;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a geração do registro TRAILER do arquivo de REMESSA.", ex);
            }
        }

        # endregion

        #endregion

        #region Métodos de processamento do arquivo retorno CNAB400
        public override HeaderRetorno LerHeaderRetornoCNAB400(string registro)
        {
            try
            {
                HeaderRetorno header = new HeaderRetorno(registro);

                return header;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao ler detalhe do arquivo de RETORNO / CNAB 400.", ex);
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
                detalhe.Agencia = Utils.ToInt32(registro.Substring(18, 4));
                detalhe.Conta = Utils.ToInt32(registro.Substring(25, 11));

                //Nº Controle do Participante
                detalhe.NumeroControle = registro.Substring(37, 25);

                //Identificação do Título no Banco
                detalhe.NossoNumeroComDV = registro.Substring(62, 11);
                detalhe.NossoNumero = registro.Substring(62, 10);
                detalhe.DACNossoNumero = registro.Substring(69, 1);

                //Identificação de Ocorrência
                detalhe.CodigoOcorrencia = Utils.ToInt32(registro.Substring(108, 2));
                
                switch (detalhe.CodigoOcorrencia)
                {
                    case (int)ECodigoOcorrenciaHSBC400.Entrada_Confirmada:
                        detalhe.Aceito = true;
                        break;
                    case (int)ECodigoOcorrenciaHSBC400.Liquidação:
                    case (int)ECodigoOcorrenciaHSBC400.Liquidação_Por_Saldo:
                    case (int)ECodigoOcorrenciaHSBC400.Liquidação_Em_Cartório:
                    case (int)ECodigoOcorrenciaHSBC400.Liquidação_Baixado_Devolvido_Em_Data_Anterior_Cheque:
                    case (int)ECodigoOcorrenciaHSBC400.Liquidação_Baixado_Devolvido_Em_Data_Anterior_Dinheiro:
                    case (int)ECodigoOcorrenciaHSBC400.Liquidação_De_Título_Não_Registrado_Em_Cheque:
                    case (int)ECodigoOcorrenciaHSBC400.Liquidação_De_Título_Não_Registrado_Em_Dinheiro:
                    case (int)ECodigoOcorrenciaHSBC400.Liquidação_Em_Cartório_Em_Cheque:
                    case (int)ECodigoOcorrenciaHSBC400.Liquidação_Em_Cartório_Em_Dinheiro:
                    case (int)ECodigoOcorrenciaHSBC400.Liquidação_Por_Conta_Em_Cheque:
                    case (int)ECodigoOcorrenciaHSBC400.Liquidação_Por_Conta_Em_Dinheiro:
                        detalhe.Baixado = true;
                        detalhe.Aceito = false;
                        break;
                    case (int)ECodigoOcorrenciaHSBC400.Baixa:
                    case (int)ECodigoOcorrenciaHSBC400.Baixado_Conforme_Instruções:
                        detalhe.Cancelado = true;
                        detalhe.Aceito = false;
                        break;
                    case (int)ECodigoOcorrenciaHSBC400.Rejeição:
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

                //Taxa de boleto
                decimal tarifaCobranca = Convert.ToInt64(registro.Substring(175, 13));
                detalhe.TarifaCobranca = tarifaCobranca / 100;

                //Banco Cobrador
                detalhe.CodigoBanco = Utils.ToInt32(registro.Substring(165, 3));

                //Agência Cobradora
                detalhe.AgenciaCobradora = Utils.ToInt32(registro.Substring(168, 5));

                //Espécie do Título
                detalhe.Especie = Utils.ToInt32(registro.Substring(173, 2));

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

                //Data Ocorrência no Banco
                int dataOcorrencia = Utils.ToInt32(registro.Substring(110, 6));
                detalhe.DataOcorrencia = Utils.ToDateTime(dataOcorrencia.ToString("##-##-##"));

                //Data Vencimento do Título
                int dataVencimento = Utils.ToInt32(registro.Substring(146, 6));
                detalhe.DataVencimento = Utils.ToDateTime(dataVencimento.ToString("##-##-##"));

                // Data do Crédito
                int dataCredito = Utils.ToInt32(registro.Substring(110, 6));
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

        public string Ocorrencia(string codigo)
        {
            switch (codigo)
            {
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
                    return "15-Liquidação em cartório em dinheiro";
                case "16":
                    return "16-Liquidação - baixado/devolvido em data anterior dinheiro";
                case "17":
                    return "17-Entregue em cartório ";
                case "18":
                    return "18-Instrução automática de protesto/serasa";
                case "21":
                    return "21-Instrução de alteração de mora";
                case "22":
                    return "22-Instrução de protesto/serasa processada/re-emitida";
                case "23":
                    return "23-Cancelamento de protesto/serasa processado";
                case "27":
                    return "27-Número do cedente ou controle do participante alterado";
                case "31":
                    return "31-Liquidação normal em cheque/compensação/banco correspondente";
                case "32":
                    return "32-Liquidação em cartório em cheque";
                case "33":
                    return "33-Liquidação por conta em cheque";
                case "36":
                    return "36-Liquidação - baixado/devolvido em data anterior em cheque";
                case "37":
                    return "37-Baixa de título protestado";
                case "38":
                    return "38-Liquidação de título não registrado - em dinheiro";
                case "39":
                    return "39-Liquidação de título não registrado - em cheque";
                case "49":
                    return "49-Vencimento alterado ";
                case "51":
                    return "51-Título DDA aceito pelo sacado";
                case "52":
                    return "52-Título DDA não reconhecido pelo sacado.";
                case "69":
                    return "69-Despesas/custas de cartório";
                case "70":
                    return "70-Ressarcimento sobre títulos";
                case "71":
                    return "71-Ocorrência/Instrução não permitida para título em garantia de operação";
                case "72":
                    return "72-Concessão de Desconto Aceito";
                case "73":
                    return "73-Cancelamento Condição de Desconto Fixo Aceito";
                case "74":
                    return "74-Cancelamento de Desconto Diário Aceito. ";
                default:
                    return "";
            }
        }
        #endregion

        # region Métodos de geração do arquivo remessa CNAB240

        #region HEADER
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

        public string GerarHeaderRemessaCNAB240(Cedente cedente, int numeroArquivoRemessa)
        {
            try
            {
                //Nota 7 (Codigo de Cobrança)
                //normalmente formado pela agencia e conta corrente para layout 240
                var codigoCobranca = cedente.ContaBancaria.Agencia + cedente.ContaBancaria.Conta;
                var dvAgencia = cedente.ContaBancaria.DigitoAgencia == null ? "0" : cedente.ContaBancaria.DigitoAgencia;
                var dvConta = cedente.ContaBancaria.DigitoConta == null ? "0" : cedente.ContaBancaria.DigitoConta;
                var agEConta = cedente.ContaBancaria.Agencia + cedente.ContaBancaria.Conta;

                string header = Utils.FormatCode(Codigo.ToString(), "0", 3, true);                  //Codigo do banco                           001-003
                header += "0000";                                                                   //Lote de serviço                           004-007
                header += "0";                                                                      //Tipo de registro                          008-008
                header += Utils.FormatCode("", " ", 9);                                             //Resevado (uso Banco)                      009-017
                header += (cedente.CPFCNPJ.Length == 11 ? "1" : "2");                               //Tipo inscrição empresa                    018-018
                header += Utils.FormatCode(cedente.CPFCNPJ, "0", 14);                               //Nº inscrição empresa                      019-032
                header += Utils.FormatCode("COB", " ", 3);                                          //Codigo do Aplicativo no banco COB/RDS     033-035
                header += Utils.FormatCode("CNAB", " ", 4);                                         //Fixo Literal CNAB                         036-039 
                header += Utils.FormatCode(codigoCobranca, "0", 13);                                //Codigo de cobrança conforme Nota 7        040-052 
                header += Utils.FormatCode(cedente.ContaBancaria.Agencia, "0", 5);                  //Agência do cedente                        053-057
                header += Utils.FormatCode(dvAgencia, "0", 1);                                      //DV Agência do cedente                     058-058
                header += Utils.FormatCode(cedente.ContaBancaria.Conta, "0", 12);                   //Numero da conta corrente do cedente       059-070
                header += Utils.FormatCode(dvConta, "0", 1);                                        //DV Numero da conta corrente do cedente    071-071
                header += Banco.Mod11(agEConta).ToString();                                         //DV da CC/AG do cedente                    072-072
                header += Utils.FitStringLength(cedente.Nome, 30, 30, ' ', 0, true, true, false);   //Nome do cedente                           073-102
                header += Utils.FormatCode("BANCO HSBC", " ", 30);                                  //Nome do Banco                             103-132
                header += Utils.FormatCode("", " ", 10);                                            //Resevado (uso Banco)                      133-142
                header += "1";                                                                      //Código remessa = 1                        143-143
                header += DateTime.Now.ToString("ddMMyyyy");                                        //Data geração do arquivo DDMMAAAA          144-151
                header += DateTime.Now.ToString("hhmmss");                                          //Hora geração do arquivo HHMMSS            152-157
                header += Utils.FormatCode(numeroArquivoRemessa.ToString(), "0", 6);                //Nº sequencial do arquivo                  158-163
                header += "010";                                                                    //Nº da versão do layout                    164-166
                header += "00000";                                                                  //Densidade                                 167-171
                header += "N";                                                                      //Duplicatas não aceitas                    172-172
                header += Utils.FormatCode("", " ", 11);                                            //Nº Contrato Limite                        173-183
                header += "S";                                                                      //Forma liberação Op.                       184-184
                header += Utils.FormatCode("", " ", 7);                                             //Reservado (Uso Banco)                     185-191
                header += Utils.FormatCode("", " ", 20);                                            //Reservado (Uso Empresa)                   192-211
                header += Utils.FormatCode("", " ", 29);                                            //Reservado (Uso FEBABRAN/CNAB)             212-240
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
                //Nota 7 (Codigo de Cobrança)
                //normalmente formado pela agencia e conta corrente para layout 240
                var codigoCobranca = cedente.ContaBancaria.Agencia + cedente.ContaBancaria.Conta;
                var dvAgencia = cedente.ContaBancaria.DigitoAgencia == null ? "0" : cedente.ContaBancaria.DigitoAgencia;
                var dvConta = cedente.ContaBancaria.DigitoConta == null ? "0" : cedente.ContaBancaria.DigitoConta;
                var agEConta = cedente.ContaBancaria.Agencia + cedente.ContaBancaria.Conta;

                string header = Utils.FormatCode(Codigo.ToString(), "0", 3, true);                  //001-003  3 Codigo do Banco
                header += "0000";                                                                   //004-007  4 Lote do Serviço HEADER = 0000 | TRAILER = 9999 
                header += "1";                                                                      //008-008  1 Registro HEADER de Lote = 1
                header += "R";                                                                      //009-009  1 Tipo de Operação | Remessa = "R", Retorno = "T"
                header += "01";                                                                     //010-011  2 Tipo de Serviço | Cobrança = "01", Desconto = "09", Conciliacao Mensal = "11"
                header += "00";                                                                     //012-013  2 Forma de lançamento, preencher com zeros
                header += "010";                                                                    //014-016  2 Nº da versão do Layout , 010
                header += " ";                                                                      //017-017  1 Uso Exclusivo Febraban/CNAB
                header += (cedente.CPFCNPJ.Length == 11 ? "1" : "2");                               //018-018  1 Tipo de Inscrição da Empresa CPF = "1", CNPJ = "2"
                header += Utils.FormatCode(cedente.CPFCNPJ, "0", 15, true);                         //019-033 15 Nº Inscrição da Empresa
                header += "COB";                                                                    //034-036  3 Codigo do Aplicativo do Banco, Cobrança = "COB", Desconto = "RDS"
                header += Utils.FormatCode("", " ", 4);                                             //037-040  4 Brancos
                header += Utils.FormatCode(codigoCobranca, "0", 13);                                //041-053 13 Codigo de Cobrança Agencia + Conta
                header += Utils.FormatCode(cedente.ContaBancaria.Agencia, "0", 5);                  //054-058  5 Agencia
                header += Utils.FormatCode(dvAgencia, "0", 1);                                      //059-059  1 DV Agência do cedente                     
                header += Utils.FormatCode(cedente.ContaBancaria.Conta, "0", 12);                   //060-071 12 Numero da conta corrente do cedente       
                header += Utils.FormatCode(dvConta, "0", 1);                                        //072-072  1 DV Numero da conta corrente do cedente
                header += Banco.Mod11(agEConta).ToString();                                         //073-073  1 DV da CC/AG do cedente 
                header += Utils.FitStringLength(cedente.Nome, 30, 30, ' ', 0, true, true, false);   //074-103 30 Nome da Empresa   
                header += Utils.FormatCode("", " ", 40);                                            //104-143 40 Mensagem 1
                header += Utils.FormatCode("", " ", 40);                                            //144-183 40 Mensagem 2
                header += Utils.FormatCode(numeroArquivoRemessa.ToString(), "0", 8);                //184-191  8 Número de Remessa
                header += DateTime.Now.ToString("ddMMyyyy");                                        //192-199  6 Data de gravação da remessa  DDMMAAAA 
                header += Utils.FormatCode("", "0", 8);                                             //200-240  8 Data do Crédito (Zeros) 
                header += Utils.FormatCode("", " ", 33);                                            //200-240 33 Brancos

                header = Utils.SubstituiCaracteresEspeciais(header);
                return header;
            }
            catch (Exception e)
            {
                throw new Exception("Erro ao gerar HEADER DO LOTE do arquivo de remessa.", e);
            }
        }

        #endregion

        #region DETALHE
        public override string GerarDetalheSegmentoPRemessa(Boleto boleto, int numeroRegistro, string codigoTransmissao)
        {
            try
            {
                var agEConta = boleto.Cedente.ContaBancaria.Agencia + boleto.Cedente.ContaBancaria.Conta;
                var dvAgencia = boleto.Cedente.ContaBancaria.DigitoAgencia == null ? "0" : boleto.Cedente.ContaBancaria.DigitoAgencia;
                string _segmentoP;

                _segmentoP = Utils.FormatCode(Codigo.ToString(), "0", 3, true);
                _segmentoP += "0001";
                _segmentoP += "3";
                _segmentoP += Utils.FitStringLength(numeroRegistro.ToString(), 5, 5, '0', 0, true, true, true);
                _segmentoP += "P";
                _segmentoP += " ";
                _segmentoP += Utils.FitStringLength(boleto.CodigoOcorrencia, 2, 2, '0', 0, true, true, true);
                _segmentoP += Utils.FitStringLength(boleto.Cedente.ContaBancaria.Agencia, 5, 5, '0', 0, true, true, true);
                _segmentoP += Utils.FitStringLength(dvAgencia, 1, 1, '0', 0, true, true, true);
                _segmentoP += Utils.FitStringLength(boleto.Cedente.ContaBancaria.Conta, 12, 12, '0', 0, true, true, true);
                _segmentoP += Utils.FitStringLength(boleto.Cedente.ContaBancaria.DigitoConta, 1, 1, '0', 0, true, true, true);
                _segmentoP += Banco.Mod11(agEConta).ToString();
                _segmentoP += Utils.FitStringLength(boleto.NossoNumero, 11, 11, '0', 0, true, true, true);
                _segmentoP += Utils.FitStringLength("", 9, 9, '0', 0, true, true, false);

                #region Emissão e Distribuição do Boleto
                var emissaoBoleto = "2"; // padrão Cliente Emite
                var distribuicaoBoleto = "2"; // padrão Cliente Distribui

                if (boleto.BancoGeraBoleto)
                {
                    emissaoBoleto = "1";
                    distribuicaoBoleto = "1";
                }
                #endregion

                _segmentoP += "1"; //Carteira unica no HSBC
                _segmentoP += "1"; //Cobrança registrada
                _segmentoP += "1"; //Tipo de documento 1=Tradicional 2=Escritural
                _segmentoP += emissaoBoleto; //Identificação da emissão do boleto 2=Cliente Emite
                _segmentoP += distribuicaoBoleto; //Distribuição dos titulos 1=Banco Distribui, 2=ClienteDistribui
                _segmentoP += Utils.FitStringLength(boleto.NumeroDocumento, 15, 15, ' ', 0, true, true, false);
                _segmentoP += Utils.FitStringLength(boleto.DataVencimento.ToString("ddMMyyyy"), 8, 8, ' ', 0, true, true, false);
                _segmentoP += Utils.FitStringLength(boleto.ValorBoleto.ToString("0.00").Replace(",", ""), 15, 15, '0', 0, true, true, true);
                _segmentoP += "00000 ";
                _segmentoP += Utils.FitStringLength(boleto.EspecieDocumento.Codigo.ToString(), 2, 2, '0', 0, true, true, true);
                _segmentoP += "N";
                _segmentoP += Utils.FitStringLength(boleto.DataDocumento.ToString("ddMMyyyy"), 8, 8, ' ', 0, true, true, false);
                #region Juros e Desconto
                if (boleto.PercJurosMora > 0)
                {
                    _segmentoP += "1";
                    _segmentoP += Utils.FitStringLength(boleto.DataVencimento.ToString("ddMMyyyy"), 8, 8, '0', 0, true, true, false);
                    _segmentoP += Utils.FitStringLength(boleto.PercJurosMora.ToString("0.00").Replace(",", ""), 15, 15, '0', 0, true, true, true);
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
                _segmentoP += Utils.FormatCode(boleto.IOF.ToString().Replace(",", "").Replace(".", ""), "0", 15);                           
                _segmentoP += Utils.FormatCode(boleto.Abatimento.ToString().Replace(",", "").Replace(".", ""), "0", 15);                      
                _segmentoP += Utils.FitStringLength(boleto.NumeroDocumento, 25, 25, ' ', 0, true, true, false);

                string codigo_protesto = "3";
                string dias_protesto = "00";

                _segmentoP += codigo_protesto;
                _segmentoP += dias_protesto;

                _segmentoP += "0000090000000000 ";

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
                _segmentoQ += "0001";
                _segmentoQ += "3";
                _segmentoQ += Utils.FitStringLength(numeroRegistro.ToString(), 5, 5, '0', 0, true, true, true);
                _segmentoQ += "Q";
                _segmentoQ += " ";
                _segmentoQ += Utils.FitStringLength(boleto.CodigoOcorrencia, 2, 2, '0', 0, true, true, true);

                if (boleto.Sacado.CPFCNPJ.Length <= 11)
                    _segmentoQ += "1";
                else
                    _segmentoQ += "2";

                _segmentoQ += Utils.FitStringLength(boleto.Sacado.CPFCNPJ, 15, 15, '0', 0, true, true, true);
                _segmentoQ += Utils.FitStringLength(boleto.Sacado.Nome.TrimStart(' '), 40, 40, ' ', 0, true, true, false).ToUpper();
                _segmentoQ += Utils.FitStringLength(boleto.Sacado.Endereco.End.TrimStart(' '), 38, 38, ' ', 0, true, true, false).ToUpper();
                _segmentoQ += Utils.FitStringLength("", 2, 2, ' ', 0, true, true, false).ToUpper();
                _segmentoQ += Utils.FitStringLength(boleto.Sacado.Endereco.Bairro.TrimStart(' '), 15, 15, ' ', 0, true, true, false).ToUpper();
                _segmentoQ += Utils.FitStringLength(boleto.Sacado.Endereco.CEP, 8, 8, ' ', 0, true, true, false).ToUpper();
                _segmentoQ += Utils.FitStringLength(boleto.Sacado.Endereco.Cidade.TrimStart(' '), 15, 15, ' ', 0, true, true, false).ToUpper();
                _segmentoQ += Utils.FitStringLength(boleto.Sacado.Endereco.UF, 2, 2, ' ', 0, true, true, false).ToUpper();
                _segmentoQ += Utils.FitStringLength("", 16, 16, '0', 0, true, true, true);
                _segmentoQ += Utils.FitStringLength("", 40, 40, ' ', 0, true, true, true);
                _segmentoQ += "000";
                _segmentoQ += Utils.FitStringLength("", 28, 28, ' ', 0, true, true, true);

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
                _segmentoR += "0001";
                _segmentoR += "3";
                _segmentoR += Utils.FitStringLength(numeroRegistro.ToString(), 5, 5, '0', 0, true, true, true);
                _segmentoR += "R";
                _segmentoR += " ";
                _segmentoR += Utils.FitStringLength(boleto.CodigoOcorrencia, 2, 2, '0', 0, true, true, true);
                // Desconto 2
                _segmentoR += "000000000000000000000000"; //24 zeros
                // Desconto 3
                _segmentoR += "000000000000000000000000"; //24 zeros
                #region Multa
                if (boleto.PercMulta > 0)
                {
                    // Código da multa 2 - percentual
                    _segmentoR += "2";
                    _segmentoR += Utils.FitStringLength(boleto.DataVencimento.ToString("ddMMyyyy"), 8, 8, '0', 0, true, true, false);
                    _segmentoR += Utils.FitStringLength(boleto.PercMulta.ToString("0.00").Replace(",", ""), 15, 15, '0', 0, true, true, true);
                }
                else if (boleto.ValorMulta > 0)
                {
                    // Código da multa 1 - valor fixo
                    _segmentoR += "1";
                    _segmentoR += Utils.FitStringLength(boleto.DataVencimento.ToString("ddMMyyyy"), 8, 8, '0', 0, true, true, false);
                    _segmentoR += Utils.FitStringLength(boleto.ValorMulta.ToString("0.00").Replace(",", ""), 15, 15, '0', 0, true, true, true);
                }
                else
                {
                    // Código da multa 0 - sem multa
                    _segmentoR += "0";
                    _segmentoR += Utils.FitStringLength("", 8, 8, '0', 0, true, true, true);
                    _segmentoR += Utils.FitStringLength("", 15, 15, '0', 0, true, true, true);
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
                _segmentoS += Utils.FormatCode(boleto.Cedente.NumeroSequencial.ToString(), "0", 5);
                _segmentoS += "S ";
                _segmentoS += Utils.FitStringLength(boleto.CodigoOcorrencia, 2, 2, '0', 0, true, true, true);
                _segmentoS += "3";

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
        public override string GerarTrailerLoteRemessa(int numeroRegistro)
        {
            try
            {
                string trailer = Utils.FormatCode(Codigo.ToString(), "0", 3, true);
                trailer += Utils.FormatCode("", "0", 4, true);
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
                trailer += Utils.FitStringLength(numeroRegistro.ToString(), 6, 6, '0', 0, true, true, true);
                trailer += Utils.FormatCode("", "0", 6, true);
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

        #region Métodos de processamento do arquivo retorno CNAB240
        public override DetalheSegmentoTRetornoCNAB240 LerDetalheSegmentoTRetornoCNAB240(string registro)
        {
            try
            {
                var segmentoT = new DetalheSegmentoTRetornoCNAB240(registro);

                if (registro.Substring(13, 1) != "T")
                    throw new Exception("Registro inválido. O detalhe não possuí as características do segmento T.");

                segmentoT.CodigoBanco = Convert.ToInt32(registro.Substring(0, 3));
                segmentoT.idCodigoMovimento = Convert.ToInt32(registro.Substring(15, 2));
                segmentoT.Agencia = Convert.ToInt32(registro.Substring(17, 5));
                segmentoT.DigitoAgencia = registro.Substring(22, 1);
                segmentoT.Conta = Convert.ToInt32(registro.Substring(23, 12));
                segmentoT.DigitoConta = registro.Substring(35, 1);
                segmentoT.NossoNumero = registro.Substring(37, 11);
                segmentoT.CodigoCarteira = Convert.ToInt32(registro.Substring(57, 1));
                segmentoT.NumeroDocumento = registro.Substring(59, 15);
                int dataVencimento = Convert.ToInt32(registro.Substring(73, 8));
                segmentoT.DataVencimento = Convert.ToDateTime(dataVencimento.ToString("##-##-####"));
                decimal valorTitulo = Convert.ToDecimal(registro.Substring(81, 13));
                segmentoT.ValorTitulo = valorTitulo / 100;
                segmentoT.IdentificacaoTituloEmpresa = registro.Substring(105, 25);
                segmentoT.TipoInscricao = Convert.ToInt32(registro.Substring(132, 1));
                segmentoT.NumeroInscricao = registro.Substring(133, 15);
                segmentoT.NomeSacado = registro.Substring(149, 40);
                segmentoT.ValorTarifas = Convert.ToDecimal(registro.Substring(193, 15)) / 100;
                segmentoT.IdRejeicao1 = registro.Substring(213, 2);
                segmentoT.IdRejeicao2 = registro.Substring(215, 2);
                segmentoT.IdRejeicao3 = registro.Substring(217, 2);
                segmentoT.IdRejeicao4 = registro.Substring(219, 2);
                segmentoT.IdRejeicao5 = registro.Substring(221, 2);
                segmentoT.UsoFebraban = registro.Substring(234, 6);
                segmentoT.CodigoMovimento = new CodigoMovimento(segmentoT.CodigoBanco, segmentoT.idCodigoMovimento);

                if (segmentoT.IdRejeicao1.Contains("A"))
                {
                    segmentoT.CodigoRejeicao1 = new CodigoRejeicao(segmentoT.CodigoBanco, 0);
                    segmentoT.CodigoRejeicao1.Codigo = 0;
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
                    case (int)EnumCodigoMovimento_HSBC.Entrada_confirmada:
                        segmentoT.Aceito = true;
                        break;
                    case (int)EnumCodigoMovimento_HSBC.Liquidação:
                    case (int)EnumCodigoMovimento_HSBC.Liquidação_após_baixa_ou_liquidação_título_não_registrado:
                        segmentoT.Baixado = true;
                        segmentoT.Aceito = false;
                        break;
                    case (int)EnumCodigoMovimento_HSBC.Baixa:
                        segmentoT.Cancelado = true;
                        segmentoT.Aceito = false;
                        break;
                    case (int)EnumCodigoMovimento_HSBC.Entrada_rejeitada:
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
            string _dadosTituloAcrescimo = registro.Substring(17, 13); //08
            string _dadosTituloValorDesconto = registro.Substring(32, 13); //09
            string _dadosTituloValorAbatimento = registro.Substring(47, 13); //10
            string _dadosTituloValorIof = registro.Substring(62, 13); //11
            string _dadosTituloValorPago = registro.Substring(77, 13); //12
            string _dadosTituloValorCreditoBruto = registro.Substring(92, 13); //13
            string _outDespesas = registro.Substring(107, 13); //14
            string _outPerCredEntRecb = registro.Substring(122, 13); //15
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
