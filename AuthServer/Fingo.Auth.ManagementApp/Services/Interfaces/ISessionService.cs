using System.Security.Claims;
using Fingo.Auth.Domain.Models.UserModels;
using Microsoft.AspNetCore.Http;

namespace Fingo.Auth.ManagementApp.Services.Interfaces
{
    public interface ISessionService
    {
        void LogIn(UserModel user , string jwt , HttpContext httpContext);
        void LogOut(HttpContext httpContext);
        bool IsLoggedIn(ClaimsPrincipal user);
    }
}