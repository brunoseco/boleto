using System;
using System.Collections.Generic;
using System.Text;

namespace BoletoNet
{
    #region Enumerado

    public enum EnumCodigoRejeicao_Bradesco
    {

    }

    #endregion

    public class CodigoRejeicao_Bradesco : AbstractCodigoRejeicao, ICodigoRejeicao
    {
        #region Construtores

        public CodigoRejeicao_Bradesco()
        {
            try
            {
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }

        public CodigoRejeicao_Bradesco(string codigo)
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

        private void carregar(string codigo)
        {
            try
            {
                this.Banco = new Banco_Bradesco();
                var _codigo = (EnumCodigoRejeicao_Bradesco)Convert.ToInt32(codigo);

                switch (_codigo)
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
