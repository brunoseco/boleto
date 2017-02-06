using System;
using System.Collections.Generic;
using System.Text;

namespace BoletoNet
{
    #region Enumerado

    public enum EnumEspecieDocumento_BancoBrasil400
    {
        DuplicataMercantil = 1, //DM – DUPLICATA MERCANTIL
        NotaPromissoria = 2, //NP – NOTA PROMISSÓRIA
        NotaSeguro = 3, //NS – NOTA DE SEGURO
        Recibo = 5, //RC – RECIBO
        LetraCambio = 8, //LC – LETRA DE CAMBIO
        Warrant = 9, //WA - WARRANT
        Cheque = 10, //CH – CHEQUE
        DuplicataServico = 12, //DS – DUPLICATA SERVICO
        NotaDebito = 13, //ND –  NOTA DE DÉBITO
        ApoliceSeguro = 15, //AP –  APÓLICE DE SEGURO
        DividaAtivaUniao = 25, //DAU –  DIVIDA ATIVA DA UNIÃO
        DividaAtivaEstado = 26, //DAE –  APÓLICE DE SEGURO
        DividaAtivaMunicipio = 27, //DAM –  APÓLICE DE SEGURO
    }

    #endregion

    public class EspecieDocumento_BancoBrasil400 : AbstractEspecieDocumento, IEspecieDocumento
    {
        #region Construtores

        public EspecieDocumento_BancoBrasil400()
        {
            try
            {
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }

        public EspecieDocumento_BancoBrasil400(string codigo)
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

        public string getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil400 especie)
        {
            switch (especie)
            {
                case EnumEspecieDocumento_BancoBrasil400.Cheque: return "10";
                case EnumEspecieDocumento_BancoBrasil400.DuplicataMercantil: return "1";
                case EnumEspecieDocumento_BancoBrasil400.DuplicataServico: return "12";
                case EnumEspecieDocumento_BancoBrasil400.LetraCambio: return "8";
                case EnumEspecieDocumento_BancoBrasil400.NotaPromissoria: return "2";
                case EnumEspecieDocumento_BancoBrasil400.NotaSeguro: return "3";
                case EnumEspecieDocumento_BancoBrasil400.Recibo: return "5";
                case EnumEspecieDocumento_BancoBrasil400.NotaDebito: return "13";
                case EnumEspecieDocumento_BancoBrasil400.ApoliceSeguro: return "15";
                default: return "1";
            }
        }

        public EnumEspecieDocumento_BancoBrasil400 getEnumEspecieByCodigo(string codigo)
        {
            switch (codigo)
            {
                case "10": return EnumEspecieDocumento_BancoBrasil400.Cheque;
                case "1": return EnumEspecieDocumento_BancoBrasil400.DuplicataMercantil;
                case "12": return EnumEspecieDocumento_BancoBrasil400.DuplicataServico;
                case "8": return EnumEspecieDocumento_BancoBrasil400.LetraCambio;
                case "2": return EnumEspecieDocumento_BancoBrasil400.NotaPromissoria;
                case "3": return EnumEspecieDocumento_BancoBrasil400.NotaSeguro;
                case "5": return EnumEspecieDocumento_BancoBrasil400.Recibo;
                case "13": return EnumEspecieDocumento_BancoBrasil400.NotaDebito;
                case "15": return EnumEspecieDocumento_BancoBrasil400.ApoliceSeguro;
                default: return EnumEspecieDocumento_BancoBrasil400.DuplicataMercantil;
            }
        }

        private void carregar(string idCodigo)
        {
            try
            {
                this.Banco = new Banco_Brasil();

                switch (getEnumEspecieByCodigo(idCodigo))
                {
                    case EnumEspecieDocumento_BancoBrasil400.Cheque:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil400.Cheque);
                        this.Especie = "CHEQUE";
                        this.Sigla = "CH";
                        break;
                    case EnumEspecieDocumento_BancoBrasil400.DuplicataMercantil:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil400.DuplicataMercantil);
                        this.Especie = "DUPLICATA MERCANTIL";
                        this.Sigla = "DM";
                        break;
                    case EnumEspecieDocumento_BancoBrasil400.DuplicataServico:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil400.DuplicataServico);
                        this.Especie = "DUPLICATA DE SERVIÇO";
                        this.Sigla = "DS";
                        break;
                    case EnumEspecieDocumento_BancoBrasil400.LetraCambio:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil400.LetraCambio);
                        this.Especie = "LETRA DE CAMBIO";
                        this.Sigla = "LC";
                        break;
                    case EnumEspecieDocumento_BancoBrasil400.NotaPromissoria:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil400.NotaPromissoria);
                        this.Especie = "NOTA PROMISSÓRIA";
                        this.Sigla = "NP";
                        break;
                    case EnumEspecieDocumento_BancoBrasil400.NotaSeguro:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil400.NotaSeguro);
                        this.Especie = "NOTA DE SEGURO";
                        this.Sigla = "NS";
                        break;
                    case EnumEspecieDocumento_BancoBrasil400.Recibo:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil400.Recibo);
                        this.Especie = "RECIBO";
                        this.Sigla = "RC";
                        break;
                    case EnumEspecieDocumento_BancoBrasil400.NotaDebito:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil400.NotaDebito);
                        this.Especie = "NOTA DE DÉBITO";
                        this.Sigla = "ND";
                        break;
                    case EnumEspecieDocumento_BancoBrasil400.ApoliceSeguro:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil400.ApoliceSeguro);
                        this.Especie = "APÓLICE DE SEGURO";
                        this.Sigla = "AP";
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
                EspecieDocumento_BancoBrasil400 ed = new EspecieDocumento_BancoBrasil400();

                alEspeciesDocumento.Add(new EspecieDocumento_BancoBrasil400(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil400.Cheque)));
                alEspeciesDocumento.Add(new EspecieDocumento_BancoBrasil400(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil400.DuplicataMercantil)));
                alEspeciesDocumento.Add(new EspecieDocumento_BancoBrasil400(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil400.DuplicataServico)));
                alEspeciesDocumento.Add(new EspecieDocumento_BancoBrasil400(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil400.LetraCambio)));
                alEspeciesDocumento.Add(new EspecieDocumento_BancoBrasil400(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil400.NotaPromissoria)));
                alEspeciesDocumento.Add(new EspecieDocumento_BancoBrasil400(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil400.NotaSeguro)));
                alEspeciesDocumento.Add(new EspecieDocumento_BancoBrasil400(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil400.Recibo)));
                alEspeciesDocumento.Add(new EspecieDocumento_BancoBrasil400(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil400.NotaDebito)));
                alEspeciesDocumento.Add(new EspecieDocumento_BancoBrasil400(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil400.ApoliceSeguro)));

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
