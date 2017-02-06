using System;
using System.Collections.Generic;
using System.Text;

namespace BoletoNet
{
    #region Enumerado

    public enum EnumCodigoRejeicao_HSBC
    {
        C�digo_do_banco_inv�lido = 01,
        C�digo_do_registro_detalhe_inv�lido = 02,
        C�digo_do_segmento_inv�lido = 03,
        C�digo_do_movimento_n�o_permitido_para_carteira = 04,
        C�digo_de_movimento_inv�lido = 05,
        Tipo_n�mero_de_inscri��o_do_cedente_inv�lidos = 06,
        C�digo_de_Conv�nio_inv�lido_ou_n�o_Cadastrado = 07,
        Nosso_n�mero_inv�lido = 08,
        Nosso_n�mero_duplicado = 09,
        Carteira_inv�lida = 10,
        Forma_de_cadastramento_do_t�tulo_inv�lido = 11,
        Tipo_de_documento_inv�lido = 12,
        Identifica��o_da_emiss�o_do_bloqueto_inv�lida = 13,
        Identifica��o_da_distribui��o_do_bloqueto_inv�lida = 14,
        Caracter�sticas_da_cobran�a_incompat�veis = 15,
        Data_de_vencimento_inv�lida = 16,
        Valor_do_t�tulo_inv�lido = 20,
        Esp�cie_do_t�tulo_inv�lida = 21,
        Aceite_inv�lido = 23,
        Data_da_emiss�o_inv�lida = 24,
        C�digo_de_juros_de_mora_inv�lido = 26,
        Valor_Taxa_de_juros_de_mora_inv�lido = 27,
        C�digo_do_desconto_inv�lido = 28,
        Desconto_a_conceder_n�o_confere = 30,
        Valor_do_abatimento_inv�lido = 33,
        N�mero_seq�encial_inv�lido = 34,
        Abatimento_a_conceder_n�o_confere = 35,
        C�digo_para_protesto_inv�lido = 37,
        Prazo_para_protesto_inv�lido = 38,
        C�digo_para_baixa_devolu��o_inv�lido = 42,
        Prazo_para_baixa_devolu��o_inv�lido = 43,
        C�digo_da_moeda_inv�lido = 44,
        Nome_do_sacado_n�o_informado = 45,
        Tipo_n�mero_de_inscri��o_do_sacado_inv�lido = 46,
        Endere�o_do_sacado_n�o_informado = 47,
        CEP_inv�lido = 48,
        CEP_sem_pra�a_de_cobran�a_n�o_localizado = 49,
        CEP_referente_a_um_Banco_Correspondente = 50,
        Unidade_da_federa��o_inv�lida = 52,
        Controle_do_participante_inv�lido = 53,
        C�digo_documento_ou_lojista_ou_filial_de_entrega_inv�lido = 55,
        C�digo_da_multa_inv�lido = 57,
        Data_da_multa_inv�lida = 58,
        Valor_Percentual_da_multa_inv�lido = 59,
        Contrato_Limite_Desconto_inv�lido_inexistente = 60,
        Valor_da_proposta_abaixo_do_valor_m�nimo_para_opera��es_de_desconto = 61,
        Tipo_de_Impress�o_inv�lido = 62,
        Entrada_para_t�tulo_j�_cadastrado = 63,
        N�mero_de_parcelas_incompat�veis = 64,
        Existe_parcela_com_erro_no_carn� = 65,
        Contrato_de_Limite_Desconto_Inoperante = 66,
        Limite_Insuficiente = 67,
        Nosso_n�mero_n�o_encontrado_para_reemiss�o = 68,
        Cliente_n�o_opera_com_desconto_de_duplicatas = 69,
        Arquivo_n�o_HSBC_ou_lote_duplicado_ou_seq��ncia_de_registro_invalida_n�o_acrescida_de_1 = 70,
        Contrato_Limite_em_processo_de_renova��o = 71,
        Erro_no_c�digo_conv�nio = 72,
        Rejeitado_pela_an�lise_de_cr�dito = 73,
        Vencimento_fora_dos_limites_aprovados = 74,
        Cliente_possui_outra_opera��o_em_andamento = 75,
        Erro_somat�rio_de_registros_do_Lote = 76,
        Seq��ncia_de_registro_inv�lida_n�o_acrescida_de_1 = 77,
        Erro_no_somat�rio_de_lotes_do_arquivo = 78,
        Erro_na_quantidade_de_registros_do_arquivo = 79,
        Sem_registros_trailer_lote_arquivo = 80,
        T�tulos_abaixo_dos_par�metros_de_valores_do_HSBC_para_Desconto = 81,
        Cancelamento_da_opera��o_pelo_cliente = 82,
        Recusado_pela_regra_de_an�lise = 83,
        N�o_Aceito_Desconto_Enviado_Carteira_Simples = 84,
        Registro_predecessor_n�o_encontrado = 85,
        Altera��o_seu_n�mero_e_uso_empresa_n�o_informado = 86,
        A��o_Gerencial = 87,
        Arquivo_fora_do_padr�o_registrado_no_Banco = 88,
        Contrato_Inoperante_para_Meios_Eletr�nicos = 89,
        Registro_protocolo_IED_n�o_encontrado_ou_registro_lote_n�o_informado = 90,
        Situa��o_do_t�tulo_n�o_permitida_para_desconto = 91,
        T�tulo_reservado_para_outra_opera��o = 92,
        Hor�rio_indispon�vel_para_opera��o_de_desconto = 93,
        T�tulo_j�_utilizado_em_outra_opera��o = 94,
        Data_de_gera��o_do_arquivo_diferente_da_data_processamento = 95,
        Opera��o_n�o_confirmada_pelo_cliente_no_Connect_Bank = 96,
        Outras_Irregularidades = 99,

    }

    #endregion

    public class CodigoRejeicao_HSBC : AbstractCodigoRejeicao, ICodigoRejeicao
    {
        #region Construtores

        public CodigoRejeicao_HSBC()
        {
            try
            {
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }

        public CodigoRejeicao_HSBC(string codigo)
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
                this.Banco = new Banco_HSBC();
                var _codigo = (EnumCodigoRejeicao_HSBC)Convert.ToInt32(codigo);

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
