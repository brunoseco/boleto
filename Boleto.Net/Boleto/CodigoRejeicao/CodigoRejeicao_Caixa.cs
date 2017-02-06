using System;
using System.Collections.Generic;
using System.Text;

namespace BoletoNet
{
    #region Enumerado

    public enum EnumCodigoRejeicao_Caixa
    {

    }

    #endregion

    public class CodigoRejeicao_Caixa : AbstractCodigoRejeicao, ICodigoRejeicao
    {
        #region Construtores

        public CodigoRejeicao_Caixa()
        {
            try
            {
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }

        public CodigoRejeicao_Caixa(string codigo)
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
                this.Banco = new Banco_Caixa();
                //var _codigo = (EnumCodigoRejeicao_Caixa)Convert.ToInt32(codigo);
                var _codigo = codigo;

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
