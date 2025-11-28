vusing Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


var builder = WebApplication.CreateBuilder(args);


builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);


builder.Services.AddHttpClient<IViaCepClient, ViaCepClient>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ViaCep:BaseUrl"] ?? "https://viacep.com.br/");
    client.DefaultRequestHeaders.Add("User-Agent", "ViaCepApi/1.0");
});


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();


var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.MapGet("/health", () => Results.Ok(new { status = "ok" }));


app.MapGet("/cep/{cep}", async (string cep, IViaCepClient viaCepClient) =>
{
    if (string.IsNullOrWhiteSpace(cep) || cep.Length < 8)
        return Results.BadRequest(new { error = "CEP inválido" });


    try
    {
        var result = await viaCepClient.GetAddressByCepAsync(cep);
        if (result == null) return Results.NotFound();
        return Results.Ok(result);
    }
    catch (Exception ex)
    {
        return Results.Problem(detail: ex.Message);
    }
});


app.Run();