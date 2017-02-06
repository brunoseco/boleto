
using System;
using System.Globalization;
using System.Web.UI;
using Microsoft.VisualBasic;
using BoletoNet.EDI.Banco;

[assembly: WebResource("BoletoNet.Imagens.001.jpg", "image/jpg")]
namespace BoletoNet
{
    internal class Banco_Brasil : AbstractBanco, IBanco
    {

        #region Variáveis

        private string _dacNossoNumero = string.Empty;
        private int _dacBoleto = 0;

        #endregion

        #region Construtores

        internal Banco_Brasil()
        {
            try
            {
                this.Codigo = 1;
                this.Digito = "9";
                this.Nome = "Banco do Brasil";
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao instanciar objeto.", ex);
            }
        }
        #endregion

        #region Métodos de Validação

        public override void ValidaBoleto(Boleto boleto)
        {
            if (string.IsNullOrEmpty(boleto.Carteira))
                throw new NotImplementedException("Banco do Brasil - Carteira não informada. Utilize a carteira 11, 16, 17, 18, 18-019, 18-027, 18-035, 18-140 ou 31.");

            //Verifica as carteiras implementadas
            if (!boleto.Carteira.Equals("11") &
                !boleto.Carteira.Equals("16") &
                !boleto.Carteira.Equals("17") &
                !boleto.Carteira.Equals("17-019") &
                !boleto.Carteira.Equals("17-027") &
                !boleto.Carteira.Equals("18") &
                !boleto.Carteira.Equals("18-019") &
                !boleto.Carteira.Equals("18-027") &
                !boleto.Carteira.Equals("18-035") &
                !boleto.Carteira.Equals("18-140") &
                !boleto.Carteira.Equals("31"))
                throw new NotImplementedException("Banco do Brasil - Carteira não informada ou incorreta. Utilize a carteira 11, 16, 17, 17-019, 17-027, 18, 18-019, 18-027, 18-035, 18-140 ou 31.");

            if (boleto.Cedente.Convenio.ToString().Length > 7 || boleto.Cedente.Convenio.ToString().Length < 4 || boleto.Cedente.Convenio.ToString().Length == 5)
                throw new NotImplementedException("Banco do Brasil - Código do convênio incorreto, é necessário que o código do convênio contenha " +
                                                  "4, 6 ou 7 digitos!");

            //Verifica se o nosso número é válido
            if (Utils.ToString(boleto.NossoNumero) == string.Empty)
                throw new NotImplementedException("Banco do Brasil - Nosso número inválido");

            #region Comentarios

            //#region Carteira 11
            ////Carteira 18 com nosso número de 11 posições
            //if (boleto.Carteira.Equals("11"))
            //{
            //    if (!boleto.TipoModalidade.Equals("21"))
            //    {
            //        if (boleto.NossoNumero.Length > 11)
            //            throw new NotImplementedException(string.Format("Banco do Brasil - Para a carteira {0}, a quantidade máxima são de 11 de posições para o nosso número", boleto.Carteira));

            //        if (boleto.Cedente.Convenio.ToString().Length == 6)
            //            boleto.NossoNumero = string.Format("{0}{1}", boleto.Cedente.Convenio, Utils.FormatCode(boleto.NossoNumero, 11));
            //        else
            //            boleto.NossoNumero = Utils.FormatCode(boleto.NossoNumero, 11);
            //    }
            //    else
            //    {
            //        if (boleto.Cedente.Convenio.ToString().Length != 6)
            //            throw new NotImplementedException(string.Format("Banco do Brasil - Para a carteira {0} e o tipo da modalidade 21, o número do convênio são de 6 posições", boleto.Carteira));

            //        boleto.NossoNumero = Utils.FormatCode(boleto.NossoNumero, 17);
            //    }
            //}
            //#endregion Carteira 11

            //#region Carteira 16
            //if (boleto.Carteira.Equals("16"))
            //{
            //    if (!boleto.TipoModalidade.Equals("21"))
            //    {
            //        if (boleto.Cedente.Convenio.ToString().Length != 6)
            //            throw new NotImplementedException(string.Format("Banco do Brasil - Para a carteira {0}, o número do convênio são de 6 posições", boleto.Carteira));

            //        if (boleto.NossoNumero.Length > 11)
            //            throw new NotImplementedException(string.Format("Banco do Brasil - Para a carteira {0} a quantidade máxima são de 11 de posições para o nosso número", boleto.Carteira));

            //        if (boleto.Cedente.Convenio.ToString().Length == 6)
            //        {
            //            var nossoNumero = boleto.NossoNumero.Substring(boleto.NossoNumero.Length - 5, 5);
            //            boleto.NossoNumero = string.Format("{0}{1}", boleto.Cedente.Convenio, Utils.FormatCode(nossoNumero, "0", 5));
            //        }
            //        else
            //            boleto.NossoNumero = Utils.FormatCode(boleto.NossoNumero, 11);
            //    }
            //    else
            //    {
            //        if (boleto.Cedente.Convenio.ToString().Length != 6)
            //            throw new NotImplementedException(string.Format("Banco do Brasil - Para a carteira {0} e o tipo da modalidade 21, o número do convênio são de 6 posições", boleto.Carteira));

            //        boleto.NossoNumero = Utils.FormatCode(boleto.NossoNumero, 17);
            //    }
            //}
            //#endregion Carteira 16

            //#region Carteira 17
            ////Carteira 17
            //if (boleto.Carteira.Equals("17"))
            //{
            //    switch (boleto.Cedente.Convenio.ToString().Length)
            //    {
            //        //O BB manda como padrão 7 posições, mas é possível solicitar um convênio com 6 posições na carteira 17
            //        case 6:
            //            if (boleto.NossoNumero.Length > 12)
            //                throw new NotImplementedException(string.Format("Banco do Brasil - Para a carteira {0}, a quantidade máxima são de 12 de posições para o nosso número", boleto.Carteira));
            //            boleto.NossoNumero = Utils.FormatCode(boleto.NossoNumero, 12);
            //            break;
            //        case 7:
            //            if (boleto.NossoNumero.Length > 17)
            //                throw new NotImplementedException(string.Format("Banco do Brasil - Para a carteira {0}, a quantidade máxima são de 10 de posições para o nosso número", boleto.Carteira));
            //            boleto.NossoNumero = string.Format("{0}{1}", boleto.Cedente.Convenio, Utils.FormatCode(boleto.NossoNumero, 10));
            //            break;
            //        default:
            //            throw new NotImplementedException(string.Format("Banco do Brasil - Para a carteira {0}, o número do convênio deve ter 6 ou 7 posições", boleto.Carteira));
            //    }
            //}
            //#endregion Carteira 17

            //#region Carteira 17-019
            ////Carteira 17, com variação 019
            //if (boleto.Carteira.Equals("17-019"))
            //{
            //    /*
            //     * Convênio de 7 posições
            //     * Nosso Número com 17 posições
            //     */
            //    if (boleto.Cedente.Convenio.ToString().Length == 7)
            //    {
            //        if (boleto.NossoNumero.Length > 10)
            //        {
            //            var nossoNumero = boleto.NossoNumero.Substring(boleto.NossoNumero.Length - 10, 10);
            //            boleto.NossoNumero = string.Format("{0}{1}", boleto.Cedente.Convenio, Utils.FormatCode(nossoNumero, 10));
            //        }
            //        else
            //            boleto.NossoNumero = string.Format("{0}{1}", boleto.Cedente.Convenio, Utils.FormatCode(boleto.NossoNumero, 10));
            //    }
            //    /*
            //     * Convênio de 6 posições
            //     * Nosso Número com 11 posições
            //     */
            //    else if (boleto.Cedente.Convenio.ToString().Length == 6)
            //    {
            //        if (boleto.NossoNumero.Length > 5)
            //            boleto.NossoNumero = boleto.NossoNumero.Substring(boleto.NossoNumero.Length - 5, 5);

            //        boleto.NossoNumero = string.Format("{0}{1}", boleto.Cedente.Convenio, Utils.FormatCode(boleto.NossoNumero, 5));
            //    }
            //    /*
            //      * Convênio de 4 posições
            //      * Nosso Número com 11 posições
            //      */
            //    else if (boleto.Cedente.Convenio.ToString().Length == 4)
            //    {
            //        if (boleto.NossoNumero.Length > 7)
            //        {
            //            var nossoNumero = boleto.NossoNumero.Substring(boleto.NossoNumero.Length - 7, 7);
            //            boleto.NossoNumero = string.Format("{0}{1}", boleto.Cedente.Convenio, Utils.FormatCode(nossoNumero, 7));
            //        }
            //        else
            //            boleto.NossoNumero = string.Format("{0}{1}", boleto.Cedente.Convenio, Utils.FormatCode(boleto.NossoNumero, "0", 7));
            //    }
            //    else
            //        boleto.NossoNumero = Utils.FormatCode(boleto.NossoNumero, 11);
            //}
            //#endregion Carteira 17-019

            //#region Carteira 18
            ////Carteira 18 com nosso número de 11 posições
            //if (boleto.Carteira.Equals("18"))
            //    boleto.NossoNumero = Utils.FormatCode(boleto.NossoNumero, 11);
            //#endregion Carteira 18

            //#region Carteira 18-019
            ////Carteira 18, com variação 019
            //if (boleto.Carteira.Equals("18-019"))
            //{
            //    /*
            //     * Convênio de 7 posições
            //     * Nosso Número com 17 posições
            //     */
            //    if (boleto.Cedente.Convenio.ToString().Length == 7)
            //    {
            //        if (boleto.NossoNumero.Length > 10)
            //            throw new NotImplementedException(string.Format("Banco do Brasil - Para a carteira {0}, a quantidade máxima são de 10 de posições para o nosso número", boleto.Carteira));

            //        boleto.NossoNumero = string.Format("{0}{1}", boleto.Cedente.Convenio, Utils.FormatCode(boleto.NossoNumero, 10));
            //    }
            //    /*
            //     * Convênio de 6 posições
            //     * Nosso Número com 11 posições
            //     */
            //    else if (boleto.Cedente.Convenio.ToString().Length == 6)
            //    {
            //        //Modalidades de Cobrança Sem Registro – Carteira 16 e 18
            //        //Nosso Número com 17 posições
            //        if (!boleto.TipoModalidade.Equals("21"))
            //        {
            //            if ((boleto.Cedente.Codigo.ToString().Length + boleto.NossoNumero.Length) > 11)
            //                throw new NotImplementedException(string.Format("Banco do Brasil - Para a carteira {0}, a quantidade máxima são de 11 de posições para o nosso número. Onde o nosso número é formado por CCCCCCNNNNN-X: C -> número do convênio fornecido pelo Banco, N -> seqüencial atribuído pelo cliente e X -> dígito verificador do “Nosso-Número”.", boleto.Carteira));

            //            boleto.NossoNumero = string.Format("{0}{1}", boleto.Cedente.Convenio, Utils.FormatCode(boleto.NossoNumero, 5));
            //        }
            //        else
            //        {
            //            if (boleto.Cedente.Convenio.ToString().Length != 6)
            //                throw new NotImplementedException(string.Format("Banco do Brasil - Para a carteira {0} e o tipo da modalidade 21, o número do convênio são de 6 posições", boleto.Carteira));

            //            boleto.NossoNumero = Utils.FormatCode(boleto.NossoNumero, 17);
            //        }
            //    }
            //    /*
            //      * Convênio de 4 posições
            //      * Nosso Número com 11 posições
            //      */
            //    else if (boleto.Cedente.Convenio.ToString().Length == 4)
            //    {
            //        if (boleto.NossoNumero.Length > 7)
            //            throw new NotImplementedException(string.Format("Banco do Brasil - Para a carteira {0}, a quantidade máxima são de 7 de posições para o nosso número [{1}]", boleto.Carteira, boleto.NossoNumero));

            //        boleto.NossoNumero = string.Format("{0}{1}", boleto.Cedente.Convenio, Utils.FormatCode(boleto.NossoNumero, 7));
            //    }
            //    else
            //        boleto.NossoNumero = Utils.FormatCode(boleto.NossoNumero, 11);
            //}
            //#endregion Carteira 18-019

            //#region Carteira 18-027
            ////Carteira 18, com variação 019
            //if (boleto.Carteira.Equals("18-027"))
            //{
            //    /*
            //     * Convênio de 7 posições
            //     * Nosso Número com 17 posições
            //     */
            //    if (boleto.Cedente.Convenio.ToString().Length == 7)
            //    {
            //        if (boleto.NossoNumero.Length > 10)
            //            throw new NotImplementedException(string.Format("Banco do Brasil - Para a carteira {0}, a quantidade máxima são de 10 de posições para o nosso número", boleto.Carteira));

            //        boleto.NossoNumero = string.Format("{0}{1}", boleto.Cedente.Convenio, Utils.FormatCode(boleto.NossoNumero, 10));
            //    }
            //    /*
            //     * Convênio de 6 posições
            //     * Nosso Número com 11 posições
            //     */
            //    else if (boleto.Cedente.Convenio.ToString().Length == 6)
            //    {
            //        //Modalidades de Cobrança Sem Registro – Carteira 16 e 18
            //        //Nosso Número com 17 posições
            //        if (!boleto.TipoModalidade.Equals("21"))
            //        {
            //            if ((boleto.Cedente.Codigo.ToString().Length + boleto.NossoNumero.Length) > 11)
            //                throw new NotImplementedException(string.Format("Banco do Brasil - Para a carteira {0}, a quantidade máxima são de 11 de posições para o nosso número. Onde o nosso número é formado por CCCCCCNNNNN-X: C -> número do convênio fornecido pelo Banco, N -> seqüencial atribuído pelo cliente e X -> dígito verificador do “Nosso-Número”.", boleto.Carteira));

            //            boleto.NossoNumero = string.Format("{0}{1}", boleto.Cedente.Convenio, Utils.FormatCode(boleto.NossoNumero, 5));
            //        }
            //        else
            //        {
            //            if (boleto.Cedente.Convenio.ToString().Length != 6)
            //                throw new NotImplementedException(string.Format("Banco do Brasil - Para a carteira {0} e o tipo da modalidade 21, o número do convênio são de 6 posições", boleto.Carteira));

            //            boleto.NossoNumero = Utils.FormatCode(boleto.NossoNumero, 17);
            //        }
            //    }
            //    /*
            //      * Convênio de 4 posições
            //      * Nosso Número com 11 posições
            //      */
            //    else if (boleto.Cedente.Convenio.ToString().Length == 4)
            //    {
            //        if (boleto.NossoNumero.Length > 7)
            //            throw new NotImplementedException(string.Format("Banco do Brasil - Para a carteira {0}, a quantidade máxima são de 7 de posições para o nosso número [{1}]", boleto.Carteira, boleto.NossoNumero));

            //        boleto.NossoNumero = string.Format("{0}{1}", boleto.Cedente.Convenio, Utils.FormatCode(boleto.NossoNumero, 7));
            //    }
            //    else
            //        boleto.NossoNumero = Utils.FormatCode(boleto.NossoNumero, 11);
            //}
            //#endregion Carteira 18-027

            //#region Carteira 18-035
            ////Carteira 18, com variação 019
            //if (boleto.Carteira.Equals("18-035"))
            //{
            //    /*
            //     * Convênio de 7 posições
            //     * Nosso Número com 17 posições
            //     */
            //    if (boleto.Cedente.Convenio.ToString().Length == 7)
            //    {
            //        if (boleto.NossoNumero.Length > 10)
            //            throw new NotImplementedException(string.Format("Banco do Brasil - Para a carteira {0}, a quantidade máxima são de 10 de posições para o nosso número", boleto.Carteira));

            //        boleto.NossoNumero = string.Format("{0}{1}", boleto.Cedente.Convenio, Utils.FormatCode(boleto.NossoNumero, 10));
            //    }
            //    /*
            //     * Convênio de 6 posições
            //     * Nosso Número com 11 posições
            //     */
            //    else if (boleto.Cedente.Convenio.ToString().Length == 6)
            //    {
            //        //Modalidades de Cobrança Sem Registro – Carteira 16 e 18
            //        //Nosso Número com 17 posições
            //        if (!boleto.TipoModalidade.Equals("21"))
            //        {
            //            if ((boleto.Cedente.Codigo.ToString().Length + boleto.NossoNumero.Length) > 11)
            //                throw new NotImplementedException(string.Format("Banco do Brasil - Para a carteira {0}, a quantidade máxima são de 11 de posições para o nosso número. Onde o nosso número é formado por CCCCCCNNNNN-X: C -> número do convênio fornecido pelo Banco, N -> seqüencial atribuído pelo cliente e X -> dígito verificador do “Nosso-Número”.", boleto.Carteira));

            //            boleto.NossoNumero = string.Format("{0}{1}", boleto.Cedente.Convenio, Utils.FormatCode(boleto.NossoNumero, 5));
            //        }
            //        else
            //        {
            //            if (boleto.Cedente.Convenio.ToString().Length != 6)
            //                throw new NotImplementedException(string.Format("Banco do Brasil - Para a carteira {0} e o tipo da modalidade 21, o número do convênio são de 6 posições", boleto.Carteira));

            //            boleto.NossoNumero = Utils.FormatCode(boleto.NossoNumero, 17);
            //        }
            //    }
            //    /*
            //      * Convênio de 4 posições
            //      * Nosso Número com 11 posições
            //      */
            //    else if (boleto.Cedente.Convenio.ToString().Length == 4)
            //    {
            //        if (boleto.NossoNumero.Length > 7)
            //            throw new NotImplementedException(string.Format("Banco do Brasil - Para a carteira {0}, a quantidade máxima são de 7 de posições para o nosso número [{1}]", boleto.Carteira, boleto.NossoNumero));

            //        boleto.NossoNumero = string.Format("{0}{1}", boleto.Cedente.Convenio, Utils.FormatCode(boleto.NossoNumero, 7));
            //    }
            //    else
            //        boleto.NossoNumero = Utils.FormatCode(boleto.NossoNumero, 11);
            //}
            //#endregion Carteira 18-035

            //#region Carteira 18-140
            ////Carteira 18, com variação 019
            //if (boleto.Carteira.Equals("18-140"))
            //{
            //    /*
            //     * Convênio de 7 posições
            //     * Nosso Número com 17 posições
            //     */
            //    if (boleto.Cedente.Convenio.ToString().Length == 7)
            //    {
            //        if (boleto.NossoNumero.Length > 10)
            //            throw new NotImplementedException(string.Format("Banco do Brasil - Para a carteira {0}, a quantidade máxima são de 10 de posições para o nosso número", boleto.Carteira));

            //        boleto.NossoNumero = string.Format("{0}{1}", boleto.Cedente.Convenio, Utils.FormatCode(boleto.NossoNumero, 10));
            //    }
            //    /*
            //     * Convênio de 6 posições
            //     * Nosso Número com 11 posições
            //     */
            //    else if (boleto.Cedente.Convenio.ToString().Length == 6)
            //    {
            //        //Modalidades de Cobrança Sem Registro – Carteira 16 e 18
            //        //Nosso Número com 17 posições
            //        if (!boleto.TipoModalidade.Equals("21"))
            //        {
            //            if ((boleto.Cedente.Codigo.ToString().Length + boleto.NossoNumero.Length) > 11)
            //                throw new NotImplementedException(string.Format("Banco do Brasil - Para a carteira {0}, a quantidade máxima são de 11 de posições para o nosso número. Onde o nosso número é formado por CCCCCCNNNNN-X: C -> número do convênio fornecido pelo Banco, N -> seqüencial atribuído pelo cliente e X -> dígito verificador do “Nosso-Número”.", boleto.Carteira));

            //            boleto.NossoNumero = string.Format("{0}{1}", boleto.Cedente.Convenio, Utils.FormatCode(boleto.NossoNumero, 5));
            //        }
            //        else
            //        {
            //            if (boleto.Cedente.Convenio.ToString().Length != 6)
            //                throw new NotImplementedException(string.Format("Banco do Brasil - Para a carteira {0} e o tipo da modalidade 21, o número do convênio são de 6 posições", boleto.Carteira));

            //            boleto.NossoNumero = Utils.FormatCode(boleto.NossoNumero, 17);
            //        }
            //    }
            //    /*
            //      * Convênio de 4 posições
            //      * Nosso Número com 11 posições
            //      */
            //    else if (boleto.Cedente.Convenio.ToString().Length == 4)
            //    {
            //        if (boleto.NossoNumero.Length > 7)
            //            throw new NotImplementedException(string.Format("Banco do Brasil - Para a carteira {0}, a quantidade máxima são de 7 de posições para o nosso número [{1}]", boleto.Carteira, boleto.NossoNumero));

            //        boleto.NossoNumero = string.Format("{0}{1}", boleto.Cedente.Convenio, Utils.FormatCode(boleto.NossoNumero, 7));
            //    }
            //    else
            //        boleto.NossoNumero = Utils.FormatCode(boleto.NossoNumero, 11);
            //}
            //#endregion Carteira 18-140

            //#region Carteira 31
            ////Carteira 31
            //if (boleto.Carteira.Equals("31"))
            //{
            //    switch (boleto.Cedente.Convenio.ToString().Length)
            //    {
            //        //O BB manda como padrão 7 posições, mas é possível solicitar um convênio com 6 posições na carteira 31
            //        case 5:
            //            if (boleto.NossoNumero.Length > 10)
            //                throw new NotImplementedException(string.Format("Banco do Brasil - Para a carteira {0}, a quantidade máxima são de 12 de posições para o nosso número", boleto.Carteira));
            //            boleto.NossoNumero = Utils.FormatCode(boleto.NossoNumero, 10);
            //            break;
            //        case 6:
            //            if (boleto.NossoNumero.Length > 10)
            //                throw new NotImplementedException(string.Format("Banco do Brasil - Para a carteira {0}, a quantidade máxima são de 12 de posições para o nosso número", boleto.Carteira));
            //            boleto.NossoNumero = Utils.FormatCode(boleto.NossoNumero, 10);
            //            break;
            //        case 7:
            //            if (boleto.NossoNumero.Length > 17)
            //                throw new NotImplementedException(string.Format("Banco do Brasil - Para a carteira {0}, a quantidade máxima são de 10 de posições para o nosso número", boleto.Carteira));
            //            boleto.NossoNumero = string.Format("{0}{1}", boleto.Cedente.Convenio, Utils.FormatCode(boleto.NossoNumero, 10));
            //            break;
            //        default:
            //            throw new NotImplementedException(string.Format("Banco do Brasil - Para a carteira {0}, o número do convênio deve ter 6 ou 7 posições", boleto.Carteira));
            //    }
            //}
            //#endregion Carteira 31

            #endregion

            #region Agência e Conta Corrente
            //Verificar se a Agencia esta correta
            if (boleto.Cedente.ContaBancaria.Agencia.Length > 4)
                throw new NotImplementedException("Banco do Brasil - A quantidade máxima de caracteres da Agência são de 04 números. A conta atual está com " + boleto.Cedente.ContaBancaria.Agencia.Length + " caracteres. Número da agência: " + boleto.Cedente.ContaBancaria.Agencia);
            else if (boleto.Cedente.ContaBancaria.Agencia.Length < 4)
                boleto.Cedente.ContaBancaria.Agencia = Utils.FormatCode(boleto.Cedente.ContaBancaria.Agencia, 4);

            //Verificar se a Conta esta correta
            /*
            if (boleto.Cedente.ContaBancaria.Conta.Length > 8)
                throw new NotImplementedException("A quantidade de dígitos da Conta " + boleto.Cedente.ContaBancaria.Conta + ", são de 8 números.");
            else if (boleto.Cedente.ContaBancaria.Conta.Length < 8)
                boleto.Cedente.ContaBancaria.Conta = Utils.FormatCode(boleto.Cedente.ContaBancaria.Conta, 8);
            */
            #endregion Agência e Conta Corrente

            //Atribui o nome do banco ao local de pagamento
            boleto.LocalPagamento = "PAGÁVEL EM QUALQUER BANCO ATÉ O VENCIMENTO";

            //Verifica se data do processamento é valida
            //if (boleto.DataProcessamento.ToString("dd/MM/yyyy") == "01/01/0001")
            if (boleto.DataProcessamento == DateTime.MinValue)
                boleto.DataProcessamento = DateTime.Now;

            //Verifica se data do documento é valida
            //if (boleto.DataDocumento.ToString("dd/MM/yyyy") == "01/01/0001")
            if (boleto.DataDocumento == DateTime.MinValue)
                boleto.DataDocumento = DateTime.Now;

            boleto.QuantidadeMoeda = 0;

            FormataCodigoBarra(boleto);
            FormataLinhaDigitavel(boleto);
            FormataNossoNumero(boleto);
        }

        # endregion

        #region Métodos de formatação do boleto

        public override void FormataCodigoBarra(Boleto boleto)
        {
            var valorBoleto = "";

            if (boleto.SegundaVia)
                valorBoleto = Utils.FormatCode(boleto.ValorCobrado.ToString("f").Replace(",", "").Replace(".", ""), 10);//10
            else
                valorBoleto = Utils.FormatCode(boleto.ValorBoleto.ToString("f").Replace(",", "").Replace(".", ""), 10);//10

            #region Comentarios

            //#region Carteira 11
            //if (boleto.Carteira.Equals("11"))
            //{
            //    if (boleto.Cedente.Convenio.ToString().Length == 6)
            //    {
            //        if (boleto.TipoModalidade.Equals("21"))
            //            boleto.CodigoBarra.Codigo = string.Format("{0}{1}{2}{3}{4}{5}{6}",
            //                Utils.FormatCode(Codigo.ToString(), 3),
            //                boleto.Moeda,
            //                FatorVencimento(boleto),
            //                valorBoleto,
            //                boleto.Cedente.Convenio,
            //                boleto.NossoNumero,
            //                "21");
            //    }
            //    else
            //    {
            //        boleto.CodigoBarra.Codigo = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}",
            //            Utils.FormatCode(Codigo.ToString(), 3),
            //            boleto.Moeda,
            //            FatorVencimento(boleto),
            //            valorBoleto,
            //            boleto.NossoNumero,
            //            boleto.Cedente.ContaBancaria.Agencia,
            //            boleto.Cedente.ContaBancaria.Conta,
            //            boleto.Carteira);
            //    }
            //}
            //#endregion Carteira 11

            //#region Carteira 16
            //if (boleto.Carteira.Equals("16"))
            //{
            //    if (boleto.Cedente.Convenio.ToString().Length == 6)
            //    {
            //            boleto.CodigoBarra.Codigo = string.Format("{0}{1}{2}{3}{4}{5}{6}",
            //               Utils.FormatCode(Codigo.ToString(), 3),
            //               boleto.Moeda,
            //               FatorVencimento(boleto),
            //               valorBoleto,
            //               boleto.Cedente.Convenio,
            //               boleto.NossoNumero,
            //               "21");
            //    }
            //    else
            //    {
            //        boleto.CodigoBarra.Codigo = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}",
            //            Utils.FormatCode(Codigo.ToString(), 3),
            //            boleto.Moeda,
            //            FatorVencimento(boleto),
            //            valorBoleto,
            //            boleto.NossoNumero,
            //            boleto.Cedente.ContaBancaria.Agencia,
            //            boleto.Cedente.ContaBancaria.Conta,
            //            boleto.Carteira);
            //    }
            //}
            //#endregion Carteira 16

            //#region Carteira 17
            //if (boleto.Carteira.Equals("17"))
            //{
            //    if (boleto.Cedente.Convenio.ToString().Length == 7)
            //    {
            //        boleto.CodigoBarra.Codigo = string.Format("{0}{1}{2}{3}{4}{5}{6}",
            //            Utils.FormatCode(Codigo.ToString(), 3),
            //            boleto.Moeda,
            //            FatorVencimento(boleto),
            //            valorBoleto,
            //            "000000",
            //            boleto.NossoNumero,
            //            Utils.FormatCode(LimparCarteira(boleto.Carteira), 2));
            //    }
            //    else if (boleto.Cedente.Convenio.ToString().Length == 6)
            //    {
            //        boleto.CodigoBarra.Codigo = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}",
            //            Utils.FormatCode(Codigo.ToString(), 3),
            //            boleto.Moeda,
            //            FatorVencimento(boleto),
            //            valorBoleto,
            //            Strings.Mid(boleto.NossoNumero, 1, 11),
            //            boleto.Cedente.ContaBancaria.Agencia,
            //            boleto.Cedente.ContaBancaria.Conta,
            //            boleto.Carteira);
            //    }
            //    else
            //    {
            //        boleto.CodigoBarra.Codigo = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}",
            //            Utils.FormatCode(Codigo.ToString(), 3),
            //            boleto.Moeda,
            //            FatorVencimento(boleto),
            //            valorBoleto,
            //            boleto.NossoNumero,
            //            boleto.Cedente.ContaBancaria.Agencia,
            //            boleto.Cedente.ContaBancaria.Conta,
            //            boleto.Carteira);
            //    }
            //}
            //#endregion Carteira 17

            //#region Carteira 17-019
            //if (boleto.Carteira.Equals("17-019"))
            //{
            //    if (boleto.Cedente.Convenio.ToString().Length == 7)
            //    {
            //        boleto.CodigoBarra.Codigo = string.Format("{0}{1}{2}{3}{4}{5}{6}",
            //            Utils.FormatCode(Codigo.ToString(), 3),
            //            boleto.Moeda,
            //            FatorVencimento(boleto),
            //            valorBoleto,
            //            "000000",
            //            boleto.NossoNumero,
            //            Utils.FormatCode(LimparCarteira(boleto.Carteira), 2));
            //    }
            //    else if (boleto.Cedente.Convenio.ToString().Length == 6)
            //    {
            //        boleto.CodigoBarra.Codigo = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}",
            //                Utils.FormatCode(Codigo.ToString(), 3),
            //                boleto.Moeda,
            //                FatorVencimento(boleto),
            //                valorBoleto,
            //                boleto.NossoNumero,
            //                boleto.Cedente.ContaBancaria.Agencia,
            //                boleto.Cedente.ContaBancaria.Conta,
            //                LimparCarteira(boleto.Carteira));
            //    }
            //    else if (boleto.Cedente.Convenio.ToString().Length == 4)
            //    {
            //        boleto.CodigoBarra.Codigo = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}",
            //            Utils.FormatCode(Codigo.ToString(), 3),
            //            boleto.Moeda,
            //            FatorVencimento(boleto),
            //            valorBoleto,
            //            boleto.NossoNumero,
            //            boleto.Cedente.ContaBancaria.Agencia,
            //            boleto.Cedente.ContaBancaria.Conta,
            //            LimparCarteira(boleto.Carteira));
            //    }
            //}
            //#endregion Carteira 17-019

            //#region Carteira 18
            //if (boleto.Carteira.Equals("18"))
            //{
            //    boleto.CodigoBarra.Codigo = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}",
            //        Utils.FormatCode(Codigo.ToString(), 3),
            //        boleto.Moeda,
            //        FatorVencimento(boleto),
            //        valorBoleto,
            //        boleto.NossoNumero,
            //        boleto.Cedente.ContaBancaria.Agencia,
            //        boleto.Cedente.ContaBancaria.Conta,
            //        boleto.Carteira);
            //}
            //#endregion Carteira 18

            //#region Carteira 18-019
            //if (boleto.Carteira.Equals("18-019"))
            //{
            //    if (boleto.Cedente.Convenio.ToString().Length == 7)
            //    {
            //        #region Especificação Convênio 7 posições
            //        /*
            //        Posição     Tamanho     Picture     Conteúdo
            //        01 a 03         03      9(3)            Código do Banco na Câmara de Compensação = ‘001’
            //        04 a 04         01      9(1)            Código da Moeda = '9'
            //        05 a 05         01      9(1)            DV do Código de Barras (Anexo 10)
            //        06 a 09         04      9(04)           Fator de Vencimento (Anexo 8)
            //        10 a 19         10      9(08)           V(2) Valor
            //        20 a 25         06      9(6)            Zeros
            //        26 a 42         17      9(17)           Nosso-Número, sem o DV
            //        26 a 32         9       (7)             Número do Convênio fornecido pelo Banco (CCCCCCC)
            //        33 a 42         9       (10)            Complemento do Nosso-Número, sem DV (NNNNNNNNNN)
            //        43 a 44         02      9(2)            Tipo de Carteira/Modalidade de Cobrança
            //         */
            //        #endregion Especificação Convênio 7 posições

            //        boleto.CodigoBarra.Codigo = string.Format("{0}{1}{2}{3}{4}{5}{6}",
            //            Utils.FormatCode(Codigo.ToString(), 3),
            //            boleto.Moeda,
            //            FatorVencimento(boleto),
            //            valorBoleto,
            //            "000000",
            //            boleto.NossoNumero,
            //            Utils.FormatCode(LimparCarteira(boleto.Carteira), 2));
            //    }
            //    else if (boleto.Cedente.Convenio.ToString().Length == 6)
            //    {
            //        if (boleto.TipoModalidade.Equals("21"))
            //            boleto.CodigoBarra.Codigo = string.Format("{0}{1}{2}{3}{4}{5}{6}",
            //                Utils.FormatCode(Codigo.ToString(), 3),
            //                boleto.Moeda,
            //                FatorVencimento(boleto),
            //                valorBoleto,
            //                boleto.Cedente.Convenio,
            //                boleto.NossoNumero,
            //                "21");
            //        else
            //            boleto.CodigoBarra.Codigo = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}",
            //                Utils.FormatCode(Codigo.ToString(), 3),
            //                boleto.Moeda,
            //                FatorVencimento(boleto),
            //                valorBoleto,
            //                boleto.NossoNumero,
            //                boleto.Cedente.ContaBancaria.Agencia,
            //                boleto.Cedente.ContaBancaria.Conta,
            //                LimparCarteira(boleto.Carteira));
            //    }
            //    else if (boleto.Cedente.Convenio.ToString().Length == 4)
            //    {
            //        boleto.CodigoBarra.Codigo = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}",
            //            Utils.FormatCode(Codigo.ToString(), 3),
            //            boleto.Moeda,
            //            FatorVencimento(boleto),
            //            valorBoleto,
            //            boleto.NossoNumero,
            //            boleto.Cedente.ContaBancaria.Agencia,
            //            boleto.Cedente.ContaBancaria.Conta,
            //            LimparCarteira(boleto.Carteira));
            //    }
            //}
            //#endregion Carteira 18-019

            //#region Carteira 18-027
            //if (boleto.Carteira.Equals("18-027"))
            //{
            //    if (boleto.Cedente.Convenio.ToString().Length == 7)
            //    {
            //        #region Especificação Convênio 7 posições
            //        /*
            //        Posição     Tamanho     Picture     Conteúdo
            //        01 a 03         03      9(3)            Código do Banco na Câmara de Compensação = ‘001’
            //        04 a 04         01      9(1)            Código da Moeda = '9'
            //        05 a 05         01      9(1)            DV do Código de Barras (Anexo 10)
            //        06 a 09         04      9(04)           Fator de Vencimento (Anexo 8)
            //        10 a 19         10      9(08)           V(2) Valor
            //        20 a 25         06      9(6)            Zeros
            //        26 a 42         17      9(17)           Nosso-Número, sem o DV
            //        26 a 32         9       (7)             Número do Convênio fornecido pelo Banco (CCCCCCC)
            //        33 a 42         9       (10)            Complemento do Nosso-Número, sem DV (NNNNNNNNNN)
            //        43 a 44         02      9(2)            Tipo de Carteira/Modalidade de Cobrança
            //         */
            //        #endregion Especificação Convênio 7 posições

            //        boleto.CodigoBarra.Codigo = string.Format("{0}{1}{2}{3}{4}{5}{6}",
            //            Utils.FormatCode(Codigo.ToString(), 3),
            //            boleto.Moeda,
            //            FatorVencimento(boleto).ToString("0000"),
            //            valorBoleto,
            //            "000000",
            //            boleto.NossoNumero,
            //            Utils.FormatCode(LimparCarteira(boleto.Carteira), 2));
            //    }
            //    else if (boleto.Cedente.Convenio.ToString().Length == 6)
            //    {
            //        if (boleto.TipoModalidade.Equals("21"))
            //            boleto.CodigoBarra.Codigo = string.Format("{0}{1}{2}{3}{4}{5}{6}",
            //                Utils.FormatCode(Codigo.ToString(), 3),
            //                boleto.Moeda,
            //                FatorVencimento(boleto),
            //                valorBoleto,
            //                boleto.Cedente.Convenio,
            //                boleto.NossoNumero,
            //                "21");
            //        else
            //            boleto.CodigoBarra.Codigo = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}",
            //                Utils.FormatCode(Codigo.ToString(), 3),
            //                boleto.Moeda,
            //                FatorVencimento(boleto),
            //                valorBoleto,
            //                boleto.NossoNumero,
            //                boleto.Cedente.ContaBancaria.Agencia,
            //                boleto.Cedente.ContaBancaria.Conta,
            //                LimparCarteira(boleto.Carteira));
            //    }
            //    else if (boleto.Cedente.Convenio.ToString().Length == 4)
            //    {
            //        boleto.CodigoBarra.Codigo = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}",
            //            Utils.FormatCode(Codigo.ToString(), 3),
            //            boleto.Moeda,
            //            FatorVencimento(boleto),
            //            valorBoleto,
            //            boleto.NossoNumero,
            //            boleto.Cedente.ContaBancaria.Agencia,
            //            boleto.Cedente.ContaBancaria.Conta,
            //            LimparCarteira(boleto.Carteira));
            //    }
            //}
            //#endregion Carteira 18-027

            //#region Carteira 18-035
            //if (boleto.Carteira.Equals("18-035"))
            //{
            //    if (boleto.Cedente.Convenio.ToString().Length == 7)
            //    {
            //        #region Especificação Convênio 7 posições
            //        /*
            //        Posição     Tamanho     Picture     Conteúdo
            //        01 a 03         03      9(3)            Código do Banco na Câmara de Compensação = ‘001’
            //        04 a 04         01      9(1)            Código da Moeda = '9'
            //        05 a 05         01      9(1)            DV do Código de Barras (Anexo 10)
            //        06 a 09         04      9(04)           Fator de Vencimento (Anexo 8)
            //        10 a 19         10      9(08)           V(2) Valor
            //        20 a 25         06      9(6)            Zeros
            //        26 a 42         17      9(17)           Nosso-Número, sem o DV
            //        26 a 32         9       (7)             Número do Convênio fornecido pelo Banco (CCCCCCC)
            //        33 a 42         9       (10)            Complemento do Nosso-Número, sem DV (NNNNNNNNNN)
            //        43 a 44         02      9(2)            Tipo de Carteira/Modalidade de Cobrança
            //         */
            //        #endregion Especificação Convênio 7 posições

            //        boleto.CodigoBarra.Codigo = string.Format("{0}{1}{2}{3}{4}{5}{6}",
            //            Utils.FormatCode(Codigo.ToString(), 3),
            //            boleto.Moeda,
            //            FatorVencimento(boleto).ToString("0000"),
            //            valorBoleto,
            //            "000000",
            //            boleto.NossoNumero,
            //            Utils.FormatCode(LimparCarteira(boleto.Carteira), 2));
            //    }
            //    else if (boleto.Cedente.Convenio.ToString().Length == 6)
            //    {
            //        if (boleto.TipoModalidade.Equals("21"))
            //            boleto.CodigoBarra.Codigo = string.Format("{0}{1}{2}{3}{4}{5}{6}",
            //                Utils.FormatCode(Codigo.ToString(), 3),
            //                boleto.Moeda,
            //                FatorVencimento(boleto),
            //                valorBoleto,
            //                boleto.Cedente.Convenio,
            //                boleto.NossoNumero,
            //                "21");
            //        else
            //            boleto.CodigoBarra.Codigo = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}",
            //                Utils.FormatCode(Codigo.ToString(), 3),
            //                boleto.Moeda,
            //                FatorVencimento(boleto),
            //                valorBoleto,
            //                boleto.NossoNumero,
            //                boleto.Cedente.ContaBancaria.Agencia,
            //                boleto.Cedente.ContaBancaria.Conta,
            //                LimparCarteira(boleto.Carteira));
            //    }
            //    else if (boleto.Cedente.Convenio.ToString().Length == 4)
            //    {
            //        boleto.CodigoBarra.Codigo = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}",
            //            Utils.FormatCode(Codigo.ToString(), 3),
            //            boleto.Moeda,
            //            FatorVencimento(boleto),
            //            valorBoleto,
            //            boleto.NossoNumero,
            //            boleto.Cedente.ContaBancaria.Agencia,
            //            boleto.Cedente.ContaBancaria.Conta,
            //            LimparCarteira(boleto.Carteira));
            //    }
            //}
            //#endregion Carteira 18-035

            //#region Carteira 18-140
            //if (boleto.Carteira.Equals("18-140"))
            //{
            //    if (boleto.Cedente.Convenio.ToString().Length == 7)
            //    {
            //        #region Especificação Convênio 7 posições
            //        /*
            //        Posição     Tamanho     Picture     Conteúdo
            //        01 a 03         03      9(3)            Código do Banco na Câmara de Compensação = ‘001’
            //        04 a 04         01      9(1)            Código da Moeda = '9'
            //        05 a 05         01      9(1)            DV do Código de Barras (Anexo 10)
            //        06 a 09         04      9(04)           Fator de Vencimento (Anexo 8)
            //        10 a 19         10      9(08)           V(2) Valor
            //        20 a 25         06      9(6)            Zeros
            //        26 a 42         17      9(17)           Nosso-Número, sem o DV
            //        26 a 32         9       (7)             Número do Convênio fornecido pelo Banco (CCCCCCC)
            //        33 a 42         9       (10)            Complemento do Nosso-Número, sem DV (NNNNNNNNNN)
            //        43 a 44         02      9(2)            Tipo de Carteira/Modalidade de Cobrança
            //         */
            //        #endregion Especificação Convênio 7 posições

            //        boleto.CodigoBarra.Codigo = string.Format("{0}{1}{2}{3}{4}{5}{6}",
            //            Utils.FormatCode(Codigo.ToString(), 3),
            //            boleto.Moeda,
            //            FatorVencimento(boleto).ToString("0000"),
            //            valorBoleto,
            //            "000000",
            //            boleto.NossoNumero,
            //            Utils.FormatCode(LimparCarteira(boleto.Carteira), 2));
            //    }
            //    else if (boleto.Cedente.Convenio.ToString().Length == 6)
            //    {
            //        if (boleto.TipoModalidade.Equals("21"))
            //            boleto.CodigoBarra.Codigo = string.Format("{0}{1}{2}{3}{4}{5}{6}",
            //                Utils.FormatCode(Codigo.ToString(), 3),
            //                boleto.Moeda,
            //                FatorVencimento(boleto),
            //                valorBoleto,
            //                boleto.Cedente.Convenio,
            //                boleto.NossoNumero,
            //                "21");
            //        else
            //            boleto.CodigoBarra.Codigo = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}",
            //                Utils.FormatCode(Codigo.ToString(), 3),
            //                boleto.Moeda,
            //                FatorVencimento(boleto),
            //                valorBoleto,
            //                boleto.NossoNumero,
            //                boleto.Cedente.ContaBancaria.Agencia,
            //                boleto.Cedente.ContaBancaria.Conta,
            //                LimparCarteira(boleto.Carteira));
            //    }
            //    else if (boleto.Cedente.Convenio.ToString().Length == 4)
            //    {
            //        boleto.CodigoBarra.Codigo = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}",
            //            Utils.FormatCode(Codigo.ToString(), 3),
            //            boleto.Moeda,
            //            FatorVencimento(boleto),
            //            valorBoleto,
            //            boleto.NossoNumero,
            //            boleto.Cedente.ContaBancaria.Agencia,
            //            boleto.Cedente.ContaBancaria.Conta,
            //            LimparCarteira(boleto.Carteira));
            //    }
            //}
            //#endregion Carteira 18-140

            //#region Carteira 31
            //if (boleto.Carteira.Equals("31"))
            //{
            //    boleto.CodigoBarra.Codigo = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}",
            //        Utils.FormatCode(Codigo.ToString(), 3),
            //        boleto.Moeda,
            //        FatorVencimento(boleto),
            //        valorBoleto,
            //        boleto.NossoNumero,
            //        boleto.Cedente.ContaBancaria.Agencia,
            //        boleto.Cedente.ContaBancaria.Conta,
            //        boleto.Carteira);
            //}
            //#endregion Carteira 31

            #endregion

            var _nossoNumero = "";
            if (boleto.NossoNumero.Contains("/"))
                _nossoNumero = boleto.NossoNumero.Substring(3, boleto.NossoNumero.Length - 3);
            else
                _nossoNumero = boleto.NossoNumero;

            if (boleto.Carteira.Equals("16"))
            {
                if (_nossoNumero.Length < 5)
                    _nossoNumero = Utils.FormatCode(_nossoNumero, "0", 5);
                else
                    _nossoNumero = _nossoNumero.Substring(0, 5);
            }

            if (boleto.Cedente.Convenio.ToString().Length == 7)
            {
                boleto.CodigoBarra.Codigo = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}",
                    Utils.FormatCode(Codigo.ToString(), 3),
                    boleto.Moeda,
                    FatorVencimento(boleto),
                    valorBoleto,
                    "000000",
                    boleto.Cedente.Convenio,
                    _nossoNumero,
                    Utils.FormatCode(LimparCarteira(boleto.Carteira), "0", 2, true));
            }
            else if (boleto.Cedente.Convenio.ToString().Length == 6)
            {
                if (boleto.Carteira.Equals("16"))
                {
                    boleto.CodigoBarra.Codigo = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}",
                          Utils.FormatCode(Codigo.ToString(), 3),
                          boleto.Moeda,
                          FatorVencimento(boleto),
                          valorBoleto,
                          boleto.Cedente.Convenio,
                          _nossoNumero,
                          boleto.Cedente.ContaBancaria.Agencia,
                          boleto.Cedente.ContaBancaria.Conta,
                          "21");
                }
                else
                {
                    boleto.CodigoBarra.Codigo = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}",
                            Utils.FormatCode(Codigo.ToString(), 3),
                            boleto.Moeda,
                            FatorVencimento(boleto),
                            valorBoleto,
                            _nossoNumero.Substring(0, _nossoNumero.Length - 2),
                            boleto.Cedente.ContaBancaria.Agencia,
                            boleto.Cedente.ContaBancaria.Conta,
                            Utils.FormatCode(LimparCarteira(boleto.Carteira), "0", 2, true));
                }
            }
            else if (boleto.Cedente.Convenio.ToString().Length == 4)
            {
                boleto.CodigoBarra.Codigo = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}",
                    Utils.FormatCode(Codigo.ToString(), 3),
                    boleto.Moeda,
                    FatorVencimento(boleto),
                    valorBoleto,
                    _nossoNumero.Substring(0, _nossoNumero.Length - 2),
                    boleto.Cedente.ContaBancaria.Agencia,
                    boleto.Cedente.ContaBancaria.Conta,
                    Utils.FormatCode(LimparCarteira(boleto.Carteira), "0", 2, true));
            }

            _dacBoleto = Mod11(boleto.CodigoBarra.Codigo, 9);

            boleto.CodigoBarra.Codigo = Strings.Left(boleto.CodigoBarra.Codigo, 4) + _dacBoleto + Strings.Right(boleto.CodigoBarra.Codigo, 39);
        }

        public override void FormataLinhaDigitavel(Boleto boleto)
        {
            string cmplivre = string.Empty;
            string campo1 = string.Empty;
            string campo2 = string.Empty;
            string campo3 = string.Empty;
            string campo4 = string.Empty;
            string campo5 = string.Empty;
            long icampo5 = 0;
            int digitoMod = 0;

            /*
            Campos 1 (AAABC.CCCCX):
            A = Código do Banco na Câmara de Compensação “001”
            B = Código da moeda "9" (*)
            C = Posição 20 a 24 do código de barras
            X = DV que amarra o campo 1 (Módulo 10, contido no Anexo 7)
             */

            cmplivre = Strings.Mid(boleto.CodigoBarra.Codigo, 20, 25);

            campo1 = Strings.Left(boleto.CodigoBarra.Codigo, 4) + Strings.Mid(cmplivre, 1, 5);
            digitoMod = Mod10(campo1);
            campo1 = campo1 + digitoMod.ToString();
            campo1 = Strings.Mid(campo1, 1, 5) + "." + Strings.Mid(campo1, 6, 5);
            /*
            Campo 2 (DDDDD.DDDDDY)
            D = Posição 25 a 34 do código de barras
            Y = DV que amarra o campo 2 (Módulo 10, contido no Anexo 7)
             */
            campo2 = Strings.Mid(cmplivre, 6, 10);
            digitoMod = Mod10(campo2);
            campo2 = campo2 + digitoMod.ToString();
            campo2 = Strings.Mid(campo2, 1, 5) + "." + Strings.Mid(campo2, 6, 6);


            /*
            Campo 3 (EEEEE.EEEEEZ)
            E = Posição 35 a 44 do código de barras
            Z = DV que amarra o campo 3 (Módulo 10, contido no Anexo 7)
             */
            campo3 = Strings.Mid(cmplivre, 16, 10);
            digitoMod = Mod10(campo3);
            campo3 = campo3 + digitoMod;
            campo3 = Strings.Mid(campo3, 1, 5) + "." + Strings.Mid(campo3, 6, 6);

            /*
            Campo 4 (K)
            K = DV do Código de Barras (Módulo 11, contido no Anexo 10)
             */
            campo4 = Strings.Mid(boleto.CodigoBarra.Codigo, 5, 1);

            /*
            Campo 5 (UUUUVVVVVVVVVV)
            U = Fator de Vencimento ( Anexo 10)
            V = Valor do Título (*)
             */
            icampo5 = Convert.ToInt64(Strings.Mid(boleto.CodigoBarra.Codigo, 6, 14));

            if (icampo5 == 0)
                campo5 = "000";
            else
                campo5 = icampo5.ToString();

            boleto.CodigoBarra.LinhaDigitavel = campo1 + " " + campo2 + " " + campo3 + " " + campo4 + " " + campo5;
        }

        public override void FormataNossoNumero(Boleto boleto)
        {
            #region Nosso Número
            /*
             * Convênio de 7 posições
             * Nosso Número com 17 posições
             */
            if (boleto.Cedente.Convenio.ToString().Length == 7)
            {
                if (boleto.NossoNumero.Length > 10)
                    boleto.NossoNumero = boleto.NossoNumero.Substring(boleto.NossoNumero.Length - 10, 10);

                boleto.NossoNumero = string.Format("{0}{1}", boleto.Cedente.Convenio, Utils.FormatCode(boleto.NossoNumero, "0", 10, true));
            }
            /*
             * Convênio de 6 posições
             * Nosso Número com 11 posições
             */
            else if (boleto.Cedente.Convenio.ToString().Length == 6)
            {
                if (!boleto.Carteira.Equals("16") && !boleto.Carteira.Equals("18"))
                {
                    if (boleto.NossoNumero.Length > 5)
                        boleto.NossoNumero = boleto.NossoNumero.Substring(boleto.NossoNumero.Length - 5, 5);

                    boleto.NossoNumero = string.Format("{0}{1}", boleto.Cedente.Convenio, Utils.FormatCode(boleto.NossoNumero, "0", 5, true));

                    var _dacNossoNumero = Mod11Peso2a9(boleto.NossoNumero);
                    boleto.NossoNumero = string.Format("{0}-{1}", boleto.NossoNumero, _dacNossoNumero);
                }
            }
            /*
              * Convênio de 4 posições
              * Nosso Número com 11 posições
              */
            else if (boleto.Cedente.Convenio.ToString().Length == 4)
            {
                if (boleto.NossoNumero.Length > 7)
                    boleto.NossoNumero = boleto.NossoNumero.Substring(boleto.NossoNumero.Length - 7, 7);

                boleto.NossoNumero = string.Format("{0}{1}", boleto.Cedente.Convenio, Utils.FormatCode(boleto.NossoNumero, "0", 7, true));

                var _dacNossoNumero = Mod11Peso2a9(boleto.NossoNumero);
                boleto.NossoNumero = string.Format("{0}-{1}", boleto.NossoNumero, _dacNossoNumero);
            }
            else
                boleto.NossoNumero = Utils.FormatCode(boleto.NossoNumero, "0", 11, true);
            #endregion

            if (boleto.Cedente.Convenio.ToString().Length == 6) //somente monta o digito verificador no nosso numero se o convenio tiver 6 posições
            {
                switch (boleto.Carteira)
                {
                    case "18-019":
                        boleto.NossoNumero = string.Format("{0}/{1}-{2}", LimparCarteira(boleto.Carteira), boleto.NossoNumero, Mod11BancoBrasil(boleto.NossoNumero));
                        return;
                    case "17-019":
                        boleto.NossoNumero = string.Format("{0}/{1}", LimparCarteira(boleto.Carteira), boleto.NossoNumero);
                        return;
                }
            }

            switch (boleto.Carteira)
            {
                case "17-019":
                case "18-019":
                    boleto.NossoNumero = string.Format("{0}/{1}", LimparCarteira(boleto.Carteira), boleto.NossoNumero);
                    return;
                case "31":
                    boleto.NossoNumero = string.Format("{0}{1}", Utils.FormatCode(boleto.Cedente.Convenio.ToString(), 7), boleto.NossoNumero);
                    return;
                case "16":
                case "18":
                    boleto.NossoNumero = string.Format("{0}", Utils.FormatCode(boleto.NossoNumero, "0", 17, true));
                    return;
            }

            boleto.NossoNumero = string.Format("{0}", boleto.NossoNumero);

        }

        public override void FormataNumeroDocumento(Boleto boleto)
        {
        }

        internal static string Mod11BancoBrasil(string value)
        {
            #region Trecho do manual DVMD11.doc
            /* 
            Multiplicar cada algarismo que compõe o número pelo seu respectivo multiplicador (PESO).
            Os multiplicadores(PESOS) variam de 9 a 2.
            O primeiro dígito da direita para a esquerda deverá ser multiplicado por 9, o segundo por 8 e assim sucessivamente.
            O resultados das multiplicações devem ser somados:
            72+35+24+27+4+9+8=179
            O total da soma deverá ser dividido por 11:
            179 / 11=16
            RESTO=3

            Se o resto da divisão for igual a 10 o D.V. será igual a X. 
            Se o resto da divisão for igual a 0 o D.V. será igual a 0.
            Se o resto for menor que 10, o D.V.  será igual ao resto.

            No exemplo acima, o dígito verificador será igual a 3
            */
            #endregion

            /* d - Dígito
             * s - Soma
             * p - Peso
             * b - Base
             * r - Resto
             */

            string d;
            int s = 0, p = 9, b = 2;

            for (int i = value.Length - 1; i >= 0; i--)
            {
                s += (int.Parse(value[i].ToString()) * p);
                if (p == b)
                    p = 9;
                else
                    p--;
            }

            int r = (s % 11);
            if (r == 10)
                d = "X";
            else if (r == 0)
                d = "0";
            else
                d = r.ToString();

            return d;
        }

        private string LimparCarteira(string carteira)
        {
            return carteira.Split('-')[0];
        }
        # endregion

        #region Métodos de geração do arquivo remessa

        public override string GerarHeaderLoteRemessa(string numeroConvenio, Cedente cedente, int numeroArquivoRemessa, TipoArquivo tipoArquivo)
        {
            try
            {
                string header = " ";

                base.GerarHeaderLoteRemessa(numeroConvenio, cedente, numeroArquivoRemessa, tipoArquivo);

                switch (tipoArquivo)
                {
                    case TipoArquivo.CNAB240:
                        header = GerarHeaderLoteRemessaCNAB240(numeroConvenio, cedente, numeroArquivoRemessa);
                        break;
                    case TipoArquivo.CNAB400:
                        header = "";
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

        public override string GerarDetalheRemessaTipo5(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
        {
            try
            {
                string _detalhe = " ";

                base.GerarDetalheRemessaTipo5(boleto, numeroRegistro, tipoArquivo);

                switch (tipoArquivo)
                {
                    case TipoArquivo.CNAB400:
                        _detalhe = GerarDetalheRemessaTipo5CNAB400(boleto, numeroRegistro, tipoArquivo);
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
                        _trailer = GerarTrailerRemessa400(numeroRegistro, 0);
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

        public override string GerarHeaderRemessa(string numeroConvenio, Cedente cedente, TipoArquivo tipoArquivo, int numeroArquivoRemessa, Boleto boletos)
        {
            throw new NotImplementedException("Função não implementada.");
        }
        #endregion

        #region Método de geração de arquivo de remessa CNAB400
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
                    #region Validações da Remessa que deverão estar preenchidas quando BANCO DO BRASIL
                    if (String.IsNullOrEmpty(boleto.Remessa.TipoDocumento))
                    {
                        vMsg += String.Concat("Boleto: ", boleto.NumeroDocumento, "; Remessa: Informe o Tipo Documento!", Environment.NewLine);
                        vRetorno = false;
                    }
                    #endregion
                }
                #endregion
            }
            //
            mensagem = vMsg;
            return vRetorno;
        }

        #region HEADER
        public string GerarHeaderRemessaCNAB400(Cedente cedente, int numeroArquivoRemessa)
        {
            try
            {
                var dvAgencia = cedente.ContaBancaria.DigitoAgencia == null ? "0" : cedente.ContaBancaria.DigitoAgencia;

                var _header = "";

                _header += "0";                                                                                                 //001-001
                _header += "1";                                                                                                 //002-002
                _header += "REMESSA";                                                                                           //003-009
                _header += "01";                                                                                                //010-011
                _header += "COBRANCA";                                                                                          //012-019
                _header += Utils.FormatCode("", " ", 7);                                                                        //020-026
                _header += Utils.FormatCode(cedente.ContaBancaria.Agencia, "0", 4, true);                                       //027-030
                _header += Utils.FormatCode(dvAgencia, "0", 1, true);                                                           //031-031
                _header += Utils.FormatCode(cedente.ContaBancaria.Conta, "0", 8, true);                                         //032-039
                _header += Utils.FormatCode(cedente.ContaBancaria.DigitoConta, "0", 1, true);                                   //040-040

                if (cedente.Convenio.ToString().Length == 7)
                {
                    _header += "000000";                                                                                        //041-046
                    _header += Utils.FitStringLength(cedente.Nome.ToUpper(), 30, 30, ' ', 0, true, true, false);                //047-076
                    _header += Utils.FitStringLength("001BANCODOBRASIL", 18, 18, ' ', 0, true, true, false);                    //077-094
                    _header += DateTime.Now.ToString("ddMMyy");                                                                 //095-100
                    _header += Utils.FormatCode(numeroArquivoRemessa.ToString(), "0", 7, true);                                 //101-107
                    _header += Utils.FormatCode("", " ", 22);                                                                   //108-129
                    _header += Utils.FormatCode(cedente.Convenio.ToString(), "0", 7, true);                                     //130-136
                    _header += Utils.FormatCode("", " ", 258);                                                                  //137-394
                    _header += "000001";                                                                                        //395-400
                }
                else
                {
                    _header += Utils.FormatCode(cedente.Convenio.ToString(), "0", 6, true);                                     //041-046
                    _header += Utils.FitStringLength(cedente.Nome.ToUpper(), 30, 30, ' ', 0, true, true, false);                //047-076
                    _header += Utils.FitStringLength("001BANCODOBRASIL", 18, 18, ' ', 0, true, true, false);                    //077-094
                    _header += DateTime.Now.ToString("ddMMyy");                                                                 //095-100
                    _header += Utils.FormatCode(numeroArquivoRemessa.ToString(), "0", 7, true);                                 //101-107
                    _header += Utils.FormatCode("", " ", 287);                                                                  //108-394
                    _header += "000001";                                                                                        //395-400
                }

                _header = Utils.SubstituiCaracteresEspeciais(_header);

                return _header;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar HEADER do arquivo de remessa do CNAB400.", ex);
            }
        }
        #endregion

        #region DETALHER
        public string GerarDetalheRemessaCNAB400(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
        {
            try
            {
                base.GerarDetalheRemessa(boleto, numeroRegistro, tipoArquivo);
                this.FormataNossoNumero(boleto);

                var dvAgencia = boleto.Cedente.ContaBancaria.DigitoAgencia == null ? "0" : boleto.Cedente.ContaBancaria.DigitoAgencia;

                var _detalhe = "";

                #region Regra Por Tamanho de Convênio
                if (boleto.Cedente.Convenio.ToString().Length.Equals(7))
                    _detalhe += Utils.FormatCode("7", "0", 1);
                else
                    _detalhe += Utils.FormatCode("1", "0", 1);
                #endregion
                #region Regra Tipo de Inscrição Cedente
                string vCpfCnpjEmi = "00";
                if (boleto.Cedente.CPFCNPJ.Length.Equals(11)) vCpfCnpjEmi = "01"; //Cpf é sempre 11;
                else if (boleto.Cedente.CPFCNPJ.Length.Equals(14)) vCpfCnpjEmi = "02"; //Cnpj é sempre 14;
                #endregion
                _detalhe += vCpfCnpjEmi;
                _detalhe += Utils.FormatCode(boleto.Cedente.CPFCNPJ, "0", 14, true);
                _detalhe += Utils.FormatCode(boleto.Cedente.ContaBancaria.Agencia, "0", 4, true);
                _detalhe += Utils.FormatCode(dvAgencia, "0", 1, true);
                _detalhe += Utils.FormatCode(boleto.Cedente.ContaBancaria.Conta, "0", 8, true);
                _detalhe += Utils.FormatCode(boleto.Cedente.ContaBancaria.DigitoConta, "0", 1, true);
                #region Regra Convênio
                if (boleto.Cedente.Convenio.ToString().Length.Equals(7))
                    _detalhe += Utils.FormatCode(boleto.Cedente.Convenio.ToString(), "0", 7, true);
                else
                    _detalhe += Utils.FormatCode(boleto.Cedente.Convenio.ToString(), "0", 6, true);
                #endregion
                _detalhe += Utils.FormatCode(boleto.NumeroDocumento, " ", 25);
                #region Regra Nosso Número
                if (boleto.Cedente.Convenio.ToString().Length.Equals(7))
                    _detalhe += Utils.FormatCode(boleto.NossoNumero, "0", 17, true);
                else
                    _detalhe += Utils.FormatCode(boleto.NossoNumero.Replace("-", ""), "0", 12, true);
                #endregion
                _detalhe += Utils.FormatCode("", "0", 2);
                _detalhe += Utils.FormatCode("", "0", 2);
                _detalhe += Utils.FormatCode("", " ", 3);
                _detalhe += Utils.FormatCode("", " ", 1);
                _detalhe += Utils.FormatCode("", " ", 3);
                _detalhe += Utils.FormatCode(boleto.VariacaoCarteira, "0", 3, true);
                _detalhe += Utils.FormatCode("", "0", 1);
                if (boleto.Cedente.Convenio.ToString().Length.Equals(6))
                {
                    _detalhe += Utils.FormatCode("", "0", 5, true);
                    _detalhe += Utils.FormatCode("", "0", 1, true);
                }
                _detalhe += Utils.FormatCode("", "0", 6);
                _detalhe += Utils.FormatCode("", " ", 5);
                _detalhe += Utils.FormatCode(boleto.Carteira, "0", 2, true);
                _detalhe += Utils.FormatCode(boleto.CodigoOcorrencia, "0", 2, true);
                _detalhe += Utils.FormatCode(boleto.NumeroDocumento, "0", 10, true);
                _detalhe += boleto.DataVencimento.ToString("ddMMyy");
                _detalhe += Utils.FormatCode(boleto.ValorBoleto.ToString("0.00").Replace(",", ""), "0", 13, true);
                _detalhe += Utils.FormatCode("001", "0", 3);
                _detalhe += Utils.FormatCode("0000", "0", 4);
                _detalhe += Utils.FormatCode("", " ", 1);
                _detalhe += Utils.FormatCode(boleto.EspecieDocumento.Codigo.ToString(), "0", 2, true);
                _detalhe += "N";
                _detalhe += boleto.DataProcessamento.ToString("ddMMyy");
                _detalhe += Utils.FormatCode("", "0", 2);
                _detalhe += Utils.FormatCode("", "0", 2);
                _detalhe += Utils.FormatCode(boleto.PercJurosMora.ToString("0.00").Replace(",", ""), "0", 13, true);
                #region Data Inicio dos Juros Mora
                if (boleto.ValorDesconto > 0)
                    _detalhe += boleto.DataVencimento.ToString("ddMMyy");
                else
                    _detalhe += Utils.FormatCode("", "0", 6);
                #endregion
                _detalhe += Utils.FormatCode(boleto.ValorDesconto.ToString("0.00").Replace(",", ""), "0", 13, true);
                _detalhe += Utils.FormatCode("", "0", 13, true);  //IOF
                _detalhe += Utils.FormatCode("", "0", 13, true);  //Abatimentos
                #region Comentario
                //#region Instruções Conforme Código de Ocorrência...
                //if (boleto.CodigoOcorrencia.Equals("35") || boleto.CodigoOcorrencia.Equals("36"))   //“35” – Cobrar Multa – ou “36” - Dispensar Multa 
                //{
                //    #region Código de Multa e Valor/Percentual Multa
                //    string vCodigoMulta = "9"; //“9” = Dispensar Multa
                //    Decimal vMulta = 0;

                //    if (boleto.ValorMulta > 0)
                //    {
                //        vCodigoMulta = "1";    //“1” = Valor
                //        vMulta = boleto.ValorMulta;
                //    }
                //    else if (boleto.PercMulta > 0)
                //    {
                //        vCodigoMulta = "2";   //“2” = Percentual
                //        vMulta = boleto.PercMulta;
                //    }
                //    #endregion

                //    #region DataVencimento
                //    string vDataVencimento = "000000";
                //    if (!boleto.DataVencimento.Equals(DateTime.MinValue))
                //        vDataVencimento = boleto.DataVencimento.ToString("ddMMyy");
                //    #endregion

                //    reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0174, 001, 0, vCodigoMulta, '0'));                          //174 a 174      Código da Multa 1=Valor 
                //    reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediDataDDMMAA___________, 0175, 006, 0, vDataVencimento, ' '));                       //175 a 180      Data de inicio para Cobrança da Multa 
                //    reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0180, 013, 2, vMulta, '0'));                                //181 a 192      Valor de Multa 
                //}
                //else
                //{
                //    #region DataDesconto
                //    string vDataDesconto = "000000";
                //    if (!boleto.DataDesconto.Equals(DateTime.MinValue))
                //        vDataDesconto = boleto.DataDesconto.ToString("ddMMyy");
                //    #endregion
                //    reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0174, 006, 0, vDataDesconto, '0'));                             //174-179                
                //    reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0180, 013, 2, boleto.ValorDesconto, '0'));                      //180-192                
                //    reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0193, 013, 2, boleto.IOF, '0'));                                //193-205
                //}
                //#endregion
                ////
                #endregion
                #region Regra Tipo de Inscrição Sacado
                string vCpfCnpjSac = "00";
                if (boleto.Sacado.CPFCNPJ.Length.Equals(11)) vCpfCnpjSac = "01"; //Cpf é sempre 11;
                else if (boleto.Sacado.CPFCNPJ.Length.Equals(14)) vCpfCnpjSac = "02"; //Cnpj é sempre 14;
                #endregion
                _detalhe += vCpfCnpjSac;
                _detalhe += Utils.FormatCode(boleto.Sacado.CPFCNPJ, "0", 14, true);
                _detalhe += Utils.FitStringLength(boleto.Sacado.Nome.ToUpper(), 37, 37, ' ', 0, true, true, false);
                _detalhe += Utils.FormatCode("", " ", 3);
                _detalhe += Utils.FitStringLength(boleto.Sacado.Endereco.End.ToUpper(), 40, 40, ' ', 0, true, true, false);
                _detalhe += Utils.FitStringLength(boleto.Sacado.Endereco.Bairro.ToUpper(), 12, 12, ' ', 0, true, true, false);
                _detalhe += Utils.FitStringLength(boleto.Sacado.Endereco.CEP.ToUpper(), 8, 8, ' ', 0, true, true, false);
                _detalhe += Utils.FitStringLength(boleto.Sacado.Endereco.Cidade.ToUpper(), 15, 15, ' ', 0, true, true, false);
                _detalhe += Utils.FitStringLength(boleto.Sacado.Endereco.UF.ToUpper(), 2, 2, ' ', 0, true, true, false);
                _detalhe += Utils.FormatCode("", " ", 40);
                _detalhe += Utils.FormatCode("", " ", 2);
                _detalhe += Utils.FormatCode("", " ", 1);
                _detalhe += Utils.FormatCode(numeroRegistro.ToString(), "0", 6, true);

                _detalhe = Utils.SubstituiCaracteresEspeciais(_detalhe);

                return _detalhe;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar DETALHE do arquivo CNAB400.", ex);
            }
        }

        public string GerarDetalheRemessaTipo5CNAB400(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
        {
            //Multa
            var _detalhe = "";

            _detalhe += "5";
            _detalhe += "99";

            if (boleto.ValorMulta > 0)
            {
                _detalhe += "1";
                _detalhe += boleto.DataVencimento.ToString("ddMMyy");
                _detalhe += Utils.FormatCode(boleto.ValorMulta.ToString("0.00").Replace(",", ""), "0", 12, true);
            }
            else if (boleto.PercMulta > 0)
            {
                _detalhe += "2";
                _detalhe += boleto.DataVencimento.ToString("ddMMyy");
                _detalhe += Utils.FormatCode(boleto.PercMulta.ToString("0.00").Replace(",", ""), "0", 12, true);
            }
            else
            {
                _detalhe += "9";
                _detalhe += "000000";
                _detalhe += Utils.FormatCode("", "0", 12, true);
            }

            _detalhe += Utils.FormatCode("", " ", 372);
            _detalhe += Utils.FormatCode(numeroRegistro.ToString(), "0", 6, true);

            return _detalhe;
        }
        #endregion

        #region TRAILER
        public string GerarTrailerRemessa400(int numeroRegistro, decimal vltitulostotal)
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
        #endregion

        #endregion

        #region Métodos de processamento do arquivo retorno CNAB400
        public override HeaderRetorno LerHeaderRetornoCNAB400(string registro)
        {
            try
            {
                HeaderRetorno header = new HeaderRetorno();
                header.RegistroHeader = registro;
                header.Agencia = Convert.ToInt32(registro.Substring(26, 4));
                header.DigitoAgencia = Convert.ToInt32(registro.Substring(30, 1));
                header.ContaCorrente = Convert.ToInt32(registro.Substring(31, 8));
                header.DigitoContaCorrente = Convert.ToInt32(registro.Substring(39, 1));
                header.CodigoCedente = Convert.ToInt32(registro.Substring(40, 6));

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
                detalhe.Agencia = Utils.ToInt32(registro.Substring(17, 4));
                detalhe.Conta = Utils.ToInt32(registro.Substring(22, 8));
                detalhe.DACConta = Utils.ToInt32(registro.Substring(30, 1));
                detalhe.CodigoCedenteConvenio = Utils.ToInt32(registro.Substring(31, 6));
                detalhe.NossoNumero = registro.Substring(69, 5); //removido 6 primeiros digitos pois são o codigo do cedente/convenio
                detalhe.NumeroDocumento = registro.Substring(69, 5);

                var carteira = registro.Substring(106, 2) + registro.Substring(91, 3) == "000" ? "" : "-" + registro.Substring(91, 3);
                detalhe.Carteira = carteira;
                detalhe.CodigoOcorrencia = Utils.ToInt32(registro.Substring(108, 2));

                switch (Convert.ToInt32(detalhe.CodigoOcorrencia))
                {
                    case (int)ECodigoOcorrenciaBancoBrasil400.Entrada_Confirmada:
                        detalhe.Aceito = true;
                        break;
                    case (int)ECodigoOcorrenciaBancoBrasil400.Liquidação:
                    case (int)ECodigoOcorrenciaBancoBrasil400.Liquidação_Em_Cartório:
                    case (int)ECodigoOcorrenciaBancoBrasil400.Liquidação_Parcial:
                    case (int)ECodigoOcorrenciaBancoBrasil400.Liquidação_Por_Saldo:
                    case (int)ECodigoOcorrenciaBancoBrasil400.Liquidação_Sem_Registro:
                        detalhe.Baixado = true;
                        break;
                    case (int)ECodigoOcorrenciaBancoBrasil400.Baixa:
                        detalhe.Cancelado = true;
                        break;
                    case (int)ECodigoOcorrenciaBancoBrasil400.Rejeição:
                        detalhe.Aceito = false;
                        break;
                }

                int dataVencimento = Utils.ToInt32(registro.Substring(146, 6));
                detalhe.DataVencimento = Utils.ToDateTime(dataVencimento.ToString("##-##-##"));

                int dataOcorrencia = Utils.ToInt32(registro.Substring(110, 6));
                detalhe.DataOcorrencia = Utils.ToDateTime(dataOcorrencia.ToString("##-##-##"));

                decimal valorTitulo = Convert.ToInt64(registro.Substring(152, 13));
                detalhe.ValorTitulo = valorTitulo / 100;

                decimal tarifaCobranca = Convert.ToInt64(registro.Substring(181, 13));
                detalhe.TarifaCobranca = tarifaCobranca / 100;

                detalhe.CodigoBanco = Utils.ToInt32(registro.Substring(165, 3));

                detalhe.Especie = Utils.ToInt32(registro.Substring(173, 2));

                decimal valorAbatimento = Convert.ToUInt64(registro.Substring(227, 13));
                detalhe.ValorAbatimento = valorAbatimento / 100;

                decimal valorDesconto = Convert.ToUInt64(registro.Substring(240, 13));
                detalhe.Descontos = valorDesconto / 100;

                decimal valorPago = Convert.ToUInt64(registro.Substring(253, 13));
                detalhe.ValorPago = valorPago / 100;

                decimal jurosMora = Convert.ToUInt64(registro.Substring(266, 13));
                detalhe.JurosMora = jurosMora / 100;

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

        public override DetalheRetorno LerDetalheRetorno7CNAB400(string registro)
        {
            try
            {
                DetalheRetorno detalhe = new DetalheRetorno(registro);
                detalhe.Agencia = Utils.ToInt32(registro.Substring(17, 4));
                detalhe.Conta = Utils.ToInt32(registro.Substring(22, 8));
                detalhe.DACConta = Utils.ToInt32(registro.Substring(30, 1));
                detalhe.CodigoCedenteConvenio = Utils.ToInt32(registro.Substring(31, 7));
                detalhe.NossoNumero = registro.Substring(70, 10); //removido 7 primeiros digitos pois são o codigo do cedente/convenio
                detalhe.NumeroDocumento = registro.Substring(70, 10);

                var carteira = registro.Substring(106, 2) + registro.Substring(91, 3) == "000" ? "" : "-" + registro.Substring(91, 3);
                detalhe.Carteira = carteira;
                detalhe.CodigoOcorrencia = Utils.ToInt32(registro.Substring(108, 2));

                switch (Convert.ToInt32(detalhe.CodigoOcorrencia))
                {
                    case (int)ECodigoOcorrenciaBancoBrasil400.Entrada_Confirmada:
                        detalhe.Aceito = true;
                        break;
                    case (int)ECodigoOcorrenciaBancoBrasil400.Liquidação:
                    case (int)ECodigoOcorrenciaBancoBrasil400.Liquidação_Em_Cartório:
                    case (int)ECodigoOcorrenciaBancoBrasil400.Liquidação_Parcial:
                    case (int)ECodigoOcorrenciaBancoBrasil400.Liquidação_Por_Saldo:
                    case (int)ECodigoOcorrenciaBancoBrasil400.Liquidação_Sem_Registro:
                        detalhe.Baixado = true;
                        break;
                    case (int)ECodigoOcorrenciaBancoBrasil400.Baixa:
                        detalhe.Cancelado = true;
                        break;
                    case (int)ECodigoOcorrenciaBancoBrasil400.Rejeição:
                        detalhe.Aceito = false;
                        break;
                }

                int dataVencimento = Utils.ToInt32(registro.Substring(146, 6));
                detalhe.DataVencimento = Utils.ToDateTime(dataVencimento.ToString("##-##-##"));

                int dataOcorrencia = Utils.ToInt32(registro.Substring(110, 6));
                detalhe.DataOcorrencia = Utils.ToDateTime(dataOcorrencia.ToString("##-##-##"));

                decimal valorTitulo = Convert.ToInt64(registro.Substring(152, 13));
                detalhe.ValorTitulo = valorTitulo / 100;

                decimal tarifaCobranca = Convert.ToInt64(registro.Substring(181, 13));
                detalhe.TarifaCobranca = tarifaCobranca / 100;

                detalhe.CodigoBanco = Utils.ToInt32(registro.Substring(165, 3));

                detalhe.Especie = Utils.ToInt32(registro.Substring(173, 2));

                decimal valorAbatimento = Convert.ToUInt64(registro.Substring(227, 13));
                detalhe.ValorAbatimento = valorAbatimento / 100;

                decimal valorDesconto = Convert.ToUInt64(registro.Substring(240, 13));
                detalhe.Descontos = valorDesconto / 100;

                decimal valorPago = Convert.ToUInt64(registro.Substring(253, 13));
                detalhe.ValorPago = valorPago / 100;

                decimal jurosMora = Convert.ToUInt64(registro.Substring(266, 13));
                detalhe.JurosMora = jurosMora / 100;

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
        #endregion

        #region Método de geração de arquivo de remessa CNAB240
        public bool ValidarRemessaCNAB240(string numeroConvenio, IBanco banco, Cedente cedente, Boletos boletos, int numeroArquivoRemessa, out string mensagem)
        {
            string vMsg = string.Empty;
            mensagem = vMsg;
            return true;
            //throw new NotImplementedException("Função não implementada.");
        }

        #region HEADER

        public string GerarHeaderRemessaCNAB240(Cedente cedente, int numeroArquivoRemessa)
        {
            try
            {
                var dvAgencia = cedente.ContaBancaria.DigitoAgencia == null ? "0" : cedente.ContaBancaria.DigitoAgencia;
                string _brancos20 = new string(' ', 20);
                string _brancos10 = new string(' ', 10);
                string _header;

                _header = "00100000         ";
                if (cedente.CPFCNPJ.Length <= 11)
                    _header += "1";
                else
                    _header += "2";
                _header += Utils.FitStringLength(cedente.CPFCNPJ, 14, 14, '0', 0, true, true, true);
                #region Codigo de Convenio BB
                var carteira = "";
                if (cedente.Carteira.Length == 2)
                    carteira = cedente.Carteira.ToString() + "000  ";
                else
                    carteira = cedente.Carteira.Replace("-", "") + "  ";
                var codigoConvenio = Utils.FormatCode(cedente.Convenio.ToString(), "0", 9, true) + "0014" + carteira;
                #endregion
                _header += codigoConvenio;
                _header += Utils.FitStringLength(cedente.ContaBancaria.Agencia, 5, 5, '0', 0, true, true, true);
                _header += Utils.FitStringLength(dvAgencia, 1, 1, ' ', 0, true, true, false);
                _header += Utils.FitStringLength(cedente.ContaBancaria.Conta, 12, 12, '0', 0, true, true, true);
                _header += Utils.FitStringLength(cedente.ContaBancaria.DigitoConta, 1, 1, ' ', 0, true, true, false);
                _header += " "; // DÍGITO VERIFICADOR DA AG./CONTA
                _header += Utils.FitStringLength(cedente.Nome, 30, 30, ' ', 0, true, true, false);
                _header += Utils.FitStringLength("BANCO DO BRASIL S.A.", 30, 30, ' ', 0, true, true, false);
                _header += _brancos10;
                _header += "1";
                _header += DateTime.Now.ToString("ddMMyyyy");
                _header += DateTime.Now.ToString("hhmmss");
                _header += Utils.FitStringLength(numeroArquivoRemessa.ToString(), 6, 6, '0', 0, true, true, true);
                _header += "083";
                _header += "00000";
                _header += _brancos20;
                _header += _brancos20;
                _header += Utils.FormatCode("", "0", 29);

                _header = Utils.SubstituiCaracteresEspeciais(_header);

                return _header;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a geração do HEADER DE ARQUIVO do arquivo de REMESSA.", ex);
            }
        }

        private string GerarHeaderLoteRemessaCNAB240(string numeroConvenio, Cedente cedente, int numeroArquivoRemessa)
        {
            try
            {
                var dvAgencia = cedente.ContaBancaria.DigitoAgencia == null ? "0" : cedente.ContaBancaria.DigitoAgencia;
                string _brancos40 = new string(' ', 40);
                string _brancos33 = new string(' ', 33);
                string _headerLote;

                _headerLote = "00100011R01  ";
                _headerLote += "042";
                _headerLote += " ";
                #region Tipo Inscrição Empresa
                if (cedente.CPFCNPJ.Length <= 11)
                    _headerLote += "1";
                else
                    _headerLote += "2";
                #endregion
                _headerLote += Utils.FitStringLength(cedente.CPFCNPJ, 15, 15, '0', 0, true, true, true);
                #region Codigo de Convenio BB
                var carteira = "";
                if (cedente.Carteira.Length == 2)
                    carteira = cedente.Carteira.ToString() + "000  ";
                else
                    carteira = cedente.Carteira.Replace("-", "") + "  ";
                var codigoConvenio = Utils.FormatCode(cedente.Convenio.ToString(), "0", 9, true) + "0014" + carteira;
                #endregion
                _headerLote += codigoConvenio;
                _headerLote += Utils.FitStringLength(cedente.ContaBancaria.Agencia, 5, 5, '0', 0, true, true, true);
                _headerLote += Utils.FitStringLength(dvAgencia, 1, 1, '0', 0, true, true, true);
                _headerLote += Utils.FitStringLength(cedente.ContaBancaria.Conta, 12, 12, '0', 0, true, true, true);
                _headerLote += Utils.FitStringLength(cedente.ContaBancaria.DigitoConta, 1, 1, '0', 0, true, true, true);
                _headerLote += " ";
                _headerLote += Utils.FitStringLength(cedente.Nome, 30, 30, ' ', 0, true, true, false);
                _headerLote += _brancos40;
                _headerLote += _brancos40;
                _headerLote += Utils.FitStringLength(numeroArquivoRemessa.ToString(), 8, 8, '0', 0, true, true, true);
                _headerLote += DateTime.Now.ToString("ddMMyyyy");
                _headerLote += "00000000";
                _headerLote += _brancos33;

                _headerLote = Utils.SubstituiCaracteresEspeciais(_headerLote);

                return _headerLote;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a geração do HEADER DE LOTE do arquivo de REMESSA.", ex);
            }
        }

        #endregion

        #region DETALHE
        public string GerarDetalheRemessaCNAB240(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
        {
            throw new NotImplementedException("Função não implementada.");
        }

        public override string GerarDetalheSegmentoPRemessa(Boleto boleto, int numeroRegistro, string numeroConvenio)
        {
            try
            {
                string _segmentoP;
                string _nossoNumero;
                var dvAgencia = boleto.Cedente.ContaBancaria.DigitoAgencia == null ? "0" : boleto.Cedente.ContaBancaria.DigitoAgencia;
                this.FormataNossoNumero(boleto);

                _segmentoP = "00100013";
                _segmentoP += Utils.FitStringLength(numeroRegistro.ToString(), 5, 5, '0', 0, true, true, true);
                _segmentoP += "P ";
                _segmentoP += Utils.FitStringLength(boleto.CodigoOcorrencia, 2, 2, '0', 0, true, true, true);
                _segmentoP += Utils.FitStringLength(boleto.Cedente.ContaBancaria.Agencia, 5, 5, '0', 0, true, true, true);
                _segmentoP += Utils.FitStringLength(dvAgencia, 1, 1, '0', 0, true, true, true);
                _segmentoP += Utils.FitStringLength(boleto.Cedente.ContaBancaria.Conta, 12, 12, '0', 0, true, true, true);
                _segmentoP += Utils.FitStringLength(boleto.Cedente.ContaBancaria.DigitoConta, 1, 1, '0', 0, true, true, true);
                _segmentoP += " ";
                #region Tratamento Nosso Número
                if (!boleto.Cedente.Convenio.ToString().Length.Equals(7))
                    _nossoNumero = boleto.NossoNumero.Replace("-", "");
                else
                    _nossoNumero = boleto.NossoNumero;
                #endregion
                _segmentoP += Utils.FitStringLength(_nossoNumero, 20, 20, ' ', 0, true, true, false);
                #region Código da Carteira
                // Informar 1 – para carteira 11/12/17-019 na modalidade Simples; 
                // 2 ou 3 – para carteira 11/17 modalidade Vinculada/Caucionada e carteira 31; 
                // 4 – para carteira 11/17 modalidade Descontada e carteira 51; 
                // 7 – para carteira 17 modalidade Simples.
                if (boleto.Carteira.Equals("17-019") || boleto.Carteira.Equals("17") || boleto.Carteira.Equals("11") || boleto.Carteira.Equals("12"))
                    _segmentoP += "1";
                else
                    _segmentoP += "0";
                #endregion
                _segmentoP += "1";
                _segmentoP += "1";
                #region Emissão e Distribuição do Boleto
                var emissaoBoleto = "2"; // padrão Cliente Emite
                var distribuicaoBoleto = "2"; // padrão Cliente Distribui

                if (boleto.BancoGeraBoleto)
                {
                    emissaoBoleto = "1";
                    distribuicaoBoleto = "1";
                }
                #endregion
                _segmentoP += emissaoBoleto;
                _segmentoP += distribuicaoBoleto;
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
                _segmentoP += Utils.FormatCode(boleto.IOF.ToString().Replace(",", "").Replace(".", ""), "0", 15); // Valor do IOF a ser Recolhido 
                _segmentoP += Utils.FormatCode(boleto.Abatimento.ToString().Replace(",", "").Replace(".", ""), "0", 15); // Valor do Abatimento 
                _segmentoP += Utils.FitStringLength(boleto.NumeroDocumento, 25, 25, ' ', 0, true, true, false);

                string codigo_protesto = "3";
                string dias_protesto = "00";

                foreach (Instrucao instrucao in boleto.Instrucoes)
                {
                    switch ((EnumInstrucoes_BancoBrasil)instrucao.Codigo)
                    {
                        case EnumInstrucoes_BancoBrasil.ProtestarAposNDiasCorridos:
                            codigo_protesto = "1";
                            dias_protesto = Utils.FitStringLength(instrucao.QuantidadeDias.ToString(), 2, 2, '0', 0, true, true, true);
                            break;
                        case EnumInstrucoes_BancoBrasil.ProtestarAposNDiasUteis:
                            codigo_protesto = "2";
                            dias_protesto = Utils.FitStringLength(instrucao.QuantidadeDias.ToString(), 2, 2, '0', 0, true, true, true);
                            break;
                        case EnumInstrucoes_BancoBrasil.NaoProtestar:
                            codigo_protesto = "3";
                            dias_protesto = "00";
                            break;
                    }
                }

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
                string _zeros16 = new string('0', 16);
                string _brancos28 = new string(' ', 28);
                string _brancos40 = new string(' ', 40);

                string _segmentoQ;

                _segmentoQ = "00100013";
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
                _segmentoQ += _zeros16;
                _segmentoQ += _brancos40;
                _segmentoQ += "000";
                _segmentoQ += _brancos28;

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
                string _brancos110 = new string(' ', 110);
                string _brancos9 = new string(' ', 9);

                string _segmentoR;

                _segmentoR = "00100013";
                _segmentoR += Utils.FitStringLength(numeroRegistro.ToString(), 5, 5, '0', 0, true, true, true);
                _segmentoR += "R ";
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
                _segmentoR += _brancos110;
                _segmentoR += "0000000000000000"; //16 zeros
                _segmentoR += " "; //1 branco
                _segmentoR += "000000000000"; //12 zeros
                _segmentoR += "  "; //2 brancos
                _segmentoR += "0"; //1 zero
                _segmentoR += _brancos9;

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
        public string GerarTrailerRemessa240()
        {
            throw new NotImplementedException("Função não implementada.");
        }

        public override string GerarTrailerLoteRemessa(int numeroRegistro)
        {
            try
            {
                string _brancos217 = new string(' ', 217);

                string _trailerLote;

                _trailerLote = "00100015         ";
                _trailerLote += Utils.FitStringLength(numeroRegistro.ToString(), 6, 6, '0', 0, true, true, true);
                _trailerLote += _brancos217;

                _trailerLote = Utils.SubstituiCaracteresEspeciais(_trailerLote);

                return _trailerLote;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a geração do TRAILER de lote do arquivo de REMESSA.", ex);
            }
        }

        public override string GerarTrailerArquivoRemessa(int numeroRegistro)
        {
            try
            {
                string _brancos205 = new string(' ', 205);

                string _trailerArquivo;

                _trailerArquivo = "00199999         000001";
                _trailerArquivo += Utils.FitStringLength(numeroRegistro.ToString(), 6, 6, '0', 0, true, true, true);
                _trailerArquivo += "000000";
                _trailerArquivo += _brancos205;

                _trailerArquivo = Utils.SubstituiCaracteresEspeciais(_trailerArquivo);

                return _trailerArquivo;
            }
            catch (Exception ex)
            {
                throw new Exception("", ex);
            }
        }
        #endregion

        #endregion

        #region Métodos de processamento do arquivo retorno CNAB240
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
                header.CodigoCedente = Convert.ToInt32(registro.Substring(32, 9));

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
                {
                    throw new Exception("Registro inválida. O detalhe não possuí as características do segmento T.");
                }
                DetalheSegmentoTRetornoCNAB240 segmentoT = new DetalheSegmentoTRetornoCNAB240(registro);
                segmentoT.CodigoBanco = Convert.ToInt32(registro.Substring(0, 3)); //01
                segmentoT.idCodigoMovimento = Convert.ToInt32(registro.Substring(15, 2)); //07
                segmentoT.Agencia = Convert.ToInt32(registro.Substring(17, 5)); //08
                segmentoT.DigitoAgencia = registro.Substring(22, 1); //09
                segmentoT.Conta = Convert.ToInt64(registro.Substring(23, 12)); //10
                segmentoT.DigitoConta = registro.Substring(35, 1); //11
                segmentoT.DACAgenciaConta = (String.IsNullOrEmpty(registro.Substring(36, 1).Trim())) ? 0 : Convert.ToInt32(registro.Substring(36, 1)); //12
                segmentoT.NossoNumero = registro.Substring(44, 12); //14
                segmentoT.CodigoCarteira = Convert.ToInt32(registro.Substring(57, 1)); //15
                segmentoT.NumeroDocumento = registro.Substring(58, 15); //16
                segmentoT.DataVencimento = registro.Substring(73, 8).ToString() == "00000000" ? DateTime.Now : DateTime.ParseExact(registro.Substring(73, 8), "ddMMyyyy", CultureInfo.InvariantCulture); //17
                segmentoT.ValorTitulo = Convert.ToDecimal(registro.Substring(81, 15)) / 100; //18
                segmentoT.IdentificacaoTituloEmpresa = registro.Substring(105, 25); //23
                segmentoT.TipoInscricao = Convert.ToInt32(registro.Substring(132, 1)); //25
                segmentoT.NumeroInscricao = registro.Substring(133, 15); //26
                segmentoT.NomeSacado = registro.Substring(148, 40); //27
                segmentoT.ValorTarifas = Convert.ToDecimal(registro.Substring(198, 15)) / 100; //29
                segmentoT.IdRejeicao1 = registro.Substring(213, 1) == "A" ? registro.Substring(214, 9) : registro.Substring(213, 10); //30
                segmentoT.IdRejeicao2 = registro.Substring(210, 2);
                segmentoT.IdRejeicao3 = registro.Substring(212, 2);
                segmentoT.IdRejeicao4 = registro.Substring(214, 2);
                segmentoT.IdRejeicao5 = registro.Substring(216, 2);
                segmentoT.CodigoMovimento = new CodigoMovimento(segmentoT.CodigoBanco, segmentoT.idCodigoMovimento);

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
                        segmentoT.CodigoRejeicao3 = new CodigoRejeicao(segmentoT.CodigoBanco, Convert.ToInt32(segmentoT.IdRejeicao3));
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
                        segmentoT.CodigoRejeicao4 = new CodigoRejeicao(segmentoT.CodigoBanco, Convert.ToInt32(segmentoT.IdRejeicao4));
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
                    case (int)EnumCodigoMovimento_BancoBrasil.EntradaConfirmada:
                        segmentoT.Aceito = true;
                        break;
                    case (int)EnumCodigoMovimento_BancoBrasil.Liquidação:
                    case (int)EnumCodigoMovimento_BancoBrasil.LiquidacaoAposBaixa:
                        segmentoT.Baixado = true;
                        segmentoT.Aceito = false;
                        break;
                    case (int)EnumCodigoMovimento_BancoBrasil.Baixa:
                        segmentoT.Cancelado = true;
                        segmentoT.Aceito = false;
                        break;
                    case (int)EnumCodigoMovimento_BancoBrasil.EntradaRejeitada:
                        segmentoT.Aceito = false;
                        break;
                }

                return segmentoT;
            }
            catch (Exception ex)
            {
                //TrataErros.Tratar(ex);
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
    }
}