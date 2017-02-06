using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using Microsoft.VisualBasic;
using System.Globalization;

[assembly: WebResource("BoletoNet.Imagens.756.jpg", "image/jpg")]
namespace BoletoNet
{
    /// <summary>
    /// Classe referente ao Bancoob Sicoob Crédibom.
    /// Autor: Janiel Madureira Oliveira
    /// E-mail: janielbelmont@msn.com
    /// Twitter: @janiel14
    /// Data: 01/02/2012
    /// Obs: Os arquivo de remessa CNAB 400 foi implementado para cobranças com registros seguindo o padrão CBR641.
    /// </summary>
    internal class Banco_Sicoob : AbstractBanco, IBanco
    {
        #region CONSTRUTOR
        /**
         * <summary>Construtor</summary>
         */
        internal Banco_Sicoob()
        {
            this.Nome = "Sicoob";
            this.Codigo = 756;
            this.Digito = "0";
        }
        #endregion CONSTRUTOR

        #region FORMATAÇÕES
        public override void FormataNossoNumero(Boleto boleto)
        {
            //Variaveis
            int resultado = 0;
            int dv = 0;
            int resto = 0;
            String constante = "319731973197319731973";
            String cooperativa = boleto.Cedente.ContaBancaria.Agencia;
            String codigo = boleto.Cedente.Codigo.ToString() + boleto.Cedente.DigitoCedente.ToString();

            if (boleto.Cedente.DigitoCedente == -1)
                codigo = boleto.Cedente.Codigo.ToString();

            String nossoNumero = boleto.NossoNumero;
            StringBuilder seqValidacao = new StringBuilder();

            /*
             * Preenchendo com zero a esquerda
             */
            //Tratando cooperativa
            for (int i = 0; i < 4 - cooperativa.Length; i++)
            {
                seqValidacao.Append("0");
            }
            seqValidacao.Append(cooperativa);
            //Tratando cliente
            for (int i = 0; i < 10 - codigo.Length; i++)
            {
                seqValidacao.Append("0");
            }
            seqValidacao.Append(codigo);
            //Tratando nosso número
            for (int i = 0; i < 7 - nossoNumero.Length; i++)
            {
                seqValidacao.Append("0");
            }
            seqValidacao.Append(nossoNumero);

            /*
             * Multiplicando cada posição por sua respectiva posição na constante.
             */
            for (int i = 0; i < 21; i++)
            {
                resultado = resultado + (Convert.ToInt16(seqValidacao.ToString().Substring(i, 1)) * Convert.ToInt16(constante.Substring(i, 1)));
            }
            //Calculando mod 11
            resto = resultado % 11;
            //Verifica resto
            if (resto == 1 || resto == 0)
            {
                dv = 0;
            }
            else
            {
                dv = 11 - resto;
            }
            //Montando nosso número
            boleto.NossoNumero = boleto.NossoNumero;
            boleto.DigitoNossoNumero = dv.ToString();
        }

        /**
         * FormataCodigoCliente
         * Inclui 0 a esquerda para preencher o tamanho do campo
         */
        public String FormataCodigoCliente(Boleto boleto)
        {
            //Variaveis
            StringBuilder novoCodigo = new StringBuilder();
            string codigoCliente = boleto.Cedente.Codigo.ToString() + boleto.Cedente.DigitoCedente.ToString();

            if (boleto.Cedente.DigitoCedente == -1)
                codigoCliente = boleto.Cedente.Codigo.ToString();

            //Formatando
            for (int i = 0; i < (7 - codigoCliente.Length); i++)
            {
                novoCodigo.Append("0");
            }
            novoCodigo.Append(codigoCliente);
            return novoCodigo.ToString();
        }

        /**
         * FormataNumeroTitulo
         * Inclui 0 a esquerda para preencher o tamanho do campo
         */
        public String FormataNumeroTitulo(Boleto boleto)
        {
            //Variaveis
            StringBuilder novoTitulo = new StringBuilder();
            //Formatando
            for (int i = 0; i < (7 - boleto.NossoNumero.Length); i++)
            {
                novoTitulo.Append("0");
            }
            novoTitulo.Append(boleto.NossoNumero + boleto.DigitoNossoNumero);
            return novoTitulo.ToString();
        }

        /**
         * FormataNumeroParcela
         * Inclui 0 a esquerda para preencher o tamanho do campo
         */
        public String FormataNumeroParcela(Boleto boleto)
        {
            //Variaveis
            StringBuilder novoNumero = new StringBuilder();
            //Formatando
            for (int i = 0; i < (3 - boleto.NumeroParcela.ToString().Length); i++)
            {
                novoNumero.Append("0");
            }
            novoNumero.Append(boleto.NumeroParcela.ToString());
            return novoNumero.ToString();
        }

        public override void FormataCodigoBarra(Boleto boleto)
        {

            var valorBoleto = "";

            if (boleto.SegundaVia)
                valorBoleto = Utils.FormatCode(boleto.ValorCobrado.ToString("f").Replace(",", "").Replace(".", ""), 10);//10
            else
                valorBoleto = Utils.FormatCode(boleto.ValorBoleto.ToString("f").Replace(",", "").Replace(".", ""), 10);//10

            //Variaveis
            int peso = 2;
            int soma = 0;
            int resultado = 0;
            int dv = 0;
            var moeda = "9";
            String codigoValidacao = boleto.Banco.Codigo.ToString() + moeda + FatorVencimento(boleto).ToString() +
                valorBoleto + boleto.Carteira + boleto.Cedente.ContaBancaria.Agencia + boleto.TipoModalidade + 
                this.FormataCodigoCliente(boleto) + this.FormataNumeroTitulo(boleto) + this.FormataNumeroParcela(boleto);

            //Calculando
            for (int i = (codigoValidacao.Length - 1); i >= 0; i--)
            {
                soma = soma + (Convert.ToInt16(codigoValidacao.Substring(i, 1)) * peso);
                peso++;
                //Verifica peso
                if (peso > 9)
                {
                    peso = 2;
                }
            }
            resultado = soma % 11;
            dv = 11 - resultado;
            //Verificando resultado
            if (dv == 0 || dv == 1 || dv > 9)
            {
                dv = 1;
            }
            //Formatando
            boleto.CodigoBarra.Codigo = codigoValidacao.Substring(0, 4) + dv + codigoValidacao.Substring(4);
        }

        public override void FormataNumeroDocumento(Boleto boleto)
        {
            throw new NotImplementedException("Função ainda não implementada.");
        }

        public override void FormataLinhaDigitavel(Boleto boleto)
        {
            //Variaveis
            String campo1 = string.Empty;
            String campo2 = string.Empty;
            String campo3 = string.Empty;
            String campo4 = string.Empty;
            String campo5 = string.Empty;
            String indice = "1212121212";
            StringBuilder linhaDigitavel = new StringBuilder();
            int soma = 0;
            int temp = 0;

            //Formatando o campo 1
            campo1 = boleto.Banco.Codigo.ToString() + boleto.Moeda.ToString() + boleto.Carteira + boleto.Cedente.ContaBancaria.Agencia;
            //Calculando CAMPO 1
            for (int i = 0; i < campo1.Length; i++)
            {
                //Calculando indice
                temp = (Convert.ToInt16(campo1.Substring(i, 1)) * Convert.ToInt16(indice.Substring(i+1, 1)));
                //Verifica se resultado é igual ou superior a 10
                if (temp >= 10)
                {
                    temp = Convert.ToInt16(temp.ToString().Substring(0, 1)) + Convert.ToInt16(temp.ToString().Substring(1, 1));
                }
                //Guardando soma
                soma = soma + temp;
            }
            linhaDigitavel.Append(string.Format("{0}.{1}{2} ", campo1.Substring(0, 5), campo1.Substring(5, 4), Multiplo10(soma)));
            
            soma = 0;
            temp = 0;
            //Formatando o campo 2
            campo2 = boleto.CodigoBarra.Codigo.Substring(24, 10);
            for (int i = 0; i < campo2.Length; i++)
            {
                //Calculando Indice 2
                temp = (Convert.ToInt16(campo2.Substring(i, 1)) * Convert.ToInt16(indice.Substring(i, 1)));
                //Verifica se resultado é igual ou superior a 10
                if (temp >= 10)
                {
                    temp = Convert.ToInt16(temp.ToString().Substring(0, 1)) + Convert.ToInt16(temp.ToString().Substring(1, 1));
                }
                //Guardando soma
                soma = soma + temp;
            }

            linhaDigitavel.Append(string.Format("{0}.{1}{2} ", campo2.Substring(0, 5), campo2.Substring(5, 5), Multiplo10(soma)));

            soma = 0;
            temp = 0;
            //Formatando campo 3
            campo3 = boleto.CodigoBarra.Codigo.Substring(34, 10);
            for (int i = 0; i < campo3.Length; i++)
            {
                //Calculando indice 2
                temp = (Convert.ToInt16(campo3.Substring(i, 1)) * Convert.ToInt16(indice.Substring(i, 1)));
                //Verifica se resultado é igual ou superior a 10
                if (temp >= 10)
                {
                    temp = Convert.ToInt16(temp.ToString().Substring(0, 1)) + Convert.ToInt16(temp.ToString().Substring(1, 1));
                }
                //Guardando resultado
                soma = soma + temp;
            }
            linhaDigitavel.Append(campo3.Substring(0, 5) + "." + campo3.Substring(5, 5) + Multiplo10(soma) + " ");
            //Formatando Campo 4
            campo4 = boleto.CodigoBarra.Codigo.Substring(4, 1);
            linhaDigitavel.Append(campo4 + " ");
            //Formatando Campo 5
            var valorBoleto = "";

            if (boleto.SegundaVia)
                valorBoleto = Utils.FormatCode(boleto.ValorCobrado.ToString("f").Replace(",", "").Replace(".", ""), 10);//10
            else
                valorBoleto = Utils.FormatCode(boleto.ValorBoleto.ToString("f").Replace(",", "").Replace(".", ""), 10);//10

            campo5 = FatorVencimento(boleto).ToString() + valorBoleto;
            linhaDigitavel.Append(campo5);
            boleto.CodigoBarra.LinhaDigitavel = linhaDigitavel.ToString();
        }
        #endregion FORMATAÇÕES

        #region VALIDAÇÕES
        public override void ValidaBoleto(Boleto boleto)
        {
            //Atribui o nome do banco ao local de pagamento
            boleto.LocalPagamento += Nome + "";
            
            //Verifica se data do processamento é valida
			//if (boleto.DataProcessamento.ToString("dd/MM/yyyy") == "01/01/0001")
			if (boleto.DataProcessamento == DateTime.MinValue)
                boleto.DataProcessamento = DateTime.Now;
            
            //Verifica se data do documento é valida
			//if (boleto.DataDocumento.ToString("dd/MM/yyyy") == "01/01/0001")
			if (boleto.DataDocumento == DateTime.MinValue)
                boleto.DataDocumento = DateTime.Now;

            boleto.QuantidadeMoeda = 0;

            //Atribui o nome do banco ao local de pagamento
            boleto.LocalPagamento = "Pagável em qualquer banco até a data de vencimento!";

            //Aplicando formatações
            this.FormataNossoNumero(boleto);
            this.FormataCodigoBarra(boleto);
            this.FormataLinhaDigitavel(boleto);
        }
        #endregion VALIDAÇÕES

        #region Métodos de geração do arquivo remessa CNAB400

        public override string GerarHeaderRemessa(string numeroConvenio, Cedente cedente, TipoArquivo tipoArquivo, int numeroArquivoRemessa, Boleto boletos)
        {
            throw new NotImplementedException("Função não implementada.");
        }
        
        public override string GerarHeaderRemessa(string numeroConvenio, Cedente cedente, TipoArquivo tipoArquivo, int numeroArquivoRemessa)
        {
            try
            {
                string _header = " ";

                base.GerarHeaderRemessa("0", cedente, tipoArquivo, numeroArquivoRemessa);

                switch (tipoArquivo)
                {
                    case TipoArquivo.CNAB240:
                        _header = GerarHeaderRemessaCNAB240(cedente);
                        break;
                    case TipoArquivo.CNAB400:
                        _header = GerarHeaderRemessaCNAB400(cedente);
                        break;
                    case TipoArquivo.Outro:
                        throw new Exception("Tipo de arquivo inexistente.");
                }

                return _header;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a geração do HEADER do arquivo de REMESSA.", ex);
            }
        }

        private static string GerarHeaderRemessaCNAB400(Cedente cedente)
        {
            //Variaveis
            StringBuilder _header = new StringBuilder();
            //Tratamento de erros
            try
            {
                //Montagem do header
                _header.Append("0"); //Posição 001
                _header.Append("1"); //Posição 002
                _header.Append("REMESSA"); //Posição 003 a 009
                _header.Append("01"); //Posição 010 a 011
                _header.Append("COBRANÇA"); //Posição 012 a 019
                _header.Append(new string(' ', 7)); //Posição 020 a 026
                _header.Append(Utils.FitStringLength(cedente.ContaBancaria.Agencia, 4, 4, '0', 0, true, true, true)); //Posição 027 a 030
                _header.Append(Utils.FitStringLength(cedente.ContaBancaria.DigitoAgencia, 1, 1, '0', 0, true, true, true)); //Posição 031
                _header.Append(Utils.FitStringLength(Convert.ToString(cedente.Codigo), 9, 9, '0', 0, true, true, true)); //Posição 032 a 034
                _header.Append(Utils.FitStringLength(cedente.ContaBancaria.Conta + cedente.ContaBancaria.DigitoConta, 6, 6, '0', 0, true, true, true)); //Posição 041 a 046
                _header.Append(Utils.FitStringLength(cedente.Nome, 30, 30, ' ', 0, true, true, false)); //Posição 047 a 076
                _header.Append(Utils.FitStringLength("756BANCOOBCED", 18, 18, ' ', 0, true, true, false)); //Posição 077 a 094
                _header.Append(DateTime.Now.ToString("ddMMyy")); //Posição 095 a 100
                _header.Append(Utils.FitStringLength(Convert.ToString(cedente.NumeroSequencial), 7, 7, '0', 0, true, true, true)); //Posição 101 a 107
                _header.Append(new string(' ', 287)); //Posição 108 a 394
                _header.Append("000001"); //Posição 395 a 400

                //Retorno
                return _header.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar HEADER do arquivo de remessa do CNAB400.", ex);
            }
        }
        
        public override string GerarDetalheRemessa(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
        {
            //Variaveis
            StringBuilder _detalhe = new StringBuilder();
            //Tratamento de erros
            try
            {
                this.FormataNossoNumero(boleto);
                var dvAgencia = boleto.Cedente.ContaBancaria.DigitoAgencia == null ? "0" : boleto.Cedente.ContaBancaria.DigitoAgencia;

                //Montagem do Detalhe
                _detalhe.Append("1"); //Posição 001
                _detalhe.Append(Utils.IdentificaTipoInscricaoSacado(boleto.Cedente.CPFCNPJ)); //Posição 002 a 003
                _detalhe.Append(Utils.FitStringLength(boleto.Cedente.CPFCNPJ.Replace(".", "").Replace("-", "").Replace("/", ""), 14, 14, '0', 0, true, true, true)); //Posição 004 a 017
                _detalhe.Append(Utils.FitStringLength(boleto.Cedente.ContaBancaria.Agencia, 4, 4, '0', 0, true, true, true)); //Posição 018 a 021
                _detalhe.Append(Utils.FitStringLength(dvAgencia, 1, 1, '0', 0, true, true, true)); //Posição 022
                _detalhe.Append(Utils.FitStringLength(boleto.Cedente.ContaBancaria.Conta, 8, 8, '0', 0, true, true, true)); //Posição 023 a 030
                _detalhe.Append(Utils.FitStringLength(boleto.Cedente.ContaBancaria.DigitoConta, 1, 1, '0', 0, true, true, true)); //Posição 031
                _detalhe.Append(Utils.FitStringLength(boleto.Cedente.Convenio.ToString(), 6, 6, '0', 0, true, true, true)); //Posição 032 a 037
                _detalhe.Append(Utils.FitStringLength("", 25, 25, ' ', 0, true, true, false)); //Posição 038 a 62
                _detalhe.Append(Utils.FitStringLength(boleto.NossoNumero, 11, 11, '0', 0, true, true, true)); //Posição 063 a 073
                _detalhe.Append(Utils.FitStringLength(boleto.DigitoNossoNumero, 1, 1, '0', 0, true, true, true)); //Posição 074
                _detalhe.Append(Utils.FitStringLength("01", 2, 2, '0', 0, true, true, true)); //Posição 075 a 076
                _detalhe.Append("00"); //Posição 077 a 078
                _detalhe.Append("   "); //Posição 079 a 081
                _detalhe.Append(" "); //Posição 082
                _detalhe.Append(Utils.FitStringLength("DM", 3, 3, ' ', 0, true, true, false)); //Posição 083 a 085
                _detalhe.Append("000"); //Posição 086 a 088
                _detalhe.Append("0"); //Posição 089
                _detalhe.Append("00000"); //Posição 090 a 094
                _detalhe.Append("0"); //Posição 095
                _detalhe.Append(Utils.FitStringLength(boleto.Cedente.NumeroBordero.ToString(), 6, 6, '0', 0, true, true, true)); //Posição 096 a 101
                _detalhe.Append(new string(' ', 4)); //Posição 102 a 105
                #region Impressão
                if (boleto.BancoGeraBoleto)
                    _detalhe.Append("1"); //Posição 106 a 106
                else
                    _detalhe.Append("2"); //Posição 106 a 106
                #endregion
                _detalhe.Append(Utils.FitStringLength(boleto.Carteira, 2, 2, '0', 0, true, true, true)); //Posição 107 a 108
                _detalhe.Append(Utils.FormatCode(boleto.CodigoOcorrencia, "0", 2, true)); //Posição 109 a 110
                _detalhe.Append(Utils.FitStringLength(boleto.NumeroDocumento, 10, 10, '0', 0, true, true, true)); //Posição 111 a 120
                _detalhe.Append(boleto.DataVencimento.ToString("ddMMyy")); //Posição 121 a 126
                _detalhe.Append(Utils.FitStringLength(boleto.ValorBoleto.ToString("0.00").Replace(",", ""), 13, 13, '0', 0, true, true, true)); //Posição 127 a 139 
                _detalhe.Append(boleto.Banco.Codigo); //Posição 140 a 142
                _detalhe.Append(Utils.FitStringLength(boleto.Cedente.ContaBancaria.Agencia, 4, 4, '0', 0, true, true, true)); //Posição 143 a 146
                _detalhe.Append(Utils.FitStringLength(boleto.Cedente.ContaBancaria.DigitoAgencia, 1, 1, '0', 0, true, true, true)); //Posição 147
                _detalhe.Append(Utils.FormatCode(boleto.EspecieDocumento.Codigo, "0", 2, true)); //Posição 148 a 149
                _detalhe.Append("0"); //Posição 150
                _detalhe.Append(boleto.DataProcessamento.ToString("ddMMyy")); //Posição 151 a 156
                _detalhe.Append("01"); //Posição 157 a 158
                _detalhe.Append("22"); //Posição 159 a 160
                #region Juros
                if (boleto.PercJurosMora > 0)
                {
                    var strJurosMora = "";
                    var jurosMoraMes = boleto.PercJurosMora * 30;

                    if (jurosMoraMes.ToString().Contains("."))
                    {
                        if (jurosMoraMes < 10)
                            strJurosMora = "0" + jurosMoraMes.ToString().Replace(".", "").Replace(",", "");
                        else
                            strJurosMora = jurosMoraMes.ToString().Replace(".", "").Replace(",", "");
                    }
                    else
                    {
                        if (jurosMoraMes < 10)
                            strJurosMora = "0" + jurosMoraMes.ToString().Replace(".", "").Replace(",", "");
                        else
                            strJurosMora = jurosMoraMes.ToString().Replace(".", "").Replace(",", "");
                    }

                    _detalhe.Append(Utils.FormatCode(strJurosMora, "0", 6)); //Posição 161 a 166
                }
                else
                    _detalhe.Append("000000"); //Posição 161 a 166
                #endregion
                #region Multa
                if (boleto.ValorMulta > 0)
                {
                    var strValorMulta = "";
                    var percValorMulta = Decimal.Round((boleto.ValorMulta / boleto.ValorBoleto) * 100, 2);

                    if (percValorMulta.ToString().Contains("."))
                    {
                        if (percValorMulta < 10)
                            strValorMulta = "0" + percValorMulta.ToString().Replace(".", "").Replace(",", "");
                        else
                            strValorMulta = percValorMulta.ToString().Replace(".", "").Replace(",", "");
                    }
                    else
                    {
                        if (percValorMulta < 10)
                            strValorMulta = "0" + percValorMulta.ToString().Replace(".", "").Replace(",", "");
                        else
                            strValorMulta = percValorMulta.ToString().Replace(".", "").Replace(",", "");
                    }

                    _detalhe.Append(Utils.FormatCode(strValorMulta, "0", 6)); //Posição 167 a 172
                }
                else
                    _detalhe.Append("000000"); //Posição 167 a 172
                #endregion
                #region Distribuição
                if (boleto.BancoGeraBoleto)
                    _detalhe.Append("1"); //Posição 173 a 173
                else
                    _detalhe.Append("2"); //Posição 173 a 173
                #endregion
                #region Desconto
                if (boleto.ValorDesconto > 0)
                {
                    _detalhe.Append(Utils.FitStringLength(boleto.DataVencimento.ToString("ddMMyy"), 6, 6, '0', 0, true, true, true)); //Posição 174 a 179
                    _detalhe.Append(Utils.FitStringLength(boleto.ValorDesconto.ToString("0.00").Replace(".", "").Replace(",", ""), 13, 13, '0', 0, true, true, true)); //Posição 180 a 192
                }
                else
                {
                    _detalhe.Append(Utils.FitStringLength("", 6, 6, '0', 0, true, true, true)); //Posição 174 a 179
                    _detalhe.Append(Utils.FitStringLength("", 13, 13, '0', 0, true, true, true)); //Posição 180 a 192
                }
                #endregion
                _detalhe.Append("9" + Utils.FitStringLength(boleto.IOF.ToString("0.00").Replace(".", "").Replace(",", ""), 12, 12, '0', 0, true, true, true)); //Posição 193 a 205
                _detalhe.Append(Utils.FitStringLength(boleto.Abatimento.ToString("0.00").Replace(".", "").Replace(",", ""), 13, 13, '0', 0, true, true, true)); //Posição 206 a 218
                _detalhe.Append(Utils.IdentificaTipoInscricaoSacado(boleto.Sacado.CPFCNPJ)); //Posição 219 a 220
                _detalhe.Append(Utils.FitStringLength(boleto.Sacado.CPFCNPJ.Replace(".", "").Replace("-", "").Replace("/", ""), 14, 14, '0', 0, true, true, true)); //Posição 221 a 234
                _detalhe.Append(Utils.FitStringLength(boleto.Sacado.Nome, 40, 40, ' ', 0, true, true, false)); //Posição 235 a 274
                _detalhe.Append(Utils.FitStringLength(boleto.Sacado.Endereco.End, 37, 37, ' ', 0, true, true, false)); //Posição 275 a 311
                _detalhe.Append(Utils.FitStringLength(boleto.Sacado.Endereco.Bairro, 15, 15, ' ', 0, true, true, false)); //Posição 312 a 326
                _detalhe.Append(Utils.FitStringLength(boleto.Sacado.Endereco.CEP, 8, 8, '0', 0, true, true, true)); //Posição 327 a 334
                _detalhe.Append(Utils.FitStringLength(boleto.Sacado.Endereco.Cidade, 15, 15, ' ', 0, true, true, false)); //Posição 335 a 349
                _detalhe.Append(boleto.Sacado.Endereco.UF); //Posição 350 a 351
                _detalhe.Append(new string(' ', 40)); //Posição 352 a 391 - OBSERVACOES
                _detalhe.Append("00"); //Posição 392 a 393 - DIAS PARA PROTESTO
                _detalhe.Append(" "); //Posição 394
                _detalhe.Append(Utils.FitStringLength(numeroRegistro.ToString(), 6, 6, '0', 0, true, true, true)); //Posição 394 a 400

                //Retorno
                return Utils.SubstituiCaracteresEspeciais(_detalhe.ToString());
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar DETALHE do arquivo de remessa do CNAB400.", ex);
            }
        }
        
        public override string GerarTrailerRemessa(int numeroRegistro, TipoArquivo tipoArquivo, Cedente cedente, decimal vltitulostotal)
        {
            //Variavies
            StringBuilder _trailer = new StringBuilder();
            //Tratamento
            try
            {
                //Montagem trailer
                _trailer.Append("9"); //Posição 001
                _trailer.Append(new string(' ', 393)); //Posição 002 a 394
                

                _trailer.Append(Utils.FitStringLength(numeroRegistro.ToString(), 6, 6, '0', 0, true, true, true)); //Posição 395 a 400

                //Retorno
                return Utils.SubstituiCaracteresEspeciais(_trailer.ToString());
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar TRAILER do arquivo de remessa do CNAB400.", ex);
            }
        }
        #endregion 

        #region Método de processamento do arquivo retorno CNAB400

        public override HeaderRetorno LerHeaderRetornoCNAB400(string registro)
        {
            try
            {
                HeaderRetorno header = new HeaderRetorno();
                header.RegistroHeader = registro;
                header.Agencia = Convert.ToInt32(registro.Substring(26, 4));
                header.DigitoAgencia = Convert.ToInt32(registro.Substring(30, 1));
                header.ContaCorrente = Convert.ToInt32(registro.Substring(31, 8));
                header.DigitoContaCorrente = Convert.ToInt32(registro.Substring(39, 1));
                header.CodigoCedente = Convert.ToInt32(registro.Substring(40, 6));

                return header;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao ler detalhe do arquivo de RETORNO / CNAB 400.", ex);
            }
        }

        public override DetalheRetorno LerDetalheRetornoCNAB400(string registro)
        {
            try
            {
                DetalheRetorno detalhe = new DetalheRetorno(registro);

                //Tipo de Inscrição Empresa
                detalhe.CodigoInscricao = Utils.ToInt32(registro.Substring(1, 2));
                //Nº Inscrição da Empresa
                detalhe.NumeroInscricao = registro.Substring(3, 14);

                //Identificação da Empresa Cedente no Banco
                detalhe.Agencia = Utils.ToInt32(registro.Substring(17, 4));
                detalhe.Conta = Utils.ToInt32(registro.Substring(22, 8));

                //Nº Controle do Participante
                detalhe.NumeroControle = registro.Substring(37, 25);
                //Identificação do Título no Banco
                detalhe.NossoNumeroComDV = registro.Substring(62, 12);
                detalhe.NossoNumero = registro.Substring(62, 11);
                
                //Identificação de Ocorrência
                detalhe.CodigoOcorrencia = Utils.ToInt32(registro.Substring(108, 2));

                switch (detalhe.CodigoOcorrencia)
                {
                    case (int)ECodigoOcorrenciaSicoob400.Entrada_Confirmada:
                        detalhe.Aceito = true;
                        break;
                    case (int)ECodigoOcorrenciaSicoob400.Liquidação:
                    case (int)ECodigoOcorrenciaSicoob400.Liquidação_Em_Cartório:
                    case (int)ECodigoOcorrenciaSicoob400.Liquidação_Sem_Registro:
                        detalhe.Baixado = true;
                        detalhe.Aceito = false;
                        break;
                    case (int)ECodigoOcorrenciaSicoob400.Baixa:
                        detalhe.Cancelado = true;
                        detalhe.Aceito = false;
                        break;
                    case (int)ECodigoOcorrenciaSicoob400.Rejeição:
                        detalhe.Aceito = false;
                        break;
                }

                //Descrição da ocorrência
                detalhe.DescricaoOcorrencia = this.Ocorrencia(registro.Substring(108, 2));

                //Número do Documento
                detalhe.NumeroDocumento = registro.Substring(62, 7);
                //Identificação do Título no Banco
                detalhe.IdentificacaoTitulo = registro.Substring(62, 7);

                //Valor do Título
                decimal valorTitulo = Convert.ToInt64(registro.Substring(152, 13));
                detalhe.ValorTitulo = valorTitulo / 100;
                //Banco Cobrador
                detalhe.CodigoBanco = Utils.ToInt32(registro.Substring(165, 3));
                //Agência Cobradora
                detalhe.AgenciaCobradora = Utils.ToInt32(registro.Substring(168, 5));
                //Espécie do Título
                detalhe.Especie = Utils.ToInt32(registro.Substring(173, 2));
                // IOF
                decimal iof = Convert.ToUInt64(registro.Substring(214, 13));
                detalhe.IOF = iof / 100;
                //Abatimento Concedido sobre o Título (Valor Abatimento Concedido)
                decimal valorAbatimento = Convert.ToUInt64(registro.Substring(227, 13));
                detalhe.ValorAbatimento = valorAbatimento / 100;
                //Desconto Concedido (Valor Desconto Concedido)
                decimal valorDesconto = Convert.ToUInt64(registro.Substring(240, 13));
                detalhe.Descontos = valorDesconto / 100;
                //Valor Pago
                decimal valorPago = Convert.ToUInt64(registro.Substring(253, 13));
                detalhe.ValorPago = valorPago / 100;
                //Juros Mora
                decimal jurosMora = Convert.ToUInt64(registro.Substring(266, 13));
                detalhe.JurosMora = jurosMora / 100;
                //Outros Créditos
                decimal outrosCreditos = Convert.ToUInt64(registro.Substring(279, 13));
                detalhe.OutrosCreditos = outrosCreditos / 100;
                //Data Ocorrência no Banco
                int dataOcorrencia = Utils.ToInt32(registro.Substring(110, 6));
                detalhe.DataOcorrencia = Utils.ToDateTime(dataOcorrencia.ToString("##-##-##"));
                //Data Vencimento do Título
                int dataVencimento = Utils.ToInt32(registro.Substring(146, 6));
                detalhe.DataVencimento = Utils.ToDateTime(dataVencimento.ToString("##-##-##"));
                // Data do Crédito
                int dataCredito = Utils.ToInt32(registro.Substring(175, 6));
                detalhe.DataCredito = Utils.ToDateTime(dataCredito.ToString("##-##-##"));
                //CPFCNPJ Sacado
                detalhe.CgcCpf = registro.Substring(342, 14);

                detalhe.NumeroSequencial = Utils.ToInt32(registro.Substring(394, 6));

                return detalhe;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao ler detalhe do arquivo de RETORNO / CNAB 400.", ex);
            }
        }

        #endregion

        #region Métodos de geração do arquivo de remessa CNAB240

        #region HEADER

        public override string GerarHeaderLoteRemessa(string numeroConvenio, Cedente cedente, int numeroArquivoRemessa, TipoArquivo tipoArquivo)
        {
            try
            {
                string header = " ";

                switch (tipoArquivo)
                {

                    case TipoArquivo.CNAB240:
                        header = GerarHeaderLoteRemessaCNAB240(cedente, numeroArquivoRemessa);
                        break;
                    case TipoArquivo.Outro:
                        throw new Exception("Tipo de arquivo inexistente.");
                }

                return header;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a geração do HEADER DO LOTE do arquivo de REMESSA.", ex);
            }
        }

        public string GerarHeaderRemessaCNAB240(Cedente cedente)
        {
            try
            {
                var dvAgencia = cedente.ContaBancaria.DigitoAgencia == null ? "0" : cedente.ContaBancaria.DigitoAgencia;

                string header = Utils.FormatCode(Codigo.ToString(), "0", 3, true);                  //Codigo do banco                           001-003
                header += "0000";                                                                   //Lote de serviço                           004-007
                header += "0";                                                                      //Tipo de registro                          008-008
                header += Utils.FormatCode("", " ", 9);                                             //Resevado (uso Banco)                      009-017
                header += (cedente.CPFCNPJ.Length == 11 ? "1" : "2");                               //Tipo inscrição empresa                    018-018
                header += Utils.FormatCode(cedente.CPFCNPJ, "0", 14);                               //Nº inscrição empresa                      019-032
                header += Utils.FormatCode("", " ", 20);                                            //Codigo convênio Sicoob: Brancos           033-052
                header += Utils.FormatCode(cedente.ContaBancaria.Agencia, "0", 5, true);            //Prefixo da Cooperativa                    053-057
                header += Utils.FormatCode(dvAgencia, " ", 1);                                      //Digito da Cooperativa                     058-058
                header += Utils.FormatCode(cedente.ContaBancaria.Conta, "0", 12, true);             //Conta Corrente                            059-070
                header += Utils.FormatCode(cedente.ContaBancaria.DigitoConta, " ", 1);              //Digito da Conta Corrente                  071-071
                header += Utils.FormatCode("", " ", 1);                                             //Resevado (uso Banco)                      072-072
                header += Utils.FitStringLength(cedente.Nome, 30, 30, ' ', 0, true, true, false);   //Nome do cedente                           073-102
                header += Utils.FormatCode("SICOOB", " ", 30);                                      //Nome do Banco                             103-132
                header += Utils.FormatCode("", " ", 10);                                            //Resevado (uso Banco)                      133-142
                header += "1";                                                                      //Código remessa = 1                        143-143
                header += DateTime.Now.ToString("ddMMyyyy");                                        //Data geração do arquivo DDMMAAAA          144-151
                header += DateTime.Now.ToString("hhmmss");                                          //Hora geração do arquivo HHMMSS            152-157
                header += Utils.FormatCode(cedente.NumeroSequencial.ToString(), "0", 6);            //Nº sequencial do arquivo                  158-163
                header += "081";                                                                    //Nº da versão do layout                    164-166
                header += Utils.FormatCode("", "0", 5);                                             //Densidade                                 167-171
                header += Utils.FormatCode("", " ", 69);                                            //Resevado (uso Banco)                      212-240
                return header;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar HEADER do arquivo de remessa do CNAB240.", ex);
            }
        }

        private string GerarHeaderLoteRemessaCNAB240(Cedente cedente, int numeroArquivoRemessa)
        {
            try
            {
                var dvAgencia = cedente.ContaBancaria.DigitoAgencia == null ? "0" : cedente.ContaBancaria.DigitoAgencia;

                string header = Utils.FormatCode(Codigo.ToString(), "0", 3, true);                  //Codigo do Banco                               001-003
                header += "0001";                                                                   //Numero lote Remessa                           004-007
                header += "1";                                                                      //Tipo de registro                              008-008
                header += "R";                                                                      //Tipo de operacao (R - Remessa)                009-009
                header += "01";                                                                     //Tipo de serviço - 01 (Cobrança)               010-011
                header += "  ";                                                                     //Resevado (uso Banco)                          012-013
                header += "040";                                                                    //Nº versão layout do lote                      014-016
                header += " ";                                                                      //Resevado (uso Banco)                          017-017
                header += (cedente.CPFCNPJ.Length == 11 ? "1" : "2");                               //Tipo inscrição empresa 1 = CPF, 2 = CNPJ      018-018
                header += Utils.FormatCode(cedente.CPFCNPJ, "0", 15);                               //Nº inscrição empresa                          019-033
                header += Utils.FormatCode("", " ", 20);                                            //Codigo convênio Sicoob: Brancos               034-052
                header += Utils.FormatCode(cedente.ContaBancaria.Agencia, " ", 5);                  //Prefixo da Cooperativa                        053-057
                header += Utils.FormatCode(dvAgencia, " ", 1);                                      //Digito da Cooperativa                         058-058
                header += Utils.FormatCode(cedente.ContaBancaria.Conta, " ", 12);                   //Conta Corrente                                059-070
                header += Utils.FormatCode(cedente.ContaBancaria.DigitoConta, " ", 1);              //Digito da Conta Corrente                      071-071  
                header += Utils.FormatCode("", " ", 1);                                             //Resevado (uso Banco)                          069-073
                header += Utils.FitStringLength(cedente.Nome, 30, 30, ' ', 0, true, true, false);   //Nome do cedente                               074-103
                header += Utils.FormatCode("", " ", 40);                                            //Mensagem 1                                    104-143
                header += Utils.FormatCode("", " ", 40);                                            //Mensagem 2                                    144-183
                header += Utils.FormatCode(numeroArquivoRemessa.ToString(), "0", 8);                //Número remessa                                184-191
                header += DateTime.Now.ToString("ddMMyyyy");                                        //Data de gravação da remessa  DDMMAAAA         192-199
                header += Utils.FormatCode("", "0", 8);                                             //Data do Crédito : preencher com 00000000      200-207
                header += Utils.FormatCode("", " ", 33);                                            //Resevado (uso Banco)                          208-240

                return header;
            }
            catch (Exception e)
            {
                throw new Exception("Erro ao gerar HEADER DO LOTE do arquivo de remessa.", e);
            }
        }

        #endregion

        #region DETALHE
        public string GerarDetalheRemessaCNAB240(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
        {
            throw new NotImplementedException("Função não implementada.");
        }

        public override string GerarDetalheSegmentoPRemessa(Boleto boleto, int numeroRegistro, string codigoTransmissao)
        {
            try
            {
                var dvAgencia = boleto.Cedente.ContaBancaria.DigitoAgencia == null ? "0" : boleto.Cedente.ContaBancaria.DigitoAgencia;
                string _segmentoP;

                _segmentoP = Utils.FormatCode(Codigo.ToString(), "0", 3, true);
                _segmentoP += "0001";
                _segmentoP += "3";
                _segmentoP += Utils.FitStringLength(numeroRegistro.ToString(), 5, 5, '0', 0, true, true, true);
                _segmentoP += "P ";
                _segmentoP += Utils.FitStringLength(boleto.CodigoOcorrencia, 2, 2, '0', 0, true, true, true);
                _segmentoP += Utils.FitStringLength(boleto.Cedente.ContaBancaria.Agencia, 5, 5, '0', 0, true, true, true);
                _segmentoP += Utils.FitStringLength(dvAgencia, 1, 1, '0', 0, true, true, true);
                _segmentoP += Utils.FitStringLength(boleto.Cedente.ContaBancaria.Conta, 12, 12, '0', 0, true, true, true);
                _segmentoP += Utils.FitStringLength(boleto.Cedente.ContaBancaria.DigitoConta, 1, 1, '0', 0, true, true, true);
                _segmentoP += Utils.FitStringLength("", 1, 1, ' ', 0, true, true, false);
                _segmentoP += Utils.FitStringLength(boleto.NossoNumero, 10, 10, '0', 0, true, true, true);
                _segmentoP += "01"; //Parcela Unica
                _segmentoP += "01"; //Modalidade 01
                _segmentoP += "1"; //Tipo Formulário
                _segmentoP += Utils.FormatCode("", " ", 5);
                _segmentoP += Utils.FitStringLength(boleto.Carteira, 1, 1, '0', 0, true, true, true); ;
                _segmentoP += "0"; //Cobrança registrada
                _segmentoP += " "; //Tipo de documento
                #region Emissão e Distribuição do Boleto
                var emissaoBoleto = "2"; // padrão Cliente Emite
                var distribuicaoBoleto = "2"; // padrão Cliente Distribui

                if (boleto.BancoGeraBoleto)
                {
                    emissaoBoleto = "1";
                    distribuicaoBoleto = "1";
                }
                #endregion
                _segmentoP += emissaoBoleto;
                _segmentoP += distribuicaoBoleto;
                _segmentoP += Utils.FitStringLength(boleto.NumeroDocumento, 15, 15, ' ', 0, true, true, false);
                _segmentoP += Utils.FitStringLength(boleto.DataVencimento.ToString("ddMMyyyy"), 8, 8, ' ', 0, true, true, false);
                _segmentoP += Utils.FitStringLength(boleto.ValorBoleto.ToString("0.00").Replace(",", ""), 15, 15, '0', 0, true, true, true);
                _segmentoP += "00000 ";
                _segmentoP += Utils.FitStringLength(boleto.EspecieDocumento.Codigo.ToString(), 2, 2, '0', 0, true, true, true);
                _segmentoP += "N";
                _segmentoP += Utils.FitStringLength(boleto.DataProcessamento.ToString("ddMMyyyy"), 8, 8, ' ', 0, true, true, false);
                #region Juros e Desconto
                if (boleto.PercJurosMora > 0)
                {
                    _segmentoP += "1";
                    _segmentoP += Utils.FitStringLength(boleto.DataVencimento.ToString("ddMMyyyy"), 8, 8, '0', 0, true, true, false);
                    _segmentoP += Utils.FitStringLength(boleto.PercJurosMora.ToString("0.00").Replace(",", ""), 15, 15, '0', 0, true, true, true);
                }
                else if (boleto.JurosPermanente)
                {
                    _segmentoP += "1";
                    _segmentoP += Utils.FormatCode("", "0", 8);
                    _segmentoP += Utils.FormatCode("", "0", 15);
                }
                else
                {
                    _segmentoP += "3";
                    _segmentoP += Utils.FormatCode("", "0", 8);
                    _segmentoP += Utils.FormatCode("", "0", 15);
                }

                if (boleto.ValorDesconto > 0)
                {
                    _segmentoP += "1";
                    _segmentoP += Utils.FitStringLength(boleto.DataVencimento.ToString("ddMMyyyy"), 8, 8, '0', 0, true, true, false);
                    _segmentoP += Utils.FitStringLength(boleto.ValorDesconto.ToString("0.00").Replace(",", ""), 15, 15, '0', 0, true, true, true);
                }
                else
                {
                    _segmentoP += "0";
                    _segmentoP += Utils.FormatCode("", "0", 8);
                    _segmentoP += Utils.FormatCode("", "0", 15);
                }
                #endregion
                _segmentoP += Utils.FormatCode(boleto.IOF.ToString().Replace(",", "").Replace(".", ""), "0", 15); // Valor do IOF a ser Recolhido 
                _segmentoP += Utils.FormatCode(boleto.Abatimento.ToString().Replace(",", "").Replace(".", ""), "0", 15); // Valor do Abatimento 
                _segmentoP += Utils.FitStringLength(boleto.NumeroDocumento, 25, 25, ' ', 0, true, true, false);

                string codigo_protesto = "3";
                string dias_protesto = "00";

                foreach (Instrucao instrucao in boleto.Instrucoes)
                {
                    switch ((EnumInstrucoes_Sicoob)instrucao.Codigo)
                    {
                        case EnumInstrucoes_Sicoob.PROTESTAR_15_DIAS_UTEIS_APOS_VENCIMENTO:
                            codigo_protesto = "1";
                            dias_protesto = Utils.FitStringLength(instrucao.QuantidadeDias.ToString(), 2, 2, '0', 0, true, true, true);
                            break;
                        case EnumInstrucoes_Sicoob.NAO_PROTESTAR:
                            codigo_protesto = "3";
                            dias_protesto = "00";
                            break;
                        default:
                            break;
                    }
                }

                _segmentoP += codigo_protesto;
                _segmentoP += dias_protesto;

                _segmentoP += "0000090000000000 ";

                _segmentoP = Utils.SubstituiCaracteresEspeciais(_segmentoP);

                return _segmentoP;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a geração do SEGMENTO P DO DETALHE do arquivo de REMESSA.", ex);
            }
        }

        public override string GerarDetalheSegmentoQRemessa(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
        {
            try
            {
                string _segmentoQ;

                _segmentoQ = Utils.FormatCode(Codigo.ToString(), "0", 3, true);
                _segmentoQ += "00013";
                _segmentoQ += Utils.FitStringLength(numeroRegistro.ToString(), 5, 5, '0', 0, true, true, true);
                _segmentoQ += "Q ";
                _segmentoQ += Utils.FitStringLength(boleto.CodigoOcorrencia, 2, 2, '0', 0, true, true, true);

                if (boleto.Sacado.CPFCNPJ.Length <= 11)
                    _segmentoQ += "1";
                else
                    _segmentoQ += "2";

                _segmentoQ += Utils.FitStringLength(boleto.Sacado.CPFCNPJ, 15, 15, '0', 0, true, true, true);
                _segmentoQ += Utils.FitStringLength(boleto.Sacado.Nome.TrimStart(' '), 40, 40, ' ', 0, true, true, false).ToUpper();
                _segmentoQ += Utils.FitStringLength(boleto.Sacado.Endereco.End.TrimStart(' '), 40, 40, ' ', 0, true, true, false).ToUpper();
                _segmentoQ += Utils.FitStringLength(boleto.Sacado.Endereco.Bairro.TrimStart(' '), 15, 15, ' ', 0, true, true, false).ToUpper();
                _segmentoQ += Utils.FitStringLength(boleto.Sacado.Endereco.CEP, 8, 8, ' ', 0, true, true, false).ToUpper(); ;
                _segmentoQ += Utils.FitStringLength(boleto.Sacado.Endereco.Cidade.TrimStart(' '), 15, 15, ' ', 0, true, true, false).ToUpper();
                _segmentoQ += Utils.FitStringLength(boleto.Sacado.Endereco.UF, 2, 2, ' ', 0, true, true, false).ToUpper();
                _segmentoQ += Utils.FitStringLength("", 16, 16, '0', 0, true, true, true);
                _segmentoQ += Utils.FitStringLength("", 40, 40, ' ', 0, true, true, true);
                _segmentoQ += "000";
                _segmentoQ += Utils.FitStringLength("", 28, 28, ' ', 0, true, true, true);

                _segmentoQ = Utils.SubstituiCaracteresEspeciais(_segmentoQ);

                return _segmentoQ;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a geração do SEGMENTO Q DO DETALHE do arquivo de REMESSA.", ex);
            }
        }

        public override string GerarDetalheSegmentoRRemessa(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
        {
            try
            {
                string _brancos110 = new string(' ', 110);
                string _zeros41 = new string('0', 41);

                string _segmentoR;

                _segmentoR = Utils.FormatCode(Codigo.ToString(), "0", 3, true);
                _segmentoR += "00013";
                _segmentoR += Utils.FitStringLength(numeroRegistro.ToString(), 5, 5, '0', 0, true, true, true);
                _segmentoR += "R ";
                _segmentoR += Utils.FitStringLength(boleto.CodigoOcorrencia, 2, 2, '0', 0, true, true, true);
                // Desconto 2
                _segmentoR += "000000000000000000000000"; //24 zeros
                // Desconto 3
                _segmentoR += "000000000000000000000000"; //24 zeros
                #region Multa
                if (boleto.ValorMulta > 0)
                {
                    _segmentoR += "1";                                                                      // Código da Multa '1' = Valor Fixo,'2' = Percentual,'0' = Sem Multa 
                    _segmentoR += boleto.DataVencimento.ToString("ddMMyyyy");                               // Data da Multa 
                    _segmentoR += Utils.FormatCode(boleto.ValorMulta.ToString().Replace(",", "").Replace(".", ""), "0", 15); // Valor/Percentual a Ser Aplicado
                }
                else if (boleto.PercMulta > 0)
                {
                    _segmentoR += "2";                                                                      // Código da Multa '1' = Valor Fixo,'2' = Percentual,'0' = Sem Multa 
                    _segmentoR += boleto.DataVencimento.ToString("ddMMyyyy");                               // Data da Multa 
                    _segmentoR += Utils.FormatCode(boleto.PercMulta.ToString().Replace(",", "").Replace(".", ""), "0", 15); // Valor/Percentual a Ser Aplicado
                }
                else
                {
                    _segmentoR += "0";                                                                      // Código da Multa '1' = Valor Fixo,'2' = Percentual,'0' = Sem Multa 
                    _segmentoR += Utils.FormatCode("", "0", 8);                                             // Data da Multa 
                    _segmentoR += Utils.FormatCode("", "0", 15);                                            // Valor/Percentual a Ser Aplicado
                }
                #endregion
                _segmentoR += _brancos110;
                _segmentoR += _zeros41;

                _segmentoR = Utils.SubstituiCaracteresEspeciais(_segmentoR);

                return _segmentoR;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a geração do SEGMENTO R DO DETALHE do arquivo de REMESSA.", ex);
            }
        }

        public override string GerarDetalheSegmentoSRemessa(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
        {
            try
            {
                string _segmentoS;

                _segmentoS = Utils.FormatCode(Codigo.ToString(), "0", 3, true);
                _segmentoS += "00013";
                _segmentoS += Utils.FormatCode(boleto.Cedente.NumeroSequencial.ToString(), "0", 5);
                _segmentoS += "S ";
                _segmentoS += Utils.FitStringLength(boleto.CodigoOcorrencia, 2, 2, '0', 0, true, true, true);
                _segmentoS += "3";

                if (boleto.Instrucoes.Count > 0)
                {
                    var instrucoes = boleto.Instrucoes[0].Descricao.Replace("<br/>", " ");
                    _segmentoS += Utils.FitStringLength(instrucoes, 200, 200, ' ', 0, true, true, false);
                }
                else
                    _segmentoS += Utils.FitStringLength("", 200, 200, ' ', 0, true, true, false);

                _segmentoS += Utils.FitStringLength("", 22, 22, ' ', 0, true, true, false);

                _segmentoS = Utils.SubstituiCaracteresEspeciais(_segmentoS);

                return _segmentoS;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a geração do SEGMENTO S DO DETALHE do arquivo de REMESSA.", ex);
            }
        }
        #endregion

        #region TRAILER
        public string GerarTrailerRemessa240()
        {
            throw new NotImplementedException("Função não implementada.");
        }

        public override string GerarTrailerLoteRemessa(int numeroRegistro)
        {
            try
            {
                string trailer = Utils.FormatCode(Codigo.ToString(), "0", 3, true);
                trailer += Utils.FormatCode("", "0", 4, true);
                trailer += "5";
                trailer += Utils.FormatCode("", " ", 9);
                trailer += Utils.FitStringLength(numeroRegistro.ToString(), 6, 6, '0', 0, true, true, true);
                trailer += Utils.FormatCode("", "0", 84);
                trailer += Utils.FormatCode("", " ", 133);
                trailer = Utils.SubstituiCaracteresEspeciais(trailer);

                return trailer;
            }
            catch (Exception e)
            {
                throw new Exception("Erro durante a geração do registro TRAILER do LOTE de REMESSA.", e);
            }
        }

        public override string GerarTrailerArquivoRemessa(int numeroRegistro)
        {
            try
            {
                string trailer = Utils.FormatCode(Codigo.ToString(), "0", 3, true);
                trailer += "9999";
                trailer += "9";
                trailer += Utils.FormatCode("", " ", 9);
                trailer += Utils.FormatCode("", "0", 6, true);
                trailer += Utils.FitStringLength(numeroRegistro.ToString(), 6, 6, '0', 0, true, true, true);
                trailer += Utils.FormatCode("", "0", 6, true);
                trailer += Utils.FormatCode("", " ", 205);
                trailer = Utils.SubstituiCaracteresEspeciais(trailer);

                return trailer;
            }
            catch (Exception e)
            {
                throw new Exception("Erro durante a geração do registro TRAILER do ARQUIVO de REMESSA.", e);
            }
        }

        #endregion

        #endregion

        #region Método de processamento do arquivo retorno CNAB240

        public override HeaderRetorno LerHeaderRetornoCNAB240(string registro)
        {
            try
            {
                HeaderRetorno header = new HeaderRetorno();
                header.RegistroHeader = registro;
                header.Agencia = Convert.ToInt32(registro.Substring(52, 5));
                header.DigitoAgencia = Convert.ToInt32(registro.Substring(57, 1));
                header.ContaCorrente = Convert.ToInt32(registro.Substring(58, 12));
                header.DigitoContaCorrente = Convert.ToInt32(registro.Substring(70, 1));
                header.CodigoCedente = Convert.ToInt32(registro.Substring(32, 20));

                return header;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao ler header do arquivo de RETORNO / CNAB 240.", ex);
            }
        }

        public override DetalheSegmentoTRetornoCNAB240 LerDetalheSegmentoTRetornoCNAB240(string registro)
        {
            try
            {
                var segmentoT = new DetalheSegmentoTRetornoCNAB240(registro);

                if (registro.Substring(13, 1) != "T")
                    throw new Exception("Registro inválido. O detalhe não possuí as características do segmento T.");

                segmentoT.CodigoBanco = Convert.ToInt32(registro.Substring(0, 3));
                segmentoT.idCodigoMovimento = Convert.ToInt32(registro.Substring(15, 2));
                segmentoT.Agencia = Convert.ToInt32(registro.Substring(17, 5));
                segmentoT.DigitoAgencia = registro.Substring(22, 1);
                segmentoT.Conta = Convert.ToInt32(registro.Substring(23, 9));
                segmentoT.DigitoConta = registro.Substring(35, 1);

                segmentoT.NossoNumero = registro.Substring(37, 10);
                segmentoT.CodigoCarteira = Convert.ToInt32(registro.Substring(57, 1));
                segmentoT.NumeroDocumento = registro.Substring(58, 15);
                int dataVencimento = Convert.ToInt32(registro.Substring(73, 8));
                segmentoT.DataVencimento = Convert.ToDateTime(dataVencimento.ToString("##-##-####"));
                decimal valorTitulo = Convert.ToDecimal(registro.Substring(81, 15));
                segmentoT.ValorTitulo = valorTitulo / 100;
                segmentoT.IdentificacaoTituloEmpresa = registro.Substring(105, 25);
                segmentoT.TipoInscricao = Convert.ToInt32(registro.Substring(132, 1));
                segmentoT.NumeroInscricao = registro.Substring(133, 15);
                segmentoT.NomeSacado = registro.Substring(148, 40);
                segmentoT.ValorTarifas = Convert.ToDecimal(registro.Substring(198, 15)) / 100;
                segmentoT.IdRejeicao1 = registro.Substring(213, 2);
                segmentoT.IdRejeicao2 = registro.Substring(215, 2);
                segmentoT.IdRejeicao3 = registro.Substring(217, 2);
                segmentoT.IdRejeicao4 = registro.Substring(219, 2);
                segmentoT.IdRejeicao5 = registro.Substring(221, 2);
                segmentoT.UsoFebraban = registro.Substring(223, 22);
                segmentoT.CodigoMovimento = new CodigoMovimento(segmentoT.CodigoBanco, segmentoT.idCodigoMovimento);

                if (segmentoT.IdRejeicao1.Contains("A"))
                {
                    segmentoT.CodigoRejeicao1 = new CodigoRejeicao(segmentoT.CodigoBanco, 0);
                    segmentoT.CodigoRejeicao1.Descricao = "Sacado DDA";
                }
                else
                {
                    segmentoT.CodigoRejeicao1 = new CodigoRejeicao(segmentoT.CodigoBanco, Convert.ToInt32(segmentoT.IdRejeicao1));
                }

                if (segmentoT.IdRejeicao2.Contains("A"))
                {
                    segmentoT.CodigoRejeicao2 = new CodigoRejeicao(segmentoT.CodigoBanco, 0);
                    segmentoT.CodigoRejeicao2.Descricao = "Sacado DDA";
                }
                else
                {
                    segmentoT.CodigoRejeicao2 = new CodigoRejeicao(segmentoT.CodigoBanco, Convert.ToInt32(segmentoT.IdRejeicao2));
                }

                if (segmentoT.IdRejeicao3.Contains("A"))
                {
                    segmentoT.CodigoRejeicao3 = new CodigoRejeicao(segmentoT.CodigoBanco, 0);
                    segmentoT.CodigoRejeicao3.Descricao = "Sacado DDA";
                }
                else
                {
                    segmentoT.CodigoRejeicao3 = new CodigoRejeicao(segmentoT.CodigoBanco, Convert.ToInt32(segmentoT.IdRejeicao3));
                }

                if (segmentoT.IdRejeicao4.Contains("A"))
                {
                    segmentoT.CodigoRejeicao4 = new CodigoRejeicao(segmentoT.CodigoBanco, 0);
                    segmentoT.CodigoRejeicao4.Descricao = "Sacado DDA";
                }
                else
                {
                    segmentoT.CodigoRejeicao4 = new CodigoRejeicao(segmentoT.CodigoBanco, Convert.ToInt32(segmentoT.IdRejeicao4));
                }

                if (segmentoT.IdRejeicao5.Contains("A"))
                {
                    segmentoT.CodigoRejeicao5 = new CodigoRejeicao(segmentoT.CodigoBanco, 0);
                    segmentoT.CodigoRejeicao5.Descricao = "Sacado DDA";
                }
                else
                {
                    segmentoT.CodigoRejeicao5 = new CodigoRejeicao(segmentoT.CodigoBanco, Convert.ToInt32(segmentoT.IdRejeicao5));
                }

                switch (Convert.ToInt32(segmentoT.idCodigoMovimento))
                {
                    case (int)EnumCodigoMovimento_Santander.Entrada_confirmada:
                        segmentoT.Aceito = true;
                        break;
                    case (int)EnumCodigoMovimento_Santander.Liquidação:
                    case (int)EnumCodigoMovimento_Santander.Liquidação_após_baixa_ou_liquidação_título_não_registrado:
                        segmentoT.Baixado = true;
                        segmentoT.Aceito = false;
                        break;
                    case (int)EnumCodigoMovimento_Santander.Baixa:
                        segmentoT.Cancelado = true;
                        segmentoT.Aceito = false;
                        break;
                    case (int)EnumCodigoMovimento_Santander.Entrada_rejeitada:
                        segmentoT.Aceito = false;
                        break;
                }

                return segmentoT;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao processar arquivo de RETORNO - SEGMENTO T.", ex);
            }


        }

        public override DetalheSegmentoURetornoCNAB240 LerDetalheSegmentoURetornoCNAB240(string registro)
        {
            try
            {

                if (!registro.Substring(13, 1).Equals(@"U"))
                {
                    throw new Exception("Registro inválida. O detalhe não possuí as características do segmento U.");
                }

                var segmentoU = new DetalheSegmentoURetornoCNAB240(registro);
                segmentoU.Servico_Codigo_Movimento_Retorno = Convert.ToDecimal(registro.Substring(15, 2)); //07.3U|Serviço|Cód. Mov.|Código de Movimento Retorno
                segmentoU.JurosMultaEncargos = Convert.ToDecimal(registro.Substring(17, 15)) / 100;
                segmentoU.ValorDescontoConcedido = Convert.ToDecimal(registro.Substring(32, 15)) / 100;
                segmentoU.ValorAbatimentoConcedido = Convert.ToDecimal(registro.Substring(47, 15)) / 100;
                segmentoU.ValorIOFRecolhido = Convert.ToDecimal(registro.Substring(62, 15)) / 100;
                segmentoU.ValorOcorrenciaSacado = segmentoU.ValorPagoPeloSacado = Convert.ToDecimal(registro.Substring(77, 15)) / 100;
                segmentoU.ValorLiquidoASerCreditado = Convert.ToDecimal(registro.Substring(92, 15)) / 100;
                segmentoU.ValorOutrasDespesas = Convert.ToDecimal(registro.Substring(107, 15)) / 100;
                segmentoU.ValorOutrosCreditos = Convert.ToDecimal(registro.Substring(122, 15)) / 100;
                segmentoU.DataOcorrencia = segmentoU.DataOcorrencia = DateTime.ParseExact(registro.Substring(137, 8), "ddMMyyyy", CultureInfo.InvariantCulture);
                segmentoU.DataCredito = registro.Substring(145, 8).ToString() == "00000000" ? segmentoU.DataOcorrencia : DateTime.ParseExact(registro.Substring(145, 8), "ddMMyyyy", CultureInfo.InvariantCulture);

                return segmentoU;
            }
            catch (Exception ex)
            {
                //TrataErros.Tratar(ex);
                throw new Exception("Erro ao processar arquivo de RETORNO - SEGMENTO U.", ex);
            }
        }

        #endregion

        public override bool ValidarRemessa(TipoArquivo tipoArquivo, string numeroConvenio, IBanco banco, Cedente cedente, Boletos boletos, int numeroArquivoRemessa, out string mensagem)
        {
            bool vRetorno = true;
            string vMsg = string.Empty;
            ////IMPLEMENTACAO PENDENTE...
            mensagem = vMsg;
            return vRetorno;
        }

        public string Ocorrencia(string codigo)
        {
            switch (codigo)
            {
                case "01":
                    return "01-Título não existe";
                case "02":
                    return "02-Entrada Confirmada";
                case "03":
                    return "03-Entrada Rejeitada";
                case "06":
                    return "06-Liquidação";
                case "09":
                    return "09-Baixa Automatica";
                case "10":
                    return "10-Baixa conf. instrução ou protesto";
                case "11":
                    return "11-Em Ser";
                case "14":
                    return "14-Alteração de Vencimento";
                case "15":
                    return "15-Liquidação em Cartório";
                case "23":
                    return "22-Encaminhado para Protesto";
                case "27":
                    return "24-Confirmação de Alteração de Dados";
                case "48":
                    return "25-Confirmação de instrução de transferência de carteira/modalidade de cobrança";
                default:
                    return "";
            }
        }
    }
}