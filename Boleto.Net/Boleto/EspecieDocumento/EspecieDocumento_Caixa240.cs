using System;
using System.Collections.Generic;
using System.Text;

namespace BoletoNet
{
    #region Enumerador

    public enum EnumEspecieDocumento_Caixa240
    {
        Cheque = 1, //CH – CHEQUE
        DuplicataMercantil = 2, //DM – DUPLICATA MERCANTIL
        DuplicataMercantilIndicacao = 3, //DMI – DUPLICATA MERCANTIL P/ INDICAÇÃO
        DuplicataServico = 4, //DS –  DUPLICATA DE SERVIÇO
        DuplicataServicoIndicacao = 5, //DSI –  DUPLICATA DE SERVIÇO P/ INDICAÇÃO
        DuplicataRural = 6, //DR – DUPLICATA RURAL
        LetraCambio = 7, //LC – LETRA DE CAMBIO
        NotaCreditoComercial = 8, //NCC – NOTA DE CRÉDITO COMERCIAL
        NotaCreditoExportacao = 9, //NCE – NOTA DE CRÉDITO A EXPORTAÇÃO
        NotaCreditoIndustrial = 10, //NCI – NOTA DE CRÉDITO INDUSTRIAL
        NotaCreditoRural = 11, //NCR – NOTA DE CRÉDITO RURAL
        NotaPromissoria = 12, //NP – NOTA PROMISSÓRIA
        NotaPromissoriaRural = 13, //NPR –NOTA PROMISSÓRIA RURAL
        TriplicataMercantil = 14, //TM – TRIPLICATA MERCANTIL
        TriplicataServico = 15, //TS –  TRIPLICATA DE SERVIÇO
        NotaSeguro = 16, //NS – NOTA DE SEGURO
        Recibo = 17, //RC – RECIBO
        Fatura = 18, //FAT – FATURA
        NotaDebito = 19, //ND –  NOTA DE DÉBITO
        ApoliceSeguro = 20, //AP –  APÓLICE DE SEGURO
        MensalidadeEscolar = 21, //ME – MENSALIDADE ESCOLAR
        ParcelaConsorcio = 22, //PC –  PARCELA DE CONSÓRCIO
        Outros = 23 //OUTROS
    }

    #endregion

    public class EspecieDocumento_Caixa240 : AbstractEspecieDocumento, IEspecieDocumento
    {
        #region Construtores

        public EspecieDocumento_Caixa240()
        {
            try
            {
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }

        public EspecieDocumento_Caixa240(string codigo)
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

        public string getCodigoEspecieByEnum(EnumEspecieDocumento_Caixa240 especie)
        {
            switch (especie)
            {
                case EnumEspecieDocumento_Caixa240.Cheque: return "1";
                case EnumEspecieDocumento_Caixa240.DuplicataMercantil: return "2";
                case EnumEspecieDocumento_Caixa240.DuplicataMercantilIndicacao: return "3";
                case EnumEspecieDocumento_Caixa240.DuplicataServico: return "4";
                case EnumEspecieDocumento_Caixa240.DuplicataServicoIndicacao: return "5";
                case EnumEspecieDocumento_Caixa240.DuplicataRural: return "6";
                case EnumEspecieDocumento_Caixa240.LetraCambio: return "7";
                case EnumEspecieDocumento_Caixa240.NotaCreditoComercial: return "8";
                case EnumEspecieDocumento_Caixa240.NotaCreditoExportacao: return "9";
                case EnumEspecieDocumento_Caixa240.NotaCreditoIndustrial: return "10";
                case EnumEspecieDocumento_Caixa240.NotaCreditoRural: return "11";
                case EnumEspecieDocumento_Caixa240.NotaPromissoria: return "12";
                case EnumEspecieDocumento_Caixa240.NotaPromissoriaRural: return "13";
                case EnumEspecieDocumento_Caixa240.TriplicataMercantil: return "14";
                case EnumEspecieDocumento_Caixa240.TriplicataServico: return "15";
                case EnumEspecieDocumento_Caixa240.NotaSeguro: return "16";
                case EnumEspecieDocumento_Caixa240.Recibo: return "17";
                case EnumEspecieDocumento_Caixa240.Fatura: return "18";
                case EnumEspecieDocumento_Caixa240.NotaDebito: return "19";
                case EnumEspecieDocumento_Caixa240.ApoliceSeguro: return "20";
                case EnumEspecieDocumento_Caixa240.MensalidadeEscolar: return "21";
                case EnumEspecieDocumento_Caixa240.ParcelaConsorcio: return "22";
                case EnumEspecieDocumento_Caixa240.Outros: return "23";
                default: return "2";

            }
        }

        public EnumEspecieDocumento_Caixa240 getEnumEspecieByCodigo(string codigo)
        {
            switch (codigo)
            {
                case "1": return EnumEspecieDocumento_Caixa240.Cheque;
                case "2": return EnumEspecieDocumento_Caixa240.DuplicataMercantil;
                case "3": return EnumEspecieDocumento_Caixa240.DuplicataMercantilIndicacao;
                case "4": return EnumEspecieDocumento_Caixa240.DuplicataServico;
                case "5": return EnumEspecieDocumento_Caixa240.DuplicataServicoIndicacao;
                case "6": return EnumEspecieDocumento_Caixa240.DuplicataRural;
                case "7": return EnumEspecieDocumento_Caixa240.LetraCambio;
                case "8": return EnumEspecieDocumento_Caixa240.NotaCreditoComercial;
                case "9": return EnumEspecieDocumento_Caixa240.NotaCreditoExportacao;
                case "10": return EnumEspecieDocumento_Caixa240.NotaCreditoIndustrial;
                case "11": return EnumEspecieDocumento_Caixa240.NotaCreditoRural;
                case "12": return EnumEspecieDocumento_Caixa240.NotaPromissoria;
                case "13": return EnumEspecieDocumento_Caixa240.NotaPromissoriaRural;
                case "14": return EnumEspecieDocumento_Caixa240.TriplicataMercantil;
                case "15": return EnumEspecieDocumento_Caixa240.TriplicataServico;
                case "16": return EnumEspecieDocumento_Caixa240.NotaSeguro;
                case "17": return EnumEspecieDocumento_Caixa240.Recibo;
                case "18": return EnumEspecieDocumento_Caixa240.Fatura;
                case "19": return EnumEspecieDocumento_Caixa240.NotaDebito;
                case "20": return EnumEspecieDocumento_Caixa240.ApoliceSeguro;
                case "21": return EnumEspecieDocumento_Caixa240.MensalidadeEscolar;
                case "22": return EnumEspecieDocumento_Caixa240.ParcelaConsorcio;
                case "23": return EnumEspecieDocumento_Caixa240.Outros;
                default: return EnumEspecieDocumento_Caixa240.DuplicataMercantil;
            }
        }

        private void carregar(string idCodigo)
        {
            try
            {
                this.Banco = new Banco_Caixa();

                switch (getEnumEspecieByCodigo(idCodigo))
                {
                    case EnumEspecieDocumento_Caixa240.Cheque:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Caixa240.Cheque);
                        this.Especie = "CHEQUE";
                        this.Sigla = "CH";
                        break;
                    case EnumEspecieDocumento_Caixa240.DuplicataMercantil:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Caixa240.DuplicataMercantil);
                        this.Especie = "DUPLICATA MERCANTIL";
                        this.Sigla = "DM";
                        break;
                    case EnumEspecieDocumento_Caixa240.DuplicataMercantilIndicacao:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Caixa240.DuplicataMercantilIndicacao);
                        this.Especie = "DUPLICATA MERCANTIL P/ INDICAÇÃO";
                        this.Sigla = "DMI";
                        break;
                    case EnumEspecieDocumento_Caixa240.DuplicataServico:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Caixa240.DuplicataServico);
                        this.Especie = "DUPLICATA DE SERVIÇO";
                        this.Sigla = "DS";
                        break;
                    case EnumEspecieDocumento_Caixa240.DuplicataServicoIndicacao:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Caixa240.DuplicataServicoIndicacao);
                        this.Especie = "DUPLICATA DE SERVIÇO P/ INDICAÇÃO";
                        this.Sigla = "DSI";
                        break;
                    case EnumEspecieDocumento_Caixa240.DuplicataRural:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Caixa240.DuplicataRural);
                        this.Especie = "DUPLICATA RURAL";
                        this.Sigla = "DR";
                        break;
                    case EnumEspecieDocumento_Caixa240.LetraCambio:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Caixa240.LetraCambio);
                        this.Especie = "LETRA DE CAMBIO";
                        this.Sigla = "LC";
                        break;
                    case EnumEspecieDocumento_Caixa240.NotaCreditoComercial:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Caixa240.NotaCreditoComercial);
                        this.Especie = "NOTA DE CRÉDITO COMERCIAL";
                        this.Sigla = "NCC";
                        break;
                    case EnumEspecieDocumento_Caixa240.NotaCreditoExportacao:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Caixa240.NotaCreditoExportacao);
                        this.Especie = "NOTA DE CRÉDITO A EXPORTAÇÃO";
                        this.Sigla = "NCE";
                        break;
                    case EnumEspecieDocumento_Caixa240.NotaCreditoIndustrial:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Caixa240.NotaCreditoIndustrial);
                        this.Especie = "NOTA DE CRÉDITO INDUSTRIAL";
                        this.Sigla = "NCI";
                        break;
                    case EnumEspecieDocumento_Caixa240.NotaCreditoRural:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Caixa240.NotaCreditoRural);
                        this.Especie = "NOTA DE CRÉDITO RURAL";
                        this.Sigla = "NCR";
                        break;
                    case EnumEspecieDocumento_Caixa240.NotaPromissoria:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Caixa240.NotaPromissoria);
                        this.Especie = "NOTA PROMISSÓRIA";
                        this.Sigla = "NP";
                        break;
                    case EnumEspecieDocumento_Caixa240.NotaPromissoriaRural:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Caixa240.NotaPromissoriaRural);
                        this.Especie = "NOTA PROMISSÓRIA RURAL";
                        this.Sigla = "NPR";
                        break;
                    case EnumEspecieDocumento_Caixa240.TriplicataMercantil:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Caixa240.TriplicataMercantil);
                        this.Especie = "TRIPLICATA MERCANTIL";
                        this.Sigla = "TM";
                        break;
                    case EnumEspecieDocumento_Caixa240.TriplicataServico:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Caixa240.TriplicataServico);
                        this.Especie = "TRIPLICATA DE SERVIÇO";
                        this.Sigla = "TS";
                        break;
                    case EnumEspecieDocumento_Caixa240.NotaSeguro:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Caixa240.NotaSeguro);
                        this.Especie = "NOTA DE SEGURO";
                        this.Sigla = "NS";
                        break;
                    case EnumEspecieDocumento_Caixa240.Recibo:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Caixa240.Recibo);
                        this.Especie = "RECIBO";
                        this.Sigla = "RC";
                        break;
                    case EnumEspecieDocumento_Caixa240.Fatura:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Caixa240.Fatura);
                        this.Especie = "FATURA";
                        this.Sigla = "FAT";
                        break;
                    case EnumEspecieDocumento_Caixa240.NotaDebito:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Caixa240.NotaDebito);
                        this.Especie = "NOTA DE DÉBITO";
                        this.Sigla = "ND";
                        break;
                    case EnumEspecieDocumento_Caixa240.ApoliceSeguro:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Caixa240.ApoliceSeguro);
                        this.Especie = "APÓLICE DE SEGURO";
                        this.Sigla = "AP";
                        break;
                    case EnumEspecieDocumento_Caixa240.MensalidadeEscolar:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Caixa240.MensalidadeEscolar);
                        this.Especie = "MENSALIDADE ESCOLAR";
                        this.Sigla = "ME";
                        break;
                    case EnumEspecieDocumento_Caixa240.ParcelaConsorcio:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Caixa240.ParcelaConsorcio);
                        this.Especie = "PARCELA DE CONSÓRCIO";
                        this.Sigla = "PC";
                        break;
                    case EnumEspecieDocumento_Caixa240.Outros:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Caixa240.Outros);
                        this.Especie = "OUTROS";
                        this.Sigla = "OUTROS";
                        break;
                    default:
                        this.Codigo = "0";
                        this.Especie = "";
                        break;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }

        public static EspeciesDocumento CarregaTodas()
        {
            try
            {
                EspeciesDocumento alEspeciesDocumento = new EspeciesDocumento();
                EspecieDocumento_Caixa240 ed = new EspecieDocumento_Caixa240();

                alEspeciesDocumento.Add(new EspecieDocumento_Caixa240(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_Caixa240.Cheque)));
                alEspeciesDocumento.Add(new EspecieDocumento_Caixa240(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_Caixa240.DuplicataMercantil)));
                alEspeciesDocumento.Add(new EspecieDocumento_Caixa240(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_Caixa240.DuplicataMercantilIndicacao)));
                alEspeciesDocumento.Add(new EspecieDocumento_Caixa240(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_Caixa240.DuplicataServico)));
                alEspeciesDocumento.Add(new EspecieDocumento_Caixa240(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_Caixa240.DuplicataServicoIndicacao)));
                alEspeciesDocumento.Add(new EspecieDocumento_Caixa240(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_Caixa240.DuplicataRural)));
                alEspeciesDocumento.Add(new EspecieDocumento_Caixa240(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_Caixa240.LetraCambio)));
                alEspeciesDocumento.Add(new EspecieDocumento_Caixa240(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_Caixa240.NotaCreditoComercial)));
                alEspeciesDocumento.Add(new EspecieDocumento_Caixa240(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_Caixa240.NotaCreditoExportacao)));
                alEspeciesDocumento.Add(new EspecieDocumento_Caixa240(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_Caixa240.NotaCreditoIndustrial)));
                alEspeciesDocumento.Add(new EspecieDocumento_Caixa240(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_Caixa240.NotaCreditoRural)));
                alEspeciesDocumento.Add(new EspecieDocumento_Caixa240(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_Caixa240.NotaPromissoria)));
                alEspeciesDocumento.Add(new EspecieDocumento_Caixa240(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_Caixa240.NotaPromissoriaRural)));
                alEspeciesDocumento.Add(new EspecieDocumento_Caixa240(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_Caixa240.TriplicataMercantil)));
                alEspeciesDocumento.Add(new EspecieDocumento_Caixa240(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_Caixa240.TriplicataServico)));
                alEspeciesDocumento.Add(new EspecieDocumento_Caixa240(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_Caixa240.NotaSeguro)));
                alEspeciesDocumento.Add(new EspecieDocumento_Caixa240(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_Caixa240.Recibo)));
                alEspeciesDocumento.Add(new EspecieDocumento_Caixa240(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_Caixa240.Fatura)));
                alEspeciesDocumento.Add(new EspecieDocumento_Caixa240(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_Caixa240.NotaDebito)));
                alEspeciesDocumento.Add(new EspecieDocumento_Caixa240(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_Caixa240.ApoliceSeguro)));
                alEspeciesDocumento.Add(new EspecieDocumento_Caixa240(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_Caixa240.MensalidadeEscolar)));
                alEspeciesDocumento.Add(new EspecieDocumento_Caixa240(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_Caixa240.ParcelaConsorcio)));
                alEspeciesDocumento.Add(new EspecieDocumento_Caixa240(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_Caixa240.Outros)));

                return alEspeciesDocumento;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao listar objetos", ex);
            }
        }

        #endregion
    }
}
