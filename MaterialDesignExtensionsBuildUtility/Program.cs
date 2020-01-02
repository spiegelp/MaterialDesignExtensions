using System;
using System.Diagnostics;
using System.IO;

namespace MaterialDesignExtensionsBuildUtility
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                Configuration configuration = Init(args);

                if (configuration.BuildConfigurations != null && configuration.BuildConfigurations.Length > 0)
                {
                    foreach (BuildConfiguration buildConfiguration in configuration.BuildConfigurations)
                    {
                        Build(configuration, buildConfiguration);
                    }

                    foreach (BuildConfiguration buildConfiguration in configuration.BuildConfigurations)
                    {
                        if (!string.IsNullOrWhiteSpace(buildConfiguration.NuspecFilename))
                        {
                            Package(configuration, buildConfiguration);
                        }
                    }
                }
            }
            catch (ArgumentException exc)
            {
                Console.WriteLine(exc.Message);
            }
            catch (BuildException exc)
            {
                Console.WriteLine(exc.Message);
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc);
            }

            Console.WriteLine("Press any key to exit...");
            Console.Read();
        }

        private static Configuration Init(string[] args)
        {
            Console.WriteLine("Initialization...");

            return ConfigurationBuilder.CreateConfiguration(args);
        }

        private static void Build(Configuration configuration, BuildConfiguration buildConfiguration)
        {
            Console.WriteLine($"Building {buildConfiguration.Name}...");

            if (!RunProcess($"\"{configuration.MsBuildExePath}\"", $"\"{configuration.ProjectFile}\" -property:Configuration={buildConfiguration.Name} -consoleloggerparameters:PerformanceSummary;Summary;ErrorsOnly"))
            {
                throw new BuildException($"Build of {buildConfiguration.Name} failed");
            }

            Console.WriteLine($"Build of {buildConfiguration.Name} succeeded");
        }

        private static void Package(Configuration configuration, BuildConfiguration buildConfiguration)
        {
            Console.WriteLine($"Packing {buildConfiguration.Name}...");

            if (!Directory.Exists(configuration.PackageDirectory))
            {
                Directory.CreateDirectory(configuration.PackageDirectory);
            }

            if (!RunProcess($"\"{configuration.NuGetExePath}\"", $"pack \"{configuration.ProjectDirectory}\\{buildConfiguration.NuspecFilename}\" -OutputDirectory \"{configuration.PackageDirectory}\""))
            {
                throw new BuildException($"Packing of {buildConfiguration.Name} failed");
            }

            Console.WriteLine($"Packing of {buildConfiguration.Name} succeeded");
        }

        private static bool RunProcess(string filename, string arguments)
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo(filename, arguments)
            {
                UseShellExecute = false,
                RedirectStandardOutput = true
            };

            Process process = Process.Start(processStartInfo);

            Console.WriteLine(process.StandardOutput.ReadToEnd());

            process.WaitForExit();

            return process.ExitCode == 0;
        }
    }
}
