using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

public interface IViaCepClient
{
    Task<ViaCepResponse?> GetAddressByCepAsync(string cep);
}

public class ViaCepClient : IViaCepClient
{
    private readonly HttpClient _http;


    public ViaCepClient(HttpClient http)
    {
        _http = http;
    }


    public async Task<ViaCepResponse?> GetAddressByCepAsync(string cep)
    {
        var sanitized = new string(cep.Where(char.IsDigit).ToArray());
        var resp = await _http.GetFromJsonAsync<ViaCepResponse>($"/ws/{sanitized}/json/");
        return resp;
    }
}

public class ViaCepResponse
{
    public string? Cep { get; set; }
    public string? Logradouro { get; set; }
    public string? Complemento { get; set; }
    public string? Bairro { get; set; }
    public string? Localidade { get; set; }
    public string? Uf { get; set; }
    public string? Unidade { get; set; }
    public string? Ibge { get; set; }
    public string? Gia { get; set; }
}
