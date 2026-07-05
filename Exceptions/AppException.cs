namespace dotnet_store.Exceptions;

// Base exception
// Exception → olmaması gereken bir şey oldu, sistem durmalı
// ServiceResult → iş akışı devam ediyor ama sonuç olumsuz

// Login örneğinde:
// Yanlış şifre → ServiceResult çünkü kullanıcı tekrar deneyebilir
// userId null → Exception çünkü [Authorize] varken bu olmamalı

public class AppException : Exception
{
    public int StatusCode { get; }

    public AppException(string message, int statusCode)
        : base(message)
    {
        StatusCode = statusCode;
    }
}

// Alt sınıflar
public class NotFoundException : AppException
{
    public NotFoundException(string message)
        : base(message, 404) { }
}

public class UnauthorizedException : AppException
{
    public UnauthorizedException(string message)
        : base(message, 401) { }
}

public class BusinessRuleException : AppException
{
    public BusinessRuleException(string message)
        : base(message, 400) { }
}
