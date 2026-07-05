namespace dotnet_store.Services.Results;

public class LoginResult
{
    public bool Succeeded { get; set; }
    public bool IsLockedOut { get; set; }
    public int LockoutMinutesLeft { get; set; }
    public string? ErrorMessage { get; set; }
}
