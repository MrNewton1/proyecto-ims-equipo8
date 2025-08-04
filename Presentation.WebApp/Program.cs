using Infrastructure;

// Construye el WebApplication a partir de los argumentos de línea de comandos
var builder = WebApplication.CreateBuilder(args);

// ───────────────────────────────────────────────────────────────────────────────
// 1. Registro de DbContext personalizados en el contenedor de DI
// ───────────────────────────────────────────────────────────────────────────────

// LibrosDbContext: se crea por llamada a petición (Transient) con la cadena
// DefaultConnection extraída de appsettings.json. Si no está configurada,
// lanza una excepción para detectar el error en inicio.
builder.Services.AddTransient<LibrosDbContext>(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();
    var connStr = configuration.GetConnectionString("DefaultConnection");
    if (string.IsNullOrEmpty(connStr))
        throw new InvalidOperationException(
            "Connection string 'DefaultConnection' is not configured.");
    return new LibrosDbContext(connStr);
});

// UsuariosDbContext: mismo patrón que LibrosDbContext, pero apuntando a la tabla de usuarios.
builder.Services.AddTransient<UsuariosDbContext>(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();
    var connStr = configuration.GetConnectionString("DefaultConnection");
    if (string.IsNullOrEmpty(connStr))
        throw new InvalidOperationException(
            "Connection string 'DefaultConnection' is not configured.");
    return new UsuariosDbContext(connStr);
});

// PrestamosDbContext: igual que los anteriores, para operaciones con préstamos.
builder.Services.AddTransient<PrestamosDbContext>(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();
    var connStr = configuration.GetConnectionString("DefaultConnection");
    if (string.IsNullOrEmpty(connStr))
        throw new InvalidOperationException(
            "Connection string 'DefaultConnection' is not configured.");
    return new PrestamosDbContext(connStr);
});

// ───────────────────────────────────────────────────────────────────────────────
// 2. Registro de MVC (Controladores con vistas)
// ───────────────────────────────────────────────────────────────────────────────
builder.Services.AddControllersWithViews();

// Construye la aplicación
var app = builder.Build();

// ───────────────────────────────────────────────────────────────────────────────
// 3. Configuración del pipeline HTTP
// ───────────────────────────────────────────────────────────────────────────────

// En producción, usa una página de error genérica y HSTS para seguridad
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// Redirigir HTTP a HTTPS
app.UseHttpsRedirection();

// Habilita la resolución de archivos estáticos (wwwroot)
app.MapStaticAssets();

// Enrutamiento de peticiones
app.UseRouting();

// Autorización (sin autenticación en este ejemplo, pero configurado para el futuro)
app.UseAuthorization();

// Define la ruta por defecto: HomeController → Index, con parámetro opcional id
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

// Ejecuta la aplicación y comienza a escuchar peticiones
app.Run();
