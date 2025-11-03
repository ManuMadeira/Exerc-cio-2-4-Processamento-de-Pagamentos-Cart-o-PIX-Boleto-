public static class PoliticasPagamento
{
    // Estratégias de Antifraude
    public static bool AntifraudeAprovada(decimal valor) => valor <= 1000m;
    public static bool AntifraudeRestritiva(decimal valor) => valor <= 500m;
    public static bool AntifraudePermissiva(decimal valor) => valor <= 5000m;
    
    // Estratégias de Câmbio
    public static decimal CambioUSD(decimal valor) => valor * 5.50m;
    public static decimal CambioEUR(decimal valor) => valor * 6.0m;
    public static decimal CambioSemConversao(decimal valor) => valor;
}
