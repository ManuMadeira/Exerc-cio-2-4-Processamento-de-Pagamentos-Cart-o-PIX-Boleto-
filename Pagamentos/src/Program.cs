using System;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== DEMONSTRAÇÃO LSP E COMPOSIÇÃO ===\n");

        Console.WriteLine("1. TESTE LSP - Substituição Transparente");
        TestarLSP();

        Console.WriteLine("\n2. TESTE COMPOSIÇÃO - Troca de Peças");
        TestarComposicao();

        Console.WriteLine("\n3. TESTE ANTIFRAUDE - Cenário de Reprovação");
        TestarAntifraudeReprova();
    }

    static void TestarLSP()
    {
        Pagamento[] pagamentos = {
            new PagamentoCartao(),
            new PagamentoPix(),
            new PagamentoBoleto()
        };

        foreach (var pagamento in pagamentos)
        {
            Console.WriteLine($"Tipo: {pagamento.GetType().Name}");
            Console.WriteLine($"Resultado: {pagamento.Processar()}");
            Console.WriteLine("---");
        }
    }

    static void TestarComposicao()
    {
        var pagamentoInternacional = new PagamentoCartao
        {
            Antifraude = PoliticasPagamento.AntifraudeAprovada,
            Cambio = PoliticasPagamento.CambioUSD
        };

        Console.WriteLine("Pagamento Internacional com conversão USD:");
        Console.WriteLine(pagamentoInternacional.Processar());
    }

    static void TestarAntifraudeReprova()
    {
        var pagamentoAltoValor = new PagamentoPix
        {
            Antifraude = PoliticasPagamento.AntifraudeRestritiva, // Só aprova até R$ 500
            Cambio = PoliticasPagamento.CambioSemConversao
        };

        Console.WriteLine("Pagamento de alto valor com antifraude restritiva:");
        Console.WriteLine(pagamentoAltoValor.Processar());
    }
}
