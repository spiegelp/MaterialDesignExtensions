using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MaterialDesignThemes.Wpf;

namespace MaterialDesignExtensions.Model
{
    public class SpecialDrive
    {
        public PackIconKind Icon { get; set; }

        public string Label { get; set; }

        public DriveInfo Info { get; set; }

        public SpecialDrive() { }
    }
}
