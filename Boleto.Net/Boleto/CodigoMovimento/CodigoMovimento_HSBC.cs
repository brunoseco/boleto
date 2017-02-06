using System;
using System.Collections.Generic;
using System.Text;

namespace BoletoNet
{
    #region Enumerado

    public enum EnumCodigoMovimento_HSBC
    {
        Entrada_confirmada = 02,
        Entrada_rejeitada = 03,
        Transferência_de_Carteira_Entrada = 04,
        Liquidação = 06,
        Baixa = 09,
        Reembolso = 10,
        Conciliação_Mensal_Títulos_em_Ser = 11,
        Confirmação_recebimento_instrução_de_abatimento = 12,
        Confirmação_recebimento_instrução_de_cancelamento_abatimento = 13,
        Confirmação_recebimento_instrução_alteração_de_vencimento = 14,
        Liquidação_após_baixa_ou_liquidação_título_não_registrado = 17,
        Confirmação_recebimento_instrução_de_protesto = 19,
        Confirmação_recebimento_instrução_de_sustação_cancelamento_de_protesto = 20,
        Remessa_a_cartório_aponte_em_cartório = 23,
        Protestado_e_baixado_baixa_por_ter_sido_protestado = 25,
        Instrução_rejeitada = 26,
        Alteração_de_Instrução_pelo_Cedente = 27,
        Despesas_de_cartório = 28,
        Alteração_de_dados_rejeitada = 30,
        Transferência_de_Carteira_Rejeitada = 31,
        Título_DDA_aceito_pelo_sacado = 51,
        Título_DDA_não_reconhecido_pelo_sacado = 52,
    }

    #endregion

    public class CodigoMovimento_HSBC : AbstractCodigoMovimento, ICodigoMovimento
    {
        #region Construtores

        public CodigoMovimento_HSBC()
        {
            try
            {
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }

        public CodigoMovimento_HSBC(int codigo)
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
                this.Banco = new Banco_HSBC();

                switch ((EnumCodigoMovimento_HSBC)codigo)
                {
                    case EnumCodigoMovimento_HSBC.Alteração_de_dados_rejeitada:
                        this.Codigo = (int)EnumCodigoMovimento_HSBC.Alteração_de_dados_rejeitada;
                        this.Descricao = "Alteração de dados rejeitada";
                        break;
                    case EnumCodigoMovimento_HSBC.Alteração_de_Instrução_pelo_Cedente:
                        this.Codigo = (int)EnumCodigoMovimento_HSBC.Alteração_de_Instrução_pelo_Cedente;
                        this.Descricao = "Alteração de Instrução pelo Cedente";
                        break;
                    case EnumCodigoMovimento_HSBC.Baixa:
                        this.Codigo = (int)EnumCodigoMovimento_HSBC.Baixa;
                        this.Descricao = "Baixa";
                        break;
                    case EnumCodigoMovimento_HSBC.Conciliação_Mensal_Títulos_em_Ser:
                        this.Codigo = (int)EnumCodigoMovimento_HSBC.Conciliação_Mensal_Títulos_em_Ser;
                        this.Descricao = "Conciliação Mensal Títulos em Ser";
                        break;
                    case EnumCodigoMovimento_HSBC.Confirmação_recebimento_instrução_alteração_de_vencimento:
                        this.Codigo = (int)EnumCodigoMovimento_HSBC.Confirmação_recebimento_instrução_alteração_de_vencimento;
                        this.Descricao = "Confirmação recebimento instrução alteração de vencimento";
                        break;
                    case EnumCodigoMovimento_HSBC.Confirmação_recebimento_instrução_de_abatimento:
                        this.Codigo = (int)EnumCodigoMovimento_HSBC.Confirmação_recebimento_instrução_de_abatimento;
                        this.Descricao = "Confirmação recebimento instrução de abatimento";
                        break;
                    case EnumCodigoMovimento_HSBC.Confirmação_recebimento_instrução_de_cancelamento_abatimento:
                        this.Codigo = (int)EnumCodigoMovimento_HSBC.Confirmação_recebimento_instrução_de_cancelamento_abatimento;
                        this.Descricao = "Confirmação recebimento instrução de cancelamento abatimento";
                        break;
                    case EnumCodigoMovimento_HSBC.Confirmação_recebimento_instrução_de_protesto:
                        this.Codigo = (int)EnumCodigoMovimento_HSBC.Confirmação_recebimento_instrução_de_protesto;
                        this.Descricao = "Confirmação recebimento instrução de protesto";
                        break;
                    case EnumCodigoMovimento_HSBC.Confirmação_recebimento_instrução_de_sustação_cancelamento_de_protesto:
                        this.Codigo = (int)EnumCodigoMovimento_HSBC.Confirmação_recebimento_instrução_de_sustação_cancelamento_de_protesto;
                        this.Descricao = "Confirmação recebimento instrução de sustação cancelamento de protesto";
                        break;
                    case EnumCodigoMovimento_HSBC.Despesas_de_cartório:
                        this.Codigo = (int)EnumCodigoMovimento_HSBC.Despesas_de_cartório;
                        this.Descricao = "Despesas de cartório";
                        break;
                    case EnumCodigoMovimento_HSBC.Entrada_confirmada:
                        this.Codigo = (int)EnumCodigoMovimento_HSBC.Entrada_confirmada;
                        this.Descricao = "Entrada confirmada";
                        break;
                    case EnumCodigoMovimento_HSBC.Entrada_rejeitada:
                        this.Codigo = (int)EnumCodigoMovimento_HSBC.Entrada_rejeitada;
                        this.Descricao = "Entrada rejeitada";
                        break;
                    case EnumCodigoMovimento_HSBC.Instrução_rejeitada:
                        this.Codigo = (int)EnumCodigoMovimento_HSBC.Instrução_rejeitada;
                        this.Descricao = "Instrução rejeitada";
                        break;
                    case EnumCodigoMovimento_HSBC.Liquidação:
                        this.Codigo = (int)EnumCodigoMovimento_HSBC.Liquidação;
                        this.Descricao = "Liquidação";
                        break;
                    case EnumCodigoMovimento_HSBC.Liquidação_após_baixa_ou_liquidação_título_não_registrado:
                        this.Codigo = (int)EnumCodigoMovimento_HSBC.Liquidação_após_baixa_ou_liquidação_título_não_registrado;
                        this.Descricao = "Liquidação após baixa ou liquidação título não registrado";
                        break;
                    case EnumCodigoMovimento_HSBC.Protestado_e_baixado_baixa_por_ter_sido_protestado:
                        this.Codigo = (int)EnumCodigoMovimento_HSBC.Protestado_e_baixado_baixa_por_ter_sido_protestado;
                        this.Descricao = "Protestado e baixado baixa por ter sido protestado";
                        break;
                    case EnumCodigoMovimento_HSBC.Reembolso:
                        this.Codigo = (int)EnumCodigoMovimento_HSBC.Reembolso;
                        this.Descricao = "Reembolso";
                        break;
                    case EnumCodigoMovimento_HSBC.Remessa_a_cartório_aponte_em_cartório:
                        this.Codigo = (int)EnumCodigoMovimento_HSBC.Remessa_a_cartório_aponte_em_cartório;
                        this.Descricao = "Remessa a cartório aponte em cartório";
                        break;
                    case EnumCodigoMovimento_HSBC.Título_DDA_aceito_pelo_sacado:
                        this.Codigo = (int)EnumCodigoMovimento_HSBC.Título_DDA_aceito_pelo_sacado;
                        this.Descricao = "Título DDA aceito pelo sacado";
                        break;
                    case EnumCodigoMovimento_HSBC.Título_DDA_não_reconhecido_pelo_sacado:
                        this.Codigo = (int)EnumCodigoMovimento_HSBC.Título_DDA_não_reconhecido_pelo_sacado;
                        this.Descricao = "Título DDA não reconhecido pelo sacado";
                        break;
                    case EnumCodigoMovimento_HSBC.Transferência_de_Carteira_Entrada:
                        this.Codigo = (int)EnumCodigoMovimento_HSBC.Transferência_de_Carteira_Entrada;
                        this.Descricao = "Transferência de Carteira Entrada";
                        break;
                    case EnumCodigoMovimento_HSBC.Transferência_de_Carteira_Rejeitada:
                        this.Codigo = (int)EnumCodigoMovimento_HSBC.Transferência_de_Carteira_Rejeitada;
                        this.Descricao = "Transferência de Carteira Rejeitada";
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

        private void Ler(int codigo)
        {
            try
            {
                switch (codigo)
                {
                    case 30:
                        this.Codigo = (int)EnumCodigoMovimento_HSBC.Alteração_de_dados_rejeitada;
                        this.Descricao = "Alteração de dados rejeitada";
                        break;
                    case 27:
                        this.Codigo = (int)EnumCodigoMovimento_HSBC.Alteração_de_Instrução_pelo_Cedente;
                        this.Descricao = "Alteração de Instrução pelo Cedente";
                        break;
                    case 9:
                        this.Codigo = (int)EnumCodigoMovimento_HSBC.Baixa;
                        this.Descricao = "Baixa";
                        break;
                    case 11:
                        this.Codigo = (int)EnumCodigoMovimento_HSBC.Conciliação_Mensal_Títulos_em_Ser;
                        this.Descricao = "Conciliação Mensal Títulos em Ser";
                        break;
                    case 14:
                        this.Codigo = (int)EnumCodigoMovimento_HSBC.Confirmação_recebimento_instrução_alteração_de_vencimento;
                        this.Descricao = "Confirmação recebimento instrução alteração de vencimento";
                        break;
                    case 12:
                        this.Codigo = (int)EnumCodigoMovimento_HSBC.Confirmação_recebimento_instrução_de_abatimento;
                        this.Descricao = "Confirmação recebimento instrução de abatimento";
                        break;
                    case 13:
                        this.Codigo = (int)EnumCodigoMovimento_HSBC.Confirmação_recebimento_instrução_de_cancelamento_abatimento;
                        this.Descricao = "Confirmação recebimento instrução de cancelamento abatimento";
                        break;
                    case 19:
                        this.Codigo = (int)EnumCodigoMovimento_HSBC.Confirmação_recebimento_instrução_de_protesto;
                        this.Descricao = "Confirmação recebimento instrução de protesto";
                        break;
                    case 20:
                        this.Codigo = (int)EnumCodigoMovimento_HSBC.Confirmação_recebimento_instrução_de_sustação_cancelamento_de_protesto;
                        this.Descricao = "Confirmação recebimento instrução de sustação cancelamento de protesto";
                        break;
                    case 28:
                        this.Codigo = (int)EnumCodigoMovimento_HSBC.Despesas_de_cartório;
                        this.Descricao = "Despesas de cartório";
                        break;
                    case 2:
                        this.Codigo = (int)EnumCodigoMovimento_HSBC.Entrada_confirmada;
                        this.Descricao = "Entrada confirmada";
                        break;
                    case 3:
                        this.Codigo = (int)EnumCodigoMovimento_HSBC.Entrada_rejeitada;
                        this.Descricao = "Entrada rejeitada";
                        break;
                    case 26:
                        this.Codigo = (int)EnumCodigoMovimento_HSBC.Instrução_rejeitada;
                        this.Descricao = "Instrução rejeitada";
                        break;
                    case 6:
                        this.Codigo = (int)EnumCodigoMovimento_HSBC.Liquidação;
                        this.Descricao = "Liquidação";
                        break;
                    case 17:
                        this.Codigo = (int)EnumCodigoMovimento_HSBC.Liquidação_após_baixa_ou_liquidação_título_não_registrado;
                        this.Descricao = "Liquidação após baixa ou liquidação título não registrado";
                        break;
                    case 25:
                        this.Codigo = (int)EnumCodigoMovimento_HSBC.Protestado_e_baixado_baixa_por_ter_sido_protestado;
                        this.Descricao = "Protestado e baixado baixa por ter sido protestado";
                        break;
                    case 10:
                        this.Codigo = (int)EnumCodigoMovimento_HSBC.Reembolso;
                        this.Descricao = "Reembolso";
                        break;
                    case 23:
                        this.Codigo = (int)EnumCodigoMovimento_HSBC.Remessa_a_cartório_aponte_em_cartório;
                        this.Descricao = "Remessa a cartório aponte em cartório";
                        break;
                    case 51:
                        this.Codigo = (int)EnumCodigoMovimento_HSBC.Título_DDA_aceito_pelo_sacado;
                        this.Descricao = "Título DDA aceito pelo sacado";
                        break;
                    case 52:
                        this.Codigo = (int)EnumCodigoMovimento_HSBC.Título_DDA_não_reconhecido_pelo_sacado;
                        this.Descricao = "Título DDA não reconhecido pelo sacado";
                        break;
                    case 4:
                        this.Codigo = (int)EnumCodigoMovimento_HSBC.Transferência_de_Carteira_Entrada;
                        this.Descricao = "Transferência de Carteira Entrada";
                        break;
                    case 31:
                        this.Codigo = (int)EnumCodigoMovimento_HSBC.Transferência_de_Carteira_Rejeitada;
                        this.Descricao = "Transferência de Carteira Rejeitada";
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
