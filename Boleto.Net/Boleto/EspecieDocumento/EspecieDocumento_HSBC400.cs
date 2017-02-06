using System;
using System.Collections.Generic;
using System.Text;

namespace BoletoNet
{
    #region Enumerado

    public enum EnumEspecieDocumento_HSBC400
    {
        //Cobrança Simplificada
        DuplicataMercantil = 1, //DP – DUPLICATA MERCANTIL
        NotaPromissoria = 2, //NP – NOTA PROMISSÓRIA
        NotaDeSeguro = 3, //NS – NOTA DE SEGURO
        Recibo = 5, //RC – RECIBO
        DuplicataServiços = 10, //DS – DUPLICATA DE SERVIÇOS

        //Cobrança Expressa
        ComplementaçãoDoBloquetoPeloCliente = 08, //SD - Com complementação do bloqueto pelo cliente

        //Cobrança Escritural
        CobrançaComEmissãoPeloBanco = 09, //CE – Cobrança com emissão total do bloqueto pelo Banco

        //Cobrança Diretiva
        CobrançaComEmissãoPeloCliente = 98, //PD – Cobrança com emissão total do bloqueto pelo cliente

        Outros = 99 //Outros
    }

    #endregion

    public class EspecieDocumento_HSBC400 : AbstractEspecieDocumento, IEspecieDocumento
    {
        #region Construtores

        public EspecieDocumento_HSBC400()
        {
            try
            {
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }

        public EspecieDocumento_HSBC400(string codigo)
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

        public string getCodigoEspecieByEnum(EnumEspecieDocumento_HSBC400 especie)
        {
            switch (especie)
            {
                case EnumEspecieDocumento_HSBC400.DuplicataMercantil: return "1";
                case EnumEspecieDocumento_HSBC400.NotaPromissoria: return "2";
                case EnumEspecieDocumento_HSBC400.NotaDeSeguro: return "3";
                case EnumEspecieDocumento_HSBC400.Recibo: return "5";
                case EnumEspecieDocumento_HSBC400.DuplicataServiços: return "10";
                case EnumEspecieDocumento_HSBC400.ComplementaçãoDoBloquetoPeloCliente: return "8";
                case EnumEspecieDocumento_HSBC400.CobrançaComEmissãoPeloBanco: return "9";
                case EnumEspecieDocumento_HSBC400.CobrançaComEmissãoPeloCliente: return "98";
                case EnumEspecieDocumento_HSBC400.Outros: return "99";
                default: return "99";
            }
        }

        public EnumEspecieDocumento_HSBC400 getEnumEspecieByCodigo(string codigo)
        {
            switch (codigo)
            {
                case "1": return EnumEspecieDocumento_HSBC400.DuplicataMercantil;
                case "2": return EnumEspecieDocumento_HSBC400.NotaPromissoria;
                case "3": return EnumEspecieDocumento_HSBC400.NotaDeSeguro;
                case "5": return EnumEspecieDocumento_HSBC400.Recibo;
                case "8": return EnumEspecieDocumento_HSBC400.ComplementaçãoDoBloquetoPeloCliente;
                case "9": return EnumEspecieDocumento_HSBC400.CobrançaComEmissãoPeloBanco;
                case "10": return EnumEspecieDocumento_HSBC400.DuplicataServiços;
                case "98": return EnumEspecieDocumento_HSBC400.CobrançaComEmissãoPeloCliente;
                case "99": return EnumEspecieDocumento_HSBC400.Outros;
                default: return EnumEspecieDocumento_HSBC400.Outros;
            }
        }

        private void carregar(string idCodigo)
        {
            try
            {
                this.Banco = new Banco_HSBC();

                switch (getEnumEspecieByCodigo(idCodigo))
                {
                    case EnumEspecieDocumento_HSBC400.DuplicataMercantil:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_HSBC400.DuplicataMercantil);
                        this.Especie = "DUPLICATA MERCANTIL";
                        this.Sigla = "DP";
                        break;
                    case EnumEspecieDocumento_HSBC400.DuplicataServiços:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_HSBC400.DuplicataServiços);
                        this.Especie = "DUPLICATA DE SERVIÇO";
                        this.Sigla = "DS";
                        break;
                    case EnumEspecieDocumento_HSBC400.NotaPromissoria:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_HSBC400.NotaPromissoria);
                        this.Especie = "NOTA PROMISSÓRIA";
                        this.Sigla = "NP";
                        break;
                    case EnumEspecieDocumento_HSBC400.NotaDeSeguro:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_HSBC400.NotaDeSeguro);
                        this.Especie = "NOTA DE SEGURO";
                        this.Sigla = "NS";
                        break;
                    case EnumEspecieDocumento_HSBC400.Recibo:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_HSBC400.Recibo);
                        this.Especie = "RECIBO";
                        this.Sigla = "RC";
                        break;
                    case EnumEspecieDocumento_HSBC400.ComplementaçãoDoBloquetoPeloCliente:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_HSBC400.ComplementaçãoDoBloquetoPeloCliente);
                        this.Especie = "COM COMPLEMENTAÇÃO DO BLOQUETO PELO CLIENTE";
                        this.Sigla = "SD";
                        break;
                    case EnumEspecieDocumento_HSBC400.CobrançaComEmissãoPeloBanco:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_HSBC400.CobrançaComEmissãoPeloBanco);
                        this.Especie = "COBRANÇA COM EMISSÃO TOTAL DO BLOQUETO PELO BANCO";
                        this.Sigla = "CE";
                        break;
                    case EnumEspecieDocumento_HSBC400.CobrançaComEmissãoPeloCliente:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_HSBC400.CobrançaComEmissãoPeloCliente);
                        this.Especie = "COBRANÇA COM EMISSÃO TOTAL DO BLOQUETO PELO CLIENTE";
                        this.Sigla = "PD";
                        break;
                    case EnumEspecieDocumento_HSBC400.Outros:
                        this.Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_HSBC400.Outros);
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
                EspecieDocumento_HSBC400 ed = new EspecieDocumento_HSBC400();

                alEspeciesDocumento.Add(new EspecieDocumento_HSBC400(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_HSBC400.DuplicataMercantil)));
                alEspeciesDocumento.Add(new EspecieDocumento_HSBC400(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_HSBC400.NotaPromissoria)));
                alEspeciesDocumento.Add(new EspecieDocumento_HSBC400(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_HSBC400.Recibo)));
                alEspeciesDocumento.Add(new EspecieDocumento_HSBC400(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_HSBC400.CobrançaComEmissãoPeloBanco)));
                alEspeciesDocumento.Add(new EspecieDocumento_HSBC400(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_HSBC400.CobrançaComEmissãoPeloCliente)));
                alEspeciesDocumento.Add(new EspecieDocumento_HSBC400(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_HSBC400.ComplementaçãoDoBloquetoPeloCliente)));
                alEspeciesDocumento.Add(new EspecieDocumento_HSBC400(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_HSBC400.DuplicataServiços)));
                alEspeciesDocumento.Add(new EspecieDocumento_HSBC400(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_HSBC400.NotaDeSeguro)));                
                alEspeciesDocumento.Add(new EspecieDocumento_HSBC400(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_HSBC400.Outros)));

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
