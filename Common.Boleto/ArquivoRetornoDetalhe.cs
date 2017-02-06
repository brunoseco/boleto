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
    public class ArquivoRetornoDetalhe
    {
        public string Registro { get; set; }
        public DateTime DataOcorrencia { get; set; }
        public string NossoNumero { get; set; }
        public decimal ValorPago { get; set; }
        public decimal DescontoTaxa { get; set; }
        public decimal JurosMora { get; set; }
        public decimal TaxaBoleto { get; set; }
        public int CodigoOcorrencia { get; set; }
        public string MotivoCodigoOcorrencia { get; set; }
        public string MotivoRejeicao1 { get; set; }
        public string MotivoRejeicao2 { get; set; }
        public string MotivoRejeicao3 { get; set; }
        public string MotivoRejeicao4 { get; set; }
        public string MotivoRejeicao5 { get; set; }
        public bool Aceito { get; set; }
        public bool Pago { get; set; }
        public bool Cancelado { get; set; }

    }
}
