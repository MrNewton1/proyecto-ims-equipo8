## Proyecto Final: Sistema de Gestión de Préstamos de Libros

**Materia:** Implantación y Mantenimiento de Sistemas
**Institución:** UNITEC
**Autor:** Equipo 8 – Proyecto IMS

---

### Descripción

Este prototipo académico es una aplicación web MVC desarrollada en .NET 9 para gestionar el préstamo de libros a usuarios. Incluye:

* **Gestión de Usuarios**: CRUD de usuarios con validaciones y lista de datos.
* **Gestión de Libros**: CRUD de libros, carga de imágenes codificadas en Base64 y vista de detalles en modal.
* **Gestión de Préstamos**: CRUD de préstamos, selección de usuarios y libros mediante dropdowns, y visualización de estadísticas.
* **Dashboard**: Gráfico “doughnut” con distribución de usuarios, libros y préstamos.

Se implementó arquitectura **Onion** (Domain → Application → Infrastructure → Presentation), separación de responsabilidades y uso de **Ajax** para vistas parciales y **Bootstrap 5** junto con **DataTables** para mejorar la experiencia de usuario.

---

### Estructura del Proyecto

```
/proyecto-ims-equipo8
│
├─ Application/           # Lógica de casos de uso (servicios, conversores)
│  └─ Services/
│      └─ FileConverterService.cs
│
├─ Domain/                # Entidades de negocio
│  ├─ IM253E08Usuario.cs
│  ├─ IM253E08Libro.cs
│  └─ IM253E08Prestamo.cs
│
├─ Infrastructure/        # Acceso a datos ADO.NET
│  └─ Data/
│      ├─ UsuariosDbContext.cs
│      ├─ LibrosDbContext.cs
│      └─ PrestamosDbContext.cs
│
└─ Presentation.WebApp/   # Capa web MVC
   ├─ Controllers/
   │   ├─ HomeController.cs
   │   ├─ UsuariosController.cs
   │   ├─ LibrosController.cs
   │   └─ PrestamosController.cs
   │
   ├─ Models/             # ViewModels y ErrorViewModel
   ├─ Views/              # Vistas Razor
   │   ├─ Shared/
   │   │   └─ _Layout.cshtml
   │   ├─ Usuarios/       # Index, Create, Edit, _DetallesPartial
   │   ├─ Libros/         # Index, Create, Edit, _DetallesPartial
   │   ├─ Prestamos/      # Index, Create, Edit, Details
   │   └─ Home/           # Index (dashboard), Privacy, Error
   │
   ├─ wwwroot/            # Archivos estáticos (CSS, JS, bibliotecas)
   │   └─ css/bootstrap.min.css  # Tema Bootswatch
   └─ appsettings.json     # Cadenas de conexión y configuración
```

---

### Prerrequisitos

* [.NET SDK 9.0](https://dotnet.microsoft.com/download/dotnet/9.0)
* SQL Server (local o Azure)
* Visual Studio 2022 / VS Code (con extensión C#)

---

### Configuración

1. Clona el repositorio y abre la solución (`.sln`) en tu IDE.
2. En **appsettings.json**, ajusta la sección `ConnectionStrings:DefaultConnection` con tu servidor y credenciales:

   ```jsonc
   "ConnectionStrings": {
     "DefaultConnection": "Server=TU_SERVIDOR;Database=im;User Id=...;Password=...;"
   }
   ```
3. Asegúrate de que las tablas `IM253E08Usuario`, `IM253E08Libro` y `IM253E08Prestamos` existen en la base de datos según el modelo.

---

### Ejecución

Desde la raíz del proyecto, en terminal:

```bash
cd Presentation.WebApp
dotnet run
```

Luego abre en el navegador:

```
https://localhost:5001
```

---

### Características Clave

* **Arquitectura por Capas**:

  * **Domain**: Entidades puros.
  * **Application**: Servicios (e.g., conversor de imágenes).
  * **Infrastructure**: Repositorios ADO.NET.
  * **Presentation**: MVC con Razor, Bootstrap y Ajax.
* **Interacción Ajax**:

  * Detalles en modales sin recargar la página.
* **DataTables**:

  * Paginación, búsqueda y exportación a Excel/PDF en listas.
* **Gráficos**:

  * Dashboard con Chart.js (doughnut chart).
* **Estilos y Temas**:

  * Tema personalizado de Bootswatch + overrides CSS (`site.css`).

---

### Buenas Prácticas y Futuras Mejoras

* **Validaciones del lado servidor y cliente** con `DataAnnotations` y `jquery-validation`.
* **Autenticación/Autorización** (e.g., Identity) para restringir acceso.
* **Manejo de errores centralizado** (middleware de excepción).
* **Pruebas unitarias** y de integración de servicios y repositorios.
* **Optimización de carga de imágenes** y almacenamiento en Azure Blob Storage.

---

### Referencias

* [Documentación oficial .NET 9](https://docs.microsoft.com/dotnet/core/dotnet-five)
* [Bootstrap 5](https://getbootstrap.com/)
* [DataTables](https://datatables.net/)
* [Chart.js](https://www.chartjs.org/)

---

© 2025 Equipo 8 – Proyecto Final IMS.
