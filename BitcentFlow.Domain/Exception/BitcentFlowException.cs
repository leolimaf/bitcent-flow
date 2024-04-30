namespace BitcentFlow.Domain.Exception;

public class BitcentFlowException : System.Exception
{
    public string Param { get; private set; }
    public BitcentFlowExceptionType ErrorType { get; private set; }
    public string? Message { get; private set; }
    
    public BitcentFlowException(string message, BitcentFlowExceptionType errorType = BitcentFlowExceptionType.INTERNAL_SERVER_ERROR) : base(message)
    {
        Message = message;
        ErrorType = errorType;
    }
    
    public BitcentFlowException(string param, BitcentFlowExceptionType errorType, string? message) : base(message ?? param)
    {
        Param = param;
        ErrorType = errorType;
        Message = message;
    }

    public object ToErrorObject()
    {
        return new ErroDTO
        {
            Error = Message ?? Param,
            Status = (int) ErrorType
        };
    }
}

public record ErroDTO
{
    public string Error { get; init; }
    public int Status { get; init; }
}