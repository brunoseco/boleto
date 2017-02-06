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

    public class FileInfoRemessa
    {

        public byte[] ArquivoDeRemessaStream { get; set; }
        public string NomeDoArquivo { get; set; }

    }
    public class ArquivoRemessa
    {
        [SecurityCritical()]
        public FileInfoRemessa GerarRemessa(int codigoBanco, decimal taxaBoleto, int numeroArquivoRemessa, BoletoCobranca cobranca)
        {
            var lstCobrancas = new List<BoletoCobranca>();
            lstCobrancas.Add(cobranca);
            return GerarRemessa(codigoBanco, taxaBoleto, numeroArquivoRemessa, lstCobrancas);
        }

        [SecurityCritical()]
        public FileInfoRemessa GerarRemessa(int codigoBanco, decimal taxaBoleto, int numeroArquivoRemessa, IEnumerable<BoletoCobranca> cobrancas)
        {
            int contagem = cobrancas.Count();
            string convenio = "";
            var banco = new BoletoNet.Banco(codigoBanco);

            var caminhoArquivo = System.IO.Path.GetTempFileName();

            var cobranca = cobrancas.First();

            var boletoCedente = new BoletoNet.Cedente(
               Convert.ToString(cobranca.Cedente.CpfCnpj), Convert.ToString(this.RemoveCaracteresEspeciais(cobranca.Cedente.Nome)),
               Convert.ToString(cobranca.Cedente.Agencia), Convert.ToString(cobranca.Cedente.DigitoAgencia),
               Convert.ToString(cobranca.Cedente.ContaCorrente), Convert.ToString(cobranca.Cedente.DigitoConta)
           );

            boletoCedente.Endereco = new BoletoNet.Endereco();
            boletoCedente.Endereco.UF = Convert.ToString(cobranca.Cedente.UF);
            boletoCedente.Endereco.CEP = Convert.ToString(cobranca.Cedente.CEP);
            boletoCedente.Endereco.End = Convert.ToString(this.RemoveCaracteresEspeciais(cobranca.Cedente.Endereco)) + " - " + Convert.ToString(cobranca.Cedente.Cidade);
            boletoCedente.Carteira = cobranca.Boleto.CodigoCarteira;
            boletoCedente.Codigo = cobranca.Cedente.CodigoConvenio.ToString();

            if (banco.Codigo.Equals((int)EBanco.Sicoob))
                boletoCedente.Convenio = 0;
            else
                boletoCedente.Convenio = cobranca.Cedente.CodigoConvenio;

            convenio = cobranca.Cedente.CodigoConvenio.ToString();

            var boletos = new BoletoNet.Boletos();

            BoletoNet.ArquivoRemessa arquivoRemessa;

            if (cobranca.Boleto.TipoArquivoRemessa.Equals("240"))
                arquivoRemessa = new BoletoNet.ArquivoRemessa(BoletoNet.TipoArquivo.CNAB240);
            else
                arquivoRemessa = new BoletoNet.ArquivoRemessa(BoletoNet.TipoArquivo.CNAB400);

            foreach (var ordemDeCobranca in cobrancas)
            {
                string nossoNumero = "";

                if (banco.Codigo.Equals(1))
                    nossoNumero = ordemDeCobranca.Cedente.CodigoConvenio.ToString() + ordemDeCobranca.Boleto.NossoNumero.PadLeft(10, '0');
                else
                    nossoNumero = ordemDeCobranca.Boleto.NossoNumero.PadLeft(7, '0');

                var boleto = new BoletoNet.Boleto(ordemDeCobranca.Boleto.DataVencimento, ordemDeCobranca.Boleto.Valor, boletoCedente.Carteira.PadLeft(2, '0'), nossoNumero, boletoCedente);
                
                boleto.NumeroDocumento = ordemDeCobranca.Boleto.NossoNumero;
                
                if (!boletoCedente.Carteira.IndexOf('-').Equals(-1))
                {
                    var carteiraEVariacao = boletoCedente.Carteira.Split('-');
                    boleto.Carteira = carteiraEVariacao[0].PadLeft(2, '0');
                    boleto.VariacaoCarteira = carteiraEVariacao[1];
                }
                else
                    boleto.Carteira = boletoCedente.Carteira.PadLeft(2, '0');

                var remessa = new BoletoNet.Remessa();
                boleto.Remessa = remessa;

                this.ConfiguraEspecieDocumento(banco, ordemDeCobranca, boleto);

                boleto.PercMulta = ordemDeCobranca.Boleto.PorcentagemMultaPorAtraso;
                boleto.PercJurosMora = ordemDeCobranca.Boleto.PorcentagemJurosMoraDia;

                if (ordemDeCobranca.Boleto.CodigoInstrucoes != null &&
                    ordemDeCobranca.Boleto.CodigoInstrucoes != string.Empty)
                {
                    this.ConfiguraCodigoInstrucao(codigoBanco, boleto, ordemDeCobranca.Boleto.CodigoInstrucoes);
                }

                var instrucoes = new BoletoNet.Instrucao(Convert.ToInt32(codigoBanco));

                foreach (var item in ordemDeCobranca.Boleto.Instrucoes)
                {
                    var validItem = item;

                    if (ordemDeCobranca.Boleto.TemDesconto)
                        if (item.Equals(MensagemPadrao.DESCONTO_PONTUALIDADE))
                            validItem = string.Format(item, ordemDeCobranca.Boleto.ValorDesconto);

                    if (item.Equals(MensagemPadrao.MULTA_APOS_VENCTO))
                        validItem = string.Format(item, Math.Round(boleto.ValorBoleto * (boleto.PercMulta / 100M), 2), boleto.PercMulta);
                    else if (item.Equals(MensagemPadrao.JUROS_MORA_DIA))
                        validItem = string.Format(item, Math.Round(boleto.ValorBoleto * (boleto.PercJurosMora / 100M), 2), boleto.PercJurosMora);
                    else if (item.Equals(MensagemPadrao.BANCO_PGTO_APOS_VENCTO))
                        validItem = string.Format(item, boleto.Banco.Nome.ToUpper());

                    instrucoes.Descricao += validItem + "<br/>";
                }

                boleto.Instrucoes.Add(instrucoes);

                boleto.BancoGeraBoleto = false;
                boleto.ValorMulta = Math.Round(boleto.ValorBoleto * (boleto.PercMulta / 100M), 2);
                boleto.PercJurosMora = ordemDeCobranca.Boleto.PorcentagemJurosMoraDia;

                if (ordemDeCobranca.Boleto.TemDesconto)
                {
                    boleto.DataDesconto = boleto.DataVencimento;
                    boleto.ValorDesconto = ordemDeCobranca.Boleto.ValorDesconto.Value;
                }

                boleto.DataDocumento = DateTime.Today;
                boleto.DataProcessamento = DateTime.Today;

                boleto.Sacado = new BoletoNet.Sacado(ordemDeCobranca.Sacado.CpfCnpj, ordemDeCobranca.Sacado.Nome);
                boleto.Sacado.Endereco.End = ordemDeCobranca.Sacado.Endereco;
                boleto.Sacado.Endereco.Bairro = ordemDeCobranca.Sacado.Bairro;
                boleto.Sacado.Endereco.Cidade = ordemDeCobranca.Sacado.Cidade;
                boleto.Sacado.Endereco.CEP = ordemDeCobranca.Sacado.CEP;
                boleto.Sacado.Endereco.UF = ordemDeCobranca.Cedente.UF;
                boleto.Sacado.Endereco.Complemento = ordemDeCobranca.Sacado.Complemento;
                boleto.Sacado.Endereco.Email = ordemDeCobranca.Sacado.Email;
                boleto.Sacado.Endereco.CEP = ordemDeCobranca.Sacado.CEP;

                boleto.Sacado.Email = ordemDeCobranca.Sacado.Email;
                boleto.CodigoOcorrencia = ordemDeCobranca.Boleto.CodigoOcorrencia;

                boletos.Add(boleto);
            }

            var arquivoDeRemessaStream = arquivoRemessa.GerarArquivoRemessa(convenio, banco, boletoCedente, boletos, caminhoArquivo, numeroArquivoRemessa);
            if (arquivoDeRemessaStream != null)
            {
                var arquivo = new FileInfo(caminhoArquivo);
                var novoCaminho = arquivo.DirectoryName;

                string novoNome = "";

                var txtNroArquivoRemessa = numeroArquivoRemessa.ToString().Length < 10 ? "0" + numeroArquivoRemessa.ToString() : numeroArquivoRemessa.ToString().Substring(numeroArquivoRemessa.ToString().Length - 2, 2);
                var nroArquivoRemessa = numeroArquivoRemessa > 10 ? txtNroArquivoRemessa.Substring(txtNroArquivoRemessa.Length - 2, 2) : "0" + txtNroArquivoRemessa;

                if (banco.Nome.Equals("Bradesco") || banco.Nome.Equals("Itaú"))
                    novoNome = "CB" + DateTime.Now.Day.ToString("D2") + DateTime.Now.Month.ToString("D2") + nroArquivoRemessa + ".REM";
                else
                    novoNome = "CB_REMESSA_" + DateTime.Now.ToString().Replace("/", "").Replace(" ", "_").Replace(":", "") + ".REM";

                novoCaminho = novoCaminho + "\\" + novoNome;

                if (banco.Codigo.Equals((int)EBanco.Sicoob))
                    ConvertEncondingToDefault(caminhoArquivo);

                caminhoArquivo = novoCaminho;
            }

            var dadosArquivoRemessa = new FileInfoRemessa {
                ArquivoDeRemessaStream = arquivoDeRemessaStream,
                NomeDoArquivo = Path.GetFileName(caminhoArquivo)
            };

            return dadosArquivoRemessa;
        }

        private static void ConvertEncondingToDefault(string caminhoArquivo)
        {
            var conteudo = File.ReadAllBytes(caminhoArquivo);
            var bytesANSI = Encoding.Convert(Encoding.UTF8, Encoding.GetEncoding(1252), conteudo);
            var stringANSI = Encoding.GetEncoding(1252).GetString(bytesANSI);
            var writer = new StreamWriter(caminhoArquivo, false, Encoding.GetEncoding(1252));
            writer.Write(stringANSI);
            writer.Close();
        }

        private void ConfiguraCodigoInstrucao(int codigoBanco, BoletoNet.Boleto boleto, string instrucao)
        {
            if (instrucao!= null && instrucao != "0" && instrucao != "")
            {
                var instrucaoCodigo = new BoletoNet.Instrucao(Convert.ToInt32(codigoBanco), instrucao.ToString());
                boleto.Instrucoes.Add(instrucaoCodigo);
            }
        }

        private void ConfiguraEspecieDocumento(BoletoNet.Banco banco, BoletoCobranca ordemDeCobranca, BoletoNet.Boleto boleto)
        {
            if (banco.Nome.Equals("Banco do Brasil") && ordemDeCobranca.Boleto.TipoArquivoRemessa.Equals("400"))
                boleto.EspecieDocumento = new BoletoNet.EspecieDocumento_BancoBrasil400("1"); // 1 - codigo para especie documento de duplicata mercantil no Banco do Brasil 400 posições 
            else if (banco.Nome.Equals("Banco do Brasil") && ordemDeCobranca.Boleto.TipoArquivoRemessa.Equals("240"))
                boleto.EspecieDocumento = new BoletoNet.EspecieDocumento_BancoBrasil240("2"); // 2 - codigo para especie documento de duplicata mercantil no Banco do Brasil 240 posições
            else if (banco.Nome.Equals("HSBC") && ordemDeCobranca.Boleto.TipoArquivoRemessa.Equals("400"))
                boleto.EspecieDocumento = new BoletoNet.EspecieDocumento_HSBC400("1"); // 1 - codigo para especie documento de duplicata mercantil no HSBC 400 posições              
            else if (banco.Nome.Equals("HSBC") && ordemDeCobranca.Boleto.TipoArquivoRemessa.Equals("240"))
                boleto.EspecieDocumento = new BoletoNet.EspecieDocumento_HSBC240("2"); // 2 - codigo para especie documento de duplicata mercantil no HSBC 240 posições    
            else if (banco.Nome.Equals("Bradesco") && ordemDeCobranca.Boleto.TipoArquivoRemessa.Equals("400"))
                boleto.EspecieDocumento = new BoletoNet.EspecieDocumento_Bradesco400("1"); // 1 - codigo para especie documento de duplicata mercantil no Bradesco 400 posições
            else if (banco.Nome.Equals("Bradesco") && ordemDeCobranca.Boleto.TipoArquivoRemessa.Equals("240"))
                boleto.EspecieDocumento = new BoletoNet.EspecieDocumento_Bradesco240("2"); // 2 - codigo para especie documento de duplicata mercantil no Bradesco 240 posições
            else if (banco.Nome.Equals("Santander") && ordemDeCobranca.Boleto.TipoArquivoRemessa.Equals("400"))
                boleto.EspecieDocumento = new BoletoNet.EspecieDocumento_Santander400("1"); // 1 - codigo para especie documento de duplicata mercantil no Santander 400 posições
            else if (banco.Nome.Equals("Santander") && ordemDeCobranca.Boleto.TipoArquivoRemessa.Equals("240"))
                boleto.EspecieDocumento = new BoletoNet.EspecieDocumento_Santander240("2"); // 2 - codigo para especie documento de duplicata mercantil no Santander 240 posições
            else if (banco.Nome.Equals("Sicoob") && ordemDeCobranca.Boleto.TipoArquivoRemessa.Equals("400"))
                boleto.EspecieDocumento = new BoletoNet.EspecieDocumento_Sicoob400("1"); // 1 - codigo para especie documento de duplicata mercantil no Sicoob 400 posições
            else if (banco.Nome.Equals("Sicoob") && ordemDeCobranca.Boleto.TipoArquivoRemessa.Equals("240"))
                boleto.EspecieDocumento = new BoletoNet.EspecieDocumento_Sicoob240("2"); // 2 - codigo para especie documento de duplicata mercantil no Sicoob 240 posições
            else if (banco.Nome.Equals("Caixa Econômica Federal") && ordemDeCobranca.Boleto.TipoArquivoRemessa.Equals("400"))
                boleto.EspecieDocumento = new BoletoNet.EspecieDocumento_Caixa400("1"); // 1 - codigo para especie documento de duplicata mercantil na Caixa 400 posições
            else if (banco.Nome.Equals("Caixa Econômica Federal") && ordemDeCobranca.Boleto.TipoArquivoRemessa.Equals("240"))
                boleto.EspecieDocumento = new BoletoNet.EspecieDocumento_Caixa240("2"); // 2 - codigo para especie documento de duplicata mercantil na Caixa 240 posições
            else if (banco.Nome.Equals("Itaú"))
                boleto.EspecieDocumento = new BoletoNet.EspecieDocumento_Itau("1"); // 1 - codigo para especie documento de duplicata mercantil no Itau único para 400 e 240 posições
        }

        public string RemoveCaracteresEspeciais(string str)
        {
            /** Troca os caracteres acentuados por não acentuados **/
            string[] acentos = new string[] { "ç", "Ç", "á", "é", "í", "ó", "ú", "ý", "Á", "É", "Í", "Ó", "Ú", "Ý", "à", "è", "ì", "ò", "ù", "À", "È", "Ì", "Ò", "Ù", "ã", "õ", "ñ", "ä", "ë", "ï", "ö", "ü", "ÿ", "Ä", "Ë", "Ï", "Ö", "Ü", "Ã", "Õ", "Ñ", "â", "ê", "î", "ô", "û", "Â", "Ê", "Î", "Ô", "Û" };
            string[] semAcento = new string[] { "c", "C", "a", "e", "i", "o", "u", "y", "A", "E", "I", "O", "U", "Y", "a", "e", "i", "o", "u", "A", "E", "I", "O", "U", "a", "o", "n", "a", "e", "i", "o", "u", "y", "A", "E", "I", "O", "U", "A", "O", "N", "a", "e", "i", "o", "u", "A", "E", "I", "O", "U" };
            for (int i = 0; i < acentos.Length; i++)
            {
                str = str.Replace(acentos[i], semAcento[i]);
            }
            /** Troca os caracteres especiais da string por "" **/
            string[] caracteresEspeciais = { "\\.", ",", "-", ":", "\\(", "\\)", "ª", "\\|", "\\\\", "°", "–" };
            for (int i = 0; i < caracteresEspeciais.Length; i++)
            {
                str = str.Replace(caracteresEspeciais[i], "");
            }
            /** Troca os espaços no início por "" **/
            str = str.Replace("^\\s+", "");
            /** Troca os espaços no início por "" **/
            str = str.Replace("\\s+$", "");
            /** Troca os espaços duplicados, tabulações e etc por  " " **/
            str = str.Replace("\\s+", " ");
            return str;
        }
    }
}
