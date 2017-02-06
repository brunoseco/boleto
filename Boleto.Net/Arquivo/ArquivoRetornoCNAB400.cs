using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace BoletoNet
{
    public class ArquivoRetornoCNAB400 : AbstractArquivoRetorno, IArquivoRetorno
    {

        private List<DetalheRetorno> _listaDetalhe = new List<DetalheRetorno>();
        private HeaderRetorno _headerRetorno = new HeaderRetorno();

        public HeaderRetorno HeaderRetorno
        {
            get { return _headerRetorno; }
            set { _headerRetorno = value; }
        }

        public List<DetalheRetorno> ListaDetalhe
        {
            get { return _listaDetalhe; }
            set { _listaDetalhe = value; }
        }

        #region Construtores

        public ArquivoRetornoCNAB400()
        {
            this.TipoArquivo = TipoArquivo.CNAB400;
        }

        #endregion

        #region M�todos de inst�ncia

        public override void LerArquivoRetorno(IBanco banco, Stream arquivo)
        {
            try
            {
                StreamReader stream = new StreamReader(arquivo, System.Text.Encoding.UTF8);
                string linha = "";

                // Lendo o arquivo
                linha = stream.ReadLine();

                HeaderRetorno header = banco.LerHeaderRetornoCNAB400(linha);
                HeaderRetorno = header;

                // Pr�xima linha (DETALHE)
                linha = stream.ReadLine();

                if (DetalheRetorno.PrimeiroCaracter(linha) == "1")
                {
                    while (DetalheRetorno.PrimeiroCaracter(linha) == "1")
                    {
                        DetalheRetorno detalhe = banco.LerDetalheRetornoCNAB400(linha);
                        ListaDetalhe.Add(detalhe);
                        OnLinhaLida(detalhe, linha);
                        linha = stream.ReadLine();
                    }
                }
                else if (DetalheRetorno.PrimeiroCaracter(linha) == "7")
                {
                    while (DetalheRetorno.PrimeiroCaracter(linha) == "7")
                    {
                        DetalheRetorno detalhe = banco.LerDetalheRetorno7CNAB400(linha);
                        ListaDetalhe.Add(detalhe);
                        OnLinhaLida(detalhe, linha);
                        linha = stream.ReadLine();
                    }
                }

                stream.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion
    }
}
