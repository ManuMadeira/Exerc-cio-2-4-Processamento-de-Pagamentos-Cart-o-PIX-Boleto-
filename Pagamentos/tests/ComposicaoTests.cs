using Xunit;

namespace Pagamentos.Tests
{
    public class ComposicaoTests
    {
        [Fact]
        public void TesteComposicao_TrocaDePecasAntifraude()
        {
            // Arrange - Testa diferentes políticas de antifraude
            var pagamentoRestritivo = new PagamentoCartao
            {
                Antifraude = PoliticasPagamento.AntifraudeRestritiva, // Só aprova até R$ 500
                Cambio = PoliticasPagamento.CambioSemConversao
            };

            var pagamentoPermissivo = new PagamentoCartao
            {
                Antifraude = PoliticasPagamento.AntifraudePermissiva, // Aprova até R$ 5000
                Cambio = PoliticasPagamento.CambioSemConversao
            };

            // Act & Assert - Ambos devem processar sem erro
            var resultado1 = pagamentoRestritivo.Processar();
            var resultado2 = pagamentoPermissivo.Processar();

            Assert.NotNull(resultado1);
            Assert.NotNull(resultado2);
        }

        [Fact]
        public void TesteComposicao_TrocaDePecasCambio()
        {
            // Arrange - Testa diferentes políticas de câmbio
            var pagamentoUSD = new PagamentoPix
            {
                Antifraude = PoliticasPagamento.AntifraudeAprovada,
                Cambio = PoliticasPagamento.CambioUSD // Converte para dólar
            };

            var pagamentoEUR = new PagamentoPix
            {
                Antifraude = PoliticasPagamento.AntifraudeAprovada,
                Cambio = PoliticasPagamento.CambioEUR // Converte para euro
            };

            // Act
            var resultadoUSD = pagamentoUSD.Processar();
            var resultadoEUR = pagamentoEUR.Processar();

            // Assert - Ambos processam com sucesso
            Assert.NotNull(resultadoUSD);
            Assert.NotNull(resultadoEUR);
            Assert.Contains("pix", resultadoUSD.ToLower());
            Assert.Contains("pix", resultadoEUR.ToLower());
        }

        [Fact]
        public void TesteComposicao_AntifraudeReprovaValorAlto()
        {
            // Arrange - Valor alto com antifraude restritiva
            var pagamento = new PagamentoBoleto
            {
                Antifraude = PoliticasPagamento.AntifraudeRestritiva, // Só aprova até R$ 500
                Cambio = PoliticasPagamento.CambioSemConversao
            };

            // Act
            var resultado = pagamento.Processar();

            // Assert - Deve ser reprovado por antifraude (valor padrão é 1000)
            Assert.Contains("reprovado", resultado.ToLower());
            Assert.Contains("antifraude", resultado.ToLower());
        }

        [Fact]
        public void TesteComposicao_AntifraudeAprovaValorBaixo()
        {
            // Arrange - Cria um pagamento com valor baixo
            var pagamento = new PagamentoCartaoBaixoValor
            {
                Antifraude = PoliticasPagamento.AntifraudeRestritiva, // Só aprova até R$ 500
                Cambio = PoliticasPagamento.CambioSemConversao
            };

            // Act
            var resultado = pagamento.Processar();

            // Assert - Deve ser aprovado (valor é 100)
            Assert.DoesNotContain("reprovado", resultado.ToLower());
            Assert.Contains("cartão", resultado.ToLower());
        }

        [Fact]
        public void TesteComposicao_MultiplasCombinacoes()
        {
            // Arrange - Combina diferentes políticas usando tuplas
            var combinacoes = new (Func<decimal, bool> Antifraude, Func<decimal, decimal> Cambio)[]
            {
                (PoliticasPagamento.AntifraudeAprovada, PoliticasPagamento.CambioUSD),        // Aprova (1000 <= 1000)
                (PoliticasPagamento.AntifraudePermissiva, PoliticasPagamento.CambioSemConversao) // Aprova (1000 <= 5000)
                // Removida a combinação restritiva que causa reprovação
            };

            // Act & Assert - Todas as combinações devem funcionar
            foreach (var combo in combinacoes)
            {
                var pagamento = new PagamentoPix
                {
                    Antifraude = combo.Antifraude,
                    Cambio = combo.Cambio
                };

                var resultado = pagamento.Processar();
                Assert.NotNull(resultado);
                Assert.Contains("pix", resultado.ToLower());
            }
        }

        [Fact]
        public void TesteComposicao_CombinacaoReprovada()
        {
            // Arrange - Combinação que deve ser reprovada
            var pagamento = new PagamentoPix
            {
                Antifraude = PoliticasPagamento.AntifraudeRestritiva, // Reprove (1000 > 500)
                Cambio = PoliticasPagamento.CambioEUR
            };

            // Act
            var resultado = pagamento.Processar();

            // Assert - Deve ser reprovado
            Assert.Contains("reprovado", resultado.ToLower());
        }
    }

    // Classe auxiliar para teste de valor baixo
    public sealed class PagamentoCartaoBaixoValor : Pagamento
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

        protected override decimal ObterValor() => 100m; // Valor baixo para teste
    }
}
