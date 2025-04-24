using Blazored.LocalStorage;
using Fluxor;
using System.Threading.Tasks;
using static CpCinemaBlazor.Store.Auth.AuthActions;

namespace CpCinemaBlazor.Store.Auth
{
    public class AuthEffects
    {
        private readonly ILocalStorageService _localStorage;

        public AuthEffects(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        [EffectMethod]
        public async Task HandleLoginSuccess(LoginSuccessAction action, IDispatcher dispatcher)
        {
            await _localStorage.SetItemAsync("authToken", action.JwtToken);
        }

        [EffectMethod]
        public async Task HandleLogout(LogoutAction action, IDispatcher dispatcher)
        {
            await _localStorage.RemoveItemAsync("authToken");
        }

        [EffectMethod]
        public async Task HandleUpdateToken(UpdateTokenAction action, IDispatcher dispatcher)
        {
            await _localStorage.SetItemAsync("authToken", action.NewToken);
        }
    }
}