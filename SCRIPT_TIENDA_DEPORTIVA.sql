CREATE DATABASE TIENDADEPORTIVA;
GO

USE TIENDADEPORTIVA;
GO

 ---TABLAS---
CREATE TABLE Person (
    perId INTEGER NOT NULL IDENTITY(1,1) PRIMARY KEY,
    perName VARCHAR(100) NOT NULL,
    perIdNumber VARCHAR(100) NOT NULL,
    perEmail VARCHAR(100) NOT NULL,
    perPassword VARCHAR(100) NOT NULL,
    perType VARCHAR(100) NOT NULL,
    CONSTRAINT UC_perIdNumber UNIQUE (perIdNumber),
    CONSTRAINT UC_perEmail UNIQUE (perEmail)
);
GO

CREATE TABLE OrderTab(
orId INTEGER NOT NULL IDENTITY(1,1) PRIMARY KEY,
orDate DateTime NOT NULL,
orStatus VARCHAR(200),
perId INTEGER NOT NULL,
FOREIGN KEY (perId) REFERENCES Person(perId) ON DELETE CASCADE
);
GO

CREATE TABLE OrderProduct (
    orProId INTEGER NOT NULL IDENTITY (1,1) PRIMARY KEY,
    orId INTEGER NOT NULL,
    pId INTEGER NOT NULL
    FOREIGN KEY (orId) REFERENCES OrderTab(orId)ON DELETE CASCADE,
    FOREIGN KEY (pId) REFERENCES Producto(pId)ON DELETE CASCADE
);
GO


CREATE TABLE Categoria(
catId INTEGER NOT NULL IDENTITY(1,1) PRIMARY KEY,
catNombre VARCHAR(50) NOT NULL,
catDescripcion VARCHAR(200)
);
GO


CREATE TABLE Producto (
    pId INTEGER NOT NULL IDENTITY (1,1) PRIMARY KEY,
    pNombre VARCHAR(50) NOT NULL,
    pPrecio DECIMAL NOT NULL,
    catId INTEGER NOT NULL,
    pDescripcion VARCHAR(200),
    FOREIGN KEY (catId) REFERENCES Categoria(catId)ON DELETE CASCADE
);
GO

---- PROCEDIMIENTOS------
---- CATEGORY--

CREATE PROCEDURE sp_SaveCategory
  @catNombre VARCHAR(50),
  @catDescripcion VARCHAR(200)
  AS
  INSERT INTO Categoria(catNombre,catDescripcion)
    VALUES(@catNombre,@catDescripcion)
GO

CREATE PROCEDURE sp_DeleteCategory @catId INTEGER
AS
Delete from Categoria WHERE catId=@catId
GO

CREATE PROCEDURE sp_Categories
AS
  select * from Categoria
GO

CREATE PROCEDURE sp_GetCategoryById
  @catId Integer
AS
  select * from Categoria
  where CatId = @catId
GO

CREATE PROCEDURE sp_UpdateCategory
  @catId Integer,
  @catNombre VARCHAR(50),
  @catDescripcion VARCHAR(200)
AS
  UPDATE Categoria
  SET  catNombre = @catNombre,catDescripcion = @catDescripcion
  WHERE catId = @catId
GO

----PERSON-----

CREATE PROCEDURE sp_DeletePerson @perId INTEGER
AS
Delete from Person WHERE perId=@perId
GO

CREATE PROCEDURE sp_SavePerson
  @perName VARCHAR(100),
  @perIdNumber VARCHAR(100),
  @perEmail VARCHAR(100),
  @perPassword VARCHAR(100),
  @perType VARCHAR(100)
  AS
  INSERT INTO Person(perName,perIdNumber,perEmail,perPassword,perType)
    VALUES(@perName,@perIdNumber,@perEmail,@perPassword,@perType)
GO

CREATE PROCEDURE sp_GetPersons
AS
  select * from Person
GO

CREATE PROCEDURE sp_GetPersonById
  @perId Integer
AS
  select * from Person
  where perId = @perId
GO

CREATE PROCEDURE sp_GetPersonByIdNumber
  @perIdNumber VARCHAR(100)
AS
  select * from Person
  where perIdNumber = @perIdNumber
GO

CREATE PROCEDURE sp_GetPersonByCredentials
  @perEmail VARCHAR(100),
  @perPassword VARCHAR(100)
AS
BEGIN
  SELECT *
  FROM Person
  WHERE perEmail = @perEmail AND perPassword = @perPassword;
END
GO

CREATE PROCEDURE sp_updatePerson
  @perId Integer,
  @perName VARCHAR(100),
  @perIdNumber VARCHAR(100),
  @perEmail VARCHAR(100),
  @perPassword VARCHAR(100),
  @perType VARCHAR(100)
  AS
  UPDATE Person set perName=@perName,perIdNumber=@perIdNumber,perEmail=@perEmail,perPassword=@perPassword,perType=@perType
  WHERE perId = @perId
GO

-------ORDER ----

CREATE PROCEDURE sp_DeleteOrder 
@orId INTEGER
AS
Delete from OrderTab WHERE orId=@orId;
GO

CREATE PROCEDURE sp_GetOrderById
  @orId Integer
AS
  select * from OrderTab
  where orId = @orId
GO

CREATE PROCEDURE sp_GetOrderByperId
  @perId Integer
AS
  select * from OrderTab
  where perId = @perId
GO

CREATE PROCEDURE sp_GetOrders
AS
  select * from OrderTab
GO

CREATE PROCEDURE sp_SaveOrder
  @orDate DateTime,
  @orStatus VARCHAR(100),
  @perId Integer,
  @newId Integer OUTPUT
AS
BEGIN
  INSERT INTO OrderTab(orDate, orStatus, perId)
  VALUES (@orDate, @orStatus, @perId)

  SET @newId = SCOPE_IDENTITY()
END
GO

CREATE PROCEDURE sp_updateOrder
  @orId Integer,
  @orDate DateTime ,
  @orStatus VARCHAR(100),
  @perId Integer
  AS
  UPDATE OrderTab set orDate=@orDate,orStatus=@orStatus,perId=@perId
  WHERE orId = @orId
  GO

  ------------ORDER PRODUCT O UNCLUYE --


CREATE PROCEDURE sp_GetProductsByOrId
@orId Integer
As
select * from OrderProduct inner join Producto on OrderProduct.pId = Producto.pId
WHERE OrderProduct.orId = @orId;
GO

CREATE PROCEDURE sp_AddOrderProduct
  @orId Integer ,
  @pId Integer
  AS
  INSERT INTO OrderProduct (orId, pId)
VALUES (@orId, @pId);
GO

CREATE PROCEDURE sp_DeleteOrderProduct
  @orId Integer ,
  @pId Integer
  AS
  Delete TOP(1) from OrderProduct WHERE orId = @orId AND pId =  @pId;
  GO

--------PRODUCTOS-------


CREATE PROCEDURE sp_SaveProduct
  @catId INTEGER,
  @pNombre VARCHAR(50),
  @pPrecio DECIMAL,
  @pDescripcion VARCHAR(200)
  AS
  INSERT INTO Producto(catId,pNombre,pPrecio,pDescripcion)
    VALUES(@catId,@pNombre,@pPrecio,@pDescripcion)
GO


CREATE PROCEDURE sp_Products
AS
  select * from Producto
GO

CREATE PROCEDURE sp_GetProductById
  @pId Integer
AS
  select * from Producto
  where pId = @pId
GO

CREATE PROCEDURE sp_GetProductByCatid
  @catId Integer
AS
  select * from Producto
  where catId = @catId
GO

CREATE PROCEDURE sp_UpdateProduct
  @pId INTEGER,
  @catId Integer,
  @pNombre VARCHAR(50),
  @pPrecio DECIMAL,
  @pDescripcion VARCHAR(200)
AS
  UPDATE Producto
  SET catId= @catId, pNombre = @pNombre,  pPrecio = @pPrecio, pDescripcion = @pDescripcion
  WHERE pId = @pId
GO


--------INSERTS ----
-- Inserts para la tabla Categoria
INSERT INTO Categoria (catNombre, catDescripcion)
VALUES ('Fútbol', 'Artículos y equipos para fútbol');

INSERT INTO Categoria (catNombre, catDescripcion)
VALUES ('Baloncesto', 'Artículos y equipos para baloncesto');

INSERT INTO Categoria (catNombre, catDescripcion)
VALUES ('Tenis', 'Artículos y equipos para tenis');

-- Inserts para la tabla Producto
INSERT INTO Producto (pNombre, pPrecio, catId, pDescripcion)
VALUES ('Pelota de Fútbol', 20, 1, 'Pelota oficial de fútbol');

INSERT INTO Producto (pNombre, pPrecio, catId, pDescripcion)
VALUES ('Zapatillas de Fútbol', 80, 1, 'Zapatillas para fútbol');

INSERT INTO Producto (pNombre, pPrecio, catId, pDescripcion)
VALUES ('Camiseta de Fútbol', 50, 1, 'Camiseta oficial de un equipo de fútbol');

INSERT INTO Producto (pNombre, pPrecio, catId, pDescripcion)
VALUES ('Balón de Baloncesto', 30, 2, 'Balón oficial de baloncesto');

INSERT INTO Producto (pNombre, pPrecio, catId, pDescripcion)
VALUES ('Zapatillas de Baloncesto', 90, 2, 'Zapatillas para baloncesto');

INSERT INTO Producto (pNombre, pPrecio, catId, pDescripcion)
VALUES ('Camiseta de Baloncesto', 60, 2, 'Camiseta oficial de un equipo de baloncesto');

INSERT INTO Producto (pNombre, pPrecio, catId, pDescripcion)
VALUES ('Raqueta de Tenis', 100, 3, 'Raqueta profesional de tenis');

INSERT INTO Producto (pNombre, pPrecio, catId, pDescripcion)
VALUES ('Pelotas de Tenis', 5, 3, 'Paquete de pelotas de tenis');

INSERT INTO Producto (pNombre, pPrecio, catId, pDescripcion)
VALUES ('Gorra de Tenis', 20, 3, 'Gorra oficial de un torneo de tenis');

-- Inserts para la tabla Prersonas
INSERT INTO Person (perName, perIdNumber, perEmail, perPassword, perType)
VALUES ('John Doe', '123456789', 'johndoe@example.com', 'password', 'Administrador');

INSERT INTO Person (perName, perIdNumber, perEmail, perPassword, perType)
VALUES ('Jane Smith', '987654321', 'janesmith@example.com', 'password', 'Cliente');


