using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialDesignExtensionsBuildUtility
{
    public class BuildConfiguration
    {
        public string Name { get; }

        public string BuildDirectory { get; }

        public string NuspecFilename { get; }

        public BuildConfiguration(string name, string buildDirectory, string nuspecFilename)
        {
            Name = name;
            BuildDirectory = buildDirectory;
            NuspecFilename = nuspecFilename;
        }
    }
}
