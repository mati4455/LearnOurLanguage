using System;
using System.Reflection;
using Autofac;
using Model.Const;

namespace LearnOurLanguage.Web.Base
{
    /// <summary>
    /// Helper do konfiguracji startupu
    /// </summary>
    public class ConfigHelper
    {
        /// <summary>
        /// Zarejestrowanie repozytoriów i serwisów w podanych projektach
        /// </summary>
        /// <param name="builder">Referencja do buildera</param>
        /// <param name="projectName">Tablica nazwa projektów</param>
        public static void BindAssemblyForBuilder(ref ContainerBuilder builder, params string[] projectName)
        {
            foreach (var name in projectName)
            {
                try
                {
                    var assembly = Assembly.Load(new AssemblyName(name));

                    builder.RegisterAssemblyTypes(assembly)
                        .Where(t => t.Name.EndsWith("Repository") ||
                                    t.Name.EndsWith("Service")
                        )
                        .AsImplementedInterfaces()
                        .InstancePerLifetimeScope();
                }
                catch (Exception ex)
                {
                    throw new Exception(ExceptionConst.AssemblyNotExist + ", " + ex);
                }
            }
        }
    }
}
