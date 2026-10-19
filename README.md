# 🏦 PBCadastroCliente - Microsserviço de Cadastro

## 🗺️ Fluxograma da Arquitetura
Abaixo está o desenho da nossa arquitetura orientada a eventos, demonstrando o fluxo da informação desde o cadastro até a emissão dos cartões:

![Fluxograma da Solução](mermaid%20fluxograma.png)

## 🛠 Tecnologias Utilizadas
- **.NET 10.0** (ASP.NET Core Web API)
- **Entity Framework Core** (In-Memory Database para simplificação do ambiente local)
- **MassTransit + RabbitMQ** (Mensageria e Arquitetura Orientada a Eventos)
- **FluentValidation** (Validação de regras de negócio e de domínio na entrada de dados)
- **xUnit + Moq** (Testes de Unidade)
- **Asp.Versioning.Http** (Versionamento de API - v1)

## 🏗 Decisões Técnicas
- **Arquitetura Orientada a Eventos (EDA):** A comunicação com o microsserviço de Propostas (MS2) é feita de forma assíncrona. Assim que o cliente é salvo no banco de dados, publicamos um evento no RabbitMQ.
- **Namespace Global de Mensagens:** Foi adotado o namespace compartilhado `ParanaBanco.Messages` para o contrato do evento, garantindo o roteamento perfeito no broker (RabbitMQ) e evitando a criação de filas "fantasmas" ou mensagens perdidas.
- **Validações na Entrada (Fail-Fast):** O uso do FluentValidation garante que requisições com dados inválidos retornem `400 Bad Request` imediatamente, poupando processamento de banco de dados e de mensageria.

## 📡 Mensageria (Eventos)
- **Produz / Publica:** `ClienteCadastrado`
    - *Payload:* `ClienteId`, `Cpf`, `Email`
    - *Namespace:* `ParanaBanco.Messages`

## 🚀 Como Executar Localmente

### Pré-requisitos
- [.NET 10 SDK](https://dotnet.microsoft.com/)
- [Docker](https://www.docker.com/) (Para rodar o RabbitMQ)

### Passos para Execução

1. **Suba o RabbitMQ via Docker:**
   Abra o terminal e execute o seguinte comando para criar um container do RabbitMQ com o painel de gerenciamento ativo:
   ```bash
   docker run -d --hostname rabbit-local --name rabbitmq-pb -p 5672:5672 -p 15672:15672 rabbitmq:3-management
   
2. **Clone e inicie a Aplicação:**
    ```bash
    dotnet run
   
3. **Acesse a API:**
   Realize um POST /api/v1/clientes com o seguinte payload de exemplo:
    ```bash
   {
       "nome": "João da Silva",
       "cpf": "674.933.810-03",
       "email": "joao@email.com"
   }