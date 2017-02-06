using System;
using System.Collections.Generic;
using System.Text;

namespace BoletoNet
{
    #region Enumerado

    public enum EnumEspecieDocumento_Bradesco240
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
    }

    #endregion

    public class EspecieDocumento_Bradesco240 : AbstractEspecieDocumento, IEspecieDocumento
    {
        #region Construtores

        public EspecieDocumento_Bradesco240()
        {
            try
            {
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }

        public EspecieDocumento_Bradesco240(string codigo)
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

        public string getCodigoEspecieByEnum(EnumEspecieDocumento_Bradesco240 especie)
        {
            switch (especie)
            {
                case EnumEspecieDocumento_Bradesco240.Cheque: return "1";
                case EnumEspecieDocumento_Bradesco240.DuplicataMercantil: return "2";
                case EnumEspecieDocumento_Bradesco240.DuplicataMercantilIndicacao: return "3";
                case EnumEspecieDocumento_Bradesco240.DuplicataServico: return "4";
                case EnumEspecieDocumento_Bradesco240.DuplicataServicoIndicacao: return "5";
                case EnumEspecieDocumento_Bradesco240.DuplicataRural: return "6";
                case EnumEspecieDocumento_Bradesco240.LetraCambio: return "7";
                case EnumEspecieDocumento_Bradesco240.NotaCreditoComercial: return "8";
                case EnumEspecieDocumento_Bradesco240.NotaCreditoExportacao: return "9";
                case EnumEspecieDocumento_Bradesco240.NotaCreditoIndustrial: return "10";
                case EnumEspecieDocumento_Bradesco240.NotaCreditoRural: return "11";
                case EnumEspecieDocumento_Bradesco240.NotaPromissoria: return "12";
                case EnumEspecieDocumento_Bradesco240.NotaPromissoriaRural: return "13";
                case EnumEspecieDocumento_Bradesco240.TriplicataMercantil: return "14";
                case EnumEspecieDocumento_Bradesco240.TriplicataServico: return "15";
                case EnumEspecieDocumento_Bradesco240.NotaSeguro: return "16";
                case EnumEspecieDocumento_Bradesco240.Recibo: return "17";
                case EnumEspecieDocumento_Bradesco240.Fatura: return "18";
                case EnumEspecieDocumento_Bradesco240.NotaDebito: return "19";
                default: return "2";

            }
        }

        public EnumEspecieDocumento_Bradesco240 getEnumEspecieByCodigo(string codigo)
        {
            switch (codigo)
            {
                case "1": return EnumEspecieDocumento_Bradesco240.Cheque;
                case "2": return EnumEspecieDocumento_Bradesco240.DuplicataMercantil;
                case "3": return EnumEspecieDocumento_Bradesco240.DuplicataMercantilIndicacao;
                case "4": return EnumEspecieDocumento_Bradesco240.DuplicataServico;
                case "5": return EnumEspecieDocumento_Bradesco240.DuplicataServicoIndicacao;
                case "6": return EnumEspecieDocumento_Bradesco240.DuplicataRural;
                case "7": return EnumEspecieDocumento_Bradesco240.LetraCambio;
                case "8": return EnumEspecieDocumento_Bradesco240.NotaCreditoComercial;
                case "9": return EnumEspecieDocumento_Bradesco240.NotaCreditoExportacao;
                case "10": return EnumEspecieDocumento_Bradesco240.NotaCreditoIndustrial;
                case "11": return EnumEspecieDocumento_Bradesco240.NotaCreditoRural;
                case "12": return EnumEspecieDocumento_Bradesco240.NotaPromissoria;
                case "13": return EnumEspecieDocumento_Bradesco240.NotaPromissoriaRural;
                case "14": return EnumEspecieDocumento_Bradesco240.TriplicataMercantil;
                case "15": return EnumEspecieDocumento_Bradesco240.TriplicataServico;
                case "16": return EnumEspecieDocumento_Bradesco240.NotaSeguro;
                case "17": return EnumEspecieDocumento_Bradesco240.Recibo;
                case "18": return EnumEspecieDocumento_Bradesco240.Fatura;
                case "19": return EnumEspecieDocumento_Bradesco240.NotaDebito;
                default: return EnumEspecieDocumento_Bradesco240.DuplicataMercantil;
            }
        }

        private void carregar(string idCodigo)
        {
            try
            {
                this.Banco = new Banco_Bradesco();

                switch (getEnumEspecieByCodigo(idCodigo))
                {
                    case EnumEspecieDocumento_Bradesco240.Cheque:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Bradesco240.Cheque);
                        this.Especie = "CHEQUE";
                        this.Sigla = "CH";
                        break;
                    case EnumEspecieDocumento_Bradesco240.DuplicataMercantil:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Bradesco240.DuplicataMercantil);
                        this.Especie = "DUPLICATA MERCANTIL";
                        this.Sigla = "DM";
                        break;
                    case EnumEspecieDocumento_Bradesco240.DuplicataMercantilIndicacao:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Bradesco240.DuplicataMercantilIndicacao);
                        this.Especie = "DUPLICATA MERCANTIL P/ INDICAÇÃO";
                        this.Sigla = "DMI";
                        break;
                    case EnumEspecieDocumento_Bradesco240.DuplicataServico:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Bradesco240.DuplicataServico);
                        this.Especie = "DUPLICATA DE SERVIÇO";
                        this.Sigla = "DS";
                        break;
                    case EnumEspecieDocumento_Bradesco240.DuplicataServicoIndicacao:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Bradesco240.DuplicataServicoIndicacao);
                        this.Especie = "DUPLICATA DE SERVIÇO P/ INDICAÇÃO";
                        this.Sigla = "DSI";
                        break;
                    case EnumEspecieDocumento_Bradesco240.DuplicataRural:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Bradesco240.DuplicataRural);
                        this.Especie = "DUPLICATA RURAL";
                        this.Sigla = "DR";
                        break;
                    case EnumEspecieDocumento_Bradesco240.LetraCambio:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Bradesco240.LetraCambio);
                        this.Especie = "LETRA DE CAMBIO";
                        this.Sigla = "LC";
                        break;
                    case EnumEspecieDocumento_Bradesco240.NotaCreditoComercial:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Bradesco240.NotaCreditoComercial);
                        this.Especie = "NOTA DE CRÉDITO COMERCIAL";
                        this.Sigla = "NCC";
                        break;
                    case EnumEspecieDocumento_Bradesco240.NotaCreditoExportacao:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Bradesco240.NotaCreditoExportacao);
                        this.Especie = "NOTA DE CRÉDITO A EXPORTAÇÃO";
                        this.Sigla = "NCE";
                        break;
                    case EnumEspecieDocumento_Bradesco240.NotaCreditoIndustrial:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Bradesco240.NotaCreditoIndustrial);
                        this.Especie = "NOTA DE CRÉDITO INDUSTRIAL";
                        this.Sigla = "NCI";
                        break;
                    case EnumEspecieDocumento_Bradesco240.NotaCreditoRural:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Bradesco240.NotaCreditoRural);
                        this.Especie = "NOTA DE CRÉDITO RURAL";
                        this.Sigla = "NCR";
                        break;
                    case EnumEspecieDocumento_Bradesco240.NotaPromissoria:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Bradesco240.NotaPromissoria);
                        this.Especie = "NOTA PROMISSÓRIA";
                        this.Sigla = "NP";
                        break;
                    case EnumEspecieDocumento_Bradesco240.NotaPromissoriaRural:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Bradesco240.NotaPromissoriaRural);
                        this.Especie = "NOTA PROMISSÓRIA RURAL";
                        this.Sigla = "NPR";
                        break;
                    case EnumEspecieDocumento_Bradesco240.TriplicataMercantil:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Bradesco240.TriplicataMercantil);
                        this.Especie = "TRIPLICATA MERCANTIL";
                        this.Sigla = "TM";
                        break;
                    case EnumEspecieDocumento_Bradesco240.TriplicataServico:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Bradesco240.TriplicataServico);
                        this.Especie = "TRIPLICATA DE SERVIÇO";
                        this.Sigla = "TS";
                        break;
                    case EnumEspecieDocumento_Bradesco240.NotaSeguro:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Bradesco240.NotaSeguro);
                        this.Especie = "NOTA DE SEGURO";
                        this.Sigla = "NS";
                        break;
                    case EnumEspecieDocumento_Bradesco240.Recibo:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Bradesco240.Recibo);
                        this.Especie = "RECIBO";
                        this.Sigla = "RC";
                        break;
                    case EnumEspecieDocumento_Bradesco240.Fatura:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Bradesco240.Fatura);
                        this.Especie = "FATURA";
                        this.Sigla = "FAT";
                        break;
                    case EnumEspecieDocumento_Bradesco240.NotaDebito:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Bradesco240.NotaDebito);
                        this.Especie = "NOTA DE DÉBITO";
                        this.Sigla = "ND";
                        break;
                    default:
                        this.Codigo = "0";
                        this.Especie = "";
                        this.Sigla = "";
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

                EspecieDocumento_Bradesco240 ed = new EspecieDocumento_Bradesco240();

                alEspeciesDocumento.Add(new EspecieDocumento_Bradesco240(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_Bradesco240.Cheque)));
                alEspeciesDocumento.Add(new EspecieDocumento_Bradesco240(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_Bradesco240.DuplicataMercantil)));
                alEspeciesDocumento.Add(new EspecieDocumento_Bradesco240(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_Bradesco240.DuplicataMercantilIndicacao)));
                alEspeciesDocumento.Add(new EspecieDocumento_Bradesco240(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_Bradesco240.DuplicataServico)));
                alEspeciesDocumento.Add(new EspecieDocumento_Bradesco240(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_Bradesco240.DuplicataServicoIndicacao)));
                alEspeciesDocumento.Add(new EspecieDocumento_Bradesco240(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_Bradesco240.DuplicataRural)));
                alEspeciesDocumento.Add(new EspecieDocumento_Bradesco240(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_Bradesco240.LetraCambio)));
                alEspeciesDocumento.Add(new EspecieDocumento_Bradesco240(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_Bradesco240.NotaCreditoComercial)));
                alEspeciesDocumento.Add(new EspecieDocumento_Bradesco240(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_Bradesco240.NotaCreditoExportacao)));
                alEspeciesDocumento.Add(new EspecieDocumento_Bradesco240(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_Bradesco240.NotaCreditoIndustrial)));
                alEspeciesDocumento.Add(new EspecieDocumento_Bradesco240(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_Bradesco240.NotaCreditoRural)));
                alEspeciesDocumento.Add(new EspecieDocumento_Bradesco240(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_Bradesco240.NotaPromissoria)));
                alEspeciesDocumento.Add(new EspecieDocumento_Bradesco240(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_Bradesco240.NotaPromissoriaRural)));
                alEspeciesDocumento.Add(new EspecieDocumento_Bradesco240(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_Bradesco240.TriplicataMercantil)));
                alEspeciesDocumento.Add(new EspecieDocumento_Bradesco240(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_Bradesco240.TriplicataServico)));
                alEspeciesDocumento.Add(new EspecieDocumento_Bradesco240(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_Bradesco240.NotaSeguro)));
                alEspeciesDocumento.Add(new EspecieDocumento_Bradesco240(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_Bradesco240.Recibo)));
                alEspeciesDocumento.Add(new EspecieDocumento_Bradesco240(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_Bradesco240.Fatura)));
                alEspeciesDocumento.Add(new EspecieDocumento_Bradesco240(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_Bradesco240.NotaDebito)));

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