using System.IO;
using MaterialDesignThemes.Wpf;

namespace MaterialDesignExtensions.Model
{
    /// <summary>
    /// A helper class to provide drives inside the file system controls.
    /// </summary>
    public class SpecialDrive
    {
        /// <summary>
        /// The icon on the user inferface for this special location.
        /// </summary>
        public PackIconKind Icon { get; set; }

        /// <summary>
        /// /// <summary>
        /// The label the user inferface for this special location.
        /// </summary>
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// The <see cref="DriveInfo" /> object with the information for this special location.
        /// </summary>
        public DriveInfo Info { get; set; }

        /// <summary>
        /// Creates a new <see cref="SpecialDrive" />.
        /// </summary>
        public SpecialDrive() { }
    }
}
