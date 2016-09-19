using System.Collections.Generic;
using System.Reflection;
using Autofac;
using Fingo.Auth.Domain.Infrastructure.Interfaces;
using Microsoft.Extensions.DependencyModel;
using System.Linq;

namespace Fingo.Auth.AuthServer.Configuration.Modules
{
    public class DomainFactoryModule : Autofac.Module
    {
        private const string LibrariesName = "Fingo.Auth.Domain.";

        protected override void Load(ContainerBuilder builder)
        {
            RegisterSubServices(builder);
        }

        private void RegisterSubServices(ContainerBuilder builder)
        {
            var loadableAssemblies = LoadAssemblies();

            builder.RegisterAssemblyTypes(loadableAssemblies.ToArray())
                .Where(t => t.GetInterfaces()
                    .Any(i => i.IsAssignableFrom(typeof(IActionFactory))))
                .AsImplementedInterfaces()
                .InstancePerDependency();
        }

        private List<Assembly> LoadAssemblies()
        {
            var loadableAssemblies = new List<Assembly>();


            var deps = DependencyContext.Default;

            foreach (var compilationLibrary in deps.CompileLibraries)
            {
                if (compilationLibrary.Name.Contains(LibrariesName))
                {
                    var assembly = Assembly.Load(new AssemblyName(compilationLibrary.Name));
                    loadableAssemblies.Add(assembly);
                }
            }
            return loadableAssemblies;
        }
    }
}