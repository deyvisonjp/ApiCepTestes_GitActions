using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;


public class CepEndpointTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;


    public CepEndpointTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }


    [Fact]
    public async Task GetCep_ReturnsOk_ForValidCep()
    {
        var client = _factory.CreateClient();
        var response = await client.GetAsync("/cep/01001000");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("Praça", content, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task InvalidCep_ReturnsBadRequest()
    {
        var client = _factory.CreateClient();
        var response = await client.GetAsync("/cep/123");
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}