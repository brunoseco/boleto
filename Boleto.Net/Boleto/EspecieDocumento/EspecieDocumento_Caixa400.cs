using System;
using System.Collections.Generic;
using System.Text;

namespace BoletoNet
{
    #region Enumerador

    public enum EnumEspecieDocumento_Caixa400
    {
        DuplicataMercantil = 1,
        NotaPromissoria,
        DuplicataServico,
        NotaSeguro = 5,
        LetraCambio,
        Outros = 9,
    }

    #endregion

    public class EspecieDocumento_Caixa400 : AbstractEspecieDocumento, IEspecieDocumento
    {
        #region Construtores

        public EspecieDocumento_Caixa400()
        {
            try
            {
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }

        public EspecieDocumento_Caixa400(string codigo)
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

        public string getCodigoEspecieByEnum(EnumEspecieDocumento_Caixa400 especie)
        {
            switch (especie)
            {
                case EnumEspecieDocumento_Caixa400.DuplicataMercantil: return "1";
                case EnumEspecieDocumento_Caixa400.NotaPromissoria: return "2";
                case EnumEspecieDocumento_Caixa400.DuplicataServico: return "3";
                case EnumEspecieDocumento_Caixa400.NotaSeguro: return "5";
                case EnumEspecieDocumento_Caixa400.LetraCambio: return "6";
                case EnumEspecieDocumento_Caixa400.Outros: return "9";
                default: return "23";
            }
        }

        public EnumEspecieDocumento_Caixa400 getEnumEspecieByCodigo(string codigo)
        {
            switch (codigo)
            {
                case "1": return EnumEspecieDocumento_Caixa400.DuplicataMercantil;
                case "2": return EnumEspecieDocumento_Caixa400.NotaPromissoria;
                case "3": return EnumEspecieDocumento_Caixa400.DuplicataServico;
                case "5": return EnumEspecieDocumento_Caixa400.NotaSeguro;
                case "6": return EnumEspecieDocumento_Caixa400.LetraCambio;
                case "9": return EnumEspecieDocumento_Caixa400.Outros;
                default: return EnumEspecieDocumento_Caixa400.DuplicataMercantil;
            }
        }

        private void carregar(string idCodigo)
        {
            try
            {
                this.Banco = new Banco_Caixa();

                switch (getEnumEspecieByCodigo(idCodigo))
                {
                    case EnumEspecieDocumento_Caixa400.DuplicataMercantil:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Caixa400.DuplicataMercantil);
                        this.Especie = "DUPLICATA MERCANTIL";
                        this.Sigla = "DM";
                        break;
                    case EnumEspecieDocumento_Caixa400.NotaPromissoria:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Caixa400.NotaPromissoria);
                        this.Especie = "NOTA PROMISSORIA";
                        this.Sigla = "NP";
                        break;
                    case EnumEspecieDocumento_Caixa400.DuplicataServico:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Caixa400.DuplicataServico);
                        this.Especie = "DUPLICATA DE PRESTACAO DE SERVICOS";
                        this.Sigla = "DS";
                        break;
                    case EnumEspecieDocumento_Caixa400.NotaSeguro:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Caixa400.NotaSeguro);
                        this.Especie = "NOTA DE SEGURO";
                        this.Sigla = "NS";
                        break;
                    case EnumEspecieDocumento_Caixa400.LetraCambio:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Caixa400.LetraCambio);
                        this.Especie = "LETRA DE CAMBIO";
                        this.Sigla = "LC";
                        break;
                    case EnumEspecieDocumento_Caixa400.Outros:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_Caixa400.Outros);
                        this.Especie = "OUTROS";
                        this.Sigla = "OU";
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
            EspecieDocumento_Caixa400 ed = new EspecieDocumento_Caixa400();

            foreach (EnumEspecieDocumento_Caixa400 item in Enum.GetValues(typeof(EnumEspecieDocumento_Caixa400)))
                especiesDocumento.Add(new EspecieDocumento_Caixa400(ed.getCodigoEspecieByEnum(item)));

            return especiesDocumento;
        }

        #endregion
    }
}
