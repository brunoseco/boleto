using System;
using System.Collections;
using System.Text;

namespace BoletoNet
{
    #region Enumerado

    public enum EnumInstrucoes_Sicoob
    {
      AUSENCIA_DE_INSTRUCOES = 00,
      COBRAR_JUROS = 01,
      PROTESTAR_3_DIAS_UTEIS_APOS_VENCIMENTO = 03,
      PROTESTAR_4_DIAS_UTEIS_APOS_VENCIMENTO = 04,
      PROTESTAR_5_DIAS_UTEIS_APOS_VENCIMENTO = 05,
      NAO_PROTESTAR = 07,
      PROTESTAR_10_DIAS_UTEIS_APOS_VENCIMENTO = 10,
      PROTESTAR_15_DIAS_UTEIS_APOS_VENCIMENTO = 15,
      PROTESTAR_20_DIAS_UTEIS_APOS_VENCIMENTO = 20,
      CONCEDER_DESCONTO_SO_ATE_DATA_ESTIPULADA = 22,
      DEVOLVER_APOS_15_DIAS_VENCIDO = 42,
      DEVOLVER_APOS_30_DIAS_VENCIDO = 43,
    }

    #endregion

    public class Instrucao_Sicoob : AbstractInstrucao, IInstrucao
    {

        #region Construtores
        public Instrucao_Sicoob()
        {
            try
            {
                this.Banco = new Banco(756);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }

        public Instrucao_Sicoob(int codigo)
        {
            this.carregar(codigo, 0);
        }

        public Instrucao_Sicoob(int codigo, int nrDias)
        {
            this.carregar(codigo, nrDias);
        }

        public Instrucao_Sicoob(int codigo, decimal vlMulta, decimal porcMulta, decimal vlJuros, decimal porcJuros)
        {
            this.carregar(codigo, vlMulta, porcMulta, vlJuros, porcJuros, 0);
        }
        #endregion

        #region Metodos Privados

        private void carregar(int idInstrucao, decimal vlMulta, decimal porcMulta, decimal vlJuros, decimal porcJuros, int nrDias)
        {
            try
            {
                this.Banco = new Banco_Sicoob();

                switch ((EnumInstrucoes_Sicoob)idInstrucao)
                {
                    case EnumInstrucoes_Sicoob.AUSENCIA_DE_INSTRUCOES:
                        this.Codigo = (int)EnumInstrucoes_Sicoob.AUSENCIA_DE_INSTRUCOES;
                        this.Descricao = "";
                        break;
                    case EnumInstrucoes_Sicoob.COBRAR_JUROS:
                        this.Codigo = (int)EnumInstrucoes_Sicoob.COBRAR_JUROS;
                        this.Descricao = "Cobrar juros";
                        break;
                    case EnumInstrucoes_Sicoob.CONCEDER_DESCONTO_SO_ATE_DATA_ESTIPULADA:
                        this.Codigo = (int)EnumInstrucoes_Sicoob.CONCEDER_DESCONTO_SO_ATE_DATA_ESTIPULADA;
                        this.Descricao = "Conceder desconto só até a data estipulada";
                        break;
                    case EnumInstrucoes_Sicoob.DEVOLVER_APOS_15_DIAS_VENCIDO:
                        this.Codigo = (int)EnumInstrucoes_Sicoob.DEVOLVER_APOS_15_DIAS_VENCIDO;
                        this.Descricao = "Devolver após 15 dias vencidos";
                        break;
                    case EnumInstrucoes_Sicoob.DEVOLVER_APOS_30_DIAS_VENCIDO:
                        this.Codigo = (int)EnumInstrucoes_Sicoob.DEVOLVER_APOS_30_DIAS_VENCIDO;
                        this.Descricao = "Devolver após 30 dias vencidos";
                        break;
                    case EnumInstrucoes_Sicoob.NAO_PROTESTAR:
                        this.Codigo = (int)EnumInstrucoes_Sicoob.NAO_PROTESTAR;
                        this.Descricao = "Não protestar";
                        break;
                    case EnumInstrucoes_Sicoob.PROTESTAR_10_DIAS_UTEIS_APOS_VENCIMENTO:
                        this.Codigo = (int)EnumInstrucoes_Sicoob.PROTESTAR_10_DIAS_UTEIS_APOS_VENCIMENTO;
                        this.Descricao = "Protestar 10 dias úteis após vencimento";
                        break;
                    case EnumInstrucoes_Sicoob.PROTESTAR_15_DIAS_UTEIS_APOS_VENCIMENTO:
                        this.Codigo = (int)EnumInstrucoes_Sicoob.PROTESTAR_15_DIAS_UTEIS_APOS_VENCIMENTO;
                        this.Descricao = "Protestar 15 dias úteis após vencimento";
                        break;
                    case EnumInstrucoes_Sicoob.PROTESTAR_20_DIAS_UTEIS_APOS_VENCIMENTO:
                        this.Codigo = (int)EnumInstrucoes_Sicoob.PROTESTAR_20_DIAS_UTEIS_APOS_VENCIMENTO;
                        this.Descricao = "Protestar 20 dias úteis após vencimento";
                        break;
                    case EnumInstrucoes_Sicoob.PROTESTAR_3_DIAS_UTEIS_APOS_VENCIMENTO:
                        this.Codigo = (int)EnumInstrucoes_Sicoob.PROTESTAR_3_DIAS_UTEIS_APOS_VENCIMENTO;
                        this.Descricao = "Protestar 3 dias úteis após vencimento";
                        break;
                    case EnumInstrucoes_Sicoob.PROTESTAR_4_DIAS_UTEIS_APOS_VENCIMENTO:
                        this.Codigo = (int)EnumInstrucoes_Sicoob.PROTESTAR_4_DIAS_UTEIS_APOS_VENCIMENTO;
                        this.Descricao = "Protestar 4 dias úteis após vencimento";
                        break;
                    case EnumInstrucoes_Sicoob.PROTESTAR_5_DIAS_UTEIS_APOS_VENCIMENTO:
                        this.Codigo = (int)EnumInstrucoes_Sicoob.PROTESTAR_5_DIAS_UTEIS_APOS_VENCIMENTO;
                        this.Descricao = "Protestar 5 dias úteis após vencimento";
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

        private void carregar(int idInstrucao, int nrDias)
        {
            try
            {
                this.Banco = new Banco_Sicoob();

                switch ((EnumInstrucoes_Sicoob)idInstrucao)
                {
                    case EnumInstrucoes_Sicoob.AUSENCIA_DE_INSTRUCOES:
                        this.Codigo = (int)EnumInstrucoes_Sicoob.AUSENCIA_DE_INSTRUCOES;
                        this.Descricao = "";
                        break;
                    case EnumInstrucoes_Sicoob.COBRAR_JUROS:
                        this.Codigo = (int)EnumInstrucoes_Sicoob.COBRAR_JUROS;
                        this.Descricao = "Cobrar juros";
                        break;
                    case EnumInstrucoes_Sicoob.CONCEDER_DESCONTO_SO_ATE_DATA_ESTIPULADA:
                        this.Codigo = (int)EnumInstrucoes_Sicoob.CONCEDER_DESCONTO_SO_ATE_DATA_ESTIPULADA;
                        this.Descricao = "Conceder desconto só até a data estipulada";
                        break;
                    case EnumInstrucoes_Sicoob.DEVOLVER_APOS_15_DIAS_VENCIDO:
                        this.Codigo = (int)EnumInstrucoes_Sicoob.DEVOLVER_APOS_15_DIAS_VENCIDO;
                        this.Descricao = "Devolver após 15 dias vencidos";
                        break;
                    case EnumInstrucoes_Sicoob.DEVOLVER_APOS_30_DIAS_VENCIDO:
                        this.Codigo = (int)EnumInstrucoes_Sicoob.DEVOLVER_APOS_30_DIAS_VENCIDO;
                        this.Descricao = "Devolver após 30 dias vencidos";
                        break;
                    case EnumInstrucoes_Sicoob.NAO_PROTESTAR:
                        this.Codigo = (int)EnumInstrucoes_Sicoob.NAO_PROTESTAR;
                        this.Descricao = "Não protestar";
                        break;
                    case EnumInstrucoes_Sicoob.PROTESTAR_10_DIAS_UTEIS_APOS_VENCIMENTO:
                        this.Codigo = (int)EnumInstrucoes_Sicoob.PROTESTAR_10_DIAS_UTEIS_APOS_VENCIMENTO;
                        this.Descricao = "Protestar 10 dias úteis após vencimento";
                        break;
                    case EnumInstrucoes_Sicoob.PROTESTAR_15_DIAS_UTEIS_APOS_VENCIMENTO:
                        this.Codigo = (int)EnumInstrucoes_Sicoob.PROTESTAR_15_DIAS_UTEIS_APOS_VENCIMENTO;
                        this.Descricao = "Protestar 15 dias úteis após vencimento";
                        break;
                    case EnumInstrucoes_Sicoob.PROTESTAR_20_DIAS_UTEIS_APOS_VENCIMENTO:
                        this.Codigo = (int)EnumInstrucoes_Sicoob.PROTESTAR_20_DIAS_UTEIS_APOS_VENCIMENTO;
                        this.Descricao = "Protestar 20 dias úteis após vencimento";
                        break;
                    case EnumInstrucoes_Sicoob.PROTESTAR_3_DIAS_UTEIS_APOS_VENCIMENTO:
                        this.Codigo = (int)EnumInstrucoes_Sicoob.PROTESTAR_3_DIAS_UTEIS_APOS_VENCIMENTO;
                        this.Descricao = "Protestar 3 dias úteis após vencimento";
                        break;
                    case EnumInstrucoes_Sicoob.PROTESTAR_4_DIAS_UTEIS_APOS_VENCIMENTO:
                        this.Codigo = (int)EnumInstrucoes_Sicoob.PROTESTAR_4_DIAS_UTEIS_APOS_VENCIMENTO;
                        this.Descricao = "Protestar 4 dias úteis após vencimento";
                        break;
                    case EnumInstrucoes_Sicoob.PROTESTAR_5_DIAS_UTEIS_APOS_VENCIMENTO:
                        this.Codigo = (int)EnumInstrucoes_Sicoob.PROTESTAR_5_DIAS_UTEIS_APOS_VENCIMENTO;
                        this.Descricao = "Protestar 5 dias úteis após vencimento";
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


        #endregion

    }
}
