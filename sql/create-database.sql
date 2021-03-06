IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [FORNECEDORES] (
    [ID] uniqueidentifier NOT NULL,
    [NOME] varchar(200) NOT NULL,
    [DOCUMENTO] varchar(14) NOT NULL,
    [TIPO_FORNECEDOR] int NOT NULL,
    [ATIVO] bit NOT NULL,
    CONSTRAINT [PK_FORNECEDORES] PRIMARY KEY ([ID])
);
GO

CREATE TABLE [ENDERECOS] (
    [ID] uniqueidentifier NOT NULL,
    [FORNECEDOR_ID] uniqueidentifier NOT NULL,
    [LOGRADOURO] varchar(200) NOT NULL,
    [NUMERO] varchar(50) NOT NULL,
    [COMPLEMENTO] varchar(250) NULL,
    [CEP] varchar(8) NOT NULL,
    [BAIRRO] varchar(100) NOT NULL,
    [CIDADE] varchar(100) NOT NULL,
    [ESTADO] varchar(50) NOT NULL,
    CONSTRAINT [PK_ENDERECOS] PRIMARY KEY ([ID]),
    CONSTRAINT [FK_ENDERECOS_FORNECEDORES_FORNECEDOR_ID] FOREIGN KEY ([FORNECEDOR_ID]) REFERENCES [FORNECEDORES] ([ID]) ON DELETE NO ACTION
);
GO

CREATE TABLE [PRODUTOS] (
    [ID] uniqueidentifier NOT NULL,
    [FORNECEDOR_ID] uniqueidentifier NOT NULL,
    [NOME] varchar(200) NOT NULL,
    [DESCRICAO] varchar(1000) NOT NULL,
    [IMAGEM] varchar(100) NOT NULL,
    [VALOR] decimal(18,2) NOT NULL,
    [DATA_CADASTRO] datetime2 NOT NULL,
    [ATIVO] bit NOT NULL,
    CONSTRAINT [PK_PRODUTOS] PRIMARY KEY ([ID]),
    CONSTRAINT [FK_PRODUTOS_FORNECEDORES_FORNECEDOR_ID] FOREIGN KEY ([FORNECEDOR_ID]) REFERENCES [FORNECEDORES] ([ID]) ON DELETE NO ACTION
);
GO

CREATE UNIQUE INDEX [IX_ENDERECOS_FORNECEDOR_ID] ON [ENDERECOS] ([FORNECEDOR_ID]);
GO

CREATE INDEX [IX_PRODUTOS_FORNECEDOR_ID] ON [PRODUTOS] ([FORNECEDOR_ID]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20220205055849_InitialMigration', N'6.0.1');
GO

COMMIT;
GO

