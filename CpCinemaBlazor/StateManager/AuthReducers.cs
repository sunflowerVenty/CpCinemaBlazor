using Fluxor;
using System;

namespace CpCinemaBlazor.StateManager
{
    public static class AuthReducers
    {
        [ReducerMethod]
        public static AuthState ReduceSetJwtTokenAction(AuthState state, SetJwtTokenAction action) =>
            new AuthState(action.JwtToken, !string.IsNullOrEmpty(action.JwtToken));

        [ReducerMethod(typeof(ClearJwtTokenAction))]
        public static AuthState ReduceClearJwtTokenAction(AuthState state) =>
            new AuthState(null, false);
    }

    public record SetJwtTokenAction(string JwtToken);

    public record ClearJwtTokenAction();
}