using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MaterialDesignExtensions.Controls;

namespace MaterialDesignExtensionsDemo.ViewModel
{
    public class StepperBaseViewModel : ViewModel
    {
        protected StepperLayout m_layout;
        protected bool m_isLinear;
        protected bool m_contentAnimationsEnabled;

        public bool ContentAnimationsEnabled
        {
            get
            {
                return m_contentAnimationsEnabled;
            }

            set
            {
                m_contentAnimationsEnabled = value;

                OnPropertyChanged(nameof(ContentAnimationsEnabled));
            }
        }

        public override string DocumentationUrl
        {
            get
            {
                return "https://spiegelp.github.io/MaterialDesignExtensions/#documentation/stepper";
            }
        }

        public bool IsLinear
        {
            get
            {
                return m_isLinear;
            }

            set
            {
                m_isLinear = value;

                OnPropertyChanged(nameof(IsLinear));
            }
        }

        public StepperLayout Layout
        {
            get
            {
                return m_layout;
            }

            set
            {
                m_layout = value;

                OnPropertyChanged(nameof(Layout));
            }
        }

        public IEnumerable<StepperLayout> Layouts
        {
            get
            {
                return new List<StepperLayout>()
                {
                    StepperLayout.Horizontal,
                    StepperLayout.Vertical
                };
            }
        }

        public StepperBaseViewModel()
            : base()
        {
            m_layout = StepperLayout.Horizontal;
            m_isLinear = false;
            m_contentAnimationsEnabled = true;
        }
    }
}
