namespace SendEmail.Domain.Model;

public sealed class Account : BaseEntity
{
    public Account()
    {
        Identificador = Guid.NewGuid().ToString();
    }

    public Account(string documento,
                   string nome,
                   string email,
                   string senha,
                   string telefone)
    {
        Documento = documento;
        NomeCompleto = nome;
        Email = email;
        Senha = senha;
    }

    public string Identificador { get; private set; }
    public string Documento { get; private set; }
    public string NomeCompleto { get; private set; }
    public string Email { get; private set; }
    public string Senha { get; private set; }
    public bool UsuarioAtivo { get; private set; } = true;
    public bool Admin { get; private set; } = false;
    public DateTime DataCriacao { get; private set; } = DateTime.Now;
}
