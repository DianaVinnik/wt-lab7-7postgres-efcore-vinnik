using wt_lab6_6minimalapi_vinnik.Data;
using wt_lab6_6minimalapi_vinnik.Models;

var builder = WebApplication.CreateBuilder(args);

// Блок 6: Добавляем встроенный OpenAPI для документации
builder.Services.AddOpenApi();

var app = builder.Build();

// Включаем OpenAPI в режиме разработки
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Блок 1: Тестовые эндпоинты
app.MapGet("/", () => "Minimal API работает!");

app.MapGet("/api/info", () => new
{
    Application = "Банковская система",
    Technology = "ASP.NET Core Minimal API",
    Version = "1.0"
});

// Блок 3: GET запросы (с фильтрацией через query-параметр)
app.MapGet("/api/accounts", (string? search) =>
{
    var accounts = AccountStore.Accounts.AsEnumerable();
    if (!string.IsNullOrWhiteSpace(search))
    {
        accounts = accounts.Where(x => x.Owner.Contains(search, StringComparison.OrdinalIgnoreCase));
    }
    return Results.Ok(accounts);
});

// Блок 3: GET запрос по ID
app.MapGet("/api/accounts/{id}", (int id) =>
{
    var account = AccountStore.Accounts.FirstOrDefault(x => x.Id == id);
    if (account == null) return Results.NotFound();
    return Results.Ok(account);
});

// Блок 4: POST запрос (Создание с автоматическим ID и статусом 201 Created)
app.MapPost("/api/accounts", (Account account) =>
{
    var newId = AccountStore.Accounts.Count == 0 ? 1 : AccountStore.Accounts.Max(x => x.Id) + 1;
    account.Id = newId;
    AccountStore.Accounts.Add(account);
    
    return Results.Created($"/api/accounts/{newId}", account);
});

// Блок 5: PUT запрос (Обновление)
app.MapPut("/api/accounts/{id}", (int id, Account updatedAccount) =>
{
    var account = AccountStore.Accounts.FirstOrDefault(x => x.Id == id);
    if (account == null) return Results.NotFound();

    account.Owner = updatedAccount.Owner;
    account.AccountNumber = updatedAccount.AccountNumber;
    account.Balance = updatedAccount.Balance;
    account.AccountType = updatedAccount.AccountType;
    account.IsActive = updatedAccount.IsActive;

    return Results.Ok(account);
});

// Блок 5: DELETE запрос (Удаление со статусом 204 No Content)
app.MapDelete("/api/accounts/{id}", (int id) =>
{
    var account = AccountStore.Accounts.FirstOrDefault(x => x.Id == id);
    if (account == null) return Results.NotFound();

    AccountStore.Accounts.Remove(account);
    return Results.NoContent();
});

// Блок 7: Настройка порта для Render
var port = Environment.GetEnvironmentVariable("PORT") ?? "10000";
app.Run($"http://0.0.0.0:{port}");