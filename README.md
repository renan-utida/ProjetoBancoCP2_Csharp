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

> **Nota:** A implementação de mensageria com RabbitMQ (filas, consumers, Docker) não foi trabalhada em aula até a data de entrega deste checkpoint, portanto não faz parte do escopo avaliado desta entrega.

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

> **Nota:** Testes automatizados com `dotnet test` não foram implementados neste checkpoint, pois este conteúdo não foi trabalhado em aula até a data de entrega.

Os fluxos críticos foram validados manualmente via **Swagger UI** e **Postman**, cobrindo todos os cenários obrigatórios:

| Fluxo Crítico | Validação | Status |
|---|---|---|
| Cadastro PF com CPF duplicado | Retorna `400` com mensagem | Correto |
| Cadastro PJ com CNPJ duplicado | Retorna `400` com mensagem | Correto |
| Vincular cliente a agência inexistente | Retorna `400` com mensagem | Correto |
| Contratação válida | Retorna `202 Accepted` com status `PENDENTE` | Correto |
| Contratação com cliente inexistente | Retorna `404` com mensagem | Correto |
| Consulta de status da contratação | Retorna contratação completa com cliente e produto | Correto |
| Empréstimo reprovado por score | Retorna `400` com score `REPROVADO` | Correto |
| Cálculo automático de parcela (fórmula Price) | `valorParcela` calculado automaticamente | Correto |

---

## 8. Mensageria (RabbitMQ)

> **Nota:** A implementação de filas com RabbitMQ não foi trabalhada em aula até a data de entrega deste checkpoint e não faz parte do escopo avaliado.

---

## 9. Evidências de Funcionamento (Swagger)

### **ProjetoBancoCP2:**

#### **Endpoints**

<img width="1919" height="1028" alt="image" src="https://github.com/user-attachments/assets/1dfbe529-24fd-4a5f-8a6a-446d2ac3bf75" />

<img width="1918" height="949" alt="image" src="https://github.com/user-attachments/assets/be415165-1b64-4f24-a674-06a4cc983c6d" />

---

Segue uma possível ordem para que não tenhamos problemas os testes

#### Agencias

---

**01 - Cadastrar Agencia (POST)**

<img width="1161" height="898" alt="image" src="https://github.com/user-attachments/assets/c237466f-c23a-4849-947a-980d66b8740d" />

---

**02 - Buscas Agencia por ID (GET)**

<img width="1157" height="777" alt="image" src="https://github.com/user-attachments/assets/4ac7c9d9-2d1b-416a-8323-8241162755cf" />

---

**03 - Buscar Agencia Inexistente - 404 (GET)**

<img width="1156" height="713" alt="image" src="https://github.com/user-attachments/assets/662d15e3-6c87-4c16-8f23-3b4aa33b2e1b" />

---

#### Clientes

---

**04 - Cadastrar PessoaFísica (POST)**

<img width="1081" height="916" alt="image" src="https://github.com/user-attachments/assets/3f21da95-bb84-4555-b918-b4572b413faf" />

---

**05 - Cadastrar PF - CPF Duplicado - 400 (POST)**

<img width="1079" height="914" alt="image" src="https://github.com/user-attachments/assets/69166c57-87d6-4744-86b2-448e01388e83" />

---

**06 - Cadastrar PessoaJuridica (POST)**

<img width="1078" height="922" alt="image" src="https://github.com/user-attachments/assets/2b7456e3-67ed-4a62-83e0-9a827aac1561" />

---

**07 - Cadastrar PJ - CNPJ Duplicado - 400 (POST)**

<img width="1080" height="922" alt="image" src="https://github.com/user-attachments/assets/d659c610-d962-46c1-90f2-ea6d083cbacf" />

---

**08 - Cadastrar PF - Agencia Inexistente - 400 (POST)**

<img width="1084" height="930" alt="image" src="https://github.com/user-attachments/assets/5aec9e11-d73f-4fd0-a9b5-fddf1e070d62" />

---

**09 - Buscar Cliente por ID (GET)**

<img width="1098" height="741" alt="image" src="https://github.com/user-attachments/assets/18eafede-a03d-454e-a868-e2983ecd388c" />

---

**10 - Buscar Cliente Inexistente - 404 (GET)**

<img width="1093" height="669" alt="image" src="https://github.com/user-attachments/assets/5dde7264-26fd-4786-8275-ec77d9b7d8f0" />

---

#### Emprestimos

---

**11 - Cadastrar Emprestimo - APROVADO (POST)**

<img width="1081" height="935" alt="image" src="https://github.com/user-attachments/assets/5d6cec95-ce45-41e0-8944-89f0e7743d62" />

---

**12 - Cadastrar Emprestimo - REPROVADO, score (POST)**

<img width="1077" height="941" alt="image" src="https://github.com/user-attachments/assets/389c95a0-ea4b-4941-ad9e-ac3dd3a21c4a" />

---

**13 - Buscar Emprestimo por ID (GET)**

<img width="1081" height="690" alt="image" src="https://github.com/user-attachments/assets/0740e143-2d0f-4247-ab62-910aeb4d8daf" />

---

**14 - Buscar Emprestimo Inexistente - 404 (GET)**

<img width="1081" height="667" alt="image" src="https://github.com/user-attachments/assets/495d8691-cde6-40b2-b249-00a3286b2895" />

---

#### Contratacoes

---

**15 - Solicitar Contratacao - 202 (POST)**

<img width="1085" height="940" alt="image" src="https://github.com/user-attachments/assets/6d5aecdb-fd32-4a37-b11d-355e40eed543" />

---

**16 - Contratar - Cliente Inexistente - 404 (POST)**

<img width="1080" height="912" alt="image" src="https://github.com/user-attachments/assets/4a35e1ee-593b-47da-9c9b-488bcb369ca4" />

---

**17 - Contratar - Produto Inexistente - 404 (POST)**

<img width="1081" height="913" alt="image" src="https://github.com/user-attachments/assets/7930122f-0f9f-4c24-ab17-4ae72c848405" />

---

**18 - Buscar Contratacao por ID (GET)**

<img width="1079" height="831" alt="image" src="https://github.com/user-attachments/assets/202b49a0-950b-426d-b7ab-f018f466b1cb" />

---

**19 - Buscar Contratacao Inexistente - 404 (GET)**

<img width="1083" height="671" alt="image" src="https://github.com/user-attachments/assets/85d98c80-0b6a-4d39-8095-6abeafbab4db" />

---

#### Agencia

---

**20 - Listar Todas as Agencias (GET)**

<img width="1088" height="703" alt="image" src="https://github.com/user-attachments/assets/d3550613-4a3a-4df8-bb5b-1e1cf082412e" />


**21 - Atualizar Agencia (PUT)**

<img width="1082" height="910" alt="image" src="https://github.com/user-attachments/assets/ef1ac39c-b763-477a-a47f-e77ddb37a8bd" />

---

#### Clientes

---

**22 - Listar Todas os Clientes (GET)**

<img width="1079" height="802" alt="image" src="https://github.com/user-attachments/assets/47a1a1d5-2f4c-4792-8cdc-139ce762f494" />

---

**23 - Atualizar PessoaFisica (PUT)**

<img width="1081" height="896" alt="image" src="https://github.com/user-attachments/assets/b4caceb5-9a66-4c5e-90fc-d863c2073816" />

---

**24 - Atualizar PF - ID Divergente - 400 (PUT)**

<img width="1079" height="930" alt="image" src="https://github.com/user-attachments/assets/f025e099-8a3e-4811-a87f-4a4c2c7184a2" />

---

**25 - Atualizar PF com ID de PJ - 404 (PUT)**

<img width="1082" height="932" alt="image" src="https://github.com/user-attachments/assets/d8257dc2-7d52-4709-8b09-be189a177b0b" />

---

**26 - Atualizar PessoaJuridica (PUT)**

<img width="1081" height="920" alt="image" src="https://github.com/user-attachments/assets/df2354b4-a0ee-4141-aa5b-45794da5b5e7" />

---

#### Emprestimos

---

**27 - Listar Todos os Emprestimos (GET)**

<img width="1106" height="667" alt="image" src="https://github.com/user-attachments/assets/acd83a97-bcb6-4558-b1a2-465267f3fff5" />

---

**28 - Atualizar Emprestimo (PUT)**

<img width="1099" height="936" alt="image" src="https://github.com/user-attachments/assets/dc51f885-387d-49e5-b6c9-0d06f7cb2724" />

---

#### Contratacoes

---

**29 - Listar Todas as Contratacoes (GET)**

<img width="1114" height="772" alt="image" src="https://github.com/user-attachments/assets/fe586a77-75ae-4f1f-86b2-c132b54758bb" />

---

**30 - Atualizar Status Contratacao - APROVADO (PUT)**

<img width="1095" height="921" alt="image" src="https://github.com/user-attachments/assets/bcb0c2b0-7613-4224-8a81-8364a2a00eb2" />

---

#### Delete

---

Para deletar, é recomendado remover:

Contratacao >> Emprestimos >> Clientes >> Agencias

- Delete a contratação antes do cliente e do empréstimo, senão vai dar erro de FK!
  
---

**31 - Deletar Contratacao (DEL)**

<img width="1078" height="568" alt="image" src="https://github.com/user-attachments/assets/f8524911-ca7b-4bb7-91b7-a598994f2a83" />
  
---

**32 - Deletar Emprestimo (DEL)**

<img width="1076" height="645" alt="image" src="https://github.com/user-attachments/assets/892170fa-06e5-4b4a-a4a7-84c0a01721d6" />
  
---

**33 - Deletar PessoaFisica (DEL)**

<img width="1072" height="569" alt="image" src="https://github.com/user-attachments/assets/8c435174-b7a7-48e4-89b6-3d9b05ba53c9" />
  
---

**34 - Deletar PessoaJuridica (DEL)**

<img width="1080" height="568" alt="image" src="https://github.com/user-attachments/assets/31f41a6f-fa43-45b5-ba36-df8c26e6e1f9" />
  
---

**35 - Deletar Agencia (DEL)**

<img width="1081" height="567" alt="image" src="https://github.com/user-attachments/assets/cbb9259a-acf8-441f-9029-f0f338c8bb82" />
  
---

> Tem as requisições disponíveis no Swagger (F5) e no Postman (docs/postman/)

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
