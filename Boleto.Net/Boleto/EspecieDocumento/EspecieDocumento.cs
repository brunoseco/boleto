using System;
using System.Collections.Generic;
using System.Text;

namespace BoletoNet
{
    public class EspecieDocumento : AbstractEspecieDocumento, IEspecieDocumento
    {

        #region Variaveis

        private IEspecieDocumento _IEspecieDocumento;

        #endregion

        #region Construtores

        internal EspecieDocumento()
        {
        }

        public EspecieDocumento(int CodigoBanco)
        {
            try
            {
                InstanciaEspecieDocumento(CodigoBanco, "0", null);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao instanciar objeto.", ex);
            }
        }

        public EspecieDocumento(int CodigoBanco, int TipoRemessa)
        {
            try
            {
                InstanciaEspecieDocumento(CodigoBanco, "0", TipoRemessa);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao instanciar objeto.", ex);
            }
        }

        public EspecieDocumento(int CodigoBanco, string codigoEspecie)
        {
            try
            {
                InstanciaEspecieDocumento(CodigoBanco, codigoEspecie, null);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao instanciar objeto.", ex);
            }
        }

        public EspecieDocumento(int CodigoBanco, string codigoEspecie, int TipoRemessa)
        {
            try
            {
                InstanciaEspecieDocumento(CodigoBanco, codigoEspecie, TipoRemessa);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao instanciar objeto.", ex);
            }
        }

        #endregion

        #region Propriedades da interface

        public override IBanco Banco
        {
            get { return _IEspecieDocumento.Banco; }
            set { _IEspecieDocumento.Banco = value; }
        }

        public override string Codigo
        {
            get { return _IEspecieDocumento.Codigo; }
            set { _IEspecieDocumento.Codigo = value; }
        }

        public override string Sigla
        {
            get { return _IEspecieDocumento.Sigla; }
            set { _IEspecieDocumento.Sigla = value; }
        }

        public override string Especie
        {
            get { return _IEspecieDocumento.Especie; }
            set { _IEspecieDocumento.Especie = value; }
        }

        #endregion

        # region M�todos Privados

        private void InstanciaEspecieDocumento(int codigoBanco, string codigoEspecie, int? TipoRemessa)
        {
            try
            {
                switch (codigoBanco)
                {
                    //341 - Ita�
                    case 341:
                        _IEspecieDocumento = new EspecieDocumento_Itau(codigoEspecie);
                        break;
                    //356 - BankBoston
                    case 479:
                        _IEspecieDocumento = new EspecieDocumento_BankBoston(codigoEspecie);
                        break;
                    //422 - Safra
                    case 1:
                        {
                            if (TipoRemessa.HasValue)
                                if (TipoRemessa.Value.Equals(400))
                                    _IEspecieDocumento = new EspecieDocumento_BancoBrasil400(codigoEspecie);
                                else
                                    _IEspecieDocumento = new EspecieDocumento_BancoBrasil240(codigoEspecie);
                            else
                                _IEspecieDocumento = new EspecieDocumento_BancoBrasil400(codigoEspecie);
                            break;
                        }
                    //237 - Bradesco
                    case 237:
                        {
                            if (TipoRemessa.HasValue)
                                if (TipoRemessa.Value.Equals(400))
                                    _IEspecieDocumento = new EspecieDocumento_Bradesco400(codigoEspecie);
                                else
                                    _IEspecieDocumento = new EspecieDocumento_Bradesco240(codigoEspecie);
                            else
                                _IEspecieDocumento = new EspecieDocumento_Bradesco400(codigoEspecie);
                            break;
                        }
                    case 356:
                        _IEspecieDocumento = new EspecieDocumento_Real(codigoEspecie);
                        break;
                    //033 - Santander
                    case 33:
                        {
                            if (TipoRemessa.HasValue)
                                if (TipoRemessa.Value.Equals(400))
                                    _IEspecieDocumento = new EspecieDocumento_Santander400(codigoEspecie);
                                else
                                    _IEspecieDocumento = new EspecieDocumento_Santander240(codigoEspecie);
                            else
                                _IEspecieDocumento = new EspecieDocumento_Santander400(codigoEspecie);
                            break;
                        }
                    case 347:
                        _IEspecieDocumento = new EspecieDocumento_Sudameris(codigoEspecie);
                        break;
                    //104 - Caixa
                    case 104:
                        {
                            if (TipoRemessa.HasValue)
                                if (TipoRemessa.Value.Equals(400))
                                    _IEspecieDocumento = new EspecieDocumento_Caixa400(codigoEspecie);
                                else
                                    _IEspecieDocumento = new EspecieDocumento_Caixa240(codigoEspecie);
                            else
                                _IEspecieDocumento = new EspecieDocumento_Caixa400(codigoEspecie);
                            break;
                        }
                    //399 - HSBC
                    case 399:
                        {
                            if (TipoRemessa.HasValue)
                                if (TipoRemessa.Value.Equals(400))
                                    _IEspecieDocumento = new EspecieDocumento_HSBC400(codigoEspecie);
                                else
                                    _IEspecieDocumento = new EspecieDocumento_HSBC240(codigoEspecie);
                            else
                                _IEspecieDocumento = new EspecieDocumento_HSBC400(codigoEspecie);
                            break;
                        }
                    //748 - Sicredi
                    case 748:
                        _IEspecieDocumento = new EspecieDocumento_Sicredi(codigoEspecie);
                        break;
                    //756 - Sicoob
                    case 756:
                        {
                            if (TipoRemessa.HasValue)
                                if (TipoRemessa.Value.Equals(400))
                                    _IEspecieDocumento = new EspecieDocumento_Sicoob400(codigoEspecie);
                                else
                                    _IEspecieDocumento = new EspecieDocumento_Sicoob240(codigoEspecie);
                            else
                                _IEspecieDocumento = new EspecieDocumento_Sicoob400(codigoEspecie);
                            break;
                        }
                    //41 - Banrisul - sidneiklein
                    case 41:
                        _IEspecieDocumento = new EspecieDocumento_Banrisul(codigoEspecie);
                        break;
                    default:
                        throw new Exception("C�digo do banco n�o implementando: " + codigoBanco);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a execu��o da transa��o.", ex);
            }
        }

        public EspeciesDocumento CarregaTodas(int codigoBanco)
        {
            return this.CarregaTodas(codigoBanco, null);
        }

        public EspeciesDocumento CarregaTodas(int codigoBanco, int? TipoRemessa)
        {
            try
            {

                switch (codigoBanco)
                {
                    case 1:
                        {
                            if (TipoRemessa.HasValue)
                                if (TipoRemessa.Value.Equals(400))
                                    return EspecieDocumento_BancoBrasil400.CarregaTodas();
                                else
                                    return EspecieDocumento_BancoBrasil240.CarregaTodas();
                            else
                                return EspecieDocumento_BancoBrasil400.CarregaTodas();
                        }
                    case 33:
                        {
                            if (TipoRemessa.HasValue)
                                if (TipoRemessa.Value.Equals(400))
                                    return EspecieDocumento_Santander400.CarregaTodas();
                                else
                                    return EspecieDocumento_Santander240.CarregaTodas();
                            else
                                return EspecieDocumento_Santander400.CarregaTodas();
                        }
                    case 237:
                        {
                            if (TipoRemessa.HasValue)
                                if (TipoRemessa.Value.Equals(400))
                                    return EspecieDocumento_Bradesco400.CarregaTodas();
                                else
                                    return EspecieDocumento_Bradesco240.CarregaTodas();
                            else
                                return EspecieDocumento_Bradesco400.CarregaTodas();
                        }
                    case 341:
                        return EspecieDocumento_Itau.CarregaTodas();
                    case 356:
                        return EspecieDocumento_Itau.CarregaTodas();
                    case 104:
                        {
                            if (TipoRemessa.HasValue)
                                if (TipoRemessa.Value.Equals(400))
                                    return EspecieDocumento_Caixa400.CarregaTodas();
                                else
                                    return EspecieDocumento_Caixa240.CarregaTodas();
                            else
                                return EspecieDocumento_Caixa400.CarregaTodas();
                        }
                    case 399:
                        {
                            if (TipoRemessa.HasValue)
                                if (TipoRemessa.Value.Equals(400))
                                    return EspecieDocumento_HSBC400.CarregaTodas();
                                else
                                    return EspecieDocumento_HSBC240.CarregaTodas();
                            else
                                return EspecieDocumento_HSBC400.CarregaTodas();
                        }
                    case 756:
                        {
                            if (TipoRemessa.HasValue)
                                if (TipoRemessa.Value.Equals(400))
                                    return EspecieDocumento_Sicoob400.CarregaTodas();
                                else
                                    return EspecieDocumento_Sicoob240.CarregaTodas();
                            else
                                return EspecieDocumento_Sicoob400.CarregaTodas();
                        }
                    case 748:
                        return EspecieDocumento_Sicredi.CarregaTodas();
                    default:
                        throw new Exception("Esp�cies do Documento n�o implementado para o banco : " + codigoBanco);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao listar objetos", ex);
            }
        }

        # endregion

        public static string ValidaSigla(IEspecieDocumento especie)
        {
            try
            {
                return especie.Sigla;
            }
            catch
            {
                return string.Empty;
            }
        }

        public static string ValidaCodigo(IEspecieDocumento especie)
        {
            try
            {
                return especie.Codigo;
            }
            catch
            {
                return "0";
            }
        }
    }
}
