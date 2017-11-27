using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialDesignExtensionsDemo.ViewModel
{
    public class OpenDirectoryControlViewModel : ViewModel
    {
        private string m_selectedAction;

        public string SelectedAction
        {
            get
            {
                return m_selectedAction;
            }

            set
            {
                m_selectedAction = value;

                OnPropertyChanged(nameof(SelectedAction));
            }
        }

        public OpenDirectoryControlViewModel()
            : base()
        {
            m_selectedAction = null;
        }
    }
}
