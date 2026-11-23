namespace Tarea8_AutenticacionToken.Models;

public class Usuario
{
    public int Id { get; set; }

    public string Username { get; set; } = string.Empty;

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Image { get; set; } = string.Empty;
}
