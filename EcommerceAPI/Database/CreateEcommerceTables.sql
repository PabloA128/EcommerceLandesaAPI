-- =============================================
-- Script para crear las tablas del Ecommerce
-- =============================================

USE Ecommerce;
GO

-- Tabla de Categorías
CREATE TABLE Categorias (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL UNIQUE,
    Descripcion NVARCHAR(500),
    Activa BIT DEFAULT 1,
    FechaCreacion DATETIME2 DEFAULT GETDATE()
);
GO

-- Tabla de Subcategorías
CREATE TABLE Subcategorias (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    Descripcion NVARCHAR(500),
    CategoriaId INT NOT NULL,
    Activa BIT DEFAULT 1,
    FechaCreacion DATETIME2 DEFAULT GETDATE(),
    CONSTRAINT FK_Subcategorias_Categorias FOREIGN KEY (CategoriaId) REFERENCES Categorias(Id) ON DELETE CASCADE,
    CONSTRAINT UQ_Subcategoria_Categoria UNIQUE (Nombre, CategoriaId)
);
GO

-- Tabla de Artículos/Productos
CREATE TABLE Articulos (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    CodigoArticulo NVARCHAR(50) NOT NULL UNIQUE,
    Nombre NVARCHAR(200) NOT NULL,
    Descripcion NVARCHAR(MAX),
    PrecioUsuario DECIMAL(10,2) NOT NULL,
    PrecioTaller DECIMAL(10,2) NOT NULL,
    CategoriaId INT NOT NULL,
    SubcategoriaId INT,
    Stock INT DEFAULT 0,
    Imagen NVARCHAR(500), -- URL o path de la imagen
    Activo BIT DEFAULT 1,
    FechaCreacion DATETIME2 DEFAULT GETDATE(),
    CONSTRAINT FK_Articulos_Categorias FOREIGN KEY (CategoriaId) REFERENCES Categorias(Id),
    CONSTRAINT FK_Articulos_Subcategorias FOREIGN KEY (SubcategoriaId) REFERENCES Subcategorias(Id) ON DELETE SET NULL
);
GO

-- Tabla para relacionar Artículos con Talleres (muchos a muchos)
CREATE TABLE ArticuloTalleres (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ArticuloId INT NOT NULL,
    TallerId INT NOT NULL, -- ID del taller que maneja este artículo
    CONSTRAINT FK_ArticuloTalleres_Articulos FOREIGN KEY (ArticuloId) REFERENCES Articulos(Id) ON DELETE CASCADE,
    CONSTRAINT UQ_Articulo_Taller UNIQUE (ArticuloId, TallerId)
);
GO

-- Índices para mejorar el rendimiento
CREATE INDEX IX_Subcategorias_CategoriaId ON Subcategorias(CategoriaId);
CREATE INDEX IX_Articulos_CategoriaId ON Articulos(CategoriaId);
CREATE INDEX IX_Articulos_SubcategoriaId ON Articulos(SubcategoriaId);
CREATE INDEX IX_Articulos_CodigoArticulo ON Articulos(CodigoArticulo);
CREATE INDEX IX_Articulos_Nombre ON Articulos(Nombre);
CREATE INDEX IX_ArticuloTalleres_ArticuloId ON ArticuloTalleres(ArticuloId);
GO

-- Insertar datos de ejemplo
INSERT INTO Categorias (Nombre, Descripcion) VALUES
('Motor', 'Partes relacionadas con el motor del vehículo'),
('Transmisión', 'Componentes de la transmisión'),
('Suspensión', 'Elementos de la suspensión'),
('Frenos', 'Sistema de frenado'),
('Eléctrico', 'Componentes eléctricos');
GO

INSERT INTO Subcategorias (Nombre, Descripcion, CategoriaId) VALUES
('Pistones', 'Pistones y componentes relacionados', 1),
('Cigüeñales', 'Cigüeñales y bielas', 1),
('Embragues', 'Kits de embrague', 2),
('Cajas de cambio', 'Transmisiones manuales y automáticas', 2),
('Amortiguadores', 'Amortiguadores delanteros y traseros', 3),
('Brazos de suspensión', 'Brazos y rótulas', 3),
('Discos de freno', 'Discos y pastillas de freno', 4),
('Bombas de freno', 'Bombas y cilindros de freno', 4),
('Alternadores', 'Alternadores y reguladores', 5),
('Baterías', 'Baterías para automóviles', 5);
GO

PRINT 'Tablas del Ecommerce creadas exitosamente';
GO
