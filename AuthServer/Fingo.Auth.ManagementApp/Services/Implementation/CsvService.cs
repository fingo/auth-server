using System;
using System.Collections.Generic;
using System.IO;
using Fingo.Auth.DbAccess.Repository.Interfaces;
using Fingo.Auth.Domain.Infrastructure.EventBus.Events.Project;
using Fingo.Auth.Domain.Infrastructure.EventBus.Interfaces;
using Fingo.Auth.Domain.Models.UserModels;
using Fingo.Auth.ManagementApp.Services.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Fingo.Auth.ManagementApp.Services.Implementation
{
    public class CsvService : ICsvService
    {
        private readonly IEventBus _eventBus;
        private readonly IProjectRepository _projectRepository;

        public CsvService(IProjectRepository projectRepository , IEventBus eventBus)
        {
            _projectRepository = projectRepository;
            _eventBus = eventBus;
        }

        public string ExportProject(int projectId)
        {
            var project = _projectRepository.GetById(projectId);
            if (project == null)
                throw new ArgumentNullException($"Could not find project with id: {projectId}.");

            var result =
                "Project name;Project GUID;Client name;Client contact data;Creation date UTC;Modification date UTC\r\n";
            result +=
                $"{ToCsvString(project.Name)};{ToCsvString(project.ProjectGuid.ToString())};{ToCsvString(project.Information.Name)};" +
                $"{ToCsvString(project.Information.ContactData)};{ToCsvString(project.CreationDate.ToString("dd.MM.yyyy HH:mm"))};" +
                $"{ToCsvString(project.ModificationDate.ToString("dd.MM.yyyy HH:mm"))}\n";

            result += ";;;;;\r\n";
            result +=
                "User e-mail;First name;Last name;Last password change UTC;Creation date UTC;Modification date UTC\r\n";

            var users = _projectRepository.GetAllUsersFromProject(projectId);
            if (users == null)
                throw new ArgumentNullException($"Could not find users of project with id: {projectId}.");

            foreach (var user in users)
                result += $"{ToCsvString(user.Login)};{ToCsvString(user.FirstName)};{ToCsvString(user.LastName)};" +
                          $"{ToCsvString(user.LastPasswordChange.ToString("dd.MM.yyyy HH:mm"))};" +
                          $"{ToCsvString(user.CreationDate.ToString("dd.MM.yyyy HH:mm"))};" +
                          $"{ToCsvString(user.ModificationDate.ToString("dd.MM.yyyy HH:mm"))}\r\n";

            _eventBus.Publish(new ProjectExported(projectId));
            return result;
        }

        public List<UserModel> CsvToUsersList(IFormFile csvFile)
        {
            const char delimiter = '\t';

            var users = new List<UserModel>();

            using (var stream = csvFile.OpenReadStream())
            {
                using (var reader = new StreamReader(stream))
                {
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var values = line.Split(delimiter);

                        if (values.Length != 3)
                            throw new FormatException();

                        users.Add(new UserModel
                        {
                            FirstName = values[0] ,
                            LastName = values[1] ,
                            Login = values[2]
                        });
                    }
                }
            }
            return users;
        }

        private string ToCsvString(string entry)
        {
            return entry == null ? "N/A" : $"\"{entry.Replace("\"" , "\"\"")}\"";
        }
    }
}