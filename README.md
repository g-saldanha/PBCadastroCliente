# Microsserviço 1: Cadastro de Clientes

Este repositório contém o primeiro microsserviço do desafio técnico para Desenvolvedor Backend do Paraná Banco. Este microsserviço é responsável por registrar novos clientes via API REST e publicar eventos de cadastro em um hub de mensageria.

## 📋 Funcionalidades

* **Cadastro de Cliente:** Endpoint REST para criação de um novo cliente com validações rigorosas de CPF, e-mail e dados obrigatórios.
* **Consulta de Cliente:** Endpoint para consultar os dados de um cliente através do seu ID.
* **Mensageria:** Publicação assíncrona do evento `ClienteCadastrado` no hub de mensageria após o cadastro com sucesso.

## 🛠️ Tecnologias Utilizadas

* **Framework:** .NET 10.0
* **Mensageria:** RabbitMQ / Azure Service Bus (conforme configuração)
* **Testes:** xUnit para testes de unidade
* **Arquitetura:** Princípios de Clean Code, separação de responsabilidades e design de APIs RESTful.


## ⚙️ Pré-requisitos

Antes de iniciar, certifique-se de ter as seguintes ferramentas instaladas:

* [.NET 10.0 SDK](https://dotnet.microsoft.com/)
* [Docker](https://www.docker.com/) (para rodar o hub de mensageria localmente)
* IDE de sua preferência (Visual Studio 2022, Rider, VS Code)

## 🚀 Como Executar Localmente

1. **Clone este repositório:**
```bash
git clone https://github.com/seu-usuario/cadastro-clientes-pb.git
cd cadastro-clientes-pb
```

2. **Suba a infraestrutura (Mensageria e Banco de Dados):**
*(Se houver um arquivo `docker-compose.yml` na raiz)*
```bash
docker-compose up -d
```

3. **Configure as Variáveis de Ambiente:**
Renomeie o arquivo `appsettings.Example.json` para `appsettings.Development.json` e preencha as configurações, especialmente a string de conexão da mensageria.
```json
{
  "ConnectionStrings": {
    "Database": "SuaConnectionStringAqui",
    "MessagingHub": "Host=localhost;Username=guest;Password=guest"
  }
}
```


4. **Execute o projeto:**
```bash
dotnet restore
dotnet run --project src/CadastroClientes.Api
```

A API estará disponível em `https://localhost:5001/swagger` (ou porta configurada).

## 📡 Endpoints da API

A documentação interativa (Swagger) está disponível ao rodar o projeto. Os endpoints principais são:

* `POST /api/v1/clientes` - Cadastra um novo cliente.
* `GET /api/v1/clientes/{id}` - Consulta um cliente específico pelo ID.

## 🧠 Decisões Técnicas

* **Validações:** Utilização de bibliotecas (ex: FluentValidation) para garantir a integridade dos dados obrigatórios (CPF e e-mail) antes da persistência e publicação do evento.
* **Mensageria Assíncrona:** A publicação do evento `ClienteCadastrado` foi implementada de forma desacoplada para não bloquear a resposta da API REST e garantir a comunicação fluida com o serviço de Proposta de Crédito.
* **Qualidade de Código:** O projeto foi estruturado seguindo boas práticas de organização para facilitar a manutenção e manter as responsabilidades bem definidas.


## 🧪 Testes

Para garantir a qualidade, foram implementados testes de unidade utilizando o **xUnit**. Os testes seguem o padrão AAA (Arrange, Act, Assert) e focam nas regras de validação e na correta publicação do evento no hub.

Para rodar os testes, execute o comando:

```bash
dotnet test
```

---
*Este projeto faz parte do desafio de Recrutamento & Seleção Técnica - Desenvolvedor Backend (Maio 2026)*.
