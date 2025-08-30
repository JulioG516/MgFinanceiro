SET NOCOUNT ON;
GO

SET QUOTED_IDENTIFIER ON;
GO

IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'MgFinanceiroDb')
CREATE DATABASE MgFinanceiroDb;
GO

USE MgFinanceiroDb;
GO

BEGIN TRY
BEGIN TRANSACTION;

IF OBJECT_ID(N'[FK_Transacoes_Categorias_CategoriaId]', 'F') IS NOT NULL
ALTER TABLE [Transacoes] DROP CONSTRAINT [FK_Transacoes_Categorias_CategoriaId];

IF OBJECT_ID(N'[Transacoes]') IS NOT NULL
DROP TABLE [Transacoes];

IF OBJECT_ID(N'[Categorias]') IS NOT NULL
DROP TABLE [Categorias];

IF OBJECT_ID(N'[Usuarios]') IS NOT NULL
DROP TABLE [Usuarios];

IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NOT NULL
DROP TABLE [__EFMigrationsHistory];

CREATE TABLE [__EFMigrationsHistory] (
    [MigrationId] nvarchar(150) NOT NULL,
    [ProductVersion] nvarchar(32) NOT NULL,
    CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );

CREATE TABLE [Categorias] (
    [Id] int NOT NULL IDENTITY,
    [Nome] nvarchar(100) NOT NULL,
    [Tipo] int NOT NULL,
    [Ativo] bit NOT NULL,
    [DataCriacao] datetime2 NOT NULL,
    CONSTRAINT [PK_Categorias] PRIMARY KEY ([Id])
    );

CREATE TABLE [Transacoes] (
    [Id] int NOT NULL IDENTITY,
    [Descricao] nvarchar(200) NOT NULL,
    [Valor] decimal(18,2) NOT NULL,
    [Data] datetime2 NOT NULL,
    [CategoriaId] int NOT NULL,
    [Observacoes] nvarchar(500) NULL,
    [DataCriacao] datetime2 NOT NULL,
    CONSTRAINT [PK_Transacoes] PRIMARY KEY ([Id])
    );

CREATE TABLE [Usuarios] (
    [Id] int NOT NULL IDENTITY,
    [Nome] nvarchar(100) NOT NULL,
    [Email] nvarchar(255) NOT NULL,
    [SenhaHash] nvarchar(max) NOT NULL,
    [DataCriacao] datetime2 NOT NULL,
    [UltimoLogin] datetime2 NULL,
    CONSTRAINT [PK_Usuarios] PRIMARY KEY ([Id])
    );

ALTER TABLE [Transacoes] ADD CONSTRAINT [FK_Transacoes_Categorias_CategoriaId] FOREIGN KEY ([CategoriaId]) REFERENCES [Categorias] ([Id]) ON DELETE CASCADE;

SET IDENTITY_INSERT [Categorias] ON;
INSERT INTO [Categorias] ([Id], [Ativo], [DataCriacao], [Nome], [Tipo])
VALUES (1, CAST(1 AS bit), '2025-08-01T00:00:00.0000000Z', N'Vendas de Produtos', 1),
       (2, CAST(1 AS bit), '2025-08-01T00:00:00.0000000Z', N'Prestação de Serviços', 1),
       (3, CAST(1 AS bit), '2025-08-01T00:00:00.0000000Z', N'Juros e Rendimentos Financeiros', 1),
       (4, CAST(1 AS bit), '2025-08-01T00:00:00.0000000Z', N'Outras Receitas Operacionais', 1),
       (5, CAST(1 AS bit), '2025-08-01T00:00:00.0000000Z', N'Salários e Encargos', 2),
       (6, CAST(1 AS bit), '2025-08-01T00:00:00.0000000Z', N'Aluguel e Manutenção Predial', 2),
       (7, CAST(1 AS bit), '2025-08-01T00:00:00.0000000Z', N'Compras de Mercadorias', 2),
       (8, CAST(1 AS bit), '2025-08-01T00:00:00.0000000Z', N'Impostos e Taxas', 2),
       (9, CAST(1 AS bit), '2025-08-01T00:00:00.0000000Z', N'Despesas com Marketing', 2),
       (10, CAST(1 AS bit), '2025-08-01T00:00:00.0000000Z', N'Despesas Administrativas', 2);
SET IDENTITY_INSERT [Categorias] OFF;

SET IDENTITY_INSERT [Transacoes] ON;
INSERT INTO [Transacoes] ([Id], [CategoriaId], [Data], [DataCriacao], [Descricao], [Observacoes], [Valor])
VALUES (1, 1, '2025-08-23T00:00:00.0000000Z', '2025-08-28T13:40:52.4186124Z', N'Venda de produtos para cliente A', NULL, 1500.0),
       (2, 2, '2025-08-25T00:00:00.0000000Z', '2025-08-28T13:40:52.4186581Z', N'Prestação de serviço de consultoria', NULL, 800.0),
       (3, 5, '2025-08-18T00:00:00.0000000Z', '2025-08-28T13:40:52.4186583Z', N'Pagamento de salários mensais', NULL, 2000.0),
       (4, 7, '2025-08-26T00:00:00.0000000Z', '2025-08-28T13:40:52.4186584Z', N'Compra de estoque', NULL, 1200.0),
       (5, 4, '2025-07-10T00:00:00.0000000Z', '2025-08-28T14:15:33.2602063Z', N'Receita de aluguel de equipamento', NULL, 600.0),
       (6, 6, '2025-07-05T00:00:00.0000000Z', '2025-08-28T14:15:33.2602064Z', N'Pagamento de aluguel mensal', NULL, 900.0),
       (7, 1, '2025-09-12T00:00:00.0000000Z', '2025-08-28T14:15:33.2602065Z', N'Venda de produtos para cliente B', NULL, 2300.0),
       (8, 9, '2025-09-15T00:00:00.0000000Z', '2025-08-28T14:15:33.2602066Z', N'Campanha de marketing online', NULL, 500.0),
       (9, 3, '2025-08-30T00:00:00.0000000Z', '2025-08-28T14:15:33.2602067Z', N'Juros recebidos de aplicação', NULL, 300.0),
       (10, 8, '2025-07-20T00:00:00.0000000Z', '2025-08-28T14:15:33.2602078Z', N'Pagamento de impostos municipais', NULL, 400.0),
       (11, 6, '2025-06-25T00:00:00.0000000Z', '2025-08-28T14:15:33.2602079Z', N'Serviço de manutenção de equipamentos', NULL, 700.0),
       (12, 1, '2025-06-15T00:00:00.0000000Z', '2025-08-28T14:15:33.2602080Z', N'Venda de produtos para cliente C', NULL, 1800.0),
       (13, 10, '2025-08-10T00:00:00.0000000Z', '2025-08-28T14:15:33.2602081Z', N'Despesas com material de escritório', NULL, 200.0),
       (14, 2, '2025-09-05T00:00:00.0000000Z', '2025-08-28T14:15:33.2602082Z', N'Receita de consultoria estratégica', NULL, 1200.0),
       (15, 7, '2025-07-28T00:00:00.0000000Z', '2025-08-28T14:15:33.2602083Z', N'Compra de matérias-primas', NULL, 1500.0),
       (16, 8, '2025-06-30T00:00:00.0000000Z', '2025-08-28T14:15:33.2602084Z', N'Pagamento de taxas estaduais', NULL, 350.0),
       (17, 4, '2025-08-05T00:00:00.0000000Z', '2025-08-28T14:15:33.2602085Z', N'Receita de royalties', NULL, 1000.0),
       (18, 6, '2025-09-20T00:00:00.0000000Z', '2025-08-28T14:15:33.2602086Z', N'Manutenção de veículos da empresa', NULL, 600.0),
       (19, 1, '2025-07-15T00:00:00.0000000Z', '2025-08-28T14:15:33.2602087Z', N'Venda de produtos para cliente D', NULL, 2500.0),
       (20, 10, '2025-08-12T00:00:00.0000000Z', '2025-08-28T14:15:33.2602089Z', N'Despesas com treinamento de equipe', NULL, 800.0);
SET IDENTITY_INSERT [Transacoes] OFF;

SET IDENTITY_INSERT [Usuarios] ON;
INSERT INTO [Usuarios] ([Id], [DataCriacao], [Email], [Nome], [SenhaHash], [UltimoLogin])
VALUES (1, '2025-01-01T00:00:00.0000000Z', N'teste@exemplo.com', N'Jose Teste',
        N'$2a$11$XCcSVqyjoEBm8AQ/gsklV.zOihn8RisCV2OVT.c7StBOaNfpRMATi', NULL);
SET IDENTITY_INSERT [Usuarios] OFF;

CREATE INDEX [IX_Transacoes_CategoriaId] ON [Transacoes] ([CategoriaId]);
CREATE INDEX [IX_Transacoes_Data] ON [Transacoes] ([Data]);
CREATE UNIQUE INDEX [IX_Categorias_Nome_Tipo] ON [Categorias] ([Nome], [Tipo]) WHERE Ativo = 1;
CREATE UNIQUE INDEX [IX_Usuarios_Email] ON [Usuarios] ([Email]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
SELECT N'20250826231900_InitialSqlServer', N'9.0.8'
WHERE NOT EXISTS (SELECT 1 FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20250826231900_InitialSqlServer');

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
SELECT N'20250826235300_AddIndexes', N'9.0.8'
WHERE NOT EXISTS (SELECT 1 FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20250826235300_AddIndexes');

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
SELECT N'20250827000200_AddLengthConstraints', N'9.0.8'
WHERE NOT EXISTS (SELECT 1 FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20250827000200_AddLengthConstraints');

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
SELECT N'20250828134052_SeedDataStatic', N'9.0.8'
WHERE NOT EXISTS (SELECT 1 FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20250828134052_SeedDataStatic');

COMMIT TRANSACTION;
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0
ROLLBACK TRANSACTION;

THROW;
END CATCH;
GO