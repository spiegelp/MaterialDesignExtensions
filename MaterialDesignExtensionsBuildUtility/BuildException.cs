using System;

namespace MaterialDesignExtensionsBuildUtility
{
    public class BuildException : Exception
    {
        public BuildException(string message) : base(message) { }
    }
}
