using System;
using System.Collections.Generic;
using System.Security.Claims;
using Fingo.Auth.AuthServer.Client.Services.Interfaces;
using Fingo.Auth.Domain.Models.UserModels;
using Fingo.Auth.ManagementApp.Configuration;
using Fingo.Auth.ManagementApp.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Authentication;

namespace Fingo.Auth.ManagementApp.Services.Implementation
{
    public class SessionService : ISessionService
    {
        private readonly IRemoteTokenService _remoteTokenService;
        private readonly ITokenService _tokenService;

        public SessionService(IRemoteTokenService remoteTokenService , ITokenService tokenService)
        {
            _remoteTokenService = remoteTokenService;
            _tokenService = tokenService;
        }

        public void LogIn(UserModel user , string jwt , HttpContext httpContext)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name , user.Login , ClaimValueTypes.String) ,
                new Claim(ClaimTypes.UserData , user.Id.ToString()) ,
                new Claim(AuthorizationConfiguration.PolicyName , jwt , ClaimValueTypes.String)
            };

            var registrationState = _tokenService.RegistrationState(jwt);
            if (registrationState != null)
                claims.Add(new Claim(ClaimTypes.Role , registrationState));

            var userIdentity = new ClaimsIdentity();
            userIdentity.AddClaims(claims);
            var userPrincipal = new ClaimsPrincipal(userIdentity);

            httpContext.Authentication.SignInAsync("Cookie" , userPrincipal ,
                new AuthenticationProperties
                {
                    ExpiresUtc = DateTime.UtcNow.AddYears(20) ,
                    IsPersistent = true ,
                    AllowRefresh = true
                });
        }

        public void LogOut(HttpContext httpContext)
        {
            httpContext.Authentication.SignOutAsync("Cookie");
        }

        public bool IsLoggedIn(ClaimsPrincipal user)
        {
            return
                user.HasClaim(
                    c => (c.Type == AuthorizationConfiguration.PolicyName) && _remoteTokenService.VerifyToken(c.Value));
        }
    }
}