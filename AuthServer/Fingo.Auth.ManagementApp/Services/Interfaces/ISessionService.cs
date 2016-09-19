using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Fingo.Auth.Domain.Models.UserModels;

namespace Fingo.Auth.ManagementApp.Services.Interfaces
{
    public interface ISessionService
    {
        void LogIn(UserModel user, string jwt, HttpContext httpContext);
        void LogOut(HttpContext httpContext);
        bool IsLoggedIn(ClaimsPrincipal user);
    }
}
