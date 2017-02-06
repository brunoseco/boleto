using System;
using System.Collections.Generic;
using System.Text;

namespace BoletoNet
{
    #region Enumerado

    public enum EnumCodigoMovimento_Sicoob
    {

    }

    #endregion

    public class CodigoMovimento_Sicoob : AbstractCodigoMovimento, ICodigoMovimento
    {
        #region Construtores

        public CodigoMovimento_Sicoob()
        {
            try
            {
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }

        public CodigoMovimento_Sicoob(int codigo)
        {
            try
            {
                this.carregar(codigo);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }

        #endregion

        #region Metodos Privados

        private void carregar(int codigo)
        {
            try
            {
                this.Banco = new Banco_Sicoob();

                switch ((EnumCodigoMovimento_Sicoob)codigo)
                {
                    default:
                        this.Codigo = 0;
                        this.Descricao = "";
                        break;
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }

        private void Ler(int codigo)
        {
            try
            {
                switch (codigo)
                {
                    default:
                        this.Codigo = 0;
                        this.Descricao = "";
                        break;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }


        #endregion
    }
}
