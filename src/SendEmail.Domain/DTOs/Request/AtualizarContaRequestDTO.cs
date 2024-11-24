namespace SendEmail.Domain.DTOs.Request;

public class AtualizarContaRequestDTO
{
    public string IdentificadorConta { get; set; }
    public bool StatusAtivo { get; set; }
}
