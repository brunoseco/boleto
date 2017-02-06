namespace BoletoNet
{
    internal enum ECodigoOcorrenciaCaixa240
    {
        //Ocorr�ncias para arquivo retorno
        Entrada_Confirmada = 02,
        Baixa = 09,
        Liquida��o = 06,
        Rejei��o = 03,
    }

    internal enum ECodigoOcorrenciaCaixa400
    {
        //Ocorr�ncias para arquivo retorno
        Entrada_Confirmada = 01,
        Baixa = 02,
        Baixa_por_Devolu��o = 23,
        Baixa_por_Franco_Pagamento  = 24,
        Baixa_por_Protesto = 25,
        Liquida��o = 21,
        Liquida��o_em_Cart�rio = 22,
        Rejei��o = 99,
    }

    internal enum ECodigoOcorrenciaBancoBrasil240
    {
        //Ocorr�ncias para arquivo retorno
        Entrada_Confirmada = 01,
        Baixa = 02,
        Liquida��o = 21,
        Rejei��o = 99,
    }

    internal enum ECodigoOcorrenciaBancoBrasil400
    {
        //Ocorr�ncias para arquivo retorno
        Entrada_Confirmada = 02,
        Rejei��o = 03,
        Liquida��o_Sem_Registro = 05,
        Liquida��o = 06,
        Liquida��o_Parcial = 07,
        Liquida��o_Por_Saldo = 08,
        Liquida��o_Em_Cart�rio = 15,
        Baixa = 09,
    }

    internal enum ECodigoOcorrenciaItau400
    {
        //Ocorr�ncias para arquivo retorno
        Entrada_Confirmada = 02,
        Rejei��o = 03,
        Liquida��o = 06,
        Liquida��o_Parcial = 07,
        Liquida��o_Em_Cart�rio = 08,
        Baixa = 09,
    }

    internal enum ECodigoOcorrenciaSantander400
    {
        //Ocorr�ncias para arquivo retorno
        Entrada_Confirmada = 02,
        Rejei��o = 03,
        Liquida��o = 06,
        Liquida��o_Parcial = 07,
        Liquida��o_Por_Saldo = 08,
        Liquida��o_Em_Cart�rio = 17,
        Baixa = 09,
    }

    internal enum ECodigoOcorrenciaHSBC400
    {
        //Ocorr�ncias para arquivo retorno
        Entrada_Confirmada = 02,
        Rejei��o = 03,
        Liquida��o = 06,
        Liquida��o_Por_Conta_Em_Dinheiro = 07,
        Liquida��o_Por_Saldo = 08,
        Liquida��o_Em_Cart�rio_Em_Dinheiro = 15,
        Liquida��o_Baixado_Devolvido_Em_Data_Anterior_Dinheiro = 16,
        Liquida��o_Em_Cart�rio = 17,
        Liquida��o_Em_Cart�rio_Em_Cheque = 32,
        Liquida��o_Por_Conta_Em_Cheque = 33,
        Liquida��o_Baixado_Devolvido_Em_Data_Anterior_Cheque = 36,
        Liquida��o_De_T�tulo_N�o_Registrado_Em_Dinheiro = 38,
        Liquida��o_De_T�tulo_N�o_Registrado_Em_Cheque = 39,
        Baixa = 09,
        Baixado_Conforme_Instru��es = 10,
    }

    internal enum ECodigoOcorrenciaBradesco400
    {
        //Ocorr�ncias para arquivo retorno
        Entrada_Confirmada = 02,
        Rejei��o = 03,
        Liquida��o = 06,
        Liquida��o_ap�s_baixa_ou_T�tulo_n�o_registrado = 17,
        Liquida��o_Em_Cart�rio = 15,
        Baixa = 09,
        Baixa_Via_Ag�ncia = 10,
    }

    internal enum ECodigoOcorrenciaSicoob400
    {
        //Ocorr�ncias para arquivo retorno
        Entrada_Confirmada = 02,
        Rejei��o = 03,
        Liquida��o_Sem_Registro = 05,
        Liquida��o = 06,
        Liquida��o_Em_Cart�rio = 15,
        Baixa = 09,
    }
}
