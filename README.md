# CRUD-de-videojuegos

## Linux
systemctl start mssql-server  
cd AppWeb  
dotnet ef migrations add Inicial  
dotnet ef database update  
dotnet restore -> restaura los paquetes NuGet  
dotnet build -> compila  
dotnet run -> ejecuta  

con": "Server=localhost;Database=AppWeb;User Id=sa;Password="pass";TrustServerCertificate=True;"  

## Windows
Add-Migration Inicial  
Update-Database  
Correlo   
"con": "Data Source=CT1-PC21;Initial Catalog=AppWeb;Integrated Security=True;Trust Server Certificate=True"  
