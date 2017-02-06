using System;
using System.Collections.Generic;
using System.Text;

namespace BoletoNet
{
    #region Enumerado

    public enum EnumEspecieDocumento_HSBC240
    {
        DuplicataMercantil = 2, //DM – DUPLICATA MERCANTIL
        DuplicataServico = 4, //DS –  DUPLICATA DE SERVIÇO
        DuplicataRural = 6, //DR – DUPLICATA RURAL
        LetraCambio = 7, //LC – LETRA DE CAMBIO
        NotaCreditoComercial = 8, //NCC – NOTA DE CRÉDITO COMERCIAL
        NotaCreditoIndustrial = 10, //NCI – NOTA DE CRÉDITO INDUSTRIAL
        NotaPromissoria = 12, //NP – NOTA PROMISSÓRIA
        NotaPromissoriaRural = 13, //NPR – NOTA PROMISSÓRIA RURAL
        NotaSeguro = 16, //NS – NOTA DE SEGURO
        Recibo = 17, //RC – RECIBO
        NotaDebito = 19, //ND –  NOTA DE DÉBITO
        ApoliceSeguro = 20, //AP –  APÓLICE DE SEGURO
        Outros = 99 //OUTROS
    }

    #endregion

    public class EspecieDocumento_HSBC240 : AbstractEspecieDocumento, IEspecieDocumento
    {
        #region Construtores

        public EspecieDocumento_HSBC240()
        {
            try
            {
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }

        public EspecieDocumento_HSBC240(string codigo)
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

        public string getCodigoEspecieByEnum(EnumEspecieDocumento_HSBC240 especie)
        {
            switch (especie)
            {
                case EnumEspecieDocumento_HSBC240.DuplicataMercantil: return "2";
                case EnumEspecieDocumento_HSBC240.DuplicataServico: return "4";
                case EnumEspecieDocumento_HSBC240.DuplicataRural: return "6";
                case EnumEspecieDocumento_HSBC240.LetraCambio: return "7";
                case EnumEspecieDocumento_HSBC240.NotaCreditoComercial: return "8";
                case EnumEspecieDocumento_HSBC240.NotaCreditoIndustrial: return "10";
                case EnumEspecieDocumento_HSBC240.NotaPromissoria: return "12";
                case EnumEspecieDocumento_HSBC240.NotaPromissoriaRural: return "13";
                case EnumEspecieDocumento_HSBC240.NotaSeguro: return "16";
                case EnumEspecieDocumento_HSBC240.Recibo: return "17";
                case EnumEspecieDocumento_HSBC240.NotaDebito: return "19";
                case EnumEspecieDocumento_HSBC240.ApoliceSeguro: return "20";
                case EnumEspecieDocumento_HSBC240.Outros: return "99";
                default: return "2";
            }
        }

        public EnumEspecieDocumento_HSBC240 getEnumEspecieByCodigo(string codigo)
        {
            switch (codigo)
            {
                case "2": return EnumEspecieDocumento_HSBC240.DuplicataMercantil;
                case "4": return EnumEspecieDocumento_HSBC240.DuplicataServico;
                case "6": return EnumEspecieDocumento_HSBC240.DuplicataRural;
                case "7": return EnumEspecieDocumento_HSBC240.LetraCambio;
                case "8": return EnumEspecieDocumento_HSBC240.NotaCreditoComercial;
                case "10": return EnumEspecieDocumento_HSBC240.NotaCreditoIndustrial;
                case "12": return EnumEspecieDocumento_HSBC240.NotaPromissoria;
                case "13": return EnumEspecieDocumento_HSBC240.NotaPromissoriaRural;
                case "16": return EnumEspecieDocumento_HSBC240.NotaSeguro;
                case "17": return EnumEspecieDocumento_HSBC240.Recibo;
                case "19": return EnumEspecieDocumento_HSBC240.NotaDebito;
                case "20": return EnumEspecieDocumento_HSBC240.ApoliceSeguro;
                case "99": return EnumEspecieDocumento_HSBC240.Outros;
                default: return EnumEspecieDocumento_HSBC240.DuplicataMercantil;
            }
        }

        private void carregar(string idCodigo)
        {
            try
            {
                this.Banco = new Banco_HSBC();

                switch (getEnumEspecieByCodigo(idCodigo))
                {
                    case EnumEspecieDocumento_HSBC240.DuplicataMercantil:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_HSBC240.DuplicataMercantil);
                        this.Especie = "DUPLICATA MERCANTIL";
                        this.Sigla = "DM";
                        break;
                    case EnumEspecieDocumento_HSBC240.DuplicataServico:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_HSBC240.DuplicataServico);
                        this.Especie = "DUPLICATA DE SERVIÇO";
                        this.Sigla = "DS";
                        break;
                    case EnumEspecieDocumento_HSBC240.DuplicataRural:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_HSBC240.DuplicataRural);
                        this.Especie = "DUPLICATA RURAL";
                        this.Sigla = "DR";
                        break;
                    case EnumEspecieDocumento_HSBC240.LetraCambio:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_HSBC240.LetraCambio);
                        this.Especie = "LETRA DE CAMBIO";
                        this.Sigla = "LC";
                        break;
                    case EnumEspecieDocumento_HSBC240.NotaCreditoComercial:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_HSBC240.NotaCreditoComercial);
                        this.Especie = "NOTA DE CRÉDITO COMERCIAL";
                        this.Sigla = "NCC";
                        break;
                    case EnumEspecieDocumento_HSBC240.NotaCreditoIndustrial:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_HSBC240.NotaCreditoIndustrial);
                        this.Especie = "NOTA DE CRÉDITO INDUSTRIAL";
                        this.Sigla = "NCI";
                        break;
                    case EnumEspecieDocumento_HSBC240.NotaPromissoria:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_HSBC240.NotaPromissoria);
                        this.Especie = "NOTA PROMISSÓRIA";
                        this.Sigla = "NP";
                        break;
                    case EnumEspecieDocumento_HSBC240.NotaPromissoriaRural:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_HSBC240.NotaPromissoriaRural);
                        this.Especie = "NOTA PROMISSÓRIA RURAL";
                        this.Sigla = "NPR";
                        break;
                    case EnumEspecieDocumento_HSBC240.NotaSeguro:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_HSBC240.NotaSeguro);
                        this.Especie = "NOTA DE SEGURO";
                        this.Sigla = "NS";
                        break;
                    case EnumEspecieDocumento_HSBC240.Recibo:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_HSBC240.Recibo);
                        this.Especie = "RECIBO";
                        this.Sigla = "RC";
                        break;
                    case EnumEspecieDocumento_HSBC240.NotaDebito:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_HSBC240.NotaDebito);
                        this.Especie = "NOTA DE DÉBITO";
                        this.Sigla = "ND";
                        break;
                    case EnumEspecieDocumento_HSBC240.ApoliceSeguro:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_HSBC240.ApoliceSeguro);
                        this.Especie = "APÓLICE DE SEGURO";
                        this.Sigla = "AP";
                        break;
                    case EnumEspecieDocumento_HSBC240.Outros:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_HSBC240.Outros);
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
                EspecieDocumento_HSBC240 ed = new EspecieDocumento_HSBC240();

                alEspeciesDocumento.Add(new EspecieDocumento_HSBC240(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_HSBC240.DuplicataMercantil)));
                alEspeciesDocumento.Add(new EspecieDocumento_HSBC240(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_HSBC240.DuplicataServico)));
                alEspeciesDocumento.Add(new EspecieDocumento_HSBC240(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_HSBC240.DuplicataRural)));
                alEspeciesDocumento.Add(new EspecieDocumento_HSBC240(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_HSBC240.LetraCambio)));
                alEspeciesDocumento.Add(new EspecieDocumento_HSBC240(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_HSBC240.NotaCreditoComercial)));
                alEspeciesDocumento.Add(new EspecieDocumento_HSBC240(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_HSBC240.NotaCreditoIndustrial)));
                alEspeciesDocumento.Add(new EspecieDocumento_HSBC240(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_HSBC240.NotaPromissoria)));
                alEspeciesDocumento.Add(new EspecieDocumento_HSBC240(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_HSBC240.NotaPromissoriaRural)));
                alEspeciesDocumento.Add(new EspecieDocumento_HSBC240(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_HSBC240.NotaSeguro)));
                alEspeciesDocumento.Add(new EspecieDocumento_HSBC240(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_HSBC240.Recibo)));
                alEspeciesDocumento.Add(new EspecieDocumento_HSBC240(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_HSBC240.NotaDebito)));
                alEspeciesDocumento.Add(new EspecieDocumento_HSBC240(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_HSBC240.ApoliceSeguro)));
                alEspeciesDocumento.Add(new EspecieDocumento_HSBC240(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_HSBC240.Outros)));

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
