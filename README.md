# Transaction Service

Serviço responsável para controle de fluxo de caixa, permitindo a aplicação cliente enviar requisições API Rest para lançamentos de crédito e débito, bem como recuperar o balanço de caixa dado um determinado dia.

## Tech Stack
Tecnologias utilizadas:
- MicroServiço em .Net Core (C#)
- SQL Server
- Docker

## Inicialização & Instalação

Todos os serviços, API e SQL Server, estão rodando em docker containers. Para utilizá-los, basta entrar no diretório raíz do projeto e rodar o seguinte comando:

`docker-compose up -d`

Agora você será capaz de enviar requisições HTTP para o endereço **localhost:7111**.

Para fazer o setup do banco de dados, acesse o docker container `sqlserver` e execute os scripts SQL contidos no diretório `/tmp`.

- Uma vez dentro do container, acessar o banco de dados: `/opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P P@ssword!!`
- Executar os scripts na seguinte ordem:
-- Criação da tabela: `/opt/mssql-tools/bin/sqlcmd -S localhost -U "sa" -P "P@ssword!!" -i "/tmp/01-CreateTable.sql"`
-- Popular algumas entradas iniciais: `/opt/mssql-tools/bin/sqlcmd -S localhost -U "sa" -P "P@ssword!!" -i "/tmp/02-InsertData.sql"`

## Documentação da API
[![Run in Insomnia}](https://insomnia.rest/images/run.svg)](https://insomnia.rest/run/?label=shifts&uri=https%3A%2F%2Fgithub.com%2FClipboard-recruiting%2Fcandidate-sse-take-home-challenge-304%2Ftree%2Fsse-thc-304%2FInsomnia_THC_304.json)

##### Endpoint **`POST /transaction`**

Cria um novo lançamento de caixa.

**Exemplo de requisição:**

```json
{
	"amount": 234,
	"description": "Test transaction"
}
```

**Exemplo de resposta:**

```json
{
	"id": "a1oKxwlCi95QapBZcmFMjrFgXBYUvntf",
	"amount": 234,
	"description": "Test transaction",
	"date": "2023-06-26T00:00:00"
}
```

HTTP 200 (Ok) - Sucesso ao retornar as informações
HTTP 400 (Bad Request) - Houve um erro na requisição.

##### Endpoint **`GET /transaction/[id]`**

Retorna as informações relacionadas a transação correspondente ao `id` fornecido.

**Exemplo de resposta:**

```json
{
	"id": "a1oKxwlCi95QapBZcmFMjrFgXBYUvntf",
	"amount": 234,
	"description": "Test transaction",
	"date": "2023-06-26T00:00:00"
}
```

HTTP 200 (Ok) - Sucesso ao retornar as informações
HTTP 204 (No Content) - Nenhuma informação existe para o `id` fornecido
HTTP 400 (Bad Request) - Houve um erro na requisição.

##### Endpoint **`DELETE /transaction/[id]`**

Remove um lançamento específico relacionado ao `id` fornecido.

**Resposta:**

HTTP 200 (Ok) - Sucesso ao remover o lançamento
HTTP 204 (No Content) - Nenhuma informação existe para o `id` fornecido
HTTP 400 (Bad Request) - Houve um erro na requisição.

##### Endpoint **`GET /transaction/balance?date=[dd/mm/yyyy]`**

Retorna o balanço consolidado relacionado à todas transações existentes no dia `date` informado. Deve respeitar o formato `dd/mm/yyyy`.

**Exemplo de resposta:**

```json
{
	"amount": 50,
	"date": "2023-06-23T00:00:00"
}
```

HTTP 200 (Ok) - Sucesso ao retornar as informações
HTTP 400 (Bad Request) - Houve um erro na requisição.

## Ideias & Melhorias
- Criar uma API para retornar o balanço em um intervalo de dias
- Utilizar paginação ao listar os lançamentos, isso melhoraria o desempenho.
- Para um ambiente mais controlado, utilizar JWT Token como forma de autorização do uso do serviço.