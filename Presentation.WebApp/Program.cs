using Infrastructure; 

var builder = WebApplication.CreateBuilder(args);

// Registrar DbContext personalizados con la cadena de conexión
builder.Services.AddTransient<LibrosDbContext>(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();
    var connStr = configuration.GetConnectionString("DefaultConnection");
    if (string.IsNullOrEmpty(connStr))
        throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured.");
    return new LibrosDbContext(connStr);
});

builder.Services.AddTransient<UsuariosDbContext>(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();
    var connStr = configuration.GetConnectionString("DefaultConnection");
    if (string.IsNullOrEmpty(connStr))
        throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured.");
    return new UsuariosDbContext(connStr);
});

builder.Services.AddTransient<PrestamosDbContext>(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();
    var connStr = configuration.GetConnectionString("DefaultConnection");
    if (string.IsNullOrEmpty(connStr))
        throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured.");
    return new PrestamosDbContext(connStr);
});

// Agregar servicios para controladores con vistas
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configuración del pipeline HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();