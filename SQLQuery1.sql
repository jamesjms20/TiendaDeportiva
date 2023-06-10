
DROP TABLE Categoria;
DROP TABLE Producto;

CREATE TABLE Categoria(
catId INTEGER NOT NULL IDENTITY(1,1) PRIMARY KEY,
catNombre VARCHAR(50) NOT NULL,
catDescripcion VARCHAR(200)
);

CREATE TABLE Producto (
    pId INTEGER NOT NULL IDENTITY (1,1) PRIMARY KEY,
    pNombre VARCHAR(50) NOT NULL,
    pPrecio DECIMAL NOT NULL,
    catId INTEGER NOT NULL,
    pDescripcion VARCHAR(200),
    FOREIGN KEY (catId) REFERENCES Categoria(catId)ON DELETE CASCADE
);
insert Categoria(catNombre,catDescripcion)values('mancuernas', 'mancuernas de hierro ');
insert Producto(catId, pNombre,pPrecio,pDescripcion) values (1,'mancuerna',1000,'mancuerna de 2000 kilos');

drop procedure sp_SaveProduct
GO

CREATE PROCEDURE sp_DeleteCategory 
@catId INTEGER
AS
DELETE FROM Producto WHERE catId=@catId
Delete from Categoria WHERE catId=@catId
GO

CREATE PROCEDURE sp_SaveProduct
  @catId INTEGER,
  @pNombre VARCHAR(50),
  @pPrecio DECIMAL,
  @pDescripcion VARCHAR(200)
  AS
  INSERT INTO Producto(catId,pNombre,pPrecio,pDescripcion)
    VALUES(@catId,@pNombre,@pPrecio,@pDescripcion)
GO
drop procedure sp_Products
go

CREATE PROCEDURE sp_Products
AS
  select * from Producto
GO

drop procedure sp_GetProductById
GO
CREATE PROCEDURE sp_GetProductById
  @pId Integer
AS
  select * from Producto
  where pId = @pId
GO

drop procedure sp_GetProductByCatid
GO
CREATE PROCEDURE sp_GetProductByCatid
  @catId Integer
AS
  select * from Producto
  where catId = @catId
GO

drop procedure sp_UpdateProduct
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
-------------------- categoria---
drop procedure sp_SaveCategory
GO
drop procedure sp_Categories
go
drop procedure sp_GetCAtegoryById
GO
drop procedure sp_Updateategory
GO
CREATE PROCEDURE sp_DeleteCategory @catId INTEGER
AS
Delete from Categoria WHERE catId=@catId
GO


CREATE PROCEDURE sp_SaveCategory
  @catNombre VARCHAR(50),
  @catDescripcion VARCHAR(200)
  AS
  INSERT INTO Categoria(catNombre,catDescripcion)
    VALUES(@catNombre,@catDescripcion)
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
-----   person
drop table Person
GO 

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


drop procedure sp_SavePerson
GO
drop procedure sp_GetPersons
go
drop procedure sp_GetPersonById
GO
drop procedure sp_updatePerson
GO

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


---------------- order 

CREATE TABLE OrderTab(
orId INTEGER NOT NULL IDENTITY(1,1) PRIMARY KEY,
orDate DateTime NOT NULL,
orStatus VARCHAR(200),
perId INTEGER NOT NULL,
FOREIGN KEY (perId) REFERENCES Person(perId) ON DELETE CASCADE
);
GO
drop procedure sp_DeleteOrder
drop procedure sp_GetOrderById
drop procedure sp_GetOrderByperId
drop procedure sp_GetOrders
drop procedure sp_SaveOrder
drop procedure sp_updateOrder
go

CREATE PROCEDURE sp_DeleteOrder 
@orId INTEGER
AS
Delete from OrderProduct where orId=@orId;
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

CREATE PROCEDURE sp_SaveOrder
  @orDate DateTime ,
  @orStatus VARCHAR(100),
  @perId Integer
  AS
  INSERT INTO OrderTab(orDate,orStatus,perId)
    VALUES(@orDate,@orStatus,@perId)

CREATE PROCEDURE sp_updateOrder
  @orId Integer,
  @orDate DateTime ,
  @orStatus VARCHAR(100),
  @perId Integer
  AS
  UPDATE OrderTab set orDate=@orDate,orStatus=@orStatus,perId=@perId
  WHERE orId = @orId
  GO
---------------


CREATE TABLE OrderProduct (
    orProId INTEGER NOT NULL IDENTITY (1,1) PRIMARY KEY,
    orId INTEGER NOT NULL,
    pId INTEGER NOT NULL
    FOREIGN KEY (orId) REFERENCES OrderTab(orId)ON DELETE CASCADE,
    FOREIGN KEY (pId) REFERENCES Producto(pId)ON DELETE CASCADE
);
GO

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





  INSERT INTO OrderTab(orDate,orStatus,perId)
    VALUES(@orDate,@orStatus,@perId)
GO


  EXEC sp_GetProductsByOrId @orId = 1;

  -- Ejemplo de inserción 1
INSERT INTO OrderTab (orDate, orStatus, perId)
VALUES ('2023-06-09', 'Pendiente', 7);

-- Ejemplo de inserción 2
INSERT INTO OrderTab (orDate, orStatus, perId)
VALUES ('2023-06-10', 'En proceso', 2);

-- Ejemplo de inserción 3
INSERT INTO OrderTab (orDate, orStatus, perId)
VALUES ('2023-06-11', 'Entregado', 2);

INSERT INTO OrderProduct (orId, pId)
VALUES (1, 3);

-- Inserción 2
INSERT INTO OrderProduct (orId, pId)
VALUES (1, 4);

-- Inserción 3
INSERT INTO OrderProduct (orId, pId)
VALUES (3, 3);