using System;
using System.Collections.Generic;
using System.Text;

namespace BoletoNet
{
    #region Enumerado

    public enum EnumEspecieDocumento_Santander240
    {
        DuplicataMercantil = 2,
        DuplicataServico = 4,
        NotaPromissoria = 12,
        NotaPromissoriaRural = 13,
        Recibo = 17,
        Fatura = 18,
        ApoliceSeguro = 20,
        Cheque = 97,
        NotaPromissoriaDireta = 98,
        LetraDeCambio = 7,
    }

    #endregion

    public class EspecieDocumento_Santander240 : AbstractEspecieDocumento, IEspecieDocumento
    {
        #region Construtores

        public EspecieDocumento_Santander240()
        {
            try
            {
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }

        public EspecieDocumento_Santander240(string codigo)
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

        public string getCodigoEspecieByEnum(EnumEspecieDocumento_Santander240 especie)
        {
            switch (especie)
            {
                case EnumEspecieDocumento_Santander240.DuplicataMercantil: return "2";
                case EnumEspecieDocumento_Santander240.DuplicataServico: return "4";
                case EnumEspecieDocumento_Santander240.NotaPromissoria: return "12";
                case EnumEspecieDocumento_Santander240.NotaPromissoriaRural: return "13";
                case EnumEspecieDocumento_Santander240.Recibo: return "17";
                case EnumEspecieDocumento_Santander240.Fatura: return "18";
                case EnumEspecieDocumento_Santander240.ApoliceSeguro: return "20";
                case EnumEspecieDocumento_Santander240.Cheque: return "97";
                case EnumEspecieDocumento_Santander240.NotaPromissoriaDireta: return "98";
                case EnumEspecieDocumento_Santander240.LetraDeCambio: return "7";
                default: return "2"; //Duplicata Mercantil
            }
        }

        public EnumEspecieDocumento_Santander240 getEnumEspecieByCodigo(string codigo)
        {
            switch (codigo)
            {
                case "2": return EnumEspecieDocumento_Santander240.DuplicataMercantil;
                case "4": return EnumEspecieDocumento_Santander240.DuplicataServico;
                case "12": return EnumEspecieDocumento_Santander240.NotaPromissoria;
                case "13": return EnumEspecieDocumento_Santander240.NotaPromissoriaRural;
                case "17": return EnumEspecieDocumento_Santander240.Recibo;
                case "18": return EnumEspecieDocumento_Santander240.Fatura;
                case "20": return EnumEspecieDocumento_Santander240.ApoliceSeguro;
                case "97": return EnumEspecieDocumento_Santander240.Cheque;
                case "98": return EnumEspecieDocumento_Santander240.NotaPromissoriaDireta;
                case "7": return EnumEspecieDocumento_Santander240.LetraDeCambio;
                default: return EnumEspecieDocumento_Santander240.DuplicataMercantil;
            }
        }

        private void carregar(string idCodigo)
        {
            try
            {
                this.Banco = new Banco_Santander();
                EspecieDocumento_Santander240 ed = new EspecieDocumento_Santander240();
                switch (ed.getEnumEspecieByCodigo(idCodigo))
                {
                    case EnumEspecieDocumento_Santander240.DuplicataMercantil:
                        this.Codigo = ed.getCodigoEspecieByEnum(EnumEspecieDocumento_Santander240.DuplicataMercantil);
                        this.Especie = "Duplicata Mercantil";
                        this.Sigla = "DM";
                        break;
                    case EnumEspecieDocumento_Santander240.DuplicataServico:
                        this.Codigo = ed.getCodigoEspecieByEnum(EnumEspecieDocumento_Santander240.DuplicataServico);
                        this.Especie = "Duplicata de Serviço";
                        this.Sigla = "DS";
                        break;
                    case EnumEspecieDocumento_Santander240.Recibo:
                        this.Codigo = ed.getCodigoEspecieByEnum(EnumEspecieDocumento_Santander240.Recibo);
                        this.Especie = "Recibo";
                        this.Sigla = "R";
                        break;
                    case EnumEspecieDocumento_Santander240.ApoliceSeguro:
                        this.Codigo = ed.getCodigoEspecieByEnum(EnumEspecieDocumento_Santander240.ApoliceSeguro);
                        this.Especie = "Apôlice de Seguro";
                        this.Sigla = "AP";
                        break;
                    case EnumEspecieDocumento_Santander240.NotaPromissoria:
                        this.Codigo = ed.getCodigoEspecieByEnum(EnumEspecieDocumento_Santander240.NotaPromissoria);
                        this.Especie = "Nota Promissória";
                        this.Sigla = "NP";
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
            EspeciesDocumento especiesDocumento = new EspeciesDocumento();
            EspecieDocumento_Santander240 ed = new EspecieDocumento_Santander240();
            foreach (EnumEspecieDocumento_Santander240 item in Enum.GetValues(typeof(EnumEspecieDocumento_Santander240)))
                especiesDocumento.Add(new EspecieDocumento_Santander240(ed.getCodigoEspecieByEnum(item)));

            return especiesDocumento;
        }

        #endregion
    }
}
