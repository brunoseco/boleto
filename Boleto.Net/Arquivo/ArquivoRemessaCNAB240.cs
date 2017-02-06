using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace BoletoNet
{
    internal class ArquivoRemessaCNAB240 : AbstractArquivoRemessa, IArquivoRemessa
    {

        #region Construtores

        public ArquivoRemessaCNAB240()
        {
            this.TipoArquivo = TipoArquivo.CNAB240;
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
                int numeroRegistro = 3;
                int numeroRegistroDetalhe = 1;
                string strline;

                //var ms = new MemoryStream(0);
                //var incluiLinha = new StreamWriter(ms);
                var incluiLinha = new StringBuilder();

                if (banco.Codigo == 104)//quando é caixa verifica o modelo de leiatue que é está em boletos.remssa.tipodocumento
                    strline = banco.GerarHeaderRemessa(numeroConvenio, cedente, TipoArquivo.CNAB240, numeroArquivoRemessa, boletos[0]);
                else
                    strline = banco.GerarHeaderRemessa(numeroConvenio, cedente, TipoArquivo.CNAB240, numeroArquivoRemessa);
                //
                incluiLinha.AppendLine(strline);
                OnLinhaGerada(null, strline, EnumTipodeLinha.HeaderDeArquivo);
                if (banco.Codigo == 104)//quando é caixa verifica o modelo de leiatue que é está em boletos.remssa.tipodocumento
                    strline = banco.GerarHeaderLoteRemessa(numeroConvenio, cedente, numeroArquivoRemessa, TipoArquivo.CNAB240, boletos[0]);
                else
                    strline = banco.GerarHeaderLoteRemessa(numeroConvenio, cedente, numeroArquivoRemessa, TipoArquivo.CNAB240);

                incluiLinha.AppendLine(strline);
                OnLinhaGerada(null, strline, EnumTipodeLinha.HeaderDeLote);

                if (banco.Codigo == 104) // Só validar boleto.Remessa quando o banco for Caixa porque quando o banco for diferente de 104 a propriedade "Remessa" fica null
                {
                    #region se Banco Caixa - 104
                    foreach (Boleto boleto in boletos)
                    {
                        boleto.Banco = banco;
                        strline = boleto.Banco.GerarDetalheSegmentoPRemessa(boleto, numeroRegistroDetalhe, numeroConvenio, cedente);
                        incluiLinha.AppendLine(strline);
                        OnLinhaGerada(boleto, strline, EnumTipodeLinha.DetalheSegmentoP);
                        numeroRegistro++;
                        numeroRegistroDetalhe++;

                        strline = boleto.Banco.GerarDetalheSegmentoQRemessa(boleto, numeroRegistroDetalhe, TipoArquivo.CNAB240);
                        incluiLinha.AppendLine(strline);
                        OnLinhaGerada(boleto, strline, EnumTipodeLinha.DetalheSegmentoQ);
                        numeroRegistro++;
                        numeroRegistroDetalhe++;

                        if (boleto.ValorMulta > 0)
                        {
                            strline = boleto.Banco.GerarDetalheSegmentoRRemessa(boleto, numeroRegistroDetalhe, TipoArquivo.CNAB240);
                            incluiLinha.AppendLine(strline);
                            OnLinhaGerada(boleto, strline, EnumTipodeLinha.DetalheSegmentoR);
                            numeroRegistro++;
                            numeroRegistroDetalhe++;
                        }

                        if (boleto.Instrucoes.Count > 0)
                        {
                            strline = boleto.Banco.GerarDetalheSegmentoSRemessa(boleto, numeroRegistroDetalhe, TipoArquivo.CNAB240);
                            incluiLinha.AppendLine(strline);
                            OnLinhaGerada(boleto, strline, EnumTipodeLinha.DetalheSegmentoS);
                            numeroRegistro++;
                            numeroRegistroDetalhe++;
                        }
                    }

                    numeroRegistro--;
                    strline = banco.GerarTrailerLoteRemessa(numeroRegistro, boletos[0]);
                    incluiLinha.AppendLine(strline);
                    OnLinhaGerada(null, strline, EnumTipodeLinha.TraillerDeLote);

                    numeroRegistro++;
                    numeroRegistro++;

                    strline = banco.GerarTrailerArquivoRemessa(numeroRegistro, boletos[0]);
                    incluiLinha.AppendLine(strline);
                    OnLinhaGerada(null, strline, EnumTipodeLinha.TraillerDeArquivo);

                    #endregion
                }
                else //para qualquer outro banco, gera CNAB240 com segmentos abaixo
                {
                    #region outros bancos
                    foreach (Boleto boleto in boletos)
                    {
                        boleto.Banco = banco;
                        strline = boleto.Banco.GerarDetalheSegmentoPRemessa(boleto, numeroRegistroDetalhe, numeroConvenio);
                        incluiLinha.AppendLine(strline);
                        OnLinhaGerada(boleto, strline, EnumTipodeLinha.DetalheSegmentoP);
                        numeroRegistro++;
                        numeroRegistroDetalhe++;

                        if (boleto.CodigoOcorrencia != "02")
                        {
                            strline = boleto.Banco.GerarDetalheSegmentoQRemessa(boleto, numeroRegistroDetalhe, TipoArquivo.CNAB240);
                            incluiLinha.AppendLine(strline);
                            OnLinhaGerada(boleto, strline, EnumTipodeLinha.DetalheSegmentoQ);
                            numeroRegistro++;
                            numeroRegistroDetalhe++;

                            if (boleto.ValorMulta > 0)
                            {
                                strline = boleto.Banco.GerarDetalheSegmentoRRemessa(boleto, numeroRegistroDetalhe, TipoArquivo.CNAB240);
                                incluiLinha.AppendLine(strline);
                                OnLinhaGerada(boleto, strline, EnumTipodeLinha.DetalheSegmentoR);
                                numeroRegistro++;
                                numeroRegistroDetalhe++;
                            }

                            if (boleto.Instrucoes.Count > 0)
                            {
                                strline = boleto.Banco.GerarDetalheSegmentoSRemessa(boleto, numeroRegistroDetalhe, TipoArquivo.CNAB240);
                                incluiLinha.AppendLine(strline);
                                OnLinhaGerada(boleto, strline, EnumTipodeLinha.DetalheSegmentoS);
                                numeroRegistro++;
                                numeroRegistroDetalhe++;
                            }
                        }
                    }

                    numeroRegistro--;
                    strline = banco.GerarTrailerLoteRemessa(numeroRegistro);
                    incluiLinha.AppendLine(strline);
                    OnLinhaGerada(null, strline, EnumTipodeLinha.TraillerDeLote);

                    numeroRegistro++;
                    numeroRegistro++;

                    strline = banco.GerarTrailerArquivoRemessa(numeroRegistro);
                    incluiLinha.AppendLine(strline);
                    OnLinhaGerada(null, strline, EnumTipodeLinha.TraillerDeArquivo);

                    #endregion
                }

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
