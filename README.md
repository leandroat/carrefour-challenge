# Transaction Service

Serviço responsável para controle de fluxo de caixa, permitindo a aplicação cliente enviar requisições API Rest para lançamentos de crédito e débito, bem como recuperar o balanço de caixa dado um determinado dia.

## Tech Stack
Tecnologias utilizadas:
- MicroServiço em .Net Core
- Docker
- SQL Server

## Inicialização & Instalação


Before to setup Worker service, you must complete the initial setup for database, described here: https://github.com/Clipboard-recruiting/candidate-sse-take-home-challenge-304

Once database setup is completed, you need to follow these steps:

- In `services\WorkService` folder, you need to edit `docker-compose.yml` file and point to same database network. It is required to allow communication between API and database services.

```
    networks:
      cbh_thc_network:
        name: seed_cbh_thc_network
        external: true
```

- Once it is done, in same folder execute the command bellow to keep API service running:

`docker-compose up -d`

- Now, you are able to send HTTP request to **localhost** and **port 7110**.

## API Documentation
[![Run in Insomnia}](https://insomnia.rest/images/run.svg)](https://insomnia.rest/run/?label=shifts&uri=https%3A%2F%2Fgithub.com%2FClipboard-recruiting%2Fcandidate-sse-take-home-challenge-304%2Ftree%2Fsse-thc-304%2FInsomnia_THC_304.json)

**Endpoint:**`/shifts/[worker-id]`

Retrieve all shifts given a **worker id**, **facility id**, **start** and **end** dates.

**Query parameters:**

f - Query parameter for *facility-id*

s - Query parameter for *start* date

e - Query parameter for *end* date

Example:

`GET /shifts/1?f=3&s=2023-02-06&e=2023-02-06`

**Response:**

HTTP 400 Bad Request - For invalid query request.

HTTP 200 Success - For a valid request, returning a shift list.

Example:

```json
[
	{
		"date": "2023-02-06",
		"shifts": [
			{
				"id": 698696,
				"start": "2023-02-06T13:00:00.591",
				"end": "2023-02-06T18:00:00.591",
				"facility": "3c5d64815100f1a02b1984e89dadbe2ed5d25d7d"
			},
			{
				"id": 1815986,
				"start": "2023-02-06T05:00:00.435",
				"end": "2023-02-06T10:00:00.435",
				"facility": "3c5d64815100f1a02b1984e89dadbe2ed5d25d7d"
			},
			{
				"id": 1296699,
				"start": "2023-02-06T05:00:00.753",
				"end": "2023-02-06T10:00:00.753",
				"facility": "3c5d64815100f1a02b1984e89dadbe2ed5d25d7d"
			}
		]
	}
]
```