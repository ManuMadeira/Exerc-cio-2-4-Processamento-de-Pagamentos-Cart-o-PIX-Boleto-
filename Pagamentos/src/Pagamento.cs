using System;

public class Pagamento
{
    public Func<decimal, bool>? Antifraude { get; set; }
    public Func<decimal, decimal>? Cambio { get; set; }

    public string Processar()
    {
        Validar();
        
        var valor = ObterValor();
        
        // Aplica política de antifraude
        if (Antifraude?.Invoke(valor) == false)
            return "Pagamento reprovado pela política de antifraude";
            
        // Aplica política de câmbio
        var valorFinal = Cambio?.Invoke(valor) ?? valor;
        
        var autorizacao = AutorizarOuCapturar(valorFinal);
        var confirmacao = Confirmar();
        
        return autorizacao + " | " + confirmacao;
    }

    protected virtual void Validar() { }
    protected virtual string AutorizarOuCapturar(decimal valor) => "Valor R$ " + valor + " autorizado";
    protected virtual string Confirmar() => "Pagamento confirmado";
    protected virtual decimal ObterValor() => 1000m; // Aumentado para 1000 para testar antifraude
}
