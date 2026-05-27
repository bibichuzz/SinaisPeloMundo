using Microsoft.EntityFrameworkCore;
using SinaisPeloMundo.Data;
using SinaisPeloMundo.Helper;
using SinaisPeloMundo.Repositorio;

var builder = WebApplication.CreateBuilder(args);

// 🔥 Render PORT
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

// Services
builder.Services.AddControllersWithViews();

// 🟢 SQLite PATH (runtime seguro)
var dbFolder = Path.Combine(Directory.GetCurrentDirectory(), "Data");
Directory.CreateDirectory(dbFolder);

var dbPath = Path.Combine(dbFolder, "banco.db");

// 🟡 Caminho do banco vindo do GitHub (seed inicial)
var seedDbPath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "banco.db");

// 🔥 COPIA O BANCO DO GITHUB SÓ SE NÃO EXISTIR NO RUNTIME
if (!File.Exists(dbPath) && File.Exists(seedDbPath))
{
    File.Copy(seedDbPath, dbPath);
}

builder.Services.AddDbContext<BancoContext>(options =>
    options.UseSqlite($"Data Source={dbPath}"));

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddScoped<IClienteRepositorio, ClienteRepositorio>();
builder.Services.AddScoped<IPedidoRepositorio, PedidoRepositorio>();
builder.Services.AddScoped<IPacoteRepositorio, PacoteRepositorio>();
builder.Services.AddScoped<IAdminRepositorio, AdminRepositorio>();
builder.Services.AddScoped<ISessao, Sessao>();

builder.Services.AddSession(o =>
{
    o.Cookie.HttpOnly = true;
    o.Cookie.IsEssential = true;
});

builder.Services.AddSignalR();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials()
              .SetIsOriginAllowed(_ => true);
    });
});

var app = builder.Build();

app.UseCors();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// ❌ Render não precisa forçar HTTPS
// app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapHub<PagamentoHub>("/pagamentoHub");

// 🟢 Garante criação de tabelas (EF Core)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<BancoContext>();
    db.Database.Migrate();
}

app.Run();
