-- Script para crear la tabla Usuario en la base de datos Ecommerce
-- Ejecutar en SQL Server Management Studio conectado a DESKTOP-FTPRH0M\SQLEXPRESS

USE Ecommerce;
GO

-- Crear la tabla Usuario si no existe
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Usuario' AND xtype='U')
BEGIN
    CREATE TABLE Usuario (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Nombre NVARCHAR(100) NOT NULL,
        Apellido NVARCHAR(100) NOT NULL,
        Mail NVARCHAR(255) NOT NULL UNIQUE,
        Telefono NVARCHAR(20) NULL,
        Domicilio NVARCHAR(500) NULL,
        PasswordHash NVARCHAR(255) NOT NULL,
        Taller BIT NOT NULL DEFAULT 0,
        NombreTaller NVARCHAR(200) NULL,
        UserType NVARCHAR(50) NOT NULL DEFAULT 'normal',
        FechaCreacion DATETIME2 NOT NULL DEFAULT GETDATE(),
        FechaActualizacion DATETIME2 NULL,
        ResetPasswordToken NVARCHAR(255) NULL,
        ResetPasswordExpires DATETIME2 NULL
    );

    -- Crear índices para mejorar el rendimiento
    CREATE INDEX IX_Usuario_Mail ON Usuario(Mail);
    CREATE INDEX IX_Usuario_ResetPasswordToken ON Usuario(ResetPasswordToken);
    CREATE INDEX IX_Usuario_UserType ON Usuario(UserType);

    PRINT 'Tabla Usuario creada exitosamente.';
END
ELSE
BEGIN
    PRINT 'La tabla Usuario ya existe.';
END
GO
