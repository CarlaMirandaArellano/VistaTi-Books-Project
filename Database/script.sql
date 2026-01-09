-- Crear la base de datos
CREATE DATABASE VistaTiBooks;
GO

USE VistaTiBooks;
GO

-- Tabla de Favoritos
CREATE TABLE Favorites (
    Id INT PRIMARY KEY IDENTITY(1,1),
    ExternalId NVARCHAR(255) NOT NULL,
    Title NVARCHAR(MAX) NOT NULL,
    Authors NVARCHAR(MAX) NULL,
    FirstPublishYear NVARCHAR(50) NULL,
    CoverUrl NVARCHAR(MAX) NULL,
    UserId INT NOT NULL
);

-- REQUERIMIENTO: Índice único para evitar duplicados por usuario
CREATE UNIQUE INDEX UIX_User_ExternalId 
ON Favorites (UserId, ExternalId);
GO