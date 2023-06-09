
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
    FOREIGN KEY (catId) REFERENCES Categoria(catId)
);
insert Categoria(catNombre,catDescripcion)values('mancuernas', 'mancuernas de hierro ');
insert Producto(catId, pNombre,pPrecio,pDescripcion) values (1,'mancuerna',1000,'mancuerna de 2000 kilos');

drop procedure sp_SaveProduct
GO

CREATE PROCEDURE sp_DeleteProduct @pId INTEGER
AS
Delete from Producto WHERE pid=@pId
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
  @catId INTEGER,
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

---------------- order 

CREATE TABLE Order(
orId INTEGER NOT NULL IDENTITY(1,1) PRIMARY KEY,
orDate DateTime NOT NULL,
orStatus VARCHAR(200),
perId INTEGER NOT NULL,
FOREIGN KEY (perId) REFERENCES Person(perId)
);

