using System;
using System.Globalization;
using System.Web.UI;
using BoletoNet;
using Microsoft.VisualBasic;
using BoletoNet.EDI.Banco;

[assembly: WebResource("BoletoNet.Imagens.104.jpg", "image/jpg")]

namespace BoletoNet
{
    /// <summary>
    /// Classe referente ao banco Banco_Caixa Economica Federal
    /// </summary>
    internal class Banco_Caixa : AbstractBanco, IBanco
    {
        private const int EMISSAO_CEDENTE = 4;

        string _dacBoleto = String.Empty;

        private bool protestar = false;
        private bool baixaDevolver = false;
        private bool desconto = false;
        private int diasProtesto = 0;
        private int diasDevolucao = 0;
        private int diasDesconto = 0;
        private string tipoArquivoRetorno = "";

        internal Banco_Caixa()
        {
            this.Codigo = 104;
            this.Digito = "0";
            this.Nome = "Caixa Econômica Federal";
        }

        #region Formatações e Validações Boleto
        public override void FormataCodigoBarra(Boleto boleto)
        {
            // Posição 01-03
            string banco = Codigo.ToString();

            //Posição 04
            string moeda = "9";

            //Posição 05 - No final ...   

            // Posição 06 - 09
            long fatorVencimento = FatorVencimento(boleto);

            // Posição 10 - 19    
            var valorDocumento = "";

            if (boleto.SegundaVia)
                valorDocumento = Utils.FormatCode(boleto.ValorCobrado.ToString("f").Replace(",", "").Replace(".", ""), 10);//10
            else
                valorDocumento = Utils.FormatCode(boleto.ValorBoleto.ToString("f").Replace(",", "").Replace(".", ""), 10);//10

            // Inicio Campo livre
            string campoLivre = string.Empty;

            if ((boleto.Carteira.Equals("SR") || boleto.Carteira.Equals("RG")) && boleto.NossoNumero.Length == 19)
            {
                //104 - Caixa Econômica Federal S.A. 
                //Carteira SR - 24 (cobrança sem registro) || Carteira RG - 14 (cobrança com registro)
                //Cobrança sem registro, nosso número com 19 dígitos. 

                //Posição 20 - 25
                string codigoCedente = Utils.FormatCode(boleto.Cedente.Codigo.ToString(), 6).Replace(".", "");

                //Posição 26
                string dvCodigoCedente = Mod11Base9(codigoCedente).ToString();

                //Posição 27 - 29
                string primeiraParteNossoNumero = boleto.NossoNumero.Substring(3, 3);

                //Posição 30
                string primeiraConstante;

                switch (boleto.Carteira)
                {
                    case "SR":
                        primeiraConstante = "2";
                        break;
                    case "RG":
                        primeiraConstante = "1";
                        break;
                    default:
                        primeiraConstante = "2";
                        break;
                }

                // Posição 31 - 33
                //DE acordo com documentação, posição 6 a 8 do nosso numero
                string segundaParteNossoNumero = boleto.NossoNumero.Substring(3, 3);

                // Posição 34
                string segundaConstante = EMISSAO_CEDENTE.ToString();// 4 => emissão do boleto pelo cedente

                //Posição 35 - 43
                //De acordo com documentaçao, posição 9 a 17 do nosso numero
                string terceiraParteNossoNumero = boleto.NossoNumero.Substring(8, 9);

                //Posição 44
                string ccc = string.Format("{0}{1}{2}{3}{4}{5}{6}",
                                           codigoCedente,
                                           dvCodigoCedente,
                                           primeiraParteNossoNumero,
                                           primeiraConstante,
                                           segundaParteNossoNumero,
                                           segundaConstante,
                                           terceiraParteNossoNumero);
                string dvCampoLivre = Mod11Base9(ccc).ToString();
                campoLivre = string.Format("{0}{1}", ccc, dvCampoLivre);
            }
            else
            {
                string codigoCedente = Utils.FormatCode(boleto.Cedente.ContaBancaria.Agencia + boleto.Cedente.ContaBancaria.OperacaoConta + boleto.Cedente.Codigo.ToString(), 15);

                string ccc = string.Format("{0}{1}",
                                          boleto.NossoNumero.Substring(0, boleto.NossoNumero.Length - 2),
                                          codigoCedente
                                          );
                campoLivre = ccc;
            }

            string xxxx = string.Format("{0}{1}{2}{3}{4}", banco, moeda, fatorVencimento, valorDocumento, campoLivre);

            string dvGeral = Mod11Peso2a9(xxxx).ToString();
            // Posição 5
            _dacBoleto = dvGeral;

            boleto.CodigoBarra.Codigo = string.Format("{0}{1}{2}{3}{4}{5}",
                   banco,
                   moeda,
                   dvGeral,
                   fatorVencimento,
                   valorDocumento,
                   campoLivre
                   );
        }

        public override void FormataLinhaDigitavel(Boleto boleto)
        {
            string Grupo1 = string.Empty;
            string Grupo2 = string.Empty;
            string Grupo3 = string.Empty;
            string Grupo4 = string.Empty;
            string Grupo5 = string.Empty;

            string str1 = string.Empty;
            string str2 = string.Empty;
            string str3 = string.Empty;

            if ((boleto.Carteira.Equals("SR") || boleto.Carteira.Equals("RG")) && boleto.NossoNumero.Length == 15)
            {
                #region Campo 1

                //POSIÇÃO 1 A 4 DO CODIGO DE BARRAS
                str1 = boleto.CodigoBarra.Codigo.Substring(0, 4);
                //POSICAO 20 A 24 DO CODIGO DE BARRAS
                str2 = boleto.CodigoBarra.Codigo.Substring(19, 5);
                //CALCULO DO DIGITO
                str3 = Mod10(str1 + str2).ToString();

                Grupo1 = str1 + str2 + str3;
                Grupo1 = Grupo1.Substring(0, 5) + "." + Grupo1.Substring(5) + " ";

                #endregion Campo 1

                #region Campo 2

                //POSIÇÃO 25 A 34 DO COD DE BARRAS
                str1 = boleto.CodigoBarra.Codigo.Substring(24, 10);
                //DIGITO
                str2 = Mod10(str1).ToString();

                Grupo2 = string.Format("{0}.{1}{2} ", str1.Substring(0, 5), str1.Substring(5, 5), str2);

                #endregion Campo 2

                #region Campo 3

                //POSIÇÃO 35 A 44 DO CODIGO DE BARRAS
                str1 = boleto.CodigoBarra.Codigo.Substring(34, 10);
                //DIGITO
                str2 = Mod10(str1).ToString();

                Grupo3 = string.Format("{0}.{1}{2} ", str1.Substring(0, 5), str1.Substring(5, 5), str2);

                #endregion Campo 3

                #region Campo 4

                string D4 = _dacBoleto.ToString();

                Grupo4 = string.Format("{0} ", D4);

                #endregion Campo 4

                #region Campo 5

                long FFFF = FatorVencimento(boleto);

                var VVVVVVVVVV = "";

                if (boleto.SegundaVia)
                    VVVVVVVVVV = Utils.FormatCode(boleto.ValorCobrado.ToString("f").Replace(",", "").Replace(".", ""), 10);//10
                else
                    VVVVVVVVVV = Utils.FormatCode(boleto.ValorBoleto.ToString("f").Replace(",", "").Replace(".", ""), 10);//10

                if (Utils.ToInt64(VVVVVVVVVV) == 0)
                    VVVVVVVVVV = "000";

                Grupo5 = string.Format("{0}{1}", FFFF, VVVVVVVVVV);

                #endregion Campo 5
            }
            else
            {
                #region Campo 1

                string BBB = boleto.CodigoBarra.Codigo.Substring(0, 3);
                string M = boleto.CodigoBarra.Codigo.Substring(3, 1);
                string CCCCC = boleto.CodigoBarra.Codigo.Substring(19, 5);
                string D1 = Mod10(BBB + M + CCCCC).ToString();

                Grupo1 = string.Format("{0}{1}{2}.{3}{4} ",
                    BBB,
                    M,
                    CCCCC.Substring(0, 1),
                    CCCCC.Substring(1, 4), D1);


                #endregion Campo 1

                #region Campo 2

                string CCCCCCCCCC2 = boleto.CodigoBarra.Codigo.Substring(24, 10);
                string D2 = Mod10(CCCCCCCCCC2).ToString();

                Grupo2 = string.Format("{0}.{1}{2} ", CCCCCCCCCC2.Substring(0, 5), CCCCCCCCCC2.Substring(5, 5), D2);

                #endregion Campo 2

                #region Campo 3

                string CCCCCCCCCC3 = boleto.CodigoBarra.Codigo.Substring(34, 10);
                string D3 = Mod10(CCCCCCCCCC3).ToString();

                Grupo3 = string.Format("{0}.{1}{2} ", CCCCCCCCCC3.Substring(0, 5), CCCCCCCCCC3.Substring(5, 5), D3);


                #endregion Campo 3

                #region Campo 4

                string D4 = _dacBoleto.ToString();

                Grupo4 = string.Format(" {0} ", D4);

                #endregion Campo 4

                #region Campo 5

                long FFFF = FatorVencimento(boleto);

                var VVVVVVVVVV = "";

                if (boleto.SegundaVia)
                    VVVVVVVVVV = Utils.FormatCode(boleto.ValorCobrado.ToString("f").Replace(",", "").Replace(".", ""), 10);//10
                else
                    VVVVVVVVVV = Utils.FormatCode(boleto.ValorBoleto.ToString("f").Replace(",", "").Replace(".", ""), 10);//10

                if (Utils.ToInt64(VVVVVVVVVV) == 0)
                    VVVVVVVVVV = "000";

                Grupo5 = string.Format("{0}{1}", FFFF, VVVVVVVVVV);

                #endregion Campo 5
            }

            //MONTA OS DADOS DA INHA DIGITÁVEL DE ACORDO COM OS DADOS OBTIDOS ACIMA
            boleto.CodigoBarra.LinhaDigitavel = Grupo1 + Grupo2 + Grupo3 + Grupo4 + Grupo5;
        }

        public override void FormataNossoNumero(Boleto boleto)
        {
            if (boleto.Carteira.Equals("SR") && boleto.Cedente.Codigo.Length.Equals(6))
                boleto.NossoNumero = "24" + Utils.FormatCode(boleto.NossoNumero, "0", 15, true);
            else if (boleto.Carteira.Equals("RG"))
                boleto.NossoNumero = "14" + Utils.FormatCode(boleto.NossoNumero, "0", 15, true);
            else if (boleto.Carteira.Equals("CR"))
                boleto.NossoNumero = "9" + Utils.FormatCode(boleto.NossoNumero, "0", 9, true);
            else if (boleto.Carteira.Equals("SR"))
                boleto.NossoNumero = "82" + Utils.FormatCode(boleto.NossoNumero, "0", 8, true);

            boleto.NossoNumero = boleto.NossoNumero + "-" + Mod11Peso2a9(boleto.NossoNumero);

            if (boleto.BancoGeraBoleto)
            {
                if (boleto.Carteira.Equals("CS") || boleto.Carteira.Equals("CR"))
                    boleto.NossoNumero = Utils.FormatCode("", "0", 12, true);
                else if (boleto.Carteira.Equals("RG"))
                    Utils.FormatCode("", "0", 17, true);
            }
        }

        public void FormataNossoNumeroRemessa(Boleto boleto)
        {
            if (boleto.Carteira.Equals("SR") && boleto.Cedente.Codigo.Length.Equals(6))
                boleto.NossoNumero = "24" + Utils.FormatCode(boleto.NossoNumero, "0", 15, true);
            else if (boleto.Carteira.Equals("RG"))
                boleto.NossoNumero = "14" + Utils.FormatCode(boleto.NossoNumero, "0", 15, true);
            else if (boleto.Carteira.Equals("CR"))
                boleto.NossoNumero = "9" + Utils.FormatCode(boleto.NossoNumero, "0", 9, true);
            else if (boleto.Carteira.Equals("SR"))
                boleto.NossoNumero = "82" + Utils.FormatCode(boleto.NossoNumero, "0", 8, true);

            boleto.NossoNumero = boleto.NossoNumero;

            if (boleto.BancoGeraBoleto)
            {
                if (boleto.Carteira.Equals("CS") || boleto.Carteira.Equals("CR"))
                    boleto.NossoNumero = Utils.FormatCode("", "0", 12, true);
                else if (boleto.Carteira.Equals("RG"))
                    Utils.FormatCode("", "0", 17, true);
            }
        }

        public override void FormataNumeroDocumento(Boleto boleto)
        {

        }

        public override void ValidaBoleto(Boleto boleto)
        {
            if (boleto.Carteira.Equals("RG") && !boleto.Cedente.Codigo.Length.Equals(6))
                throw new Exception("Código do cedente inválido, para carteira RG, módulo de cobrança SIGCB o código do cedente deve ter 6 caracteres");

            if (boleto.Carteira.Equals("CR") && !boleto.Cedente.Codigo.Length.Equals(5))
                throw new Exception("Código do cedente inválido, para carteira CR, módulo de cobrança SICOB o código do cedente deve ter 5 caracteres");

            if (boleto.Carteira.Equals("SR") && boleto.Cedente.Codigo.Length.Equals(6))
            {
                if (boleto.Cedente.DigitoCedente == -1)
                    boleto.Cedente.DigitoCedente = Convert.ToInt32(boleto.Cedente.ContaBancaria.DigitoConta);
            }
            else if (boleto.Carteira.Equals("SR") && boleto.Cedente.Codigo.Length > 6)
            {
                if (boleto.Cedente.DigitoCedente == -1)
                    boleto.Cedente.DigitoCedente = Convert.ToInt32(boleto.Cedente.ContaBancaria.DigitoConta);
            }
            else if (boleto.Carteira.Equals("RG"))
            {
                if (boleto.Cedente.DigitoCedente == -1)
                    boleto.Cedente.DigitoCedente = Convert.ToInt32(boleto.Cedente.ContaBancaria.DigitoConta);
            }
            else if (boleto.Carteira.Equals("CS"))
            {
                foreach (char ch in boleto.NossoNumero)
                {
                    if (!ch.Equals('0'))
                        throw new Exception("Nosso Número inválido, Para Caixa Econômica - SIGCB carteira simples, o Nosso Número deve estar zerado.");
                }
            }
            else
            {
                if (boleto.NossoNumero.Length != 8)
                {
                    throw new Exception(
                        "Nosso Número inválido, Para Caixa Econômica carteira indefinida, o Nosso Número deve conter 8 caracteres.");
                }


                if (boleto.Cedente.DigitoCedente == -1)
                    boleto.Cedente.DigitoCedente = Convert.ToInt32(boleto.Cedente.Codigo.Substring(boleto.Cedente.Codigo.Length - 1, 1));
            }

            if (boleto.DataDocumento == DateTime.MinValue)
                boleto.DataDocumento = DateTime.Now;

            //Atribui o nome do banco ao local de pagamento
            boleto.LocalPagamento = "Preferencialmente nas casas lotéricas até o valor limite";

            /* 
             * Na Carteira Simples não é necessário gerar a impressão do boleto,
             * logo não é necessário formatar linha digitável nem cód de barras
             * Jéferson (jefhtavares) em 10/03/14
             */

            if (!boleto.Carteira.Equals("CS"))
            {
                FormataNossoNumero(boleto);
                FormataCodigoBarra(boleto);
                FormataLinhaDigitavel(boleto);
            }
        }
        #endregion

        #region Métodos de geração do arquivo remessa

        public override bool ValidarRemessa(TipoArquivo tipoArquivo, string numeroConvenio, IBanco banco, Cedente cedente, Boletos boletos, int numeroArquivoRemessa, out string mensagem)
        {
            bool vRetorno = true;
            string vMsg = string.Empty;
            //            
            switch (tipoArquivo)
            {
                case TipoArquivo.CNAB240:
                    vRetorno = ValidarRemessaCNAB240(numeroConvenio, banco, cedente, boletos, numeroArquivoRemessa, out vMsg);
                    break;
                case TipoArquivo.CNAB400:
                    vRetorno = ValidarRemessaCNAB400(numeroConvenio, banco, cedente, boletos, numeroArquivoRemessa, out vMsg);
                    break;
                case TipoArquivo.Outro:
                    throw new Exception("Tipo de arquivo inexistente.");
            }
            //
            mensagem = vMsg;
            return vRetorno;
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
                throw new Exception("Erro durante a geração do HEADER do arquivo de REMESSA.", ex);
            }
        }

        public override string GerarHeaderRemessa(string numeroConvenio, Cedente cedente, TipoArquivo tipoArquivo, int numeroArquivoRemessa, Boleto boletos)
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
                throw new Exception("Erro durante a geração do HEADER do arquivo de REMESSA.", ex);
            }
        }

        public override string GerarDetalheSegmentoPRemessa(Boleto boleto, int numeroRegistro, string numeroConvenio, Cedente cedente)
        {
            return GerarDetalheSegmentoPRemessaCNAB240(boleto, numeroRegistro, numeroConvenio, cedente);
        }

        public override string GerarDetalheSegmentoQRemessa(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
        {
            return GerarDetalheSegmentoQRemessaCNAB240(boleto, numeroRegistro, tipoArquivo);
        }

        public override string GerarDetalheSegmentoRRemessa(Boleto boleto, int numeroRegistroDetalhe, TipoArquivo CNAB240)
        {
            return GerarDetalheSegmentoRRemessaCNAB240(boleto, numeroRegistroDetalhe, CNAB240);
        }

        public override string GerarDetalheSegmentoSRemessa(Boleto boleto, int numeroRegistroDetalhe, TipoArquivo CNAB240)
        {
            return GerarDetalheSegmentoSRemessaCNAB240(boleto, numeroRegistroDetalhe, CNAB240);
        }

        public override string GerarTrailerLoteRemessa(int numeroRegistro, Boleto boletos)
        {
            return GerarTrailerLoteRemessaCNAB240(numeroRegistro, boletos);
        }

        public override string GerarTrailerArquivoRemessa(int numeroRegistro, Boleto boletos)
        {
            return GerarTrailerArquivoRemessaCNAB240(numeroRegistro);
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
                    case TipoArquivo.CNAB400:
                        //header = GerarHeaderLoteRemessaCNAB400(0, cedente, numeroArquivoRemessa);
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

        public override string GerarHeaderLoteRemessa(string numeroConvenio, Cedente cedente, int numeroArquivoRemessa, TipoArquivo tipoArquivo, Boleto boletos)
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
                        //header = GerarHeaderLoteRemessaCNAB400(0, cedente, numeroArquivoRemessa);
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

        #endregion

        #region Métodos de geração do arquivo remessa CNAB240

        public bool ValidarRemessaCNAB240(string numeroConvenio, IBanco banco, Cedente cedente, Boletos boletos, int numeroArquivoRemessa, out string mensagem)
        {
            bool vRetorno = true;
            string vMsg = string.Empty;
            //
            #region Pré Validações
            if (banco == null)
            {
                vMsg += String.Concat("Remessa: O Banco é Obrigatório!", Environment.NewLine);
                vRetorno = false;
            }
            if (cedente == null)
            {
                vMsg += String.Concat("Remessa: O Cedente/Beneficiário é Obrigatório!", Environment.NewLine);
                vRetorno = false;
            }
            if (boletos == null || boletos.Count.Equals(0))
            {
                vMsg += String.Concat("Remessa: Deverá existir ao menos 1 boleto para geração da remessa!", Environment.NewLine);
                vRetorno = false;
            }
            #endregion
            //
            //validação de cada boleto
            foreach (Boleto boleto in boletos)
            {
                #region Validação de cada boleto
                if (boleto.Remessa == null)
                {
                    vMsg += String.Concat("Boleto: ", boleto.NumeroDocumento, "; Remessa: Informe as diretrizes de remessa!", Environment.NewLine);
                    vRetorno = false;
                }
                else if (boleto.Remessa.TipoDocumento.Equals("1") && String.IsNullOrEmpty(boleto.Sacado.Endereco.CEP)) //1 - SICGB - Com registro
                {
                    //Para o "Remessa.TipoDocumento = "1", o CEP é Obrigatório!
                    vMsg += String.Concat("Para o Tipo Documento [1 - SIGCB - COM REGISTRO], o CEP do SACADO é Obrigatório!", Environment.NewLine);
                    vRetorno = false;
                }
                if (boleto.NossoNumero.Length > 15)
                    boleto.NossoNumero = boleto.NossoNumero.Substring(0, 15);
                //if (!boleto.Remessa.TipoDocumento.Equals("2")) //2 - SIGCB - SEM REGISTRO
                //{
                //    //Para o "Remessa.TipoDocumento = "2", não poderá ter NossoNumero Gerado!
                //    vMsg += String.Concat("Tipo Documento de boleto não Implementado!", Environment.NewLine);
                //    vRetorno = false;
                //}
                #endregion
            }
            //
            mensagem = vMsg;
            return vRetorno;
        }
        private void validaInstrucoes240(Boleto boleto)
        {
            if (boleto.Instrucoes.Count.Equals(0))
                return;

            protestar = false;
            baixaDevolver = false;
            desconto = false;
            diasProtesto = 0;
            diasDevolucao = 0;
            diasDesconto = 0;
            foreach (IInstrucao instrucao in boleto.Instrucoes)
            {
                if (instrucao.Codigo.Equals((int)EnumInstrucoes_Caixa.Protestar) ||
                    instrucao.Codigo.Equals((int)EnumInstrucoes_Caixa.ProtestoFinsFalimentares) ||
                    instrucao.Codigo.Equals((int)EnumInstrucoes_Caixa.ProtestarAposNDiasCorridos) ||
                    instrucao.Codigo.Equals((int)EnumInstrucoes_Caixa.ProtestarAposNDiasUteis))
                {
                    protestar = true;
                    diasProtesto = instrucao.QuantidadeDias;
                }
                else if (instrucao.Codigo.Equals((int)EnumInstrucoes_Caixa.NaoReceberAposNDias) ||
                    instrucao.Codigo.Equals((int)EnumInstrucoes_Caixa.DevolverAposNDias) ||
                    instrucao.Codigo.Equals((int)EnumInstrucoes_Caixa.DevolverApos30Dias))
                {
                    baixaDevolver = true;
                    diasDevolucao = instrucao.QuantidadeDias;
                }
                else if (instrucao.Codigo.Equals((int)EnumInstrucoes_Caixa.DescontoporDia))
                {
                    desconto = true;
                    diasDesconto = instrucao.QuantidadeDias;
                }
            }
        }

        #region HEADER
        public string GerarHeaderRemessaCNAB240(Cedente cedente, int numeroArquivoRemessa)
        {
            try
            {
                var dvAgencia = cedente.ContaBancaria.DigitoAgencia == null ? "0" : cedente.ContaBancaria.DigitoAgencia;

                string header = Utils.FormatCode(Codigo.ToString(), "0", 3, true);                      // código do banco na compensação
                header += "0000";                                                                       // Lote de Serviço 
                header += "0";                                                                          // Tipo de Registro 
                header += Utils.FormatCode("", " ", 9);                                                 // Uso Exclusivo FEBRABAN/CNAB
                header += (cedente.CPFCNPJ.Length == 11 ? "1" : "2");                                   // Tipo de Inscrição 
                header += Utils.FormatCode(cedente.CPFCNPJ, "0", 14);                                   // CPF/CNPJ do cedente 
                #region Tratamento Método de Cobrança Codigo do Cedente SICOB
                //Se a carteira for CR, CS ou DE (SICOB)
                if (cedente.Carteira.Equals("CR") || cedente.Carteira.Equals("CS") || cedente.Carteira.Equals("DE"))
                {
                    header += Utils.FormatCode(cedente.Codigo.ToString(), "0", 16);                     // Código do Convênio no Banco 
                    header += Utils.FormatCode("", "0", 4);                                             // Uso Exclusivo CAIXA
                }
                //Se a carteira for RG (SIGCB)
                else
                    header += Utils.FormatCode("", "0", 20);                                            // Uso Exclusivo CAIXA
                #endregion
                header += Utils.FormatCode(cedente.ContaBancaria.Agencia, "0", 5, true);                // Agência Mantenedora da Conta 
                header += Utils.FormatCode(dvAgencia, "0", 1);                                          // Dígito Verificador da Agência 
                #region Tratamento Método de Cobrança Codigo do Cedente SIGCB
                //Se a carteira for CR, CS ou DE (SICOB)
                if (cedente.Carteira.Equals("CR") || cedente.Carteira.Equals("CS") || cedente.Carteira.Equals("DE"))
                {
                    header += Utils.FormatCode(cedente.ContaBancaria.Conta, "0", 12);                   // Código do Cedente (sem operação)  
                    header += Utils.FormatCode(cedente.ContaBancaria.DigitoConta, "0", 1);              // Díg. Verif. Cedente (sem operação) 
                }
                //Se a carteira for RG (SIGCB)
                else
                {
                    header += Utils.FormatCode(cedente.Codigo.ToString(), "0", 6);                      // Código do Convênio no Banco 
                    header += Utils.FormatCode("", "0", 7);                                             // Uso Exclusivo CAIXA 
                }
                #endregion
                //header += Banco.Mod11(cedente.ContaBancaria.Agencia + cedente.ContaBancaria.Conta).ToString();       // Dígito Verif. Ag./Ced  (sem operação)
                header += "0";                                                                                       // Dígito Verif. Ag./Ced  (sem operação) (informar zero)
                header += Utils.FitStringLength(cedente.Nome, 30, 30, ' ', 0, true, true, false);                    // Nome do cedente
                header += Utils.FitStringLength("CAIXA ECONOMICA FEDERAL", 30, 30, ' ', 0, true, true, false);       // Nome do Banco
                header += Utils.FormatCode("", " ", 10);                                                             // Uso Exclusivo FEBRABAN/CNAB
                header += "1";                                                                                       // Código 1 - Remessa / 2 - Retorno 
                header += DateTime.Now.ToString("ddMMyyyy");                                                         // Data de Geração do Arquivo
                header += DateTime.Now.ToString("HHmmss");                                                           // Hora de Geração do Arquivo
                header += Utils.FormatCode(numeroArquivoRemessa.ToString(), "0", 6, true);                           // Número Seqüencial do Arquivo 
                #region Versão do Layout Método de Cobrança
                if (cedente.Carteira.Equals("CR") || cedente.Carteira.Equals("CS") | cedente.Carteira.Equals("DE"))
                    header += "050";                                                                                 // Número da Versão do Layout do Arquivo 
                else
                    header += "030";
                #endregion
                header += Utils.FormatCode("", "0", 5);                                                 // Densidade de Gravação do Arquivo 
                header += Utils.FormatCode("", " ", 20);                                                // Para Uso Reservado do Banco
                // Na fase de testes (simulado), deverá conter a literal ‘REMESSA-TESTE’ e na fase de produção deverá conter a literal ‘REMESSA-PRODUCAO’
                header += Utils.FormatCode("REMESSA-PRODUCAO", " ", 20);                                // Para Uso Reservado da Empresa  
                header += Utils.FormatCode("", " ", 29);                                                // Uso Exclusivo FEBRABAN/CNAB

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
                var dvAgencia = cedente.ContaBancaria.DigitoAgencia == null ? "0" : cedente.ContaBancaria.DigitoAgencia;

                string header = Utils.FormatCode(Codigo.ToString(), "0", 3, true);                      // código do banco na compensação
                header += "0001";                                                                       // Lote de Serviço
                header += "1";                                                                          // Tipo de Registro 
                header += "R";                                                                          // Tipo de Operação 
                header += "01";                                                                         // Tipo de Serviço '01' = Cobrança, '03' = Bloqueto Eletrônico 
                #region Versão do Layout Método de Cobrança
                if (cedente.Carteira.Equals("CR") || cedente.Carteira.Equals("CS") | cedente.Carteira.Equals("DE"))
                {
                    header += "  ";                                                                         // Uso Exclusivo FEBRABAN/CNAB
                    header += "020";                                                                        // Número da Versão do Layout do Arquivo 
                }
                else
                {
                    header += "00";                                                                         // Uso Exclusivo FEBRABAN/CNAB        
                    header += "030";                                                                        // Número da Versão do Layout do Arquivo 
                }
                #endregion
                header += " ";                                                                          // Uso Exclusivo FEBRABAN/CNAB
                header += (cedente.CPFCNPJ.Length == 11 ? "1" : "2");                                   // Tipo de Inscrição 
                header += Utils.FormatCode(cedente.CPFCNPJ, "0", 15, true);                             // CPF/CNPJ do cedente
                #region Tratamento Código do Cedente Método de Cobrança
                //Se a carteira for CR, CS ou DE (SICOB)
                if (cedente.Carteira.Equals("CR") || cedente.Carteira.Equals("CS") | cedente.Carteira.Equals("DE"))
                {
                    header += Utils.FormatCode(cedente.Codigo.ToString(), "0", 16);                     // Código do Convênio no Banco 
                    header += Utils.FormatCode("", "0", 4);                                             // Uso Exclusivo CAIXA
                }
                //Se a carteira for RG (SIGCB)
                else
                {
                    header += Utils.FormatCode(cedente.Codigo.ToString(), "0", 6);                      // Código do Convênio no Banco
                    header += Utils.FormatCode("", "0", 14);                                            // Uso Exclusivo CAIXA
                }
                #endregion
                header += Utils.FormatCode(cedente.ContaBancaria.Agencia, "0", 5, true);                // Agência Mantenedora da Conta 
                header += Utils.FormatCode(dvAgencia, "0", 1);                                          // Dígito Verificador da Agência 
                #region Tratamento Conta Método de Cobrança
                if (cedente.Carteira.Equals("CR") || cedente.Carteira.Equals("CS") | cedente.Carteira.Equals("DE"))
                {
                    header += Utils.FormatCode(cedente.ContaBancaria.Conta, "0", 12);                         // Número da Conta Corrente 
                    header += Utils.FormatCode(cedente.ContaBancaria.DigitoConta, "0", 1);                    // Dígito da Conta Corrente 
                    //header += Banco.Mod11(cedente.ContaBancaria.Agencia + cedente.ContaBancaria.Conta).ToString();  // Dígito Verif. Ag./Ced  (sem operação)
                    header += "0";                                                                            // Dígito Verif. Ag./Ced  (sem operação) (informar zero)
                }
                else
                {
                    header += Utils.FormatCode(cedente.Codigo, "0", 6);                                    // Código do Convênio no Banco 
                    header += Utils.FormatCode("", "0", 7);                                                // Código do Modelo Personalizado 
                    header += Utils.FormatCode("", "0", 1);                                                // Uso Exclusivo CAIXA 
                }
                #endregion
                header += Utils.FitStringLength(cedente.Nome, 30, 30, ' ', 0, true, true, false);       // Nome do cedente
                header += Utils.FormatCode("", " ", 40);                                                // Mensagem 1
                header += Utils.FormatCode("", " ", 40);                                                // Mensagem 2
                header += Utils.FormatCode(numeroArquivoRemessa.ToString(), "0", 8, true);              // Número Remessa/Retorno
                header += DateTime.Now.ToString("ddMMyyyy");                                            // Data de Gravação Remessa/Retorno 
                header += Utils.FormatCode("", "0", 8);                                                 // Data do Crédito 
                header += Utils.FormatCode("", " ", 33);                                                // Uso Exclusivo FEBRABAN/CNAB

                return header;
            }
            catch (Exception e)
            {
                throw new Exception("Erro ao gerar HEADER DO LOTE do arquivo de remessa.", e);
            }
        }
        #endregion

        #region DETALHE
        public string GerarDetalheSegmentoPRemessaCNAB240(Boleto boleto, int numeroRegistro, string numeroConvenio, Cedente cedente)
        {
            try
            {
                validaInstrucoes240(boleto); // Para protestar, devolver ou desconto.

                var dvAgencia = cedente.ContaBancaria.DigitoAgencia == null ? "0" : cedente.ContaBancaria.DigitoAgencia;
                this.FormataNossoNumeroRemessa(boleto);

                string _segmentoP = Utils.FormatCode(Codigo.ToString(), "0", 3, true);                      // código do banco na compensação
                _segmentoP += "0001";                                                                       // Lote de Serviço
                _segmentoP += "3";                                                                          // Tipo de Registro 
                _segmentoP += Utils.FormatCode(numeroRegistro.ToString(), "0", 5, true);                    // Nº Sequencial do Registro no Lote 
                _segmentoP += "P";                                                                          // Cód. Segmento do Registro Detalhe
                _segmentoP += " ";                                                                          // Uso Exclusivo FEBRABAN/CNAB
                _segmentoP += Utils.FormatCode(boleto.CodigoOcorrencia, "0", 2);                            // Código de Movimento Remessa 
                _segmentoP += Utils.FormatCode(cedente.ContaBancaria.Agencia, "0", 5, true);                // Agência Mantenedora da Conta 
                _segmentoP += Utils.FormatCode(dvAgencia, "0", 1);                                          // Dígito Verificador da Agência 
                #region Tratamento Conta Método de Cobrança
                if (cedente.Carteira.Equals("CR") || cedente.Carteira.Equals("CS") | cedente.Carteira.Equals("DE"))
                {
                    _segmentoP += Utils.FormatCode(cedente.ContaBancaria.Conta, "0", 12);                       // Número da Conta Corrente 
                    _segmentoP += Utils.FormatCode(cedente.ContaBancaria.DigitoConta, "0", 1);                  // Digito da Conta Corrente 
                    _segmentoP += Banco.Mod11(cedente.ContaBancaria.Agencia + cedente.ContaBancaria.Conta).ToString();// Dígito Verif. Ag./Ced  (sem operação)
                    _segmentoP += Utils.FormatCode("", "0", 9);                                                 // Uso Exclusivo CAIXA
                    _segmentoP += Utils.FormatCode(boleto.NossoNumero, "0", 11);                                // Identificação do Título no Banco
                }
                else
                {
                    _segmentoP += Utils.FormatCode(cedente.Codigo, "0", 6);                                    // Código do Convênio no Banco 
                    _segmentoP += Utils.FormatCode("", "0", 8);                                                // Uso Exclusivo CAIXA 
                    _segmentoP += Utils.FormatCode("", "0", 3);                                                // Uso Exclusivo CAIXA 
                    _segmentoP += Utils.FormatCode(boleto.NossoNumero, "0", 17);                               // Identificação do Título no Banco
                }
                #endregion
                _segmentoP += "1";                                                                          // Código da Carteira (Cobrança Simples = 1)
                _segmentoP += (boleto.Carteira == "SR" ? "2" : "1");                                        // Forma de Cadastr. do Título no Banco 
                _segmentoP += "2";                                                                          // Tipo de Documento 
                #region Emissão e Distribuição do Boleto
                var emissaoBoleto = ""; // padrão Cliente Emite
                var distribuicaoBoleto = ""; // padrão Cliente Distribui

                if (cedente.Carteira.Equals("CR") || cedente.Carteira.Equals("CS") | cedente.Carteira.Equals("DE"))
                {
                    emissaoBoleto = "2";
                    distribuicaoBoleto = "2";
                }
                else
                {
                    emissaoBoleto = "2";
                    distribuicaoBoleto = "0";
                }

                if (boleto.BancoGeraBoleto)
                {
                    emissaoBoleto = "1";
                    distribuicaoBoleto = "1";
                }
                #endregion
                _segmentoP += emissaoBoleto;                                                                // Identificação da Emissão do Bloqueto 
                _segmentoP += distribuicaoBoleto;                                                           // Identificação da Distribuição
                _segmentoP += Utils.FormatCode(boleto.NumeroDocumento, "0", 11);                            // Número do Documento de Cobrança 
                _segmentoP += "    ";                                                                       // Uso Exclusivo CAIXA
                _segmentoP += boleto.DataVencimento.ToString("ddMMyyyy");                                   // Data de Vencimento do Título
                #region Valor
                if (cedente.Carteira.Equals("CR"))
                {
                    _segmentoP += Utils.FitStringLength(boleto.ValorBoleto.ToString("0.00").Replace(",", ""), 15, 15, '0', 0, true, true, true);
                }
                else
                {
                    _segmentoP += Utils.FitStringLength(boleto.ValorBoleto.ToString("0.00").Replace(",", ""), 15, 15, '0', 0, true, true, true);
                }
                #endregion
                _segmentoP += Utils.FormatCode("", "0", 5);                                                 // Agência Encarregada da Cobrança 
                _segmentoP += "0";                                                                          // Dígito Verificador da Agência 
                _segmentoP += Utils.FormatCode(boleto.EspecieDocumento.Codigo.ToString(), "0", 2);          // Espécie do Título 
                _segmentoP += "N";                                                                          // Identific. de Título Aceito/Não Aceito
                _segmentoP += boleto.DataDocumento.ToString("ddMMyyyy");                                    // Data da Emissão do Título 
                #region Juros e Desconto
                if (boleto.PercJurosMora > 0)
                {
                    _segmentoP += "1";                                                                      // Código do Juros de Mora '1' = Valor por Dia - '2' = Taxa Mensal 
                    _segmentoP += boleto.DataVencimento.AddDays(1).ToString("ddMMyyyy");                    // Data do Juros de Mora 
                    _segmentoP += Utils.FormatCode(boleto.PercJurosMora.ToString().Replace(",", "").Replace(".", ""), "0", 15); // Juros de Mora por Dia/Taxa 
                }
                else
                {
                    _segmentoP += "3";                                                                      // Código do Juros de Mora '1' = Valor por Dia - '2' = Taxa Mensal 
                    _segmentoP += Utils.FormatCode("", "0", 8);                                             // Data do Juros de Mora 
                    _segmentoP += Utils.FormatCode("", "0", 15);                                            // Juros de Mora por Dia/Taxa 
                }

                if (boleto.ValorDesconto > 0)
                {
                    _segmentoP += "1";                                                                      // Código do Desconto 
                    _segmentoP += boleto.DataVencimento.ToString("ddMMyyyy");                               // Data do Desconto
                    _segmentoP += Utils.FormatCode(boleto.ValorDesconto.ToString().Replace(",", "").Replace(".", ""), "0", 15); // Valor/Percentual a ser Concedido 
                }
                else
                {
                    _segmentoP += "0";                                                                      // Código do Desconto 
                    _segmentoP += Utils.FormatCode("", "0", 8);                                             // Data do Desconto
                    _segmentoP += Utils.FormatCode("", "0", 15);                                            // Valor/Percentual a ser Concedido
                }
                #endregion
                _segmentoP += Utils.FormatCode(boleto.IOF.ToString().Replace(",", "").Replace(".", ""), "0", 15); // Valor do IOF a ser Recolhido 
                _segmentoP += Utils.FormatCode(boleto.Abatimento.ToString().Replace(",", "").Replace(".", ""), "0", 15); // Valor do Abatimento 
                _segmentoP += Utils.FormatCode("", " ", 25);                                                // Identificação do Título na Empresa
                _segmentoP += (protestar ? "1" : "3");                                                      // Código para Protesto
                _segmentoP += diasProtesto.ToString("00");                                                  // Número de Dias para Protesto 2 posi
                _segmentoP += (baixaDevolver ? "1" : "2");                                                  // Código para Baixa/Devolução 1 posi
                _segmentoP += diasDevolucao.ToString("000");                                                 // Número de Dias para Baixa/Devolução 3 posi
                _segmentoP += boleto.Moeda.ToString("00");                                                  // Código da Moeda 

                if (cedente.Carteira.Equals("CR") || cedente.Carteira.Equals("CS") | cedente.Carteira.Equals("DE"))
                    _segmentoP += Utils.FormatCode("", " ", 10);                                                // Uso Exclusivo FEBRABAN/CNAB 
                else
                    _segmentoP += Utils.FormatCode("", "0", 10);                                                // Uso Exclusivo FEBRABAN/CNAB 

                _segmentoP += Utils.FormatCode("", " ", 1);                                                 // Uso Exclusivo FEBRABAN/CNAB 

                return _segmentoP;
            }
            catch (Exception e)
            {
                throw new Exception("Erro ao gerar SEGMENTO P do arquivo de remessa.", e);
            }
        }

        public string GerarDetalheSegmentoQRemessaCNAB240(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
        {
            try
            {
                string _segmentoQ = Utils.FormatCode(Codigo.ToString(), "0", 3, true);                                  // código do banco na compensação
                _segmentoQ += "0001";                                                                                   // Lote de Serviço
                _segmentoQ += "3";                                                                                      // Tipo de Registro 
                _segmentoQ += Utils.FormatCode(numeroRegistro.ToString(), "0", 5, true);                                // Nº Sequencial do Registro no Lote 
                _segmentoQ += "Q";                                                                                      // Cód. Segmento do Registro Detalhe
                _segmentoQ += " ";                                                                                      // Uso Exclusivo FEBRABAN/CNAB
                _segmentoQ += Utils.FormatCode(boleto.CodigoOcorrencia, "0", 2);                                        // Código de Movimento Remessa
                _segmentoQ += (boleto.Sacado.CPFCNPJ.Length == 11 ? "1" : "2");                                         // Tipo de Inscrição 
                _segmentoQ += Utils.FormatCode(boleto.Sacado.CPFCNPJ, "0", 15, true);                                   // Número de Inscrição 
                _segmentoQ += Utils.FitStringLength(boleto.Sacado.Nome, 40, 40, ' ', 0, true, true, false);             // Nome
                _segmentoQ += Utils.FitStringLength(boleto.Sacado.Endereco.End, 40, 40, ' ', 0, true, true, false);     // Endereço
                _segmentoQ += Utils.FitStringLength(boleto.Sacado.Endereco.Bairro, 15, 15, ' ', 0, true, true, false);  // Bairro 
                _segmentoQ += boleto.Sacado.Endereco.CEP;                                                               // CEP + Sufixo do CEP
                _segmentoQ += Utils.FitStringLength(boleto.Sacado.Endereco.Cidade, 15, 15, ' ', 0, true, true, false);  // Cidade 
                _segmentoQ += boleto.Sacado.Endereco.UF;                                                                // Unidade da Federação
                _segmentoQ += "0";                                                                                      // Tipo de Inscrição 
                _segmentoQ += Utils.FormatCode("", "0", 15);                                                            // Número de Inscrição CPF/CNPJ
                _segmentoQ += Utils.FormatCode("", " ", 40);                                                            // Nome do Sacador/Avalista 
                _segmentoQ += Utils.FormatCode("", " ", 31);                                                            // Uso Exclusivo FEBRABAN/CNAB

                _segmentoQ = Utils.SubstituiCaracteresEspeciais(_segmentoQ);

                return _segmentoQ;
            }
            catch (Exception e)
            {
                throw new Exception("Erro ao gerar SEGMENTO Q do arquivo de remessa.", e);
            }
        }

        public string GerarDetalheSegmentoRRemessaCNAB240(Boleto boleto, int numeroRegistroDetalhe, TipoArquivo CNAB240)
        {
            try
            {
                string _segmentoR = Utils.FormatCode(Codigo.ToString(), "0", 3, true);                      // código do banco na compensação
                _segmentoR += "0001";                                                                       // Lote de Serviço
                _segmentoR += "3";                                                                          // Tipo de Registro 
                _segmentoR += Utils.FormatCode(numeroRegistroDetalhe.ToString(), "0", 5, true);             // Nº Sequencial do Registro no Lote 
                _segmentoR += "R";                                                                          // Cód. Segmento do Registro Detalhe
                _segmentoR += " ";                                                                          // Uso Exclusivo FEBRABAN/CNAB
                _segmentoR += Utils.FormatCode(boleto.CodigoOcorrencia, "0", 2);                            // Código de Movimento Remessa
                _segmentoR += Utils.FormatCode("", " ", 48);                                                // Uso Exclusivo FEBRABAN/CNAB 
                #region Multa
                if (boleto.ValorMulta > 0)
                {
                    _segmentoR += "1";                                                                      // Código da Multa '1' = Valor Fixo,'2' = Percentual,'0' = Sem Multa 
                    _segmentoR += boleto.DataVencimento.ToString("ddMMyyyy");                               // Data da Multa 
                    _segmentoR += Utils.FormatCode(boleto.ValorMulta.ToString().Replace(",", "").Replace(".", ""), "0", 15); // Valor/Percentual a Ser Aplicado
                }
                else if (boleto.PercMulta > 0)
                {
                    _segmentoR += "2";                                                                      // Código da Multa '1' = Valor Fixo,'2' = Percentual,'0' = Sem Multa 
                    _segmentoR += boleto.DataVencimento.ToString("ddMMyyyy");                               // Data da Multa 
                    _segmentoR += Utils.FormatCode(boleto.PercMulta.ToString().Replace(",", "").Replace(".", ""), "0", 15); // Valor/Percentual a Ser Aplicado
                }
                else
                {
                    _segmentoR += "0";                                                                      // Código da Multa '1' = Valor Fixo,'2' = Percentual,'0' = Sem Multa 
                    _segmentoR += Utils.FormatCode("", "0", 8);                                             // Data da Multa 
                    _segmentoR += Utils.FormatCode("", "0", 15);                                            // Valor/Percentual a Ser Aplicado
                }
                #endregion
                _segmentoR += Utils.FormatCode("", " ", 10);                                                // Informação ao Sacado
                _segmentoR += Utils.FormatCode("", " ", 40);                                                // Mensagem 3
                _segmentoR += Utils.FormatCode("", " ", 40);                                                // Mensagem 4
                _segmentoR += Utils.FormatCode("", " ", 61);                                                // Uso Exclusivo FEBRABAN/CNAB 

                return _segmentoR;
            }
            catch (Exception e)
            {
                throw new Exception("Erro ao gerar SEGMENTO Q do arquivo de remessa.", e);
            }
        }

        public string GerarDetalheSegmentoSRemessaCNAB240(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
        {
            try
            {
                string _segmentoS;

                _segmentoS = Utils.FormatCode(Codigo.ToString(), "0", 3, true);
                _segmentoS += "00013";
                _segmentoS += Utils.FormatCode(numeroRegistro.ToString(), "0", 5, true);
                _segmentoS += "S ";
                _segmentoS += Utils.FormatCode(boleto.CodigoOcorrencia, "0", 2);
                _segmentoS += "3";

                if (boleto.Instrucoes.Count > 0)
                {
                    var instrucoes = string.Empty;
                    foreach (var item in boleto.Instrucoes)
                        instrucoes += item.Descricao.Replace("<br/>", " ") + " ";

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
        public string GerarTrailerLoteRemessaCNAB240(int numeroRegistro, Boleto boletos)
        {
            try
            {
                string _trailerLote = Utils.FormatCode(Codigo.ToString(), "0", 3, true);                      // código do banco na compensação
                _trailerLote += "0001";                                                                       // Lote de Serviço
                _trailerLote += "5";                                                                          // Tipo de Registro 
                _trailerLote += Utils.FormatCode("", " ", 9);                                                 // Uso Exclusivo FEBRABAN/CNAB
                _trailerLote += Utils.FormatCode(numeroRegistro.ToString(), "0", 6);                          // Nº Sequencial do Registro no Lote 
                // Totalização da Cobrança Simples
                _trailerLote += Utils.FormatCode("", "0", 6);                                                 // Quantidade de Títulos em Cobrança
                _trailerLote += Utils.FormatCode("", "0", 17);                                                // Valor Total dos Títulos em Carteiras
                #region Tratamento Zeros Método de Cobrança
                if (boletos.Cedente.Carteira.Equals("CR") || boletos.Cedente.Carteira.Equals("CS") | boletos.Cedente.Carteira.Equals("DE"))
                {
                    _trailerLote += Utils.FormatCode("", "0", 6);                                                 // Uso Exclusivo FEBRABAN/CNAB
                    _trailerLote += Utils.FormatCode("", "0", 17);                                                // Uso Exclusivo FEBRABAN/CNAB 
                }
                #endregion
                // Totalização da Cobrança Caucionada
                _trailerLote += Utils.FormatCode("", "0", 6);                                                 // Quantidade de Títulos em Cobrança
                _trailerLote += Utils.FormatCode("", "0", 17);                                                // Valor Total dos Títulos em Carteiras
                // Totalização da Cobrança Descontada
                _trailerLote += Utils.FormatCode("", "0", 6);                                                 // Quantidade de Títulos em Cobrança
                _trailerLote += Utils.FormatCode("", "0", 17);                                                // Valor Total dos Títulos em Carteiras
                #region Tratamento Brancos Método de Cobrança
                if (boletos.Cedente.Carteira.Equals("CR") || boletos.Cedente.Carteira.Equals("CS") | boletos.Cedente.Carteira.Equals("DE"))
                    _trailerLote += Utils.FormatCode("", " ", 8);                                                 // Uso Exclusivo FEBRABAN/CNAB
                else
                    _trailerLote += Utils.FormatCode("", " ", 31);                                                 // Uso Exclusivo FEBRABAN/CNAB
                #endregion
                _trailerLote += Utils.FormatCode("", " ", 117);                                               // Uso Exclusivo FEBRABAN/CNAB

                return _trailerLote;
            }
            catch (Exception e)
            {
                throw new Exception("Erro ao gerar Trailer de Lote do arquivo de remessa.", e);
            }
        }

        public string GerarTrailerArquivoRemessaCNAB240(int numeroRegistro)
        {
            try
            {
                string header = Utils.FormatCode(Codigo.ToString(), "0", 3, true);                      // código do banco na compensação
                header += "9999";                                                                       // Lote de Serviço
                header += "9";                                                                          // Tipo de Registro 
                header += Utils.FormatCode("", " ", 9);                                                 // Uso Exclusivo FEBRABAN/CNAB
                header += "000001";                                                                     // Quantidade de Lotes do Arquivo
                header += Utils.FormatCode(numeroRegistro.ToString(), "0", 6, true);                    // Quantidade de Registros do Arquivo
                header += Utils.FormatCode("", " ", 6);                                                 // Uso Exclusivo FEBRABAN/CNAB
                header += Utils.FormatCode("", " ", 205);                                               // Uso Exclusivo FEBRABAN/CNAB

                return header;
            }
            catch (Exception e)
            {
                throw new Exception("Erro ao gerar Trailer de arquivo de remessa.", e);
            }
        }
        #endregion

        #endregion

        #region Métodos de processamento do arquivo retorno CNAB240
        public override HeaderRetorno LerHeaderRetornoCNAB240(string registro)
        {
            try
            {
                HeaderRetorno header = new HeaderRetorno(registro);
                tipoArquivoRetorno = registro.Substring(163, 3);

                //Se for SIGCB
                if (tipoArquivoRetorno.Equals("040") || tipoArquivoRetorno.Equals("050"))
                {
                    header.Agencia = Convert.ToInt32(registro.Substring(52, 5));
                    header.DigitoAgencia = Convert.ToInt32(registro.Substring(57, 1));
                    header.CodigoCedente = Convert.ToInt32(registro.Substring(58, 6));
                }
                // Se for SICOB
                else
                {
                    header.Agencia = Convert.ToInt32(registro.Substring(52, 5));
                    header.DigitoAgencia = Convert.ToInt32(registro.Substring(57, 1));
                    header.CodigoCedente = Convert.ToInt32(registro.Substring(58, 6));
                }

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
                if (!registro.Substring(13, 1).Equals(@"T"))
                    throw new Exception("Registro inválida. O detalhe não possuí as características do segmento T.");

                DetalheSegmentoTRetornoCNAB240 segmentoT = new DetalheSegmentoTRetornoCNAB240(registro);

                #region Tratamento por Método de Cobrança
                if (tipoArquivoRetorno.Equals("050") || tipoArquivoRetorno.Equals("040"))
                {
                    if (!registro.Substring(38, 18).IndexOf(" ").Equals(-1))
                        segmentoT.NossoNumero = registro.Substring(41, 15);
                    else
                        segmentoT.NossoNumero = registro.Substring(38, 9);

                    segmentoT.CodigoCedenteConvenio = Convert.ToInt32(registro.Substring(23, 6));
                }
                else
                {
                    segmentoT.Conta = Convert.ToInt32(registro.Substring(23, 12));
                    segmentoT.DigitoConta = registro.Substring(35, 1);
                    segmentoT.NossoNumero = registro.Substring(46, 11);
                }
                #endregion

                segmentoT.Agencia = Convert.ToInt32(registro.Substring(17, 5));
                segmentoT.DigitoAgencia = registro.Substring(22, 1);
                segmentoT.CodigoCarteira = Convert.ToInt32(registro.Substring(57, 1));
                segmentoT.CodigoBanco = Convert.ToInt32(registro.Substring(0, 3));
                segmentoT.idCodigoMovimento = Convert.ToInt32(registro.Substring(15, 2));
                segmentoT.CodigoMovimento = new CodigoMovimento(001, segmentoT.idCodigoMovimento);
                segmentoT.NumeroDocumento = registro.Substring(58, 11);
                segmentoT.DataVencimento = registro.Substring(73, 8).ToString() == "00000000" ? DateTime.Now : DateTime.ParseExact(registro.Substring(73, 8), "ddMMyyyy", CultureInfo.InvariantCulture);
                segmentoT.ValorTitulo = Convert.ToDecimal(registro.Substring(81, 15)) / 100;
                segmentoT.IdentificacaoTituloEmpresa = registro.Substring(105, 25);
                segmentoT.TipoInscricao = Convert.ToInt32(registro.Substring(132, 1));
                segmentoT.NumeroInscricao = registro.Substring(133, 15);
                segmentoT.NomeSacado = registro.Substring(148, 40);
                segmentoT.ValorTarifas = Convert.ToDecimal(registro.Substring(198, 15)) / 100;
                segmentoT.CodigoMovimento = new CodigoMovimento(segmentoT.CodigoBanco, segmentoT.idCodigoMovimento);
                segmentoT.IdRejeicao1 = registro.Substring(208, 2);
                segmentoT.IdRejeicao2 = registro.Substring(210, 2);
                segmentoT.IdRejeicao3 = registro.Substring(212, 2);
                segmentoT.IdRejeicao4 = registro.Substring(214, 2);
                segmentoT.IdRejeicao5 = registro.Substring(216, 2);

                if (segmentoT.IdRejeicao1.Contains("A"))
                {
                    segmentoT.CodigoRejeicao1 = new CodigoRejeicao(segmentoT.CodigoBanco, 0);
                    segmentoT.CodigoRejeicao1.Descricao = "Sacado DDA";
                }
                else
                {
                    if (!segmentoT.IdRejeicao1.Equals("  "))
                        segmentoT.CodigoRejeicao1 = new CodigoRejeicao(segmentoT.CodigoBanco, Convert.ToInt32(segmentoT.IdRejeicao1));
                    else
                        segmentoT.CodigoRejeicao1 = new CodigoRejeicao(segmentoT.CodigoBanco, 0);
                }

                if (segmentoT.IdRejeicao2.Contains("A"))
                {
                    segmentoT.CodigoRejeicao2 = new CodigoRejeicao(segmentoT.CodigoBanco, 0);
                    segmentoT.CodigoRejeicao2.Descricao = "Sacado DDA";
                }
                else
                {
                    if (!segmentoT.IdRejeicao2.Equals("  "))
                        segmentoT.CodigoRejeicao2 = new CodigoRejeicao(segmentoT.CodigoBanco, Convert.ToInt32(segmentoT.IdRejeicao2));
                    else
                        segmentoT.CodigoRejeicao2 = new CodigoRejeicao(segmentoT.CodigoBanco, 0);
                }

                if (segmentoT.IdRejeicao3.Contains("A"))
                {
                    segmentoT.CodigoRejeicao3 = new CodigoRejeicao(segmentoT.CodigoBanco, 0);
                    segmentoT.CodigoRejeicao3.Descricao = "Sacado DDA";
                }
                else
                {
                    if (!segmentoT.IdRejeicao3.Equals("  "))
                        segmentoT.CodigoRejeicao3 = new CodigoRejeicao(segmentoT.CodigoBanco, segmentoT.IdRejeicao3);
                    else
                        segmentoT.CodigoRejeicao3 = new CodigoRejeicao(segmentoT.CodigoBanco, 0);
                }

                if (segmentoT.IdRejeicao4.Contains("A"))
                {
                    segmentoT.CodigoRejeicao4 = new CodigoRejeicao(segmentoT.CodigoBanco, 0);
                    segmentoT.CodigoRejeicao4.Descricao = "Sacado DDA";
                }
                else
                {
                    if (!segmentoT.IdRejeicao4.Equals("  "))
                        segmentoT.CodigoRejeicao4 = new CodigoRejeicao(segmentoT.CodigoBanco, segmentoT.IdRejeicao4);
                    else
                        segmentoT.CodigoRejeicao4 = new CodigoRejeicao(segmentoT.CodigoBanco, 0);
                }

                if (segmentoT.IdRejeicao5.Contains("A"))
                {
                    segmentoT.CodigoRejeicao5 = new CodigoRejeicao(segmentoT.CodigoBanco, 0);
                    segmentoT.CodigoRejeicao5.Descricao = "Sacado DDA";
                }
                else
                {
                    if (!segmentoT.IdRejeicao5.Equals("  "))
                        segmentoT.CodigoRejeicao5 = new CodigoRejeicao(segmentoT.CodigoBanco, Convert.ToInt32(segmentoT.IdRejeicao5));
                    else
                        segmentoT.CodigoRejeicao5 = new CodigoRejeicao(segmentoT.CodigoBanco, 0);
                }

                switch (Convert.ToInt32(segmentoT.idCodigoMovimento))
                {
                    case (int)EnumCodigoMovimento_Caixa.EntradaConfirmada:
                        segmentoT.Aceito = true;
                        break;
                    case (int)EnumCodigoMovimento_Caixa.Liquidação:
                        segmentoT.Baixado = true;
                        segmentoT.Aceito = false;
                        break;
                    case (int)EnumCodigoMovimento_Caixa.Baixa:
                        segmentoT.Cancelado = true;
                        segmentoT.Aceito = false;
                        break;
                    case (int)EnumCodigoMovimento_Caixa.EntradaRejeitada:
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
            try
            {
                if (!registro.Substring(13, 1).Equals(@"U"))
                {
                    throw new Exception("Registro inválida. O detalhe não possuí as características do segmento U.");
                }

                DetalheSegmentoURetornoCNAB240 segmentoU = new DetalheSegmentoURetornoCNAB240(registro);
                segmentoU.JurosMultaEncargos = Convert.ToDecimal(registro.Substring(17, 15)) / 100;
                segmentoU.ValorDescontoConcedido = Convert.ToDecimal(registro.Substring(32, 15)) / 100;
                segmentoU.ValorAbatimentoConcedido = Convert.ToDecimal(registro.Substring(47, 15)) / 100;
                segmentoU.ValorIOFRecolhido = Convert.ToDecimal(registro.Substring(62, 15)) / 100;
                segmentoU.ValorOcorrenciaSacado = segmentoU.ValorPagoPeloSacado = Convert.ToDecimal(registro.Substring(77, 15)) / 100;
                segmentoU.ValorLiquidoASerCreditado = Convert.ToDecimal(registro.Substring(92, 15)) / 100;
                segmentoU.ValorOutrasDespesas = Convert.ToDecimal(registro.Substring(107, 15)) / 100;
                segmentoU.ValorOutrosCreditos = Convert.ToDecimal(registro.Substring(122, 15)) / 100;
                segmentoU.DataOcorrencia = segmentoU.DataOcorrencia = DateTime.ParseExact(registro.Substring(137, 8), "ddMMyyyy", CultureInfo.InvariantCulture);
                segmentoU.DataCredito = registro.Substring(145, 8).Equals("00000000") ? DateTime.Now : DateTime.ParseExact(registro.Substring(145, 8), "ddMMyyyy", CultureInfo.InvariantCulture);

                return segmentoU;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao processar arquivo de RETORNO - SEGMENTO U.", ex);
            }
        }
        #endregion

        #region Métodos de geração do arquivo remessa CNAB400
        public bool ValidarRemessaCNAB400(string numeroConvenio, IBanco banco, Cedente cedente, Boletos boletos, int numeroArquivoRemessa, out string mensagem)
        {
            bool vRetorno = true;
            string vMsg = string.Empty;
            //
            #region Pré Validações
            if (banco == null)
            {
                vMsg += String.Concat("Remessa: O Banco é Obrigatório!", Environment.NewLine);
                vRetorno = false;
            }
            if (cedente == null)
            {
                vMsg += String.Concat("Remessa: O Cedente/Beneficiário é Obrigatório!", Environment.NewLine);
                vRetorno = false;
            }
            if (boletos == null || boletos.Count.Equals(0))
            {
                vMsg += String.Concat("Remessa: Deverá existir ao menos 1 boleto para geração da remessa!", Environment.NewLine);
                vRetorno = false;
            }
            #endregion
            //
            foreach (Boleto boleto in boletos)
            {
                #region Validação de cada boleto
                if (boleto.Remessa == null)
                {
                    vMsg += String.Concat("Boleto: ", boleto.NumeroDocumento, "; Remessa: Informe as diretrizes de remessa!", Environment.NewLine);
                    vRetorno = false;
                }
                else
                {
                    //#region Validações da Remessa que deverão estar preenchidas quando CAIXA
                    //if (String.IsNullOrEmpty(boleto.Remessa.Ambiente))
                    //{
                    //    vMsg += String.Concat("Boleto: ", boleto.NumeroDocumento, "; Remessa: Informe o Tipo Documento!", Environment.NewLine);
                    //    vRetorno = false;
                    //}
                    //#endregion
                }
                #endregion
            }
            //
            mensagem = vMsg;
            return vRetorno;
        }

        #region HEADER
        public string GerarHeaderRemessaCNAB400(int numeroConvenio, Cedente cedente, int numeroArquivoRemessa)
        {
            try
            {
                TRegistroEDI reg = new TRegistroEDI();
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0001, 001, 0, "0", ' '));                                   //001-001
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0002, 001, 0, "1", ' '));                                   //002-002
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0003, 007, 0, "REMESSA", ' '));                             //003-009 REM.TST
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0010, 002, 0, "01", ' '));                                  //010-011
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0012, 015, 0, "COBRANCA", ' '));                            //012-026
                #region Tratamento Método de Cobrança
                //Se a carteira for CR (SICOB)
                if (cedente.Carteira.Equals("CR") || cedente.Carteira.Equals("CS") | cedente.Carteira.Equals("DE"))
                {
                    reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0027, 016, 0, cedente.Codigo, ' '));                //027-042
                    reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0043, 004, 0, string.Empty, ' '));                  //043-046
                }
                //Se a carteira for RG (SIGCB)
                else
                {
                    reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0027, 004, 0, cedente.ContaBancaria.Agencia, ' ')); //027-030
                    reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0031, 006, 0, cedente.Codigo, ' '));                //031-036
                    reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0037, 010, 0, string.Empty, ' '));                  //037-046
                }
                #endregion
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0047, 030, 0, cedente.Nome.ToUpper(), ' '));                //047-076
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0077, 003, 0, "104", ' '));                                 //077-079
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0080, 015, 0, "C ECON FEDERAL", ' '));                      //080-094
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediDataDDMMAA___________, 0095, 006, 0, DateTime.Now, ' '));                          //095-100
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0101, 289, 0, "", ' '));                                    //101-389
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0390, 005, 0, numeroArquivoRemessa.ToString(), ' '));       //390-394
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0395, 006, 0, "000001", ' '));                              //395-400
                //
                reg.CodificarLinha();
                //
                string vLinha = reg.LinhaRegistro;
                string _header = Utils.SubstituiCaracteresEspeciais(vLinha);
                //
                return _header;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar HEADER do arquivo de remessa do CNAB400.", ex);
            }
        }
        #endregion

        #region DETALHER
        public override string GerarDetalheRemessa(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
        {
            try
            {
                string _detalhe = " ";

                base.GerarDetalheRemessa(boleto, numeroRegistro, tipoArquivo);

                switch (tipoArquivo)
                {
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
                #region Tratamento Método de Cobrança
                //Se a carteira for CR - CS - DE (SICOB)
                if (boleto.Cedente.Carteira.Equals("CR") || boleto.Cedente.Carteira.Equals("CS") | boleto.Cedente.Carteira.Equals("DE"))
                {
                    #region SICOB
                    reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0018, 016, 0, boleto.Cedente.Codigo, '0'));                 //018-033
                    reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0034, 002, 0, string.Empty, '0'));                          //034-035
                    reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0036, 002, 0, "00", '0'));                                  //036-037 //Taxa de permanencia - codigo adotado para identificar juros e mora (00 sendo por dia)
                    reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0038, 025, 0, string.Empty, ' '));                          //038-062
                    reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0063, 011, 0, boleto.NossoNumero, '0'));                    //063-073
                    #endregion
                }
                //Se a carteira for RG (SIGCB)
                else
                {
                    #region SIGCB
                    var nossoNumero = boleto.NossoNumero.Substring(0, boleto.NossoNumero.Length - 2);
                    reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0018, 004, 0, boleto.Cedente.ContaBancaria.Agencia, '0'));  //018-021
                    reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0022, 006, 0, boleto.Cedente.Codigo, '0'));                 //022-027
                    #region Tratamento de Emissão e Entrega do Boleto
                    var emissaoBoleto = "";
                    var entregaBoleto = "";
                    if (boleto.BancoGeraBoleto)
                    {
                        emissaoBoleto = "1"; //Pelo banco
                        entregaBoleto = "1"; //Via correio pelo banco
                    }
                    else
                    {
                        emissaoBoleto = "0"; //Pelo beneficiário
                        entregaBoleto = "0"; //Pelo beneficiário
                    }
                    #endregion
                    reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0028, 001, 0, emissaoBoleto, '0'));                         //028-028
                    reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0029, 001, 0, entregaBoleto, '0'));                         //029-029
                    reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0030, 002, 0, "00", '0'));                                  //030-031 //Taxa de permanencia - codigo adotado para identificar juros e mora (00 sendo por dia)
                    reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0032, 025, 0, boleto.NumeroDocumento, '0'));                //032-056
                    reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0057, 017, 0, nossoNumero, '0'));                           //057-073
                    #endregion
                }
                #endregion
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0074, 003, 0, string.Empty, ' '));                              //074-076
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0077, 030, 0, string.Empty, ' '));                              //077-106
                #region Carteiras
                var codigoCarteira = "";
                if (boleto.Cedente.Carteira.Equals("CS"))
                    codigoCarteira = "11";
                else if (boleto.Cedente.Carteira.Equals("CR"))
                    codigoCarteira = "12";
                else if (boleto.Cedente.Carteira.Equals("DE"))
                    codigoCarteira = "41";
                else if (boleto.Cedente.Carteira.Equals("RG"))
                    codigoCarteira = "01";
                #endregion
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0107, 002, 0, codigoCarteira, '0'));                            //107-108
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0109, 002, 0, boleto.CodigoOcorrencia, ' '));                   //109-110   //REMESSA
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0111, 010, 0, boleto.NumeroDocumento, ' '));                    //111-120   
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediDataDDMMAA___________, 0121, 006, 0, boleto.DataVencimento, ' '));                     //121-126
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0127, 013, 2, boleto.ValorBoleto, '0'));                        //127-139
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0140, 003, 0, "104", '0'));                                     //140-142
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0143, 005, 0, "0", '0'));                                       //143-147
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0148, 002, 0, boleto.EspecieDocumento.Codigo.ToString(), '0')); //148-149
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0150, 001, 0, boleto.Aceite, ' '));                             //150-150
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediDataDDMMAA___________, 0151, 006, 0, boleto.DataProcessamento, ' '));                  //151-156
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0157, 002, 0, "", '0'));                                        //157-158
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0159, 002, 0, "", '0'));                                        //159-160
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0161, 013, 2, boleto.PercJurosMora, '0'));                      //161-173
                #region DataDesconto
                string vDataDesconto = "000000";
                if (!boleto.DataDesconto.Equals(DateTime.MinValue))
                    vDataDesconto = boleto.DataDesconto.ToString("ddMMyy");
                #endregion
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0174, 006, 0, vDataDesconto, '0'));                             //174-179
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0180, 013, 2, boleto.ValorDesconto, '0'));                      //180-192
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0193, 013, 2, boleto.IOF, '0'));                                //193-205
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0206, 013, 2, boleto.Abatimento, '0'));                         //206-218
                #region Regra Tipo de Inscrição Sacado
                string vCpfCnpjSac = "99";
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
                #region DataMulta
                string vDataMulta = "000000";
                if (!boleto.DataMulta.Equals(DateTime.MinValue))
                    vDataMulta = boleto.DataMulta.ToString("ddMMyy");
                #endregion
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0352, 006, 0, vDataMulta, '0'));                                //352-357
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0358, 010, 2, boleto.ValorMulta, '0'));                         //358-367
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0368, 022, 0, string.Empty, ' '));                              //368-389
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0390, 002, 0, "", '0'));                                        //390-391
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0392, 002, 0, 0, '0'));                                         //392-393
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0394, 001, 0, boleto.Moeda, '0'));                              //394-394
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
        #endregion

        #region TRAILER
        public override string GerarTrailerRemessa(int numeroRegistro, TipoArquivo tipoArquivo, Cedente cedente, decimal vltitulostotal)
        {
            try
            {
                string _trailer = " ";

                base.GerarTrailerRemessa(numeroRegistro, tipoArquivo, cedente, vltitulostotal);

                switch (tipoArquivo)
                {
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
                TRegistroEDI reg = new TRegistroEDI();
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0001, 001, 0, "9", ' '));            //001-001
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0002, 393, 0, string.Empty, ' '));   //002-394
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
        #endregion

        #endregion

        #region Métodos de processamento do arquivo retorno CNAB400
        public override HeaderRetorno LerHeaderRetornoCNAB400(string registro)
        {
            try
            {
                HeaderRetorno header = new HeaderRetorno();
                header.RegistroHeader = registro;

                //Se for SICOB
                if (registro.Substring(36, 1).IndexOf(" ").Equals(-1))
                {
                    header.Agencia = Convert.ToInt32(registro.Substring(26, 4));
                    header.CodigoCedente = Convert.ToInt32(registro.Substring(26, 16));
                }
                //Se for SIGCB
                else
                {
                    header.Agencia = Convert.ToInt32(registro.Substring(26, 4));
                    header.CodigoCedente = Convert.ToInt32(registro.Substring(30, 6));
                }

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
                //Carteira
                detalhe.Carteira = registro.Substring(106, 2);

                #region Tratamento Leitura Método de Cobrança
                if (detalhe.Carteira.Equals("RG"))
                {
                    //Identificação da Empresa Cedente no Banco
                    detalhe.Agencia = Utils.ToInt32(registro.Substring(18, 4));
                    detalhe.CodigoCedenteConvenio = Utils.ToInt32(registro.Substring(21, 6));
                    //Identificação do Título no Banco
                    detalhe.NossoNumeroComDV = registro.Substring(56, 17);
                    detalhe.NossoNumero = registro.Substring(56, 17);
                }
                else
                {
                    //Identificação da Empresa Cedente no Banco
                    detalhe.Agencia = Utils.ToInt32(registro.Substring(18, 4));
                    detalhe.CodigoCedenteConvenio = Utils.ToInt32(registro.Substring(18, 16));
                    //Identificação do Título no Banco
                    detalhe.NossoNumeroComDV = registro.Substring(62, 11);
                    detalhe.NossoNumero = registro.Substring(62, 10);
                    detalhe.DACNossoNumero = registro.Substring(69, 1);
                }
                #endregion

                //Nº Documento
                detalhe.NumeroDocumento = registro.Substring(37, 25);

                //Identificação de Ocorrência
                detalhe.CodigoOcorrencia = Utils.ToInt32(registro.Substring(108, 2));

                switch (Convert.ToInt32(detalhe.CodigoOcorrencia))
                {
                    case (int)ECodigoOcorrenciaCaixa400.Entrada_Confirmada:
                        detalhe.Aceito = true;
                        break;
                    case (int)ECodigoOcorrenciaCaixa400.Liquidação:
                    case (int)ECodigoOcorrenciaCaixa400.Liquidação_em_Cartório:
                        detalhe.Baixado = true;
                        detalhe.Aceito = true;
                        break;
                    case (int)ECodigoOcorrenciaCaixa400.Baixa:
                    case (int)ECodigoOcorrenciaCaixa400.Baixa_por_Devolução:
                    case (int)ECodigoOcorrenciaCaixa400.Baixa_por_Franco_Pagamento:
                    case (int)ECodigoOcorrenciaCaixa400.Baixa_por_Protesto:
                        detalhe.Cancelado = true;
                        detalhe.Aceito = true;
                        break;
                    case (int)ECodigoOcorrenciaCaixa400.Rejeição:
                        detalhe.Aceito = false;
                        break;
                }

                //Descrição da ocorrência
                detalhe.DescricaoOcorrencia = this.Ocorrencia(detalhe.CodigoOcorrencia.ToString());

                //Valor do Título
                decimal valorTitulo = Convert.ToInt64(registro.Substring(152, 15));
                detalhe.ValorTitulo = valorTitulo / 100;

                //Taxa de boleto
                decimal tarifaCobranca = Convert.ToInt64(registro.Substring(175, 15));
                detalhe.TarifaCobranca = tarifaCobranca / 100;

                //Banco Cobrador
                detalhe.CodigoBanco = Utils.ToInt32(registro.Substring(165, 3));

                //Agência Cobradora
                detalhe.AgenciaCobradora = Utils.ToInt32(registro.Substring(168, 5));

                //Espécie do Título
                detalhe.Especie = Utils.ToInt32(registro.Substring(173, 2));

                //Abatimento Concedido sobre o Título (Valor Abatimento Concedido)
                decimal valorAbatimento = Convert.ToUInt64(registro.Substring(227, 15));
                detalhe.ValorAbatimento = valorAbatimento / 100;

                //Desconto Concedido (Valor Desconto Concedido)
                decimal valorDesconto = Convert.ToUInt64(registro.Substring(240, 15));
                detalhe.Descontos = valorDesconto / 100;

                //Valor Pago
                decimal valorPago = Convert.ToUInt64(registro.Substring(253, 15));
                detalhe.ValorPago = valorPago / 100;

                //Juros Mora
                decimal jurosMora = Convert.ToUInt64(registro.Substring(266, 15));
                detalhe.JurosMora = jurosMora / 100;

                //Multa
                decimal multa = Convert.ToUInt64(registro.Substring(279, 15));
                detalhe.Multa = multa / 100;

                //Data Ocorrência no Banco
                int dataOcorrencia = Utils.ToInt32(registro.Substring(110, 6));
                detalhe.DataOcorrencia = Utils.ToDateTime(dataOcorrencia.ToString("##-##-##"));

                //Data Vencimento do Título
                int dataVencimento = Utils.ToInt32(registro.Substring(146, 6));
                detalhe.DataVencimento = Utils.ToDateTime(dataVencimento.ToString("##-##-##"));

                // Data do Crédito
                int dataCredito = Utils.ToInt32(registro.Substring(110, 6));
                detalhe.DataCredito = Utils.ToDateTime(dataCredito.ToString("##-##-##"));

                detalhe.NumeroSequencial = Utils.ToInt32(registro.Substring(394, 6));

                return detalhe;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao ler detalhe do arquivo de RETORNO / CNAB 400.", ex);
            }
        }

        private string Ocorrencia(string codigo)
        {
            switch (codigo)
            {
                case "02":
                    return "02-Entrada Confirmada";
                case "03":
                    return "03-Entrada Rejeitada";
                case "21":
                    return "21-Liquidação";
                case "22":
                    return "22-Liquidação Em Cartório";
                case "23":
                    return "23-Baixa Por Devolução";
                case "24":
                    return "24-Baixa Por Franco Pagamento";
                case "25":
                    return "25-Baixa Por Protesto";
                case "99":
                    return "99-Rejeitado";
                default:
                    return "";
            }
        }
        #endregion
    }
}