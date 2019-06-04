using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MaterialDesignThemes.Wpf;

#if LONG_PATH
using DirectoryInfo = Pri.LongPath.DirectoryInfo;
#endif

namespace MaterialDesignExtensions.Model
{
    public class SpecialDirectory
    {
        private string m_label;

        public PackIconKind Icon { get; set; }

        public DirectoryInfo Info { get; set; }

        public string Label
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(m_label))
                {
                    return m_label;
                }
                else
                {
                    return Info?.Name;
                }
            }

            set
            {
                m_label = value;
            }
        }

        public SpecialDirectory() { }
    }
}
