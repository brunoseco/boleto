using System;
using System.Collections.Generic;
using System.Text;

namespace BoletoNet
{
    public class CodigoRejeicao : AbstractCodigoRejeicao, ICodigoRejeicao
    {

        #region Variaveis

        private ICodigoRejeicao _ICodigoRejeicao = null;

        #endregion

        #region Propriedades da interface

        public override IBanco Banco
        {
            get { return _ICodigoRejeicao.Banco; }
        }

        public override int Codigo
        {
            get { return _ICodigoRejeicao.Codigo; }
        }

        public override string Descricao
        {
            get { return _ICodigoRejeicao.Descricao; }
        }

        #endregion

        #region Construtores

        public CodigoRejeicao(int codigoBanco, int codigoRejeicao)
        {
            try
            {
                InstanciaCodigoRejeicao(codigoBanco, codigoRejeicao.ToString());
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao instanciar objeto.", ex);
            }
        }

        public CodigoRejeicao(int codigoBanco, string codigoRejeicao)
        {
            try
            {
                InstanciaCodigoRejeicao(codigoBanco, codigoRejeicao);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao instanciar objeto.", ex);
            }
        }


        #endregion Construtores

        #region Métodos
        private void InstanciaCodigoRejeicao(int codigoBanco, string codigoRejeicao)
        {
            try
            {
                switch (codigoBanco)
                {
                    case 104:
                        _ICodigoRejeicao = new CodigoRejeicao_Caixa(codigoRejeicao);
                        break;
                    case 341:
                        _ICodigoRejeicao = new CodigoRejeicao_Itau(codigoRejeicao);
                        break;
                    case 1:
                        _ICodigoRejeicao = new CodigoRejeicao_BancoBrasil(codigoRejeicao);
                        break;
                    case 237:
                        _ICodigoRejeicao = new CodigoRejeicao_Bradesco(codigoRejeicao);
                        break;
                    case 353:
                    case 008:
                    case 033:
                        _ICodigoRejeicao = new CodigoRejeicao_Santander(codigoRejeicao);
                        break;
                    case 756:
                        _ICodigoRejeicao = new CodigoRejeicao_Sicoob(codigoRejeicao);
                        break;
                    case 399:
                        _ICodigoRejeicao = new CodigoRejeicao_HSBC(codigoRejeicao);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a execução da transação.", ex);
            }
        }
        #endregion Métodos
    }
}
