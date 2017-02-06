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
        Transfer�ncia_de_carteira_entrada = 04,
        Transfer�ncia_de_carteira_baixa = 05,
        Liquida��o = 06,
        Baixa = 09,
        T�tulos_em_carteira_em_ser = 11,
        Confirma��o_recebimento_instru��o_de_abatimento = 12,
        Confirma��o_recebimento_instru��o_de_cancelamento_abatimento = 13,
        Confirma��o_recebimento_instru��o_altera��o_de_vencimento = 14,
        Liquida��o_ap�s_baixa_ou_liquida��o_t�tulo_n�o_registrado = 17,
        Confirma��o_recebimento_instru��o_de_protesto = 19,
        Confirma��o_recebimento_instru��o_de_susta��o_cancelamento_de_protesto = 20,
        Remessa_a_cartorio_aponte_em_cartorio = 23,
        Retirada_de_cartorio_e_manuten��o_em_carteira = 24,
        Protestado_e_baixado_baixa_por_ter_sido_protestado = 25,
        Instru��o_rejeitada = 26,
        Confirma��o_do_pedido_de_altera��o_de_outros_dados = 27,
        Debito_de_tarifas_custas = 28,
        Ocorr�ncias_do_sacado = 29,
        Altera��o_de_dados_rejeitada = 30,
        T�tulo_DDA_reconhecido_pelo_sacado = 51,
        T�tulo_DDA_n�o_reconhecido_pelo_sacado = 52,
        T�tulo_DDA_n�o_recusado_pela_CIP = 53,
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
                    case EnumCodigoMovimento_Santander.Transfer�ncia_de_carteira_entrada:
                        this.Codigo = (int)EnumCodigoMovimento_Santander.Transfer�ncia_de_carteira_entrada;
                        this.Descricao = "Transfer�ncia de carteira/entrada";
                        break;
                    case EnumCodigoMovimento_Santander.Transfer�ncia_de_carteira_baixa:
                        this.Codigo = (int)EnumCodigoMovimento_Santander.Transfer�ncia_de_carteira_baixa;
                        this.Descricao = "transfer�ncia de carteira/baixa";
                        break;
                    case EnumCodigoMovimento_Santander.Liquida��o:
                        this.Codigo = (int)EnumCodigoMovimento_Santander.Liquida��o;
                        this.Descricao = "Liquida��o";
                        break;
                    case EnumCodigoMovimento_Santander.Baixa:
                        this.Codigo = (int)EnumCodigoMovimento_Santander.Baixa;
                        this.Descricao = "Baixa";
                        break;
                    case EnumCodigoMovimento_Santander.T�tulos_em_carteira_em_ser:
                        this.Codigo = (int)EnumCodigoMovimento_Santander.T�tulos_em_carteira_em_ser;
                        this.Descricao = "T�tulos em carteira (em ser)";
                        break;
                    case EnumCodigoMovimento_Santander.Confirma��o_recebimento_instru��o_de_abatimento:
                        this.Codigo = (int)EnumCodigoMovimento_Santander.Confirma��o_recebimento_instru��o_de_abatimento;
                        this.Descricao = "Confirma��o recebimento instru��o de abatimento";
                        break;
                    case EnumCodigoMovimento_Santander.Confirma��o_recebimento_instru��o_de_cancelamento_abatimento:
                        this.Codigo = (int)EnumCodigoMovimento_Santander.Confirma��o_recebimento_instru��o_de_cancelamento_abatimento;
                        this.Descricao = "Confirma��o recebimento instru��o de cancelamento de abatimento";
                        break;
                    case EnumCodigoMovimento_Santander.Confirma��o_recebimento_instru��o_altera��o_de_vencimento:
                        this.Codigo = (int)EnumCodigoMovimento_Santander.Confirma��o_recebimento_instru��o_altera��o_de_vencimento;
                        this.Descricao = "Confirma��o recebimento instru��o altera��o de vencimento";
                        break;
                    case EnumCodigoMovimento_Santander.Liquida��o_ap�s_baixa_ou_liquida��o_t�tulo_n�o_registrado:
                        this.Codigo = (int)EnumCodigoMovimento_Santander.Liquida��o_ap�s_baixa_ou_liquida��o_t�tulo_n�o_registrado;
                        this.Descricao = "Liquida��o ap�s baixa ou liquida��o t�tulo n�o registrado";
                        break;
                    case EnumCodigoMovimento_Santander.Confirma��o_recebimento_instru��o_de_protesto:
                        this.Codigo = (int)EnumCodigoMovimento_Santander.Confirma��o_recebimento_instru��o_de_protesto;
                        this.Descricao = "Confirma��o recebimento instru��o de protesto";
                        break;
                    case EnumCodigoMovimento_Santander.Confirma��o_recebimento_instru��o_de_susta��o_cancelamento_de_protesto:
                        this.Codigo = (int)EnumCodigoMovimento_Santander.Confirma��o_recebimento_instru��o_de_susta��o_cancelamento_de_protesto;
                        this.Descricao = "Confirma��o recebimento instru��o de susta��o/cancelamento de protesto";
                        break;
                    case EnumCodigoMovimento_Santander.Remessa_a_cartorio_aponte_em_cartorio:
                        this.Codigo = (int)EnumCodigoMovimento_Santander.Remessa_a_cartorio_aponte_em_cartorio;
                        this.Descricao = "Remessa a cartorio ( aponte em cartorio)";
                        break;
                    case EnumCodigoMovimento_Santander.Retirada_de_cartorio_e_manuten��o_em_carteira:
                        this.Codigo = (int)EnumCodigoMovimento_Santander.Retirada_de_cartorio_e_manuten��o_em_carteira;
                        this.Descricao = "Retirada de cartorio e manuten��o em carteira";
                        break;
                    case EnumCodigoMovimento_Santander.Protestado_e_baixado_baixa_por_ter_sido_protestado:
                        this.Codigo = (int)EnumCodigoMovimento_Santander.Protestado_e_baixado_baixa_por_ter_sido_protestado;
                        this.Descricao = "Protestado e baixado ( baixa por ter sido protestado)";
                        break;
                    case EnumCodigoMovimento_Santander.Instru��o_rejeitada:
                        this.Codigo = (int)EnumCodigoMovimento_Santander.Instru��o_rejeitada;
                        this.Descricao = "Instru��o rejeitada";
                        break;
                    case EnumCodigoMovimento_Santander.Confirma��o_do_pedido_de_altera��o_de_outros_dados:
                        this.Codigo = (int)EnumCodigoMovimento_Santander.Confirma��o_do_pedido_de_altera��o_de_outros_dados;
                        this.Descricao = "Confirma��o do pedido de altera��o de outros dado";
                        break;
                    case EnumCodigoMovimento_Santander.Debito_de_tarifas_custas:
                        this.Codigo = (int)EnumCodigoMovimento_Santander.Debito_de_tarifas_custas;
                        this.Descricao = "Debito de tarifas/custas";
                        break;
                    case EnumCodigoMovimento_Santander.Ocorr�ncias_do_sacado:
                        this.Codigo = (int)EnumCodigoMovimento_Santander.Ocorr�ncias_do_sacado;
                        this.Descricao = "Ocorr�ncias do sacado";
                        break;
                    case EnumCodigoMovimento_Santander.Altera��o_de_dados_rejeitada:
                        this.Codigo = (int)EnumCodigoMovimento_Santander.Altera��o_de_dados_rejeitada;
                        this.Descricao = "Altera��o de dados rejeitada";
                        break;
                    case EnumCodigoMovimento_Santander.T�tulo_DDA_reconhecido_pelo_sacado:
                        this.Codigo = (int)EnumCodigoMovimento_Santander.T�tulo_DDA_reconhecido_pelo_sacado;
                        this.Descricao = "T�tulo DDA reconhecido pelo sacado";
                        break;
                    case EnumCodigoMovimento_Santander.T�tulo_DDA_n�o_reconhecido_pelo_sacado:
                        this.Codigo = (int)EnumCodigoMovimento_Santander.T�tulo_DDA_n�o_reconhecido_pelo_sacado;
                        this.Descricao = "T�tulo DDA n�o reconhecido pelo sacado";
                        break;
                    case EnumCodigoMovimento_Santander.T�tulo_DDA_n�o_recusado_pela_CIP:
                        this.Codigo = (int)EnumCodigoMovimento_Santander.T�tulo_DDA_n�o_recusado_pela_CIP;
                        this.Descricao = "T�tulo DDA recusado pela CIP";
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
                        this.Codigo = (int)EnumCodigoMovimento_Santander.Transfer�ncia_de_carteira_entrada;
                        this.Descricao = "Transfer�ncia de carteira/entrada";
                        break;
                    case 5:
                        this.Codigo = (int)EnumCodigoMovimento_Santander.Transfer�ncia_de_carteira_baixa;
                        this.Descricao = "transfer�ncia de carteira/baixa";
                        break;
                    case 6:
                        this.Codigo = (int)EnumCodigoMovimento_Santander.Liquida��o;
                        this.Descricao = "Liquida��o";
                        break;
                    case 9:
                        this.Codigo = (int)EnumCodigoMovimento_Santander.Baixa;
                        this.Descricao = "Baixa";
                        break;
                    case 11:
                        this.Codigo = (int)EnumCodigoMovimento_Santander.T�tulos_em_carteira_em_ser;
                        this.Descricao = "T�tulos em carteira (em ser)";
                        break;
                    case 12:
                        this.Codigo = (int)EnumCodigoMovimento_Santander.Confirma��o_recebimento_instru��o_de_abatimento;
                        this.Descricao = "Confirma��o recebimento instru��o de abatimento";
                        break;
                    case 13:
                        this.Codigo = (int)EnumCodigoMovimento_Santander.Confirma��o_recebimento_instru��o_de_cancelamento_abatimento;
                        this.Descricao = "Confirma��o recebimento instru��o de cancelamento de abatimento";
                        break;
                    case 14:
                        this.Codigo = (int)EnumCodigoMovimento_Santander.Confirma��o_recebimento_instru��o_altera��o_de_vencimento;
                        this.Descricao = "Confirma��o recebimento instru��o altera��o de vencimento";
                        break;
                    case 17:
                        this.Codigo = (int)EnumCodigoMovimento_Santander.Liquida��o_ap�s_baixa_ou_liquida��o_t�tulo_n�o_registrado;
                        this.Descricao = "Liquida��o ap�s baixa ou liquida��o t�tulo n�o registrado";
                        break;
                    case 19:
                        this.Codigo = (int)EnumCodigoMovimento_Santander.Confirma��o_recebimento_instru��o_de_protesto;
                        this.Descricao = "Confirma��o recebimento instru��o de protesto";
                        break;
                    case 20:
                        this.Codigo = (int)EnumCodigoMovimento_Santander.Confirma��o_recebimento_instru��o_de_susta��o_cancelamento_de_protesto;
                        this.Descricao = "Confirma��o recebimento instru��o de susta��o/cancelamento de protesto";
                        break;
                    case 23:
                        this.Codigo = (int)EnumCodigoMovimento_Santander.Remessa_a_cartorio_aponte_em_cartorio;
                        this.Descricao = "Remessa a cartorio ( aponte em cartorio)";
                        break;
                    case 24:
                        this.Codigo = (int)EnumCodigoMovimento_Santander.Retirada_de_cartorio_e_manuten��o_em_carteira;
                        this.Descricao = "Retirada de cartorio e manuten��o em carteira";
                        break;
                    case 25:
                        this.Codigo = (int)EnumCodigoMovimento_Santander.Protestado_e_baixado_baixa_por_ter_sido_protestado;
                        this.Descricao = "Protestado e baixado ( baixa por ter sido protestado)";
                        break;
                    case 26:
                        this.Codigo = (int)EnumCodigoMovimento_Santander.Instru��o_rejeitada;
                        this.Descricao = "Instru��o rejeitada";
                        break;
                    case 27:
                        this.Codigo = (int)EnumCodigoMovimento_Santander.Confirma��o_do_pedido_de_altera��o_de_outros_dados;
                        this.Descricao = "Confirma��o do pedido de altera��o de outros dado";
                        break;
                    case 28:
                        this.Codigo = (int)EnumCodigoMovimento_Santander.Debito_de_tarifas_custas;
                        this.Descricao = "Debito de tarifas/custas";
                        break;
                    case 29:
                        this.Codigo = (int)EnumCodigoMovimento_Santander.Ocorr�ncias_do_sacado;
                        this.Descricao = "Ocorr�ncias do sacado";
                        break;
                    case 30:
                        this.Codigo = (int)EnumCodigoMovimento_Santander.Altera��o_de_dados_rejeitada;
                        this.Descricao = "Altera��o de dados rejeitada";
                        break;
                    case 51:
                        this.Codigo = (int)EnumCodigoMovimento_Santander.T�tulo_DDA_reconhecido_pelo_sacado;
                        this.Descricao = "T�tulo DDA reconhecido pelo sacado";
                        break;
                    case 52:
                        this.Codigo = (int)EnumCodigoMovimento_Santander.T�tulo_DDA_n�o_reconhecido_pelo_sacado;
                        this.Descricao = "T�tulo DDA n�o reconhecido pelo sacado";
                        break;
                    case 53:
                        this.Codigo = (int)EnumCodigoMovimento_Santander.T�tulo_DDA_n�o_recusado_pela_CIP;
                        this.Descricao = "T�tulo DDA recusado pela CIP";
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
