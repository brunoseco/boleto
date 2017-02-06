using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Boleto
{
    public class Sacado
    {
        public string CpfCnpj { get; set; }
        public string Nome { get; set; }
        public string UF { get; set; }
        public string CEP { get; set; }
        public string Endereco { get; set; }
        public string Cidade { get; set; }
        public string Bairro { get; set; }
        public string Complemento { get; set; }
        public string Email { get; set; }
        public ICollection<AlunoBoleto> Alunos { get; set; }

        public Sacado()
        {
            this.Alunos = new List<AlunoBoleto>();
        }
    }
}
