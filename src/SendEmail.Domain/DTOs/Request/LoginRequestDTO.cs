namespace SendEmail.Domain.DTOs.Request;

public class LoginRequestDTO
{
    public string DocumentoOuEmail { get; set; }
    public string Senha { get; set; }
}
