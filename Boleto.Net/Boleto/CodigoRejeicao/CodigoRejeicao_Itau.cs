using System;
using System.Collections.Generic;
using System.Text;

namespace BoletoNet
{
    #region Enumerado

    public enum EnumCodigoRejeicao_Itau
    {

    }

    #endregion

    public class CodigoRejeicao_Itau : AbstractCodigoRejeicao, ICodigoRejeicao
    {
        #region Construtores

        public CodigoRejeicao_Itau()
        {
            try
            {
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }

        public CodigoRejeicao_Itau(string codigo)
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
                this.Banco = new Banco_Itau();
                var _codigo = (EnumCodigoRejeicao_Itau)Convert.ToInt32(codigo);
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
