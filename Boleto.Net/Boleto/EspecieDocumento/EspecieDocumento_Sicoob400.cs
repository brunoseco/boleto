using System;
using System.Collections.Generic;
using System.Text;

namespace BoletoNet
{
    #region Enumerado

    public enum EnumEspecieDocumento_Sicoob400
    {
        Duplicata_Mercantil = 1,
        Nota_Promiss�ria = 2,
        Nota_de_Seguro = 3,
        Recibo = 5, 
        Duplicata_Rural = 6, 
        Letra_de_C�mbio = 8,
        Warrant = 9,
        Cheque = 10,
        Duplicata_de_Servi�o = 12,
        Nota_de_D�bito = 13,
        Triplicata_Mercantil = 14,
        Triplicata_de_Servi�o = 15,
        Fatura = 18,
        Ap�lice_de_Seguro = 20,
        Mensalidade_Escolar = 21,
        Parcela_de_Cons�rcio = 22,
        Outros = 99
    }

    #endregion

    public class EspecieDocumento_Sicoob400 : AbstractEspecieDocumento, IEspecieDocumento
    {
        #region Construtores

        public EspecieDocumento_Sicoob400()
        {
            try
            {
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }

        public EspecieDocumento_Sicoob400(string codigo)
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

        public string getCodigoEspecieByEnum(EnumEspecieDocumento_Sicoob400 especie)
        {
            switch (especie)
            {
                case EnumEspecieDocumento_Sicoob400.Ap�lice_de_Seguro: return "20";
                case EnumEspecieDocumento_Sicoob400.Cheque: return "10";
                case EnumEspecieDocumento_Sicoob400.Duplicata_de_Servi�o: return "12";
                case EnumEspecieDocumento_Sicoob400.Duplicata_Mercantil: return "1";
                case EnumEspecieDocumento_Sicoob400.Duplicata_Rural: return "6";
                case EnumEspecieDocumento_Sicoob400.Fatura: return "18";
                case EnumEspecieDocumento_Sicoob400.Letra_de_C�mbio: return "8";
                case EnumEspecieDocumento_Sicoob400.Mensalidade_Escolar: return "21";
                case EnumEspecieDocumento_Sicoob400.Nota_de_D�bito: return "13";
                case EnumEspecieDocumento_Sicoob400.Nota_de_Seguro: return "3";
                case EnumEspecieDocumento_Sicoob400.Nota_Promiss�ria: return "2";
                case EnumEspecieDocumento_Sicoob400.Outros: return "99";
                case EnumEspecieDocumento_Sicoob400.Parcela_de_Cons�rcio: return "22";
                case EnumEspecieDocumento_Sicoob400.Recibo: return "5";
                case EnumEspecieDocumento_Sicoob400.Triplicata_de_Servi�o: return "15";
                case EnumEspecieDocumento_Sicoob400.Triplicata_Mercantil: return "14";
                case EnumEspecieDocumento_Sicoob400.Warrant: return "9";
                default: return "1"; //Duplicata Mercantil
            }
        }

        public EnumEspecieDocumento_Sicoob400 getEnumEspecieByCodigo(string codigo)
        {
            switch (codigo)
            {
                case "20": return EnumEspecieDocumento_Sicoob400.Ap�lice_de_Seguro;
                case "10": return EnumEspecieDocumento_Sicoob400.Cheque;
                case "12": return EnumEspecieDocumento_Sicoob400.Duplicata_de_Servi�o;
                case "1": return EnumEspecieDocumento_Sicoob400.Duplicata_Mercantil;
                case "6": return EnumEspecieDocumento_Sicoob400.Duplicata_Rural;
                case "18": return EnumEspecieDocumento_Sicoob400.Fatura;
                case "8": return EnumEspecieDocumento_Sicoob400.Letra_de_C�mbio;
                case "21": return EnumEspecieDocumento_Sicoob400.Mensalidade_Escolar;
                case "13": return EnumEspecieDocumento_Sicoob400.Nota_de_D�bito;
                case "3": return EnumEspecieDocumento_Sicoob400.Nota_de_Seguro;
                case "2": return EnumEspecieDocumento_Sicoob400.Nota_Promiss�ria;
                case "99": return EnumEspecieDocumento_Sicoob400.Outros;
                case "22": return EnumEspecieDocumento_Sicoob400.Parcela_de_Cons�rcio;
                case "5": return EnumEspecieDocumento_Sicoob400.Recibo;
                case "15": return EnumEspecieDocumento_Sicoob400.Triplicata_de_Servi�o;
                case "14": return EnumEspecieDocumento_Sicoob400.Triplicata_Mercantil;
                case "9": return EnumEspecieDocumento_Sicoob400.Warrant;
                default: return EnumEspecieDocumento_Sicoob400.Duplicata_Mercantil;

            }
        }

        private void carregar(string idCodigo)
        {
            try
            {
                this.Banco = new Banco_Sicoob();
                EspecieDocumento_Sicoob400 ed = new EspecieDocumento_Sicoob400();
                switch (ed.getEnumEspecieByCodigo(idCodigo))
                {
                    case EnumEspecieDocumento_Sicoob400.Duplicata_Mercantil:
                        this.Codigo = ed.getCodigoEspecieByEnum(EnumEspecieDocumento_Sicoob400.Duplicata_Mercantil);
                        this.Especie = "Duplicata Mercantil";
                        this.Sigla = "DM";
                        break;
                    case EnumEspecieDocumento_Sicoob400.Duplicata_de_Servi�o:
                        this.Codigo = ed.getCodigoEspecieByEnum(EnumEspecieDocumento_Sicoob400.Duplicata_de_Servi�o);
                        this.Especie = "Duplicata de Servi�o";
                        this.Sigla = "DS";
                        break;
                    case EnumEspecieDocumento_Sicoob400.Recibo:
                        this.Codigo = ed.getCodigoEspecieByEnum(EnumEspecieDocumento_Sicoob400.Recibo);
                        this.Especie = "Recibo";
                        this.Sigla = "R";
                        break;
                    case EnumEspecieDocumento_Sicoob400.Nota_Promiss�ria:
                        this.Codigo = ed.getCodigoEspecieByEnum(EnumEspecieDocumento_Sicoob400.Nota_Promiss�ria);
                        this.Especie = "Nota Promiss�ria";
                        this.Sigla = "NP";
                        break;
                    case EnumEspecieDocumento_Sicoob400.Cheque:
                        this.Codigo = ed.getCodigoEspecieByEnum(EnumEspecieDocumento_Sicoob400.Cheque);
                        this.Especie = "Cheque";
                        this.Sigla = "CH";
                        break;
                    case EnumEspecieDocumento_Sicoob400.Ap�lice_de_Seguro:
                        this.Codigo = ed.getCodigoEspecieByEnum(EnumEspecieDocumento_Sicoob400.Ap�lice_de_Seguro);
                        this.Especie = "Ap�lice de Seguro";
                        this.Sigla = "AS";
                        break;
                    case EnumEspecieDocumento_Sicoob400.Duplicata_Rural:
                        this.Codigo = ed.getCodigoEspecieByEnum(EnumEspecieDocumento_Sicoob400.Duplicata_Rural);
                        this.Especie = "Duplicata Rural";
                        this.Sigla = "DR";
                        break;
                    case EnumEspecieDocumento_Sicoob400.Fatura:
                        this.Codigo = ed.getCodigoEspecieByEnum(EnumEspecieDocumento_Sicoob400.Fatura);
                        this.Especie = "Fatura";
                        this.Sigla = "F";
                        break;
                    case EnumEspecieDocumento_Sicoob400.Letra_de_C�mbio:
                        this.Codigo = ed.getCodigoEspecieByEnum(EnumEspecieDocumento_Sicoob400.Letra_de_C�mbio);
                        this.Especie = "Letra de C�mbio";
                        this.Sigla = "LC";
                        break;
                    case EnumEspecieDocumento_Sicoob400.Mensalidade_Escolar:
                        this.Codigo = ed.getCodigoEspecieByEnum(EnumEspecieDocumento_Sicoob400.Mensalidade_Escolar);
                        this.Especie = "Mensalidade Escolar";
                        this.Sigla = "ME";
                        break;
                    case EnumEspecieDocumento_Sicoob400.Nota_de_D�bito:
                        this.Codigo = ed.getCodigoEspecieByEnum(EnumEspecieDocumento_Sicoob400.Nota_de_D�bito);
                        this.Especie = "Nota de D�bito";
                        this.Sigla = "ND";
                        break;
                    case EnumEspecieDocumento_Sicoob400.Nota_de_Seguro:
                        this.Codigo = ed.getCodigoEspecieByEnum(EnumEspecieDocumento_Sicoob400.Nota_de_Seguro);
                        this.Especie = "Nota de Seguro";
                        this.Sigla = "NS";
                        break;
                    case EnumEspecieDocumento_Sicoob400.Outros:
                        this.Codigo = ed.getCodigoEspecieByEnum(EnumEspecieDocumento_Sicoob400.Outros);
                        this.Especie = "Outros";
                        this.Sigla = "O";
                        break;
                    case EnumEspecieDocumento_Sicoob400.Parcela_de_Cons�rcio:
                        this.Codigo = ed.getCodigoEspecieByEnum(EnumEspecieDocumento_Sicoob400.Parcela_de_Cons�rcio);
                        this.Especie = "Parcela de Cons�rcio";
                        this.Sigla = "PC";
                        break;
                    case EnumEspecieDocumento_Sicoob400.Triplicata_de_Servi�o:
                        this.Codigo = ed.getCodigoEspecieByEnum(EnumEspecieDocumento_Sicoob400.Triplicata_de_Servi�o);
                        this.Especie = "Triplicata de Servi�o";
                        this.Sigla = "TS";
                        break;
                    case EnumEspecieDocumento_Sicoob400.Triplicata_Mercantil:
                        this.Codigo = ed.getCodigoEspecieByEnum(EnumEspecieDocumento_Sicoob400.Triplicata_Mercantil);
                        this.Especie = "Triplicata Mercantil";
                        this.Sigla = "TM";
                        break;
                    case EnumEspecieDocumento_Sicoob400.Warrant:
                        this.Codigo = ed.getCodigoEspecieByEnum(EnumEspecieDocumento_Sicoob400.Warrant);
                        this.Especie = "Warrant";
                        this.Sigla = "W";
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
            EspecieDocumento_Sicoob400 ed = new EspecieDocumento_Sicoob400();
            foreach (EnumEspecieDocumento_Sicoob400 item in Enum.GetValues(typeof(EnumEspecieDocumento_Sicoob400)))
                especiesDocumento.Add(new EspecieDocumento_Sicoob400(ed.getCodigoEspecieByEnum(item)));

            return especiesDocumento;
        }

        #endregion
    }
}
