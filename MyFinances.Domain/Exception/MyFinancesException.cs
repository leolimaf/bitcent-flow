namespace MyFinances.Domain.Exception;

public class MyFinancesException : System.Exception
{
    public string Param { get; private set; }
    public MyFinancesExceptionType ErrorType { get; private set; }
    public string? Message { get; private set; }
    
    public MyFinancesException(string message, MyFinancesExceptionType errorType = MyFinancesExceptionType.INTERNAL_SERVER_ERROR) : base(message)
    {
        Message = message;
        ErrorType = errorType;
    }
    
    public MyFinancesException(string param, MyFinancesExceptionType errorType, string? message) : base(message ?? param)
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