namespace dotnet_store.Services.Results;

// Exception → olmaması gereken bir şey oldu, sistem durmalı
// ServiceResult → iş akışı devam ediyor ama sonuç olumsuz

// Login örneğinde:
// Yanlış şifre → ServiceResult çünkü kullanıcı tekrar deneyebilir
// userId null → Exception çünkü [Authorize] varken bu olmamalı

public class ServiceResult
{
    public bool Succeeded { get; set; }
    public string? ErrorMessage { get; set; }

    public static ServiceResult Success() => new ServiceResult { Succeeded = true };

    public static ServiceResult Fail(string message) =>
        new ServiceResult { Succeeded = false, ErrorMessage = message };
}

public class ServiceResult<T> : ServiceResult
{
    public T? Data { get; set; }

    public static ServiceResult<T> Success(T data) =>
        new ServiceResult<T> { Succeeded = true, Data = data };

    public static new ServiceResult<T> Fail(string message) =>
        new ServiceResult<T> { Succeeded = false, ErrorMessage = message };
}
