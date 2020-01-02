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
