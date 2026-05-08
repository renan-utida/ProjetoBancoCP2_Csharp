# ProjetoBancoCP2 — Backend Banco Digital

Backend de um banco digital desenvolvido em **ASP.NET Core Web API (.NET 8)** com **Entity Framework Core** e **Oracle Database**. Projeto avaliativo da disciplina de Engenharia de Software — FIAP.

**Turma** - 3ESPW.

**Professor** - Rafael Santos Novo Pereira

**Link Repositório** - [Repositório GitHub ProjetoBancoCP2](https://github.com/renan-utida/ProjetoBancoCP2_Csharp)

---

## 1. Identificação

| Nome | RM |
|---|---|
| Renan Dias Utida | RM558540 |
| Luan Ramos Garcia de Souza | RM558537 |

---

## 2. Produto Bancário Escolhido

**Produto:** Empréstimo

**Justificativa:** O Empréstimo foi escolhido por permitir a implementação de regras de negócio reais e relevantes para o contexto bancário. Além do CRUD básico, implementamos duas regras extras obrigatórias para dupla:

- **Cálculo automático de parcela** via fórmula Price (juros compostos): `PMT = PV * (i * (1+i)^n) / ((1+i)^n - 1)`. O valor da parcela é calculado automaticamente pelo sistema ao cadastrar ou atualizar um empréstimo — o usuário não precisa informá-lo.
- **Avaliação de score de crédito** com base no valor solicitado e prazo: empréstimos até R$20.000 em até 36 meses são `APROVADO`, até R$50.000 em até 60 meses ficam em `ANALISE`, e acima disso são `REPROVADO`.

---

## 3. Decisão de Modelagem de Filas

> ⚠️ **Nota:** A implementação de mensageria com RabbitMQ (filas, consumers, Docker) não foi trabalhada em aula até a data de entrega deste checkpoint, portanto não faz parte do escopo avaliado desta entrega.

O endpoint `POST /api/contratacoes` simula o comportamento assíncrono retornando **202 Accepted**, com o status da contratação iniciando como `PENDENTE`. O status pode ser atualizado via `PUT /api/contratacoes/{id}` para refletir o resultado do processamento (`APROVADO`, `REPROVADO`, `ANALISE`).

Caso fosse implementado com RabbitMQ, a decisão seria: **1 fila por tipo de produto** (ex: `fila.emprestimo`, `fila.maquina-cartao`, `fila.salario`), pois cada produto tem regras de processamento distintas e consumidores independentes, facilitando o reprocessamento seletivo em caso de falha.

---

## 4. Diagrama de Classes

<img width="1748" height="674" alt="Diagrama_CP2_C#" src="https://github.com/user-attachments/assets/88d28e25-a304-4574-83ba-0f3d9c4de81d" />


> Arquivo fonte disponível em `docs/diagram/Diagrama_CP2_C#.drawio`

**Link do Diagrama (draw.io):**

[https://drive.google.com/file/d/1sKFWeG3nSd7Y5A0lASS0Ur4-SWXl5gcO/view?usp=sharing](https://drive.google.com/file/d/1sKFWeG3nSd7Y5A0lASS0Ur4-SWXl5gcO/view?usp=sharing)

---

## 5. Como Rodar Localmente

### Pré-requisitos

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) (ou VS Code com extensão C#)
- Acesso à rede FIAP (VPN ou presencial) para conectar ao Oracle
- [Oracle SQL Developer](https://www.oracle.com/database/sqldeveloper/) (opcional, para visualizar as tabelas)

### Credenciais Oracle

```
Host:     oracle.fiap.com.br
Porta:    1521
SID:      ORCL
Usuário:  RM558540
Senha:    ********
```

### Configurar connection string

Em `ProjetoBancoCP2/appsettings.json`, substitua com suas credenciais:

```json
{
  "ConnectionStrings": {
    "OracleConnection": "User Id=SEU_RM;Password=SUA_SENHA;Data Source=oracle.fiap.com.br:1521/ORCL"
  }
}
```

### Aplicar migrations e rodar

```bash
# Restaurar pacotes
dotnet restore

# Aplicar migrations (criar tabelas no Oracle)
dotnet ef database update

# Rodar a API
dotnet run --project ProjetoBancoCP2
```

A API estará disponível em:
- `https://localhost:7106/swagger` — Swagger UI
- `https://localhost:7106/api/...` — Endpoints REST

---

## 6. Endpoints Disponíveis

### Agências

#### `POST /api/Agencias` — Cadastrar agência
**Request:**
```json
{
  "nmAgencia": "Agência Centro SP",
  "cep": "01310100",
  "dsEndereco": "Av. Paulista, 1000 - Bela Vista, São Paulo"
}
```
**Response:** `201 Created`
```json
{
  "idAgencia": 1,
  "nmAgencia": "Agência Centro SP",
  "cep": "01310100",
  "dsEndereco": "Av. Paulista, 1000 - Bela Vista, São Paulo"
}
```

---

#### `GET /api/Agencias/{id}` — Buscar agência por ID
**Response:** `200 OK`
```json
{
  "idAgencia": 1,
  "nmAgencia": "Agência Centro SP",
  "cep": "01310100",
  "dsEndereco": "Av. Paulista, 1000 - Bela Vista, São Paulo"
}
```
**Response:** `404 Not Found`
```json
{ "mensagem": "Agência não encontrada." }
```

---

#### `GET /api/Agencias` — Listar todas as agências
**Response:** `200 OK` — array de agências

---

#### `PUT /api/Agencias/{id}` — Atualizar agência
**Request:**
```json
{
  "idAgencia": 1,
  "nmAgencia": "Agência Paulista Atualizada",
  "cep": "01310200",
  "dsEndereco": "Av. Paulista, 2000 - Bela Vista, São Paulo"
}
```
**Response:** `204 No Content`

---

#### `DELETE /api/Agencias/{id}` — Deletar agência
**Response:** `204 No Content`

---

### Clientes

#### `POST /api/Clientes/pf` — Cadastrar Pessoa Física
**Request:**
```json
{
  "nmCliente": "Renan Dias",
  "idAgencia": 1,
  "cpf": "12345678901",
  "dataNascimento": "2001-01-01"
}
```
**Response:** `201 Created`
```json
{
  "cpf": "12345678901",
  "dataNascimento": "2001-01-01T00:00:00",
  "idCliente": 1,
  "nmCliente": "Renan Dias",
  "idAgencia": 1,
  "agencia": { "idAgencia": 1, "nmAgencia": "Agência Centro SP", "cep": "01310100", "dsEndereco": "..." }
}
```
**Response:** `400 Bad Request` — CPF duplicado
```json
{ "mensagem": "CPF já cadastrado." }
```
**Response:** `400 Bad Request` — Agência inexistente
```json
{ "mensagem": "Agência informada não existe." }
```

---

#### `POST /api/Clientes/pj` — Cadastrar Pessoa Jurídica
**Request:**
```json
{
  "nmCliente": "Empresa Teste LTDA",
  "idAgencia": 1,
  "cnpj": "12345678000195",
  "razaoSocial": "Empresa Teste Ltda"
}
```
**Response:** `201 Created`

**Response:** `400 Bad Request` — CNPJ duplicado
```json
{ "mensagem": "CNPJ já cadastrado." }
```

---

#### `GET /api/Clientes/{id}` — Buscar cliente por ID
**Response:** `200 OK`
```json
{
  "idCliente": 1,
  "nmCliente": "Renan Dias",
  "idAgencia": 1,
  "agencia": {
    "idAgencia": 1,
    "nmAgencia": "Agência Centro SP",
    "cep": "01310100",
    "dsEndereco": "Av. Paulista, 1000 - Bela Vista, São Paulo"
  }
}
```
**Response:** `404 Not Found`
```json
{ "mensagem": "Cliente não encontrado." }
```

---

#### `GET /api/Clientes` — Listar todos os clientes
**Response:** `200 OK` — array de clientes (PF e PJ)

---

#### `PUT /api/Clientes/pf/{id}` — Atualizar Pessoa Física
**Request:**
```json
{
  "idCliente": 1,
  "nmCliente": "Renan Dias Atualizado",
  "idAgencia": 1,
  "cpf": "12345678901",
  "dataNascimento": "2002-02-02"
}
```
**Response:** `204 No Content`

**Response:** `404 Not Found` — ID de PJ passado no PUT de PF
```json
{ "mensagem": "Cliente PF não encontrado." }
```

---

#### `PUT /api/Clientes/pj/{id}` — Atualizar Pessoa Jurídica
**Request:**
```json
{
  "idCliente": 2,
  "nmCliente": "Empresa Teste Atualizada LTDA",
  "idAgencia": 1,
  "cnpj": "12345678000195",
  "razaoSocial": "Empresa Teste Atualizada Ltda"
}
```
**Response:** `204 No Content`

---

#### `DELETE /api/Clientes/pf/{id}` — Deletar Pessoa Física
**Response:** `204 No Content`

#### `DELETE /api/Clientes/pj/{id}` — Deletar Pessoa Jurídica
**Response:** `204 No Content`

---

### Empréstimos

#### `POST /api/Emprestimos` — Cadastrar empréstimo
> O campo `valorParcela` é calculado automaticamente pela fórmula Price. O score é avaliado automaticamente com base no valor e prazo.

**Request:**
```json
{
  "nmProduto": "Empréstimo Pessoal",
  "valorSolicitado": 10000,
  "taxaJuros": 2.5,
  "prazoMeses": 24,
  "valorParcela": 0
}
```
**Response:** `201 Created` — score APROVADO
```json
{
  "emprestimo": {
    "idProduto": 1,
    "nmProduto": "Empréstimo Pessoal",
    "valorSolicitado": 10000.00,
    "taxaJuros": 2.50,
    "prazoMeses": 24,
    "valorParcela": 565.29
  },
  "score": "APROVADO",
  "mensagem": "Empréstimo criado com sucesso."
}
```
**Response:** `400 Bad Request` — score REPROVADO
```json
{
  "mensagem": "Empréstimo reprovado por score de crédito.",
  "score": "REPROVADO"
}
```

---

#### `GET /api/Emprestimos/{id}` — Buscar empréstimo por ID
**Response:** `200 OK`

**Response:** `404 Not Found`
```json
{ "mensagem": "Empréstimo não encontrado." }
```

---

#### `GET /api/Emprestimos` — Listar todos os empréstimos
**Response:** `200 OK` — array de empréstimos

---

#### `PUT /api/Emprestimos/{id}` — Atualizar empréstimo
> O `valorParcela` é recalculado automaticamente ao atualizar.

**Request:**
```json
{
  "idProduto": 1,
  "nmProduto": "Empréstimo Pessoal Atualizado",
  "valorSolicitado": 8000,
  "taxaJuros": 2.0,
  "prazoMeses": 12,
  "valorParcela": 0
}
```
**Response:** `204 No Content`

---

#### `DELETE /api/Emprestimos/{id}` — Deletar empréstimo
**Response:** `204 No Content`

---

### Contratações

#### `POST /api/Contratacoes` — Solicitar contratação
**Request:**
```json
{
  "idCliente": 1,
  "idProduto": 1
}
```
**Response:** `202 Accepted`
```json
{
  "idContratacao": 1,
  "idCliente": 1,
  "idProduto": 1,
  "status": "PENDENTE",
  "dtSolicitacao": "2026-05-07T16:47:32"
}
```
**Response:** `404 Not Found` — cliente inexistente
```json
{ "mensagem": "Cliente não encontrado." }
```
**Response:** `404 Not Found` — produto inexistente
```json
{ "mensagem": "Produto não encontrado." }
```

---

#### `GET /api/Contratacoes/{id}` — Consultar status da contratação
**Response:** `200 OK`
```json
{
  "idContratacao": 1,
  "idCliente": 1,
  "cliente": {
    "idCliente": 1,
    "nmCliente": "Renan Dias",
    "idAgencia": 1,
    "agencia": { "idAgencia": 1, "nmAgencia": "Agência Centro SP", "cep": "01310100", "dsEndereco": "..." }
  },
  "idProduto": 1,
  "produto": { "idProduto": 1, "nmProduto": "Empréstimo Pessoal" },
  "status": "PENDENTE",
  "dtSolicitacao": "2026-05-07T16:47:32"
}
```
**Response:** `404 Not Found`
```json
{ "mensagem": "Contratação não encontrada." }
```

---

#### `GET /api/Contratacoes` — Listar todas as contratações
**Response:** `200 OK` — array de contratações com cliente e produto completos

---

#### `PUT /api/Contratacoes/{id}` — Atualizar status da contratação
**Request:**
```json
{
  "idContratacao": 1,
  "idCliente": 1,
  "idProduto": 1,
  "status": "APROVADO",
  "dtSolicitacao": "2026-05-07T00:00:00"
}
```
**Response:** `204 No Content`

---

#### `DELETE /api/Contratacoes/{id}` — Deletar contratação
**Response:** `204 No Content`

---

### Ordem recomendada para testes

Para evitar erros de chave estrangeira, siga esta ordem:

**Cadastro:** `Agência` → `Empréstimo` → `Cliente (PF ou PJ)` → `Contratação`

**Deleção:** `Contratação` → `Cliente` → `Empréstimo` → `Agência`

> A coleção completa do Postman com todos os endpoints está disponível em `docs/postman/ProjetoBancoCP2.postman_collection.json`

---

## 7. Testes

> ⚠️ **Nota:** Testes automatizados com `dotnet test` não foram implementados neste checkpoint, pois este conteúdo não foi trabalhado em aula até a data de entrega.

Os fluxos críticos foram validados manualmente via **Swagger UI** e **Postman**, cobrindo todos os cenários obrigatórios:

| Fluxo Crítico | Validação | Status |
|---|---|---|
| Cadastro PF com CPF duplicado | Retorna `400` com mensagem | ✅ |
| Cadastro PJ com CNPJ duplicado | Retorna `400` com mensagem | ✅ |
| Vincular cliente a agência inexistente | Retorna `400` com mensagem | ✅ |
| Contratação válida | Retorna `202 Accepted` com status `PENDENTE` | ✅ |
| Contratação com cliente inexistente | Retorna `404` com mensagem | ✅ |
| Consulta de status da contratação | Retorna contratação completa com cliente e produto | ✅ |
| Empréstimo reprovado por score | Retorna `400` com score `REPROVADO` | ✅ |
| Cálculo automático de parcela (fórmula Price) | `valorParcela` calculado automaticamente | ✅ |

---

## 8. Mensageria (RabbitMQ)

> ⚠️ **Nota:** A implementação de filas com RabbitMQ não foi trabalhada em aula até a data de entrega deste checkpoint e não faz parte do escopo avaliado.

---

## 9. Evidências de Funcionamento

> Prints do Swagger e Postman demonstrando os endpoints funcionando, incluindo contratação com status `APROVADO`, estão disponíveis na pasta `docs/`.

---

## Stack Utilizada

| Camada | Tecnologia |
|---|---|
| Runtime | .NET 8.0 |
| API | ASP.NET Core Web API |
| ORM | Entity Framework Core 9.0 |
| Banco de Dados | Oracle (oracle.fiap.com.br:1521/ORCL) |
| Documentação | Swagger / OpenAPI |
| Testes manuais | Postman + Swagger UI |
