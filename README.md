# GestorMensajesInstitucionales

Esqueleto de aplicación WPF (.NET) con SQLite para registrar y gestionar mensajes/volantes institucionales con digitalización de PDF, adjuntos ZIP/RAR, temas, usuarios con roles y auditoría.

## Arquitectura
- **Domain**: Entidades y enums de negocio.
- **Application**: Interfaces de servicios (registros, usuarios, catálogos, auditoría, almacenamiento de archivos).
- **Infrastructure**: Implementaciones con Entity Framework Core (SQLite) y almacenamiento en disco.
- **UI**: Aplicación WPF con ventana de login y ventana principal de listado/visores.

Se eligió **Entity Framework Core** para aprovechar el mapeo objeto-relacional, validación de relaciones y unificación de acceso a datos/auditoría sobre SQLite.

## Base de datos
El script `database/schema.sql` crea todas las tablas y datos iniciales de temas predefinidos. Para iniciar una base local:
```bash
sqlite3 gestor.db < database/schema.sql
```

## Compilación y ejecución
Requiere SDK .NET 8 en Windows (WPF). Pasos sugeridos:
```bash
# Restaurar paquetes
# dotnet restore

# Compilar solución
# dotnet build GestorMensajesInstitucionales.sln

# Ejecutar la app WPF (desde Windows)
# dotnet run --project GestorMensajesInstitucionales.UI
```

La app usa una carpeta `storage` junto al ejecutable para guardar PDFs y adjuntos. Los ZIP se descomprimen usando `System.IO.Compression`; para RAR se requiere un descompresor externo antes de usar la opción.
