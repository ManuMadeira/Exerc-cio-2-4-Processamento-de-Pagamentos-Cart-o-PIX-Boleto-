public sealed class PagamentoPix : Pagamento
{
    protected override void Validar()
    {
        Console.WriteLine("Validando chave PIX...");
    }

    protected override string AutorizarOuCapturar(decimal valor)
    {
        return "PIX: QR Code gerado para R$ " + valor;
    }

    protected override string Confirmar()
    {
        return "Pagamento PIX confirmado - TXID: PIX789";
    }
}
