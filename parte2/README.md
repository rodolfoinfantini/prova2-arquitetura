# Sistema de Gerenciamento de Pedidos (Design Patterns)

## Integrantes do Grupo
*   **Nome:** Rodolfo Infantini Carneiro | **RA:** 23000993
*   **Nome:** Marcella Pereira de Brito | **RA:** 23008631
*   **Nome:** David dos Santos Moreno | **RA:** 23022082

Este projeto é uma API REST desenvolvida em C# (.NET) com o objetivo de demonstrar a aplicação prática de múltiplos Padrões de Projeto (Design Patterns) em um cenário de E-commerce.

A aplicação simula um backend para criação e gerenciamento de pedidos, permitindo seleção de produtos, cálculo de frete, processamento de pagamentos e fluxo de estados do pedido.

## Instruções de Execução

1. Navegue até a pasta raiz do projeto via terminal (**pasta que este README se encontra**):
   ```bash
   cd parte2
   ```

### Executando com Docker

1. Construa a imagem Docker:
   ```bash
   docker build -t rodolfo-marcella-david .
   ```

2. Execute o container na porta 5000:
   ```bash
   docker run -p 5000:5000 rodolfo-marcella-david
   ```

### Ou execute pelo dotnet cli.

1. Instale o [.NET SDK](https://dotnet.microsoft.com/download)

2. Execute o projeto:
   ```bash
   dotnet run --project Efc2/Efc2.csproj
   ```

3. Ou então rode os testes unitários:
   ```bash
   dotnet test Efc2.Tests/Efc2.Tests.csproj --logger "console;verbosity=detailed"
   ```

### Acesse a documentação (Swagger) para testar os endpoints:
   - Disponível em: [http://localhost:5000/swagger](http://localhost:5000/swagger)
   - Olhar os logs no terminal da aplicação.

---

## Padrões de Projeto Implementados

O projeto incorpora **6 padrões de projeto**, divididos entre criacionais e comportamentais. Abaixo está a descrição de cada um e onde encontrá-los no código.

### 1. Singleton
Utilizado para garantir que o banco de dados em memória (`InMemoryOrderDatabase`) tenha apenas uma instância durante todo o ciclo de vida da aplicação, preservando os dados entre as requisições HTTP.
- **Localização:** `Program.cs` (Configuração do container de DI) e `Database/InMemoryOrderDatabase.cs`.
- **Trecho:** `builder.Services.AddSingleton<IDatabase<...>, ...>();`

### 2. Builder
Utilizado para simplificar a criação de objetos complexos da classe `Order`. Ele encapsula a lógica de configuração do pedido (atribuir cliente, produtos, selecionar estratégias) passo a passo.
- **Localização:** `Patterns/Builder/OrderBuilder.cs`
- **Uso:** No `OrdersController`, para converter o DTO de entrada na entidade `Order`.

### 3. Factory
Utilizado para encapsular a lógica de instanciação dos métodos de pagamento. A fábrica decide qual classe concreta (`CreditCardPayment` ou `PixPayment`) criar com base em uma string identificadora.
- **Localização:** `Patterns/Factory/PaymentFactory.cs`
- **Uso:** Chamado internamente pelo `OrderBuilder` ao definir o método de pagamento.

### 4. Strategy
Utilizado para o cálculo de frete. Permite que o algoritmo de cálculo (Sedex, Normal, Grátis) seja trocado dinamicamente sem alterar a classe do pedido.
- **Localização:** `Patterns/Strategy/IShippingStrategy.cs` (e suas implementações na mesma pasta).
- **Uso:** A classe `Order` possui uma propriedade `IShippingStrategy`.

### 5. State
Utilizado para gerenciar o fluxo de vida do pedido (Novo -> Pago -> Enviado -> Cancelado). Cada estado é uma classe que define quais comportamentos são permitidos, eliminando grandes estruturas condicionais (`if/else` ou `switch`).
- **Localização:** `Patterns/State/OrderState.cs` (e implementações como `NewState`, `PaidState`).
- **Uso:** A classe `Order` delega ações como `Pay()`, `Ship()` e `Cancel()` para o estado atual.

### 6. Observer
Utilizado para o sistema de notificações. O pedido atua como "Sujeito" e notifica observadores (como Email ou Whatsapp) sempre que ocorre uma mudança de estado relevante.
- **Localização:** `Patterns/Observer/IObserver.cs` e `Models/Order.cs` (implementação de `ISubject`).
- **Uso:** Ao transicionar de estado, o método `Notify()` dispara atualizações para os observadores registrados.

---

## Estrutura de Pastas Relevante

```
Efc2/
├── Controllers/       # Pontos de entrada da API
├── Database/          # Singleton do banco em memória
├── Models/            # Entidades de domínio (Order, Product)
├── Patterns/          # Implementações dos Design Patterns
│   ├── Builder/
│   ├── Factory/
│   ├── Observer/
│   ├── State/
│   └── Strategy/
└── Program.cs         # Configuração da aplicação e Injeção de Dependência
```


