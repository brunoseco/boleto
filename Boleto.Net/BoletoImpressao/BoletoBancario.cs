using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Web.UI;
using Microsoft.VisualBasic;
using System.IO;
using System.Net.Mail;
using System.Net.Mime;
using System.Reflection;

[assembly: WebResource("BoletoNet.BoletoImpressao.BoletoNet.css", "text/css", PerformSubstitution = true)]
[assembly: WebResource("BoletoNet.Imagens.barra.gif", "image/gif")]

namespace BoletoNet
{
    [Serializable(),
    Designer(typeof(BoletoBancarioDesigner)),
    ToolboxBitmap(typeof(BoletoBancario)),
    ToolboxData("<{0}:BoletoBancario Runat=\"server\"></{0}:BoletoBancario>")]
    public class BoletoBancario : System.Web.UI.Control
    {
        String vLocalLogoCedente = String.Empty;

        #region Propriedade para Exibição

        public string ExibeDataVencimento
        {
            get
            {
                if (this.Boleto == null)
                    return "";

                var dataVencimento = Boleto.DataVencimento.ToString("dd/MM/yyyy");

                if (this.MostrarContraApresentacaoNaDataVencimento)
                    dataVencimento = "Contra Apresentação";

                return dataVencimento;
            }


        }
        public string ExibeEnderecoCedente
        {
            get
            {
                var enderecoCedente = "";

                if (this.Cedente == null)
                    return enderecoCedente;

                if (this.Cedente.Endereco == null)
                    return enderecoCedente;

                if (this.Boleto.Banco.Codigo == 104)
                    return this.Boleto.Cedente.Endereco.End + " - " + this.Boleto.Cedente.Endereco.UF + " - " + this.Boleto.Cedente.Endereco.CEP;

                if (this.Boleto.Banco.Codigo.Equals(001))
                    return this.Boleto.Cedente.Endereco.End + " - " + this.Boleto.Cedente.Endereco.UF + " - " + this.Boleto.Cedente.Endereco.CEP;

                if (this.MostrarEnderecoCedente)
                {
                    string numero = !String.IsNullOrEmpty(this.Cedente.Endereco.Numero) ? this.Cedente.Endereco.Numero + ", " : "";
                    enderecoCedente = string.Concat(this.Cedente.Endereco.End, " , ", numero);

                    if (Cedente.Endereco.CEP == String.Empty)
                        enderecoCedente += string.Format("{0} - {1}/{2}", this.Cedente.Endereco.Bairro, this.Cedente.Endereco.Cidade, this.Cedente.Endereco.UF);
                    else
                        enderecoCedente += string.Format("{0} - {1}/{2} - CEP: {3}", this.Cedente.Endereco.Bairro, this.Cedente.Endereco.Cidade, Cedente.Endereco.UF, Utils.FormataCEP(this.Cedente.Endereco.CEP));

                }

                return enderecoCedente;
            }


        }

        public string ExibeNomeCedente
        {
            get
            {
                var nomeCedente = "";

                if (this.Cedente == null)
                    return nomeCedente;

                if (this.Boleto == null)
                    return nomeCedente;

                if (this.Boleto.Banco == null)
                    return nomeCedente;

                nomeCedente = this.Cedente.Nome;

                if (this.Boleto.Banco.Codigo == 399)
                    nomeCedente += " - CNPJ: " + this.Cedente.CPFCNPJ;

                if (this.Boleto.Banco.Codigo == 001 && !this.FormatoCarne)
                    nomeCedente += "<br />" + this.ExibeEnderecoCedente;

                nomeCedente = nomeCedente.Replace("–", "-");

                return nomeCedente;
            }
        }

        public string ExibeDescricaoCarteira
        {
            get
            {
                if (this.MostrarCodigoCarteira)
                {
                    string descricaoCarteira = "";
                    int carteira = Utils.ToInt32(this.Boleto.Carteira);

                    switch (Banco.Codigo)
                    {
                        case 1:
                            descricaoCarteira = new Carteira_BancoBrasil(carteira).Codigo;
                            break;
                        case 353:
                        case 8:
                        case 33:
                            descricaoCarteira = new Carteira_Santander(carteira).Codigo;
                            break;
                        case 104:
                            descricaoCarteira = new Carteira_Caixa(carteira).Codigo;
                            break;
                        case 341:
                            descricaoCarteira = new Carteira_Itau(carteira).Codigo;
                            break;

                        default:
                            throw new Exception(string.Format("A descrição para o banco {0} não foi implementada.", this.Boleto.Banco));
                            throw new Exception(string.Format("A descrição da carteira {0} / banco {1} não foi implementada (marque false na propriedade MostrarCodigoCarteira)", carteira, this.Banco.Codigo));

                    }

                    if (string.IsNullOrEmpty(descricaoCarteira))
                    {
                        throw new Exception("O código da carteira não foi implementado.");
                    }

                    return string.Format("{0} - {1}", this.Boleto.Carteira, descricaoCarteira);
                }
                else
                    return this.Boleto.Carteira;

            }

        }
        public string ExibeSacado
        {
            get
            {
                string sacado = "";

                if (this.Sacado == null)
                    return sacado;

                if (this.Sacado.CPFCNPJ == string.Empty)
                    sacado = Sacado.Nome;

                else
                {
                    if (this.Sacado.CPFCNPJ.Length <= 11)
                        sacado = string.Format("{0}  CPF: {1}", this.Sacado.Nome, Utils.FormataCPF(this.Sacado.CPFCNPJ));
                    else
                        sacado = string.Format("{0}  CNPJ: {1}", this.Sacado.Nome, Utils.FormataCNPJ(this.Sacado.CPFCNPJ));
                }

                return sacado;
            }

        }
        public string ExibeInfoSacado
        {
            get
            {
                if (this.Sacado == null)
                    return "";

                if (this.Sacado.InformacoesSacado == null)
                    return "";

                if (this.Sacado.Endereco == null)
                    return "";

                string infoSacado = this.Sacado.InformacoesSacado.GeraHTML(false);

                if (!this.OcultarEnderecoSacado)
                {
                    string enderecoSacado = "";

                    if (Sacado.Endereco.CEP == String.Empty)
                        enderecoSacado = string.Format("{0} - {1}/{2}", Sacado.Endereco.Bairro, Sacado.Endereco.Cidade, Sacado.Endereco.UF);
                    else if (Sacado.Endereco.CEP != null)
                        enderecoSacado = string.Format("{0} - {1}/{2} - CEP: {3}", Sacado.Endereco.Bairro,
                        Sacado.Endereco.Cidade, Sacado.Endereco.UF, Utils.FormataCEP(Sacado.Endereco.CEP));
                    else
                        enderecoSacado = "";

                    if (Sacado.Endereco.End != string.Empty)
                        if (infoSacado == string.Empty)
                            infoSacado += InfoSacado.Render(Sacado.Endereco.End, enderecoSacado, false);
                        else
                            infoSacado += InfoSacado.Render(Sacado.Endereco.End, enderecoSacado, true);
                }


                return infoSacado;
            }

        }
        public string ExibeAgenciaCodigoCedente
        {
            get
            {
                if (this.Cedente == null)
                    return "";

                if (this.Cedente.ContaBancaria == null)
                    return "";

                string agenciaConta = Utils.FormataAgenciaConta(this.Cedente.ContaBancaria.Agencia, this.Cedente.ContaBancaria.DigitoAgencia, this.Cedente.ContaBancaria.Conta, this.Cedente.ContaBancaria.DigitoConta);

                string agenciaCodigoCedente;

                if (!this.Cedente.DigitoCedente.Equals(-1))
                {
                    if (!String.IsNullOrEmpty(this.Cedente.ContaBancaria.OperacaoConta))
                        agenciaCodigoCedente = string.Format("{0}/{1}.{2}-{3}", this.Cedente.ContaBancaria.Agencia, this.Cedente.ContaBancaria.OperacaoConta, Utils.FormatCode(this.Cedente.Codigo.ToString(), 6), this.Cedente.DigitoCedente.ToString());

                    switch (Boleto.Banco.Codigo)
                    {
                        case 756:
                            agenciaCodigoCedente = string.Format("{0}/{1}", Utils.FormatCode(this.Cedente.ContaBancaria.Agencia, 4), this.Cedente.Codigo);
                            break;
                        case 748:
                            agenciaCodigoCedente = string.Format("{0}.{1}.{2}", Utils.FormatCode(this.Cedente.ContaBancaria.Agencia, 4), this.Cedente.ContaBancaria.OperacaoConta, this.Cedente.Codigo);
                            break;
                        case 104:
                            {
                                if (this.Cedente.Codigo.Length.Equals(6))
                                {
                                    agenciaCodigoCedente = string.Format("{0}/{1}-{2}", Utils.FormatCode(this.Cedente.ContaBancaria.Agencia, 4), this.Cedente.Codigo, this.Cedente.DigitoCedente);
                                    break;
                                }
                                else
                                {
                                    agenciaCodigoCedente = string.Format("{0}.{1}.000{2}-{3}", Utils.FormatCode(this.Cedente.ContaBancaria.Agencia, 4), this.Cedente.ContaBancaria.OperacaoConta, this.Cedente.Codigo, this.Cedente.DigitoCedente);
                                    break;
                                }
                            }
                        case 41:
                            agenciaCodigoCedente = string.Format("{0}.{1}/{2}.{3}.{4}", this.Cedente.ContaBancaria.Agencia, this.Cedente.ContaBancaria.DigitoAgencia, this.Cedente.Codigo.Substring(4, 6), this.Cedente.Codigo.Substring(10, 1), this.Cedente.DigitoCedente);
                            break;
                        default:
                            agenciaCodigoCedente = string.Format("{0}/{1}-{2}", this.Cedente.ContaBancaria.Agencia, Utils.FormatCode(this.Cedente.Codigo.ToString(), 6), this.Cedente.DigitoCedente.ToString());
                            break;
                    }
                }
                else
                {
                    if (Boleto.Banco.Codigo == 033)
                    {
                        agenciaCodigoCedente = string.Format("{0}-{1}/{2}", this.Cedente.ContaBancaria.Agencia, this.Cedente.ContaBancaria.DigitoAgencia, Utils.FormatCode(this.Cedente.Codigo.ToString(), 6));
                        if (String.IsNullOrEmpty(this.Cedente.ContaBancaria.DigitoAgencia))
                            agenciaCodigoCedente = String.Format("{0}/{1}", this.Cedente.ContaBancaria.Agencia, Utils.FormatCode(this.Cedente.Codigo.ToString(), 6));
                    }
                    else if (Boleto.Banco.Codigo == 399)
                        agenciaCodigoCedente = string.Format("{0} {1}", this.Cedente.ContaBancaria.Agencia, this.Cedente.ContaBancaria.Conta);
                    else if (Boleto.Banco.Codigo == 748)
                        agenciaCodigoCedente = string.Format("{0}.{1}.{2}", this.Cedente.ContaBancaria.Agencia, this.Cedente.ContaBancaria.OperacaoConta, this.Cedente.Codigo);
                    else if (Boleto.Banco.Codigo == 756)
                        agenciaCodigoCedente = string.Format("{0}/{1}", Utils.FormatCode(this.Cedente.ContaBancaria.Agencia, 4), this.Cedente.Codigo);
                    else if (Boleto.Banco.Codigo == 1)
                        agenciaCodigoCedente = string.Format("{0}/{1}-{2}", this.Cedente.ContaBancaria.Agencia, Utils.FormatCode(this.Cedente.ContaBancaria.Conta, 6), this.Cedente.ContaBancaria.DigitoConta);
                    else
                        agenciaCodigoCedente = agenciaConta;
                }

                return agenciaCodigoCedente;
            }

        }
        public string ExibeDadosCedente
        {
            get
            {
                var str = "";

                if (this.Boleto == null)
                    return str;

                if (this.Boleto.Banco == null)
                    return str;

                if (this.Boleto.Cedente == null)
                    return str;

                if (this.Boleto.Cedente.Endereco == null)
                    return str;

                if (!this.Boleto.Banco.Equals(104))
                {
                    str = this.Boleto.Cedente.Nome + " CNPJ: " + this.Boleto.Cedente.CPFCNPJ + " <br /> " +
                           this.Boleto.Cedente.Endereco.End + " - " + this.Boleto.Cedente.Endereco.Cidade +
                           this.Boleto.Cedente.Endereco.UF + " - " + this.Boleto.Cedente.Endereco.CEP;

                }

                str = str.Replace("–", "-");
                return str;
            }
        }

        public string ExibeNossoNumero
        {
            get
            {
                if (this.Boleto == null)
                    return string.Empty;

                if (this.Boleto.Banco.Codigo == 1 && (this.Boleto.Carteira.Equals("17-019") || this.Boleto.Carteira.Equals("18-019")))
                    return this.Boleto.NossoNumero.Substring(3);

                else
                    return this.Boleto.NossoNumero;
            }

        }
        public string ExibeValorDocumento
        {
            get
            {
                if (this.Boleto == null)
                    return "";

                if (this.Boleto.ValorBoleto == 0)
                    return "";

                return this.Boleto.ValorBoleto.ToString("R$ ##,##0.00");
            }

        }
        public string ExibeValorCobrado
        {
            get
            {
                if (this.Boleto == null)
                    return "";

                if (this.Boleto.ValorCobrado == 0)
                    return "";

                return this.Boleto.ValorCobrado.ToString("R$ ##,##0.00");
            }

        }
        public string ExibeCodigoBanco
        {
            get
            {
                return Utils.FormatCode(_ibanco.Codigo.ToString(), 3);
            }

        }
        public string ExibeDigitoBanco
        {
            get
            {
                return _ibanco.Digito.ToString();
            }

        }
        public string ExibeEspecieDocumento
        {
            get
            {
                if (this.Boleto == null)
                    return "";

                return EspecieDocumento.ValidaSigla(this.Boleto.EspecieDocumento);
            }

        }
        public string ExibeDataProcessamento
        {
            get
            {
                if (this.Boleto == null)
                    return "";

                return this.Boleto.DataProcessamento.ToString("dd/MM/yyyy");
            }

        }
        public string ExibeDataDocumento
        {
            get
            {
                if (this.Boleto == null)
                    return "";

                return this.Boleto.DataDocumento.ToString("dd/MM/yyyy");
            }

        }
        public string ExibeQuantidade
        {
            get
            {
                if (this.Boleto == null)
                    return "";

                if (this.Boleto.QuantidadeMoeda == 0)
                    return "";

                return Boleto.QuantidadeMoeda.ToString();
            }

        }
        public string ExibeOutrosAcrescimos
        {
            get
            {
                if (this.Boleto == null)
                    return "";

                if (this.Boleto.OutrosAcrescimos == 0)
                    return "";

                return Boleto.OutrosAcrescimos.ToString("R$ ##,##0.00");
            }

        }
        public string ExibeOutrasDeducoes
        {
            get
            {
                if (this.Boleto == null)
                    return "";

                if (this.Boleto.OutrosDescontos == 0)
                    return "";

                return Boleto.OutrosDescontos.ToString("R$ ##,##0.00");
            }

        }
        public string ExibeDescontos
        {
            get
            {
                if (this.Boleto == null)
                    return "";

                if (this.Boleto.ValorDesconto == 0)
                    return "";

                return Boleto.ValorDesconto.ToString("R$ ##,##0.00");
            }

        }
        public string ExibeMoraMulta
        {
            get
            {
                if (this.Boleto == null)
                    return "";

                if (this.Boleto.ValorMulta == 0)
                    return "";

                return Boleto.ValorMulta.ToString("R$ ##,##0.00");
            }

        }
        public string ExibeTituloInstrucoesImpressao
        {
            get
            {
                return "Instruções de Impressão";
            }

        }
        public string ExibeInstrucoesImpressao
        {
            get
            {
                return "Imprimir em impressora jato de tinta (ink jet) ou laser em qualidade normal. (Não use modo econômico).<br />Utilize folha A4 (210 x 297 mm) ou Carta (216 x 279 mm) - Corte na linha indicada<br />";
            }

        }
        public string ExibeInstrucoesSacado
        {
            get
            {
                var instrucao = "";

                if (this.Boleto == null)
                    return instrucao;

                if (this.Boleto.Banco == null)
                    return instrucao;

                if (this.Boleto.Sacado == null)
                    return instrucao;

                if (this.Boleto.Banco.Codigo == 104)
                    return "SAC CAIXA: 0800 726 0101 (informações, reclamações, sugestões e elogios) <br />" +
                           "         Para pessoas com deficiência auditiva ou de fala: 0800 726 2492 <br />" +
                           "    Ouvidoria: 0800 725 7474 (reclamações não solucionadas e denúncias) <br />" +
                           "                                                  caixa.gov.br <br />";

                if (this.Boleto.Sacado.Instrucoes.Count > 0)
                    instrucao = this.MontaInstrucoes(this.Boleto.Sacado.Instrucoes);

                return instrucao;
            }


        }
        public string ExibeTituloInstrucoesCedente
        {
            get
            {
                var titulo = "Instruções (Texto de responsabilidade do beneficiário)";

                if (Boleto.Banco.Codigo == 341)
                    titulo = "Instruções de responsabilidade do BENEFICIÁRIO. Qualquer dúvida sobre este boleto, contate o BENEFICIÁRIO.";

                if (Boleto.Banco.Codigo == 237)
                    titulo = "Informações de responsabilidade do beneficiário.";

                return titulo;
            }

        }
        public string ExibeInstrucoesCedente
        {
            get
            {
                var instrucao = "";

                if (this.Boleto == null)
                    return instrucao;

                if (this.Boleto.Sacado == null)
                    return instrucao;

                if (this.Boleto.Cedente.Instrucoes.Count > 0)
                    instrucao = this.MontaInstrucoes(this.Boleto.Cedente.Instrucoes);

                return instrucao;
            }


        }

        #endregion

        #region Variaveis

        private Banco _ibanco = null;
        private short _codigoBanco = 0;
        private Boleto _boleto;
        private Cedente _cedente;
        private Sacado _sacado;
        private List<IInstrucao> _instrucoes = new List<IInstrucao>();

        private bool _mostrarCodigoCarteira = false;
        private bool _formatoCarne = false;

        #endregion Variaveis

        #region Propriedades

        [Browsable(true), Description("Código do banco em que será gerado o boleto. Ex. 341-Itaú, 237-Bradesco")]
        public short CodigoBanco
        {
            get { return _codigoBanco; }
            set { _codigoBanco = value; }
        }

        /// <summary>
        /// Mostra o código da carteira
        /// </summary>
        [Browsable(true), Description("Mostra a descrição da carteira")]
        public bool MostrarCodigoCarteira
        {
            get { return _mostrarCodigoCarteira; }
            set { _mostrarCodigoCarteira = value; }
        }

        /// <summary>
        /// Mostra o código da carteira
        /// </summary>
        [Browsable(true), Description("Formata o boleto no layout de carnê")]
        public bool FormatoCarne
        {
            get { return _formatoCarne; }
            set { _formatoCarne = value; }
        }

        [Browsable(false)]
        public Boleto Boleto
        {
            get { return _boleto; }
            set
            {
                _boleto = value;

                if (_ibanco == null)
                    _boleto.Banco = this.Banco;

                _cedente = _boleto.Cedente;
                _sacado = _boleto.Sacado;
            }
        }

        [Browsable(false)]
        public Sacado Sacado
        {
            get { return _sacado; }
        }

        [Browsable(false)]
        public Cedente Cedente
        {
            get { return _cedente; }
        }

        [Browsable(false)]
        public Banco Banco
        {
            get
            {
                if (_ibanco == null || _ibanco.Codigo != _codigoBanco)
                    _ibanco = new Banco(_codigoBanco);

                if (_boleto != null)
                    _boleto.Banco = _ibanco;

                return _ibanco;
            }
        }

        [Browsable(true), Description("Mostra o comprovante de entrega sem dados para marcar")]
        public bool MostrarComprovanteEntregaLivre
        {
            get { return Utils.ToBool(ViewState["1"]); }
            set { ViewState["1"] = value; }
        }

        [Browsable(true), Description("Mostra o comprovante de entrega")]
        public bool MostrarComprovanteEntrega
        {
            get { return Utils.ToBool(ViewState["2"]); }
            set { ViewState["2"] = value; }
        }

        [Browsable(true), Description("Oculta as intruções do boleto")]
        public bool OcultarEnderecoSacado
        {
            get { return Utils.ToBool(ViewState["3"]); }
            set { ViewState["3"] = value; }
        }

        [Browsable(true), Description("Oculta as intruções do boleto")]
        public bool OcultarInstrucoes
        {
            get { return Utils.ToBool(ViewState["4"]); }
            set { ViewState["4"] = value; }
        }

        [Browsable(true), Description("Oculta o recibo do sacado do boleto")]
        public bool OcultarReciboSacado
        {
            get { return Utils.ToBool(ViewState["5"]); }
            set { ViewState["5"] = value; }
        }

        [Browsable(true), Description("Gerar arquivo de remessa")]
        public bool GerarArquivoRemessa
        {
            get { return Utils.ToBool(ViewState["6"]); }
            set { ViewState["6"] = value; }
        }

        public bool MostrarContraApresentacaoNaDataVencimento
        {
            get { return Utils.ToBool(ViewState["7"]); }
            set { ViewState["7"] = value; }
        }

        [Browsable(true), Description("Mostra o endereço do Cedente")]
        public bool MostrarEnderecoCedente
        {
            get { return Utils.ToBool(ViewState["8"]); }
            set { ViewState["8"] = value; }
        }

        public List<IInstrucao> Instrucoes
        {
            get
            {
                return _instrucoes;
            }
        }

        #endregion Propriedades

        #region Override
        protected override void OnPreRender(EventArgs e)
        {
            string alias = "BoletoNet.BoletoImpressao.BoletoNet.css";
            string csslink = "<link rel=\"stylesheet\" type=\"text/css\" href=\"" +
                Page.ClientScript.GetWebResourceUrl(typeof(BoletoBancario), alias) + "\" />";

            var include = new LiteralControl(csslink);
            Page.Header.Controls.Add(include);

            base.OnPreRender(e);
        }

        protected override void OnLoad(EventArgs e)
        {
        }

        [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "Execution")]
        protected override void Render(HtmlTextWriter output)
        {
            if (_ibanco == null)
            {
                output.Write("<b>Erro gerando o boleto bancário: faltou definir o banco.</b>");
                return;
            }
            string urlImagemLogo = Page.ClientScript.GetWebResourceUrl(typeof(BoletoBancario), "BoletoNet.Imagens." + Utils.FormatCode(_ibanco.Codigo.ToString(), 3) + ".jpg");
            string urlImagemBarra = Page.ClientScript.GetWebResourceUrl(typeof(BoletoBancario), "BoletoNet.Imagens.barra.gif");
            output.Write(MontaHtml(urlImagemLogo, urlImagemBarra, "<img src=\"ImagemCodigoBarra.ashx?code=" + Boleto.CodigoBarra.Codigo + "\" alt=\"Código de Barras\" />"));
        }
        #endregion Override

        #region Html

        public string GeraHtmlInstrucoes()
        {
            try
            {
                var html = new StringBuilder();

                html.Append(Html.Instrucoes);
                html.Append("<br />");

                return html.ToString()
                    .Replace("@TITULO", this.ExibeTituloInstrucoesImpressao)
                    .Replace("@INSTRUCAO", this.ExibeInstrucoesImpressao);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a execução da transação.", ex);
            }
        }

        private string GeraHtmlCarne(string telefone, string htmlBoleto)
        {
            var html = new StringBuilder();

            html.Append(Html.Carne);

            return html.ToString()
                .Replace("@TELEFONE", telefone)
                .Replace("#BOLETO#", htmlBoleto);
        }

        public string GeraHtmlReciboSacado()
        {
            try
            {
                var html = new StringBuilder();

                if (Boleto.Banco.Codigo == 104)
                {
                    html.Append(HtmlCaixa.ReciboSacadoParte1);
                    html.Append("<br />");
                    html.Append(HtmlCaixa.ReciboSacadoParte2);
                    html.Append(HtmlCaixa.ReciboSacadoParte3);

                    if (MostrarEnderecoCedente)
                        html.Append(HtmlCaixa.ReciboSacadoParte10);

                    html.Append(HtmlCaixa.ReciboSacadoParte4);
                    html.Append(HtmlCaixa.ReciboSacadoParte5);
                    html.Append(HtmlCaixa.ReciboSacadoParte6);
                    html.Append(HtmlCaixa.ReciboSacadoParte7);
                    html.Append(HtmlCaixa.ReciboSacadoParte8);
                    html.Append(HtmlCaixa.ReciboSacadoParte9);

                    html.Replace("Carteira /", "");
                }
                else
                {
                    html.Append(Html.ReciboSacadoParte1);
                    html.Append("<br />");
                    html.Append(Html.ReciboSacadoParte2);
                    html.Append(Html.ReciboSacadoParte3);

                    if (MostrarEnderecoCedente)
                    {
                        html.Append(Html.ReciboSacadoParte10);
                    }

                    html.Append(Html.ReciboSacadoParte4);
                    html.Append(Html.ReciboSacadoParte5);
                    html.Append(Html.ReciboSacadoParte6);
                    html.Append(Html.ReciboSacadoParte7);
                    html.Append(Html.ReciboSacadoParte8);

                    if (Boleto.Banco.Codigo.Equals(033) || Boleto.Banco.Codigo.Equals(399))
                        html.Replace("Carteira /", "");

                }

                return html.ToString().Replace("@INSTRUCOESSACADO", this.ExibeInstrucoesSacado);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a execução da transação.", ex);
            }
        }

        public string GeraHtmlReciboCedente()
        {
            try
            {
                var html = new StringBuilder();

                if (Boleto.Banco.Codigo == 104)
                {
                    html.Append(HtmlCaixa.ReciboCedenteParte1);
                    html.Append(HtmlCaixa.ReciboCedenteParte2);
                    html.Append(HtmlCaixa.ReciboCedenteParte3);
                    html.Append(HtmlCaixa.ReciboCedenteParte4);
                    html.Append(HtmlCaixa.ReciboCedenteParte5);
                    html.Append(HtmlCaixa.ReciboCedenteParte6);
                    html.Append(HtmlCaixa.ReciboCedenteParte7);
                    html.Append(HtmlCaixa.ReciboCedenteParte8);
                    html.Append(HtmlCaixa.ReciboCedenteParte9);
                    html.Append(HtmlCaixa.ReciboCedenteParte10);

                    html.Replace("Carteira /", "");
                    html.Replace("@CEDENTE", "@CEDENTE - CNPJ: @CPFCNPJ <br /> @CEDENTEENDERECO");
                }
                else
                {
                    html.Append(Html.ReciboCedenteParte1);
                    html.Append(Html.ReciboCedenteParte2);
                    html.Append(Html.ReciboCedenteParte3);
                    html.Append(Html.ReciboCedenteParte4);
                    html.Append(Html.ReciboCedenteParte5);
                    html.Append(Html.ReciboCedenteParte6);
                    html.Append(Html.ReciboCedenteParte7);
                    html.Append(Html.ReciboCedenteParte8);
                    html.Append(Html.ReciboCedenteParte9);
                    html.Append(Html.ReciboCedenteParte10);

                    html.Replace("Instruções (Texto de responsabilidade do beneficiário)", this.ExibeTituloInstrucoesCedente);

                    if (Boleto.Banco.Codigo == 399)
                        html.Replace("Espécie doc.", "PD");

                    if (Boleto.Banco.Codigo == 1 && (Boleto.Carteira.Equals("17-019") || Boleto.Carteira.Equals("18-019")))
                        html.Replace("Carteira /", "");

                    if (Boleto.Banco.Codigo.Equals(033) || Boleto.Banco.Codigo.Equals(399))
                        html.Replace("Carteira /", "");

                }

                return html.ToString().Replace("@INSTRUCOESCEDENTE", this.ExibeInstrucoesCedente);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro na execução da transação.", ex);
            }
        }

        public string HtmlComprovanteEntrega
        {
            get
            {
                var html = new StringBuilder();

                html.Append(Html.ComprovanteEntrega1);
                html.Append(Html.ComprovanteEntrega2);
                html.Append(Html.ComprovanteEntrega3);
                html.Append(Html.ComprovanteEntrega4);
                html.Append(Html.ComprovanteEntrega5);
                html.Append(Html.ComprovanteEntrega6);

                html.Append(MostrarComprovanteEntregaLivre ? Html.ComprovanteEntrega71 : Html.ComprovanteEntrega7);

                html.Append("<br />");
                return html.ToString();
            }
        }

        private string MontaInstrucoes(IList<IInstrucao> instrucoes)
        {
            var printInstrucoes = "";

            if (instrucoes.Count > 0)
            {
                foreach (IInstrucao instrucao in instrucoes)
                    printInstrucoes += string.Format("{0}<br />", instrucao.Descricao);

                printInstrucoes = Strings.Left(printInstrucoes, printInstrucoes.Length - 6);
            }

            return printInstrucoes;
        }

        private string MontaHtml(string urlImagemLogo, string urlImagemBarra, string imagemCodigoBarras)
        {
            var html = new StringBuilder();

            if (!this.OcultarInstrucoes)
                html.Append(this.GeraHtmlInstrucoes());

            if (!this.FormatoCarne)
            {
                if (this.MostrarComprovanteEntrega || this.MostrarComprovanteEntregaLivre)
                {
                    html.Append(this.HtmlComprovanteEntrega);
                    if (this.OcultarReciboSacado)
                        html.Append(Html.ReciboSacadoParte8);
                }

                if (!this.OcultarReciboSacado)
                    html.Append(this.GeraHtmlReciboSacado());

                html.Append(this.GeraHtmlReciboCedente());
            }
            else
                html.Append(this.GeraHtmlCarne("", this.GeraHtmlReciboCedente()));

            if (String.IsNullOrEmpty(vLocalLogoCedente))
                vLocalLogoCedente = urlImagemLogo;

            if (Boleto.Banco.Codigo == 104)
            {
                html.Replace("@UF", this.Boleto.Cedente.Endereco.UF)
                    .Replace("@CEP", this.Boleto.Cedente.Endereco.CEP)
                    .Replace("@ENDERECOCEDENTE", this.Boleto.Cedente.Endereco.End)
                    .Replace("@CEDENTEENDERECO", this.ExibeEnderecoCedente);
            }
            else
                html.Replace("@CEDENTECOMDADOS", this.ExibeDadosCedente);

            if (this.Boleto.Banco.Codigo.Equals(001))
                html.Replace("@CEDENTESACADO", this.ExibeDadosCedente);

            return html
                .Replace("@=VALORDOCUMENTO", this.ExibeValorDocumento)
                .Replace("@VALORCOBRADO", this.ExibeValorCobrado)
                .Replace("@CODIGOBANCO", this.ExibeCodigoBanco)
                .Replace("@DIGITOBANCO", this.ExibeDigitoBanco)
                .Replace("@URLIMAGEMLOGO", urlImagemLogo)
                .Replace("@URLIMGCEDENTE", vLocalLogoCedente)
                .Replace("@URLIMAGEMBARRA", urlImagemBarra)
                .Replace("@LINHADIGITAVEL", this.Boleto.CodigoBarra.LinhaDigitavel)
                .Replace("@LOCALPAGAMENTO", this.Boleto.LocalPagamento)
                .Replace("@DATAVENCIMENTO", this.ExibeDataVencimento)
                .Replace("@CEDENTESACADO", this.ExibeNomeCedente)
                .Replace("@CEDENTE", this.Cedente.Nome)
                .Replace("@DATADOCUMENTO", this.ExibeDataDocumento)
                .Replace("@NUMERODOCUMENTO", this.Boleto.NumeroDocumento)
                .Replace("@ESPECIEDOCUMENTO", this.ExibeEspecieDocumento)
                .Replace("@DATAPROCESSAMENTO", this.ExibeDataProcessamento)
                .Replace("@NOSSONUMERO", this.ExibeNossoNumero)
                .Replace("@CARTEIRA", this.ExibeDescricaoCarteira)
                .Replace("@ESPECIE", this.Boleto.Especie)
                .Replace("@QUANTIDADE", this.ExibeQuantidade)
                .Replace("@VALORDOCUMENTO", this.Boleto.ValorMoeda)
                .Replace("@OUTROSACRESCIMOS", this.ExibeOutrosAcrescimos)
                .Replace("@OUTRASDEDUCOES", this.ExibeOutrasDeducoes)
                .Replace("@DESCONTOS", this.ExibeDescontos)
                .Replace("@AGENCIACONTA", this.ExibeAgenciaCodigoCedente)
                .Replace("@SACADO", this.ExibeSacado)
                .Replace("@INFOSACADO", this.ExibeInfoSacado)
                .Replace("@AGENCIACODIGOCEDENTE", this.ExibeAgenciaCodigoCedente)
                .Replace("@CPFCNPJ", this.Cedente.CPFCNPJ)
                .Replace("@MORAMULTA", this.ExibeMoraMulta)
                .Replace("@AUTENTICACAOMECANICA", "")
                .Replace("@USODOBANCO", this.Boleto.UsoBanco)
                .Replace("@IMAGEMCODIGOBARRA", imagemCodigoBarras)
                .Replace("@ACEITE", this.Boleto.Aceite).ToString()
                .Replace("@ENDERECOCEDENTE", this.ExibeEnderecoCedente);
        }

        #endregion Html

        #region Geração do Html OffLine

        protected StringBuilder HtmlOffLine(string textoNoComecoDoEmail, string srcLogo, string srcBarra, string srcCodigoBarra)
        {
            this.OnLoad(EventArgs.Empty);

            StringBuilder html = new StringBuilder();
            HtmlOfflineHeader(html);
            if (textoNoComecoDoEmail != null && textoNoComecoDoEmail != "")
                html.Append(textoNoComecoDoEmail);

            html.Append(MontaHtml(srcLogo, srcBarra, "<img src=\"" + srcCodigoBarra + "\" alt=\"Código de Barras\" />"));
            HtmlOfflineFooter(html);
            return html;
        }

        public string MontaHtmlExportacao(string path)
        {
            this.OnLoad(EventArgs.Empty);

            string fnLogo = Path.Combine(path, Utils.FormatCode(_ibanco.Codigo.ToString(), 3) + ".jpg");

            string fnBarra = Path.Combine(path, "Barra.gif");

            C2of5i cb = new C2of5i(Boleto.CodigoBarra.Codigo, 1, 50, Boleto.CodigoBarra.Codigo.Length);
            string strCodigoBarras = cb.ToString(path);

            return HtmlOffLine(null, fnLogo, fnBarra, strCodigoBarras).ToString();
        }

        public string MontaHtmlExporta(string path)
        {
            this.OnLoad(EventArgs.Empty);

            string fnLogo = Path.Combine(path, Utils.FormatCode(_ibanco.Codigo.ToString(), 3) + ".jpg");

            string fnBarra = Path.Combine(path, "Barra.gif");

            C2of5i cb = new C2of5i(Boleto.CodigoBarra.Codigo, 1, 50, Boleto.CodigoBarra.Codigo.Length);
            string strCodigoBarras = cb.ToString(path);

            return HtmlBoletoOffLine(null, fnLogo, fnBarra, null, strCodigoBarras).ToString();
        }

        protected StringBuilder HtmlBoletoOffLine(string textoNoComecoDoEmail, string srcLogo, string srcBarra, string srcCodigoBarra)
        {
            return HtmlBoletoOffLine(textoNoComecoDoEmail, srcLogo, srcBarra, srcCodigoBarra, null);
        }

        protected StringBuilder HtmlBoletoOffLine(string textoNoComecoDoEmail, string srcLogo, string srcBarra, string srcCodigoBarra, string strCodigoBarra)
        {
            this.OnLoad(EventArgs.Empty);

            StringBuilder html = new StringBuilder();
            if (textoNoComecoDoEmail != null && textoNoComecoDoEmail != "")
            {
                html.Append(textoNoComecoDoEmail);
            }

            if (srcCodigoBarra != null && srcCodigoBarra != "")
                html.Append(MontaHtml(srcLogo, srcBarra, "<img src=\"" + srcCodigoBarra + "\" alt=\"Código de Barras\" />"));

            if (strCodigoBarra != null && strCodigoBarra != "")
                html.Append(MontaHtml(srcLogo, srcBarra, strCodigoBarra));

            return html;
        }

        protected static void HtmlOfflineHeader(StringBuilder html)
        {
            html.Append("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">\n");
            html.Append("<html xmlns=\"http://www.w3.org/1999/xhtml\">\n");
            html.Append("<head>");
            html.Append("    <title>Boleto.Net</title>\n");

            Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("BoletoNet.BoletoImpressao.BoletoNet.css");

            using (StreamReader sr = new StreamReader(stream))
            {
                html.Append("<style>\n");
                html.Append(sr.ReadToEnd());
                html.Append("</style>\n");
                sr.Close();
                sr.Dispose();
            }

            html.Append("     </head>\n");
            html.Append("<body>\n");
        }

        protected static void HtmlOfflineFooter(StringBuilder saida)
        {
            saida.Append("</body>\n");
            saida.Append("</html>\n");
        }

        public static AlternateView GeraHtmlDeVariosBoletosParaEmail(BoletoBancario[] arrayDeBoletos)
        {
            return GeraHtmlDeVariosBoletosParaEmail(null, arrayDeBoletos);
        }

        public static AlternateView GeraHtmlDeVariosBoletosParaEmail(string textoNoComecoDoEmail, BoletoBancario[] arrayDeBoletos)
        {
            var corpoDoEmail = new StringBuilder();

            var linkedResources = new List<LinkedResource>();

            HtmlOfflineHeader(corpoDoEmail);

            if (textoNoComecoDoEmail != null && textoNoComecoDoEmail != "")
                corpoDoEmail.Append(textoNoComecoDoEmail);

            foreach (var umBoleto in arrayDeBoletos)
            {
                if (umBoleto != null)
                {
                    LinkedResource lrImagemLogo;
                    LinkedResource lrImagemBarra;
                    LinkedResource lrImagemCodigoBarra;
                    umBoleto.GeraGraficosParaEmailOffLine(out lrImagemLogo, out lrImagemBarra, out lrImagemCodigoBarra);
                    var theOutput = umBoleto.MontaHtml(
                        "cid:" + lrImagemLogo.ContentId,
                        "cid:" + lrImagemBarra.ContentId,
                        "<img src=\"cid:" + lrImagemCodigoBarra.ContentId + "\" alt=\"Código de Barras\" />");

                    corpoDoEmail.Append(theOutput);

                    linkedResources.Add(lrImagemLogo);
                    linkedResources.Add(lrImagemBarra);
                    linkedResources.Add(lrImagemCodigoBarra);
                }
            }

            HtmlOfflineFooter(corpoDoEmail);

            AlternateView av = AlternateView.CreateAlternateViewFromString(corpoDoEmail.ToString(), Encoding.Default, "text/html");
            foreach (var theResource in linkedResources)
                av.LinkedResources.Add(theResource);

            return av;
        }

        public AlternateView HtmlBoletoParaEnvioEmail()
        {
            return HtmlBoletoParaEnvioEmail(null);
        }

        public AlternateView HtmlBoletoParaEnvioEmail(string textoNoComecoDoEmail)
        {
            LinkedResource lrImagemLogo;
            LinkedResource lrImagemBarra;
            LinkedResource lrImagemCodigoBarra;

            GeraGraficosParaEmailOffLine(out lrImagemLogo, out lrImagemBarra, out lrImagemCodigoBarra);
            StringBuilder html = HtmlOffLine(textoNoComecoDoEmail, "cid:" + lrImagemLogo.ContentId, "cid:" + lrImagemBarra.ContentId, "cid:" + lrImagemCodigoBarra.ContentId);

            AlternateView av = AlternateView.CreateAlternateViewFromString(html.ToString(), Encoding.Default, "text/html");

            av.LinkedResources.Add(lrImagemLogo);
            av.LinkedResources.Add(lrImagemBarra);
            av.LinkedResources.Add(lrImagemCodigoBarra);
            return av;
        }

        void GeraGraficosParaEmailOffLine(out LinkedResource lrImagemLogo, out LinkedResource lrImagemBarra, out LinkedResource lrImagemCodigoBarra)
        {
            this.OnLoad(EventArgs.Empty);

            var randomSufix = new Random().Next().ToString();

            Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("BoletoNet.Imagens." + Utils.FormatCode(_ibanco.Codigo.ToString(), 3) + ".jpg");
            lrImagemLogo = new LinkedResource(stream, MediaTypeNames.Image.Jpeg);
            lrImagemLogo.ContentId = "logo" + randomSufix;


            MemoryStream ms = new MemoryStream(Utils.ConvertImageToByte(Html.barra));
            lrImagemBarra = new LinkedResource(ms, MediaTypeNames.Image.Gif);
            lrImagemBarra.ContentId = "barra" + randomSufix; ;

            C2of5i cb = new C2of5i(Boleto.CodigoBarra.Codigo, 1, 50, Boleto.CodigoBarra.Codigo.Length);
            ms = new MemoryStream(Utils.ConvertImageToByte(cb.ToBitmap()));

            lrImagemCodigoBarra = new LinkedResource(ms, MediaTypeNames.Image.Gif);
            lrImagemCodigoBarra.ContentId = "codigobarra" + randomSufix; ;

        }

        public void MontaHtmlNoArquivoLocal(string fileName)
        {
            using (FileStream f = new FileStream(fileName, FileMode.Create))
            {
                StreamWriter w = new StreamWriter(f, System.Text.Encoding.Default);
                w.Write(MontaHtml());
                w.Close();
                f.Close();
            }
        }

        public string MontaHtml()
        {
            return MontaHtml(null, null);
        }

        public string MontaHtml(string fileName, string logoCedente)
        {
            if (fileName == null)
                fileName = System.IO.Path.GetTempPath();

            if (logoCedente != null)
                vLocalLogoCedente = logoCedente;

            this.OnLoad(EventArgs.Empty);

            string fnLogo = fileName + @"BoletoNet" + Utils.FormatCode(_ibanco.Codigo.ToString(), 3) + ".jpg";

            if (!System.IO.File.Exists(fnLogo))
                Image.FromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("BoletoNet.Imagens." + Utils.FormatCode(_ibanco.Codigo.ToString(), 3) + ".jpg")).Save(fnLogo);

            string fnBarra = fileName + @"BoletoNetBarra.gif";
            if (!File.Exists(fnBarra))
                Html.barra.Save(fnBarra);

            string fnCodigoBarras = System.IO.Path.GetTempFileName();
            C2of5i cb = new C2of5i(Boleto.CodigoBarra.Codigo, 1, 50, Boleto.CodigoBarra.Codigo.Length);
            cb.ToBitmap().Save(fnCodigoBarras);

            return HtmlOffLine(null, fnLogo, fnBarra, fnCodigoBarras).ToString();
        }

        public string MontaHtml(string url, string fileName, bool useMapPathSecure = true)
        {
            string pathServer = "";

            if (url == null)
                throw new ArgumentException("Você precisa informar uma pasta padrão.");

            else
            {
                if (useMapPathSecure)
                {
                    if (url.Substring(url.Length - 1, 1) != "/")
                        url = url + "/";

                    if (url.Substring(0, 1) != "/")
                        url = url + "/";

                    pathServer = MapPathSecure(string.Format("~{0}", url));
                }

                if (!Directory.Exists(pathServer))
                    throw new ArgumentException("A o caminho físico '{0}' não existe.", pathServer);
            }
            if (fileName == null)
                fileName = DateTime.Now.Ticks.ToString();

            else
            {
                if (fileName == "")
                    fileName = DateTime.Now.Ticks.ToString();
            }

            this.OnLoad(EventArgs.Empty);

            string fnLogo = pathServer + @"BoletoNet" + Utils.FormatCode(_ibanco.Codigo.ToString(), 3) + ".jpg";
            string fnLogoUrl = url + @"BoletoNet" + Utils.FormatCode(_ibanco.Codigo.ToString(), 3) + ".jpg";

            if (!System.IO.File.Exists(fnLogo))
                Image.FromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("BoletoNet.Imagens." + Utils.FormatCode(_ibanco.Codigo.ToString(), 3) + ".jpg")).Save(fnLogo);

            string fnBarra = pathServer + @"BoletoNetBarra.gif";
            string fnBarraUrl = url + @"BoletoNetBarra.gif";

            if (!File.Exists(fnBarra))
                Html.barra.Save(fnBarra);

            string fnCodigoBarras = string.Format("{0}{1}_codigoBarras.jpg", pathServer, fileName);
            string fnCodigoBarrasUrl = string.Format("{0}{1}_codigoBarras.jpg", url, fileName);

            C2of5i cb = new C2of5i(Boleto.CodigoBarra.Codigo, 1, 50, Boleto.CodigoBarra.Codigo.Length);

            cb.ToBitmap().Save(fnCodigoBarras);

            return HtmlOffLine(null, fnLogoUrl, fnBarraUrl, fnCodigoBarrasUrl).ToString();
        }

        public string MontaHtmlEmbedded()
        {
            OnLoad(EventArgs.Empty);

            var assembly = Assembly.GetExecutingAssembly();

            var streamLogo = assembly.GetManifestResourceStream("BoletoNet.Imagens." + CodigoBanco.ToString("000") + ".jpg");
            string base64Logo = Convert.ToBase64String(new BinaryReader(streamLogo).ReadBytes((int)streamLogo.Length));
            string fnLogo = string.Format("data:image/gif;base64,{0}", base64Logo);

            var streamBarra = assembly.GetManifestResourceStream("BoletoNet.Imagens.barra.gif");
            string base64Barra = Convert.ToBase64String(new BinaryReader(streamBarra).ReadBytes((int)streamBarra.Length));
            string fnBarra = string.Format("data:image/gif;base64,{0}", base64Barra);

            var cb = new C2of5i(Boleto.CodigoBarra.Codigo, 1, 50, Boleto.CodigoBarra.Codigo.Length);
            string base64CodigoBarras = Convert.ToBase64String(cb.ToByte());
            string fnCodigoBarras = string.Format("data:image/gif;base64,{0}", base64CodigoBarras);

            return HtmlOffLine(null, fnLogo, fnBarra, fnCodigoBarras).ToString();
        }

        #endregion Geração do Html OffLine

        #region Geração do Html Básico para impressão em PDF

        public string MontaHtmlBasico(String pathImagens, String Observacoes)
        {
            return MontaHtmlBasico(pathImagens, Observacoes, string.Empty);
        }

        public string MontaHtmlBasico(String pathImagens, String Observacoes, String Instrucoes)
        {
            StringBuilder sb = new StringBuilder();

            C2of5i cb = new C2of5i(Boleto.CodigoBarra.Codigo, 1, 50, Boleto.CodigoBarra.Codigo.Length);

            string fnLogo = pathImagens + Utils.FormatCode(_ibanco.Codigo.ToString(), 3) + ".jpg";
            string strCodigoBarras = cb.ToString(pathImagens);

            sb.AppendLine("<p align=\"center\"> ");
            sb.AppendLine("<TABLE WIDTH=520 CELLSPACING=0 CELLPADDING=0 BORDER=0> ");
            sb.AppendLine("  	<TR> ");
            sb.AppendLine("		<TD class=cp VALIGN=BOTTOM WIDTH=225><img src=\"" + fnLogo + "\" border=0></TD> ");
            sb.AppendLine("		<TD ALIGN=RIGHT VALIGN=BOTTOM><FONT class=ld><B>RECIBO DO SACADO</B></FONT></TD> ");
            sb.AppendLine("	</TR> ");
            sb.AppendLine("</TABLE> ");
            sb.AppendLine("<TABLE WIDTH=520 BORDER=1 CELLSPACING=0 CELLPADDING=1> ");
            sb.AppendLine("	<TR> ");
            sb.AppendLine("		<TD COLSPAN=2> ");
            sb.AppendLine("			<FONT class=ct>Cedente</FONT><BR> ");
            sb.AppendLine("			<FONT class=cp>&nbsp;" + Cedente.Nome + "</FONT> ");
            sb.AppendLine("		</TD> ");
            sb.AppendLine("		<TD> ");
            sb.AppendLine("			<FONT class=ct>Ag&ecirc;ncia / C&oacute;digo Cedente</FONT><BR> ");

            if (Cedente.Codigo != "0" && Cedente.Codigo != string.Empty)
                if (_ibanco.Codigo == 134)
                    sb.AppendLine("			<FONT class=cn>" + Cedente.ContaBancaria.Agencia + "/" + Cedente.ContaBancaria.Agencia + "-" + Cedente.ContaBancaria.DigitoAgencia + "&nbsp;/&nbsp;" + Cedente.Codigo + "</FONT> ");
                else
                    sb.AppendLine("			<FONT class=cn>" /*+ Cedente.ContaBancaria.Agencia +*/ + Cedente.ContaBancaria.Agencia + "-" + Cedente.ContaBancaria.DigitoAgencia + "&nbsp;/&nbsp;" + Cedente.Codigo + "</FONT> ");
            else
                sb.AppendLine("			<FONT align=center class=cn>&nbsp;" + Cedente.ContaBancaria.Agencia + "-" + Cedente.ContaBancaria.DigitoAgencia + "&nbsp;/&nbsp;" + Cedente.ContaBancaria.Conta + "-" + Cedente.ContaBancaria.DigitoConta + "</FONT> ");

            sb.AppendLine("		</TD> ");
            sb.AppendLine("		<TD> ");
            sb.AppendLine("			<FONT class=ct>Nosso Número</FONT><BR> ");
            sb.AppendLine("			<FONT class=cn>&nbsp;" + Boleto.NossoNumero + "</FONT> ");
            sb.AppendLine("		</TD> ");
            sb.AppendLine("		<TD bgcolor='#CCCCCC'> ");
            sb.AppendLine("			<TABLE WIDTH=100% BORDER=0 CELLSPACING=0 CELLPADDING=0> ");
            sb.AppendLine("		    	<TR> ");
            sb.AppendLine("			 		<TD align=left><FONT class=ct>Vencimento</FONT></TD> ");
            sb.AppendLine("				</TR> ");
            sb.AppendLine("				<TR> ");
            sb.AppendLine("					<TD align=right><FONT class=cp>" + Boleto.DataVencimento.ToString("dd/MM/yyyy") + "</FONT></TD> ");
            sb.AppendLine("				</TR> ");
            sb.AppendLine("			</TABLE> ");
            sb.AppendLine("		</TD> ");
            sb.AppendLine("	</TR> ");
            sb.AppendLine("	<TR> ");
            sb.AppendLine("   	<TD COLSPAN=2> ");
            sb.AppendLine("			<FONT class=ct>Sacado</FONT><BR> ");
            sb.AppendLine("			<FONT class=cp>&nbsp;" + Sacado.Nome + "</FONT> ");
            sb.AppendLine("		</TD> ");
            sb.AppendLine("<TD valign=top><FONT class=ct>Data Proc.</FONT><BR><FONT class=cn>&nbsp;" + Boleto.DataProcessamento + "</TD> ")                      /*"		<TD valign=\"top\"><FONT class=ct>Data Processamento</FONT></TD> "*/;
            sb.AppendLine("   	<TD> ");
            sb.AppendLine("			<FONT class=ct>Número Documento</FONT><BR> ");
            sb.AppendLine("			<FONT class=cn>&nbsp;" + Boleto.NumeroDocumento + "</FONT> ");
            sb.AppendLine("		</TD> ");
            sb.AppendLine("		<TD bgcolor=\"#CCCCCC\"> ");
            sb.AppendLine("			<TABLE WIDTH=100% BORDER=0 CELLSPACING=0 CELLPADDING=0> ");
            sb.AppendLine("		    	<TR> ");
            sb.AppendLine("			 		<TD align=left><FONT class=ct>Valor do Documento</FONT></TD> ");
            sb.AppendLine("				</TR> ");
            sb.AppendLine("				<TR> ");
            sb.AppendLine("					<TD align=right><FONT class=cp>" + (Boleto.ValorBoleto == 0 ? "" : Boleto.ValorBoleto.ToString("R$ ##,##0.00")) + "</FONT></TD> ");
            sb.AppendLine("				</TR> ");
            sb.AppendLine("			</TABLE> ");
            sb.AppendLine("		</TD> ");
            sb.AppendLine("	</TR> ");
            sb.AppendLine("	<TR> ");
            sb.AppendLine("   	<TD> ");
            sb.AppendLine("			<FONT class=ct>CNPJ do Cedente</FONT><BR> ");
            sb.AppendLine("			<FONT class=cp>" /*+ Sacado.CPFCNPJ +*/+ Cedente.CPFCNPJ + "</FONT> ");
            sb.AppendLine("		</TD> ");
            sb.AppendLine("		<TD valign=\"top\"><FONT class=ct>(-) Desconto / Abatimento</FONT></TD> ");
            sb.AppendLine("		<TD valign=\"top\"><FONT class=ct>(-) Outras Dedu&ccedil;&otilde;es</FONT></TD> ");
            sb.AppendLine("   		<TD valign=\"top\"><FONT class=ct>(+) Mora / Multa</FONT></TD> ");
            sb.AppendLine("   		<TD bgcolor=\"#CCCCCC\" valign=\"top\"><FONT class=ct>(=) Valor Cobrado</FONT></TD> ");
            sb.AppendLine("	</TR> ");
            sb.AppendLine("	<TR> ");
            sb.AppendLine("		<TD COLSPAN=5 valign=top align=left> ");
            sb.AppendLine("			<FONT class=ct>Observações:</FONT><br> ");
            sb.AppendLine("			<TABLE ALIGN=RIGHT CELLSPACING=0 CELLPADDING=0 BORDER=0> ");
            sb.AppendLine("				<TR> ");
            sb.AppendLine("					<TD valign=top align=left>" + Observacoes + "<br></TD> ");
            sb.AppendLine("				</TR> ");
            sb.AppendLine("			</TABLE> ");
            sb.AppendLine("	 	</TD> ");
            sb.AppendLine("	</TR> ");
            sb.AppendLine("</TABLE> ");
            sb.AppendLine("<TABLE WIDTH=\"520\" CELLSPACING=0 CELLPADDING=0 BORDER=0> ");
            sb.AppendLine("	<TR> ");
            sb.AppendLine("		<TD align=right><FONT class=ct>Autentica&ccedil;&atilde;o Mec&acirc;nica</FONT><BR><BR></TD> ");
            sb.AppendLine("	</TR> ");
            sb.AppendLine("</TABLE> ");
            sb.AppendLine("<img src=\"" + pathImagens + "corte.gif\" border=0 width=\"520\"><br><br> ");
            sb.AppendLine("<TABLE WIDTH=\"520\" BORDER=0 CELLSPACING=0 CELLPADDING=0> ");
            sb.AppendLine("	<tr> ");
            sb.AppendLine("		<td><img src=\"" + fnLogo + "\" border=0></td> ");
            sb.AppendLine("		<td valign=\"bottom\" align=\"right\"><font class=\"bc\" size=\"4\">" + Utils.FormatCode(_ibanco.Codigo.ToString(), 3) + "-" + _ibanco.Digito + "</font></td> ");
            sb.AppendLine("		<td colspan=3 align=right valign=\"bottom\"><nobr><span class='ld'><p align=\"right\">&nbsp;<font size=\"2\">" + Boleto.CodigoBarra.LinhaDigitavel + "</font></span></nobr></td> ");
            sb.AppendLine("	</tr> ");
            sb.AppendLine("</TABLE> ");
            sb.AppendLine("<TABLE WIDTH=\"520\" BORDER=1 CELLSPACING=0 CELLPADDING=1> ");
            sb.AppendLine("	<TR> ");
            sb.AppendLine("		<TD COLSPAN=6> ");
            sb.AppendLine("			<TABLE WIDTH=100% BORDER=0 CELLSPACING=0 CELLPADDING=0> ");
            sb.AppendLine("				<TR> ");
            sb.AppendLine("					<TD colspan=\"2\" align=left><FONT class=ct>Local de Pagamento</FONT><BR></TD> ");
            sb.AppendLine("				</TR> ");
            sb.AppendLine("				<TR> ");
            sb.AppendLine("					<td>&nbsp;&nbsp;</td> ");
            sb.AppendLine("					<TD><FONT class=cn>" + Boleto.LocalPagamento + "</FONT></TD> ");
            sb.AppendLine("				</TR> ");
            sb.AppendLine("			</TABLE> ");
            sb.AppendLine("		</TD> ");
            sb.AppendLine("		<TD bgcolor=\"#CCCCCC\" valign=\"top\" colspan=2> ");
            sb.AppendLine("			<TABLE WIDTH=100% BORDER=0 CELLSPACING=0 CELLPADDING=0> ");
            sb.AppendLine("				<TR> ");
            sb.AppendLine("					<TD align=left><FONT class=ct>Vencimento</FONT></TD> ");
            sb.AppendLine("				</TR> ");
            sb.AppendLine("				<TR> ");
            sb.AppendLine("					<TD align=right><FONT class=cp>" + Boleto.DataVencimento.ToString("dd/MM/yyyy") + "</FONT></TD> ");
            sb.AppendLine("				</TR> ");
            sb.AppendLine("			</TABLE> ");
            sb.AppendLine("		</TD> ");
            sb.AppendLine("	</TR> ");
            sb.AppendLine("	<TR> ");
            sb.AppendLine("		<TD COLSPAN=6> ");
            sb.AppendLine("			<FONT class=ct>Cedente</FONT><BR> ");
            sb.AppendLine("			<FONT class=cn>&nbsp;" + Cedente.Nome + "</FONT> ");
            sb.AppendLine("		</TD> ");
            sb.AppendLine("		<TD colspan=2> ");
            sb.AppendLine("			<TABLE WIDTH=100% BORDER=0 CELLSPACING=0 CELLPADDING=0> ");
            sb.AppendLine("				<TR> ");
            sb.AppendLine("					<TD align=left><FONT class=ct>Ag&ecirc;ncia / C&oacute;digo Cedente</FONT></TD> ");
            sb.AppendLine("				</TR> ");
            sb.AppendLine("				<TR> ");

            if (Cedente.Codigo != "0" && Cedente.Codigo != string.Empty)
                sb.AppendLine("				<TD align=right><FONT class=cn>" + Cedente.ContaBancaria.Agencia + "-" + Cedente.ContaBancaria.DigitoAgencia + "&nbsp;/&nbsp;" + Cedente.Codigo + "</FONT></TD> ");
            else
                sb.AppendLine("				<TD align=right><FONT class=cn>" + Cedente.ContaBancaria.Agencia + "-" + Cedente.ContaBancaria.DigitoAgencia + "&nbsp;/&nbsp;" + Cedente.ContaBancaria.Conta + "-" + Cedente.ContaBancaria.DigitoConta + "</FONT></TD> ");

            sb.AppendLine("				</TR> ");
            sb.AppendLine("			</TABLE> ");
            sb.AppendLine("		</TD> ");
            sb.AppendLine("	</TR> ");
            sb.AppendLine("	<TR> ");
            sb.AppendLine("		<TD valign=top> ");
            sb.AppendLine("			<FONT class=ct>Data Documento</FONT><BR> ");
            sb.AppendLine("			<FONT class=cn>&nbsp;" + Boleto.DataDocumento.ToString("dd/MM/yyyy") + "</FONT> ");
            sb.AppendLine("		</TD> ");
            sb.AppendLine("		<TD valign=top colspan=\"2\"> ");
            sb.AppendLine("			<FONT class=ct>Número Documento</FONT><BR> ");
            sb.AppendLine("			<FONT class=cn>&nbsp;" + Boleto.NumeroDocumento + "</FONT> ");
            sb.AppendLine("		</TD> ");
            sb.AppendLine("		<TD valign=top> ");
            sb.AppendLine("			<FONT class=ct>Espécie Documento</FONT><BR> ");
            sb.AppendLine("			<FONT class=cn>&nbsp;" + "DM" + "</FONT> ");
            sb.AppendLine("		</TD> ");
            sb.AppendLine("		<TD valign=top> ");
            sb.AppendLine("			<FONT class=ct>Aceite</FONT><BR> ");
            sb.AppendLine("			<FONT class=cn>&nbsp;" + Boleto.Aceite + "</FONT> ");
            sb.AppendLine("		</TD> ");
            sb.AppendLine("		<TD valign=top><FONT class=ct>Data Proc.</FONT><BR><FONT class=cn>&nbsp;" + Boleto.DataProcessamento + "</TD> ");
            sb.AppendLine("		<TD valign=\"top\" colspan=2> ");
            sb.AppendLine("			<TABLE WIDTH=100% BORDER=0 CELLSPACING=0 CELLPADDING=0> ");
            sb.AppendLine("				<TR> ");
            sb.AppendLine("					<TD align=left colspan=\"2\"><FONT class=ct>Nosso Número</FONT></TD> ");
            sb.AppendLine("				</TR> ");
            sb.AppendLine("				<TR> ");
            sb.AppendLine("					<TD align=right><FONT class=cn>" + Boleto.NossoNumero + "</FONT></TD> ");
            sb.AppendLine("				</TR> ");
            sb.AppendLine("			</TABLE> ");
            sb.AppendLine("		</TD> ");
            sb.AppendLine("	</TR> ");
            sb.AppendLine("	<TR> ");
            sb.AppendLine("		<TD valign=top><FONT class=ct>Uso Banco</FONT></TD> ");
            sb.AppendLine("		<TD valign=top> ");
            sb.AppendLine("			<FONT class=ct>Carteira</FONT><BR> ");
            sb.AppendLine("			<FONT class=cn>&nbsp;" + Boleto.Carteira + "</FONT> ");
            sb.AppendLine("		</TD> ");
            sb.AppendLine("		<TD valign=top> ");
            sb.AppendLine("			<FONT class=ct>Espécie</FONT><BR> ");
            sb.AppendLine("			<FONT class=cn>&nbsp;" + Boleto.Especie + "</FONT> ");
            sb.AppendLine("		</TD> ");
            sb.AppendLine("		<TD valign=top colspan=\"2\"> ");
            sb.AppendLine("			<FONT class=ct>Quantidade</FONT><BR> ");
            sb.AppendLine("			<FONT class=cn>&nbsp;</FONT> ");
            sb.AppendLine("		</TD> ");
            sb.AppendLine("		<TD valign=top> ");
            sb.AppendLine("			<FONT class=ct>Valor</FONT><BR> ");
            sb.AppendLine("			<FONT class=cn>&nbsp;</FONT> ");
            sb.AppendLine("		</TD> ");
            sb.AppendLine("		<TD bgcolor=\"#CCCCCC\" valign=\"top\"  colspan=2> ");
            sb.AppendLine("			<TABLE WIDTH=100% BORDER=0 CELLSPACING=0 CELLPADDING=0> ");
            sb.AppendLine("				<TR> ");
            sb.AppendLine("					<TD align=left><FONT class=ct>Valor do Documento</FONT></TD> ");
            sb.AppendLine("				</TR> ");
            sb.AppendLine("				<TR> ");
            sb.AppendLine("					<TD align=right><FONT class=cp>" + (Boleto.ValorBoleto == 0 ? "" : Boleto.ValorBoleto.ToString("R$ ##,##0.00")) + "</FONT></TD> ");
            sb.AppendLine("				</TR> ");
            sb.AppendLine("			</TABLE> ");
            sb.AppendLine("		</TD> ");
            sb.AppendLine("	</TR> ");
            sb.AppendLine("	<TR> ");
            sb.AppendLine("		<TH COLSPAN=6 ROWSPAN=4 valign=top align=LEFT> ");
            sb.AppendLine("			<FONT class=ct>Instru&ccedil;&otilde;es (Todas as informações deste bloqueto são de exclusiva responsabilidade do cedente)&nbsp;</FONT><BR> ");
            sb.AppendLine("			<TABLE CELLSPACING=0 CELLPADDING=0 BORDER=0> ");
            sb.AppendLine("				<TR> ");
            sb.AppendLine("					<TD valign=top align=left><br><FONT class=cn>" + Instrucoes + "</FONT></TD>");
            sb.AppendLine("				</TR> ");
            sb.AppendLine("			</TABLE> ");
            sb.AppendLine("		</TH> ");
            sb.AppendLine("		<TD  colspan=2> ");
            sb.AppendLine("			<FONT class=ct>(-) Desconto / Abatimento</FONT><BR> ");
            sb.AppendLine("			<FONT class=cn>&nbsp;<br></FONT> ");
            sb.AppendLine("		</TD> ");
            sb.AppendLine("	</TR> ");
            sb.AppendLine("	<TR> ");
            sb.AppendLine("		<TD  colspan=2> ");
            sb.AppendLine("			<FONT class=ct>(-) Outras Deduções</FONT><BR> ");
            sb.AppendLine("			<FONT class=cn>&nbsp;<br></FONT> ");
            sb.AppendLine("		</TD> ");
            sb.AppendLine("	</TR> ");
            sb.AppendLine("	<TR> ");
            sb.AppendLine("		<TD colspan=2> ");
            sb.AppendLine("			<FONT class=ct>(+) Mora / Multa</FONT><BR> ");
            sb.AppendLine("			<FONT class=cn>&nbsp;<br></FONT> ");
            sb.AppendLine("		</TD> ");
            sb.AppendLine("	</TR> ");
            sb.AppendLine("	<TR> ");
            sb.AppendLine("		<TD colspan=2 bgcolor=\"#CCCCCC\"> ");
            sb.AppendLine("			<FONT class=ct>(=) Valor Cobrado</FONT><BR> ");
            sb.AppendLine("			<FONT class=cn>&nbsp;<br></FONT> ");
            sb.AppendLine("		</TD> ");
            sb.AppendLine("	</TR> ");
            sb.AppendLine("</TABLE> ");
            sb.AppendLine("<TABLE WIDTH=520 BORDER=1 CELLSPACING=0 CELLPADDING=1> ");

            sb.AppendLine("                        <tr>");
            sb.AppendLine("                            <td valign='top' align='left'>");
            sb.AppendLine("                                <font class='cn' size='1' face='Tahoma'>");
            sb.AppendLine("                                </font> Sacado:");
            sb.AppendLine("                                <br/>");
            sb.AppendLine("                                <font class='cn' size='1' face='Tahoma'>");
            sb.AppendLine("                                    " + Sacado.Nome + " - " + Sacado.CPFCNPJ + "<br/>");
            sb.AppendLine("                                    " + Sacado.Endereco.End + " - " + Sacado.Endereco.Bairro + " - " + Sacado.Endereco.Cidade + " / " + Sacado.Endereco.UF + "<br/>");
            sb.AppendLine("                                    " + Sacado.Email + "");
            sb.AppendLine("                                </font>");
            sb.AppendLine("                                <br/>");
            sb.AppendLine("                                <font class='cn' size='1' face='Tahoma'>");
            sb.AppendLine("                                </font> Sacador/Avalista:");
            sb.AppendLine("                                <br/></td>");

            sb.AppendLine("                        </tr>");
            sb.AppendLine("</TABLE><BR></p> ");
            sb.AppendLine("<TABLE WIDTH=\"520\" CELLSPACING=0 CELLPADDING=0 BORDER=0> ");
            sb.AppendLine("	<TR> ");
            sb.AppendLine("		<TD class=ct align=right> ");
            sb.AppendLine("			<div align=\"right\">Autenticação Mecânica - <b class=\"cp\">Ficha de Compensação</b></div> ");
            sb.AppendLine("		</TD> ");
            sb.AppendLine("	</TR> ");
            sb.AppendLine("	<TR> ");
            sb.AppendLine("		<TD align=left>" + strCodigoBarras + "</TD> ");
            sb.AppendLine("	</TR> ");
            sb.AppendLine("</TABLE><BR></p> ");

            return sb.ToString();
        }
        #endregion

        public static string UrlLogo(int banco)
        {
            var page = System.Web.HttpContext.Current.CurrentHandler as Page;
            return page.ClientScript.GetWebResourceUrl(typeof(BoletoBancario), "BoletoNet.Imagens." + Utils.FormatCode(banco.ToString(), 3) + ".jpg");
        }

        public System.Drawing.Image GeraImagemCodigoBarras(Boleto boleto)
        {
            C2of5i cb = new C2of5i(boleto.CodigoBarra.Codigo, 1, 50, boleto.CodigoBarra.Codigo.Length);
            return cb.ToBitmap();
        }

    }
}