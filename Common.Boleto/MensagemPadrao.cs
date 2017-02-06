using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Boleto
{
    public static class MensagemPadrao
    {
        public const string LINHA_VAZIA = "";
        public const string VALORES_EM_REAIS = "****VALORES EXPRESSOS EM REAIS****";
        public const string MULTA_APOS_VENCTO = "Após Vencimento Multa de R$ {0} ({1}%)";
        public const string JUROS_MORA_DIA = "+ Juros de R$ {0} ({1}%) ao dia";
        public const string BANCO_PGTO_APOS_VENCTO = "APOS VENCTO PAGAVEL SOMENTE NO BANCO {0}";
        public const string PAGAVEL_QUALQUER_BANCO = "Pagável em qualquer banco até o vencimento.";
        public const string DESCONTO_PONTUALIDADE = "ATÉ O VENCIMENTO, DESCONTO DE R$ {0} .";
        public const string SEGUNDA_VIA = "Segunda Via - Após Vencimento Não Receber";
    }
}
