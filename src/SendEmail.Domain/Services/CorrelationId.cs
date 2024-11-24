using SendEmail.Domain.Interfaces.Services;

namespace SendEmail.Domain.Services;

public class CorrelationId : ICorrelationId
{
    private string _correlationId;

    public void GerarCorrelationId()
    {
        _correlationId = Guid.NewGuid().ToString();
    }

    public string ObterCorrelationId() => _correlationId;
}
