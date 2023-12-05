CREATE DATABASE Estoque
USE Estoque

CREATE TABLE Produto (
	IdProd VARCHAR(5),
	Nome VARCHAR(50),
	Descricao VARCHAR(500),

	PRIMARY KEY (IdProd),
)

CREATE TABLE Cliente (
	IdCli VARCHAR(5),
	Nome VARCHAR(100),
	CPF VARCHAR(15),

	PRIMARY KEY (IdCli),
)

CREATE TABLE Venda (
	IdProd VARCHAR(5),
	IdCli VARCHAR(5),
	DataVenda DATETIME,

	PRIMARY KEY (IdProd, IdCli),
	FOREIGN KEY (IdProd) REFERENCES Produto,
	FOREIGN KEY (IdCli) REFERENCES Cliente,
)


CREATE TABLE OrdemServico (
	IdSer INT IDENTITY,
	IdCli VARCHAR(5),
	IdProd VARCHAR(5),

	PRIMARY KEY (IdSer, IdCli, IdProd),
	FOREIGN KEY (IdProd) REFERENCES Produto,
	FOREIGN KEY (IdCli) REFERENCES Cliente,
)



CREATE TABLE Estoque (
	IdProd INT,
	Quantidade INT,

	PRIMARY KEY (IdProd),
)


CREATE PROCEDURE uspGerirProduto
	@Acao INT,
	@IdProd VARCHAR(5) = NULL,
	@Nome VARCHAR(50) = NULL,
	@Descricao VARCHAR(500) = NULL

AS
BEGIN
	IF (@Acao = 1) -- INSERIR
		BEGIN
			INSERT INTO Produto (IdProd,
								 Nome,
								 Descricao)
						VALUES  (@IdProd,
								 @Nome,
								 @Descricao);

								 SELECT @IdProd AS IdProd,
										@Nome AS Nome, 
								        @Descricao AS Descricao
		END
	ELSE IF (@Acao = 2) -- ALTERAR
		BEGIN 
			UPDATE Produto
				SET IdProd = @IdProd,
					Nome = @Nome,
					Descricao = @Descricao

					WHERE IdProd = @IdProd

					SELECT @IdProd AS IdProd,
						   @Nome AS Nome, 
			    	       @Descricao AS Descricao
		END
	ELSE IF (@Acao = 3) 
		BEGIN
			DELETE FROM Produto
			WHERE IdProd = @IdProd
		END
	ELSE
		BEGIN 
			RAISERROR('Ação Inválida. Verifique e tente novamente', 14, 1)
		END
END


DROP PROCEDURE uspGerirProduto
EXEC uspGerirProduto 1, '120', 'Roteador', 'Roteador 300 mpbs'
