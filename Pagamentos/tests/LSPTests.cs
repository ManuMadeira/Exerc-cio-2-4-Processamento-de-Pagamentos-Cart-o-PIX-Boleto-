using Xunit;

namespace Pagamentos.Tests
{
    public class LSPTests
    {
        [Fact]
        public void TesteLSP_SubstituicaoTransparente()
        {
            // Arrange - Cliente aceita qualquer Pagamento
            Pagamento[] pagamentos = {
                new PagamentoCartao(),
                new PagamentoPix(),
                new PagamentoBoleto()
            };

            // Act & Assert - Deve funcionar sem exceções para todos
            foreach (var pagamento in pagamentos)
            {
                var resultado = pagamento.Processar();
                
                // Assert - Verifica comportamento consistente
                Assert.NotNull(resultado);
                Assert.NotEmpty(resultado);
                Assert.DoesNotContain("Exception", resultado);
                Assert.DoesNotContain("erro", resultado.ToLower());
            }
        }

        [Fact]
        public void TesteLSP_ProcessarRetornaStringValida()
        {
            // Arrange
            Pagamento pagamento = new PagamentoCartao();

            // Act
            var resultado = pagamento.Processar();

            // Assert
            Assert.NotNull(resultado);
            Assert.IsType<string>(resultado);
            Assert.True(resultado.Length > 10);
        }

        [Fact]
        public void TesteLSP_MesmoRitualParaTodos()
        {
            // Arrange
            var cartao = new PagamentoCartao();
            var pix = new PagamentoPix();
            var boleto = new PagamentoBoleto();

            // Act
            var resultadoCartao = cartao.Processar();
            var resultadoPix = pix.Processar();
            var resultadoBoleto = boleto.Processar();

            // Assert - Todos seguem o mesmo ritual
            Assert.Contains("autorizado", resultadoCartao.ToLower());
            Assert.Contains("confirmado", resultadoCartao.ToLower());
            Assert.Contains("pix", resultadoPix.ToLower());
            Assert.Contains("boleto", resultadoBoleto.ToLower());
        }
    }
}
