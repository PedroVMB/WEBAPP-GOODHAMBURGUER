# Good Hamburger 🍔

Sistema de pedidos para a lanchonete **Good Hamburger**, construído com .NET 10, ASP.NET Core, Blazor Server e EF Core InMemory.

---

## Como executar

### Pré-requisitos
- [.NET 10 SDK](https://dotnet.microsoft.com/download)

### 1. Clonar o repositório
```bash
git clone <url-do-repositorio>
cd GoodHamburguer
```

### 2. Rodar a API
```bash
cd GoodHamburguer.API
dotnet run
```
A API estará disponível em `https://localhost:7255`.  
Swagger UI: `https://localhost:7255/swagger`

### 3. Rodar o Frontend Blazor
Em outro terminal:
```bash
cd GoodHamburguer.Web
dotnet run
```
O frontend estará disponível em `https://localhost:7001` (porta configurável em `Properties/launchSettings.json`).

### 4. Rodar os testes
```bash
dotnet test GoodHamburguer.Tests
```

---

## Endpoints da API

| Método | Rota | Descrição |
|--------|------|-----------|
| GET | `/api/menu` | Lista o cardápio completo |
| GET | `/api/orders` | Lista todos os pedidos |
| GET | `/api/orders/{id}` | Retorna um pedido pelo ID |
| POST | `/api/orders` | Cria um novo pedido |
| PUT | `/api/orders/{id}` | Atualiza um pedido existente |
| DELETE | `/api/orders/{id}` | Remove um pedido (soft delete) |

### Exemplo de criação de pedido (POST `/api/orders`)
```json
{
  "sandwich": 1,
  "includeFries": true,
  "includeSoda": true
}
```

**Valores de `sandwich`:** `1` = X-Burger, `2` = X-Egg, `3` = X-Bacon

---

## Arquitetura

O projeto segue arquitetura em camadas seguindo o padrão clean arch:

```
GoodHamburguer.Model          → Entidades e Enums do domínio
GoodHamburguer.Infrastructure → EF Core, DbContext, Repositórios
GoodHamburguer.Application    → DTOs, Serviços, Lógica de negócio
GoodHamburguer.API            → Controllers REST (ASP.NET Core)
GoodHamburguer.Web            → Frontend Blazor Server
GoodHamburguer.Tests          → Testes unitários (xUnit + Moq)
```

### Decisões técnicas

- **EF Core InMemory** — Banco de dados em memória para simplicidade. Sem necessidade de configurar banco externo para executar o projeto.
- **Soft Delete** — Pedidos removidos têm `IsDisabled = true`. Não são deletados fisicamente do banco.
- **DiscountCalculator estático** — A lógica de desconto é pura (sem efeitos colaterais), portanto implementada como classe estática, facilitando testes unitários diretos sem mocks.
- **DTOs separados** — `CreateOrderRequest`, `UpdateOrderRequest` e `OrderResponse` desacoplam a camada de apresentação do modelo de domínio.
- **Blazor WEB** — Escolhido por simplicidade de comunicação com a API via `HttpClient` injetado.
- **CORS configurado** — A API permite chamadas do frontend Blazor (OBS: Não tem uma camada de segurança 100% válida, apenas foi criado uma policy para o sistema web se comunicar com a API).

---

## Cardápio e Regras de Desconto

| Item | Tipo | Preço |
|------|------|-------|
| X-Burger | Sanduíche | R$ 5,00 |
| X-Egg | Sanduíche | R$ 4,50 |
| X-Bacon | Sanduíche | R$ 7,00 |
| Batata Frita | Acompanhamento | R$ 2,00 |
| Refrigerante | Acompanhamento | R$ 2,50 |

**Descontos:**
- Sanduíche + Batata + Refrigerante → **20% de desconto**
- Sanduíche + Refrigerante → **15% de desconto**
- Sanduíche + Batata → **10% de desconto**

---

## Testes

21 testes cobrindo:
- **DiscountCalculatorTests** (8) — Todas as combinações de desconto
- **OrderServiceTests** (8) — Criação, atualização, deleção com mocks dos pedidos
- **OrderRepositoryTests** (5) — CRUD com EF Core InMemory real 

---

## O que ficou fora, mas poderá ser implementado facilmente após o desafio

- **Banco de dados persistente** (SQL Server / PostgreSQL) — Optou-se pelo InMemory para facilitar a execução sem infraestrutura.
- **Autenticação/Autorização** — Fora do escopo do desafio, mas fácil de ser implementado, basta conectar diretamente com o Identity, configurar um mapeamento simples pelo mesmo e adicionar uma interface de login, isso é claro, sem configurar policys.
- **Paginação** na listagem de pedidos. 
- **Testes de integração** e testes de componentes Blazor.

