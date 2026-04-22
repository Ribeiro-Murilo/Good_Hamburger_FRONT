using GoodHamburgerFront.Models;
using System.Net.Http.Json;
using System.Text.Json;

namespace GoodHamburgerFront.Services;

public class CardapioService
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

    public CardapioService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<TipoItemDto>> GetTiposItemAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("api/tipoItem");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<TipoItemDto>>(content, _jsonOptions) ?? new();
        }
        catch
        {
            return new();
        }
    }

    public async Task<List<MenuItemDto>> GetMenuAsync(int? tipoItemId = null)
    {
        try
        {
            var url = "api/menu";
            if (tipoItemId.HasValue)
            {
                url += $"?tipoItem={tipoItemId.Value}";
            }

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<MenuItemDto>>(content, _jsonOptions) ?? new();
        }
        catch
        {
            return new();
        }
    }
}
