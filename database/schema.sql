-- SQLite schema for GestorMensajesInstitucionales
PRAGMA foreign_keys=ON;

CREATE TABLE IF NOT EXISTS Promotor (
    Id TEXT PRIMARY KEY,
    Nombre TEXT NOT NULL,
    Activo INTEGER NOT NULL DEFAULT 1
);

CREATE TABLE IF NOT EXISTS Ejecutivo (
    Id TEXT PRIMARY KEY,
    Nombre TEXT NOT NULL,
    Activo INTEGER NOT NULL DEFAULT 1
);

CREATE TABLE IF NOT EXISTS ClasificacionSeguridad (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Nombre TEXT NOT NULL,
    Activo INTEGER NOT NULL DEFAULT 1
);

CREATE TABLE IF NOT EXISTS Registro (
    Id TEXT PRIMARY KEY,
    TipoRegistro INTEGER NOT NULL,
    TipoFlujo INTEGER NOT NULL,
    PromotorId TEXT NOT NULL,
    EjecutivoId TEXT NOT NULL,
    ClasificacionSeguridadId INTEGER NOT NULL,
    Asunto TEXT NOT NULL,
    Comentario TEXT,
    TieneFechaTermino INTEGER NOT NULL DEFAULT 0,
    FechaTermino TEXT,
    PdfPath TEXT,
    PredecesorId TEXT,
    FechaCreacion TEXT NOT NULL,
    FechaActualizacion TEXT NOT NULL,
    FOREIGN KEY (PromotorId) REFERENCES Promotor(Id),
    FOREIGN KEY (EjecutivoId) REFERENCES Ejecutivo(Id),
    FOREIGN KEY (ClasificacionSeguridadId) REFERENCES ClasificacionSeguridad(Id),
    FOREIGN KEY (PredecesorId) REFERENCES Registro(Id)
);

CREATE TABLE IF NOT EXISTS Attachment (
    Id TEXT PRIMARY KEY,
    RegistroId TEXT NOT NULL,
    FileName TEXT NOT NULL,
    FileType TEXT NOT NULL,
    FileSizeBytes INTEGER NOT NULL,
    StoredPath TEXT NOT NULL,
    FechaCreacion TEXT NOT NULL,
    FOREIGN KEY (RegistroId) REFERENCES Registro(Id) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS Topic (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Nombre TEXT NOT NULL,
    EsProyecto INTEGER NOT NULL DEFAULT 0
);

CREATE TABLE IF NOT EXISTS RegistroTopic (
    RegistroId TEXT NOT NULL,
    TopicId INTEGER NOT NULL,
    PRIMARY KEY (RegistroId, TopicId),
    FOREIGN KEY (RegistroId) REFERENCES Registro(Id) ON DELETE CASCADE,
    FOREIGN KEY (TopicId) REFERENCES Topic(Id) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS Usuario (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Username TEXT NOT NULL UNIQUE,
    NombreCompleto TEXT NOT NULL,
    PasswordHash BLOB NOT NULL,
    PasswordSalt BLOB NOT NULL,
    Rol INTEGER NOT NULL,
    Activo INTEGER NOT NULL DEFAULT 1,
    FechaCreacion TEXT NOT NULL,
    UltimoAcceso TEXT
);

CREATE TABLE IF NOT EXISTS AuditLog (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    UsuarioId INTEGER NOT NULL,
    FechaHora TEXT NOT NULL,
    Accion TEXT NOT NULL,
    Entidad TEXT NOT NULL,
    EntidadId TEXT NOT NULL,
    Detalle TEXT NOT NULL,
    FOREIGN KEY (UsuarioId) REFERENCES Usuario(Id)
);

INSERT INTO Topic (Id, Nombre, EsProyecto) VALUES (1, 'Antivirus', 0) ON CONFLICT(Id) DO NOTHING;
INSERT INTO Topic (Id, Nombre, EsProyecto) VALUES (2, 'Firewall', 0) ON CONFLICT(Id) DO NOTHING;
INSERT INTO Topic (Id, Nombre, EsProyecto) VALUES (3, 'Capacitación', 0) ON CONFLICT(Id) DO NOTHING;
INSERT INTO Topic (Id, Nombre, EsProyecto) VALUES (4, 'Info de Personal', 0) ON CONFLICT(Id) DO NOTHING;
