using System;
using System.Collections.Generic;
using System.Text;

namespace BoletoNet
{
    #region Enumerado

    public enum EnumEspecieDocumento_Bradesco400
    {
        DuplicataMercantil = 1,
        NotaPromissoria = 2,
        NotaSeguro = 3,
        CobrancaSeriada = 4,
        Recibo = 5,
        LetraCambio = 10,
        NotaDebito = 11,
        DuplicataServico = 12,
        Outros = 99,
    }

    #endregion

    public class EspecieDocumento_Bradesco400 : AbstractEspecieDocumento, IEspecieDocumento
    {
        #region Construtores

        public EspecieDocumento_Bradesco400()
        {
            try
            {
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }

        public EspecieDocumento_Bradesco400(string codigo)
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

        public string getCodigoEspecieByEnum(EnumEspecieDocumento_Bradesco400 especie)
        {
            switch (especie)
            {
                case EnumEspecieDocumento_Bradesco400.DuplicataMercantil: return "1";
                case EnumEspecieDocumento_Bradesco400.NotaPromissoria: return "2";
                case EnumEspecieDocumento_Bradesco400.NotaSeguro: return "3";
                case EnumEspecieDocumento_Bradesco400.CobrancaSeriada: return "4";
                case EnumEspecieDocumento_Bradesco400.Recibo: return "5";
                case EnumEspecieDocumento_Bradesco400.LetraCambio: return "10";
                case EnumEspecieDocumento_Bradesco400.NotaDebito: return "11";
                case EnumEspecieDocumento_Bradesco400.DuplicataServico: return "12";
                case EnumEspecieDocumento_Bradesco400.Outros: return "99";
                default: return "99";

            }
        }

        public EnumEspecieDocumento_Bradesco400 getEnumEspecieByCodigo(string codigo)
        {
            switch (codigo)
            {
                case "1": return EnumEspecieDocumento_Bradesco400.DuplicataMercantil;
                case "2": return EnumEspecieDocumento_Bradesco400.NotaPromissoria;
                case "3": return EnumEspecieDocumento_Bradesco400.NotaSeguro;
                case "4": return EnumEspecieDocumento_Bradesco400.CobrancaSeriada;
                case "5": return EnumEspecieDocumento_Bradesco400.Recibo;
                case "10": return EnumEspecieDocumento_Bradesco400.LetraCambio;
                case "11": return EnumEspecieDocumento_Bradesco400.NotaDebito;
                case "12": return EnumEspecieDocumento_Bradesco400.DuplicataServico;
                case "99": return EnumEspecieDocumento_Bradesco400.Outros;
                default: return EnumEspecieDocumento_Bradesco400.Outros;
            }
        }

        private void carregar(string idCodigo)
        {
            try
            {
                this.Banco = new Banco_Bradesco();

                switch (getEnumEspecieByCodigo(idCodigo))
                {
                    case EnumEspecieDocumento_Bradesco400.DuplicataMercantil:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Bradesco400.DuplicataMercantil);
                        this.Especie = "Duplicata mercantil";
                        this.Sigla = "DM";
                        break;
                    case EnumEspecieDocumento_Bradesco400.NotaPromissoria:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Bradesco400.NotaPromissoria);
                        this.Especie = "Nota promissória";
                        this.Sigla = "NP";
                        break;
                    case EnumEspecieDocumento_Bradesco400.NotaSeguro:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Bradesco400.NotaSeguro);
                        this.Especie = "Nota de seguro";
                        this.Sigla = "NS";
                        break;
                    case EnumEspecieDocumento_Bradesco400.CobrancaSeriada:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Bradesco400.CobrancaSeriada);
                        this.Especie = "Cobrança seriada";
                        this.Sigla = "CS";
                        break;
                    case EnumEspecieDocumento_Bradesco400.Recibo:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Bradesco400.Recibo);
                        this.Especie = "Recibo";
                        this.Sigla = "RC";
                        break;
                    case EnumEspecieDocumento_Bradesco400.LetraCambio:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Bradesco400.LetraCambio);
                        this.Sigla = "LC";
                        this.Especie = "Letra de câmbio";
                        break;
                    case EnumEspecieDocumento_Bradesco400.NotaDebito:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Bradesco400.NotaDebito);
                        this.Sigla = "ND";
                        this.Especie = "Nota de débito";
                        break;
                    case EnumEspecieDocumento_Bradesco400.DuplicataServico:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Bradesco400.DuplicataServico);
                        this.Sigla = "DS";
                        this.Especie = "Duplicata de serviço";
                        break;
                    case EnumEspecieDocumento_Bradesco400.Outros:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Bradesco400.Outros);
                        this.Especie = "Outros";
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

                EspecieDocumento_Bradesco400 ed = new EspecieDocumento_Bradesco400();

                alEspeciesDocumento.Add(new EspecieDocumento_Bradesco400(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_Bradesco400.DuplicataMercantil)));
                alEspeciesDocumento.Add(new EspecieDocumento_Bradesco400(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_Bradesco400.NotaPromissoria)));
                alEspeciesDocumento.Add(new EspecieDocumento_Bradesco400(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_Bradesco400.NotaSeguro)));
                alEspeciesDocumento.Add(new EspecieDocumento_Bradesco400(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_Bradesco400.CobrancaSeriada)));
                alEspeciesDocumento.Add(new EspecieDocumento_Bradesco400(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_Bradesco400.Recibo)));
                alEspeciesDocumento.Add(new EspecieDocumento_Bradesco400(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_Bradesco400.LetraCambio)));
                alEspeciesDocumento.Add(new EspecieDocumento_Bradesco400(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_Bradesco400.NotaDebito)));
                alEspeciesDocumento.Add(new EspecieDocumento_Bradesco400(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_Bradesco400.DuplicataServico)));
                alEspeciesDocumento.Add(new EspecieDocumento_Bradesco400(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_Bradesco400.Outros)));

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