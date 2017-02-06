using System;
using System.Collections.Generic;
using System.Web.UI;
using BoletoNet;
using Microsoft.VisualBasic;
using System.Globalization;

[assembly: WebResource("BoletoNet.Imagens.237.jpg", "image/jpg")]

namespace BoletoNet
{
    internal class Banco_Bradesco : AbstractBanco, IBanco
    {
        private string _dacNossoNumero = string.Empty;
        private int _dacBoleto = 0;

        internal Banco_Bradesco()
        {
            this.Codigo = 237;
            this.Digito = "2";
            this.Nome = "Bradesco";
        }

        #region IBanco Members

        public override void FormataLinhaDigitavel(Boleto boleto)
        {

            //BBBMC.CCCCD1 CCCCC.CCCCCD2 CCCCC.CCCCCD3 D4 FFFFVVVVVVVVVV


            #region Campo 1

            string Grupo1 = string.Empty;

            string BBB = boleto.CodigoBarra.Codigo.Substring(0, 3);
            string M = boleto.CodigoBarra.Codigo.Substring(3, 1);
            string CCCCC = boleto.CodigoBarra.Codigo.Substring(19, 5);
            string D1 = Mod10(BBB + M + CCCCC).ToString();

            Grupo1 = string.Format("{0}{1}{2}.{3}{4} ", BBB, M, CCCCC.Substring(0, 1), CCCCC.Substring(1, 4), D1);


            #endregion Campo 1

            #region Campo 2

            string Grupo2 = string.Empty;

            string CCCCCCCCCC2 = boleto.CodigoBarra.Codigo.Substring(24, 10);
            string D2 = Mod10(CCCCCCCCCC2).ToString();

            Grupo2 = string.Format("{0}.{1}{2} ", CCCCCCCCCC2.Substring(0, 5), CCCCCCCCCC2.Substring(5, 5), D2);

            #endregion Campo 2

            #region Campo 3

            string Grupo3 = string.Empty;

            string CCCCCCCCCC3 = boleto.CodigoBarra.Codigo.Substring(34, 10);
            string D3 = Mod10(CCCCCCCCCC3).ToString();

            Grupo3 = string.Format("{0}.{1}{2} ", CCCCCCCCCC3.Substring(0, 5), CCCCCCCCCC3.Substring(5, 5), D3);


            #endregion Campo 3

            #region Campo 4

            string Grupo4 = string.Empty;

            string D4 = _dacBoleto.ToString();

            Grupo4 = string.Format("{0} ", D4);

            #endregion Campo 4

            #region Campo 5

            string Grupo5 = string.Empty;

            //string FFFF = boleto.CodigoBarra.Codigo.Substring(5, 4);//FatorVencimento(boleto).ToString() ;
            string FFFF = FatorVencimento(boleto).ToString();

            //if (boleto.Carteira == "06" && !Utils.DataValida(boleto.DataVencimento))
            //    FFFF = "0000";

            var VVVVVVVVVV = "";

            if (boleto.SegundaVia)
                VVVVVVVVVV = Utils.FormatCode(boleto.ValorCobrado.ToString("f").Replace(",", "").Replace(".", ""), 10);//10
            else
                VVVVVVVVVV = Utils.FormatCode(boleto.ValorBoleto.ToString("f").Replace(",", "").Replace(".", ""), 10);//10

            //if (Utils.ToInt64(VVVVVVVVVV) == 0)
            //    VVVVVVVVVV = "000";

            Grupo5 = string.Format("{0}{1}", FFFF, VVVVVVVVVV);

            #endregion Campo 5

            boleto.CodigoBarra.LinhaDigitavel = Grupo1 + Grupo2 + Grupo3 + Grupo4 + Grupo5;

        }

        public override void FormataCodigoBarra(Boleto boleto)
        {
            var valorBoleto = "";
            //4
            if (boleto.SegundaVia)
                valorBoleto = Utils.FormatCode(boleto.ValorCobrado.ToString("f").Replace(",", "").Replace(".", ""), 10);//10
            else
                valorBoleto = Utils.FormatCode(boleto.ValorBoleto.ToString("f").Replace(",", "").Replace(".", ""), 10);//10

            if (boleto.Carteira == "02" || boleto.Carteira == "03" || boleto.Carteira == "09" || boleto.Carteira == "19" || boleto.Carteira == "16")
            {
                boleto.CodigoBarra.Codigo = string.Format("{0}{1}{2}{3}{4}", Codigo.ToString(), boleto.Moeda,
                FatorVencimento(boleto), valorBoleto, FormataCampoLivre(boleto));
            }
            else if (boleto.Carteira == "06")
            {
                if (boleto.ValorBoleto == 0)
                {
                    boleto.CodigoBarra.Codigo = string.Format("{0}{1}0000{2}{3}", Codigo.ToString(), boleto.Moeda,
                        valorBoleto, FormataCampoLivre(boleto));
                }
                else
                {
                    boleto.CodigoBarra.Codigo = string.Format("{0}{1}{2}{3}{4}", Codigo.ToString(), boleto.Moeda,
                        FatorVencimento(boleto), valorBoleto, FormataCampoLivre(boleto));
                }

            }
            else
            {
                throw new NotImplementedException("Carteira ainda n�o implementada.");
            }


            _dacBoleto = Mod11(boleto.CodigoBarra.Codigo, 9);

            boleto.CodigoBarra.Codigo = Strings.Left(boleto.CodigoBarra.Codigo, 4) + _dacBoleto + Strings.Right(boleto.CodigoBarra.Codigo, 39);
        }

        public string FormataCampoLivre(Boleto boleto)
        {

            string FormataCampoLivre = string.Format("{0}{1}{2}{3}{4}", boleto.Cedente.ContaBancaria.Agencia, boleto.Carteira,
                                            boleto.NossoNumero, boleto.Cedente.ContaBancaria.Conta, "0");

            return FormataCampoLivre;
        }

        public override void FormataNumeroDocumento(Boleto boleto)
        {
            throw new NotImplementedException("Fun��o ainda n�o implementada.");
        }

        public override void FormataNossoNumero(Boleto boleto)
        {
            boleto.NossoNumero = string.Format("{0}/{1}-{2}", boleto.Carteira, boleto.NossoNumero, boleto.DigitoNossoNumero);
        }

        public override string GerarHeaderRemessa(string numeroConvenio, Cedente cedente, TipoArquivo tipoArquivo, int numeroArquivoRemessa, Boleto boletos)
        {
            throw new NotImplementedException("Fun��o n�o implementada.");
        }

        public override void ValidaBoleto(Boleto boleto)
        {
            if (boleto.Carteira != "02" && boleto.Carteira != "03" && boleto.Carteira != "06" && boleto.Carteira != "09" && boleto.Carteira != "19" && boleto.Carteira != "16")
                throw new NotImplementedException("Bradesco - Carteira n�o implementada. Carteiras implementadas 02, 03, 06, 09, 19.");

            //O valor � obrigat�rio para a carteira 03
            if (boleto.Carteira == "03")
            {
                if (boleto.ValorBoleto == 0)
                    throw new NotImplementedException("Bradesco - Para a carteira 03, o valor do boleto n�o pode ser igual a zero");
            }

            //O valor � obrigat�rio para a carteira 09
            if (boleto.Carteira == "09" || boleto.Carteira == "16")
            {
                if (boleto.ValorBoleto == 0)
                    throw new NotImplementedException("Bradesco - Para a carteira 09, o valor do boleto n�o pode ser igual a zero");
            }

            //Verifica se o nosso n�mero � v�lido
            if (boleto.NossoNumero.Length > 11)
                throw new NotImplementedException("Bradesco - A quantidade m�xima de caracteres do nosso n�mero, s�o 11 n�meros.");
            else if (boleto.NossoNumero.Length < 11)
                boleto.NossoNumero = Utils.FormatCode(boleto.NossoNumero, 11);

            //Verificar se a Agencia esta correta
            if (boleto.Cedente.ContaBancaria.Agencia.Length > 4)
                throw new NotImplementedException("Bradesco - A quantidade m�xima de caracteres da Ag�ncia s�o de 04 n�meros. A conta atual est� com " + boleto.Cedente.ContaBancaria.Agencia.Length + " caracteres. N�mero da ag�ncia: " + boleto.Cedente.ContaBancaria.Agencia);
            else if (boleto.Cedente.ContaBancaria.Agencia.Length < 4)
                boleto.Cedente.ContaBancaria.Agencia = Utils.FormatCode(boleto.Cedente.ContaBancaria.Agencia, 4);

            //Verificar se a Conta esta correta
            if (boleto.Cedente.ContaBancaria.Conta.Length > 7)
                throw new NotImplementedException("Bradesco - A quantidade m�xima de caracteres da Conta s�o de 07 n�meros. A conta atual est� com " + boleto.Cedente.ContaBancaria.Conta.Length + " caracteres. N�mero da conta: " + boleto.Cedente.ContaBancaria.Conta);
            else if (boleto.Cedente.ContaBancaria.Conta.Length < 7)
                boleto.Cedente.ContaBancaria.Conta = Utils.FormatCode(boleto.Cedente.ContaBancaria.Conta, 7);

            //Atribui o nome do banco ao local de pagamento
            boleto.LocalPagamento += Nome + "";

            //Verifica se data do processamento � valida
            //if (boleto.DataProcessamento.ToString("dd/MM/yyyy") == "01/01/0001")
            if (boleto.DataProcessamento == DateTime.MinValue) // diegomodolo (diego.ribeiro@nectarnet.com.br)
                boleto.DataProcessamento = DateTime.Now;


            //Verifica se data do documento � valida
            //if (boleto.DataDocumento.ToString("dd/MM/yyyy") == "01/01/0001")
            if (boleto.DataDocumento == DateTime.MinValue) // diegomodolo (diego.ribeiro@nectarnet.com.br)
                boleto.DataDocumento = DateTime.Now;

            boleto.QuantidadeMoeda = 0;

            //Atribui o nome do banco ao local de pagamento
            boleto.LocalPagamento = "PAG�VEL PREFERENCIALMENTE NAS AG�NCIAS DO BRADESCO";

            // Calcula o DAC do Nosso N�mero
            _dacNossoNumero = CalcularDigitoNossoNumero(boleto);
            boleto.DigitoNossoNumero = _dacNossoNumero;

            FormataCodigoBarra(boleto);
            FormataLinhaDigitavel(boleto);
            FormataNossoNumero(boleto);
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
                    return "06-Liquida��o normal";
                case "09":
                    return "09-Baixado Automaticamente via Arquivo";
                case "10":
                    return "10-Baixado conforme instru��es da Ag�ncia";
                case "11":
                    return "11-Em Ser - Arquivo de T�tulos pendentes";
                case "12":
                    return "12-Abatimento Concedido";
                case "13":
                    return "13-Abatimento Cancelado";
                case "14":
                    return "14-Vencimento Alterado";
                case "15":
                    return "15-Liquida��o em Cart�rio";
                case "17":
                    return "17-Liquida��o ap�s baixa ou T�tulo n�o registrado";
                case "18":
                    return "18-Acerto de Deposit�ria";
                case "19":
                    return "19-Confirma��o Recebimento Instru��o de Protesto";
                case "20":
                    return "20-Confirma��o Recebimento Instru��o Susta��o de Protesto";
                case "21":
                    return "21-Acerto do Controle do Participante";
                case "23":
                    return "23-Entrada do T�tulo em Cart�rio";
                case "24":
                    return "24-Entrada rejeitada por CEP Irregular";
                case "27":
                    return "27-Baixa Rejeitada";
                case "28":
                    return "28-D�bito de tarifas/custas";
                case "30":
                    return "30-Altera��o de Outros Dados Rejeitados";
                case "32":
                    return "32-Instru��o Rejeitada";
                case "33":
                    return "33-Confirma��o Pedido Altera��o Outros Dados";
                case "34":
                    return "34-Retirado de Cart�rio e Manuten��o Carteira";
                case "35":
                    return "35-Desagendamento ) d�bito autom�tico";
                case "68":
                    return "68-Acerto dos dados ) rateio de Cr�dito";
                case "69":
                    return "69-Cancelamento dos dados ) rateio";
                default:
                    return "";
            }
        }

        public string MotivoRejeicao(string codigo)
        {
            switch (codigo)
            {
                case "02":
                    return "02-C�digo do registro detalhe inv�lido";
                case "03":
                    return "03-C�digo da ocorr�ncia inv�lida";
                case "04":
                    return "04-C�digo de ocorr�ncia n�o permitida para a carteira";
                case "05":
                    return "05-C�digo de ocorr�ncia n�o num�rico";
                case "07":
                    return "07-Ag�ncia/conta/Digito - Inv�lido";
                case "08":
                    return "08-Nosso n�mero inv�lido";
                case "09":
                    return "09-Nosso n�mero duplicado";
                case "10":
                    return "10-Carteira inv�lida";
                case "16":
                    return "16-Data de vencimento inv�lida";
                case "18":
                    return "18-Vencimento fora do prazo de opera��o";
                case "20":
                    return "20-Valor do T�tulo inv�lido";
                case "21":
                    return "21-Esp�cie do T�tulo inv�lida";
                case "22":
                    return "22-Esp�cie n�o permitida para a carteira";
                case "24":
                    return "24-Data de emiss�o inv�lida";
                case "38":
                    return "38-Prazo para protesto inv�lido";
                case "44":
                    return "44-Ag�ncia Cedente n�o prevista";
                case "50":
                    return "50-CEP irregular - Banco Correspondente";
                case "63":
                    return "63-Entrada para T�tulo j� cadastrado";
                case "68":
                    return "68-D�bito n�o agendado - erro nos dados de remessa";
                case "69":
                    return "69-D�bito n�o agendado - Sacado n�o consta no cadastro de autorizante";
                case "70":
                    return "70-D�bito n�o agendado - Cedente n�o autorizado pelo Sacado";
                case "71":
                    return "71-D�bito n�o agendado - Cedente n�o participa da modalidade de d�bito autom�tico";
                case "72":
                    return "72-D�bito n�o agendado - C�digo de moeda diferente de R$";
                case "73":
                    return "73-D�bito n�o agendado - Data de vencimento inv�lida";
                case "74":
                    return "74-D�bito n�o agendado - Conforme seu pedido, T�tulo n�o registrado";
                case "75":
                    return "75-D�bito n�o agendado - Tipo de n�mero de inscri��o do debitado inv�lido";
                default:
                    return "";
            }
        }

        private string Mod11Bradesco(string seq, int b)
        {
            #region Trecho do manual layout_cobranca_port.pdf do BRADESCO
            /* 
            Para o c�lculo do d�gito, ser� necess�rio acrescentar o n�mero da carteira � esquerda antes do Nosso N�mero, 
            e aplicar o m�dulo 11, com base 7.
            Multiplicar cada algarismo que comp�e o n�mero pelo seu respectivo multiplicador (PESO).
            Os multiplicadores(PESOS) variam de 2 a 7.
            O primeiro d�gito da direita para a esquerda dever� ser multiplicado por 2, o segundo por 3 e assim sucessivamente.
             
              Carteira   Nosso Numero
                ______   _________________________________________
                1    9   0   0   0   0   0   0   0   0   0   0   2
                x    x   x   x   x   x   x   x   x   x   x   x   x
                2    7   6   5   4   3   2   7   6   5   4   3   2
                =    =   =   =   =   =   =   =   =   =   =   =   =
                2 + 63 + 0 + 0 + 0 + 0 + 0 + 0 + 0 + 0 + 0 + 0 + 4 = 69

            O total da soma dever� ser dividido por 11: 69 / 11 = 6 tendo como resto = 3
            A diferen�a entre o divisor e o resto, ser� o d�gito de autoconfer�ncia: 11 - 3 = 8 (d�gito de auto-confer�ncia)
            
            Se o resto da divis�o for �1�, desprezar o c�lculo de subtra��o e considerar o d�gito como �P�. 
            Se o resto da divis�o for �0�, desprezar o c�lculo de subtra��o e considerar o d�gito como �0�.
            */
            #endregion

            /* Vari�veis
             * -------------
             * s - Soma
             * p - Peso
             * b - Base
             * r - Resto
             */

            int s = 0, p = 2;

            for (int i = seq.Length; i > 0; i--)
            {
                s = s + (Convert.ToInt32(Microsoft.VisualBasic.Strings.Mid(seq, i, 1)) * p);
                if (p == b)
                    p = 2;
                else
                    p = p + 1;
            }

            int r = (s % 11);

            if (r == 0)
                return "0";
            else if (r == 1)
                return "P";
            else
                return (11 - r).ToString();
        }

        public override bool ValidarRemessa(TipoArquivo tipoArquivo, string numeroConvenio, IBanco banco, Cedente cedente, Boletos boletos, int numeroArquivoRemessa, out string mensagem)
        {
            bool vRetorno = true;
            string vMsg = string.Empty;
            ////IMPLEMENTACAO PENDENTE...
            mensagem = vMsg;
            return vRetorno;
        }

        public string CalcularDigitoNossoNumero(Boleto boleto)
        {
            return Mod11Bradesco(boleto.Carteira + boleto.NossoNumero, 7);
        }

        #endregion

        #region M�todo de gera��o de arquivo de remessa CNAB400

        #region HEADER
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
                        _header = GerarHeaderRemessaCNAB400(numeroConvenio, cedente, numeroArquivoRemessa);
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

        public string GerarHeaderRemessaCNAB400(string numeroConvenio, Cedente cedente, int numeroArquivoRemessa)
        {
            try
            {
                string complemento = new string(' ', 277);
                string _header;

                _header = "01REMESSA01COBRANCA       ";
                _header += Utils.FitStringLength(cedente.Convenio.ToString(), 20, 20, '0', 0, true, true, true);
                _header += Utils.FitStringLength(cedente.Nome, 30, 30, ' ', 0, true, true, false).ToUpper();
                _header += "237";
                _header += "BRADESCO       ";
                _header += DateTime.Now.ToString("ddMMyy");
                _header += "        ";
                _header += "MX";
                _header += Utils.FitStringLength(numeroArquivoRemessa.ToString(), 7, 7, '0', 0, true, true, true);
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
                        _detalhe = GerarDetalheRemessaCNAB240();
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

        private string GerarDetalheRemessaCNAB240()
        {
            throw new NotImplementedException();
        }

        public override string GerarDetalheRemessaTipo2(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
        {
            try
            {
                string _detalhe = "";

                base.GerarDetalheRemessa(boleto, numeroRegistro, tipoArquivo);

                switch (tipoArquivo)
                {
                    case TipoArquivo.CNAB400:
                        _detalhe += GerarDetalheRemessaCNAB400Tipo2(boleto, numeroRegistro, tipoArquivo);
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
                string identificaOperacaoBanco = new string(' ', 10);
                string nrDeControle = new string(' ', 25);
                string mensagem = new string(' ', 12);
                string mensagem2 = new string(' ', 60);

                string usoBanco = new string(' ', 10);
                string _detalhe;
                //detalhe                           (tamanho,tipo) A= Alfanumerico, N= Numerico
                _detalhe = "1"; //Identifica��o do Registro         (1, N)

                //Parte N�o Necessaria - Parte de dados do Sacado
                _detalhe += "00000"; //Agencia de Debito            (5, N) N�o Usado
                _detalhe += " "; //Dig da Agencia                   (1, A) N�o Usado
                _detalhe += "00000"; //Razao da Conta Corrente      (5, N) N�o Usado
                _detalhe += "0000000"; //Conta Corrente             (7, N) N�o Usado
                _detalhe += " "; //Dig da Conta Corrente            (1, A) N�o Usado

                //Identifica��o da Empresa Cedente no Banco (17, A)
                _detalhe += "0";
                _detalhe += Utils.FitStringLength(boleto.Carteira, 3, 3, '0', 0, true, true, true); // Codigo da carteira (3)
                _detalhe += Utils.FitStringLength(boleto.Cedente.ContaBancaria.Agencia, 5, 5, '0', 0, true, true, true); //N da agencia(5)
                _detalhe += Utils.FitStringLength(boleto.Cedente.ContaBancaria.Conta, 7, 7, '0', 0, true, true, true); //Conta Corrente(7)
                _detalhe += Utils.FitStringLength(boleto.Cedente.ContaBancaria.DigitoConta, 1, 1, '0', 0, true, true, true);//D da conta(1)
                //N� de Controle do Participante - uso livre da empresa (25, A)  //  brancos
                _detalhe += nrDeControle;
                //C�digo do Banco, s� deve ser preenchido quando cliente cedente optar por "D�bito Autom�tico".
                _detalhe += "000";
                //0=sem multa, 2=com multa (1, N)
                if (boleto.PercMulta > 0)
                {
                    _detalhe += "2";
                    _detalhe += Utils.FitStringLength(boleto.PercMulta.ToString("0.00").Replace(",", ""), 4, 4, '0', 0, true, true, true); //Percentual Multa 9(2)V99 - (04)
                }
                else
                {
                    _detalhe += "0";
                    _detalhe += "0000";
                }

                //Identifica��o do T�tulo no Banco (12, A)
                _detalhe += Utils.FitStringLength(boleto.NossoNumero, 11, 11, '0', 0, true, true, true); //Nosso Numero (11)

                // For�a o NossoNumero a ter 7 d�gitos. Alterado por Luiz Ponce 07/07/2012
                // Digito de Auto Conferencia do Nosso N�mero (01)
                // Se o banco gerar o boleto, o digito do nosso numero deve vim zerado, sen�o � feito o calculo do d�gito
                if (boleto.BancoGeraBoleto)
                    _detalhe += "0";
                else
                    _detalhe += Mod11Bradesco(boleto.Carteira + Utils.FitStringLength(boleto.NossoNumero, 11, 11, '0', 0, true, true, true), 7); // Digito de Auto Conferencia do Nosso N�mero (01)
                //Desconto Bonifica��o por dia (10, N)
                _detalhe += "0000000000";

                // 1 = Banco emite e Processa o registro
                // 2 = Cliente emite e o Banco somente processa
                //Condi��o para Emiss�o da Papeleta de Cobran�a(1, N)
                if (boleto.BancoGeraBoleto)
                    _detalhe += "1";
                else
                    _detalhe += "2";
                //Ident. se emite papeleta para D�bito Autom�tico (1, A)
                _detalhe += "N";
                //Identifica��o da Opera��o do Banco (10, A) Em Branco
                _detalhe += identificaOperacaoBanco;

                //Indicador de Rateio de Cr�dito (1, A)
                //Somente dever� ser preenchido com a Letra �R�, se a Empresa participa da rotina 
                // de rateio de cr�dito, caso n�o participe, informar Branco.
                _detalhe += " ";

                //Endere�amento para Aviso do D�bito Autom�tico em Conta Corrente (1, N)
                //1 = emite aviso, e assume o endere�o do Sacado constante do Arquivo-Remessa;
                //2 = n�o emite aviso;
                //diferente de 1 ou 2 = emite e assume o endere�o do cliente debitado, constante do nosso cadastro.
                if (boleto.TipoModalidade == "1")
                    _detalhe += " ";
                else
                    _detalhe += "2";

                _detalhe += "  "; //Branco (2, A)

                //Identifica��o ocorr�ncia(2, N)
                /*
                01..Remessa
                02..Pedido de baixa
                04..Concess�o de abatimento
                05..Cancelamento de abatimento concedido
                06..Altera��o de vencimento
                07..Altera��o do controle do participante
                08..Altera��o de seu n�mero
                09..Pedido de protesto
                18..Sustar protesto e baixar T�tulo
                19..Sustar protesto e manter em carteira
                31..Altera��o de outros dados
                35..Desagendamento do d�bito autom�tico
                68..Acerto nos dados do rateio de Cr�dito
                69..Cancelamento do rateio de cr�dito.
                */
                _detalhe += boleto.CodigoOcorrencia; //"01";

                _detalhe += Utils.Right(boleto.NumeroDocumento, 10, '0', true); //N� do Documento (10, A)
                _detalhe += boleto.DataVencimento.ToString("ddMMyy"); //Data do Vencimento do T�tulo (10, N) DDMMAA

                //Valor do T�tulo (13, N)
                _detalhe += Utils.FitStringLength(boleto.ValorBoleto.ToString("0.00").Replace(",", ""), 13, 13, '0', 0, true, true, true);

                _detalhe += "000"; //Banco Encarregado da Cobran�a (3, N)
                _detalhe += "00000"; //Ag�ncia Deposit�ria (5, N)

                /*Esp�cie de T�tulo (2,N)
                * 01-Duplicata
                02-Nota Promiss�ria
                03-Nota de Seguro
                04-Cobran�a Seriada
                05-Recibo
                10-Letras de C�mbio
                11-Nota de D�bito
                12-Duplicata de Serv.
                99-Outros
                */
                //_detalhe += "99";
                _detalhe += Utils.FitStringLength(boleto.EspecieDocumento.Codigo.ToString(), 2, 2, '0', 0, true, true, true);

                _detalhe += "N"; //Identifica��o (1, A) A � aceito; N - n�o aceito
                _detalhe += boleto.DataDocumento.ToString("ddMMyy"); //Data da emiss�o do T�tulo (6, N) DDMMAA

                //Valida se tem instru��o no list de instru��es, repassa ao arquivo de remessa
                string vInstrucao1 = "00"; //1� instru��o (2, N) Caso Queira colocar um cod de uma instru��o. ver no Manual caso nao coloca 00
                string vInstrucao2 = "00"; //2� instru��o (2, N) Caso Queira colocar um cod de uma instru��o. ver no Manual caso nao coloca 00

                //foreach (Instrucao_Bradesco instrucao in boleto.Instrucoes)
                //{
                //    switch ((EnumInstrucoes_Bradesco)instrucao.Codigo)
                //    {
                //        case EnumInstrucoes_Bradesco.Protestar:
                //            vInstrucao1 = "06"; //Indicar o c�digo �06� - (Protesto)
                //            vInstrucao2 = "00";
                //            break;
                //        case EnumInstrucoes_Bradesco.NaoProtestar:
                //            vInstrucao1 = "00";
                //            vInstrucao2 = "00";
                //            break;
                //        case EnumInstrucoes_Bradesco.ProtestoFinsFalimentares:
                //            vInstrucao1 = "06"; //Indicar o c�digo �06� - (Protesto)
                //            vInstrucao2 = "00";
                //            break;
                //        case EnumInstrucoes_Bradesco.ProtestarAposNDiasCorridos:
                //            vInstrucao1 = "06"; //Indicar o c�digo �06� - (Protesto)
                //            vInstrucao2 = Utils.FitStringLength(instrucao.QuantidadeDias.ToString(), 2, 2, '0', 0, true, true, true);
                //            break;
                //        case EnumInstrucoes_Bradesco.ProtestarAposNDiasUteis:
                //            vInstrucao1 = "06"; //Indicar o c�digo �06� - (Protesto)
                //            vInstrucao2 = Utils.FitStringLength(instrucao.QuantidadeDias.ToString(), 2, 2, '0', 0, true, true, true);
                //            break;
                //        case EnumInstrucoes_Bradesco.NaoReceberAposNDias:
                //            vInstrucao1 = "00";
                //            vInstrucao2 = "00";
                //            break;
                //        case EnumInstrucoes_Bradesco.DevolverAposNDias:
                //            vInstrucao1 = "00";
                //            vInstrucao2 = "00";
                //            break;
                //    }
                //}
                _detalhe += vInstrucao1; //posi��es: 157 a 158 do leiaute
                _detalhe += vInstrucao2; //posi��es: 159 a 160 do leiaute
                //

                // Valor a ser cobrado por Dia de Atraso (13, N)
                _detalhe += Utils.FitStringLength(boleto.JurosMora.ToString("0.00").Replace(",", ""), 13, 13, '0', 0, true, true, true);

                //Data Limite P/Concess�o de Desconto (06, N)
                //if (boleto.DataDesconto.ToString("dd/MM/yyyy") == "01/01/0001")
                if (boleto.DataDesconto == DateTime.MinValue) // diegomodolo (diego.ribeiro@nectarnet.com.br)
                {
                    _detalhe += "000000"; //Caso nao tenha data de vencimento
                }
                else
                {
                    _detalhe += boleto.DataDesconto.ToString("ddMMyy");
                }

                //Valor do Desconto (13, N)
                _detalhe += Utils.FitStringLength(boleto.ValorDesconto.ToString("0.00").Replace(",", ""), 13, 13, '0', 0, true, true, true);

                //Valor do IOF (13, N)
                _detalhe += Utils.FitStringLength(boleto.IOF.ToString("0.00").Replace(",", ""), 13, 13, '0', 0, true, true, true);

                //Valor do Abatimento a ser concedido ou cancelado (13, N)
                _detalhe += Utils.FitStringLength(boleto.Abatimento.ToString("0.00").Replace(",", ""), 13, 13, '0', 0, true, true, true);

                /*Identifica��o do Tipo de Inscri��o do Sacado (02, N)
                01-CPF
                02-CNPJ
                03-PIS/PASEP
                98-N�o tem
                99-Outros 
                00-Outros 
                */
                if (boleto.Sacado.CPFCNPJ.Length <= 11)
                    _detalhe += "01";  // CPF
                else
                    _detalhe += "02"; // CNPJ

                //N� Inscri��o do Sacado (14, N)
                string cpf_Cnpj = boleto.Sacado.CPFCNPJ.Replace("/", "").Replace(".", "").Replace("-", "");
                _detalhe += Utils.FitStringLength(cpf_Cnpj, 14, 14, '0', 0, true, true, true);

                //Nome do Sacado (40, A)
                _detalhe += Utils.FitStringLength(boleto.Sacado.Nome.TrimStart(' '), 40, 40, ' ', 0, true, true, false).ToUpper();

                //Endere�o Completo (40, A)
                _detalhe += Utils.FitStringLength(boleto.Sacado.Endereco.End.TrimStart(' '), 40, 40, ' ', 0, true, true, false).ToUpper();

                //1� Mensagem (12, A)
                /*Campo livre para uso da Empresa. A mensagem enviada nesse campo ser� impressa
                somente no boleto e n�o ser� confirmada no Arquivo Retorno.
                */
                _detalhe += Utils.FitStringLength(mensagem, 12, 12, ' ', 0, true, true, false);

                //CEP (5, N) + Sufixo do CEP (3, N) Total (8, N)
                _detalhe += Utils.FitStringLength(boleto.Sacado.Endereco.CEP.Replace("-", ""), 8, 8, '0', 0, true, true, true);

                //Sacador|Avalista ou 2� Mensagem (60, A)
                _detalhe += Utils.FitStringLength(mensagem2, 60, 60, ' ', 0, true, true, false);

                //N� Seq�encial do Registro (06, N)
                _detalhe += Utils.FitStringLength(numeroRegistro.ToString(), 6, 6, '0', 0, true, true, true);

                _detalhe = Utils.SubstituiCaracteresEspeciais(_detalhe);

                return _detalhe;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar DETALHE do arquivo CNAB400.", ex);
            }
        }

        public string GerarDetalheRemessaCNAB400Tipo2(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
        {
            try
            {
                base.GerarDetalheRemessa(boleto, numeroRegistro, tipoArquivo);

                string _detalhe = "";
                //detalhe                           (tamanho,tipo) A= Alfanumerico, N= Numerico
                _detalhe = "2"; //Identifica��o do Registro         (1, N)

                if (boleto.Sacado.Instrucoes[0].Descricao != string.Empty)
                {
                    _detalhe += Utils.FitStringLength(boleto.Sacado.Instrucoes[0].Descricao, 320, 320, ' ', 0, true, true, false);

                    /*
                    // Mensagem 1 do detalhe da instru��o do sacado (002 a 081) (80, A) (para pular linha, utilizar somente 40 caracteres dessa faixa)
                    _detalhe += Utils.FitStringLength(boleto.Sacado.Instrucoes[0].Descricao.Substring(0, 79), 80, 80, ' ', 0, true, true, false);

                    // Mensagem 2 do detalhe da instru��o do sacado (082 a 161) (80, A) (para pular linha, utilizar somente 40 caracteres dessa faixa)
                    _detalhe += Utils.FitStringLength(boleto.Sacado.Instrucoes[0].Descricao.Substring(80, 159), 80, 80, ' ', 0, true, true, false);

                    // Mensagem 3 do detalhe da instru��o do sacado (162 a 241) (80, A) (para pular linha, utilizar somente 40 caracteres dessa faixa)
                    _detalhe += Utils.FitStringLength(boleto.Sacado.Instrucoes[0].Descricao.Substring(160, 239), 80, 80, ' ', 0, true, true, false);

                    // Mensagem 4 do detalhe da instru��o do sacado (162 a 241) (80, A) (para pular linha, utilizar somente 40 caracteres dessa faixa)
                    _detalhe += Utils.FitStringLength(boleto.Sacado.Instrucoes[0].Descricao.Substring(240, 319), 80, 80, ' ', 0, true, true, false);
                     */
                }
                else
                {
                    _detalhe += Utils.FitStringLength(boleto.Sacado.Instrucoes[0].Descricao, 320, 320, ' ', 0, true, true, false);
                }

                /*
                //Data Limite P/Concess�o de Desconto 2 (06, N)
                //if (boleto.DataDesconto.ToString("dd/MM/yyyy") == "01/01/0001")
                if (boleto.DataDesconto == DateTime.MinValue)
                {
                    _detalhe += "000000"; //Caso nao tenha data de vencimento
                }
                else
                {
                    _detalhe += boleto.DataDesconto.ToString("ddMMyy");
                }

                //Valor do Desconto 2 (13, N)
                _detalhe += Utils.FitStringLength(boleto.ValorDesconto.ToString("0.00").Replace(",", ""), 13, 13, '0', 0, true, true, true);
                                
                //Data Limite P/Concess�o de Desconto 3 (06, N)
                //if (boleto.DataDesconto.ToString("dd/MM/yyyy") == "01/01/0001")
                if (boleto.DataDesconto == DateTime.MinValue)
                {
                    _detalhe += "000000"; //Caso nao tenha data de vencimento
                }
                else
                {
                    _detalhe += boleto.DataDesconto.ToString("ddMMyy");
                }

                //Valor do Desconto 3 (13, N)
                _detalhe += Utils.FitStringLength(boleto.ValorDesconto.ToString("0.00").Replace(",", ""), 13, 13, '0', 0, true, true, true);
                */

                //Reserva (Filler) (45, A)
                _detalhe += Utils.FitStringLength("", 45, 45, ' ', 0, true, true, false);

                //Informa��es do banco
                //Carteira
                _detalhe += Utils.FitStringLength(boleto.Carteira, 3, 3, '0', 0, true, true, true);
                //agencia
                _detalhe += Utils.FitStringLength(boleto.Cedente.ContaBancaria.Agencia, 5, 5, '0', 0, true, true, true);
                //conta corrente
                _detalhe += Utils.FitStringLength(boleto.Cedente.ContaBancaria.Conta, 7, 7, '0', 0, true, true, true);
                //digito c/c
                _detalhe += Utils.FitStringLength(boleto.Cedente.ContaBancaria.DigitoConta, 1, 1, '0', 0, true, true, true);
                //nosso numero (zerado pois o numero e gerado pelo banco)
                _detalhe += Utils.FitStringLength("", 11, 11, '0', 0, true, true, true);
                //DAC nosso numero (zerado pois o DAC e gerado pelo banco)
                _detalhe += "0";
                //N� Sequencial de registro
                _detalhe += Utils.FitStringLength(numeroRegistro.ToString(), 6, 6, '0', 0, true, true, true);

                return _detalhe;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar DETALHE 2 do arquivo CNAB400.", ex);
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

        private string GerarTrailerRemessa240()
        {
            throw new NotImplementedException();
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

        # endregion

        #endregion

        #region M�todos de processamento do arquivo retorno CNAB400
        public override HeaderRetorno LerHeaderRetornoCNAB400(string registro)
        {
            try
            {
                HeaderRetorno header = new HeaderRetorno();
                header.RegistroHeader = registro;
                header.CodigoCedente = Convert.ToInt32(registro.Substring(26, 20));

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

                //Tipo de Inscri��o Empresa
                detalhe.CodigoInscricao = Utils.ToInt32(registro.Substring(1, 2));
                //N� Inscri��o da Empresa
                detalhe.NumeroInscricao = registro.Substring(3, 14);

                //Identifica��o da Empresa Cedente no Banco
                detalhe.Agencia = Utils.ToInt32(registro.Substring(24, 6));
                detalhe.Conta = Utils.ToInt32(registro.Substring(30, 7));
                detalhe.DACConta = Utils.ToInt32(registro.Substring(36, 1));

                //N� Controle do Participante
                detalhe.NumeroControle = registro.Substring(37, 25);
                //Identifica��o do T�tulo no Banco
                detalhe.NossoNumeroComDV = registro.Substring(70, 12);
                detalhe.NossoNumero = registro.Substring(70, 11);//Sem o DV
                detalhe.DACNossoNumero = registro.Substring(81, 1); //DV
                //Carteira
                detalhe.Carteira = registro.Substring(107, 1);
                //Identifica��o de Ocorr�ncia
                detalhe.CodigoOcorrencia = Utils.ToInt32(registro.Substring(108, 2));

                switch (detalhe.CodigoOcorrencia)
                {
                    case (int)ECodigoOcorrenciaBradesco400.Entrada_Confirmada:
                        detalhe.Aceito = true;
                        break;
                    case (int)ECodigoOcorrenciaBradesco400.Liquida��o:
                    case (int)ECodigoOcorrenciaBradesco400.Liquida��o_ap�s_baixa_ou_T�tulo_n�o_registrado:
                    case (int)ECodigoOcorrenciaBradesco400.Liquida��o_Em_Cart�rio:
                        detalhe.Baixado = true;
                        detalhe.Aceito = false;
                        break;
                    case (int)ECodigoOcorrenciaBradesco400.Baixa_Via_Ag�ncia:
                    case (int)ECodigoOcorrenciaBradesco400.Baixa:
                        detalhe.Cancelado = true;
                        detalhe.Aceito = false;
                        break;
                    case (int)ECodigoOcorrenciaBradesco400.Rejei��o:
                        detalhe.Aceito = false;
                        break;
                }

                //Descri��o da ocorr�ncia
                detalhe.DescricaoOcorrencia = this.Ocorrencia(registro.Substring(108, 2));

                //N�mero do Documento
                detalhe.NumeroDocumento = registro.Substring(116, 10);
                //Identifica��o do T�tulo no Banco
                detalhe.IdentificacaoTitulo = registro.Substring(126, 20);

                //Valor do T�tulo
                decimal valorTitulo = Convert.ToInt64(registro.Substring(152, 13));
                detalhe.ValorTitulo = valorTitulo / 100;
                //Banco Cobrador
                detalhe.CodigoBanco = Utils.ToInt32(registro.Substring(165, 3));
                //Ag�ncia Cobradora
                detalhe.AgenciaCobradora = Utils.ToInt32(registro.Substring(168, 5));
                //Esp�cie do T�tulo
                detalhe.Especie = Utils.ToInt32(registro.Substring(173, 2));
                //Despesas de cobran�a para os C�digos de Ocorr�ncia (Valor Despesa)
                decimal valorDespesa = Convert.ToUInt64(registro.Substring(175, 13));
                detalhe.ValorDespesa = valorDespesa / 100;
                //Outras despesas Custas de Protesto (Valor Outras Despesas)
                decimal valorOutrasDespesas = Convert.ToUInt64(registro.Substring(188, 13));
                detalhe.ValorOutrasDespesas = valorOutrasDespesas / 100;
                // IOF
                decimal iof = Convert.ToUInt64(registro.Substring(214, 13));
                detalhe.IOF = iof / 100;
                //Abatimento Concedido sobre o T�tulo (Valor Abatimento Concedido)
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
                //Outros Cr�ditos
                decimal outrosCreditos = Convert.ToUInt64(registro.Substring(279, 13));
                detalhe.OutrosCreditos = outrosCreditos / 100;
                //Motivo do C�digo de Ocorr�ncia 19 (Confirma��o de Instru��o de Protesto)
                detalhe.MotivoCodigoOcorrencia = registro.Substring(294, 1);

                //Data Ocorr�ncia no Banco
                int dataOcorrencia = Utils.ToInt32(registro.Substring(110, 6));
                detalhe.DataOcorrencia = Utils.ToDateTime(dataOcorrencia.ToString("##-##-##"));
                //Data Vencimento do T�tulo
                int dataVencimento = Utils.ToInt32(registro.Substring(146, 6));
                detalhe.DataVencimento = Utils.ToDateTime(dataVencimento.ToString("##-##-##"));
                // Data do Cr�dito
                int dataCredito = Utils.ToInt32(registro.Substring(295, 6));
                detalhe.DataCredito = Utils.ToDateTime(dataCredito.ToString("##-##-##"));

                //Origem Pagamento
                detalhe.OrigemPagamento = registro.Substring(301, 3);

                //Motivos das Rejei��es para os C�digos de Ocorr�ncia
                detalhe.MotivosRejeicao = registro.Substring(318, 10);
                //N�mero do Cart�rio
                detalhe.NumeroCartorio = Utils.ToInt32(registro.Substring(365, 2));
                //N�mero do Protocolo
                detalhe.NumeroProtocolo = registro.Substring(365, 2);
                //Nome do Sacado
                detalhe.NomeSacado = "";

                detalhe.NumeroSequencial = Utils.ToInt32(registro.Substring(394, 6));

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
        private string GerarHeaderRemessaCNAB240()
        {
            throw new NotImplementedException();
        }

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

        private string GerarHeaderLoteRemessaCNAB240(Cedente cedente, int numeroArquivoRemessa)
        {
            try
            {
                var dvAgencia = cedente.ContaBancaria.DigitoAgencia == null ? "0" : cedente.ContaBancaria.DigitoAgencia;
                var dvConta = cedente.ContaBancaria.DigitoConta == null ? "0" : cedente.ContaBancaria.DigitoConta;
                var agEConta = cedente.ContaBancaria.Agencia + cedente.ContaBancaria.Conta;

                string header = Utils.FormatCode(Codigo.ToString(), "0", 3, true);                //001-003  3 Codigo do Banco
                header += "0001";                                                                 //004-007  4 Lote do Servi�o HEADER = 0000 | TRAILER = 9999 
                header += "1";                                                                    //008-008  1 Registro HEADER de Lote = 1
                header += "R";                                                                    //009-009  1 Tipo de Opera��o | Remessa = "R", Retorno = "T"
                header += "01";                                                                   //010-011  2 Tipo de Servi�o | Cobran�a = "01", Desconto = "09", Conciliacao Mensal = "11"
                header += "  ";                                                                   //012-013  2 Uso Exclusivo FEBRABAN/CNAB
                header += "042";                                                                  //014-016  2 N� da vers�o do Layout , 042
                header += " ";                                                                    //017-017  1 Uso Exclusivo Febraban/CNAB
                header += (cedente.CPFCNPJ.Length == 11 ? "1" : "2");                             //018-018  1 Tipo de Inscri��o da Empresa CPF = "1", CNPJ = "2"
                header += Utils.FormatCode(cedente.CPFCNPJ, "0", 15, true);                       //019-033 15 N� Inscri��o da Empresa
                header += Utils.FormatCode(cedente.Codigo, "0", 20, true);                        //034-053 20 Codigo do cedente 
                header += Utils.FormatCode(cedente.ContaBancaria.Agencia, "0", 5, true);          //054-058  5 Agencia
                header += Utils.FormatCode(dvAgencia, "0", 1);                                    //059-059  1 DV Ag�ncia do cedente                     
                header += Utils.FormatCode(cedente.ContaBancaria.Conta, "0", 12, true);           //060-071 12 Numero da conta corrente do cedente       
                header += Utils.FormatCode(dvConta, "0", 1);                                      //072-072  1 DV Numero da conta corrente do cedente
                header += " ";                                                                    //073-073  1 DV da CC/AG do cedente 
                header += Utils.FitStringLength(cedente.Nome, 30, 30, ' ', 0, true, true, false); //074-103 30 Nome da Empresa                           
                header += Utils.FormatCode("", " ", 40);                                          //104-143 40 Mensagem 1
                header += Utils.FormatCode("", " ", 40);                                          //144-183 40 Mensagem 2
                header += Utils.FormatCode(numeroArquivoRemessa.ToString(), "0", 8);              //184-191  8 N�mero de Remessa
                header += DateTime.Now.ToString("ddMMyyyy");                                      //192-199  6 Data de grava��o da remessa  DDMMAAAA 
                header += Utils.FormatCode("", " ", 8);                                           //200-207  8 Zeros
                header += Utils.FormatCode("", " ", 33);                                          //200-240 41 Brancos

                header = Utils.SubstituiCaracteresEspeciais(header);
                return header;
            }
            catch (Exception e)
            {
                throw new Exception("Erro ao gerar HEADER DO LOTE do arquivo de remessa.", e);
            }
        }

        public string GerarHeaderRemessaCNAB240(Cedente cedente, int numeroArquivoRemessa)
        {
            try
            {
                var dvAgencia = cedente.ContaBancaria.DigitoAgencia == null ? "0" : cedente.ContaBancaria.DigitoAgencia;
                var dvConta = cedente.ContaBancaria.DigitoConta == null ? "0" : cedente.ContaBancaria.DigitoConta;
                var agEConta = cedente.ContaBancaria.Agencia + cedente.ContaBancaria.Conta;

                string header = Utils.FormatCode(Codigo.ToString(), "0", 3, true);              //Codigo do banco                           001-003
                header += "0000";                                                               //Lote de servi�o                           004-007
                header += "0";                                                                  //Tipo de registro                          008-008
                header += Utils.FormatCode("", " ", 9);                                         //Resevado (uso Banco)                      009-017
                header += (cedente.CPFCNPJ.Length == 11 ? "1" : "2");                           //Tipo inscri��o empresa                    018-018
                header += Utils.FormatCode(cedente.CPFCNPJ, "0", 14);                           //N� inscri��o empresa                      019-032
                header += Utils.FormatCode(cedente.Codigo, "0", 20, true);                      //Codigo de cobran�a conforme Nota 7        033-052 
                header += Utils.FormatCode(cedente.ContaBancaria.Agencia, "0", 5, true);        //Ag�ncia do cedente                        053-057
                header += Utils.FormatCode(dvAgencia, "0", 1);                                  //DV Ag�ncia do cedente                     058-058
                header += Utils.FormatCode(cedente.ContaBancaria.Conta, "0", 12, true);         //Numero da conta corrente do cedente       059-070
                header += Utils.FormatCode(dvConta, "0", 1);                                    //DV Numero da conta corrente do cedente    071-071
                header += " ";                                                                  //DV da CC/AG do cedente (Branco)           072-072
                header += Utils.FitStringLength(cedente.Nome,30,30,' ', 0, true, true, false);  //Nome do cedente                           073-102
                header += Utils.FormatCode("BRADESCO", " ", 30);                                //Nome do Banco                             103-132
                header += Utils.FormatCode("", " ", 10);                                        //Resevado (uso Banco)                      133-142
                header += "1";                                                                  //C�digo remessa = 1                        143-143
                header += DateTime.Now.ToString("ddMMyyyy");                                    //Data gera��o do arquivo DDMMAAAA          144-151
                header += DateTime.Now.ToString("HHmmss");                                      //Hora gera��o do arquivo DDMMAAAA          152-157
                header += Utils.FormatCode(numeroArquivoRemessa.ToString(), "0", 6);            //N� sequencial do arquivo                  158-163
                header += "084";                                                                //N� da vers�o do layout                    164-166
                header += "06250";                                                              //Densidade                                 167-171
                header += Utils.FormatCode("", " ", 20);                                        //Reservado (Uso Banco)                     172-191
                header += Utils.FormatCode("", " ", 20);                                        //Reservado (Uso Empresa)                   192-211
                header += Utils.FormatCode("", " ", 29);                                        //Reservado (Uso FEBABRAN/CNAB)             212-240
                return header;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar HEADER do arquivo de remessa do CNAB240.", ex);
            }
        }

        #endregion

        #region DETALHE

        public override string GerarDetalheSegmentoPRemessa(Boleto boleto, int numeroRegistro, string codigoTransmissao)
        {
            try
            {
                var dvAgencia = boleto.Cedente.ContaBancaria.DigitoAgencia == null ? "0" : boleto.Cedente.ContaBancaria.DigitoAgencia;
                string _segmentoP;

                _segmentoP = Utils.FormatCode(Codigo.ToString(), "0", 3, true);                                                                //001-003 
                _segmentoP += "0001";                                                                                                          //004-007 
                _segmentoP += "3";                                                                                                             //008-008
                _segmentoP += Utils.FitStringLength(numeroRegistro.ToString(), 5, 5, '0', 0, true, true, true);                                //009-013
                _segmentoP += "P";                                                                                                             //014-014 
                _segmentoP += " ";                                                                                                             //015-015 
                _segmentoP += Utils.FitStringLength(boleto.CodigoOcorrencia, 2, 2, '0', 0, true, true, true);                                  //016-017
                _segmentoP += Utils.FitStringLength(boleto.Cedente.ContaBancaria.Agencia, 5, 5, '0', 0, true, true, true);                     //018-022
                _segmentoP += Utils.FitStringLength(dvAgencia, 1, 1, '0', 0, true, true, true);                                                //023-023
                _segmentoP += Utils.FitStringLength(boleto.Cedente.ContaBancaria.Conta, 12, 12, '0', 0, true, true, true);                     //024-035
                _segmentoP += Utils.FitStringLength(boleto.Cedente.ContaBancaria.DigitoConta, 1, 1, '0', 0, true, true, true);                 //036-036
                _segmentoP += " ";                                                                                                             //037-037

                #region Carteiras (C006)
                //C�digo da carteira
                //1 - Cobran�a Simples
                //2 - Cobran�a Vinculada
                //3 - Cobran�a Caucionada
                //4 - Cobran�a Descontada
                string carteira;
                var carteiraAR = "";
                switch (boleto.Carteira)
                {
                    case "09": //Carteira 09 (Com Registro)
                        carteira = "009";
                        carteiraAR = "1";
                        break;
                    case "19": //Carteira 19 (Com Registro)
                        carteira = "001";
                        carteiraAR = "1";
                        break;
                    default:
                        throw new Exception("Carteira n�o implementada para emiss�o de remessa");
                }
                #endregion

                #region Identifica��o Titulo (G069)
                _dacNossoNumero = CalcularDigitoNossoNumero(boleto);
                boleto.DigitoNossoNumero = _dacNossoNumero;

                //Identifica��o do titulo consta Codigo da Carteira + Zeros + Nosso Numero + DV Nosso Numero
                var identificacaoTitulo = carteira + "00000" + Utils.FitStringLength(boleto.NossoNumero + boleto.DigitoNossoNumero, 12, 12, ' ', 0, true, true, true);
                #endregion

                #region Emiss�o e Distribui��o do Boleto
                var emissaoBoleto = "2"; // padr�o Cliente Emite
                var distribuicaoBoleto = "2"; // padr�o Cliente Distribui

                if (boleto.BancoGeraBoleto)
                {
                    emissaoBoleto = "1";
                    distribuicaoBoleto = "1";
                }
                #endregion

                _segmentoP += identificacaoTitulo;                                                                                             //038-057
                _segmentoP += carteiraAR;                                                                                                      //058-058
                _segmentoP += "1"; //Cobran�a registrada                                                                                       //059-059
                _segmentoP += "1"; //Tipo de documento 1=Tradicional 2=Escritural                                                              //060-060
                _segmentoP += emissaoBoleto; //Identifica��o da emiss�o do boleto 2=Cliente Emite                                              //061-061
                _segmentoP += distribuicaoBoleto; //Distribui��o dos titulos 1=Banco Distribui, 2=ClienteDistribui                             //062-062
                _segmentoP += Utils.FitStringLength(boleto.NumeroDocumento, 15, 15, ' ', 0, true, true, false);                                //063-077
                _segmentoP += Utils.FitStringLength(boleto.DataVencimento.ToString("ddMMyyyy"), 8, 8, ' ', 0, true, true, false);              //078-085
                _segmentoP += Utils.FitStringLength(boleto.ValorBoleto.ToString("0.00").Replace(",", ""), 15, 15, '0', 0, true, true, true);   //086-100
                _segmentoP += "00000 ";                                                                                                        //101-106
                _segmentoP += Utils.FitStringLength(boleto.EspecieDocumento.Codigo.ToString(), 2, 2, '0', 0, true, true, true);                //107-108
                _segmentoP += "N";                                                                                                             //109-109
                _segmentoP += Utils.FitStringLength(boleto.DataDocumento.ToString("ddMMyyyy"), 8, 8, ' ', 0, true, true, false);               //110-117
                #region Juros e Desconto
                if (boleto.PercJurosMora > 0)
                {
                    _segmentoP += "1";                                                                                                             //118-118
                    _segmentoP += Utils.FitStringLength(boleto.DataVencimento.ToString("ddMMyyyy"), 8, 8, '0', 0, true, true, false);              //119-126
                    _segmentoP += Utils.FitStringLength(boleto.PercJurosMora.ToString("0.00").Replace(",", ""), 15, 15, '0', 0, true, true, true); //127-141
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
                    _segmentoP += "1";                                                                                                             //142-142
                    _segmentoP += Utils.FitStringLength(boleto.DataVencimento.ToString("ddMMyyyy"), 8, 8, '0', 0, true, true, false);              //143-150
                    _segmentoP += Utils.FitStringLength(boleto.ValorDesconto.ToString("0.00").Replace(",", ""), 15, 15, '0', 0, true, true, true); //151-165
                }
                else
                {
                    _segmentoP += "0";                                                                                                       
                    _segmentoP += Utils.FormatCode("", "0", 8);                                                                             
                    _segmentoP += Utils.FormatCode("", "0", 15);                                                                            
                }
                #endregion
                _segmentoP += Utils.FormatCode(boleto.IOF.ToString().Replace(",", "").Replace(".", ""), "0", 15);                           //166-180
                _segmentoP += Utils.FormatCode(boleto.Abatimento.ToString().Replace(",", "").Replace(".", ""), "0", 15);                    //181-195                                                                               
                _segmentoP += Utils.FitStringLength(boleto.NumeroDocumento, 25, 25, ' ', 0, true, true, false);                             //196-220

                string codigo_protesto = "3";
                string dias_protesto = "00";

                foreach (Instrucao instrucao in boleto.Instrucoes)
                {
                    switch ((EnumInstrucoes_Bradesco)instrucao.Codigo)
                    {
                        case EnumInstrucoes_Bradesco.Protestar:
                            codigo_protesto = "1";
                            dias_protesto = Utils.FitStringLength(instrucao.QuantidadeDias.ToString(), 2, 2, '0', 0, true, true, true);
                            break;
                        case EnumInstrucoes_Bradesco.NaoProtestar:
                            codigo_protesto = "3";
                            dias_protesto = "00";
                            break;
                        default:
                            break;
                    }
                }

                _segmentoP += codigo_protesto;                                                                                              //221-221
                _segmentoP += dias_protesto;                                                                                                //222-223

                _segmentoP += "0000090000000000 ";                                                                                          //224-240

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
                _segmentoR += "00013";
                _segmentoR += Utils.FitStringLength(numeroRegistro.ToString(), 5, 5, '0', 0, true, true, true);
                _segmentoR += "R ";
                _segmentoR += Utils.FitStringLength(boleto.CodigoOcorrencia, 2, 2, '0', 0, true, true, true);
                // Desconto 2
                _segmentoR += "000000000000000000000000"; //24 zeros
                // Desconto 3
                _segmentoR += "000000000000000000000000"; //24 zeros
                #region Multa
                if (boleto.ValorMulta > 0)
                {
                    _segmentoR += "1";                                                                      // C�digo da Multa '1' = Valor Fixo,'2' = Percentual,'0' = Sem Multa 
                    _segmentoR += boleto.DataVencimento.ToString("ddMMyyyy");                               // Data da Multa 
                    _segmentoR += Utils.FormatCode(boleto.ValorMulta.ToString().Replace(",", "").Replace(".", ""), "0", 15, true); // Valor/Percentual a Ser Aplicado
                }
                else if (boleto.PercMulta > 0)
                {
                    _segmentoR += "2";                                                                      // C�digo da Multa '1' = Valor Fixo,'2' = Percentual,'0' = Sem Multa 
                    _segmentoR += boleto.DataVencimento.ToString("ddMMyyyy");                               // Data da Multa 
                    _segmentoR += Utils.FormatCode(boleto.PercMulta.ToString().Replace(",", "").Replace(".", ""), "0", 15, true); // Valor/Percentual a Ser Aplicado
                }
                else
                {
                    _segmentoR += "0";                                                                      // C�digo da Multa '1' = Valor Fixo,'2' = Percentual,'0' = Sem Multa 
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
                _segmentoS += Utils.FormatCode(numeroRegistro.ToString(), "0", 5);
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
        public override string GerarTrailerLoteRemessa(int numeroRegistro)
        {
            try
            {
                string trailer = Utils.FormatCode(Codigo.ToString(), "0", 3, true);
                trailer += Utils.FormatCode("1", "0", 4, true);
                trailer += "5";
                trailer += Utils.FormatCode("", " ", 9);
                trailer += Utils.FitStringLength(numeroRegistro.ToString(), 6, 6, '0', 0, true, true, true);
                trailer += Utils.FormatCode("", "0", 100);
                trailer += Utils.FormatCode("", " ", 117);
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
                trailer += Utils.FormatCode("1", "0", 6, true);
                trailer += Utils.FitStringLength(numeroRegistro.ToString(), 6, 6, '0', 0, true, true, true);
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

        #region M�todos de processamento do arquivo retorno CNAB240
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
                segmentoT.ValorTarifas = Convert.ToDecimal(registro.Substring(198, 15)) / 100;
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
                    case (int)EnumCodigoMovimento_HSBC.Liquida��o:
                    case (int)EnumCodigoMovimento_HSBC.Liquida��o_ap�s_baixa_ou_liquida��o_t�tulo_n�o_registrado:
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
            string _dadosTituloAcrescimo = registro.Substring(17, 15); //08
            string _dadosTituloValorDesconto = registro.Substring(32, 15); //09
            string _dadosTituloValorAbatimento = registro.Substring(47, 15); //10
            string _dadosTituloValorIof = registro.Substring(62, 15); //11
            string _dadosTituloValorPago = registro.Substring(77, 15); //12
            string _dadosTituloValorCreditoBruto = registro.Substring(92, 15); //13
            string _outDespesas = registro.Substring(107, 15); //14
            string _outPerCredEntRecb = registro.Substring(122, 15); //15
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
                segmentoU.Servico_Codigo_Movimento_Retorno = Convert.ToDecimal(_Servico_codMov); //07.3U|Servi�o|C�d. Mov.|C�digo de Movimento Retorno
                segmentoU.JurosMultaEncargos = Convert.ToDecimal(_dadosTituloAcrescimo) / 100;
                segmentoU.ValorDescontoConcedido = Convert.ToDecimal(_dadosTituloValorDesconto) / 100;
                segmentoU.ValorAbatimentoConcedido = Convert.ToDecimal(_dadosTituloValorAbatimento) / 100;
                segmentoU.ValorIOFRecolhido = Convert.ToDecimal(_dadosTituloValorIof) / 100;
                segmentoU.ValorOcorrenciaSacado = segmentoU.ValorPagoPeloSacado = Convert.ToDecimal(_dadosTituloValorPago) / 100;
                segmentoU.ValorLiquidoASerCreditado = Convert.ToDecimal(_dadosTituloValorCreditoBruto) / 100;
                segmentoU.ValorOutrasDespesas = Convert.ToDecimal(_outDespesas) / 100;
                segmentoU.ValorOutrosCreditos = Convert.ToDecimal(_outPerCredEntRecb) / 100;
                segmentoU.DataOcorrencia = segmentoU.DataOcorrencia = DateTime.ParseExact(_outDataOcorrencia, "ddMMyyyy", CultureInfo.InvariantCulture);
                segmentoU.DataCredito = _outDataCredito.ToString() == "00000000" ? segmentoU.DataOcorrencia : DateTime.ParseExact(_outDataCredito, "ddMMyyyy", CultureInfo.InvariantCulture);

                return segmentoU;
            }
            catch (Exception ex)
            {
                //TrataErros.Tratar(ex);
                throw new Exception("Erro ao processar arquivo de RETORNO - SEGMENTO U.", ex);
            }
        }
        #endregion

    }
}
