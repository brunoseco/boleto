using System;
using System.Collections.Generic;
using System.Text;

namespace BoletoNet
{
    #region Enumerado

    public enum EnumEspecieDocumento_Santander400
    {
        DuplicataMercantil = 1,
        DuplicataServico = 6,
        NotaPromissoria = 2,
        ApoliceSeguro = 3,
        LetraDeCambio = 7,
        Recibo = 5,
    }

    #endregion

    public class EspecieDocumento_Santander400 : AbstractEspecieDocumento, IEspecieDocumento
    {
        #region Construtores

        public EspecieDocumento_Santander400()
        {
            try
            {
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }

        public EspecieDocumento_Santander400(string codigo)
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

        public string getCodigoEspecieByEnum(EnumEspecieDocumento_Santander400 especie)
        {
            switch (especie)
            {
                case EnumEspecieDocumento_Santander400.DuplicataMercantil: return "1";
                case EnumEspecieDocumento_Santander400.DuplicataServico: return "6";
                case EnumEspecieDocumento_Santander400.NotaPromissoria: return "2";
                case EnumEspecieDocumento_Santander400.ApoliceSeguro: return "3";
                case EnumEspecieDocumento_Santander400.LetraDeCambio: return "7";
                case EnumEspecieDocumento_Santander400.Recibo: return "5";
                default: return "1"; //Duplicata Mercantil
            }
        }

        public EnumEspecieDocumento_Santander400 getEnumEspecieByCodigo(string codigo)
        {
            switch (codigo)
            {
                case "1": return EnumEspecieDocumento_Santander400.DuplicataMercantil;
                case "6": return EnumEspecieDocumento_Santander400.DuplicataServico;
                case "2": return EnumEspecieDocumento_Santander400.NotaPromissoria;
                case "3": return EnumEspecieDocumento_Santander400.ApoliceSeguro;
                case "7": return EnumEspecieDocumento_Santander400.LetraDeCambio;
                case "5": return EnumEspecieDocumento_Santander400.Recibo;
                default: return EnumEspecieDocumento_Santander400.DuplicataMercantil;
            }
        }

        private void carregar(string idCodigo)
        {
            try
            {
                this.Banco = new Banco_Santander();
                EspecieDocumento_Santander400 ed = new EspecieDocumento_Santander400();
                switch (ed.getEnumEspecieByCodigo(idCodigo))
                {
                    case EnumEspecieDocumento_Santander400.DuplicataMercantil:
                        this.Codigo = ed.getCodigoEspecieByEnum(EnumEspecieDocumento_Santander400.DuplicataMercantil);
                        this.Especie = "Duplicata Mercantil";
                        this.Sigla = "DM";
                        break;
                    case EnumEspecieDocumento_Santander400.DuplicataServico:
                        this.Codigo = ed.getCodigoEspecieByEnum(EnumEspecieDocumento_Santander400.DuplicataServico);
                        this.Especie = "Duplicata de Serviço";
                        this.Sigla = "DS";
                        break;
                    case EnumEspecieDocumento_Santander400.Recibo:
                        this.Codigo = ed.getCodigoEspecieByEnum(EnumEspecieDocumento_Santander400.Recibo);
                        this.Especie = "Recibo";
                        this.Sigla = "R";
                        break;
                    case EnumEspecieDocumento_Santander400.ApoliceSeguro:
                        this.Codigo = ed.getCodigoEspecieByEnum(EnumEspecieDocumento_Santander400.ApoliceSeguro);
                        this.Especie = "Apôlice de Seguro";
                        this.Sigla = "AP";
                        break;
                    case EnumEspecieDocumento_Santander400.NotaPromissoria:
                        this.Codigo = ed.getCodigoEspecieByEnum(EnumEspecieDocumento_Santander400.NotaPromissoria);
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
            EspecieDocumento_Santander400 ed = new EspecieDocumento_Santander400();
            foreach (EnumEspecieDocumento_Santander400 item in Enum.GetValues(typeof(EnumEspecieDocumento_Santander400)))
                especiesDocumento.Add(new EspecieDocumento_Santander400(ed.getCodigoEspecieByEnum(item)));

            return especiesDocumento;
        }

        #endregion
    }
}
