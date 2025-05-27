using RegistroTecnico.Components;
using Microsoft.EntityFrameworkCore;
using RegistroTecnico.Components.DAL;
using RegistroTecnico.Components.Services;
using Blazored.Toast;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddBlazoredToast();

var conStr = builder.Configuration.GetConnectionString("SqlServerConStr");
builder.Services.AddDbContextFactory<Contexto>(o => o.UseSqlServer(conStr));

builder.Services.AddScoped<TecnicoService>();
builder.Services.AddScoped<ClienteService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();