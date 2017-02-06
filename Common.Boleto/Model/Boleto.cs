using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Boleto
{
    public class Boleto
    {
        public string CodigoBanco { get; set; }
        public string CodigoCarteira { get; set; }
        public string CodigoOcorrencia { get; set; }
        public string NossoNumero { get; set; }
        public DateTime DataVencimento { get; set; }
        public decimal Valor { get; set; }
        public decimal ValorSegundaVia { get; set; }
        public decimal ValorMulta { get; set; }
        public decimal ValorJuros { get; set; }
        public bool TemDesconto { get; set; }
        public decimal? ValorDesconto { get; set; }
        public decimal PorcentagemMultaPorAtraso { get; set; }
        public decimal PorcentagemJurosMoraDia { get; set; }
        public bool Carne { get; set; }
        public string TipoArquivoRemessa { get; set; }
        public string CodigoInstrucoes { get; set; }
        public IEnumerable<string> Instrucoes { get; set; }
        public string LocalDePagamento { get; set; }
        public string PathPastaImagens { get; set; }
        public string PathCss { get; set; }
        public string pathArquivosBoletoNet { get; set; }
    }
}
