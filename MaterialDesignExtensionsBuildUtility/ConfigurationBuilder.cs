using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialDesignExtensionsBuildUtility
{
    public class ConfigurationBuilder
    {
        private const string ProjectDirectoryArgumentName = "projectDirectory";
        private const string BuildConfigurationArgumentName = "buildConfiguration";
        private const char BuildConfigurationsSeparator = ';';

        private const string ProjectDirectoryName = "MaterialDesignExtensions";
        private const string ProjectFileName = "MaterialDesignExtensions.csproj";
        
        private static readonly string[] MsBuildExePaths = new string[]
        {
            @"C:\Program Files (x86)\Microsoft Visual Studio\2017\Community\MSBuild\15.0\Bin\MSBuild.exe",
            @"C:\Program Files (x86)\Microsoft Visual Studio\2017\Professional\MSBuild\15.0\Bin\MSBuild.exe",
            @"C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin\MSBuild.exe",
            @"C:\Program Files (x86)\Microsoft Visual Studio\2019\Professional\MSBuild\Current\Bin\MSBuild.exe"
        };

        private static readonly ISet<BuildConfiguration> AllBuildConfigurations = new HashSet<BuildConfiguration>()
        {
            new BuildConfiguration("Release", @"bin\Release", "MaterialDesignExtensions.nuspec"),
            new BuildConfiguration("Debug", @"bin\Debug", null)
        };

        public static Configuration CreateConfiguration(string[] args)
        {
            Configuration configuration = new Configuration();

            ParseArguments(configuration, args);

            InitRepositoryProjectConfiguration(configuration);

            if (string.IsNullOrWhiteSpace(configuration.ProjectFile) || !File.Exists(configuration.ProjectFile))
            {
                throw new ArgumentException("Did not find project file");
            }

            InitCommandsConfiguration(configuration);

            if (string.IsNullOrWhiteSpace(configuration.MsBuildExePath) || !File.Exists(configuration.MsBuildExePath))
            {
                throw new ArgumentException("Did not find MSBuild.exe");
            }

            if (string.IsNullOrWhiteSpace(configuration.NuGetExePath) || !File.Exists(configuration.NuGetExePath))
            {
                throw new ArgumentException("Did not find nuget.exe");
            }

            if (configuration.BuildConfigurations == null || configuration.BuildConfigurations.Length == 0)
            {
                configuration.BuildConfigurations = AllBuildConfigurations.ToArray();
            }

            configuration.BuildConfigurations = configuration.BuildConfigurations.OrderBy(buildConfiguration => buildConfiguration.Name).ToArray();

            return configuration;
        }

        private static void ParseArguments(Configuration configuration, string[] args)
        {
            if (args != null && args.Length > 0)
            {
                for (int i = 0; i < args.Length; i++)
                {
                    if (args[i].StartsWith($"-{ProjectDirectoryArgumentName}"))
                    {
                        i++;

                        if (i < args.Length)
                        {
                            configuration.ProjectDirectory = StripQuotes(args[i]).Trim();
                        }
                    }
                    else if (args[i].StartsWith($"-{BuildConfigurationArgumentName}"))
                    {
                        i++;

                        if (i < args.Length)
                        {
                            string buildConfigurationsStr = StripQuotes(args[i]);
                            ISet<string> buildConfigurationNames = new HashSet<string>();

                            if (buildConfigurationsStr.Contains(BuildConfigurationsSeparator))
                            {
                                buildConfigurationNames.UnionWith(buildConfigurationsStr.Split(BuildConfigurationsSeparator).Select(str => str.Trim()));
                            }
                            else
                            {
                                buildConfigurationNames.Add(buildConfigurationsStr.Trim());
                            }

                            configuration.BuildConfigurations = AllBuildConfigurations.Where(buildConfiguration => buildConfigurationNames.Contains(buildConfiguration.Name)).ToArray();
                        }
                    }
                }
            }
        }

        private static string StripQuotes(string str)
        {
            if (!string.IsNullOrWhiteSpace(str))
            {
                if (str.StartsWith("\"") && str.Length > 1)
                {
                    str = str.Substring(1);
                }

                if (str.EndsWith("\""))
                {
                    str = str.Substring(0, str.Length - 1);
                }
            }

            return str;
        }

        private static void InitRepositoryProjectConfiguration(Configuration configuration)
        {
            bool repositoryDirectoryFound = false;
            DirectoryInfo currentDirectory = null;

            if (string.IsNullOrWhiteSpace(configuration.ProjectDirectory))
            {
                currentDirectory = new DirectoryInfo(Directory.GetCurrentDirectory());

                while (!(repositoryDirectoryFound = IsRepositoryDirectory(currentDirectory)) && currentDirectory.Parent != null)
                {
                    currentDirectory = currentDirectory.Parent;
                }
            }
            else
            {
                currentDirectory = new DirectoryInfo(configuration.ProjectDirectory).Parent;
                repositoryDirectoryFound = true;
            }

            if (repositoryDirectoryFound)
            {
                configuration.RepositoryBaseDirectory = currentDirectory.FullName;
                configuration.ProjectDirectory = $@"{configuration.RepositoryBaseDirectory}\{ProjectDirectoryName}";
                configuration.ProjectFile = $@"{configuration.ProjectDirectory}\{ProjectFileName}";
                configuration.PackageDirectory = $@"{configuration.RepositoryBaseDirectory}\packageBuilds";
            }
        }

        private static bool IsRepositoryDirectory(DirectoryInfo directory)
        {
            return directory.GetDirectories()
                .Any(directoryItem => directoryItem.Name.Equals(ProjectDirectoryName, StringComparison.InvariantCultureIgnoreCase));
        }

        private static void InitCommandsConfiguration(Configuration configuration)
        {
            configuration.MsBuildExePath = MsBuildExePaths.FirstOrDefault(msBuildExePath => File.Exists(msBuildExePath));

            configuration.NuGetExePath = Environment.GetEnvironmentVariable("PATH").Split(';')
                .Select(path => Path.Combine(path, "nuget.exe"))
                .Where(file => File.Exists(file))
                .FirstOrDefault();
        }
    }
}
