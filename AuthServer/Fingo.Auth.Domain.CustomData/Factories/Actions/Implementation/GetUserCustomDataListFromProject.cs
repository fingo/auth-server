using System;
using System.Collections.Generic;
using System.Linq;
using Fingo.Auth.DbAccess.Models.CustomData.Enums;
using Fingo.Auth.DbAccess.Repository.Interfaces;
using Fingo.Auth.Domain.CustomData.ConfigurationClasses.User;
using Fingo.Auth.Domain.CustomData.Factories.Actions.Interfaces;
using Fingo.Auth.Domain.CustomData.Services.Interfaces;

namespace Fingo.Auth.Domain.CustomData.Factories.Actions.Implementation
{
    public class GetUserCustomDataListFromProject : IGetUserCustomDataListFromProject
    {
        private readonly ICustomDataJsonConvertService _convertService;
        private readonly IProjectRepository _projectRepository;
        private readonly IUserRepository _userRepository;

        public GetUserCustomDataListFromProject(IProjectRepository projectRepository ,
            IUserRepository userRepository , ICustomDataJsonConvertService convertService)
        {
            _convertService = convertService;
            _userRepository = userRepository;
            _projectRepository = projectRepository;
        }

        public List<Tuple<string , string>> Invoke(Guid projectGuid , string userLogin)
        {
            var user = _userRepository.GetAll().FirstOrDefault(u => u.Login == userLogin);
            var project = _projectRepository.GetAll().FirstOrDefault(p => p.ProjectGuid == projectGuid);

            if ((user == null) || (project == null))
                throw new ArgumentNullException();

            var projectId = project.Id;

            project = _projectRepository.GetByIdWithCustomDatas(projectId);

            if (project == null)
                throw new ArgumentNullException();

            var userCustomData = user.UserCustomData.Where(ucd => ucd.ProjectCustomData.ProjectId == projectId);

            var list = new List<Tuple<string , string>>();

            foreach (var customData in userCustomData)
            {
                var key = customData.ProjectCustomData.ConfigurationName;
                string value;

                switch (customData.ProjectCustomData.ConfigurationType)
                {
                    case ConfigurationType.Number:
                        value = ((NumberUserConfiguration) _convertService.DeserializeUser(ConfigurationType.Number ,
                            customData.SerializedConfiguration)).Value.ToString();
                        break;
                    case ConfigurationType.Boolean:
                        value = ((BooleanUserConfiguration) _convertService.DeserializeUser(ConfigurationType.Boolean ,
                            customData.SerializedConfiguration)).Value.ToString();
                        break;
                    case ConfigurationType.Text:
                        value = ((TextUserConfiguration) _convertService.DeserializeUser(ConfigurationType.Text ,
                            customData.SerializedConfiguration)).Value;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                list.Add(new Tuple<string , string>(key , value));
            }

            return list;
        }
    }
}