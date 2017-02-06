using BoletoNet;
using Common.Boleto.Enum;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Common.Boleto
{
    public class ArquivoRetorno
    {
        public HeaderRetorno Header { get; set; }
        public List<ArquivoRetornoDetalhe> lstDetalhes { get; set; }

        [SecurityCritical()]
        public ArquivoRetorno ImportarArquivoRetorno(ELayoutArquivoRetorno layout, int codigoBanco, Stream arquivoRetornostream)
        {
            if (layout != ELayoutArquivoRetorno.CNAB400 && layout != ELayoutArquivoRetorno.CNAB240)
                throw new ApplicationException("Layout do arquivo inválido (não implementado)");

            var arquivoRetorno = new ArquivoRetorno();
            arquivoRetorno.lstDetalhes = new List<ArquivoRetornoDetalhe>();

            using (var arquivo = arquivoRetornostream)
            {
                try
                {
                    PreparaArquivoRetornoLeitura(layout, codigoBanco, arquivoRetorno, arquivo);
                }
                catch (Exception ex)
                {
                    arquivo.Dispose();
                    throw new ApplicationException("Erro ao ler arquivo - " + ex.Message.ToString());
                }

            }

            return arquivoRetorno;
        }

        private static void PreparaArquivoRetornoLeitura(ELayoutArquivoRetorno layout, int codigoBanco, ArquivoRetorno arquivoRetorno, Stream arquivo)
        {
            var banco = new BoletoNet.Banco(codigoBanco);
            var nossosNumeros = new List<string>();
            var nossosNumerosConvertido = new List<int>();
            var nossosNumerosInvalidos = new List<string>();

            if (layout == ELayoutArquivoRetorno.CNAB240)
                PreparaLeituraCNAB240(arquivoRetorno, arquivo, banco);

            else
                PreparaLeituraCNAB400(arquivoRetorno, arquivo, banco);
        }

        private static void PreparaLeituraCNAB400(ArquivoRetorno arquivoRetorno, Stream arquivo, BoletoNet.Banco banco)
        {
            var retorno = new BoletoNet.ArquivoRetornoCNAB400();
            retorno.LerArquivoRetorno(banco, arquivo);
            arquivoRetorno.Header = retorno.HeaderRetorno;
            var nossoNumero = 0;

            foreach (var detalhe in retorno.ListaDetalhe)
            {
                if (Int32.TryParse(detalhe.NossoNumero, out nossoNumero))
                {
                    arquivoRetorno.lstDetalhes.Add(new ArquivoRetornoDetalhe
                    {
                        Registro = detalhe.NumeroInscricao,
                        DataOcorrencia = detalhe.DataOcorrencia != DateTime.MinValue ? detalhe.DataOcorrencia : DateTime.Today,
                        NossoNumero = detalhe.NossoNumero,
                        ValorPago = detalhe.ValorPago,
                        DescontoTaxa = detalhe.Descontos + detalhe.Abatimentos,
                        JurosMora = detalhe.JurosMora,
                        TaxaBoleto = detalhe.TarifaCobranca,
                        CodigoOcorrencia = detalhe.CodigoOcorrencia,
                        MotivoCodigoOcorrencia = detalhe.MotivoCodigoOcorrencia,
                        MotivoRejeicao1 = detalhe.MotivosRejeicao,
                        Aceito = detalhe.Aceito,
                        Pago = detalhe.Baixado,
                        Cancelado = detalhe.Cancelado,
                    });
                }
            }
        }

        private static void PreparaLeituraCNAB240(ArquivoRetorno arquivoRetorno, Stream arquivo, BoletoNet.Banco banco)
        {
            var retorno = new BoletoNet.ArquivoRetornoCNAB240();
            retorno.LerArquivoRetorno(banco, arquivo);
            arquivoRetorno.Header = retorno.HeaderRetorno;
            var nossoNumero = 0;

            foreach (var detalhe in retorno.ListaDetalhes)
            {
                if (Int32.TryParse(detalhe.SegmentoT.NossoNumero, out nossoNumero))
                {
                    arquivoRetorno.lstDetalhes.Add(new ArquivoRetornoDetalhe
                    {
                        Registro = arquivoRetorno.Header.RegistroHeader.Substring(18, 15),
                        DataOcorrencia = detalhe.SegmentoU.DataOcorrencia != DateTime.MinValue ? detalhe.SegmentoU.DataOcorrencia : DateTime.Today,
                        NossoNumero = detalhe.SegmentoT.NossoNumero,
                        ValorPago = detalhe.SegmentoU.ValorPagoPeloSacado,
                        TaxaBoleto = detalhe.SegmentoT.ValorTarifas,
                        CodigoOcorrencia = detalhe.SegmentoT.CodigoMovimento.Codigo,
                        DescontoTaxa = detalhe.SegmentoU.ValorDescontoConcedido + detalhe.SegmentoU.ValorAbatimentoConcedido,
                        JurosMora = detalhe.SegmentoU.JurosMultaEncargos,
                        MotivoCodigoOcorrencia = detalhe.SegmentoT.CodigoMovimento.Descricao,
                        MotivoRejeicao1 = detalhe.SegmentoT.CodigoRejeicao1.Codigo.ToString() + " " + detalhe.SegmentoT.CodigoRejeicao1.Descricao,
                        MotivoRejeicao2 = detalhe.SegmentoT.CodigoRejeicao2.Codigo.ToString() + " " + detalhe.SegmentoT.CodigoRejeicao2.Descricao,
                        MotivoRejeicao3 = detalhe.SegmentoT.CodigoRejeicao3.Codigo.ToString() + " " + detalhe.SegmentoT.CodigoRejeicao3.Descricao,
                        MotivoRejeicao4 = detalhe.SegmentoT.CodigoRejeicao4.Codigo.ToString() + " " + detalhe.SegmentoT.CodigoRejeicao4.Descricao,
                        MotivoRejeicao5 = detalhe.SegmentoT.CodigoRejeicao5.Codigo.ToString() + " " + detalhe.SegmentoT.CodigoRejeicao5.Descricao,
                        Aceito = detalhe.SegmentoT.Aceito,
                        Pago = detalhe.SegmentoT.Baixado,
                        Cancelado = detalhe.SegmentoT.Cancelado,
                    });
                }
            }
        }
    }
}
