using System.Net.Http.Json;
using System.Text.Json;
using Tarea8_AutenticacionToken.Models;

namespace Tarea8_AutenticacionToken.Services;

public class AuthService
{
    private readonly HttpClient _httpClient;

    public AuthService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<LoginRespuesta?> IniciarSesionAsync(
        string usuario, string contrasena)
    {
        var datos = new
        {
            username = usuario,
            password = contrasena
        };

        var respuesta = await _httpClient.PostAsJsonAsync(
            "https://dummyjson.com/auth/login", datos);

        respuesta.EnsureSuccessStatusCode();

        return await respuesta.Content.ReadFromJsonAsync<LoginRespuesta>();
    }

    public async Task<Usuario?> ObtenerPerfilAsync(string token)
    {
        using var solicitud = new HttpRequestMessage(
            HttpMethod.Get, "https://dummyjson.com/auth/me");

        solicitud.Headers.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue(
                "Bearer", token);

        var respuesta = await _httpClient.SendAsync(solicitud);

        respuesta.EnsureSuccessStatusCode();

        return await respuesta.Content.ReadFromJsonAsync<Usuario>();
    }
}

