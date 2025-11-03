public sealed class PagamentoBoleto : Pagamento
{
    protected override void Validar()
    {
        Console.WriteLine("Validando dados do boleto...");
    }

    protected override string AutorizarOuCapturar(decimal valor)
    {
        return "Boleto: linha digitável gerada para R$ " + valor;
    }

    protected override string Confirmar()
    {
        return "Boleto aguardando compensação";
    }
}
