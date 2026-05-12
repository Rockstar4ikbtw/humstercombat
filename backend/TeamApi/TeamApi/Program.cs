using Microsoft.Extensions.Options;
using System.Xml.Linq;

var builder = WebApplication.CreateBuilder(args);
// Разрешаем фронтенду обращаться к API (CORS)
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});
var app = builder.Build();
app.UseCors();
// Данные о команде — замените на реальные имена!
// GET /api/team — вернуть список участников
var team = new[]
{
new { name = "Ярослав", role = "Tech Lead", fact = "Люблю C#" },
new { name = "Умар", role = "Developer", fact = "Пишу на C# с 1 курса" },
new { name = "Алиса", role = "QA", fact = "Нахожу баги быстрее всех" },
};
app.MapGet("/api/team", () => Results.Ok(team));
// GET /api/team/{name} — вернуть одного участника
app.MapGet("/api/team/{name}", (string name) =>
{
    var member = team.FirstOrDefault(m =>
    m.name.Contains(name, StringComparison.OrdinalIgnoreCase));
    return member is not null
    ? Results.Ok(member)
    : Results.NotFound(new { error = "Участник не найден" });
});
app.Run();