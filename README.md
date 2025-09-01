# MgFinanceiro - Backend

### Principais Funcionalidades

- **Autenticação**: Login e registro de usuários com tokens JWT.
- **Gerenciamento de Categorias**: Criação, consulta e atualização de status de categorias (Ativo/Inativo).
- **Transações**: Criação, exclusão e atualização de transações financeiras.
- **Relatórios**: Visualização e exportação de relatórios financeiros com filtros.
- **Validação**: Validações robustas para entradas de dados.
- **Log**: Registro estruturado de eventos para monitoramento e depuração.
- **Testes Unitários**: Cobertura de testes para serviços e regras de negócio, garantindo confiabilidade.

## Tecnologias Utilizadas

O projeto foi construído com as seguintes tecnologias e práticas:

- **.NET Core 8**: Framework principal para desenvolvimento da API.
- **Clean Architecture**: Estrutura organizada em camadas (Domain, Application, Infrastructure) para separação de responsabilidades e escalabilidade.
- **FluentValidation**: Biblioteca para validação de dados de entrada, garantindo consistência e robustez.
- **Serilog**: Ferramenta para logging estruturado, com saídas em console e arquivos JSON para facilitar monitoramento.
- **JWT (JSON Web Tokens)**: Utilizado para autenticação segura de usuários.
- **Riok.Mapperly**: Biblioteca de mapeamento estático, escolhida por seu alto desempenho em comparação com alternativas dinâmicas.
- **Swagger (Swashbuckle)**: Documentação interativa da API, acessível em `/swagger` no ambiente de desenvolvimento.
- **Entity Framework Core**: ORM para interação com o banco de dados SQL Server.
- **SQL Server**: Banco de dados relacional para armazenamento de dados.
- **xUnit**: Framework para testes unitários, garantindo a qualidade do código.
- **Docker**: Containerização da API, SQL Server e Adminer para gerenciamento do banco de dados.

## Estrutura do Projeto

O projeto segue a **Clean Architecture**, dividindo o código em camadas bem definidas:

- **Domain**: Contém entidades, interfaces e regras de negócio centrais.
- **Application**: Inclui serviços, DTOs (Data Transfer Objects) e lógica de aplicação, como orquestração de fluxos.
- **Infrastructure**: Responsável pela implementação de repositórios, acesso a banco de dados e integrações externas.
- **API (Controllers)**: Camada de apresentação, com endpoints RESTful que expõem as funcionalidades.
- **Tests**: Contém testes unitários para serviços, validadores e regras de negócio.


## Documentação da API

A API está totalmente documentada utilizando **Swagger**. Para acessar a documentação interativa:

1. Execute a aplicação em ambiente de desenvolvimento.
2. Acesse `https://localhost:<porta>/swagger`.

A documentação inclui:

- Detalhes de cada endpoint (métodos, parâmetros, respostas).
- Exemplos de requisições e respostas em JSON.
- Suporte a autenticação via Bearer Token (JWT).

## NOTA

Para utilizar completamente a API, é necessário estar autenticado. A autenticação é realizada por meio de um **Bearer Token** (JWT) incluído no cabeçalho `Authorization` das requisições. O token pode ser gerado através do endpoint de login, que autentica o usuário e retorna o token JWT. Inclua o token no formato `Bearer <token>` em todas as requisições aos endpoints protegidos.

**Exemplo de uso**:

1. Realize o login via endpoint (ex.: `POST /api/Auth/login`) com credenciais válidas.
2. Receba o token JWT na resposta.
3. Adicione o token ao cabeçalho `Authorization` das requisições:

```http
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

**Observação**: Certifique-se de que o token não esteja expirado. Caso expire, realize um novo login para obter um token válido.

## Decisões Técnicas

### Clean Architecture

A escolha pela **Clean Architecture** foi feita para garantir:

- **Escalabilidade**: Facilita a adição de novas funcionalidades sem impactar camadas existentes.
- **Manutenibilidade**: Separação clara de responsabilidades torna o código mais legível e fácil de testar.
- **Testabilidade**: Interfaces bem definidas permitem a criação de testes unitários e de integração.

### FluentValidation

- Utilizado para validação de dados de entrada (DTOs) nos endpoints.
- Garante que as requisições atendam a regras de negócio antes de serem processadas.
- Configurado para validação automática nos controladores, reduzindo código boilerplate.

### Serilog

- Implementado para logging estruturado, com saídas em console e arquivos JSON.
- Configuração inclui rotação diária de logs e retenção de 7 dias.
- Formato JSON compacto para facilitar parsing em ferramentas de monitoramento.

### JWT

- Autenticação baseada em tokens JWT para segurança.
- Configuração inclui validação de emissor, audiência e chave de assinatura.
- Tokens com expiração controlada e sem atraso padrão (`ClockSkew = 0`).

### Riok.Mapperly

- Escolhido por ser um mapeador estático, oferecendo melhor desempenho em comparação com mapeadores dinâmicos (como
  AutoMapper).
- Gera código em tempo de compilação, reduzindo overhead em runtime.

### Entity Framework Core

- Utilizado para acesso ao banco de dados SQL Server.
- Configurado com retry em caso de falhas temporárias, aumentando a resiliência.

### iText

- Utilizado para geração de arquivos PDF nos endpoints de exportação de relatórios (`/api/Relatorios/resumo/export` e `/api/Relatorios/por-categoria/export`).
- Escolhido por sua robustez e flexibilidade na criação de documentos PDF, permitindo formatação personalizada, como tabelas e cabeçalhos, com suporte a layouts complexos.

### ClosedXML

- Utilizado para geração de arquivos Excel (formato `.xlsx`) nos endpoints de exportação de relatórios.
- Escolhido por sua simplicidade e capacidade de criar planilhas Excel sem dependência de bibliotecas externas ou instalação do Microsoft Excel, garantindo compatibilidade multiplataforma.

### Testes Unitários (xUnit e Shouldly)
- Testes unitários implementados para serviços, validadores e regras de negócio.
- Garante a confiabilidade e integridade do código, cobrindo casos de uso principais e cenários de erro.
- Localizados na pasta `Tests`, utilizando o framework xUnit e Shouldly.

### Docker
- O projeto inclui um `Dockerfile` para containerizar a API e um `docker-compose.yml` para orquestrar a API, o SQL Server e o Adminer.
- O script `docker-compose.yml` cria automaticamente o banco de dados `MgFinanceiroDb` e o popula com dados iniciais via `init.sql`.

## Configuração e Execução
### Pré-requisitos

- **.NET 8 SDK** (ou versão compatível).
- **SQL Server** (local ou remoto).
- Pacotes NuGet necessários:
    - `Microsoft.EntityFrameworkCore`
    - `Microsoft.AspNetCore.Authentication.JwtBearer`
    - `Swashbuckle.AspNetCore`
    - `FluentValidation.AspNetCore`
    - `Serilog.AspNetCore`
    - `Serilog.Sinks.File`
    - `Serilog.Formatting.Compact`
    - `Riok.Mapperly`
    - `Itext`
    - `ClosedXml`

### Passos Para Executar com Docker
1. **Clone o repositório**:
   ```
   git clone https://github.com/JulioG516/MgFinanceiro
   cd MgFinanceiro
   ```

2. **Execute com Docker Compose**:
    - Certifique-se de que o Docker e o Docker Compose estão instalados.
    - Execute o comando:
      ```
      docker-compose up -d
      ```
    - O `docker-compose.yml` sobe os seguintes serviços:
        - **mgfinanceiro-api**: A API rodando na porta `8080`.
        - **sqlserver**: Instância do SQL Server 2022 na porta `1433`, com banco de dados `MgFinanceiroDb` criado e populado via `init.sql`.
        - **adminer**: Interface web para gerenciamento do banco, acessível em `http://localhost:8090`.

3. **Acesse a API**:
    - API: `http://localhost:8080/api`
    - Swagger: `http://localhost:8080/swagger`
    - Adminer: `http://localhost:8090` (use `sqlserver` como servidor, `sa` como usuário e `MgFinanceiroSenha123Forte` como senha).

4. **Parar os containers**:
   ```
   docker-compose down
   ```



### Passos para Executar sem Docker

1. **Clone o repositório**:
   ```bash
   git clone https://github.com/JulioG516/MgFinanceiro
   cd MgFinanceiro
   ```

## Configuração do Banco de Dados


### Atualize a String de Conexão
No arquivo `appsettings.json`, configure a string de conexão do banco de dados para corresponder ao seu ambiente. Substitua `<seu-servidor>` pelo endereço do seu servidor SQL Server e ajuste outros parâmetros, conforme necessário:

```json
"ConnectionStrings": {
  "SqlServerConnection": "Server=<seu-servidor>;Database=MgFinanceiro;Trusted_Connection=True;MultipleActiveResultSets=True;"
}
```

**Notas**:
- **Autenticação**: Use `Trusted_Connection=True` para autenticação do Windows. Para autenticação SQL Server, substitua por `User Id=<seu-usuario>;Password=<sua-senha>;`.
- **MultipleActiveResultSets**: O parâmetro `MultipleActiveResultSets=True` é recomendado para suportar múltiplas consultas simultâneas.
- **Banco de Dados**: Certifique-se de que o banco de dados `MgFinanceiro` já existe ou será criado nos passos a seguir.
- **Segurança**: Evite armazenar senhas diretamente no `appsettings.json` em ambientes de produção. Considere usar variáveis de ambiente ou um gerenciador de segredos.

### Escolha uma Opção para Configurar o Esquema do Banco de Dados

#### Opção 1: Usar Migrações do Entity Framework Core
O Entity Framework Core permite criar ou atualizar o banco de dados automaticamente com base nos modelos definidos no projeto. Execute os seguintes comandos no terminal, na pasta do projeto:

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

**Explicação**:
- `dotnet ef migrations add InitialCreate`: Gera os arquivos de migração com base nos modelos do Entity Framework, criando um snapshot inicial do esquema do banco de dados.
- `dotnet ef database update`: Aplica as migrações ao banco de dados, criando ou atualizando as tabelas conforme necessário.
- **Pré-requisitos**: Certifique-se de que o pacote `Microsoft.EntityFrameworkCore.Design` está instalado e que o projeto contém um `DbContext` configurado.

#### Opção 2: Usar o Script `init.sql`
Como alternativa, você pode usar o arquivo `init.sql` para criar o esquema do banco de dados manualmente. Este método é útil para ambientes onde as migrações automáticas não são desejadas ou quando você prefere controle total sobre os scripts SQL.

**Passos**:
1. Localize o arquivo `init.sql` na pasta raiz do projeto.
2. Abra o SQL Server Management Studio (SSMS) ou outra ferramenta compatível com SQL Server.
3. Conecte-se ao servidor especificado na string de conexão.
4. Execute o script `init.sql` para criar o banco de dados, as tabelas, índices e outros objetos necessários.

3. **Configure as chaves JWT**:
    - No arquivo `appsettings.json`, configure as chaves JWT:
      ```json
      "JwtSettings": {
        "SecretKey": "<sua-chave-secreta>",
        "Issuer": "MgFinanceiro",
        "Audience": "MgFinanceiro"
      }
      ```

4. **Execute a aplicação**:
   ```bash
   dotnet restore
   dotnet run
   ```

5. **Acesse a API**:
    - API: `https://localhost:<porta>/api`
    - Swagger: `https://localhost:<porta>/swagger`



## Endpoints Principais

### 🔐 Autenticação

#### `POST /api/Auth/login`

Autentica um usuário e retorna um token JWT.

**Descrição**: Autentica um usuário com base nas credenciais fornecidas e retorna um token JWT.

**Request Body**:

```json
{
  "email": "string",
  "senha": "string"
}
```

**Responses**:

- **200 OK**: Autenticação bem-sucedida

```json
{
  "token": "string",
  "nome": "string",
  "email": "string",
  "expiresAt": "2025-08-31T12:00:00Z"
}
```

- **401 Unauthorized**: Credenciais inválidas

#### `POST /api/Auth/registrar`

Registra um novo usuário.

**Descrição**: Cria um novo usuário com base nos dados fornecidos: nome, e-mail e senha.

**Request Body**:

```json
{
  "nome": "string",
  "email": "string",
  "senha": "string"
}
```

**Responses**:

- **201 Created**: Usuário criado com sucesso

```json
{
  "id": 1,
  "nome": "string",
  "email": "string",
  "dataCriacao": "2025-08-31T12:00:00Z",
  "ultimoLogin": "2025-08-31T12:00:00Z"
}
```

- **400 Bad Request**: Dados inválidos

---

### 📂 Categorias

#### `GET /api/Categorias`

Lista todas as categorias.

**Descrição**: Retorna todas as categorias cadastradas. Pode ser filtrado por tipo (1 - Receita ou 2 - Despesa).

**Query Parameters**:

- `tipoCategoria` (opcional): Tipo da categoria para filtro
    - `1`: Receita
    - `2`: Despesa
- `statusCategoriaAtivo` (opcional): Define se devem ser retornadas apenas categorias ativas (true) ou apenas inativas (
  false). Se não informado, retorna todas.

**Responses**:

- **200 OK**: Lista de categorias

```json
[
  {
    "id": 1,
    "nome": "string",
    "tipo": "string",
    "ativo": true,
    "dataCriacao": "2025-08-31T12:00:00Z"
  }
]
```

#### `GET /api/Categorias/{id}`

Obtém uma categoria por ID.

**Descrição**: Retorna os detalhes de uma categoria específica com base no ID fornecido.

**Path Parameters**:

- `id` (required): ID da categoria

**Query Parameters**:

- `id` (required): ID da categoria a ser buscada

**Responses**:

- **200 OK**: Detalhes da categoria
- **404 Not Found**: Categoria não encontrada

#### `POST /api/Categorias`

Cria uma nova categoria.

**Descrição**: Cria uma nova categoria com nome e tipo (Receita ou Despesa).

**Request Body**:

```json
{
  "nome": "string",
  "tipo": "string"
}
```

**Responses**:

- **201 Created**: Categoria criada com sucesso
- **400 Bad Request**: Dados inválidos

#### `PATCH /api/Categorias/{id}/status`

Atualiza o status de uma categoria.

**Descrição**: Atualiza o status (ativo ou inativo) de uma categoria específica com base no ID fornecido.

**Path Parameters**:

- `id` (required): ID da categoria a ser atualizada

**Request Body**:

```json
{
  "id": 1,
  "ativo": true
}
```

**Responses**:

- **200 OK**: Status atualizado com sucesso
- **400 Bad Request**: Dados inválidos
- **404 Not Found**: Categoria não encontrada

---

### 💰 Transações

#### `GET /api/Transacoes`

Lista todas as transações.

**Descrição**: Retorna todas as transações cadastradas. Pode ser filtrado por período (dataInicio e dataFim),
categoria (categoriaId) e tipo de categoria (1 - Receita ou 2 - Despesa).

**Query Parameters**:

- `dataInicio` (opcional): Data inicial para filtro das transações (formato ISO 8601, ex.: 2025-08-01T00:00:00Z)
- `dataFim` (opcional): Data final para filtro das transações (formato ISO 8601, ex.: 2025-08-31T23:59:59Z)
- `categoriaId` (opcional): ID da categoria para filtro das transações
- `tipoCategoria` (opcional): Tipo da categoria para filtro
    - `1`: Receita
    - `2`: Despesa

**Responses**:

- **200 OK**: Lista de transações

```json
[
  {
    "id": 1,
    "descricao": "string",
    "valor": 0.0,
    "data": "2025-08-31T12:00:00Z",
    "categoriaId": 1,
    "categoriaNome": "string",
    "categoriaTipo": "string",
    "observacoes": "string",
    "dataCriacao": "2025-08-31T12:00:00Z"
  }
]
```

- **400 Bad Request**: Parâmetros inválidos

#### `GET /api/Transacoes/{id}`

Obtém uma transação por ID.

**Descrição**: Retorna os detalhes de uma transação específica com base no ID fornecido.

**Path Parameters**:

- `id` (required): ID da transação

**Responses**:

- **200 OK**: Detalhes da transação
- **404 Not Found**: Transação não encontrada

#### `POST /api/Transacoes`

Cria uma nova transação.

**Descrição**: Cria uma nova transação com descrição, valor, data, categoria e observações opcionais.

**Request Body**:

```json
{
  "descricao": "string",
  "valor": 0.0,
  "data": "2025-08-31T12:00:00Z",
  "categoriaId": 1,
  "observacoes": "string"
}
```

**Responses**:

- **201 Created**: Transação criada com sucesso
- **400 Bad Request**: Dados inválidos

#### `PUT /api/Transacoes/{id}`

Atualiza uma transação.

**Descrição**: Atualiza os detalhes de uma transação existente com base no ID fornecido.

**Path Parameters**:

- `id` (required): ID da transação a ser atualizada

**Request Body**:

```json
{
  "id": 1,
  "descricao": "string",
  "valor": 0.0,
  "data": "2025-08-31T12:00:00Z",
  "categoriaId": 1,
  "observacoes": "string"
}
```

**Responses**:

- **204 No Content**: Transação atualizada com sucesso
- **400 Bad Request**: Dados inválidos

#### `DELETE /api/Transacoes/{id}`

Exclui uma transação.

**Descrição**: Exclui uma transação existente com base no ID fornecido.

**Path Parameters**:

- `id` (required): ID da transação a ser excluída

**Responses**:

- **204 No Content**: Transação excluída com sucesso
- **400 Bad Request**: Erro na exclusão

---

### 📊 Relatórios

#### `GET /api/Relatorios/resumo`

Resumo de transações por período.

**Descrição**: Retorna um resumo com saldo total, receitas e despesas para o ano atual (todos os meses) ou para um
mês/ano específico.

**Query Parameters**:

- `ano` (opcional): Ano para filtro (ex.: 2024). Opcional, padrão é o ano atual
- `mes` (opcional): Mês para filtro (1 a 12). Opcional, se não fornecido, retorna todos os meses do ano

**Responses**:

- **200 OK**: Resumo das transações

```json
[
  {
    "ano": 2025,
    "mes": 8,
    "saldoTotal": 0.0,
    "totalReceitas": 0.0,
    "totalDespesas": 0.0
  }
]
```

- **400 Bad Request**: Parâmetros inválidos


#### `GET /api/Relatorios/resumo/export`

Exporta um resumo de transações.

**Descrição**: Gera e retorna um arquivo com o resumo de transações no formato PDF ou Excel para o período especificado.

**Query Parameters**:

- `ano` (opcional): Ano para filtro (ex.: 2024). Opcional, padrão é o ano atual
- `mes` (opcional): Mês para filtro (1 a 12). Opcional, se não fornecido, retorna todos os meses do ano
- `formato` (obrigatório): Formato do arquivo a ser exportado (`pdf` ou `xlsx`)

**Responses**:

- **200 OK**: Arquivo PDF ou Excel com o resumo das transações
    - Content-Type: `application/pdf` (para formato=pdf) ou `application/vnd.openxmlformats-officedocument.spreadsheetml.sheet` (para formato=xlsx)
    - Content-Disposition: `attachment; filename=Relatorio_Resumo_YYYYMMDDHHMMSS.[pdf|xlsx]`

- **400 Bad Request**: Parâmetros inválidos ou formato inválido

#### `GET /api/Relatorios/por-categoria`

Transações por categoria.

**Descrição**: Retorna transações agrupadas por categoria para o ano atual (todos os meses) ou para um mês/ano
específico.

**Query Parameters**:

- `ano` (opcional): Ano para filtro (ex.: 2024). Opcional, padrão é o ano atual
- `mes` (opcional): Mês para filtro (1 a 12). Opcional, se não fornecido, retorna todos os meses do ano

**Responses**:

- **200 OK**: Transações agrupadas por categoria

```json
[
  {
    "ano": 2025,
    "mes": 8,
    "categoriaId": 1,
    "categoriaNome": "string",
    "categoriaTipo": "string",
    "total": 0.0
  }
]
```

- **400 Bad Request**: Parâmetros inválidos

#### `GET /api/Relatorios/por-categoria/export`

Exporta transações por categoria.

**Descrição**: Gera e retorna um arquivo com transações agrupadas por categoria no formato PDF ou Excel para o período especificado.

**Query Parameters**:

- `ano` (opcional): Ano para filtro (ex.: 2024). Opcional, padrão é o ano atual
- `mes` (opcional): Mês para filtro (1 a 12). Opcional, se não fornecido, retorna todos os meses do ano
- `formato` (obrigatório): Formato do arquivo a ser exportado (`pdf` ou `xlsx`)

**Responses**:

- **200 OK**: Arquivo PDF ou Excel com as transações por categoria
    - Content-Type: `application/pdf` (para formato=pdf) ou `application/vnd.openxmlformats-officedocument.spreadsheetml.sheet` (para formato=xlsx)
    - Content-Disposition: `attachment; filename=Relatorio_PorCategoria_YYYYMMDDHHMMSS.[pdf|xlsx]`

- **400 Bad Request**: Parâmetros inválidos ou formato inválido

---

## Contato

- **E-mail**: juliogabriel516@gmail.com