using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Tenaris.Fava.Production.Reporting.Model.Interfaces;
using Tenaris.Fava.Production.Reporting.Model.Model;

namespace Tenaris.Fava.Production.Reporting.ViewModel.Reflection
{
    public static class ReflectionStrategy
    {

        public static IActions MyStrategy { get; set; }

        public static void LoaderReflection()
        {
            MyStrategy = GetInstance(GetAssembly());
        }

        private static Assembly GetAssembly()
        {
            try
            {
                DirectoryInfo directory = new DirectoryInfo(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
                FileInfo foundAssembly = directory
                                            .GetFiles("*.dll", SearchOption.AllDirectories)
                                            .ToList<FileInfo>()
                                            .FirstOrDefault(f => AssemblyName.GetAssemblyName(f.FullName).Name == Configurations.Instance.PathStrategy);

                if (foundAssembly == null)
                    return null;

                return Assembly.LoadFrom(foundAssembly.FullName);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static IActions GetInstance(Assembly assembly)
        {
            try
            {
                Type foundType = assembly
                    .GetTypes()
                    .ToList<Type>()
                    .FirstOrDefault(x => Match(x, Configurations.Instance.StrategyWork));
                if (foundType == null
                    || !(typeof(IActions).IsAssignableFrom(foundType)
                    && foundType.IsAbstract == false))
                    return null;
                return foundType.InvokeMember(null, BindingFlags.CreateInstance, null, null, null) as IActions;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static bool Match(Type type, string stringType)
        {
            return type.AssemblyQualifiedName.ToUpper().StartsWith(stringType.Trim().ToUpper());
        }


    }
}
