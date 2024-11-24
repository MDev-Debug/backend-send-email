using SendEmail.Domain.Enum;

namespace SendEmail.Domain.DTOs;

public class Response
{
    public Response(string correlationId, object responseData, EResponse status)
    {
        CorrelationId = correlationId;
        ResponseData = responseData;
        Status = status;
    }

    public string CorrelationId { get; }
    public object ResponseData { get; }
    public EResponse Status { get; }
}
