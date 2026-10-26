# Paraná Banco — Cadastro de Clientes

Microsserviço responsável por registrar clientes via API REST e publicar o evento
`ClienteCadastrado` no hub de mensageria (RabbitMQ). É o ponto de entrada do fluxo
de concessão de crédito.

## Arquitetura

Projeto em camadas (Clean Architecture):

```
Clientes.API            → Controllers, Program.cs, configuração
Clientes.Application    → Services (orquestração), DTOs, Common (Result/Error)
Cliente.Core            → Entidades, validações, contrato do evento e interface do publisher
Cliente.Infrastructure  → EF Core (SQL Server), repositório, mensageria (RabbitMQ)
Clientes.Testes         → Testes de unidade (xUnit + Moq + Bogus)
```

As dependências apontam para dentro (API → Application → Core; Infrastructure → Core).
A mensageria é um detalhe de infraestrutura: o domínio depende apenas da abstração
`IClienteEventPublisher`.

## Endpoints

| Método | Rota | Descrição |
|--------|------|-----------|
| POST | `/api/v1/clientes` | Cadastra um novo cliente |
| GET | `/api/v1/clientes/{id}` | Consulta um cliente por ID |

### Exemplo — POST /api/v1/clientes

```json
{
  "nome": "João da Silva",
  "dataNascimento": "1990-05-20",
  "cpf": "11144477735",
  "email": "joao@teste.com",
  "rendaMensal": 4500.00
}
```

Validações: CPF (dígitos verificadores), e-mail, campos obrigatórios e idade mínima
de 18 anos. O CPF é aceito com ou sem máscara e persistido apenas com os 11 dígitos.

## Mensageria

Ao cadastrar um cliente com sucesso, publica o evento **`ClienteCadastrado`**:

- **Exchange:** `clientes.exchange` (topic, durable)
- **Routing key:** `cliente.cadastrado`
- **Payload (fat event):** dados necessários para o consumidor calcular o score
  (renda e data de nascimento), evitando que a Proposta precise consultar este serviço.

## Variáveis de ambiente / configuração

Configuração em `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=SEU_SERVIDOR;Database=ParanaBancoClientes;Trusted_Connection=True;TrustServerCertificate=True;"
  },
  "RabbitMq": {
    "HostName": "localhost",
    "Port": 5672,
    "UserName": "guest",
    "Password": "guest",
    "VirtualHost": "/",
    "Exchange": "clientes.exchange",
    "ClienteCadastradoRoutingKey": "cliente.cadastrado"
  }
}
```

Ajuste `DefaultConnection` para o seu SQL Server.

## Como executar localmente

Pré-requisitos: .NET 10 SDK, SQL Server, RabbitMQ.

1. **Subir o RabbitMQ** (via Docker):
   ```bash
   docker run -d --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3.13-management
   ```
   Painel de gerenciamento: http://localhost:15672 (guest/guest)

2. **Aplicar as migrations** (cria o banco e as tabelas):
   ```bash
   dotnet ef database update --project Cliente.Infrastructure --startup-project Clientes.API
   ```

3. **Rodar a API:**
   ```bash
   dotnet run --project Clientes.API
   ```
   Disponível em `http://localhost:5057`.

## Testes

```bash
dotnet test
```

Cobrem a validação de CPF e o fluxo de cadastro (incluindo a publicação do evento),
usando xUnit, Moq (mock das dependências) e Bogus (dados fake).

## Decisões técnicas

- **Mensageria com RabbitMQ**: atende o requisito de DLQ/retry nativamente e roda
  local via Docker, sem dependência de nuvem.
- **Fat event (event-carried state transfer)**: o evento carrega os dados que o
  consumidor precisa, mantendo os serviços desacoplados (a Proposta não chama este
  serviço de volta).
- **Normalização de CPF**: removida a máscara antes de persistir, garantindo
  consistência entre validação, checagem de unicidade e o formato `char(11)` do banco.
- **Padrão Result (`ResultViewModel`/`Error`)**: erros de negócio esperados retornam
  um resultado de falha, sem exceções no fluxo normal.
```
