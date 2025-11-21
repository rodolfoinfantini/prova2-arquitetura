# Resumo Teórico e Justificativas Arquiteturais

## Integrantes do Grupo
*   **Nome:** Rodolfo Infantini Carneiro | **RA:** 23000993
*   **Nome:** Marcella Pereira de Brito | **RA:** 23008631
*   **Nome:** David dos Santos Moreno | **RA:** 23022082

Este documento apresenta o embasamento teórico dos padrões de projeto utilizados no sistema de gerenciamento de pedidos, bem como uma análise crítica da aplicação prática de cada um, detalhando os problemas resolvidos e os benefícios obtidos.
Escolhemos 6 padrões de projeto para implementar nesse sistema: Singleton, Builder, Factory, Strategy, State e Observer.

---

## 1. Estudo Teórico dos Padrões Escolhidos

### 1.1. Singleton (Criacional)
*   **Descrição:** O Singleton garante que uma classe tenha apenas uma única instância e fornece um ponto global de acesso a ela. É fundamental para recursos compartilhados.
*   **Iterações e Variações:**
    *   **Lazy vs Eager:** Pode ser instanciado no momento da declaração (Eager) ou apenas quando solicitado pela primeira vez (Lazy).
    *   **Thread-Safety:** Em ambientes multithread, é necessário usar mecanismos de bloqueio (`Mutex` ou `Semaphore`) para evitar a criação de múltiplas instâncias concorrentes.
    *   **Injeção de Dependência (DI):** Em frameworks modernos como .NET, o Singleton é frequentemente gerenciado pelo container de DI (ciclo de vida "Singleton"), em vez de uma implementação estática manual dentro da classe.
*   **Comparação:** Diferente de uma classe puramente estática, um Singleton pode implementar interfaces e ser passado como parâmetro, permitindo maior flexibilidade e testes (mocking).

### 1.2. Builder (Criacional)
*   **Descrição:** Separa a construção de um objeto complexo da sua representação. Permite criar diferentes representações do mesmo objeto passo a passo.
*   **Iterações e Variações:**
    *   **Fluent Interface:** Métodos que retornam o próprio builder (`return this`), permitindo encadeamento de chamadas (`.Id().Email().Build()`).
    *   **Director:** O padrão clássico (GoF) sugere uma classe "Director" que orquestra a ordem das chamadas do Builder. Em implementações modernas, o Director é frequentemente omitido em favor de chamadas diretas pelo cliente.
*   **Comparação:** Enquanto o **Factory** cria o objeto em um único passo (focado na família do objeto), o **Builder** foca na complexidade da montagem interna do objeto.

### 1.3. Factory (Criacional)
*   **Descrição:** A Factory encapsula a lógica de instanciação baseada em parâmetros, retornando uma instância de uma classe base ou interface.
*   **Iterações e Variações:**
    *   **Factory Method:** Define uma interface para criar um objeto, mas deixa as subclasses decidirem qual classe instanciar.
    *   **Abstract Factory:** Cria famílias de objetos relacionados ou dependentes sem especificar suas classes concretas.
*   **Comparação:** É mais simples que o **Abstract Factory**. Frequentemente usado em conjunto com **Strategies**, onde a Factory escolhe qual estratégia instanciar.

### 1.4. Strategy (Comportamental)
*   **Descrição:** Define uma família de algoritmos, encapsula cada um deles e os torna intercambiáveis. Permite que o algoritmo varie independentemente dos clientes que o utilizam.
*   **Iterações e Variações:**
    *   **Stateful vs Stateless:** Estratégias podem manter estado interno ou serem puramente funcionais recebendo todo o contexto via parâmetros.
    *   **Lambda/Delegates:** Em linguagens funcionais ou C# moderno, estratégias podem ser implementadas como simples delegates ou lambdas em vez de classes completas.
*   **Comparação:** Estruturalmente idêntico ao padrão **State**, mas com intenção diferente. O **Strategy** é escolhido pelo cliente (ex: "Quero envio Sedex"), enquanto o **State** é alterado internamente pelo fluxo do sistema.

### 1.5. State (Comportamental)
*   **Descrição:** Permite que um objeto altere seu comportamento quando seu estado interno muda. O objeto parecerá ter mudado de classe.
*   **Iterações e Variações:**
    *   **Contexto Centralizado vs Descentralizado:** As transições de estado podem ser controladas pela classe Contexto (Order) ou pelas próprias classes de Estado (o estado 'Novo' sabe que o próximo é 'Pago').
*   **Comparação:** Semelhante ao **Strategy**, mas focado em quando algo acontece (transição temporal), não como algo acontece.

### 1.6. Observer (Comportamental)
*   **Descrição:** Define uma dependência um-para-muitos entre objetos, de modo que quando um objeto muda de estado, todos os seus dependentes são notificados e atualizados automaticamente.
*   **Iterações e Variações:**
    *   **Push vs Pull:** No modelo Push, o sujeito envia os dados na notificação. No Pull, o observador recebe o sinal e busca os dados que precisa no sujeito.
    *   **Eventos Nativos:** Em C#, o padrão é frequentemente implementado usando `Events` e `Delegates` nativos da linguagem, embora a interface `IObserver/ISubject` seja a forma clássica.
*   **Comparação:** Frequentemente usado com **Mediator** para reduzir acoplamento em sistemas complexos de GUI ou mensageria.

---

## 2. Justificativas Detalhadas da Aplicação no Projeto

### 2.1. Singleton (Banco de Dados em Memória)
*   **Contexto:** Classe `InMemoryOrderDatabase` registrada no `Program.cs`.
*   **Por que foi escolhido:** Como a aplicação não utiliza um banco de dados real (SQL/NoSQL), precisávamos de uma estrutura de dados que persistisse enquanto o servidor estivesse rodando.
*   **Problema Resolvido:** Evita que a lista de pedidos seja recriada (e esvaziada) a cada nova requisição HTTP recebida pela API.
*   **Benefícios:** Consistência de dados durante a execução e simplicidade para prototipagem.
*   **Sem o padrão:** Cada vez que você fizesse um `POST /orders`, o pedido seria salvo, mas ao fazer um `GET /orders` em seguida, a lista estaria vazia, pois o controller criaria uma nova instância do repositório.

### 2.2. Builder (Construção de Pedidos)
*   **Contexto:** `OrderBuilder` utilizado no `OrdersController`.
*   **Por que foi escolhido:** A classe `Order` possui muitas dependências (estratégia de frete, pagamento, lista de produtos, dados do cliente) e validações complexas.
*   **Problema Resolvido:** "Telescoping Constructor Anti-pattern" (construtores com muitos parâmetros) e a lógica espalhada de conversão de Strings (do JSON) para Objetos complexos.
*   **Benefícios:** Código do Controller fica limpo e legível. Centraliza a lógica de montagem e validação inicial.
*   **Sem o padrão:** O Controller teria que instanciar manualmente as estratégias, verificar nulos, adicionar produtos um a um e setar propriedades, tornando o código do endpoint longo, frágil e difícil de ler.

### 2.3. Factory (Criação de Pagamentos)
*   **Contexto:** `PaymentFactory` decidindo entre `Pix` e `CreditCard`.
*   **Por que foi escolhido:** O input do usuário é uma `string` ("pix", "credit"), mas o sistema precisa de um objeto `IPaymentMethod`.
*   **Problema Resolvido:** Remove a lógica condicional (`if/switch`) de dentro do Builder ou da classe de Pedido. Respeita o Princípio da Responsabilidade Única (SRP).
*   **Benefícios:** Facilidade de extensão. Para adicionar "Boleto", basta criar a classe e adicionar uma linha no `switch` da Factory, sem tocar no resto do sistema.
*   **Sem o padrão:** Teríamos `if/else` espalhados pelo código sempre que fosse necessário instanciar um pagamento, violando o princípio Aberto/Fechado (OCP).

### 2.4. Strategy (Cálculo de Frete)
*   **Contexto:** `IShippingStrategy` (Sedex, Normal, Free).
*   **Por que foi escolhido:** O cálculo do frete varia drasticamente dependendo da escolha do usuário, e novas formas de envio podem surgir.
*   **Problema Resolvido:** Elimina condicionais complexas dentro da classe `Order`. A classe `Order` não precisa saber *como* calcular frete, apenas que existe um custo.
*   **Benefícios:** Código desacoplado e coeso. Cada algoritmo de frete fica em seu próprio arquivo, facilitando testes unitários específicos para cada regra de cálculo.
*   **Sem o padrão:** Um método `CalculateShipping()` gigante dentro de `Order` com múltiplos `if (type == "sedex") { ... } else if ...`, tornando a classe `Order` difícil de manter e testar.

### 2.5. State (Máquina de Estados do Pedido)
*   **Contexto:** `OrderState` controlando transições (New -> Paid -> Shipped).
*   **Por que foi escolhido:** O pedido tem regras de negócio estritas sobre o que pode acontecer em cada fase (ex: não pode enviar sem pagar, não pode cancelar se já enviou).
*   **Problema Resolvido:** Controle de fluxo complexo e validação de transições. Evita estados inconsistentes.
*   **Benefícios:** Encapsulamento do comportamento específico de cada estado. O código reflete explicitamente o ciclo de vida do negócio.
*   **Sem o padrão:** Flags booleanas (`isPaid`, `isShipped`) e verificações defensivas manuais em todos os métodos (`if (!isPaid) throw ...`), resultando em código propenso a bugs lógicos.

### 2.6. Observer (Notificações)
*   **Contexto:** `ISubject` (Order) notificando `IObserver` (Email/Whatsapp).
*   **Por que foi escolhido:** Necessidade de realizar ações secundárias (enviar email) quando o estado principal muda, sem acoplar essas ações à lógica central.
*   **Problema Resolvido:** Alto acoplamento. A classe `Order` não deve depender de bibliotecas de envio de e-mail ou APIs de WhatsApp.
*   **Benefícios:** Baixo acoplamento. Pode-se adicionar novos observadores (ex: Log, Push Notification, SMS) sem alterar uma linha de código na classe `Order`.
*   **Sem o padrão:** A classe `Order` teria chamadas diretas como `EmailService.Send(...)` dentro de seus métodos de transição, misturando lógica de domínio com lógica de infraestrutura.

