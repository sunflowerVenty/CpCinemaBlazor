using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Fluxor;
using CpCinemaBlazor.StateManager;

namespace CpCinemaBlazor
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly IState<AuthState> _authState;

        public CustomAuthenticationStateProvider(IState<AuthState> authState)
        {
            _authState = authState;
        }

        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = _authState.Value.JwtToken;

            if (string.IsNullOrEmpty(token))
            {
                return Task.FromResult(new AuthenticationState(new ClaimsPrincipal()));
            }

            var claims = ParseClaimsFromJwt(token);
            var identity = new ClaimsIdentity(claims, "jwt");
            var user = new ClaimsPrincipal(identity);

            return Task.FromResult(new AuthenticationState(user));
        }

        public async Task NotifyUserAuthentication(string token)
        {
            await Task.Run(() =>
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);

                var identity = new ClaimsIdentity(jwtToken.Claims, "jwt");
                var user = new ClaimsPrincipal(identity);

                NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
            });
        }

        private IEnumerable<Claim> ParseClaimsFromJwt(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            return jwtToken.Claims;
        }
    }
}
