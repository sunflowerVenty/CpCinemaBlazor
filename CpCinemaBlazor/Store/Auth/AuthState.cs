namespace CpCinemaBlazor.Store.Auth
{
    public record AuthState
    {
        public string? JwtToken { get; init; }
        public bool IsAuthenticated { get; init; }
        public string? UserRole { get; init; }
        public DateTime? TokenExpiration { get; init; }
    }
}
