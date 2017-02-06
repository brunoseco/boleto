using System;
using System.Web.UI;
using Microsoft.VisualBasic;
using System.Text;
using System.Globalization;

[assembly: WebResource("BoletoNet.Imagens.341.jpg", "image/jpg")]
namespace BoletoNet
{
    /// <summary>
    /// Classe referente ao banco Ita�
    /// </summary>
    internal class Banco_Itau : AbstractBanco, IBanco
    {

        #region Vari�veis

        private int _dacBoleto = 0;
        private int _dacNossoNumero = 0;

        #endregion

        #region Construtores

        internal Banco_Itau()
        {
            try
            {
                this.Codigo = 341;
                this.Digito = "7";
                this.Nome = "Ita�";
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao instanciar objeto.", ex);
            }
        }

        #endregion

        #region M�todos de Inst�ncia

        public override void ValidaBoleto(Boleto boleto)
        {
            try
            {
                //Carteiras v�lidas
                int[] cv = new int[] { 175, 176, 178, 109, 198, 107, 122, 142, 143, 196, 126, 131, 146, 150, 169, 121 };
                bool valida = false;

                foreach (int c in cv)
                    if (Utils.ToString(boleto.Carteira) == Utils.ToString(c))
                        valida = true;

                if (!valida)
                {
                    StringBuilder carteirasImplementadas = new StringBuilder(100);

                    carteirasImplementadas.Append(". Carteiras implementadas: ");
                    foreach (int c in cv)
                    {
                        carteirasImplementadas.AppendFormat(" {0}", c);
                    }
                    throw new NotImplementedException("Carteira n�o implementada: " + boleto.Carteira + carteirasImplementadas.ToString());
                }
                //Verifica se o tamanho para o NossoNumero s�o 8 d�gitos
                if (Convert.ToInt32(boleto.NossoNumero).ToString().Length > 8)
                    throw new NotImplementedException("A quantidade de d�gitos do nosso n�mero para a carteira " + boleto.Carteira + ", s�o 8 n�meros.");
                else if (Convert.ToInt32(boleto.NossoNumero).ToString().Length < 8)
                    boleto.NossoNumero = Utils.FormatCode(boleto.NossoNumero, 8);

                //� obrigat�rio o preenchimento do n�mero do documento
                if (boleto.Carteira == "106" || boleto.Carteira == "107" || boleto.Carteira == "122" || boleto.Carteira == "142" || boleto.Carteira == "143" || boleto.Carteira == "195" || boleto.Carteira == "196" || boleto.Carteira == "198")
                {
                    if (Utils.ToInt32(boleto.NumeroDocumento) == 0)
                        throw new NotImplementedException("O n�mero do documento n�o pode ser igual a zero.");
                }

                //Formato o n�mero do documento 
                if (Utils.ToInt32(boleto.NumeroDocumento) > 0)
                    boleto.NumeroDocumento = Utils.FormatCode(boleto.NumeroDocumento, 7);

                // Calcula o DAC do Nosso N�mero a maioria das carteiras
                // agencia/conta/carteira/nosso numero
                if (boleto.Carteira != "126" && boleto.Carteira != "131"
                    && boleto.Carteira != "146" && boleto.Carteira != "150"
                    && boleto.Carteira != "168")
                    _dacNossoNumero = Mod10(boleto.Cedente.ContaBancaria.Agencia + boleto.Cedente.ContaBancaria.Conta + boleto.Carteira + boleto.NossoNumero);
                else
                    // Excess�o 126 - 131 - 146 - 150 - 168
                    // carteira/nosso numero
                    _dacNossoNumero = Mod10(boleto.Carteira + boleto.NossoNumero);

                //// Calcula o DAC da Conta Corrente
                //boleto.Cedente.ContaBancaria.DigitoConta = Mod10(boleto.Cedente.ContaBancaria.Agencia + boleto.Cedente.ContaBancaria.Conta).ToString();

                //Atribui o nome do banco ao local de pagamento
                boleto.LocalPagamento = " Ap�s o vencimento, somente no banco Ita�";

                //Verifica se o nosso n�mero � v�lido
                if (Utils.ToInt64(boleto.NossoNumero) == 0)
                    throw new NotImplementedException("Nosso n�mero inv�lido");

                //Verifica se data do processamento � valida
                //if (boleto.DataProcessamento.ToString("dd/MM/yyyy") == "01/01/0001")
                if (boleto.DataProcessamento == DateTime.MinValue)
                    boleto.DataProcessamento = DateTime.Now;

                //Verifica se data do documento � valida
                //if (boleto.DataDocumento.ToString("dd/MM/yyyy") == "01/01/0001")
                if (boleto.DataDocumento == DateTime.MinValue)
                    boleto.DataDocumento = DateTime.Now;

                boleto.FormataCampos();
            }
            catch (Exception e)
            {
                throw new Exception("Erro ao validar boletos." + e.Message, e);
            }
        }

        # endregion

        #region M�todos de formata��o do boleto

        public override void FormataCodigoBarra(Boleto boleto)
        {
            try
            {
                // C�digo de Barras
                //banco & moeda & fator & valor & carteira & nossonumero & dac_nossonumero & agencia & conta & dac_conta & "000"
                var valorBoleto = "";

                if (boleto.SegundaVia)
                    valorBoleto = Utils.FormatCode(boleto.ValorCobrado.ToString("f").Replace(",", "").Replace(".", ""), 10);//10
                else
                    valorBoleto = Utils.FormatCode(boleto.ValorBoleto.ToString("f").Replace(",", "").Replace(".", ""), 10);//10

                string numeroDocumento = Utils.FormatCode(boleto.NumeroDocumento.ToString(), 7);
                string codigoCedente = Utils.FormatCode(boleto.Cedente.Codigo.ToString(), 5);

                if (boleto.Carteira == "175" || boleto.Carteira == "176" || boleto.Carteira == "178" || boleto.Carteira == "109" || boleto.Carteira == "121")//Flavio(fhlviana@hotmail.com) - Adicionado carteira 109
                {
                    boleto.CodigoBarra.Codigo =
                        string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}000", Codigo, boleto.Moeda,
                                      FatorVencimento(boleto), valorBoleto, boleto.Carteira,
                                      boleto.NossoNumero, _dacNossoNumero, boleto.Cedente.ContaBancaria.Agencia,//Flavio(fhlviana@hotmail.com) => Cedente.ContaBancaria.Agencia --> boleto.Cedente.ContaBancaria.Agencia
                                      Utils.FormatCode(boleto.Cedente.ContaBancaria.Conta, 5), boleto.Cedente.ContaBancaria.DigitoConta);//Flavio(fhlviana@hotmail.com) => Cedente.ContaBancaria.DigitoConta --> boleto.Cedente.ContaBancaria.DigitoConta
                }
                else if (boleto.Carteira == "198" || boleto.Carteira == "107"
                         || boleto.Carteira == "122" || boleto.Carteira == "142"
                         || boleto.Carteira == "143" || boleto.Carteira == "196")
                {
                    boleto.CodigoBarra.Codigo = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}0", Codigo, boleto.Moeda,
                        FatorVencimento(boleto), valorBoleto, boleto.Carteira,
                        boleto.NossoNumero, numeroDocumento, codigoCedente,
                        Mod10(boleto.Carteira + boleto.NossoNumero + numeroDocumento + codigoCedente));
                }

                _dacBoleto = Mod11(boleto.CodigoBarra.Codigo, 9, 0);

                boleto.CodigoBarra.Codigo = Strings.Left(boleto.CodigoBarra.Codigo, 4) + _dacBoleto + Strings.Right(boleto.CodigoBarra.Codigo, 39);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao formatar c�digo de barras.", ex);
            }
        }

        public override void FormataLinhaDigitavel(Boleto boleto)
        {
            try
            {
                string numeroDocumento = Utils.FormatCode(boleto.NumeroDocumento.ToString(), 7);
                string codigoCedente = Utils.FormatCode(boleto.Cedente.Codigo.ToString(), 5);

                string AAA = Utils.FormatCode(Codigo.ToString(), 3);
                string B = boleto.Moeda.ToString();
                string CCC = boleto.Carteira.ToString();
                string DD = boleto.NossoNumero.Substring(0, 2);
                string X = Mod10(AAA + B + CCC + DD).ToString();
                string LD = string.Empty; //Linha Digit�vel

                string DDDDDD = boleto.NossoNumero.Substring(2, 6);

                string K = string.Format(" {0} ", _dacBoleto);

                string UUUU = FatorVencimento(boleto).ToString();
                var VVVVVVVVVV = "";

                if (boleto.SegundaVia)
                    VVVVVVVVVV = Utils.FormatCode(boleto.ValorCobrado.ToString("f").Replace(",", "").Replace(".", ""), 10);//10
                else
                    VVVVVVVVVV = Utils.FormatCode(boleto.ValorBoleto.ToString("f").Replace(",", "").Replace(".", ""), 10);//10

                string C1 = string.Empty;
                string C2 = string.Empty;
                string C3 = string.Empty;
                string C5 = string.Empty;

                #region AAABC.CCDDX

                C1 = string.Format("{0}{1}{2}.", AAA, B, CCC.Substring(0, 1));
                C1 += string.Format("{0}{1}{2} ", CCC.Substring(1, 2), DD, X);

                #endregion AAABC.CCDDX

                #region UUUUVVVVVVVVVV

                C5 = UUUU + VVVVVVVVVV;

                #endregion UUUUVVVVVVVVVV

                if (boleto.Carteira == "175" || boleto.Carteira == "176" || boleto.Carteira == "178" || boleto.Carteira == "109" || boleto.Carteira == "121")//Flavio(fhlviana@hotmail.com) - adicionado carteira 109
                {
                    #region Defini��es
                    /* AAABC.CCDDX.DDDDD.DEFFFY.FGGGG.GGHHHZ.K.UUUUVVVVVVVVVV
              * ------------------------------------------------------
              * Campo 1
              * AAABC.CCDDX
              * AAA - C�digo do Banco
              * B   - Moeda
              * CCC - Carteira
              * DD  - 2 primeiros n�meros Nosso N�mero
              * X   - DAC Campo 1 (AAABC.CCDD) Mod10
              * 
              * Campo 2
              * DDDDD.DEFFFY
              * DDDDD.D - Restante Nosso N�mero
              * E       - DAC (Ag�ncia/Conta/Carteira/Nosso N�mero)
              * FFF     - Tr�s primeiros da ag�ncia
              * Y       - DAC Campo 2 (DDDDD.DEFFF) Mod10
              * 
              * Campo 3
              * FGGGG.GGHHHZ
              * F       - Restante da Ag�ncia
              * GGGG.GG - N�mero Conta Corrente + DAC
              * HHH     - Zeros (N�o utilizado)
              * Z       - DAC Campo 3
              * 
              * Campo 4
              * K       - DAC C�digo de Barras
              * 
              * Campo 5
              * UUUUVVVVVVVVVV
              * UUUU       - Fator Vencimento
              * VVVVVVVVVV - Valor do T�tulo 
              */
                    #endregion Defini��es

                    #region DDDDD.DEFFFY

                    string E = _dacNossoNumero.ToString();
                    string FFF = boleto.Cedente.ContaBancaria.Agencia.Substring(0, 3);
                    string Y = Mod10(DDDDDD + E + FFF).ToString();

                    C2 = string.Format("{0}.", DDDDDD.Substring(0, 5));
                    C2 += string.Format("{0}{1}{2}{3} ", DDDDDD.Substring(5, 1), E, FFF, Y);

                    #endregion DDDDD.DEFFFY

                    #region FGGGG.GGHHHZ

                    string F = boleto.Cedente.ContaBancaria.Agencia.Substring(3, 1);
                    string GGGGGG = boleto.Cedente.ContaBancaria.Conta + boleto.Cedente.ContaBancaria.DigitoConta;
                    string HHH = "000";
                    string Z = Mod10(F + GGGGGG + HHH).ToString();

                    C3 = string.Format("{0}{1}.{2}{3}{4}", F, GGGGGG.Substring(0, 4), GGGGGG.Substring(4, 2), HHH, Z);

                    #endregion FGGGG.GGHHHZ
                }
                else if (boleto.Carteira == "198" || boleto.Carteira == "107"
                     || boleto.Carteira == "122" || boleto.Carteira == "142"
                     || boleto.Carteira == "143" || boleto.Carteira == "196")
                {
                    #region Defini��es
                    /* AAABC.CCDDX.DDDDD.DEEEEY.EEEFF.FFFGHZ.K.UUUUVVVVVVVVVV
              * ------------------------------------------------------
              * Campo 1 - AAABC.CCDDX
              * AAA - C�digo do Banco
              * B   - Moeda
              * CCC - Carteira
              * DD  - 2 primeiros n�meros Nosso N�mero
              * X   - DAC Campo 1 (AAABC.CCDD) Mod10
              * 
              * Campo 2 - DDDDD.DEEEEY
              * DDDDD.D - Restante Nosso N�mero
              * EEEE    - 4 primeiros numeros do n�mero do documento
              * Y       - DAC Campo 2 (DDDDD.DEEEEY) Mod10
              * 
              * Campo 3 - EEEFF.FFFGHZ
              * EEE     - Restante do n�mero do documento
              * FFFFF   - C�digo do Cliente
              * G       - DAC (Carteira/Nosso Numero(sem DAC)/Numero Documento/Codigo Cliente)
              * H       - zero
              * Z       - DAC Campo 3
              * 
              * Campo 4 - K
              * K       - DAC C�digo de Barras
              * 
              * Campo 5 - UUUUVVVVVVVVVV
              * UUUU       - Fator Vencimento
              * VVVVVVVVVV - Valor do T�tulo 
              */
                    #endregion Defini��es

                    #region DDDDD.DEEEEY

                    string EEEE = numeroDocumento.Substring(0, 4);
                    string Y = Mod10(DDDDDD + EEEE).ToString();

                    C2 = string.Format("{0}.", DDDDDD.Substring(0, 5));
                    C2 += string.Format("{0}{1}{2} ", DDDDDD.Substring(5, 1), EEEE, Y);

                    #endregion DDDDD.DEEEEY

                    #region EEEFF.FFFGHZ

                    string EEE = numeroDocumento.Substring(4, 3);
                    string FFFFF = codigoCedente;
                    string G = Mod10(boleto.Carteira + boleto.NossoNumero + numeroDocumento + codigoCedente).ToString();
                    string H = "0";
                    string Z = Mod10(EEE + FFFFF + G + H).ToString();
                    C3 = string.Format("{0}{1}.{2}{3}{4}{5}", EEE, FFFFF.Substring(0, 2), FFFFF.Substring(2, 3), G, H, Z);

                    #endregion EEEFF.FFFGHZ
                }
                else if (boleto.Carteira == "126" || boleto.Carteira == "131" || boleto.Carteira == "146" || boleto.Carteira == "150" || boleto.Carteira == "168")
                {
                    throw new NotImplementedException("Fun��o n�o implementada.");
                }

                boleto.CodigoBarra.LinhaDigitavel = C1 + C2 + C3 + K + C5;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao formatar linha digit�vel.", ex);
            }
        }

        public override void FormataNossoNumero(Boleto boleto)
        {
            try
            {
                boleto.NossoNumero = string.Format("{0}/{1}-{2}", boleto.Carteira, boleto.NossoNumero, _dacNossoNumero);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao formatar nosso n�mero", ex);
            }
        }

        public override void FormataNumeroDocumento(Boleto boleto)
        {
            try
            {
                boleto.NumeroDocumento = string.Format("{0}-{1}", boleto.NumeroDocumento, Mod10(boleto.NumeroDocumento));
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao formatar n�mero do documento.", ex);
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
                case "04":
                    return "04-Altera��o de Dados-Nova entrada ou Altera��o/Exclus�a de dados acatada";
                case "05":
                    return "05-Altera��o de dados-Baixa";
                case "06":
                    return "06-Liquida��o normal";
                case "08":
                    return "08-Liquida��o em cart�rio";
                case "09":
                    return "09-Baixa simples";
                case "10":
                    return "10-Baixa por ter sido liquidado";
                case "11":
                    return "11-Em Ser (S� no retorno mensal)";
                case "12":
                    return "12-Abatimento Concedido";
                case "13":
                    return "13-Abatimento Cancelado";
                case "14":
                    return "14-Vencimento Alterado";
                case "15":
                    return "15-Baixas rejeitadas";
                case "16":
                    return "16-Instru��es rejeitadas";
                case "17":
                    return "17-Altera��o/Exclus�o de dados rejeitados";
                case "18":
                    return "18-Cobran�a contratual-Instru��es/Altera��es rejeitadas/pendentes";
                case "19":
                    return "19-Confirma Recebimento Instru��o de Protesto";
                case "20":
                    return "20-Confirma Recebimento Instru��o Susta��o de Protesto/Tarifa";
                case "21":
                    return "21-Confirma Recebimento Instru��o de N�o Protestar";
                case "23":
                    return "23-T�tulo enviado a Cart�rio/Tarifa";
                case "24":
                    return "24-Instru��o de Protesto Rejeitada/Sustada/Pendente";
                case "25":
                    return "25-Alega��es do Sacado";
                case "26":
                    return "26-Tarifa de Aviso de Cobran�a";
                case "27":
                    return "27-Tarifa de Extrato Posi��o";
                case "28":
                    return "28-Tarifa de Rela��o das Liquida��es";
                case "29":
                    return "29-Tarifa de Manuten��o de T�tulos Vencidos";
                case "30":
                    return "30-D�bito Mensal de Tarifas (Para Entradas e Baixas)";
                case "32":
                    return "32-Baixa por ter sido Protestado";
                case "33":
                    return "33-Custas de Protesto";
                case "34":
                    return "34-Custas de Susta��o";
                case "35":
                    return "35-Custas de Cart�rio Distribuidor";
                case "36":
                    return "36-Custas de Edital";
                case "37":
                    return "37-Tarifa de Emiss�o de Boleto/Tarifa de Envio de Duplicata";
                case "38":
                    return "38-Tarifa de Instru��o";
                case "39":
                    return "39-Tarifa de Ocorr�ncias";
                case "40":
                    return "40-Tarifa Mensal de Emiss�o de Boleto/Tarifa Mensal de Envio de Duplicata";
                case "41":
                    return "41-D�bito Mensal de Tarifas-Extrato de Posi��o(B4EP/B4OX)";
                case "42":
                    return "42-D�bito Mensal de Tarifas-Outras Instru��es";
                case "43":
                    return "43-D�bito Mensal de Tarifas-Manuten��o de T�tulos Vencidos";
                case "44":
                    return "44-D�bito Mensal de Tarifas-Outras Ocorr�ncias";
                case "45":
                    return "45-D�bito Mensal de Tarifas-Protesto";
                case "46":
                    return "56-D�bito Mensal de Tarifas-Susta��o de Protesto";
                case "47":
                    return "47-Baixa com Transfer�ncia para Protesto";
                case "48":
                    return "48-Custas de Susta��o Judicial";
                case "51":
                    return "51-Tarifa Mensal Ref a Entradas Bancos Correspondentes na Carteira";
                case "52":
                    return "52-Tarifa Mensal Baixas na Carteira";
                case "53":
                    return "53-Tarifa Mensal Baixas em Bancos Correspondentes na Carteira";
                case "54":
                    return "54-Tarifa Mensal de Liquida��es na Carteira";
                case "55":
                    return "55-Tarifa Mensal de Liquida��es em Bancos Correspondentes na Carteira";
                case "56":
                    return "56-Custas de Irregularidade";
                case "57":
                    return "57-Instru��o Cancelada";
                case "59":
                    return "59-Baixa por Cr�dito em C/C Atrav�s do SISPAG";
                case "60":
                    return "60-Entrada Rejeitada Carn�";
                case "61":
                    return "61-Tarifa Emiss�o Aviso de Movimenta��o de T�tulos";
                case "62":
                    return "62-D�bito Mensal de Tarifa-Aviso de Movimenta��o de T�tulos";
                case "63":
                    return "63-T�tulo Sustado Judicialmente";
                case "64":
                    return "64-Entrada Confirmada com Rateio de Cr�dito";
                case "69":
                    return "69-Cheque Devolvido";
                case "71":
                    return "71-Entrada Registrada-Aguardando Avalia��o";
                case "72":
                    return "72-Baixa por Cr�dito em C/C Atrav�s do SISPAG sem T�tulo Correspondente";
                case "73":
                    return "73-Confirma��o de Entrada na Cobran�a Simples-Entrada N�o Aceita na Cobran�a Contratual";
                case "76":
                    return "76-Cheque Compensado";
                default:
                    return "";
            }
        }

        # endregion

        #region M�todos de gera��o do arquivo remessa CNAB400

        #region HEADER
        public override string GerarHeaderRemessa(string numeroConvenio, Cedente cendente, TipoArquivo tipoArquivo, int numeroArquivoRemessa, Boleto boletos)
        {
            throw new NotImplementedException("Fun��o n�o implementada.");
        }

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
                        _header = GerarHeaderRemessaCNAB240(cedente, numeroArquivoRemessa);
                        break;
                    case TipoArquivo.CNAB400:
                        _header = GerarHeaderRemessaCNAB400(0, cedente, numeroArquivoRemessa);
                        break;
                    case TipoArquivo.Outro:
                        throw new Exception("Tipo de arquivo inexistente.");
                }

                return _header;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a gera��o do HEADER do arquivo de REMESSA.", ex);
            }
        }

        public string GerarHeaderRemessaCNAB400(int numeroConvenio, Cedente cedente, int numeroArquivoRemessa)
        {
            try
            {
                string complemento = new string(' ', 294);
                string _header;

                _header = "01REMESSA01COBRANCA       ";
                _header += Utils.FitStringLength(cedente.ContaBancaria.Agencia, 4, 4, '0', 0, true, true, true);
                _header += "00";
                _header += Utils.FitStringLength(cedente.ContaBancaria.Conta, 5, 5, '0', 0, true, true, true);
                _header += Utils.FitStringLength(cedente.ContaBancaria.DigitoConta, 1, 1, ' ', 0, true, true, false);
                _header += "        ";
                _header += Utils.FitStringLength(cedente.Nome, 30, 30, ' ', 0, true, true, false).ToUpper();
                _header += "341";
                _header += "BANCO ITAU SA  ";
                _header += DateTime.Now.ToString("ddMMyy");
                _header += complemento;
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
                throw new Exception("Erro durante a gera��o do DETALHE arquivo de REMESSA.", ex);
            }
        }

        public string GerarDetalheRemessaCNAB400(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
        {
            try
            {
                base.GerarDetalheRemessa(boleto, numeroRegistro, tipoArquivo);

                // USO DO BANCO - Identifica��o da opera��o no Banco (posi��o 87 a 107)
                string identificaOperacaoBanco = new string(' ', 21);
                string usoBanco = new string(' ', 10);
                string nrDocumento = new string(' ', 25);
                string _detalhe;

                _detalhe = "1";

                // Tipo de inscri��o da empresa

                // Normalmente definem o tipo (CPF/CNPJ) e o n�mero de inscri��o do cedente. 
                // Se o t�tulo for negociado, dever�o ser utilizados para indicar o CNPJ/CPF do sacador 
                // (cedente original), uma vez que os cart�rios exigem essa informa��o para efetiva��o 
                // dos protestos. Para este fim, tamb�m poder� ser utilizado o registro tipo �5�.
                // 01 - CPF DO CEDENTE
                // 02 - CNPJ DO CEDENTE
                // 03 - CPF DO SACADOR
                // 04 - CNPJ DO SACADOR
                // O arquivo gerado pelo aplicativo do Banco ITA�, sempre atriubuiu 04 para o tipo de inscri��o da empresa

                if (boleto.Cedente.CPFCNPJ.Length <= 11)
                    _detalhe += "01";
                else
                    _detalhe += "02";
                _detalhe += Utils.FitStringLength(boleto.Cedente.CPFCNPJ.ToString(), 14, 14, '0', 0, true, true, true);
                _detalhe += Utils.FitStringLength(boleto.Cedente.ContaBancaria.Agencia.ToString(), 4, 4, '0', 0, true, true, true);
                _detalhe += "00";
                _detalhe += Utils.FitStringLength(boleto.Cedente.ContaBancaria.Conta.ToString(), 5, 5, '0', 0, true, true, true);
                _detalhe += Utils.FitStringLength(boleto.Cedente.ContaBancaria.DigitoConta.ToString(), 1, 1, ' ', 0, true, true, false);
                _detalhe += "    "; // Complemento do registro - 4 posi��es em branco

                // C�digo da instru��o/alega��o a ser cancelada

                // Deve ser preenchido na remessa somente quando utilizados, na posi��o 109-110, os c�digos de ocorr�ncia 35 � 
                // Cancelamento de Instru��o e 38 � Cedente n�o concorda com alega��o do sacado. Para os demais c�digos de 
                // ocorr�ncia este campo dever� ser preenchido com zeros. 
                //Obs.: No arquivo retorno ser� informado o mesmo c�digo da instru��o cancelada, e para o cancelamento de alega��o 
                // de sacado n�o h� retorno da informa��o.

                // Por enquanto o objetivo � apenas gerar o arquivo de remessa e n�o utilizar o arquivo para enviar instru��es
                // para t�tulos que j� est�o no banco, portanto o campo ser� preenchido com zeros.
                _detalhe += "0000";

                _detalhe += nrDocumento; // Utils.FitStringLength(boleto.NumeroDocumento, 25, 25, ' ', 0, true, true, false); //Identifica��o do t�tulo na empresa
                _detalhe += Utils.FitStringLength(boleto.NossoNumero, 8, 8, '0', 0, true, true, true);
                // Quantidade de moeda vari�vel - Preencher com zeros se a moeda for REAL
                // O manual do Banco ITA� n�o diz como preencher caso a moeda n�o seja o REAL
                if (boleto.Moeda == 9)
                    _detalhe += "0000000000000";

                _detalhe += Utils.FitStringLength(boleto.Carteira, 3, 3, '0', 0, true, true, true);
                _detalhe += Utils.FitStringLength(identificaOperacaoBanco, 21, 21, ' ', 0, true, true, true);
                // C�digo da carteira
                if (boleto.Moeda == 9)
                    _detalhe += "I"; //O c�digo da carteira s� muda para dois tipos, quando a cobran�a for em d�lar

                _detalhe += boleto.CodigoOcorrencia; // "01"; // Identifica��o da ocorr�ncia - 01 REMESSA
                _detalhe += Utils.FitStringLength(boleto.NumeroDocumento, 10, 10, ' ', 0, true, true, false);
                _detalhe += boleto.DataVencimento.ToString("ddMMyy");
                _detalhe += Utils.FitStringLength(boleto.ValorBoleto.ToString("0.00").Replace(",", ""), 13, 13, '0', 0, true, true, true);
                _detalhe += "341";
                _detalhe += "00000"; // Ag�ncia onde o t�tulo ser� cobrado - no arquivo de remessa, preencher com ZEROS

                _detalhe += Utils.FitStringLength(EspecieDocumento.ValidaCodigo(boleto.EspecieDocumento).ToString(), 2, 2, '0', 0, true, true, true);
                _detalhe += "N"; // Identifica��o de t�tulo, Aceito ou N�o aceito

                //A data informada neste campo deve ser a mesma data de emiss�o do t�tulo de cr�dito 
                //(Duplicata de Servi�o / Duplicata Mercantil / Nota Fiscal, etc), que deu origem a esta Cobran�a. 
                //Existindo diverg�ncia, na exist�ncia de protesto, a documenta��o poder� n�o ser aceita pelo Cart�rio.
                _detalhe += boleto.DataDocumento.ToString("ddMMyy");

                switch (boleto.Instrucoes.Count)
                {
                    case 0:
                        _detalhe += "0000";
                        break;
                    case 1:
                        _detalhe += Utils.FitStringLength(boleto.Instrucoes[0].Codigo.ToString(), 2, 2, '0', 0, true, true, true);
                        _detalhe += "00";
                        break;
                    default:
                        _detalhe += Utils.FitStringLength(boleto.Instrucoes[0].Codigo.ToString(), 2, 2, '0', 0, true, true, true);
                        _detalhe += Utils.FitStringLength(boleto.Instrucoes[1].Codigo.ToString(), 2, 2, '0', 0, true, true, true);
                        break;
                }

                //if (boleto.Instrucoes.Count > 1)
                //    _detalhe += Utils.FitStringLength(boleto.Instrucoes[0].Codigo.ToString(), 2, 2, '0', 0, true, true, true);

                //if (boleto.Instrucoes.Count > 2)
                //    _detalhe += Utils.FitStringLength(boleto.Instrucoes[1].Codigo.ToString(), 2, 2, '0', 0, true, true, true);
                //else
                //_detalhe += "  ";
                //    _detalhe += "    ";

                // Juros de 1 dia
                //Se o cliente optar pelo padr�o do Banco Ita� ou solicitar o cadastramento permanente na conta corrente, 
                //n�o haver� a necessidade de informar esse valor.
                //Caso seja expresso em moeda vari�vel, dever� ser preenchido com cinco casas decimais.

                //_detalhe += "0000000000000";
                var valorJuros = boleto.ValorBoleto * (boleto.PercJurosMora / 100);

                _detalhe += Utils.FitStringLength(valorJuros.ToString("0.00").Replace(",", ""), 13, 13, '0', 0, true, true, true);

                // Data limite para desconto
                _detalhe += boleto.DataVencimento.ToString("ddMMyy");
                _detalhe += Utils.FitStringLength(boleto.ValorDesconto.ToString("0.00").Replace(",", ""), 13, 13, '0', 0, true, true, true);
                _detalhe += "0000000000000"; // Valor do IOF
                _detalhe += "0000000000000"; // Valor do Abatimento

                if (boleto.Sacado.CPFCNPJ.Length <= 11)
                    _detalhe += "01";  // CPF
                else
                    _detalhe += "02"; // CNPJ

                _detalhe += Utils.FitStringLength(boleto.Sacado.CPFCNPJ, 14, 14, '0', 0, true, true, true).ToUpper();
                _detalhe += Utils.FitStringLength(boleto.Sacado.Nome.TrimStart(' '), 30, 30, ' ', 0, true, true, false);
                _detalhe += usoBanco;
                _detalhe += Utils.FitStringLength(boleto.Sacado.Endereco.End.TrimStart(' '), 40, 40, ' ', 0, true, true, false).ToUpper();
                _detalhe += Utils.FitStringLength(boleto.Sacado.Endereco.Bairro.TrimStart(' '), 12, 12, ' ', 0, true, true, false).ToUpper();
                _detalhe += Utils.FitStringLength(boleto.Sacado.Endereco.CEP, 8, 8, ' ', 0, true, true, false).ToUpper();
                ;
                _detalhe += Utils.FitStringLength(boleto.Sacado.Endereco.Cidade, 15, 15, ' ', 0, true, true, false).ToUpper();
                _detalhe += Utils.FitStringLength(boleto.Sacado.Endereco.UF, 2, 2, ' ', 0, true, true, false).ToUpper();
                // SACADOR/AVALISTA
                // Normalmente deve ser preenchido com o nome do sacador/avalista. Alternativamente este campo poder� 
                // ter dois outros usos:
                // a) 2o e 3o descontos: para de operar com mais de um desconto(depende de cadastramento pr�vio do 
                // indicador 19.0 pelo Banco Ita�, conforme item 5)
                // b) Mensagens ao sacado: se utilizados as instru��es 93 ou 94 (Nota 11), transcrever a mensagem desejada
                _detalhe += Utils.FitStringLength(boleto.Sacado.Nome, 30, 30, ' ', 0, true, true, false).ToUpper();
                _detalhe += "    "; // Complemento do registro
                _detalhe += boleto.DataVencimento.ToString("ddMMyy");
                // PRAZO - Quantidade de DIAS - ver nota 11(A) - depende das instru��es de cobran�a 
                _detalhe += "00";
                _detalhe += " "; // Complemento do registro
                _detalhe += Utils.FitStringLength(numeroRegistro.ToString(), 6, 6, '0', 0, true, true, true);

                _detalhe = Utils.SubstituiCaracteresEspeciais(_detalhe);

                return _detalhe;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar DETALHE do arquivo CNAB400.", ex);
            }
        }

        # endregion DETALHE

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
                        _trailer = GerarTrailerRemessa400(numeroRegistro);
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

        public string GerarTrailerRemessa400(int numeroRegistro)
        {
            try
            {
                string complemento = new string(' ', 393);
                string _trailer;

                _trailer = "9";
                _trailer += complemento;
                _trailer += Utils.FitStringLength(numeroRegistro.ToString(), 6, 6, '0', 0, true, true, true); // N�mero sequencial do registro no arquivo.

                _trailer = Utils.SubstituiCaracteresEspeciais(_trailer);

                return _trailer;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a gera��o do registro TRAILER do arquivo de REMESSA.", ex);
            }
        }
        #endregion

        #endregion 

        #region M�todos de processamento do arquivo retorno CNAB400

        public override HeaderRetorno LerHeaderRetornoCNAB400(string registro)
        {
            try
            {
                HeaderRetorno header = new HeaderRetorno();
                header.RegistroHeader = registro;
                header.Agencia = Convert.ToInt32(registro.Substring(26, 4));
                header.ContaCorrente = Convert.ToInt32(registro.Substring(32, 5));
                header.DigitoContaCorrente = Convert.ToInt32(registro.Substring(37, 1));
                //header.CodigoCedente = Convert.ToInt32(string.Format("{0}{1}", header.ContaCorrente, header.DigitoContaCorrente));

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
                int dataOcorrencia = Utils.ToInt32(registro.Substring(110, 6));
                int dataVencimento = Utils.ToInt32(registro.Substring(146, 6));
                int dataCredito = Utils.ToInt32(registro.Substring(295, 6));

                DetalheRetorno detalhe = new DetalheRetorno(registro);

                detalhe.CodigoInscricao = Utils.ToInt32(registro.Substring(1, 2));
                detalhe.NumeroInscricao = registro.Substring(3, 14);
                detalhe.Agencia = Utils.ToInt32(registro.Substring(17, 4));
                detalhe.Conta = Utils.ToInt32(registro.Substring(23, 5));
                detalhe.DACConta = Utils.ToInt32(registro.Substring(28, 1));
                detalhe.UsoEmpresa = registro.Substring(37, 25);
                //
                detalhe.NossoNumeroComDV = registro.Substring(85, 9);
                detalhe.NossoNumero = registro.Substring(85, 8); //Sem o DV
                detalhe.DACNossoNumero = registro.Substring(93, 1); //DV
                //
                detalhe.Carteira = registro.Substring(107, 1);
                detalhe.CodigoOcorrencia = Utils.ToInt32(registro.Substring(108, 2));

                switch (detalhe.CodigoOcorrencia)
                {
                    case (int)ECodigoOcorrenciaItau400.Entrada_Confirmada:
                        detalhe.Aceito = true;
                        break;
                    case (int)ECodigoOcorrenciaItau400.Liquida��o:
                    case (int)ECodigoOcorrenciaItau400.Liquida��o_Em_Cart�rio:
                    case (int)ECodigoOcorrenciaItau400.Liquida��o_Parcial:
                        detalhe.Baixado = true;
                        detalhe.Aceito = false;
                        break;
                    case (int)ECodigoOcorrenciaItau400.Baixa:
                        detalhe.Cancelado = true;
                        detalhe.Aceito = false;
                        break;
                    case (int)ECodigoOcorrenciaItau400.Rejei��o:
                        detalhe.Aceito = false;
                        break;
                }

                //Descri��o da ocorr�ncia
                detalhe.DescricaoOcorrencia = this.Ocorrencia(registro.Substring(108, 2));
                detalhe.MotivoCodigoOcorrencia = detalhe.DescricaoOcorrencia;
                detalhe.DataOcorrencia = Utils.ToDateTime(dataOcorrencia.ToString("##-##-##"));
                detalhe.NumeroDocumento = registro.Substring(116, 10);
                //
                detalhe.DataVencimento = Utils.ToDateTime(dataVencimento.ToString("##-##-##"));
                decimal valorTitulo = Convert.ToInt64(registro.Substring(152, 13));
                detalhe.ValorTitulo = valorTitulo / 100;
                detalhe.CodigoBanco = Utils.ToInt32(registro.Substring(165, 3));
                detalhe.AgenciaCobradora = Utils.ToInt32(registro.Substring(168, 4));
                detalhe.Especie = Utils.ToInt32(registro.Substring(173, 2));
                decimal tarifaCobranca = Convert.ToUInt64(registro.Substring(175, 13));
                detalhe.TarifaCobranca = tarifaCobranca / 100;
                // 26 brancos
                decimal iof = Convert.ToUInt64(registro.Substring(214, 13));
                detalhe.IOF = iof / 100;
                decimal valorAbatimento = Convert.ToUInt64(registro.Substring(227, 13));
                detalhe.ValorAbatimento = valorAbatimento / 100;

                decimal valorDescontos = Convert.ToUInt64(registro.Substring(240, 13));
                detalhe.Descontos = valorDescontos / 100;

                decimal valorPrincipal = Convert.ToUInt64(registro.Substring(253, 13));
                detalhe.ValorPrincipal = valorPrincipal / 100;

                decimal jurosMora = Convert.ToUInt64(registro.Substring(266, 13));
                detalhe.JurosMora = jurosMora / 100;
                // 293 - 3 brancos
                detalhe.DataCredito = Utils.ToDateTime(dataCredito.ToString("##-##-##"));
                detalhe.InstrucaoCancelada = Utils.ToInt32(registro.Substring(301, 4));
                // 306 - 6 brancos
                // 311 - 13 zeros
                detalhe.NomeSacado = registro.Substring(324, 30);
                // 354 - 23 brancos
                detalhe.Erros = registro.Substring(377, 8);
                // 377 - Registros rejeitados ou alega��o do sacado
                // 386 - 7 brancos

                detalhe.CodigoLiquidacao = registro.Substring(392, 2);
                detalhe.NumeroSequencial = Utils.ToInt32(registro.Substring(394, 6));
                detalhe.ValorPago = detalhe.ValorPrincipal + detalhe.TarifaCobranca;

                // A correspond�ncia de Valor Pago no RETORNO ITA� � o Valor Principal (Valor lan�ado em Conta Corrente - Conforme Manual)
                // A determina��o se D�bito ou Cr�dito dever� ser feita nos aplicativos por se tratar de personaliza��o.
                // Para isso, considerar o C�digo da Ocorr�ncia e tratar de acordo com suas necessidades.
                // Alterado por jsoda em 04/06/2012
                //
                //// Valor principal � d�bito ou cr�dito ?
                //if ( (detalhe.ValorTitulo < detalhe.TarifaCobranca) ||
                //     ((detalhe.ValorTitulo - detalhe.Descontos) < detalhe.TarifaCobranca)
                //    )
                //{
                //    detalhe.ValorPrincipal *= -1; // Para d�bito coloca valor negativo
                //}


                //// Valor Pago � a soma do Valor Principal (Valor que entra na conta) + Tarifa de Cobran�a
                //detalhe.ValorPago = detalhe.ValorPrincipal + detalhe.TarifaCobranca;

                return detalhe;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao ler detalhe do arquivo de RETORNO / CNAB 400.", ex);
            }
        }

        #endregion

        #region M�todos de gera��o do arquivo remessa CNAB240

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
                throw new Exception("Erro durante a gera��o do HEADER DO LOTE do arquivo de REMESSA.", ex);
            }
        }

        public string GerarHeaderRemessaCNAB240(Cedente cedente, int numeroArquivoRemessa)
        {
            try
            {
                var agEConta = cedente.ContaBancaria.Agencia + cedente.ContaBancaria.Conta;

                string header = Utils.FormatCode(Codigo.ToString(), "0", 3, true);                      //001-003 Codigo do banco                           
                header += "0000";                                                                       //004-007 Lote do Servi�o = "0000"
                header += "0";                                                                          //008-008 Registro Header do Arquivo = "0"
                header += Utils.FormatCode("", " ", 9);                                                 //009-017 Complemento do Registro (BRANCOS)
                header += (cedente.CPFCNPJ.Length == 11 ? "1" : "2");                                   //018-018 Tipo de Inscri��o da Empresa 1=CPF, 2=CNPJ
                header += Utils.FormatCode(cedente.CPFCNPJ, "0", 14, true);                             //019-032 Nro Inscri��o da Empresa 
                header += Utils.FormatCode("", " ", 20);                                                //033-052 Complemento de Registro (BRANCOS)
                header += "0";                                                                          //053-053 Complemento de Registro "0"
                header += Utils.FormatCode(cedente.ContaBancaria.Agencia, " ", 4, true);                //054-057 Agencia
                header += " ";                                                                          //058-058 Complemento de Registro (BRANCOS)
                header += Utils.FormatCode("", "0", 7);                                                 //059-065 Complemento de Registro "0000000"
                header += Utils.FormatCode(cedente.ContaBancaria.Conta, "0", 5, true);                  //066-070 Conta Corrente da Empresa
                header += Utils.FormatCode("", " ", 1);                                                 //071-071 Complemento de Registro (BRANCOS)
                header += Banco.Mod11(agEConta).ToString();                                             //072-072 DAC Ag/Conta Empresa
                header += Utils.FitStringLength(cedente.Nome, 30, 30, ' ', 0, true, true, false);       //073-102 Nome da Empresa
                header += Utils.FitStringLength("BANCO ITAU SA", 30, 30, ' ', 0, true, true, false);    //103-132 Nome do Banco = "BANCO ITAU SA"
                header += Utils.FormatCode("", " ", 10);                                                //133-142 Complemento de Registro (BRANCOS)
                header += "1";                                                                          //143-143 C�digo Remessa = "1"
                header += DateTime.Now.ToString("ddMMyyyy");                                            //144-151 Data de Gera��o do Arquivo DDMMAAAA
                header += DateTime.Now.ToString("HHmmss");                                              //152-157 Hora de Gera��o do Arquivo HHMMSS
                header += Utils.FormatCode(numeroArquivoRemessa.ToString(), "0", 6);                    //158-163 Numero Sequ�ncial do Arquivo de Retorno
                header += "040";                                                                        //164-166 Numero da Vers�o do Layout do Arquivo = "040"                                                    
                header += "00000";                                                                      //167-171 Complemento de Registro "00000"
                header += Utils.FormatCode("", " ", 54);                                                //172-225 Complemento de Registro (BRANCOS)
                header += "000";                                                                        //226-228 Complemento de Registro "000"
                header += Utils.FormatCode("", " ", 12);                                                //229-240 Complemento de Registro (BRANCOS)
                header = Utils.SubstituiCaracteresEspeciais(header);
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
                var agEConta = cedente.ContaBancaria.Agencia + cedente.ContaBancaria.Conta;

                string header = Utils.FormatCode(Codigo.ToString(), "0", 3, true);                  //001-003 C�digo do Banco
                header += "0001";                                                                   //004-007 Lote do Servi�o
                header += "1";                                                                      //008-008 Registro Header do Lote
                header += "R";                                                                      //009-009 Tipo de Opera��o R-Remessa T-Retorno
                header += "01";                                                                     //010-011 Identifica��o do tipo de servi�o = "01"
                header += "00";                                                                     //012-013 Complemento do Registro "00"
                header += "030";                                                                    //014-016 Nro do Layout do Lote = "030"
                header += " ";                                                                      //017-017 Complemento do Registro (BRANCOS)
                header += (cedente.CPFCNPJ.Length == 11 ? "1" : "2");                               //018-018 Tipo de Inscri��o da Empresa 1=CPF, 2=CNPJ
                header += Utils.FormatCode(cedente.CPFCNPJ, "0", 15, true);                         //019-033 Nro de Inscri��o da Empresa
                header += Utils.FormatCode("", " ", 20);                                            //034-053 Complemento do Registro (BRANCOS)                                     
                header += "0";                                                                      //054-054 Complemento do Registro "0"
                header += Utils.FormatCode(cedente.ContaBancaria.Agencia, "0", 4, true);            //055-058 Agencia
                header += " ";                                                                      //059-059 Complemento do Registro (BRANCOS)
                header += "0000000";                                                                //060-066 Complemento do Registro "0000000"
                header += Utils.FormatCode(cedente.ContaBancaria.Conta, "0", 5, true);              //067-071 Conta Corrente
                header += " ";                                                                      //072-072 Complemento do Registro (BRANCOS)
                header += Banco.Mod11(agEConta).ToString();                                         //073-073 DAC Ag/Conta Empresa
                header += Utils.FitStringLength(cedente.Nome, 30, 30, ' ', 0, true, true, false);   //074-103 Nome da Empresa
                header += Utils.FormatCode("", " ", 80);                                            //104-183 Complemento do Registro (BRANCOS)
                header += Utils.FormatCode(numeroArquivoRemessa.ToString(), "0", 8);                //184-191 Numero Sequencial do arquivo retorno
                header += DateTime.Now.ToString("ddMMyyyy");                                        //192-199 6 Data de grava��o da remessa  DDMMAAAA 
                header += Utils.FormatCode("", " ", 41);                                            //199-240 BRANCOS
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
        public string GerarDetalheRemessaCNAB240(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
        {
            throw new NotImplementedException("Fun��o n�o implementada.");
        }

        public override string GerarDetalheSegmentoPRemessa(Boleto boleto, int numeroRegistro, string codigoTransmissao)
        {
            try
            {
                string _segmentoP;

                var dvAgencia = boleto.Cedente.ContaBancaria.DigitoAgencia == null ? "0" : boleto.Cedente.ContaBancaria.DigitoAgencia;
                var agEConta = boleto.Cedente.ContaBancaria.Agencia + boleto.Cedente.ContaBancaria.Conta;

                _segmentoP = Utils.FormatCode(Codigo.ToString(), "0", 3, true);
                _segmentoP += "0001";
                _segmentoP += "3";
                _segmentoP += Utils.FitStringLength(numeroRegistro.ToString(), 5, 5, '0', 0, true, true, true);
                _segmentoP += "P";
                _segmentoP += " ";
                _segmentoP += Utils.FitStringLength(boleto.CodigoOcorrencia, 2, 2, '0', 0, true, true, true);
                _segmentoP += "0";
                _segmentoP += Utils.FitStringLength(boleto.Cedente.ContaBancaria.Agencia, 4, 4, '0', 0, true, true, true);
                _segmentoP += " ";
                _segmentoP += "0000000";
                _segmentoP += Utils.FitStringLength(boleto.Cedente.ContaBancaria.Conta, 5, 5, '0', 0, true, true, true);
                _segmentoP += " ";
                _segmentoP += Banco.Mod11(agEConta).ToString();
                _segmentoP += Utils.FitStringLength(boleto.Carteira, 3, 3, '0', 0, true, true, true);
                _segmentoP += Utils.FitStringLength(boleto.NossoNumero, 9, 9, '0', 0, true, true, true);
                _segmentoP += Utils.FormatCode("", " ", 8, true);
                _segmentoP += Utils.FormatCode("", "0", 5, true);
                _segmentoP += Utils.FitStringLength(boleto.NumeroDocumento, 10, 10, ' ', 0, true, true, false);
                _segmentoP += Utils.FormatCode("", " ", 5, true);
                _segmentoP += Utils.FitStringLength(boleto.DataVencimento.ToString("ddMMyyyy"), 8, 8, ' ', 0, true, true, false);
                _segmentoP += Utils.FitStringLength(boleto.ValorBoleto.ToString("0.00").Replace(",", ""), 15, 15, '0', 0, true, true, true);
                _segmentoP += "00000 ";
                _segmentoP += Utils.FitStringLength(boleto.EspecieDocumento.Codigo.ToString(), 2, 2, '0', 0, true, true, true);
                _segmentoP += "N";
                _segmentoP += Utils.FitStringLength(boleto.DataDocumento.ToString("ddMMyyyy"), 8, 8, ' ', 0, true, true, false);
                _segmentoP += "0";
                #region Juros e Desconto
                if (boleto.PercJurosMora > 0)
                {
                    var valorJuros = boleto.ValorBoleto * (boleto.PercJurosMora / 100);

                    _segmentoP += Utils.FitStringLength(boleto.DataVencimento.ToString("ddMMyyyy"), 8, 8, '0', 0, true, true, false);
                    _segmentoP += Utils.FitStringLength(valorJuros.ToString("0.00").Replace(",", ""), 15, 15, '0', 0, true, true, true);
                }
                else
                {
                    _segmentoP += Utils.FitStringLength("", 8, 8, '0', 0, true, true, false);
                    _segmentoP += Utils.FitStringLength("", 15, 15, '0', 0, true, true, false);
                }

                _segmentoP += "0";

                if (boleto.ValorDesconto > 0)
                {
                    _segmentoP += Utils.FitStringLength(boleto.DataVencimento.ToString("ddMMyyyy"), 8, 8, '0', 0, true, true, false);
                    _segmentoP += Utils.FitStringLength(boleto.ValorDesconto.ToString("0.00").Replace(",", ""), 15, 15, '0', 0, true, true, true);
                }
                else
                {
                    _segmentoP += Utils.FitStringLength("", 8, 8, '0', 0, true, true, false);
                    _segmentoP += Utils.FitStringLength("", 15, 15, '0', 0, true, true, false);
                }
                #endregion
                _segmentoP += Utils.FitStringLength("", 15, 15, '0', 0, true, true, false); //iof
                _segmentoP += Utils.FitStringLength("", 15, 15, '0', 0, true, true, false); //abatimento
                _segmentoP += Utils.FitStringLength(boleto.NumeroDocumento, 25, 25, ' ', 0, true, true, false);

                string codigo_protesto = "3";
                string dias_protesto = "00";

                foreach (Instrucao instrucao in boleto.Instrucoes)
                {
                    switch ((EnumInstrucoes_Itau)instrucao.Codigo)
                    {
                        case EnumInstrucoes_Itau.Protestar:
                            codigo_protesto = "1";
                            dias_protesto = Utils.FitStringLength(instrucao.QuantidadeDias.ToString(), 2, 2, '0', 0, true, true, true);
                            break;
                        case EnumInstrucoes_Itau.NaoProtestar:
                            codigo_protesto = "3";
                            dias_protesto = "00";
                            break;
                        default:
                            break;
                    }
                }

                _segmentoP += codigo_protesto;
                _segmentoP += dias_protesto;

                _segmentoP += "0000000000000000 ";

                _segmentoP = Utils.SubstituiCaracteresEspeciais(_segmentoP);

                return _segmentoP;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a gera��o do SEGMENTO P DO DETALHE do arquivo de REMESSA.", ex);
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
                _segmentoQ += Utils.FitStringLength(boleto.Sacado.Nome.TrimStart(' '), 30, 30, ' ', 0, true, true, false).ToUpper();
                _segmentoQ += Utils.FormatCode("", " ", 10, true);
                _segmentoQ += Utils.FitStringLength(boleto.Sacado.Endereco.End.TrimStart(' '), 40, 40, ' ', 0, true, true, false).ToUpper();
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
                throw new Exception("Erro durante a gera��o do SEGMENTO Q DO DETALHE do arquivo de REMESSA.", ex);
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
                    // C�digo da multa 2 - percentual
                    _segmentoR += "2";
                    _segmentoR += Utils.FitStringLength(boleto.DataVencimento.ToString("ddMMyyyy"), 8, 8, '0', 0, true, true, false);
                    _segmentoR += Utils.FitStringLength(boleto.ValorMulta.ToString("0.00").Replace(",", ""), 15, 15, '0', 0, true, true, true);
                }
                else if (boleto.ValorMulta > 0)
                {
                    // C�digo da multa 1 - valor fixo
                    _segmentoR += "1";
                    _segmentoR += Utils.FitStringLength(boleto.DataVencimento.ToString("ddMMyyyy"), 8, 8, '0', 0, true, true, false);
                    _segmentoR += Utils.FitStringLength(boleto.ValorMulta.ToString("0.00").Replace(",", ""), 15, 15, '0', 0, true, true, true);
                }
                else
                {
                    // C�digo da multa 0 - sem multa
                    _segmentoR += "0";
                    _segmentoR += Utils.FitStringLength("", 8, 8, '0', 0, true, true, false);
                    _segmentoR += Utils.FitStringLength("", 15, 15, '0', 0, true, true, true);
                }
                #endregion
                _segmentoR += Utils.FormatCode("", " ", 10, true);
                _segmentoR += Utils.FormatCode("", " ", 40, true);
                _segmentoR += Utils.FormatCode("", " ", 60, true);
                _segmentoR += Utils.FormatCode("", "0", 16, true);
                _segmentoR += " ";
                _segmentoR += Utils.FormatCode("", "0", 12, true);
                _segmentoR += "  0         ";

                _segmentoR = Utils.SubstituiCaracteresEspeciais(_segmentoR);

                return _segmentoR;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a gera��o do SEGMENTO R DO DETALHE do arquivo de REMESSA.", ex);
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
                throw new Exception("Erro durante a gera��o do SEGMENTO S DO DETALHE do arquivo de REMESSA.", ex);
            }
        }
        #endregion

        #region TRAILER
        public string GerarTrailerRemessa240()
        {
            throw new NotImplementedException("Fun��o n�o implementada.");
        }

        public override string GerarTrailerLoteRemessa(int numeroRegistro)
        {
            try
            {
                string trailer = Utils.FormatCode(Codigo.ToString(), "0", 3, true);
                trailer += Utils.FormatCode("", "0", 4, true);
                trailer += "5";
                trailer += Utils.FormatCode("", " ", 9);
                trailer += Utils.FormatCode("", "0", 6, true);
                trailer += Utils.FormatCode("", "0", 18, true);
                trailer += Utils.FormatCode("", "0", 18, true);
                trailer += Utils.FormatCode("", " ", 171);
                trailer += Utils.FormatCode("", " ", 10);
                trailer = Utils.SubstituiCaracteresEspeciais(trailer);

                return trailer;
            }
            catch (Exception e)
            {
                throw new Exception("Erro durante a gera��o do registro TRAILER do LOTE de REMESSA.", e);
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
                trailer += Utils.FormatCode("", "0", 6, true);
                trailer += Utils.FormatCode("", "0", 6, true);
                trailer += Utils.FormatCode("", "0", 6, true);
                trailer += Utils.FormatCode("", " ", 205);
                trailer = Utils.SubstituiCaracteresEspeciais(trailer);

                return trailer;
            }
            catch (Exception e)
            {
                throw new Exception("Erro durante a gera��o do registro TRAILER do ARQUIVO de REMESSA.", e);
            }
        }
        #endregion

        #endregion

        #region M�todo de processamento do arquivo retorno CNAB240
        public override HeaderRetorno LerHeaderRetornoCNAB240(string registro)
        {
            try
            {
                HeaderRetorno header = new HeaderRetorno();
                header.RegistroHeader = registro;
                header.Agencia = Convert.ToInt32(registro.Substring(52, 5));
                header.DigitoAgencia = Convert.ToInt32(registro.Substring(57, 1));
                header.ContaCorrente = Convert.ToInt32(registro.Substring(58, 12));
                header.DigitoContaCorrente = Convert.ToInt32(registro.Substring(70, 1));
                header.CodigoCedente = Convert.ToInt32(registro.Substring(32, 20));

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
                    throw new Exception("Registro inv�lido. O detalhe n�o possu� as caracter�sticas do segmento T.");

                segmentoT.CodigoBanco = Convert.ToInt32(registro.Substring(0, 3));
                segmentoT.idCodigoMovimento = Convert.ToInt32(registro.Substring(15, 2));
                segmentoT.Agencia = Convert.ToInt32(registro.Substring(18, 4));
                segmentoT.Conta = Convert.ToInt32(registro.Substring(30, 5));
                segmentoT.DigitoConta = registro.Substring(36, 1);

                segmentoT.NossoNumero = registro.Substring(40, 8);
                segmentoT.CodigoCarteira = Convert.ToInt32(registro.Substring(37, 3));
                segmentoT.NumeroDocumento = registro.Substring(58, 10);
                int dataVencimento = Convert.ToInt32(registro.Substring(73, 8));
                segmentoT.DataVencimento = Convert.ToDateTime(dataVencimento.ToString("##-##-####"));
                decimal valorTitulo = Convert.ToDecimal(registro.Substring(81, 15));
                segmentoT.ValorTitulo = valorTitulo / 100;
                segmentoT.IdentificacaoTituloEmpresa = registro.Substring(105, 25);
                segmentoT.TipoInscricao = Convert.ToInt32(registro.Substring(132, 1));
                segmentoT.NumeroInscricao = registro.Substring(133, 15);
                segmentoT.NomeSacado = registro.Substring(148, 30);
                segmentoT.ValorTarifas = Convert.ToDecimal(registro.Substring(198, 15)) / 100;
                segmentoT.IdRejeicao1 = registro.Substring(213, 2);
                segmentoT.IdRejeicao2 = registro.Substring(215, 2);
                segmentoT.IdRejeicao3 = registro.Substring(217, 2);
                segmentoT.IdRejeicao4 = registro.Substring(219, 2);
                segmentoT.UsoFebraban = registro.Substring(221, 22);
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

                switch (Convert.ToInt32(segmentoT.idCodigoMovimento))
                {
                    case (int)EnumCodigoMovimento_Itau.Entrada_confirmada:
                        segmentoT.Aceito = true;
                        break;
                    case (int)EnumCodigoMovimento_Itau.Liquida��o:
                    case (int)EnumCodigoMovimento_Itau.Liquida��o_Em_Cart�rio:
                        segmentoT.Baixado = true;
                        segmentoT.Aceito = false;
                        break;
                    case (int)EnumCodigoMovimento_Itau.Baixa:
                    case (int)EnumCodigoMovimento_Itau.Baixa_Por_Ter_Sido_Liquidado:
                        segmentoT.Cancelado = true;
                        segmentoT.Aceito = false;
                        break;
                    case (int)EnumCodigoMovimento_Itau.Entrada_rejeitada:
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
            string _Controle_lote = registro.Substring(3, 4); //02
            string _Controle_regis = registro.Substring(7, 1); //03
            string _Servico_numRegis = registro.Substring(8, 5); //04
            string _Servico_segmento = registro.Substring(13, 1); //05
            string _cnab06 = registro.Substring(14, 1); //06
            string _Servico_codMov = registro.Substring(15, 2); //07
            string _dadosTituloAcrescimo = registro.Substring(17, 15); //08
            string _dadosTituloValorDesconto = registro.Substring(32, 15); //09
            string _dadosTituloValorAbatimento = registro.Substring(47, 15); //10
            string _dadosTituloValorIof = registro.Substring(62, 15); //11
            string _dadosTituloValorPago = registro.Substring(77, 15); //12
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
                    throw new Exception("Registro inv�lida. O detalhe n�o possu� as caracter�sticas do segmento U.");
                }

                var segmentoU = new DetalheSegmentoURetornoCNAB240(registro);
                segmentoU.Servico_Codigo_Movimento_Retorno = Convert.ToDecimal(registro.Substring(15, 2)); //07.3U|Servi�o|C�d. Mov.|C�digo de Movimento Retorno
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
