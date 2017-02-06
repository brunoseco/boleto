using System;
using System.Collections.Generic;
using System.Text;

namespace BoletoNet
{
    // Códigos de rejeicoes de 1 a 64 associados ao código de movimento 3, 26 e 30

    #region Enumerado

    public enum EnumCodigoRejeicao_Santander
    {
        CodigoBancoInvalido = 1,
        CodigoRegistroDetalheInvalido = 2,
        CodigoSegmentoInvalido = 3,
        CodigoMovimentoNaoPermitidoParaCarteira = 4,
        CodigoMovimentoInvalido = 5,
        TipoNumeroInscricaoCedenteInvalidos = 6,
        AgenciaContaDVInvalido = 7,
        NossoNumeroInvalido = 8,
        NossoNumeroDuplicado = 9,
        CarteiraInvalida = 10,
        FormaCadastramentoTituloInvalido = 11,
        TipoDocumentoInvalido = 12,
        IdentificacaoEmissaoBloquetoInvalida = 13,
        IdentificacaoDistribuicaoBloquetoInvalida = 14,
        CaracteristicasCobrancaIncompativeis = 15,
        DataVencimentoInvalida = 16,
        DataVencimentoAnteriorDataEmissao = 17,
        VencimentoForadoPrazodeOperacao = 18,
        TituloCargoBancosCorrespondentesVencimentoInferiorXXDias = 19,
        ValorTituloInvalido = 20,
        EspecieTituloInvalida = 21,
        EspecieNaoPermitidaParaCarteira = 22,
        AceiteInvalido = 23,
        DataEmissaoInvalida = 24,
        DataEmissaoPosteriorData = 25,
        CodigoJurosMoraInvalido = 26,
        ValorJurosMoraInvalido = 27,
        CodigoDescontoInvalido = 28,
        ValorDescontoMaiorIgualValorTitulo = 29,
        DescontoConcederNaoConfere = 30,
        ConcessaoDescontoJaExiste = 31,
        ValorIOFInvalido = 32,
        ValorAbatimentoInvalido = 33,
        ValorAbatimentoMaiorIgualValorTitulo = 34,
        AbatimentoConcederNaoConfere = 35,
        ConcessaoAbatimentoJaExiste = 36,
        CodigoProtestoInvalido = 37,
        PrazoProtestoInvalido = 38,
        PedidoProtestoNaoPermitidoParaTitulo = 39,
        TituloComOrdemProtestoEmitida = 40,
        PedidoCancelamentoParaTitulosSemInstrucaoProtesto = 41,
        CodigoParaBaixaDevolucaoInvalido = 42,
        PrazoParaBaixaDevolucaoInvalido = 43,
        CodigoMoedaInvalido = 44,
        NomeSacadoNaoInformado = 45,
        TipoNumeroInscricaoSacadoInvalidos = 46,
        EnderecoSacadoNaoInformado = 47,
        CEPInvalido = 48,
        CEPSemPracaCobranca = 49,
        CEPReferenteBancoCorrespondente = 50,
        CEPIncompativelComUnidadeFederacao = 51,
        UnidadeFederacaoInvalida = 52,
        TipoNumeroInscricaoSacadorAvalistaInvalido = 53,
        SacadorAvalistaNaoInformado = 54,
        NossoNumeroBancoCorrespondenteNaoInformado = 55,
        CodigoBancoCorrespondenteNaoInformado = 56,
        CodigoMultaInvalido = 57,
        DataMultaInvalida = 58,
        ValorPercentualMultaInvalido = 59,
        MovimentoParaTituloNaoCadastrado = 60,
        AlteracaoAgenciaCobradoraInvalida = 61,
        TipoImpressaoInvalido = 62,
        EntradaParaTituloJaCadastrado = 63,
        NumeroLinhaInvalido = 64,
        CodigoBancoDebitoInvalido = 65,
        AgenciaContaDVParaDebitoInvalido = 66,
        DadosParaDebitoIncompativel = 67,
        ArquivoEmDuplicidade = 88,
        ContratoInexistente = 99,
    }

    #endregion

    public class CodigoRejeicao_Santander : AbstractCodigoRejeicao, ICodigoRejeicao
    {
        #region Construtores

        public CodigoRejeicao_Santander()
        {
            try
            {
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }

        public CodigoRejeicao_Santander(string codigo)
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
                this.Banco = new Banco_Santander();

                var _codigo = (EnumCodigoRejeicao_Santander)Convert.ToInt32(codigo);

                switch (_codigo)
                {
                    case EnumCodigoRejeicao_Santander.CodigoBancoInvalido:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.CodigoBancoInvalido;
                        this.Descricao = "Código do banco inválido";
                        break;
                    case EnumCodigoRejeicao_Santander.CodigoRegistroDetalheInvalido:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.CodigoRegistroDetalheInvalido;
                        this.Descricao = "Código do registro detalhe inválido";
                        break;
                    case EnumCodigoRejeicao_Santander.CodigoSegmentoInvalido:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.CodigoSegmentoInvalido;
                        this.Descricao = "Código do segmento inválido";
                        break;
                    case EnumCodigoRejeicao_Santander.CodigoMovimentoNaoPermitidoParaCarteira:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.CodigoMovimentoNaoPermitidoParaCarteira;
                        this.Descricao = "Código do movimento não permitido para a carteira";
                        break;
                    case EnumCodigoRejeicao_Santander.CodigoMovimentoInvalido:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.CodigoMovimentoInvalido;
                        this.Descricao = "Código do movimento inválido";
                        break;
                    case EnumCodigoRejeicao_Santander.TipoNumeroInscricaoCedenteInvalidos:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.TipoNumeroInscricaoCedenteInvalidos;
                        this.Descricao = "Tipo/Número de inscrição do cendente inválidos";
                        break;
                    case EnumCodigoRejeicao_Santander.AgenciaContaDVInvalido:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.AgenciaContaDVInvalido;
                        this.Descricao = "Agência/Conta/DV inválido";
                        break;
                    case EnumCodigoRejeicao_Santander.NossoNumeroInvalido:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.NossoNumeroInvalido;
                        this.Descricao = "Nosso número inválido";
                        break;
                    case EnumCodigoRejeicao_Santander.NossoNumeroDuplicado:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.NossoNumeroDuplicado;
                        this.Descricao = "Nosso número duplicado";
                        break;
                    case EnumCodigoRejeicao_Santander.CarteiraInvalida:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.CarteiraInvalida;
                        this.Descricao = "Carteira inválida";
                        break;
                    case EnumCodigoRejeicao_Santander.FormaCadastramentoTituloInvalido:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.FormaCadastramentoTituloInvalido;
                        this.Descricao = "Forma de cadastramento do título inválido";
                        break;
                    case EnumCodigoRejeicao_Santander.TipoDocumentoInvalido:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.TipoDocumentoInvalido;
                        this.Descricao = "Tipo de documento inválido";
                        break;
                    case EnumCodigoRejeicao_Santander.IdentificacaoEmissaoBloquetoInvalida:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.IdentificacaoEmissaoBloquetoInvalida;
                        this.Descricao = "Identificação da emissão do bloqueto inválida";
                        break;
                    case EnumCodigoRejeicao_Santander.IdentificacaoDistribuicaoBloquetoInvalida:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.IdentificacaoDistribuicaoBloquetoInvalida;
                        this.Descricao = "Identificação da distribuição do bloqueto inválida";
                        break;
                    case EnumCodigoRejeicao_Santander.CaracteristicasCobrancaIncompativeis:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.CaracteristicasCobrancaIncompativeis;
                        this.Descricao = "Características da cobrança incompatíveis";
                        break;
                    case EnumCodigoRejeicao_Santander.DataVencimentoInvalida:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.DataVencimentoInvalida;
                        this.Descricao = "Data de vencimento inválida";
                        break;
                    case EnumCodigoRejeicao_Santander.DataVencimentoAnteriorDataEmissao:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.DataVencimentoAnteriorDataEmissao;
                        this.Descricao = "Data de vencimento anterior a data de emissão";
                        break;
                    case EnumCodigoRejeicao_Santander.VencimentoForadoPrazodeOperacao:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.VencimentoForadoPrazodeOperacao;
                        this.Descricao = "Vencimento fora do prazo de emissão";
                        break;
                    case EnumCodigoRejeicao_Santander.TituloCargoBancosCorrespondentesVencimentoInferiorXXDias:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.TituloCargoBancosCorrespondentesVencimentoInferiorXXDias;
                        this.Descricao = "Titulo a cargo de bancos correspondentes com vencimento inferior a XX dias";
                        break;
                    case EnumCodigoRejeicao_Santander.ValorTituloInvalido:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.ValorTituloInvalido;
                        this.Descricao = "Valor do título inválido";
                        break;
                    case EnumCodigoRejeicao_Santander.EspecieTituloInvalida:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.EspecieTituloInvalida;
                        this.Descricao = "Espécie do título inválida";
                        break;
                    case EnumCodigoRejeicao_Santander.EspecieNaoPermitidaParaCarteira:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.EspecieNaoPermitidaParaCarteira;
                        this.Descricao = "Espécie não permitida para a carteira";
                        break;
                    case EnumCodigoRejeicao_Santander.AceiteInvalido:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.AceiteInvalido;
                        this.Descricao = "Aceite inválido";
                        break;
                    case EnumCodigoRejeicao_Santander.DataEmissaoInvalida:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.DataEmissaoInvalida;
                        this.Descricao = "Data de emissão inválida";
                        break;
                    case EnumCodigoRejeicao_Santander.DataEmissaoPosteriorData:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.DataEmissaoPosteriorData;
                        this.Descricao = "Data de emissão posterior a data";
                        break;
                    case EnumCodigoRejeicao_Santander.CodigoJurosMoraInvalido:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.CodigoJurosMoraInvalido;
                        this.Descricao = "Código de juros de mora inválido";
                        break;
                    case EnumCodigoRejeicao_Santander.ValorJurosMoraInvalido:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.ValorJurosMoraInvalido;
                        this.Descricao = "Valor/Taxa de juros de mora inválido";
                        break;
                    case EnumCodigoRejeicao_Santander.CodigoDescontoInvalido:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.CodigoDescontoInvalido;
                        this.Descricao = "Código do desconto inválido";
                        break;
                    case EnumCodigoRejeicao_Santander.ValorDescontoMaiorIgualValorTitulo:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.ValorDescontoMaiorIgualValorTitulo;
                        this.Descricao = "Valor do desconto maior ou igual ao valor do título";
                        break;
                    case EnumCodigoRejeicao_Santander.DescontoConcederNaoConfere:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.DescontoConcederNaoConfere;
                        this.Descricao = "Desconto a conceder não confere";
                        break;
                    case EnumCodigoRejeicao_Santander.ConcessaoDescontoJaExiste:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.ConcessaoDescontoJaExiste;
                        this.Descricao = "Concessão de desconto - já existe desconto anterior";
                        break;
                    case EnumCodigoRejeicao_Santander.ValorIOFInvalido:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.ValorIOFInvalido;
                        this.Descricao = "Valor do IOF inválido";
                        break;
                    case EnumCodigoRejeicao_Santander.ValorAbatimentoInvalido:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.ValorAbatimentoInvalido;
                        this.Descricao = "Valor do abatimento inválido";
                        break;
                    case EnumCodigoRejeicao_Santander.ValorAbatimentoMaiorIgualValorTitulo:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.ValorAbatimentoMaiorIgualValorTitulo;
                        this.Descricao = "Valor do abatimento maior ou igual ao valor do título";
                        break;
                    case EnumCodigoRejeicao_Santander.AbatimentoConcederNaoConfere:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.AbatimentoConcederNaoConfere;
                        this.Descricao = "Abatimento a conceder não confere";
                        break;
                    case EnumCodigoRejeicao_Santander.ConcessaoAbatimentoJaExiste:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.ConcessaoAbatimentoJaExiste;
                        this.Descricao = "Concessão de abatimento - já existe abatimendo anterior";
                        break;
                    case EnumCodigoRejeicao_Santander.CodigoProtestoInvalido:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.CodigoProtestoInvalido;
                        this.Descricao = "Código para protesto inválido";
                        break;
                    case EnumCodigoRejeicao_Santander.PrazoProtestoInvalido:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.PrazoProtestoInvalido;
                        this.Descricao = "Prazo para protesto inválido";
                        break;
                    case EnumCodigoRejeicao_Santander.PedidoProtestoNaoPermitidoParaTitulo:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.PedidoProtestoNaoPermitidoParaTitulo;
                        this.Descricao = "Pedido de protesto não permitido para o título";
                        break;
                    case EnumCodigoRejeicao_Santander.TituloComOrdemProtestoEmitida:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.TituloComOrdemProtestoEmitida;
                        this.Descricao = "Título com ordem de protesto emitida";
                        break;
                    case EnumCodigoRejeicao_Santander.PedidoCancelamentoParaTitulosSemInstrucaoProtesto:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.PedidoCancelamentoParaTitulosSemInstrucaoProtesto;
                        this.Descricao = "Pedido de cancelamento para títulos sem instrução de protesto";
                        break;
                    case EnumCodigoRejeicao_Santander.CodigoParaBaixaDevolucaoInvalido:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.CodigoParaBaixaDevolucaoInvalido;
                        this.Descricao = "Código para baixa/devolução inválido";
                        break;
                    case EnumCodigoRejeicao_Santander.PrazoParaBaixaDevolucaoInvalido:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.PrazoParaBaixaDevolucaoInvalido;
                        this.Descricao = "Prazo para baixa/devolução inválido";
                        break;
                    case EnumCodigoRejeicao_Santander.CodigoMoedaInvalido:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.CodigoMoedaInvalido;
                        this.Descricao = "Código da moeda inválido";
                        break;
                    case EnumCodigoRejeicao_Santander.NomeSacadoNaoInformado:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.NomeSacadoNaoInformado;
                        this.Descricao = "Nome do sacado não informado";
                        break;
                    case EnumCodigoRejeicao_Santander.TipoNumeroInscricaoSacadoInvalidos:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.TipoNumeroInscricaoSacadoInvalidos;
                        this.Descricao = "Tipo/Número de inscrição do sacado inválidos";
                        break;
                    case EnumCodigoRejeicao_Santander.EnderecoSacadoNaoInformado:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.EnderecoSacadoNaoInformado;
                        this.Descricao = "Endereço do sacado não informado";
                        break;
                    case EnumCodigoRejeicao_Santander.CEPInvalido:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.CEPInvalido;
                        this.Descricao = "CEP inválido";
                        break;
                    case EnumCodigoRejeicao_Santander.CEPSemPracaCobranca:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.CEPSemPracaCobranca;
                        this.Descricao = "CEP sem praça de cobrança";
                        break;
                    case EnumCodigoRejeicao_Santander.CEPReferenteBancoCorrespondente:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.CEPReferenteBancoCorrespondente;
                        this.Descricao = "CEP referente a um banco correspondente";
                        break;
                    case EnumCodigoRejeicao_Santander.CEPIncompativelComUnidadeFederacao:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.CEPIncompativelComUnidadeFederacao;
                        this.Descricao = "CEP incompatível com a UF";
                        break;
                    case EnumCodigoRejeicao_Santander.UnidadeFederacaoInvalida:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.UnidadeFederacaoInvalida;
                        this.Descricao = "UF inválida";
                        break;
                    case EnumCodigoRejeicao_Santander.TipoNumeroInscricaoSacadorAvalistaInvalido:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.TipoNumeroInscricaoSacadorAvalistaInvalido;
                        this.Descricao = "Tipo/Número de inscrição do sacador/avalista inválido";
                        break;
                    case EnumCodigoRejeicao_Santander.SacadorAvalistaNaoInformado:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.SacadorAvalistaNaoInformado;
                        this.Descricao = "Sacador/Avalista não informado";
                        break;
                    case EnumCodigoRejeicao_Santander.NossoNumeroBancoCorrespondenteNaoInformado:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.NossoNumeroBancoCorrespondenteNaoInformado;
                        this.Descricao = "Nosso número no banco correspondente não informado";
                        break;
                    case EnumCodigoRejeicao_Santander.CodigoBancoCorrespondenteNaoInformado:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.CodigoBancoCorrespondenteNaoInformado;
                        this.Descricao = "Código do banco correspondente não informado";
                        break;
                    case EnumCodigoRejeicao_Santander.CodigoMultaInvalido:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.CodigoMultaInvalido;
                        this.Descricao = "Código da multa inválido";
                        break;
                    case EnumCodigoRejeicao_Santander.DataMultaInvalida:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.DataMultaInvalida;
                        this.Descricao = "Data da multa inválida";
                        break;
                    case EnumCodigoRejeicao_Santander.ValorPercentualMultaInvalido:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.ValorPercentualMultaInvalido;
                        this.Descricao = "Valor/Percentual da multa inválida";
                        break;
                    case EnumCodigoRejeicao_Santander.MovimentoParaTituloNaoCadastrado:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.MovimentoParaTituloNaoCadastrado;
                        this.Descricao = "Movimento para título não cadastrado";
                        break;
                    case EnumCodigoRejeicao_Santander.AlteracaoAgenciaCobradoraInvalida:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.AlteracaoAgenciaCobradoraInvalida;
                        this.Descricao = "Alteração da agência cobradora/dv inválida";
                        break;
                    case EnumCodigoRejeicao_Santander.TipoImpressaoInvalido:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.TipoImpressaoInvalido;
                        this.Descricao = "Tipo de impressão inválido";
                        break;
                    case EnumCodigoRejeicao_Santander.EntradaParaTituloJaCadastrado:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.EntradaParaTituloJaCadastrado;
                        this.Descricao = "Entrada para título já cadastrado";
                        break;
                    case EnumCodigoRejeicao_Santander.NumeroLinhaInvalido:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.NumeroLinhaInvalido;
                        this.Descricao = "Número da linha inválido";
                        break;
                    case EnumCodigoRejeicao_Santander.CodigoBancoDebitoInvalido:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.CodigoBancoDebitoInvalido;
                        this.Descricao = "Código do banco para débito inválido";
                        break;
                    case EnumCodigoRejeicao_Santander.AgenciaContaDVParaDebitoInvalido:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.AgenciaContaDVParaDebitoInvalido;
                        this.Descricao = "Agência/Conta/DV para débito inválido";
                        break;
                    case EnumCodigoRejeicao_Santander.DadosParaDebitoIncompativel:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.DadosParaDebitoIncompativel;
                        this.Descricao = "Dados para débito incompatível com a identificação da emissão do boleto";
                        break;
                    case EnumCodigoRejeicao_Santander.ArquivoEmDuplicidade:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.ArquivoEmDuplicidade;
                        this.Descricao = "Arquivo em duplicidade";
                        break;
                    case EnumCodigoRejeicao_Santander.ContratoInexistente:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.ContratoInexistente;
                        this.Descricao = "Contrato inexistente";
                        break;
                    default:
                        this.Codigo = 0;
                        this.Descricao = "( Selecione )";
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
                    case 1:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.CodigoBancoInvalido;
                        this.Descricao = "Código do banco inválido";
                        break;
                    case 2:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.CodigoRegistroDetalheInvalido;
                        this.Descricao = "Código do registro detalhe inválido";
                        break;
                    case 3:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.CodigoSegmentoInvalido;
                        this.Descricao = "Código do segmento inválido";
                        break;
                    case 4:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.CodigoMovimentoNaoPermitidoParaCarteira;
                        this.Descricao = "Código do movimento não permitido para a carteira";
                        break;
                    case 5:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.CodigoMovimentoInvalido;
                        this.Descricao = "Código do movimento inválido";
                        break;
                    case 6:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.TipoNumeroInscricaoCedenteInvalidos;
                        this.Descricao = "Tipo/Número de inscrição do cendente inválidos";
                        break;
                    case 7:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.AgenciaContaDVInvalido;
                        this.Descricao = "Agência/Conta/DV inválido";
                        break;
                    case 8:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.NossoNumeroInvalido;
                        this.Descricao = "Nosso número inválido";
                        break;
                    case 9:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.NossoNumeroDuplicado;
                        this.Descricao = "Nosso número duplicado";
                        break;
                    case 10:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.CarteiraInvalida;
                        this.Descricao = "Carteira inválida";
                        break;
                    case 11:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.FormaCadastramentoTituloInvalido;
                        this.Descricao = "Forma de cadastramento do título inválido";
                        break;
                    case 12:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.TipoDocumentoInvalido;
                        this.Descricao = "Tipo de documento inválido";
                        break;
                    case 13:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.IdentificacaoEmissaoBloquetoInvalida;
                        this.Descricao = "Identificação da emissão do bloqueto inválida";
                        break;
                    case 14:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.IdentificacaoDistribuicaoBloquetoInvalida;
                        this.Descricao = "Identificação da distribuição do bloqueto inválida";
                        break;
                    case 15:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.CaracteristicasCobrancaIncompativeis;
                        this.Descricao = "Características da cobrança incompatíveis";
                        break;
                    case 16:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.DataVencimentoInvalida;
                        this.Descricao = "Data de vencimento inválida";
                        break;
                    case 17:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.DataVencimentoAnteriorDataEmissao;
                        this.Descricao = "Data de vencimento anterior a data de emissão";
                        break;
                    case 18:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.VencimentoForadoPrazodeOperacao;
                        this.Descricao = "Vencimento fora do prazo de emissão";
                        break;
                    case 19:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.TituloCargoBancosCorrespondentesVencimentoInferiorXXDias;
                        this.Descricao = "Titulo a cargo de bancos correspondentes com vencimento inferior a XX dias";
                        break;
                    case 20:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.ValorTituloInvalido;
                        this.Descricao = "Valor do título inválido";
                        break;
                    case 21:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.EspecieTituloInvalida;
                        this.Descricao = "Espécie do título inválida";
                        break;
                    case 22:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.EspecieNaoPermitidaParaCarteira;
                        this.Descricao = "Espécie não permitida para a carteira";
                        break;
                    case 23:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.AceiteInvalido;
                        this.Descricao = "Aceite inválido";
                        break;
                    case 24:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.DataEmissaoInvalida;
                        this.Descricao = "Data de emissão inválida";
                        break;
                    case 25:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.DataEmissaoPosteriorData;
                        this.Descricao = "Data de emissão posterior a data";
                        break;
                    case 26:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.CodigoJurosMoraInvalido;
                        this.Descricao = "Código de juros de mora inválido";
                        break;
                    case 27:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.ValorJurosMoraInvalido;
                        this.Descricao = "Valor/Taxa de juros de mora inválido";
                        break;
                    case 28:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.CodigoDescontoInvalido;
                        this.Descricao = "Código do desconto inválido";
                        break;
                    case 29:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.ValorDescontoMaiorIgualValorTitulo;
                        this.Descricao = "Valor do desconto maior ou igual ao valor do título";
                        break;
                    case 30:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.DescontoConcederNaoConfere;
                        this.Descricao = "Desconto a conceder não confere";
                        break;
                    case 31:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.ConcessaoDescontoJaExiste;
                        this.Descricao = "Concessão de desconto - já existe desconto anterior";
                        break;
                    case 32:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.ValorIOFInvalido;
                        this.Descricao = "Valor do IOF inválido";
                        break;
                    case 33:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.ValorAbatimentoInvalido;
                        this.Descricao = "Valor do abatimento inválido";
                        break;
                    case 34:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.ValorAbatimentoMaiorIgualValorTitulo;
                        this.Descricao = "Valor do abatimento maior ou igual ao valor do título";
                        break;
                    case 35:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.AbatimentoConcederNaoConfere;
                        this.Descricao = "Abatimento a conceder não confere";
                        break;
                    case 36:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.ConcessaoAbatimentoJaExiste;
                        this.Descricao = "Concessão de abatimento - já existe abatimendo anterior";
                        break;
                    case 37:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.CodigoProtestoInvalido;
                        this.Descricao = "Código para protesto inválido";
                        break;
                    case 38:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.PrazoProtestoInvalido;
                        this.Descricao = "Prazo para protesto inválido";
                        break;
                    case 39:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.PedidoProtestoNaoPermitidoParaTitulo;
                        this.Descricao = "Pedido de protesto não permitido para o título";
                        break;
                    case 40:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.TituloComOrdemProtestoEmitida;
                        this.Descricao = "Título com ordem de protesto emitida";
                        break;
                    case 41:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.PedidoCancelamentoParaTitulosSemInstrucaoProtesto;
                        this.Descricao = "Pedido de cancelamento para títulos sem instrução de protesto";
                        break;
                    case 42:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.CodigoParaBaixaDevolucaoInvalido;
                        this.Descricao = "Código para baixa/devolução inválido";
                        break;
                    case 43:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.PrazoParaBaixaDevolucaoInvalido;
                        this.Descricao = "Prazo para baixa/devolução inválido";
                        break;
                    case 44:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.CodigoMoedaInvalido;
                        this.Descricao = "Código da moeda inválido";
                        break;
                    case 45:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.NomeSacadoNaoInformado;
                        this.Descricao = "Nome do sacado não informado";
                        break;
                    case 46:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.AbatimentoConcederNaoConfere;
                        this.Descricao = "Tipo/Número de inscrição do sacado inválidos";
                        break;
                    case 47:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.EnderecoSacadoNaoInformado;
                        this.Descricao = "Endereço do sacado não informado";
                        break;
                    case 48:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.CEPInvalido;
                        this.Descricao = "CEP inválido";
                        break;
                    case 49:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.CEPSemPracaCobranca;
                        this.Descricao = "CEP sem praça de cobrança";
                        break;
                    case 50:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.CEPReferenteBancoCorrespondente;
                        this.Descricao = "CEP referente a um banco correspondente";
                        break;
                    case 51:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.CEPIncompativelComUnidadeFederacao;
                        this.Descricao = "CEP incompatível com a UF";
                        break;
                    case 52:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.UnidadeFederacaoInvalida;
                        this.Descricao = "UF inválida";
                        break;
                    case 53:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.TipoNumeroInscricaoSacadorAvalistaInvalido;
                        this.Descricao = "Tipo/Número de inscrição do sacador/avalista inválido";
                        break;
                    case 54:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.SacadorAvalistaNaoInformado;
                        this.Descricao = "Sacador/Avalista não informado";
                        break;
                    case 55:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.NossoNumeroBancoCorrespondenteNaoInformado;
                        this.Descricao = "Nosso número no banco correspondente não informado";
                        break;
                    case 56:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.CodigoBancoCorrespondenteNaoInformado;
                        this.Descricao = "Código do banco correspondente não informado";
                        break;
                    case 57:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.CodigoMultaInvalido;
                        this.Descricao = "Código da multa inválido";
                        break;
                    case 58:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.DataMultaInvalida;
                        this.Descricao = "Data da multa inválida";
                        break;
                    case 59:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.ValorPercentualMultaInvalido;
                        this.Descricao = "Valor/Percentual da multa inválida";
                        break;
                    case 60:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.MovimentoParaTituloNaoCadastrado;
                        this.Descricao = "Movimento para título não cadastrado";
                        break;
                    case 61:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.AlteracaoAgenciaCobradoraInvalida;
                        this.Descricao = "Alteração da agência cobradora/dv inválida";
                        break;
                    case 62:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.TipoImpressaoInvalido;
                        this.Descricao = "Tipo de impressão inválido";
                        break;
                    case 63:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.EntradaParaTituloJaCadastrado;
                        this.Descricao = "Entrada para título já cadastrado";
                        break;
                    case 64:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.NumeroLinhaInvalido;
                        this.Descricao = "Número da linha inválido";
                        break;
                    case 65:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.CodigoBancoDebitoInvalido;
                        this.Descricao = "Código do banco para débito inválido";
                        break;
                    case 66:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.AgenciaContaDVParaDebitoInvalido;
                        this.Descricao = "Agência/Conta/DV para débito inválido";
                        break;
                    case 67:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.DadosParaDebitoIncompativel;
                        this.Descricao = "Dados para débito incompatível com a identificação da emissão do boleto";
                        break;
                    case 88:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.ArquivoEmDuplicidade;
                        this.Descricao = "Arquivo em duplicidade";
                        break;
                    case 99:
                        this.Codigo = (int)EnumCodigoRejeicao_Santander.ContratoInexistente;
                        this.Descricao = "Contrato inexistente";
                        break;
                    default:
                        this.Codigo = 0;
                        this.Descricao = "( Selecione )";
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
