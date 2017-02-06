using System;
using System.Collections.Generic;
using System.Text;

namespace BoletoNet
{
    #region Enumerado

    public enum EnumCodigoMovimento_Itau
    {
        Entrada_confirmada = 02,
        Entrada_rejeitada = 03,
        Liquidação = 06,
        Liquidação_Em_Cartório = 08,
        Baixa = 09,
        Baixa_Por_Ter_Sido_Liquidado = 10,
    }

    #endregion

    public class CodigoMovimento_Itau : AbstractCodigoMovimento, ICodigoMovimento
    {
        #region Construtores

        public CodigoMovimento_Itau()
        {
            try
            {
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }

        public CodigoMovimento_Itau(int codigo)
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

        private void carregar(int codigo)
        {
            try
            {
                this.Banco = new Banco_Itau();

                switch ((EnumCodigoMovimento_Itau)codigo)
                {
                    case EnumCodigoMovimento_Itau.Entrada_confirmada:
                        this.Codigo = (int)EnumCodigoMovimento_Itau.Entrada_confirmada;
                        this.Descricao = "Entrada confirmada";
                        break;
                    case EnumCodigoMovimento_Itau.Entrada_rejeitada:
                        this.Codigo = (int)EnumCodigoMovimento_Itau.Entrada_rejeitada;
                        this.Descricao = "Entrada rejeitada";
                        break;
                    case EnumCodigoMovimento_Itau.Baixa:
                        this.Codigo = (int)EnumCodigoMovimento_Itau.Baixa;
                        this.Descricao = "Baixa";
                        break;
                    case EnumCodigoMovimento_Itau.Baixa_Por_Ter_Sido_Liquidado:
                        this.Codigo = (int)EnumCodigoMovimento_Itau.Baixa_Por_Ter_Sido_Liquidado;
                        this.Descricao = "Baixa por ter sido liquidado";
                        break;
                    case EnumCodigoMovimento_Itau.Liquidação:
                        this.Codigo = (int)EnumCodigoMovimento_Itau.Liquidação;
                        this.Descricao = "Liquidação";
                        break;
                    case EnumCodigoMovimento_Itau.Liquidação_Em_Cartório:
                        this.Codigo = (int)EnumCodigoMovimento_Itau.Liquidação_Em_Cartório;
                        this.Descricao = "Liquidação em cartório";
                        break;
                    default:
                        this.Codigo = 0;
                        this.Descricao = "";
                        break;
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }

        private void Ler(int codigo)
        {
            try
            {
                switch (codigo)
                {
                    case 2:
                        this.Codigo = (int)EnumCodigoMovimento_Itau.Entrada_confirmada;
                        this.Descricao = "Entrada confirmada";
                        break;
                    case 3:
                        this.Codigo = (int)EnumCodigoMovimento_Itau.Entrada_rejeitada;
                        this.Descricao = "Entrada rejeitada";
                        break;
                    case 9:
                        this.Codigo = (int)EnumCodigoMovimento_Itau.Baixa;
                        this.Descricao = "Baixa";
                        break;
                    case 10:
                        this.Codigo = (int)EnumCodigoMovimento_Itau.Baixa_Por_Ter_Sido_Liquidado;
                        this.Descricao = "Baixa por ter sido liquidado";
                        break;
                    case 6:
                        this.Codigo = (int)EnumCodigoMovimento_Itau.Liquidação;
                        this.Descricao = "Liquidação";
                        break;
                    case 8:
                        this.Codigo = (int)EnumCodigoMovimento_Itau.Liquidação_Em_Cartório;
                        this.Descricao = "Liquidação em cartório";
                        break;
                    default:
                        this.Codigo = 0;
                        this.Descricao = "";
                        break;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }


        #endregion
    }
}
