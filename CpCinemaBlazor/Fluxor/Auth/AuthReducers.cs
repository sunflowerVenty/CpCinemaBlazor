using System;
using CpCinemaBlazor.Fluxor.Auth;
using Fluxor;

namespace CpCinemaBlazor.Store.Auth
{
    public static class AuthReducers
    {
        [ReducerMethod]
        public static AuthState ReduceLoginSuccess(AuthState state, LoginSuccessAction action) =>
            new()
            {
                JwtToken = action.JwtToken,
                IsAuthenticated = true,
                UserRole = action.UserRole,
                TokenExpiration = action.TokenExpiration
            };

        [ReducerMethod]
        public static AuthState ReduceLogout(AuthState state, LogoutAction _) =>
            new()
            {
                JwtToken = null,
                IsAuthenticated = false,
                UserRole = null,
                TokenExpiration = null
            };

        [ReducerMethod]
        public static AuthState ReduceUpdateToken(AuthState state, UpdateTokenAction action) =>
            new()
            {
                JwtToken = action.NewToken,
                IsAuthenticated = true,
                UserRole = state.UserRole, // Роль остается той же
                TokenExpiration = action.NewExpiration
            };
    }
}