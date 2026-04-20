using GoodHamburguer.Web.Components;
using GoodHamburguer.Web.Services;
using Refit;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

var apiBaseUrl = builder.Configuration["ApiBaseUrl"] ?? "http://localhost:5066";

var jsonOptions = new JsonSerializerOptions
{
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    PropertyNameCaseInsensitive = true
};

builder.Services.AddRefitClient<IGoodHamburguerApi>(new RefitSettings
{
    ContentSerializer = new SystemTextJsonContentSerializer(jsonOptions)
}).ConfigureHttpClient(client => client.BaseAddress = new Uri(apiBaseUrl));

builder.Services.AddScoped<IOrderApiService, OrderApiService>();

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();