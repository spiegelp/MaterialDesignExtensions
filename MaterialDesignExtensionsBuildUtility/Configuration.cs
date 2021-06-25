using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialDesignExtensionsBuildUtility
{
    public class Configuration
    {
        public string MsBuildExePath { get; set; }

        public string NuGetExePath { get; set; }

        public string RepositoryBaseDirectory { get; set; }

        public string ProjectDirectory { get; set; }

        public string ProjectFile { get; set; }

        public BuildConfiguration[] BuildConfigurations { get; set; }

        public string PackageDirectory { get; set; }

        public Configuration() { }
    }
}
