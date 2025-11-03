public sealed class PagamentoCartao : Pagamento
{
    protected override void Validar()
    {
        Console.WriteLine("Validando dados do cartão...");
    }

    protected override string AutorizarOuCapturar(decimal valor)
    {
        return "Cartão: valor R$ " + valor + " autorizado e capturado - Código: CARD123";
    }

    protected override string Confirmar()
    {
        return "Pagamento com cartão confirmado";
    }
}
