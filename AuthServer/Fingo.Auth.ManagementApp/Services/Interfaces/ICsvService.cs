using System.Collections.Generic;
using Fingo.Auth.Domain.Models.UserModels;
using Microsoft.AspNetCore.Http;

namespace Fingo.Auth.ManagementApp.Services.Interfaces
{
    public interface ICsvService
    {
        string ExportProject(int projectId);
        List<UserModel> CsvToUsersList(IFormFile csvFile);
    }
}