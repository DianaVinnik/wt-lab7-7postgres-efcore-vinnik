using Microsoft.EntityFrameworkCore;
using wt_lab7_7postgres_efcore_vinnik.Data;
using wt_lab7_7postgres_efcore_vinnik.Models;

var builder = WebApplication.CreateBuilder(args);

// 1. Получаем строку подключения
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? "";
var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");

// 2. Если есть DATABASE_URL (на Render), преобразуем URL в формат Npgsql
if (!string.IsNullOrWhiteSpace(databaseUrl))
{
    var uri = new Uri(databaseUrl);
    var userInfo = uri.UserInfo.Split(':');
    
    // Если порт не указан (равен -1), используем стандартный 5432
    var port = uri.Port == -1 ? 5432 : uri.Port;
    
    connectionString = $"Host={uri.Host};Port={port};Database={uri.AbsolutePath.TrimStart('/')};Username={userInfo[0]};Password={userInfo[1]};SSL Mode=Require;Trust Server Certificate=true";
}

// 3. Регистрация DbContext
builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));

var app = builder.Build();

// 4. Автоматическое применение миграций при запуске
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

// --- ЭНДПОИНТЫ ---

// Блок 1: Тестовые эндпоинты
app.MapGet("/", () => "Minimal API с PostgreSQL работает!");

app.MapGet("/api/info", () => new 
{ 
    Application = "Банковская система", 
    Technology = "ASP.NET Core + EF Core + PostgreSQL", 
    Version = "1.0" 
});

// Блок 3: GET запросы
app.MapGet("/api/accounts", async (string? search, AppDbContext db) =>
{
    var query = db.Accounts.AsQueryable();
    if (!string.IsNullOrWhiteSpace(search))
    {
        query = query.Where(x => x.Owner.ToLower().Contains(search.ToLower()));
    }
    return Results.Ok(await query.ToListAsync());
});

app.MapGet("/api/accounts/{id}", async (int id, AppDbContext db) =>
{
    var account = await db.Accounts.FirstOrDefaultAsync(x => x.Id == id);
    if (account == null) return Results.NotFound();
    return Results.Ok(account);
});

// Блок 4: POST запрос (Создание)
app.MapPost("/api/accounts", async (Account account, AppDbContext db) =>
{
    db.Accounts.Add(account);
    await db.SaveChangesAsync();
    return Results.Created($"/api/accounts/{account.Id}", account);
});

// Блок 5: PUT запрос (Обновление)
app.MapPut("/api/accounts/{id}", async (int id, Account updatedAccount, AppDbContext db) =>
{
    var account = await db.Accounts.FirstOrDefaultAsync(x => x.Id == id);
    if (account == null) return Results.NotFound();

    account.Owner = updatedAccount.Owner;
    account.AccountNumber = updatedAccount.AccountNumber;
    account.Balance = updatedAccount.Balance;
    account.AccountType = updatedAccount.AccountType;
    account.IsActive = updatedAccount.IsActive;

    await db.SaveChangesAsync();
    return Results.Ok(account);
});

// Блок 5: DELETE запрос (Удаление)
app.MapDelete("/api/accounts/{id}", async (int id, AppDbContext db) =>
{
    var account = await db.Accounts.FirstOrDefaultAsync(x => x.Id == id);
    if (account == null) return Results.NotFound();

    db.Accounts.Remove(account);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

// Блок 17: LINQ-запросы (Фильтрация, сортировка, выборка)
app.MapGet("/api/accounts/filter", async (AppDbContext db) =>
{
    var accounts = await db.Accounts.Where(x => x.Balance > 1000).ToListAsync();
    return Results.Ok(accounts);
});

app.MapGet("/api/accounts/sorted", async (AppDbContext db) =>
{
    var accounts = await db.Accounts.OrderBy(x => x.Balance).ToListAsync();
    return Results.Ok(accounts);
});

app.MapGet("/api/accounts/active", async (AppDbContext db) =>
{
    var accounts = await db.Accounts
        .Where(x => x.IsActive)
        .OrderBy(x => x.Owner)
        .ToListAsync();
    return Results.Ok(accounts);
});

// Настройка порта для Render
var port = Environment.GetEnvironmentVariable("PORT") ?? "10000";
app.Run($"http://0.0.0.0:{port}");