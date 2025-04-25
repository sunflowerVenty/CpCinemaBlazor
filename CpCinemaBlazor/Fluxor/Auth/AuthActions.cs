namespace CpCinemaBlazor.Fluxor.Auth
{
    public record LoginSuccessAction(string JwtToken, string UserRole, DateTime TokenExpiration);
    public record LogoutAction;
    public record UpdateTokenAction(string NewToken, DateTime NewExpiration);
}
