# CRUD-de-videojuegos

## Linux
systemctl start mssql-server  
cd AppWeb  
dotnet ef migrations add Inicial  
dotnet ef database update  
dotnet restore -> restaura los paquetes NuGet  
dotnet build -> compila  
dotnet run -> ejecuta  

"con": "Server=localhost;Database=AppWeb;User Id=sa;Password=pass;TrustServerCertificate=True;"  

## Windows
Add-Migration Inicial  
Update-Database  
Correlo   
"con": "Data Source=CT1-PC21;Initial Catalog=AppWeb;Integrated Security=True;Trust Server Certificate=True"  

### Usuario con sal

USE AppWeb;  
GO

DECLARE @salt NVARCHAR(50) = NEWID();  

INSERT INTO Usuarios (Nombre, Correo, Contrasena, salt, FechaRegistro)
VALUES (
    'William',
    'william@gmail.com',
    HASHBYTES('SHA2_256', @salt + '123'),
    @salt,
    '2026/03/24'
)  
GO

### Roles

INSERT INTO Roles (NombreRol)
VALUES ('Administrador'), ('Cliente')

### Usuario con sal

USE AppWeb;  
GO

DECLARE @salt NVARCHAR(50) = NEWID();  

INSERT INTO Usuarios (Nombre, Correo, Contrasena, salt, FechaRegistro)
VALUES (
    'William',
    'william@gmail.com',
    HASHBYTES('SHA2_256', @salt + '123'),
    @salt,
    '2026/03/24'
)  
GO

### Roles

INSERT INTO Roles (NombreRol)
VALUES ('Administrador'), ('Cliente')

### Usuario con sal

USE AppWeb;  
GO

DECLARE @salt NVARCHAR(50) = NEWID();  

INSERT INTO Usuarios (Nombre, Correo, Contrasena, salt, FechaRegistro)
VALUES (
    'William',
    'william@gmail.com',
    HASHBYTES('SHA2_256', @salt + '123'),
    @salt,
    '2026/03/24'
)  
GO

### Roles

INSERT INTO Roles (NombreRol)
VALUES ('Administrador'), ('Cliente')