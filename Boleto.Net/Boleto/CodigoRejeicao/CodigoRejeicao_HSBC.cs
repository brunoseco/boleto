using System;
using System.Collections.Generic;
using System.Text;

namespace BoletoNet
{
    #region Enumerado

    public enum EnumCodigoRejeicao_HSBC
    {
        Código_do_banco_inválido = 01,
        Código_do_registro_detalhe_inválido = 02,
        Código_do_segmento_inválido = 03,
        Código_do_movimento_não_permitido_para_carteira = 04,
        Código_de_movimento_inválido = 05,
        Tipo_número_de_inscrição_do_cedente_inválidos = 06,
        Código_de_Convênio_inválido_ou_não_Cadastrado = 07,
        Nosso_número_inválido = 08,
        Nosso_número_duplicado = 09,
        Carteira_inválida = 10,
        Forma_de_cadastramento_do_título_inválido = 11,
        Tipo_de_documento_inválido = 12,
        Identificação_da_emissão_do_bloqueto_inválida = 13,
        Identificação_da_distribuição_do_bloqueto_inválida = 14,
        Características_da_cobrança_incompatíveis = 15,
        Data_de_vencimento_inválida = 16,
        Valor_do_título_inválido = 20,
        Espécie_do_título_inválida = 21,
        Aceite_inválido = 23,
        Data_da_emissão_inválida = 24,
        Código_de_juros_de_mora_inválido = 26,
        Valor_Taxa_de_juros_de_mora_inválido = 27,
        Código_do_desconto_inválido = 28,
        Desconto_a_conceder_não_confere = 30,
        Valor_do_abatimento_inválido = 33,
        Número_seqüencial_inválido = 34,
        Abatimento_a_conceder_não_confere = 35,
        Código_para_protesto_inválido = 37,
        Prazo_para_protesto_inválido = 38,
        Código_para_baixa_devolução_inválido = 42,
        Prazo_para_baixa_devolução_inválido = 43,
        Código_da_moeda_inválido = 44,
        Nome_do_sacado_não_informado = 45,
        Tipo_número_de_inscrição_do_sacado_inválido = 46,
        Endereço_do_sacado_não_informado = 47,
        CEP_inválido = 48,
        CEP_sem_praça_de_cobrança_não_localizado = 49,
        CEP_referente_a_um_Banco_Correspondente = 50,
        Unidade_da_federação_inválida = 52,
        Controle_do_participante_inválido = 53,
        Código_documento_ou_lojista_ou_filial_de_entrega_inválido = 55,
        Código_da_multa_inválido = 57,
        Data_da_multa_inválida = 58,
        Valor_Percentual_da_multa_inválido = 59,
        Contrato_Limite_Desconto_inválido_inexistente = 60,
        Valor_da_proposta_abaixo_do_valor_mínimo_para_operações_de_desconto = 61,
        Tipo_de_Impressão_inválido = 62,
        Entrada_para_título_já_cadastrado = 63,
        Número_de_parcelas_incompatíveis = 64,
        Existe_parcela_com_erro_no_carnê = 65,
        Contrato_de_Limite_Desconto_Inoperante = 66,
        Limite_Insuficiente = 67,
        Nosso_número_não_encontrado_para_reemissão = 68,
        Cliente_não_opera_com_desconto_de_duplicatas = 69,
        Arquivo_não_HSBC_ou_lote_duplicado_ou_seqüência_de_registro_invalida_não_acrescida_de_1 = 70,
        Contrato_Limite_em_processo_de_renovação = 71,
        Erro_no_código_convênio = 72,
        Rejeitado_pela_análise_de_crédito = 73,
        Vencimento_fora_dos_limites_aprovados = 74,
        Cliente_possui_outra_operação_em_andamento = 75,
        Erro_somatório_de_registros_do_Lote = 76,
        Seqüência_de_registro_inválida_não_acrescida_de_1 = 77,
        Erro_no_somatório_de_lotes_do_arquivo = 78,
        Erro_na_quantidade_de_registros_do_arquivo = 79,
        Sem_registros_trailer_lote_arquivo = 80,
        Títulos_abaixo_dos_parâmetros_de_valores_do_HSBC_para_Desconto = 81,
        Cancelamento_da_operação_pelo_cliente = 82,
        Recusado_pela_regra_de_análise = 83,
        Não_Aceito_Desconto_Enviado_Carteira_Simples = 84,
        Registro_predecessor_não_encontrado = 85,
        Alteração_seu_número_e_uso_empresa_não_informado = 86,
        Ação_Gerencial = 87,
        Arquivo_fora_do_padrão_registrado_no_Banco = 88,
        Contrato_Inoperante_para_Meios_Eletrônicos = 89,
        Registro_protocolo_IED_não_encontrado_ou_registro_lote_não_informado = 90,
        Situação_do_título_não_permitida_para_desconto = 91,
        Título_reservado_para_outra_operação = 92,
        Horário_indisponível_para_operação_de_desconto = 93,
        Título_já_utilizado_em_outra_operação = 94,
        Data_de_geração_do_arquivo_diferente_da_data_processamento = 95,
        Operação_não_confirmada_pelo_cliente_no_Connect_Bank = 96,
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
