using System;
using System.Collections.Generic;
using System.Text;

namespace BoletoNet
{
    public class Instrucao : AbstractInstrucao, IInstrucao
    {

        #region Variaveis

        private IInstrucao _IInstrucao;

        #endregion

        #region Construtores

        internal Instrucao()
        {
        }

        public Instrucao(int CodigoBanco, string codigoInstrucao)
        {
            try
            {
                InstanciaInstrucao(CodigoBanco, codigoInstrucao);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao instanciar objeto.", ex);
            }
        }

        public Instrucao(int CodigoBanco)
        {
            try
            {
                InstanciaInstrucao(CodigoBanco, "0");
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao instanciar objeto.", ex);
            }
        }

        #endregion

        # region Métodos Privados

        private void InstanciaInstrucao(int codigoBanco, string codigoInstrucao)
        {
            try
            {
                var codigo = Convert.ToInt32(codigoInstrucao);

                switch (codigoBanco)
                {
                    //399 - HSBC
                    case 399:
                        _IInstrucao = new Instrucao_HSBC(codigo);
                        break;
                    //104 - Caixa
                    case 104:
                        _IInstrucao = new Instrucao_Caixa(codigo);
                        break;
                    //341 - Itaú
                    case 341:
                        _IInstrucao = new Instrucao_Itau(codigo);
                        break;
                    //1 - Banco do Brasil
                    case 1:
                        _IInstrucao = new Instrucao_BancoBrasil(codigo);
                        break;
                    //356 - Real
                    case 356:
                        _IInstrucao = new Instrucao_Real(codigo);
                        break;
                    //422 - Safra
                    case 422:
                        _IInstrucao = new Instrucao_Safra(codigo);
                        break;
                    //237 - Bradesco
                    case 237:
                        _IInstrucao = new Instrucao_Bradesco(codigo);
                        break;
                    //347 - Sudameris
                    case 347:
                        _IInstrucao = new Instrucao_Sudameris(codigo);
                        break;
                    //353 - Santander
                    case 353:
                    case 33:
                    case 8:
                        //case 8:
                        _IInstrucao = new Instrucao_Santander(codigo);
                        break;
                    //070 - BRB
                    case 70:
                        _IInstrucao = new Instrucao_BRB(codigo);
                        break;
                    //756 - Sicoob
                    case 756:
                        _IInstrucao = new Instrucao_Sicoob(codigo);
                        break;
                    //479 - BankBoston
                    case 479:
                        _IInstrucao = new Instrucao_BankBoston(codigo);
                        break;
                    //41 - Banrisul
                    case 41:
                        _IInstrucao = new Instrucao_Banrisul(codigo);
                        break;
                    default:
                        throw new Exception("Código do banco não implementando: " + codigoBanco);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a execução da transação.", ex);
            }
        }

        # endregion

        #region Propriedades da interface

        public override IBanco Banco
        {
            get { return _IInstrucao.Banco; }
            set { _IInstrucao.Banco = value; }
        }

        public override int Codigo
        {
            get { return _IInstrucao.Codigo; }
            set { _IInstrucao.Codigo = value; }
        }

        public override string Descricao
        {
            get { return _IInstrucao.Descricao; }
            set { _IInstrucao.Descricao = value; }
        }

        public override int QuantidadeDias
        {
            get { return _IInstrucao.QuantidadeDias; }
            set { _IInstrucao.QuantidadeDias = value; }
        }

        #endregion

        #region Métodos de interface

        public override void Valida()
        {
            try
            {
                //_IInstrucao.Valida();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a validação dos campos.", ex);
            }
        }

        #endregion

    }
}
