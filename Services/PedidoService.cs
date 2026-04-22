using GoodHamburgerFront.Models;
using System.Net.Http.Json;
using System.Text.Json;

namespace GoodHamburgerFront.Services;

public class PedidoService
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

    public PedidoService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<(bool Success, Guid? Id, string? ErrorMessage)> CriarPedidoAsync()
    {
        try
        {
            var request = new PedidoRequestDto { Itens = new() };
            var response = await _httpClient.PostAsJsonAsync("api/pedidos", request);

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await ExtractErrorMessage(response);
                return (false, null, errorMessage);
            }

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<PedidoResponseDto>(content, _jsonOptions);
            return (true, result?.Id, null);
        }
        catch
        {
            return (false, null, "Erro ao criar pedido. Tente novamente.");
        }
    }

    public async Task<(bool Success, string? ErrorMessage)> AdicionarItemAsync(Guid pedidoId, int produtoId)
    {
        try
        {
            var request = new PedidoRequestDto
            {
                Itens = new List<ItemPedidoRequestDto>
                {
                    new() { Id = produtoId }
                }
            };

            var response = await _httpClient.PostAsJsonAsync($"api/pedidos/{pedidoId}/itens", request);

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await ExtractErrorMessage(response);
                return (false, errorMessage);
            }

            return (true, null);
        }
        catch
        {
            return (false, "Erro ao adicionar item. Tente novamente.");
        }
    }

    public async Task<(bool Success, string? ErrorMessage)> RemoverItemAsync(Guid pedidoId, int itemId)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"api/pedidos/{pedidoId}/itens/{itemId}");

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await ExtractErrorMessage(response);
                return (false, errorMessage);
            }

            return (true, null);
        }
        catch
        {
            return (false, "Erro ao remover item. Tente novamente.");
        }
    }

    public async Task<(bool Success, PedidoGetResponseDto? Pedido, string? ErrorMessage)> ObterPedidoAsync(Guid pedidoId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"api/pedidos/{pedidoId}");

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await ExtractErrorMessage(response);
                return (false, null, errorMessage);
            }

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<PedidoGetResponseDto>(content, _jsonOptions);
            return (true, result, null);
        }
        catch
        {
            return (false, null, "Erro ao obter pedido. Tente novamente.");
        }
    }

    public async Task<(bool Success, string? ErrorMessage)> FecharPedidoAsync(Guid pedidoId)
    {
        try
        {
            var response = await _httpClient.PostAsync($"api/pedidos/{pedidoId}/fechar", null);

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await ExtractErrorMessage(response);
                return (false, errorMessage);
            }

            return (true, null);
        }
        catch
        {
            return (false, "Erro ao fechar pedido. Tente novamente.");
        }
    }

    public async Task<(bool Success, string? ErrorMessage)> CancelarPedidoAsync(Guid pedidoId)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"api/pedidos/{pedidoId}");

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await ExtractErrorMessage(response);
                return (false, errorMessage);
            }

            return (true, null);
        }
        catch
        {
            return (false, "Erro ao cancelar pedido. Tente novamente.");
        }
    }

    private async Task<string> ExtractErrorMessage(HttpResponseMessage response)
    {
        try
        {
            var content = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrEmpty(content))
                return "Erro na solicitação. Tente novamente.";

            using var doc = JsonDocument.Parse(content);
            var root = doc.RootElement;

            // Tenta encontrar a propriedade "mensagem"
            if (root.TryGetProperty("mensagem", out var mensagem))
            {
                return mensagem.GetString() ?? "Erro na solicitação.";
            }

            // Tenta encontrar a propriedade "detail"
            if (root.TryGetProperty("detail", out var detail))
            {
                return detail.GetString() ?? "Erro na solicitação.";
            }

            // Tenta encontrar a propriedade "title"
            if (root.TryGetProperty("title", out var title))
            {
                return title.GetString() ?? "Erro na solicitação.";
            }

            return "Erro na solicitação. Tente novamente.";
        }
        catch
        {
            return "Erro na solicitação. Tente novamente.";
        }
    }
}
