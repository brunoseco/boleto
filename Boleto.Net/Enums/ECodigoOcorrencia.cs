namespace BoletoNet
{
    internal enum ECodigoOcorrenciaCaixa240
    {
        //Ocorrências para arquivo retorno
        Entrada_Confirmada = 02,
        Baixa = 09,
        Liquidação = 06,
        Rejeição = 03,
    }

    internal enum ECodigoOcorrenciaCaixa400
    {
        //Ocorrências para arquivo retorno
        Entrada_Confirmada = 01,
        Baixa = 02,
        Baixa_por_Devolução = 23,
        Baixa_por_Franco_Pagamento  = 24,
        Baixa_por_Protesto = 25,
        Liquidação = 21,
        Liquidação_em_Cartório = 22,
        Rejeição = 99,
    }

    internal enum ECodigoOcorrenciaBancoBrasil240
    {
        //Ocorrências para arquivo retorno
        Entrada_Confirmada = 01,
        Baixa = 02,
        Liquidação = 21,
        Rejeição = 99,
    }

    internal enum ECodigoOcorrenciaBancoBrasil400
    {
        //Ocorrências para arquivo retorno
        Entrada_Confirmada = 02,
        Rejeição = 03,
        Liquidação_Sem_Registro = 05,
        Liquidação = 06,
        Liquidação_Parcial = 07,
        Liquidação_Por_Saldo = 08,
        Liquidação_Em_Cartório = 15,
        Baixa = 09,
    }

    internal enum ECodigoOcorrenciaItau400
    {
        //Ocorrências para arquivo retorno
        Entrada_Confirmada = 02,
        Rejeição = 03,
        Liquidação = 06,
        Liquidação_Parcial = 07,
        Liquidação_Em_Cartório = 08,
        Baixa = 09,
    }

    internal enum ECodigoOcorrenciaSantander400
    {
        //Ocorrências para arquivo retorno
        Entrada_Confirmada = 02,
        Rejeição = 03,
        Liquidação = 06,
        Liquidação_Parcial = 07,
        Liquidação_Por_Saldo = 08,
        Liquidação_Em_Cartório = 17,
        Baixa = 09,
    }

    internal enum ECodigoOcorrenciaHSBC400
    {
        //Ocorrências para arquivo retorno
        Entrada_Confirmada = 02,
        Rejeição = 03,
        Liquidação = 06,
        Liquidação_Por_Conta_Em_Dinheiro = 07,
        Liquidação_Por_Saldo = 08,
        Liquidação_Em_Cartório_Em_Dinheiro = 15,
        Liquidação_Baixado_Devolvido_Em_Data_Anterior_Dinheiro = 16,
        Liquidação_Em_Cartório = 17,
        Liquidação_Em_Cartório_Em_Cheque = 32,
        Liquidação_Por_Conta_Em_Cheque = 33,
        Liquidação_Baixado_Devolvido_Em_Data_Anterior_Cheque = 36,
        Liquidação_De_Título_Não_Registrado_Em_Dinheiro = 38,
        Liquidação_De_Título_Não_Registrado_Em_Cheque = 39,
        Baixa = 09,
        Baixado_Conforme_Instruções = 10,
    }

    internal enum ECodigoOcorrenciaBradesco400
    {
        //Ocorrências para arquivo retorno
        Entrada_Confirmada = 02,
        Rejeição = 03,
        Liquidação = 06,
        Liquidação_após_baixa_ou_Título_não_registrado = 17,
        Liquidação_Em_Cartório = 15,
        Baixa = 09,
        Baixa_Via_Agência = 10,
    }

    internal enum ECodigoOcorrenciaSicoob400
    {
        //Ocorrências para arquivo retorno
        Entrada_Confirmada = 02,
        Rejeição = 03,
        Liquidação_Sem_Registro = 05,
        Liquidação = 06,
        Liquidação_Em_Cartório = 15,
        Baixa = 09,
    }
}
