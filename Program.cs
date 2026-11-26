using Microsoft.EntityFrameworkCore;
using wt_lab7_7postgres_efcore_vinnik.Data;
using wt_lab7_7postgres_efcore_vinnik.Models;

var builder = WebApplication.CreateBuilder(args);

// Регистрация DbContext для работы с PostgreSQL
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));

var app = builder.Build();

// Автоматическое применение миграций при запуске 
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

// Блок 1: Тестовые эндпоинты
app.MapGet("/", () => "Minimal API с PostgreSQL работает!");

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

//  Использование LINQ (минимум 3 запроса)

// 1. Фильтрация (баланс больше 1000)
app.MapGet("/api/accounts/filter", async (AppDbContext db) =>
{
    var accounts = await db.Accounts
        .Where(x => x.Balance > 1000)
        .ToListAsync();
    return Results.Ok(accounts);
});

// 2. Сортировка (по балансу)
app.MapGet("/api/accounts/sorted", async (AppDbContext db) =>
{
    var accounts = await db.Accounts
        .OrderBy(x => x.Balance)
        .ToListAsync();
    return Results.Ok(accounts);
});

// 3. Выборка с условием (только активные, отсортированные по имени)
app.MapGet("/api/accounts/active", async (AppDbContext db) =>
{
    var accounts = await db.Accounts
        .Where(x => x.IsActive)
        .OrderBy(x => x.Owner)
        .ToListAsync();
    return Results.Ok(accounts);
});

// 4. Собственный запрос (поиск по типу счета)
app.MapGet("/api/accounts/type/{type}", async (string type, AppDbContext db) =>
{
    var accounts = await db.Accounts
        .Where(x => x.AccountType.ToLower() == type.ToLower())
        .ToListAsync();
    return Results.Ok(accounts);
});

// Настройка порта для Render
var port = Environment.GetEnvironmentVariable("PORT") ?? "10000";
app.Run($"http://0.0.0.0:{port}");