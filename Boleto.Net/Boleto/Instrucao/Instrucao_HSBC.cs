using System;
using System.Collections;
using System.Text;

namespace BoletoNet
{
    #region Enumerado

    public enum EnumInstrucoes_HSBC
    {
        NãoReceberApósXDiasVencimento = 71,
        JurosdeMora = 998,
    }

    #endregion

    public class Instrucao_HSBC : AbstractInstrucao, IInstrucao
    {

        #region Construtores
        public Instrucao_HSBC()
        {
            try
            {
                this.Banco = new Banco(399);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }
        public Instrucao_HSBC(Banco Banco, int Codigo)
        {
            try
            {

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }

        public Instrucao_HSBC(int codigo)
        {
            this.carregar(codigo, 0);
        }

        public Instrucao_HSBC(EnumInstrucoes_HSBC codigo)
        {
            this.carregar((int)codigo, 0);
        }

        public Instrucao_HSBC(int codigo, int nrDias)
        {
            this.carregar(codigo, nrDias);
        }
        #endregion

        #region Metodos Privados

        private void carregar(int idInstrucao, double valor)
        {
            try
            {
                this.Banco = new Banco_HSBC();
                this.Valida();

                switch ((EnumInstrucoes_HSBC)idInstrucao)
                {
                    case EnumInstrucoes_HSBC.JurosdeMora:
                        this.Codigo = (int)EnumInstrucoes_BancoBrasil.JurosdeMora;
                        this.Descricao = "Após vencimento cobrar R$ " + valor + " por dia de atraso";
                        break;
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

        public override void Valida()
        {
            //base.Valida();
        }

        #endregion

    }
}
