using TestApp.Cache;
using TestApp.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddHttpClient<ExternalApiService>();
builder.Services.AddScoped<ExternalApiService>();
builder.Services.AddScoped<JsonExternalApiService>();
builder.Services.AddSingleton<ExternalApiCache>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
