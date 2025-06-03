/* ################################### */
/* ############# TABELAS ############# */
/* ################################### */

CREATE TABLE Usuarios (
    id INT PRIMARY KEY,
    nome_usuario VARCHAR(50) NOT NULL,
	login_usuario VARCHAR(50) NOT NULL,
    senha_usuario VARCHAR(50) NOT NULL,
    flag_admin TINYINT NOT NULL
);

CREATE TABLE Alteracoes (
    id INT PRIMARY KEY,
    id_usuario INT NOT NULL,
    data_alteracao DATETIME NOT NULL,
    setpoint DECIMAL(5, 2) NOT NULL,
    config_ma_mf VARCHAR(2) NOT NULL, /* Inserir as opções "MA" e "MF" */
    descricao_alteracao VARCHAR(100),
    FOREIGN KEY (id_usuario) REFERENCES Usuarios(id)
);

CREATE TABLE Kits (
	id INT PRIMARY KEY,
	nome VARCHAR(20) NOT NULL,
	situacao VARCHAR(20) NOT NULL, /* Inserir as opções "Em Manutenção", "Com Defeito", "Em Funcionamento" */
	data_ultima_manutencao DATETIME,
	descricao_equipamento VARCHAR(100),
	preco_equipamento MONEY NOT NULL,
	imagem VARBINARY(MAX) NULL
);

/* ################################### */
/* ######## STORED PROCEDURES ######## */
/* ################################### */

/* #### Genéricas #### */
create procedure spDelete
(
 @id int ,
 @tabela varchar(max)
)
as
begin
 declare @sql varchar(max);
 set @sql = ' delete ' + @tabela +
 ' where id = ' + cast(@id as varchar(max))
 exec(@sql)
end
go
create procedure spConsulta
(
 @id int ,
 @tabela varchar(max)
)
as
begin
 declare @sql varchar(max);
 set @sql = 'select * from ' + @tabela +
 ' where id = ' + cast(@id as varchar(max))
 exec(@sql)
end
go
create procedure spListagem
(
 @tabela varchar(max),
 @ordem varchar(max))
as
begin
 exec('select * from ' + @tabela +
 ' order by ' + @ordem)
end
go
create procedure spProximoId
(@tabela varchar(max))
as
begin
 exec('select isnull(max(id) +1, 1) as MAIOR from '
+@tabela)
end
go
/* #### Específicas #### */

/* Usuários */
create procedure spInsert_Usuarios
(
 @id int,
 @nome_usuario varchar(max),
 @login_usuario varchar(max),
 @senha_usuario varchar(max),
 @flag_admin int
)
as
begin
 insert into Usuarios 
 (id, nome_usuario, login_usuario, senha_usuario, flag_admin)
 values
 (@id, @nome_usuario, @login_usuario, @senha_usuario, @flag_admin)
end
go
create procedure spUpdate_Usuarios
(
 @id int,
 @nome_usuario varchar(max),
 @login_usuario varchar(max),
 @senha_usuario varchar(max),
 @flag_admin int
)
as
begin
 update Usuarios set 
 nome_usuario = @nome_usuario,
 login_usuario = @login_usuario,
 senha_usuario = @senha_usuario,
 flag_admin = @flag_admin
 where id = @id 
end
go
CREATE PROCEDURE spConsultaPorLogin
    @login_usuario VARCHAR(50)
AS
BEGIN
    SELECT * FROM Usuarios
    WHERE login_usuario = @login_usuario
END
go
/* Alterações */
create procedure spInsert_Alteracoes
(
 @id int,
 @id_usuario int,
 @data_alteracao varchar(max),
 @setpoint decimal(5, 2),
 @config_ma_mf varchar(max),
 @descricao_alteracao varchar(max)
)
as
begin
 insert into Alteracoes 
 (id, id_usuario, data_alteracao, setpoint, config_ma_mf, descricao_alteracao)
 values
 (@id, @id_usuario, @data_alteracao, @setpoint, @config_ma_mf, @descricao_alteracao)
end
go
create procedure spUpdate_Alteracoes
(
 @id int,
 @id_usuario int,
 @data_alteracao varchar(max),
 @setpoint decimal(5, 2),
 @config_ma_mf varchar(max),
 @descricao_alteracao varchar(max)
)
as
begin
 update Alteracoes set
 id_usuario = @id_usuario,
 data_alteracao = @data_alteracao,
 setpoint = @setpoint,
 config_ma_mf = @config_ma_mf,
 descricao_alteracao = @descricao_alteracao
 where id = @id 
end
go
create procedure spListagemAlteracoes(
   @tabela varchar(max),
   @ordem varchar(max)
 )
as
  begin
    select Alteracoes.*, Usuarios.nome_usuario as 'Responsável' from Alteracoes
	inner join Usuarios on Usuarios.id  = Alteracoes.id_usuario
end
go
/* Kits */
create procedure spInsert_Kits
(
    @id                        int,
	@nome                      varchar(20),
    @situacao                  varchar(20),
    @data_ultima_manutencao    datetime       = NULL,
    @descricao_equipamento     varchar(100),
    @preco_equipamento         money,
    @imagem                    varbinary(MAX)
)
as
begin
    insert into Kits
        (id, nome, situacao, data_ultima_manutencao, descricao_equipamento, preco_equipamento, imagem)
    values
        (@id, @nome, @situacao, @data_ultima_manutencao, @descricao_equipamento, @preco_equipamento, @imagem);
end
go
create procedure spUpdate_Kits
(
    @id                        int,
	@nome                      varchar(20),
    @situacao                  varchar(20),
    @data_ultima_manutencao    datetime       = NULL,
    @descricao_equipamento     varchar(100),
    @preco_equipamento         money,
    @imagem                    varbinary(MAX)
)
as
begin
    update Kits
    set
		nome                    = @nome,
        situacao                = @situacao,
        data_ultima_manutencao  = @data_ultima_manutencao,
        descricao_equipamento   = @descricao_equipamento,
        preco_equipamento       = @preco_equipamento,
        imagem                  = @imagem
    where id = @id;
end
go