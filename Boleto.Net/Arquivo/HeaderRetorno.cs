using System;
using System.Collections.Generic;
using System.Text;

namespace BoletoNet
{
    public class HeaderRetorno
    {

        #region Variáveis

        private string _registroHeader;
        private int _agencia;
        private int _digitoAgencia;
        private int _contaCorrente;
        private int _digitoContaCorrente;
        private int _codigoCedente; 

        public string RegistroHeader
        {
          get { return _registroHeader; }
          set { _registroHeader = value; }
        }

        public int Agencia
        {
            get { return _agencia; }
            set { _agencia = value; }
        }

        public int DigitoAgencia
        {
            get { return _digitoAgencia; }
            set { _digitoAgencia = value; }
        }

        public int ContaCorrente
        {
            get { return _contaCorrente; }
            set { _contaCorrente = value; }
        }

        public int DigitoContaCorrente
        {
            get { return _digitoContaCorrente; }
            set { _digitoContaCorrente = value; }
        }

        public int CodigoCedente
        {
            get { return _codigoCedente; }
            set { _codigoCedente = value; }
        }
        #endregion

        #region Construtores

        public HeaderRetorno()
        {
        }

        public HeaderRetorno(string registro)
        {
            _registroHeader = registro;
        }

        #endregion

        #region Métodos de Instância

        public void LerHeaderRetornoCNAB400(string registro)
        {
            try
            {
                _registroHeader = registro;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao ler detalhe do arquivo de RETORNO / CNAB 400.", ex);
            }
        }

        #endregion

    }
}
