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

// ===============================
// 🟢 SQLITE SETUP (SEED + RUNTIME)
// ===============================

// pasta onde o banco vai rodar no Render
var runtimeFolder = Path.Combine(Directory.GetCurrentDirectory(), "Data");
Directory.CreateDirectory(runtimeFolder);

// banco ativo (runtime)
var runtimeDbPath = Path.Combine(runtimeFolder, "banco.db");

// banco inicial vindo do GitHub
var seedDbPath = Path.Combine(Directory.GetCurrentDirectory(), "DataSeed", "banco.db");

// 🔥 copia o banco do GitHub só na primeira execução
if (!File.Exists(runtimeDbPath) && File.Exists(seedDbPath))
{
    File.Copy(seedDbPath, runtimeDbPath);
}

// ===============================
// DB CONTEXT
// ===============================
builder.Services.AddDbContext<BancoContext>(options =>
    options.UseSqlite($"Data Source={runtimeDbPath}"));

// ===============================
// DEPENDÊNCIAS
// ===============================
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

// ===============================
// PIPELINE
// ===============================
app.UseCors();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// ❌ NÃO usar HTTPS no Render
// app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapHub<PagamentoHub>("/pagamentoHub");

// ===============================
// MIGRATION / DB INIT
// ===============================
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<BancoContext>();
    db.Database.Migrate();
}

app.Run();
