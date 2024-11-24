namespace SendEmail.Domain.Interfaces.Services;

public interface ICorrelationId
{
    void GerarCorrelationId();
    string ObterCorrelationId();
}
