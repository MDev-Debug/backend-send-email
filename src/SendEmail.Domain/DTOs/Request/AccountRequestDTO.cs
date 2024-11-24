namespace SendEmail.Domain.DTOs.Request;

public class AccountRequestDTO
{
    public AccountRequestDTO() { }
    public AccountRequestDTO(string documento,
                             string nomeCompleto,
                             string email,
                             string password,
                             string confirPassword,
                             string telefone)
    {
        Documento = documento;
        NomeCompleto = nomeCompleto;
        Email = email;
        Senha = password;
        ConfirmacaoSenha = confirPassword;
        Telefone = telefone;
    }

    public string Documento { get; set; }
    public string NomeCompleto { get; set; }
    public string Email { get; set; }
    public string Senha { get; set; }
    public string ConfirmacaoSenha { get; set; }
    public string Telefone { get; set; }
}
