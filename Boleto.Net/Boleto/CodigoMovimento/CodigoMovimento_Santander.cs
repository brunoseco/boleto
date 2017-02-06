using System;
using System.Collections.Generic;
using System.Text;

namespace BoletoNet
{
    #region Enumerado

    public enum EnumCodigoMovimento_Santander
    {
        Entrada_confirmada = 02,
        Entrada_rejeitada = 03,
        Transferência_de_carteira_entrada = 04,
        Transferência_de_carteira_baixa = 05,
        Liquidação = 06,
        Baixa = 09,
        Títulos_em_carteira_em_ser = 11,
        Confirmação_recebimento_instrução_de_abatimento = 12,
        Confirmação_recebimento_instrução_de_cancelamento_abatimento = 13,
        Confirmação_recebimento_instrução_alteração_de_vencimento = 14,
        Liquidação_após_baixa_ou_liquidação_título_não_registrado = 17,
        Confirmação_recebimento_instrução_de_protesto = 19,
        Confirmação_recebimento_instrução_de_sustação_cancelamento_de_protesto = 20,
        Remessa_a_cartorio_aponte_em_cartorio = 23,
        Retirada_de_cartorio_e_manutenção_em_carteira = 24,
        Protestado_e_baixado_baixa_por_ter_sido_protestado = 25,
        Instrução_rejeitada = 26,
        Confirmação_do_pedido_de_alteração_de_outros_dados = 27,
        Debito_de_tarifas_custas = 28,
        Ocorrências_do_sacado = 29,
        Alteração_de_dados_rejeitada = 30,
        Título_DDA_reconhecido_pelo_sacado = 51,
        Título_DDA_não_reconhecido_pelo_sacado = 52,
        Título_DDA_não_recusado_pela_CIP = 53,
    }

    #endregion

    public class CodigoMovimento_Santander : AbstractCodigoMovimento, ICodigoMovimento
    {
        #region Construtores

        public CodigoMovimento_Santander()
        {
            try
            {
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }

        public CodigoMovimento_Santander(int codigo)
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
                this.Banco = new Banco_Santander();

                switch ((EnumCodigoMovimento_Santander)codigo)
                {
                    case EnumCodigoMovimento_Santander.Entrada_confirmada:
                        this.Codigo = (int)EnumCodigoMovimento_Santander.Entrada_confirmada;
                        this.Descricao = "Entrada confirmada";
                        break;
                    case EnumCodigoMovimento_Santander.Entrada_rejeitada:
                        this.Codigo = (int)EnumCodigoMovimento_Santander.Entrada_rejeitada;
                        this.Descricao = "Entrada rejeitada";
                        break;
                    case EnumCodigoMovimento_Santander.Transferência_de_carteira_entrada:
                        this.Codigo = (int)EnumCodigoMovimento_Santander.Transferência_de_carteira_entrada;
                        this.Descricao = "Transferência de carteira/entrada";
                        break;
                    case EnumCodigoMovimento_Santander.Transferência_de_carteira_baixa:
                        this.Codigo = (int)EnumCodigoMovimento_Santander.Transferência_de_carteira_baixa;
                        this.Descricao = "transferência de carteira/baixa";
                        break;
                    case EnumCodigoMovimento_Santander.Liquidação:
                        this.Codigo = (int)EnumCodigoMovimento_Santander.Liquidação;
                        this.Descricao = "Liquidação";
                        break;
                    case EnumCodigoMovimento_Santander.Baixa:
                        this.Codigo = (int)EnumCodigoMovimento_Santander.Baixa;
                        this.Descricao = "Baixa";
                        break;
                    case EnumCodigoMovimento_Santander.Títulos_em_carteira_em_ser:
                        this.Codigo = (int)EnumCodigoMovimento_Santander.Títulos_em_carteira_em_ser;
                        this.Descricao = "Títulos em carteira (em ser)";
                        break;
                    case EnumCodigoMovimento_Santander.Confirmação_recebimento_instrução_de_abatimento:
                        this.Codigo = (int)EnumCodigoMovimento_Santander.Confirmação_recebimento_instrução_de_abatimento;
                        this.Descricao = "Confirmação recebimento instrução de abatimento";
                        break;
                    case EnumCodigoMovimento_Santander.Confirmação_recebimento_instrução_de_cancelamento_abatimento:
                        this.Codigo = (int)EnumCodigoMovimento_Santander.Confirmação_recebimento_instrução_de_cancelamento_abatimento;
                        this.Descricao = "Confirmação recebimento instrução de cancelamento de abatimento";
                        break;
                    case EnumCodigoMovimento_Santander.Confirmação_recebimento_instrução_alteração_de_vencimento:
                        this.Codigo = (int)EnumCodigoMovimento_Santander.Confirmação_recebimento_instrução_alteração_de_vencimento;
                        this.Descricao = "Confirmação recebimento instrução alteração de vencimento";
                        break;
                    case EnumCodigoMovimento_Santander.Liquidação_após_baixa_ou_liquidação_título_não_registrado:
                        this.Codigo = (int)EnumCodigoMovimento_Santander.Liquidação_após_baixa_ou_liquidação_título_não_registrado;
                        this.Descricao = "Liquidação após baixa ou liquidação título não registrado";
                        break;
                    case EnumCodigoMovimento_Santander.Confirmação_recebimento_instrução_de_protesto:
                        this.Codigo = (int)EnumCodigoMovimento_Santander.Confirmação_recebimento_instrução_de_protesto;
                        this.Descricao = "Confirmação recebimento instrução de protesto";
                        break;
                    case EnumCodigoMovimento_Santander.Confirmação_recebimento_instrução_de_sustação_cancelamento_de_protesto:
                        this.Codigo = (int)EnumCodigoMovimento_Santander.Confirmação_recebimento_instrução_de_sustação_cancelamento_de_protesto;
                        this.Descricao = "Confirmação recebimento instrução de sustação/cancelamento de protesto";
                        break;
                    case EnumCodigoMovimento_Santander.Remessa_a_cartorio_aponte_em_cartorio:
                        this.Codigo = (int)EnumCodigoMovimento_Santander.Remessa_a_cartorio_aponte_em_cartorio;
                        this.Descricao = "Remessa a cartorio ( aponte em cartorio)";
                        break;
                    case EnumCodigoMovimento_Santander.Retirada_de_cartorio_e_manutenção_em_carteira:
                        this.Codigo = (int)EnumCodigoMovimento_Santander.Retirada_de_cartorio_e_manutenção_em_carteira;
                        this.Descricao = "Retirada de cartorio e manutenção em carteira";
                        break;
                    case EnumCodigoMovimento_Santander.Protestado_e_baixado_baixa_por_ter_sido_protestado:
                        this.Codigo = (int)EnumCodigoMovimento_Santander.Protestado_e_baixado_baixa_por_ter_sido_protestado;
                        this.Descricao = "Protestado e baixado ( baixa por ter sido protestado)";
                        break;
                    case EnumCodigoMovimento_Santander.Instrução_rejeitada:
                        this.Codigo = (int)EnumCodigoMovimento_Santander.Instrução_rejeitada;
                        this.Descricao = "Instrução rejeitada";
                        break;
                    case EnumCodigoMovimento_Santander.Confirmação_do_pedido_de_alteração_de_outros_dados:
                        this.Codigo = (int)EnumCodigoMovimento_Santander.Confirmação_do_pedido_de_alteração_de_outros_dados;
                        this.Descricao = "Confirmação do pedido de alteração de outros dado";
                        break;
                    case EnumCodigoMovimento_Santander.Debito_de_tarifas_custas:
                        this.Codigo = (int)EnumCodigoMovimento_Santander.Debito_de_tarifas_custas;
                        this.Descricao = "Debito de tarifas/custas";
                        break;
                    case EnumCodigoMovimento_Santander.Ocorrências_do_sacado:
                        this.Codigo = (int)EnumCodigoMovimento_Santander.Ocorrências_do_sacado;
                        this.Descricao = "Ocorrências do sacado";
                        break;
                    case EnumCodigoMovimento_Santander.Alteração_de_dados_rejeitada:
                        this.Codigo = (int)EnumCodigoMovimento_Santander.Alteração_de_dados_rejeitada;
                        this.Descricao = "Alteração de dados rejeitada";
                        break;
                    case EnumCodigoMovimento_Santander.Título_DDA_reconhecido_pelo_sacado:
                        this.Codigo = (int)EnumCodigoMovimento_Santander.Título_DDA_reconhecido_pelo_sacado;
                        this.Descricao = "Título DDA reconhecido pelo sacado";
                        break;
                    case EnumCodigoMovimento_Santander.Título_DDA_não_reconhecido_pelo_sacado:
                        this.Codigo = (int)EnumCodigoMovimento_Santander.Título_DDA_não_reconhecido_pelo_sacado;
                        this.Descricao = "Título DDA não reconhecido pelo sacado";
                        break;
                    case EnumCodigoMovimento_Santander.Título_DDA_não_recusado_pela_CIP:
                        this.Codigo = (int)EnumCodigoMovimento_Santander.Título_DDA_não_recusado_pela_CIP;
                        this.Descricao = "Título DDA recusado pela CIP";
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
                    case 2:
                        this.Codigo = (int)EnumCodigoMovimento_Santander.Entrada_confirmada;
                        this.Descricao = "Entrada confirmada";
                        break;
                    case 3:
                        this.Codigo = (int)EnumCodigoMovimento_Santander.Entrada_rejeitada;
                        this.Descricao = "Entrada rejeitada";
                        break;
                    case 4:
                        this.Codigo = (int)EnumCodigoMovimento_Santander.Transferência_de_carteira_entrada;
                        this.Descricao = "Transferência de carteira/entrada";
                        break;
                    case 5:
                        this.Codigo = (int)EnumCodigoMovimento_Santander.Transferência_de_carteira_baixa;
                        this.Descricao = "transferência de carteira/baixa";
                        break;
                    case 6:
                        this.Codigo = (int)EnumCodigoMovimento_Santander.Liquidação;
                        this.Descricao = "Liquidação";
                        break;
                    case 9:
                        this.Codigo = (int)EnumCodigoMovimento_Santander.Baixa;
                        this.Descricao = "Baixa";
                        break;
                    case 11:
                        this.Codigo = (int)EnumCodigoMovimento_Santander.Títulos_em_carteira_em_ser;
                        this.Descricao = "Títulos em carteira (em ser)";
                        break;
                    case 12:
                        this.Codigo = (int)EnumCodigoMovimento_Santander.Confirmação_recebimento_instrução_de_abatimento;
                        this.Descricao = "Confirmação recebimento instrução de abatimento";
                        break;
                    case 13:
                        this.Codigo = (int)EnumCodigoMovimento_Santander.Confirmação_recebimento_instrução_de_cancelamento_abatimento;
                        this.Descricao = "Confirmação recebimento instrução de cancelamento de abatimento";
                        break;
                    case 14:
                        this.Codigo = (int)EnumCodigoMovimento_Santander.Confirmação_recebimento_instrução_alteração_de_vencimento;
                        this.Descricao = "Confirmação recebimento instrução alteração de vencimento";
                        break;
                    case 17:
                        this.Codigo = (int)EnumCodigoMovimento_Santander.Liquidação_após_baixa_ou_liquidação_título_não_registrado;
                        this.Descricao = "Liquidação após baixa ou liquidação título não registrado";
                        break;
                    case 19:
                        this.Codigo = (int)EnumCodigoMovimento_Santander.Confirmação_recebimento_instrução_de_protesto;
                        this.Descricao = "Confirmação recebimento instrução de protesto";
                        break;
                    case 20:
                        this.Codigo = (int)EnumCodigoMovimento_Santander.Confirmação_recebimento_instrução_de_sustação_cancelamento_de_protesto;
                        this.Descricao = "Confirmação recebimento instrução de sustação/cancelamento de protesto";
                        break;
                    case 23:
                        this.Codigo = (int)EnumCodigoMovimento_Santander.Remessa_a_cartorio_aponte_em_cartorio;
                        this.Descricao = "Remessa a cartorio ( aponte em cartorio)";
                        break;
                    case 24:
                        this.Codigo = (int)EnumCodigoMovimento_Santander.Retirada_de_cartorio_e_manutenção_em_carteira;
                        this.Descricao = "Retirada de cartorio e manutenção em carteira";
                        break;
                    case 25:
                        this.Codigo = (int)EnumCodigoMovimento_Santander.Protestado_e_baixado_baixa_por_ter_sido_protestado;
                        this.Descricao = "Protestado e baixado ( baixa por ter sido protestado)";
                        break;
                    case 26:
                        this.Codigo = (int)EnumCodigoMovimento_Santander.Instrução_rejeitada;
                        this.Descricao = "Instrução rejeitada";
                        break;
                    case 27:
                        this.Codigo = (int)EnumCodigoMovimento_Santander.Confirmação_do_pedido_de_alteração_de_outros_dados;
                        this.Descricao = "Confirmação do pedido de alteração de outros dado";
                        break;
                    case 28:
                        this.Codigo = (int)EnumCodigoMovimento_Santander.Debito_de_tarifas_custas;
                        this.Descricao = "Debito de tarifas/custas";
                        break;
                    case 29:
                        this.Codigo = (int)EnumCodigoMovimento_Santander.Ocorrências_do_sacado;
                        this.Descricao = "Ocorrências do sacado";
                        break;
                    case 30:
                        this.Codigo = (int)EnumCodigoMovimento_Santander.Alteração_de_dados_rejeitada;
                        this.Descricao = "Alteração de dados rejeitada";
                        break;
                    case 51:
                        this.Codigo = (int)EnumCodigoMovimento_Santander.Título_DDA_reconhecido_pelo_sacado;
                        this.Descricao = "Título DDA reconhecido pelo sacado";
                        break;
                    case 52:
                        this.Codigo = (int)EnumCodigoMovimento_Santander.Título_DDA_não_reconhecido_pelo_sacado;
                        this.Descricao = "Título DDA não reconhecido pelo sacado";
                        break;
                    case 53:
                        this.Codigo = (int)EnumCodigoMovimento_Santander.Título_DDA_não_recusado_pela_CIP;
                        this.Descricao = "Título DDA recusado pela CIP";
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
