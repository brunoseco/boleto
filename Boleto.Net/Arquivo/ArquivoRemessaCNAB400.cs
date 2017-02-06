using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace BoletoNet
{
    internal class ArquivoRemessaCNAB400 : AbstractArquivoRemessa, IArquivoRemessa
    {

        #region Construtores

        public ArquivoRemessaCNAB400()
        {
            this.TipoArquivo = TipoArquivo.CNAB400;
        }

        #endregion

        #region Métodos de instância
       
        public override bool ValidarArquivoRemessa(string numeroConvenio, IBanco banco, Cedente cedente, Boletos boletos, int numeroArquivoRemessa, out string mensagem)
        {
            try
            {
                bool vRetorno = true;
                string vMsg = string.Empty;
                //
                foreach (Boleto boleto in boletos)
                {
                    string vMsgBol = string.Empty;
                    bool vRetBol = boleto.Banco.ValidarRemessa(this.TipoArquivo, numeroConvenio, banco, cedente, boletos, numeroArquivoRemessa, out vMsgBol);
                    if (!vRetBol && !String.IsNullOrEmpty(vMsgBol))
                    {
                        vMsg += vMsgBol;
                        vRetorno = vRetBol;
                    }
                }
                //
                mensagem = vMsg;
                return vRetorno;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override byte[] GerarArquivoRemessa(string numeroConvenio, IBanco banco, Cedente cedente, Boletos boletos, String arquivo, int numeroArquivoRemessa)
        {
            try
            {
                int numeroRegistro = 2;
                string strline;
                decimal vltitulostotal = 0;                 //Uso apenas no registro TRAILER do banco Santander - jsoda em 09/05/2012 - Add no registro TRAILER do banco Banrisul - sidneiklein em 08/08/2013

                //var ms = new MemoryStream();
                //StreamWriter incluiLinha = new StreamWriter(ms);
                var incluiLinha = new StringBuilder();

                strline = banco.GerarHeaderRemessa(numeroConvenio, cedente, TipoArquivo.CNAB400, numeroArquivoRemessa);
                incluiLinha.AppendLine(strline);

                foreach (Boleto boleto in boletos)
                {
                    boleto.Banco = banco;
                    strline = boleto.Banco.GerarDetalheRemessa(boleto, numeroRegistro, TipoArquivo.CNAB400);
                    incluiLinha.AppendLine(strline);
                    vltitulostotal += boleto.ValorBoleto; 
                    numeroRegistro++;
                    if (boleto.BancoGeraBoleto)
                    {
                        strline = boleto.Banco.GerarDetalheRemessaTipo2(boleto, numeroRegistro, TipoArquivo.CNAB400);
                        incluiLinha.AppendLine(strline);
                        numeroRegistro++;
                    }
                    if ((boleto.PercMulta > 0 || boleto.ValorMulta > 0) && boleto.Banco.Codigo.Equals(1))
                    {
                        strline = boleto.Banco.GerarDetalheRemessaTipo5(boleto, numeroRegistro, TipoArquivo.CNAB400);
                        incluiLinha.AppendLine(strline);
                        numeroRegistro++;
                    }
                }

                strline = banco.GerarTrailerRemessa(numeroRegistro, TipoArquivo.CNAB400, cedente, vltitulostotal);

                incluiLinha.AppendLine(strline);

                //return Encoding.UTF8.GetBytes(incluiLinha.ToString());
                //return new UTF8Encoding(true).GetBytes(incluiLinha.ToString());
                return Encoding.Default.GetBytes(incluiLinha.ToString());
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar arquivo remessa. " + ex.Message, ex);
            }
        }

        #endregion

    }
}
