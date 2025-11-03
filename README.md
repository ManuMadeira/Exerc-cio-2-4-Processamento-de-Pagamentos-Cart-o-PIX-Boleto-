# Processamento de Pagamentos
Emmanuelly Madeira 

## Como Executar
```bash
cd src && dotnet run
cd ../tests && dotnet test
```

## Herança vs Composição
**Herança** especializa o ritual fixo de pagamento (Cartão/PIX/Boleto). **Composição** com delegates troca políticas independentes (antifraude/câmbio) sem criar subclasses.
