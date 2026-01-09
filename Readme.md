# VistaTi Books Project

Este proyecto es una solución Full-Stack diseñada para la búsqueda y gestión de libros favoritos. Integra la API externa de **Open Library**, un **Backend en .NET 8**, un **Frontend en Angular 18** y persistencia en **SQL Server** ejecutándose sobre **Docker**.

**Hay ciertas funciones que no quedaron de la manera esperada y por tiempo no pude solucionar, por ejemplo para buscar un libro hay que presionar el ícono de arriba "Buscar Libros" para que la búsqueda se realice.**

**Además generalmente hay que hacer click 2 veces para lograr las búsquedas y eliminar favoritos.**

**Decisiones Técnicas**


Arquitectura Decoupled: Se implementó un patrón de Servicios para separar la lógica de negocio y las llamadas HTTP externas de los controladores de la API.

Persistencia Containerizada: Se optó por Docker para garantizar que la base de datos SQL Server sea idéntica en cualquier entorno de desarrollo sin necesidad de instalaciones locales complejas.

Manejo de Datos de Autores: Debido a que la API de Open Library devuelve una lista de autores y SQL Server almacena tipos simples, se decidió persistir los autores como un string delimitado por comas, transformándolos dinámicamente en el Frontend.

Gestión de Usuario Único: Por requerimiento técnico, se utiliza un UserId = 1 estático para todas las operaciones, simulando un perfil de usuario único sin añadir la complejidad de un sistema de autenticación completo (Auth0/Identity).


##  Requisitos Previos

Se necesita tener instalado:
* **Docker Desktop**: Necesario para el contenedor de base de datos.
* **.NET 8 SDK**: Para el entorno de ejecución del Backend.
* **Node.js (v18+)** y **Angular CLI**: Para el entorno del Frontend.
* **Git**: Para el control de versiones.

---

##  1. Configuración de la Base de Datos (Docker)

El proyecto utiliza **SQL Server 2022**. Siga estos pasos para preparar el entorno:

1.  **Levantar el contenedor:**
    Ejecute el siguiente comando para crear la instancia con la contraseña configurada:
    ```bash
    docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=SqlServer123" \
       -p 1433:1433 --name sql_server_dev \
       -d [mcr.microsoft.com/mssql/server:2022-latest](https://mcr.microsoft.com/mssql/server:2022-latest)
    ```

2.  **Ejecutar el DDL (Estructura de Datos):**
    Una vez el contenedor esté corriendo, ejecute este comando para crear la base de datos `VistaTiBooks` y la tabla `Favorites`:
    ```bash
    docker exec -it sql_server_dev /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P "SqlServer123" -Q "CREATE DATABASE VistaTiBooks; GO; USE VistaTiBooks; CREATE TABLE Favorites (Id INT PRIMARY KEY IDENTITY(1,1), ExternalId NVARCHAR(255) NOT NULL, Title NVARCHAR(MAX) NOT NULL, Authors NVARCHAR(MAX) NULL, FirstPublishYear NVARCHAR(50) NULL, CoverUrl NVARCHAR(MAX) NULL, UserId INT NOT NULL);"
    ```

---

## ⚙️ 2. Configuración del Backend (.NET)

1.  **Connection String:**
    Verifique que el archivo `Backend/VistaTi.Api/appsettings.json` contenga las credenciales correctas:
    ```json
    "ConnectionStrings": {
      "DefaultConnection": "Server=localhost,1433;Database=VistaTiBooks;User Id=sa;Password=SqlServer123;TrustServerCertificate=True;"
    }
    ```

2.  **Ejecución:**
    Navegue a la carpeta del API y lance la aplicación:
    ```bash
    cd Backend/VistaTi.Api
    dotnet restore
    dotnet run
    ```
    *El API estará disponible en `http://localhost:5272`.*

---

##  3. Configuración del Frontend (Angular)

1.  **Instalación de dependencias:**
    ```bash
    cd Frontend/VistaTi-Books
    npm install
    ```

2.  **Ejecución:**
    ```bash
    ng serve
    ```
    *La aplicación web se abrirá en `http://localhost:4200`.*

---

##  4. Ejecución de Tests

Se han incluido pruebas unitarias para validar la integración con Open Library y la lógica de los servicios.

```bash
cd Backend/VistaTi.Tests
dotnet test



 



